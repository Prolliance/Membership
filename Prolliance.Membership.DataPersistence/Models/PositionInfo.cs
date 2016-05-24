
namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 岗位表模型 (唯一条件：OrgCode + Code)
    /// </summary>
    public class PositionInfo : IModel
    {
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
    }
}
