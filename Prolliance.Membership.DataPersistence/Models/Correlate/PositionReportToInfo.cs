using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 岗位汇报关系表模型（唯一条件：OrganizationCode + PositionCode + HigherOrganizationCode + HigherPositionCode）
    /// </summary>
    public class PositionReportToInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKey]
        public string OrganizationId { get; set; }
        [ModelIdKey]
        public string HigherOrganizationId { get; set; }

        [ModelIdKey]
        public string PositionId { get; set; }
        [ModelIdKey]
        public string HigherPositionId { get; set; }
    }
}
