using System;
using System.Collections.Generic;

namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public class User : ModelBase
    {
        internal const string SERVICE_TYPE = "user";

        /// <summary>
        /// 默认构造
        /// </summary>
        public User() { }

        /// <summary>
        /// 当前登录状态
        /// </summary>
        public UserState State { get; set; }

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

        public Dictionary<string, object> Extensions { get; set; }  

        #endregion

        #region 静态方法
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="parameter">登录信息</param>
        /// <returns>用户状态</returns>
        public static UserState Login(AuthParameter parameter)
        {
            UserState state = ServiceClient.Post<UserState>(SERVICE_TYPE, "Login", new
            {
                parameter = parameter
            });
            return state;
        }

        /// <summary>
        /// 通过密钥获取一个用户状态
        /// </summary>
        /// <param name="token"></param>
        /// <returns>用户状态</returns>
        public static UserState GetState(string token)
        {
            UserState state = ServiceClient.Post<UserState>(SERVICE_TYPE, "GetState", new
            {
                token = token
            });
            return state;
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns>用户</returns>
        public static User GetUser(UserState state)
        {
            User user = ServiceClient.Get<User>(SERVICE_TYPE, "GetUser", new
            {
                token = state.Token
            });
            user.State = state;
            return user;
        }

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户</returns>
        public static User GetUserById(string userId)
        {
            return ServiceClient.Get<User>(SERVICE_TYPE, "GetUserById", new
            {
                userId = userId
            });
        }

        /// <summary>
        /// 获取人员扩展列
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetTableColumns()
        {
            return ServiceClient.Get<Dictionary<string, string>>(SERVICE_TYPE, "GetTableColumns", null);
        }

        /// <summary>
        /// 通过账号获取一个用户
        /// </summary>
        /// <param name="account">账号</param>
        /// <returns>用户</returns>
        public static User GetUserByAccount(string account)
        {
            return ServiceClient.Get<User>(SERVICE_TYPE, "GetUserByAccount", new
            {
                account = account
            });
        }

     

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAllUserList()
        {
            return ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetAllUserList", null);
        }

        /// <summary>
        /// 获取所有属于至少一个组织用户
        /// </summary>
        /// <returns></returns>
        public static List<User> GetUserListOfOrganization()
        {
            return ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserListOfOrganization", null);
        }

        /// <summary>
        /// 获取人员照片
        /// </summary>
        /// <param name="account"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static UserPhotoInfo GetUserPhotoByAccount(string account, string format)
        {
            return ServiceClient.Get<UserPhotoInfo>(SERVICE_TYPE, "GetUserPhotoByAccount", new { account = account, format = format });
        }

        /// <summary>
        /// 创建新用户
        /// </summary>
        /// <param name="user">user实体</param>
        public static void CreateUser(User user)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "CreateUser", new { user = user });
        }

        /// <summary>
        /// 新增或编辑用户
        /// </summary>
        /// <param name="user"></param>
        public static void Save(User user)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Save", new
            {
                userInfo = user
            });
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        public static void Delete(string userId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Delete", new
            {
                userId = userId
            });
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="newPassword">新密码</param>
        public static void ResetPassword(string account, string newPassword)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "ResetPassword", new { account = account, newPassword = newPassword });
        }

        public static void LoadUserRemoteUser()
        {
            ServiceClient.Get<object>(SERVICE_TYPE, "LoadUserRemoteUser", null);
        }
        #endregion

        #region 针对当前会话用户实例的方法
        /// <summary>
        /// 登出（仅当前会话用户可以调用）
        /// </summary>
        public void Logout()
        {
            if (State == null) throw new Exception(string.Format("不能访问用户 ‘{0}’ 的会话状态", this.Account));
            ServiceClient.Post<object>(SERVICE_TYPE, "Logout", new
            {
                token = State.Token,
                userId = this.Id
            });
        }

        /// <summary>
        /// 更改当前用户密码（仅当前会话用户可以调用）
        /// </summary>
        public void ChangePassword(string newPassword)
        {
            if (State == null) throw new Exception(string.Format("不能访问用户 ‘{0}’ 的会话状态", this.Account));
            ServiceClient.Post<object>(SERVICE_TYPE, "ChangePassword", new
            {
                token = State.Token,
                userId = this.Id,
                newPassword = newPassword
            });
        }

        /// <summary>
        /// 检查权限（仅当前会话用户可以调用）
        /// </summary>
        /// <param name="operation">操作描述对象</param>
        /// <returns>逻辑结果</returns>
        public bool CheckPermission(Operation operation)
        {
            if (State == null) throw new Exception(string.Format("不能访问用户 ‘{0}’ 的会话状态", this.Account));
            operation.AppKey = ServiceClient.Options.AppKey;
            bool? rs = ServiceClient.Get<bool?>(SERVICE_TYPE, "CheckPermission", new
            {
                token = State.Token,
                userId = this.Id,
                operation = operation
            });
            if (rs == null) return false;
            return Convert.ToBoolean(rs);
        }

        List<Operation> _OperationList;
        /// <summary>
        /// 获取权限操作列表（仅当前会话用户可以调用）
        /// </summary>
        /// <returns>权限操作列表</returns>
        public List<Operation> GetOperationList()
        {
            if (_OperationList == null)
            {
                if (State == null) throw new Exception(string.Format("不能访问用户 ‘{0}’ 的会话状态", this.Account));
                _OperationList = ServiceClient.Get<List<Operation>>(SERVICE_TYPE, "GetOperationList", new
                {
                    token = State.Token,
                    userId = this.Id
                });
            }
            return _OperationList;
        }

        public  List<User> GetReportToUsers()
        {
            return ServiceClient.Post<List<User>>(SERVICE_TYPE, "GetReportToUsers", new
            {
                userId = this.Id
            });
        } 

        List<Target> _TargetList;
        /// <summary>
        /// 获取权限对象列表（仅当前会话用户可以调用）
        /// </summary>
        /// <returns>权限对象列表</returns>
        public List<Target> GetTargetList()
        {
            if (_TargetList == null)
            {
                if (State == null) throw new Exception(string.Format("不能访问用户 ‘{0}’ 的会话状态", this.Account));
                _TargetList = ServiceClient.Get<List<Target>>(SERVICE_TYPE, "GetTargetList", new
                {
                    token = State.Token,
                    userId = this.Id
                });
            }
            return _TargetList;
        }
        #endregion

        #region 针对任意用户实例的方法

        List<Organization> _OrganizationList;
        /// <summary>
        /// 获取当前用户所属的部门列表
        /// </summary>
        /// <returns>部门列表</returns>
        public List<Organization> GetOrganizationList()
        {
            if (_OrganizationList == null)
            {
                _OrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetOrganizationList", new
                {
                    userId = this.Id
                });
            }
            return _OrganizationList;
        }


        public List<Organization> GetRangeOrganization(string orgcode)
        {
            if (_OrganizationList == null)
            {
                _OrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetRangeOrganization", new
                {
                    userAccount = this.Account,
                    orgcode=orgcode
                });
            }
            return _OrganizationList;
        }
            
        List<Position> _PositionList;
        /// <summary>
        /// 获取当前用户所属的职位列表
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                _PositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionList", new
                {
                    userId = this.Id
                });
            }
            return _PositionList;
        }

        List<Position> _ReportToList;
        /// <summary>
        /// 获取当前用户汇报上级职位列表
        /// </summary>
        /// <returns></returns>
        public List<Position> GetReportToList()
        {
            if (_ReportToList == null)
            {
                _ReportToList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetReportToList", new
                {
                    userId = this.Id
                });
            }
            return _ReportToList;
        }

        List<Role> _RoleList;
        /// <summary>
        /// 获取当前用户所属的角色列表
        /// </summary>
        /// <returns>角色列表</returns>
        public List<Role> GetRoleList()
        {
            if (_RoleList == null)
            {
                _RoleList = ServiceClient.Get<List<Role>>(SERVICE_TYPE, "GetRoleList", new
                {
                    userId = this.Id
                });
            }
            return _RoleList;
        }

        /// <summary>
        /// 以当前用户身份解析一段表达式
        /// </summary>
        /// <param name="expr">表达式</param>
        /// <returns>解析结果</returns>
        public string ParseExpr(string expr)
        {
            var rs = ServiceClient.Get<object>(SERVICE_TYPE, "ParseExprByUserId", new
            {
                userId = this.Id,
                expr = expr
            });
            if (rs != null)
            {
                return rs.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role GiveRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "GiveRole", new { userid = this.Id, roleInfo = role });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role CancelRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "CancelRole", new { userid = this.Id, roleInfo = role });
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class UserPhotoInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] PhotoBinary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PhotoExt { get; set; }
    }
}
