
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 组织表模型 (唯一条件：Code)
    /// </summary>
    public class Organization : ModelBase
    {
        #region 普通属性
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId { set; get; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public Dictionary<string, object> Extensions { get; set; }  
        #endregion

        internal const string SERVICE_TYPE = "organization";

        #region 静态方法
        /// <summary>
        /// 获取完整组织列表
        /// </summary>
        /// <returns>组织列表</returns>
        public static List<Organization> GetAllOrganizationList()
        {
            return ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetAllOrganizationList", null);
        }
        /// <summary>
        /// 通一个 Id 获取一个组织
        /// </summary>
        /// <param name="orgId">组织Id</param>
        /// <returns></returns>
        public static Organization GetOrganizationById(string orgId)
        {
            return ServiceClient.Get<Organization>(SERVICE_TYPE, "GetOrganizationById", new
            {
                orgId = orgId
            });
        }

        /// <summary>
        /// 获取组织扩展列
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetTableColumns()
        {
            return ServiceClient.Get<Dictionary<string, string>>(SERVICE_TYPE, "GetTableColumns", null);
        }

        /// <summary>
        /// 通一个 Code 获取一个组织
        /// </summary>
        /// <param name="orgCode">组织Code</param>
        /// <returns></returns>
        public static Organization GetOrganization(string orgCode)
        {
            return ServiceClient.Get<Organization>(SERVICE_TYPE, "GetOrganization", new
            {
                orgCode = orgCode
            });
        }

        /// <summary>
        /// 新增或编辑组织
        /// </summary>
        /// <param name="organization"></param>
        public static void Save(Organization organization)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Save", new
            {
                organizationInfo = organization
            });
        }

        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="organId"></param>
        public static void Delete(string organId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Delete", new
            {
                organId = organId
            });
        }

        #endregion

        #region 成员方法

        List<Organization> _ChildOrganizationList;
        /// <summary>
        /// 获取所有子组织
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetChildOrganizationList()
        {
            if (_ChildOrganizationList == null)
            {
                _ChildOrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetChildOrganizationList", new
                {
                    orgCode = this.Code
                });
            }
            return _ChildOrganizationList;
        }

        List<Organization> _DeepChildOrganizationList;
        /// <summary>
        /// 获取所有子组织(包括所有层级子级)
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetDeepChildOrganizationList()
        {
            if (_DeepChildOrganizationList == null)
            {
                _DeepChildOrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetDeepChildOrganizationList", new
                {
                    orgCode = this.Code
                });
            }
            return _DeepChildOrganizationList;
        }

        List<User> _UserList;
        /// <summary>
        /// 获取部门下所有用户
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _UserList;
        }

        List<User> _DeepUserList;
        /// <summary>
        /// 获取部门下所有用户(包括所有子组织用户)
        /// </summary>
        /// <returns></returns>
        public List<User> GetDeepUserList()
        {
            if (_DeepUserList == null)
            {
                _DeepUserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetDeepUserListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _DeepUserList;
        }

        List<Position> _PositionList;
        /// <summary>
        /// 获取部门下的所有职位
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                _PositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _PositionList;
        }

        List<Position> _DeepPositionList;
        /// <summary>
        /// 获取部门下的所有职位 (包括子组织的职位)
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetDeepPositionList()
        {
            if (_DeepPositionList == null)
            {
                _DeepPositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetDeepPositionListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _DeepPositionList;
        }

        /// <summary>
        /// 在组织下面添加人员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User AddUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "AddUser", new { orgId = this.Id, user = user });
        }

        /// <summary>
        /// 在组织下面移除人员
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User RemoveUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "RemoveUser", new { orgId = this.Id, user = user });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role GiveRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "GiveRole", new { organId = this.Id, roleInfo = role });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role CancelRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "CancelRole", new { organId = this.Id, roleInfo = role });
        }
        #endregion
    }
}
