
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 岗位表模型 (唯一条件：OrgCode + Code)
    /// </summary>
    public class Position : ModelBase
    {
        internal const string SERVICE_TYPE = "position";

        #region 属性
        /// <summary>
        /// 组织Code
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public string OrganizationCode { get; set; }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获取所有职位列表
        /// </summary>
        /// <returns>职位</returns>
        public static List<Position> GetPositionList()
        {
            return ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionList", new { });
        }

        /// <summary>
        /// 通过组织代码和职位代码获取职位
        /// </summary>
        /// <param name="organizationCode">组织代码</param>
        /// <param name="positionCode">职位代码</param>
        /// <returns>职位</returns>
        public static Position GetPosition(string organizationCode, string positionCode)
        {
            return ServiceClient.Get<Position>(SERVICE_TYPE, "GetPosition", new
            {
                organizationCode = organizationCode,
                positionCode = positionCode
            });
        }

        /// <summary>
        /// 通过职位id获了职位
        /// </summary>
        /// <param name="positionId">职位id</param>
        /// <returns>职位</returns>
        public static Position GetPositionById(string positionId)
        {
            return ServiceClient.Get<Position>(SERVICE_TYPE, "GetPositionById", new
            {
                positionId = positionId
            });
        }

        /// <summary>
        /// 新增或编辑组织
        /// </summary>
        /// <param name="position"></param>
        public static void Save(Position position)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Save", new
            {
                positionInfo = position
            });
        }

        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="positionId"></param>
        public static void Delete(string positionId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Delete", new
            {
                positionId = positionId
            });
        }
        #endregion

        #region 实例方法

        List<Position> _ReportToList;
        /// <summary>
        /// 获取汇报链的上级职位列表
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetReportToList()
        {
            if (_ReportToList == null)
            {
                _ReportToList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetReportToListById", new
                {
                    positionId = this.Id
                });
            }
            return _ReportToList;
        }

        List<User> _UserList;

        /// <summary>
        /// 获取汇报关系
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public List<Position> ReportToList()
        {
            var list = ServiceClient.Post<List<Position>>(SERVICE_TYPE, "ReportToList", new
            {
                positionId = this.Id
            });
            return list;
        }

        /// <summary>
        /// 移除汇报关系
        /// </summary>
        /// <param name="toPositionId"></param>
        /// <returns></returns>
        public Position RemoveReportTo(string toPositionId)
        {
            var position = ServiceClient.Post<Position>(SERVICE_TYPE, "RemoveReportTo", new
            {
                positionId = this.Id,
                toPositionId = toPositionId
            });
            return position;
        }

        /// <summary>
        /// 添加汇报关系
        /// </summary>
        /// <param name="toPositionId"></param>
        /// <returns></returns>
        public Position AddReportTo(string toPositionId)
        {
            var position = ServiceClient.Post<Position>(SERVICE_TYPE, "AddReportTo", new
            {
                positionId = this.Id,
                toPositionId = toPositionId
            });
            return position;
        }

        /// <summary>
        /// 获取当前职位的用户列表
        /// </summary>
        /// <returns>职位列表</returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserList", new
                {
                    positionId = this.Id
                });
            }
            return _UserList;
        }

        /// <summary>
        /// 在职位下面添加人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User AddUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "AddUser", new { positionId = this.Id, user = user });
        }

        /// <summary>
        /// 在职位下面移除人员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User RemoveUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "RemoveUser", new { positionId = this.Id, user = user });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role GiveRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "GiveRole", new { positionId = this.Id, roleInfo = role });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role CancelRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "CancelRole", new { positionId = this.Id, roleInfo = role });
        }
        #endregion
    }
}
