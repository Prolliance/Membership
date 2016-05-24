using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Passport.Database
{
    /// <summary>
    /// 基于数据库的身份认证
    /// </summary>
    public class PassportProvider : IPassportProvider
    {
        internal static DataRepo<UserSecurityInfo> UserSecurityInfoRepo = new DataRepo<UserSecurityInfo>();
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
            password = StringFactory.HashBySolt(password);
            return UserSecurityInfoRepo
                 .Read()
                 .Exists(us => us.Account.ToLower() == account.ToLower()
                     && us.Password == password);
        }

        public void SetPassword(string account, string password)
        {
            if (string.IsNullOrWhiteSpace(account)
                 || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("设置密码时发生参数错误");
            }
            UserSecurityInfo securityInfo = UserSecurityInfoRepo
                .Read()
                .FirstOrDefault(us => us.Account.ToLower() == account.ToLower());
            if (securityInfo == null)
            {
                securityInfo = new UserSecurityInfo();
                securityInfo.Id = StringFactory.NewGuid();
            }
            securityInfo.Account = account;
            securityInfo.Password = StringFactory.HashBySolt(password);
            UserSecurityInfoRepo.Save(securityInfo);
        }

        public List<UserInfo> LoadUser()
        {
            return null;
        }
    }
}
