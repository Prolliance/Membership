
namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 角色表模型（唯一条件：Code）
    /// </summary>
    public class RoleInfo : IModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }
    }
}
