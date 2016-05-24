
namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 应用表模型 (唯一条件：Key)
    /// </summary>
    public class AppInfo : IModel
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Name { get; set; }
        public string WhiteList { get; set; }
        public bool IsActive { get; set; }
        public string Summary { get; set; }
        public int Sort { get; set; }
    }
}
