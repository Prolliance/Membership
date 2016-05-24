using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;

namespace Prolliance.Membership.Passport.DC
{

    public class MultiDCPassportProvider : IPassportProvider
    {
        internal static DataRepo<UserInfo> UserInfoRepo = new DataRepo<UserInfo>();

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Validate(string account, string password)
        {
            if (string.IsNullOrWhiteSpace(account)
                || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            foreach (var dc in DC)
            {
                //this.DC = (this.DC ?? "").Trim();
                DirectoryEntry connectEntry = ConnectDC(dc, account, password);
                if (connectEntry == null)
                {
                    continue;
                };
                //检查用户表中是否存在，不存在则同步当前用户
                UserInfo userInfo = GetUserInfo(connectEntry, account);
                if (userInfo == null)
                {
                    continue;
                    
                }
                UserInfoRepo.Save(userInfo);
                return true;
            }
         
            return false;
        }

        /// <summary>
        /// 用来重置密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        public void SetPassword(string account, string password)
        {
            if (string.IsNullOrWhiteSpace(account)
                 || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("设置密码时发生错误");
            }
            //需要使用域管理员账号和密码（也可以将应用程序池配置为域管理员账号）
            //this.DC = (this.DC ?? "").Trim();
            foreach (var dc in DC)
            {
                this.Administrator = (this.Administrator ?? "").Trim();
                this.Password = (this.Password ?? "").Trim();
                DirectoryEntry connectEntry = ConnectDC(dc, this.Administrator, this.Password);
                if (connectEntry == null)
                {
                    throw new Exception("连接 DC 时发生错误");
                }
                DirectoryEntry userEntry = GetUserEntry(connectEntry, account);
                if (userEntry == null)
                {
                    throw new Exception("设置密码时发生错误");
                }
                //调SetPassword方法重置密码
                userEntry.Invoke("SetPassword", new object[] { password });
                userEntry.CommitChanges();//提交修改
                userEntry.Dispose();
            }
            
        }

        public List<UserInfo> LoadUser()
        {
            List<UserInfo> userInfoList = new List<UserInfo>();
            foreach (var dc in DC)
            {
                //this.DC = (this.DC ?? "").Trim();
                this.Administrator = (this.Administrator ?? "").Trim();
                this.Password = (this.Password ?? "").Trim();
                DirectoryEntry connectEntry = ConnectDC(dc, this.Administrator, this.Password);
                if (connectEntry == null)
                {
                    throw new Exception("连接 DC 时发生错误");
                }
                List<DirectoryEntry> entryList = FindAll(connectEntry, "(&(objectClass=user)(!(objectClass=computer)))");
              
                foreach (DirectoryEntry entry in entryList)
                {
                    userInfoList.Add(ConverToUser(entry));
                }
                
            }
            //需要使用域管理员账号和密码（也可以将应用程序池配置为域管理员账号）
           
            return userInfoList;
        }

        #region 暂未用到
        /*
        public void ChangePassword(string account, string oldPassword, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(account)
                 || string.IsNullOrWhiteSpace(oldPassword)
                 || string.IsNullOrWhiteSpace(newPassword))
            {
                throw new Exception("设置密码时发生错误");
            }
            if (!Validate(account, oldPassword))
            {
                throw new Exception("设置密码时，身份验证失败");
            }
            DirectoryEntry connectEntry = ConnectDC(this.DC, account, oldPassword);
            DirectoryEntry userEntry = GetUserEntry(connectEntry, account);
            userEntry.Username = account;
            userEntry.Password = oldPassword;
            //调ChangePassword方法修改密码
            userEntry.Invoke("ChangePassword", new object[] { oldPassword, newPassword });
            userEntry.CommitChanges();//提交修改
            userEntry.Dispose();
        }
        */
        #endregion

        #region AD操作
        public List<string> DC { get; set; }
        public string Administrator { get; set; }
        public string Password { get; set; }
        private DirectoryEntry ConnectDC(string dc, string account, string password)
        {
            try
            {
                DirectoryEntry entry = new DirectoryEntry(string.Format("LDAP://{0}", dc), account, password);
                return entry;
            }
            catch
            {
                throw new Exception("连接 DC 时发生错误");
            }
        }
        private DirectoryEntry FindOne(DirectoryEntry rootEntry, string filter)
        {
            DirectorySearcher search = new DirectorySearcher(rootEntry);
            search.Filter = filter;
            search.PropertiesToLoad.Add("cn");
            search.SearchScope = SearchScope.Subtree;
            SearchResult result = search.FindOne();
            if (result == null) return null;
            DirectoryEntry userEntry = result.GetDirectoryEntry();
            if (userEntry == null) return null;
            rootEntry.Dispose();
            return userEntry;
        }
        private List<DirectoryEntry> FindAll(DirectoryEntry rootEntry, string filter)
        {
            DirectorySearcher search = new DirectorySearcher(rootEntry);
            search.Filter = filter;
            search.PropertiesToLoad.Add("cn");
            search.SearchScope = SearchScope.Subtree;
            search.PageSize = int.MaxValue;
            SearchResultCollection resultCollection = search.FindAll();
            if (resultCollection == null) return null;
            List<DirectoryEntry> entryList = new List<DirectoryEntry>();
            foreach (SearchResult result in resultCollection)
            {
                DirectoryEntry entry = result.GetDirectoryEntry();
                if (entry == null) continue;
                entryList.Add(entry);
            }
            rootEntry.Dispose();
            return entryList;
        }
        private DirectoryEntry GetUserEntry(DirectoryEntry rootEntry, string account)
        {
            if (rootEntry == null || string.IsNullOrWhiteSpace(account))
            {
                return null;
            }
            return FindOne(rootEntry, string.Format("(SAMAccountName={0})", account));
        }
        private UserInfo GetUserInfo(DirectoryEntry rootEntry, string account)
        {
            DirectoryEntry userEntry = GetUserEntry(rootEntry, account);
            return ConverToUser(userEntry);
        }
        private UserInfo ConverToUser(DirectoryEntry userEntry)
        {
            if (userEntry == null) return null;
            var account = GetEntryPropertyValue(userEntry, "SAMAccountName");
            UserInfo userInfo = UserInfoRepo
                .Read()
                .FirstOrDefault(ui => ui.Account != null
                    && ui.Account.ToLower() == account.ToLower());
            if (userInfo == null)
            {
                userInfo = new UserInfo();
                userInfo.Id = StringFactory.NewGuid();
                userInfo.IsActive = true;
            }
            userInfo.Account = account;
            userInfo.Name = GetEntryPropertyValue(userEntry, "DisplayName")
                            ?? GetEntryPropertyValue(userEntry, "Name")
                            ?? userInfo.Account;
            userInfo.Email = GetEntryPropertyValue(userEntry, "Mail") ?? "";
            userEntry.Dispose();
            return userInfo;
        }
        private string GetEntryPropertyValue(DirectoryEntry entry, string propertyName)
        {
            if (entry.Properties.Contains(propertyName)
                && entry.Properties[propertyName].Count > 0)
            {
                return Convert.ToString(entry.Properties[propertyName][0]);
            }
            return null;
        }
        #endregion
    }
}
