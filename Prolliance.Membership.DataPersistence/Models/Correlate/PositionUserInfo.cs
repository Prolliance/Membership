using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 岗位用户关系表模型（唯一条件：OrganizationCode + PositionCode + Account）
    /// </summary>
    public class PositionUserInfo : IModel
    {
        public string Id { get; set; }

        [ModelIdKey]
        public string OrganizationId { get; set; }
        [ModelIdKey]
        public string PositionId { get; set; }
        [ModelIdKey]
        public string UserId { get; set; }
        public int Sort { get; set; }
    }
}
