using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色互斥关系表模型（唯一条件：Group + Type + RoleCode）
    /// </summary>
    public partial class RoleMutexInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKeyAttribute]
        public string Group { get; set; }
        [ModelIdKeyAttribute]
        public int Type { get; set; }
        [ModelIdKeyAttribute]
        public string RoleId { get; set; }
    }
}
