
namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 权限对象操作表模型（唯一条件：AppKey + TargetCode + Code）
    /// </summary>
    public class OperationInfo : IModel
    {
        public string Id { get; set; }
        //冗余 AppId 方便通过 App 删除 Operation
        public string AppId { get; set; }
        public string TargetId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }
}
