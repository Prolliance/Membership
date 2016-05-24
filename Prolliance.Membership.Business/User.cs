using Prolliance.Membership.Business.Utils;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 用户业务模型（业务模型不包括 Passowrd 成员）
    /// </summary>
    public class User : ModelBase
    {
        internal static DataRepo<UserInfo> UserInfoRepo = new DataRepo<UserInfo>();
        internal static DataRepo<PositionInfo> PositionInfoRepo = new DataRepo<PositionInfo>();
        internal static DataRepo<RoleOperationInfo> RoleOperationInfoRepo = new DataRepo<RoleOperationInfo>();
        internal static DataRepo<RoleUserInfo> RoleUserInfoRepo = new DataRepo<RoleUserInfo>();
        internal static DataRepo<RoleInfo> RoleInfoRepo = new DataRepo<RoleInfo>();
        internal static DataRepo<OrganizationInfo> OrganizationInfoRepo = new DataRepo<OrganizationInfo>();
        internal static DataRepo<PositionUserInfo> PositionUserInfoRepo = new DataRepo<PositionUserInfo>();
        internal static DataRepo<UserSecurityInfo> UserSecurityInfoRepo = new DataRepo<UserSecurityInfo>();
        internal static IPassportProvider passport = Passport.Cteate();

        #region 属性
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public string JianPin { get; set; }

        /// <summary>
        /// 全拼
        /// </summary>
        public string QuanPin { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 全局排序
        /// </summary>
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; }  
        #endregion

        #region 普通实列方法
        /// <summary>
        /// 新增
        /// </summary>
        public void Save()
        {
            UserInfo userInfo = UserInfoRepo.Read()
                .FirstOrDefault(user => user.Account != null
                    && this.Account != null
                    && user.Account.ToLower() == this.Account.ToLower());
            if (userInfo != null && userInfo.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            UserInfoRepo.Save(this.MappingTo<UserInfo>());
        }

     

        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            UserInfoRepo.Delete(this.MappingTo<UserInfo>());
            //移除角色用户关系
            List<RoleUserInfo> ruList = RoleUserInfoRepo.Read().Where(ru => ru.UserId == this.Id).ToList();
            foreach (var ru in ruList)
            {
                RoleUserInfoRepo.Delete(ru);
            }
            //移除岗位用户关系
            List<PositionUserInfo> puList = PositionUserInfoRepo.Read().Where(pu => pu.UserId == this.Id).ToList();
            foreach (var pu in puList)
            {
                PositionUserInfoRepo.Delete(pu);
            }
            //移除用户安全信息
            var securityInfo = UserSecurityInfoRepo
                .Read()
                .FirstOrDefault(us => us.Account == this.Account);
            UserSecurityInfoRepo.Delete(securityInfo);
        }

        /// <summary>
        /// 获取当前用户在所有设备上的状态
        /// </summary>
        /// <returns></returns>
        public List<UserState> GetStateList()
        {
            return UserState.GetStateListByAccount(this.Account);
        }

        /// <summary>
        /// 获取当前用户在某一设备上的状态
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public UserState GetStateByDeviceId(string deviceId)
        {
            return UserState.GetState(this.Account, deviceId);
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static User GetUser(string account)
        {
            if (account == null) return null;
            return UserInfoRepo.Read()
                .FirstOrDefault(u => u.Account.ToLower() == account.ToLower())
                .MappingTo<User>();
        }

        public static List<Business.User> GetUserByOrg(string deptId, string positionId)
        {
           var userIdList=  PositionUserInfoRepo.Read()
                .Where(p => p.OrganizationId == deptId && p.PositionId == positionId)
                .Select(p => p.UserId)
                .ToList();
            var userList = UserInfoRepo.Read().Where(p => userIdList.Contains(p.Id)).ToList().MappingToList<Business.User>();
            return userList;
        }

        public static List<User> GetUserList()
        {
            var userList = UserInfoRepo.Read()
                 .OrderByDescending(u => u.Sort)
                 .ToList().MappingToList<User>();
            return userList;
        }

        public static void LoadUserRemoteUser()
        {
            List<User> existUserList = GetUserList();
            List<UserInfo> remoteUserList = passport.LoadUser();
            if (remoteUserList != null)
            {
                List<User> loadUserList = remoteUserList
                    .Where(remoteUser => !remoteUser.Account.EndsWith("$")
                        && !existUserList
                            .Exists(existUser => existUser.Account == remoteUser.Account))
                    .ToList()
                    .MappingToList<User>();
                foreach (User loadUser in loadUserList)
                {
                    loadUser.Save();
                }
            }
        }
        public static User Create()
        {
            User user = new User();
            return user;
        }
        public static User GetUserById(string id)
        {
            return UserInfoRepo.Read()
                .FirstOrDefault(u => u.Id == id)
                .MappingTo<User>();
        }

        public static Dictionary<string, string> GetTableColumns()
        {
            return UserInfoRepo.GetTableStruct("User");
        }

        public static string AddExtensionField(ExtensionField field)
        {
            return UserInfoRepo.AddField(field);
        }
        #endregion

        #region 有关组织
        private List<Organization> _OrganizationList = null;
        /// <summary>
        /// 组织
        /// </summary>
        public List<Organization> OrganizationList
        {
            get
            {
                if (_OrganizationList == null)
                {
                    var organizationInfoList = OrganizationInfoRepo.Read();
                    var positionUserInfoList = PositionUserInfoRepo.Read();
                    var result = from org in organizationInfoList
                                 from positionUser in positionUserInfoList
                                 where positionUser.UserId == this.Id
                                    && positionUser.OrganizationId == org.Id
                                    && org.IsActive
                                 select org;
                    _OrganizationList = result.ToList()
                        .MappingToList<Organization>()
                        .OrderByDescending(org => org.Sort)
                        .DistinctBy(org => org.Id)
                        .ToList();
                }
                return _OrganizationList;
            }
        }

        private List<Organization> _DeepOrganizationList = null;
        public List<Organization> DeepOrganizationList
        {
            get
            {
                if (_DeepOrganizationList == null)
                {
                    List<Organization> rsOrgList = new List<Organization>();
                    List<Organization> orgList = this.OrganizationList;
                    foreach (var org in orgList)
                    {
                        rsOrgList.AddRange(org.DeepParentList);
                    }
                    rsOrgList.AddRange(orgList);
                    _DeepOrganizationList = rsOrgList
                        .DistinctBy(item => item.Id)
                        .OrderByDescending(org => org.Sort)
                        .ToList();
                }
                return _DeepOrganizationList;
            }
        }
        #endregion

        #region 有关岗位
        private List<Position> _PositionList = null;
        /// <summary>
        /// 获得岗位
        /// </summary>
        public List<Position> PositionList
        {
            get
            {
                if (_PositionList == null)
                {
                    var result = from position in PositionInfoRepo.Read().ToList()
                                 from positionUser in PositionUserInfoRepo.Read().ToList()
                                 where positionUser.UserId == this.Id
                                       && position.Id == positionUser.PositionId
                                       && position.IsActive
                                 orderby position.Sort descending
                                 select position;
                    _PositionList = result
                        .DistinctBy(item => item.Id)
                        .ToList()
                        .MappingToList<Position>();
                }
                return _PositionList;
            }
        }

        private List<Position> _ReportToList = null;
        /// <summary>
        /// 汇报上级职位列表
        /// </summary>
        public List<Position> ReportToList
        {
            get
            {
                if (_ReportToList == null)
                {
                    var userPositionList = this.PositionList;
                    if (userPositionList == null) return null;
                    var reportToPositionList = new List<Position>();
                    foreach (Position userPosition in userPositionList)
                    {
                        reportToPositionList.AddRange(userPosition.ReportToList);
                    }
                    _ReportToList = reportToPositionList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _ReportToList;
            }
        }

        List<User> userList = new List<User>();
        /// <summary>
        /// 汇报上级用户
        /// </summary>
        public List<User> ReportToUserList
        {
            get
            {
                if (_ReportToList == null)
                {
                    var userPositionList = this.PositionList;
                    if (userPositionList == null) return null;
                    var reportToPositionList = new List<Position>();
                    foreach (Position userPosition in userPositionList)
                    {
                        reportToPositionList.AddRange(userPosition.ReportToList);
                    }
                    _ReportToList = reportToPositionList
                        .DistinctBy(item => item.Id)
                        .ToList();
                  
                    foreach (var position in _ReportToList)
                    {
                        userList.AddRange(GetUserByOrg(position.OrganizationId, position.Id));
                    }
                }
                return userList;
            }
        } 
        #endregion

        #region 权限验证
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool CheckPermission(Operation operation)
        {
            List<Role> roleList = this.CalculatedRoleList;
            foreach (Role role in roleList)
            {
                var rs = role.CheckPermission(operation);
                if (rs)
                {
                    return rs;
                }
            }
            return false;
        }
        #endregion

        #region 有关角色
        /// <summary>
        /// 授予角色
        /// </summary>
        /// <param name="role"></param>
        public Role GiveRole(Role role)
        {
            //角色是否已存在
            if (RoleUserInfoRepo.Read().Exists(ru => ru.RoleId == role.Id && ru.UserId == this.Id))
            {
                return role;
            }

            //检查互斥
            List<Role> linkRoleList = this.DeepRoleList;
            foreach (Role linkRole in linkRoleList)
            {
                if (RoleMutex.CheckMutex(role, linkRole, RoleMutexType.Static))
                {
                    throw new Exception("授予的角色和当前用户或其所在的组织的角色存在静态互斥关系");
                }
            }

            RoleUserInfo roleUserInfo = new RoleUserInfo();
            roleUserInfo.RoleId = role.Id;
            roleUserInfo.UserId = this.Id;
            RoleUserInfoRepo.Add(roleUserInfo);
            return role;
        }

        /// <summary>
        /// 取消角色
        /// </summary>
        /// <param name="role"></param>
        public Role CancelRole(Role role)
        {
            RoleUserInfo roleUserInfo = new RoleUserInfo();
            roleUserInfo.RoleId = role.Id;
            roleUserInfo.UserId = this.Id;
            RoleUserInfoRepo.Delete(roleUserInfo);
            return role;
        }

        private List<Role> _RoleList;
        /// <summary>
        /// 根据用户帐号获取角色列表
        /// </summary>
        /// <returns></returns>
        public List<Role> RoleList
        {
            get
            {
                if (_RoleList == null)
                {
                    var rolelist = from role in RoleInfoRepo.Read()
                                   from roleUser in RoleUserInfoRepo.Read()
                                   where roleUser.UserId == this.Id
                                        && roleUser.RoleId == role.Id
                                   orderby role.Sort descending
                                   select role;
                    _RoleList = rolelist
                        .DistinctBy(item => item.Id)
                        .ToList()
                        .MappingToList<Role>();
                }
                return _RoleList;
            }
        }

        private List<Role> _DeepRoleList;
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<Role> DeepRoleList
        {
            get
            {
                if (_DeepRoleList == null)
                {
                    /*用户自身角色 + 所属组织角色 + 所属岗位角色*/

                    //用户自身角色 
                    List<Role> roleList = this.RoleList;
                    //所在组织的角色
                    List<Organization> orgList = this.DeepOrganizationList;
                    foreach (var org in orgList)
                    {
                        roleList.AddRange(org.DeepRoleList);
                    }
                    //所属岗位的角色
                    List<Position> positionList = this.PositionList;
                    foreach (var position in positionList)
                    {
                        roleList.AddRange(position.DeepRoleList);
                    }
                    _DeepRoleList = roleList
                        .DistinctBy(item => item.Id)
                        .OrderByDescending(role => role.Sort)
                        .ToList();
                }
                return _DeepRoleList;
            }
        }

        List<Role> _CalculatedRoleList;
        /// <summary>
        /// 与 DeepRoleList 的区别是，排除了禁用的角色，以及排除的禁用的组织、岗位的影响。
        /// </summary>
        public List<Role> CalculatedRoleList
        {
            get
            {
                if (_CalculatedRoleList == null)
                {
                    /*用户自身角色 + 所属组织角色 + 所属岗位角色*/
                    //检查用户状态
                    if (!this.IsActive)
                    {
                        return new List<Role>();
                    }
                    //用户自身角色 
                    List<Role> roleList = this.RoleList;
                    //所在启用状态组织的角色
                    List<Organization> orgList = this.DeepOrganizationList
                        .Where(org => org.IsActive)
                        .ToList();
                    foreach (var org in orgList)
                    {
                        roleList.AddRange(org.DeepRoleList);
                    }
                    //所属启用状态岗位的角色
                    List<Position> positionList = this.PositionList
                        .Where(p => p.IsActive)
                        .ToList();
                    foreach (var position in positionList)
                    {
                        roleList.AddRange(position.DeepRoleList);
                    }
                    //去重、排序、排除禁用
                    _CalculatedRoleList = roleList
                        .Where(role => role.IsActive)
                        .DistinctBy(role => role.Id)
                        .OrderByDescending(role => role.Sort)
                        .ToList();
                }
                return _CalculatedRoleList;
            }
        }
        #endregion

        #region 权限操作
        List<Operation> _OperationList;
        public List<Operation> OperationList
        {
            get
            {
                if (_OperationList == null)
                {
                    List<Operation> operationList = new List<Operation>();
                    List<Role> roleList = this.RoleList;
                    foreach (var role in roleList)
                    {
                        operationList.AddRange(role.OperationList);
                    }
                    _OperationList = operationList.DistinctBy(item => item.Id).ToList();
                }
                return _OperationList;
            }
        }

        List<Operation> _DeepOperationList;
        public List<Operation> DeepOperationList
        {
            get
            {
                if (_DeepOperationList == null)
                {
                    List<Operation> operationList = new List<Operation>();
                    List<Role> roleList = this.DeepRoleList;
                    foreach (var role in roleList)
                    {
                        operationList.AddRange(role.OperationList);
                    }
                    _DeepOperationList = operationList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _DeepOperationList;
            }
        }
        #endregion

        #region 权限对象
        List<Target> _TargetList;
        public List<Target> TargetList
        {
            get
            {
                if (_TargetList == null)
                {
                    List<Target> targetList = new List<Target>();
                    List<Role> roleList = this.RoleList;
                    foreach (var role in roleList)
                    {
                        targetList.AddRange(role.TargetList);
                    }
                    _TargetList = targetList.DistinctBy(tag => tag.Id).ToList();
                }
                return _TargetList;
            }
        }

        List<Target> _DeapTargetList;
        public List<Target> DeapTargetList
        {
            get
            {
                if (_DeapTargetList == null)
                {
                    List<Target> targetList = new List<Target>();
                    List<Role> roleList = this.DeepRoleList;
                    foreach (var role in roleList)
                    {
                        targetList.AddRange(role.TargetList);
                    }
                    _DeapTargetList = targetList
                        .DistinctBy(tag => tag.Id)
                        .ToList();
                }
                return _DeapTargetList;
            }
        }

        /// <summary>
        /// 解析一段表达式
        /// </summary>
        /// <param name="expr"></param>
        /// <returns></returns>
        public string ParseExpr(string expr)
        {
            return ExpressionCalculator.Calculate(expr, this);
        }

        List<Target> _CalculatedDeapTargetList;
        /// <summary>
        /// 计算过表达式的权限对象列表
        /// </summary>
        public List<Target> CalculatedDeapTargetList
        {
            get
            {
                if (_CalculatedDeapTargetList == null)
                {
                    List<Target> targetList = this.DeapTargetList;
                    foreach (Target target in targetList)
                    {
                        target.Scope = this.ParseExpr(target.Scope);
                    }
                    _CalculatedDeapTargetList = targetList;
                }
                return _CalculatedDeapTargetList;
            }
        }
        #endregion

        #region 身份验证
        /// <summary>
        /// 检查登录状态
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static UserState GetState(string token)
        {
            //--var md5Token = Encrypt.EncodeBySolt(token);
            UserState userState = UserState.GetStateByToken(token);
            if (userState == null)
            {
                return null;
            }
            //判断是否过期
            if (Common.AppSettings.TokenTimeout != 0
                && (DateTime.Now - userState.LastActive)
                .TotalMinutes >= Common.AppSettings.TokenTimeout)
            {
                userState.Delete();
                return null;
            }
            //更新最后最后活动时间
            userState.LastActive = DateTime.Now;
            userState.Save();
            //向上层提供原Token
            //userState.Token = token;
            return userState;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public static UserState CreateState(AuthParameter parameter)
        {
            #region 检查参数
            if (parameter == null
                || string.IsNullOrWhiteSpace(parameter.Type)
                || string.IsNullOrWhiteSpace(parameter.Account))
            {
                return null;
            }
            #endregion

            #region 验证身份
            if (parameter.Type == AuthType.PASSWORD
                && !string.IsNullOrWhiteSpace(parameter.Password))
            {
                //通过密码验证
                bool validateResult = passport.Validate(parameter.Account, parameter.Password);
                if (!validateResult) return null;
            }
            else if (parameter.Type == AuthType.CLIENT
                && !string.IsNullOrWhiteSpace(parameter.AppKey)
                && !string.IsNullOrWhiteSpace(parameter.AppIp))
            {
                //通过信任的 App 验证
                App app = App.GetApp(parameter.AppKey);
                if (app == null
                    || app.Secret != parameter.AppSecret)
                {//如果 app 不存在或 secret 错误，则返回 null ，验证失败
                    return null;
                }
                else
                {
                    app.WhiteList = app.WhiteList ?? "";
                    var whiteList = app.WhiteList.Split(',')
                        .Select(ip => (ip ?? "").Trim())
                        .ToList();
                    whiteList.AddRange(new List<string> { 
                            "localhost","127.0.0.1","::1"
                        });
                    var appIp = (parameter.AppIp ?? "").Trim();
                    if (!whiteList.Exists(ip => !string.IsNullOrWhiteSpace(ip)
                        && !string.IsNullOrWhiteSpace(appIp)
                        && ip == appIp))
                    {
                        return null;
                    }
                }
            }
            else
            {
                return null;
            }
            #endregion

            #region 检查用户状态
            var existUser = User.GetUser(parameter.Account);
            if (existUser == null || !existUser.IsActive)
            {
                return null;
            }
            #endregion

            parameter.Device = parameter.Device ?? "";
            parameter.DeviceId = parameter.DeviceId ?? "";
            parameter.Ip = parameter.Ip ?? "";

            //检查在同一 DeviceId 下是否已存在状态
            UserState existUserState = UserState.GetState(parameter.Account, parameter.DeviceId);
            if (existUserState != null)
            {
                //利用当前类中 GetState 方法中写好的更新最后活动时间
                return GetState(existUserState.Token);
            }

            //创建新用户会话状态开始
            //生成Token
            string srcToken = StringFactory.HashBySolt(StringFactory.NewGuid());
            UserState newUserState = new UserState();
            newUserState.Account = parameter.Account;
            newUserState.LastActive = DateTime.Now;
            //--存储MD5后的Token，有数据库权限的人也不应该有查看别人会话密钥的权力
            newUserState.Token = srcToken; //Encrypt.EncodeBySolt(srcToken);
            newUserState.Device = parameter.Device;
            newUserState.DeviceId = parameter.DeviceId ?? parameter.Ip;
            newUserState.Ip = parameter.Ip;
            newUserState.Save();
            //创建用户会话状态结束
            //向上层提供原Token
            newUserState.Token = srcToken;
            //
            return newUserState;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static void CancelState(string token)
        {
            UserState userState = GetState(token);
            userState.Delete();
        }
        public static User GetUserByToken(string token)
        {
            UserState userState = GetState(token);
            if (userState == null) return null;
            return GetUser(userState.Account);
        }
        /// <summary>
        /// 重置密码
        /// UI 或 Service 中要实现用户自助 ChangePassword 功能时需自行验证用户是针对自身的行为
        /// </summary>
        /// <param name="password"></param>
        public void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("不能将 {0} 的密码设置为空或空字符串");
            }
            passport.SetPassword(this.Account, password);
        }
        #endregion
    }
}