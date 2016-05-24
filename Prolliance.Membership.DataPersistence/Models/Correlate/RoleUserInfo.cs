using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色用户系表模型（唯一条件：RoleCode + Account）
    /// </summary>
    public class RoleUserInfo : IModel
    {
        public string Id { get; set; }
        [ModelIdKey]
        public string RoleId { get; set; }
        [ModelIdKey]
        public string UserId { get; set; }
    }
}
