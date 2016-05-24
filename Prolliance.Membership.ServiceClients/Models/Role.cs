
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 角色表模型（唯一条件：Code）
    /// </summary>
    public class Role : ModelBase
    {
        internal const string SERVICE_TYPE = "role";

        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoleList()
        {
            return ServiceClient.Get<List<Role>>(SERVICE_TYPE, "GetRoleList", null);
        }

        /// <summary>
        /// 通过角色ID获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Role GetRoleById(string roleId)
        {
            return ServiceClient.Get<Role>(SERVICE_TYPE, "GetRoleById", new { roleId = roleId });
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role">角色实体</param>
        public static void AddRole(Role role)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddRole", new { roleInfo = role });
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">角色实体</param>
        public static void UpdateRole(Role role)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "UpdateRole", new { roleInfo = role });
        }

     
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public static void DeleteRole(string roleId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "DeleteRole", new { roleId = roleId });
        }
        #endregion

        #region 实例方法
        List<User> _UserList;
        /// <summary>
        /// 获取属于这个角色的所有用户
        /// </summary>
        /// <returns>用户列表</returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserList", new
                {
                    roleId = this.Id
                });
            }
            return _UserList;
        }

        List<Organization> _OrganizationList;
        /// <summary>
        /// 获取属于这个角色的所有组织
        /// </summary>
        /// <returns>组织列表</returns>
        public List<Organization> GetOrganizationList()
        {
            if (_OrganizationList == null)
            {
                _OrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetOrganizationList", new
                {
                    roleId = this.Id
                });
            }
            return _OrganizationList;
        }

        List<Operation> _OperationList;
        /// <summary>
        /// 获取属于这个角色的所有权限
        /// </summary>
        /// <returns>权限列表</returns>
        public List<Operation> GetOperationList()
        {
            if(_OperationList == null)
            {
                _OperationList = ServiceClient.Get<List<Operation>>(SERVICE_TYPE, "GetOperationList", new
                {
                    roleId = this.Id
                });
            }
            return _OperationList;
        }

        List<Position> _PositionList;
        /// <summary>
        /// 获取属于这个角色的所有职位
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                _PositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionList", new
                {
                    roleId = this.Id
                });
            }
            return _PositionList;
        }

        /// <summary>
        /// 添加用户账号到角色下
        /// </summary>
        /// <param name="account"></param>
        public void AddUserToRole(string account)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddUserToRole", new { roleId = this.Id, account = account });
        }
        public void GivePermission(Operation operation)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "GivePermission", new { roleId = this.Id, operation = operation });
        }

        public void CancelPermission(Operation operation)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "CancelPermission", new { roleId = this.Id, operation = operation });
        }
        #endregion
    }
}
