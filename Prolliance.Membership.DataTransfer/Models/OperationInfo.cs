
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 权限对象操作表模型（唯一条件：AppKey + TargetCode + Code）
    /// </summary>
    public class OperationInfo : ModelBase
    {
        public string AppId { get; set; }
        /// <summary>
        /// TargetCode 用于指定操作属于那个权限对象，必须指定
        /// </summary>
        public string TargetId { get; set; }
        /// <summary>
        /// Code 是指操作的编码，在同一个 TargetCode 下必须唯一，必须指定
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Name 为操作的名称，可以重复，必须指定
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 操作的说明信息，通常可以省略
        /// </summary>
        public string Summary { get; set; }
        public string AppKey { get; set; }
        public string TargetCode { get; set; }

    }
}
