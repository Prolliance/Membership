using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色组织关系表模型（唯一条件：RoleCode + OrgCode）
    /// </summary>
    public class RoleOrganizationInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKey]
        public string RoleId { get; set; }
        [ModelIdKey]
        public string OrganizationId { get; set; }
    }
}
