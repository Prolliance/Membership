using System;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 针对权限对象的操作
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Operation : ModelBase
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public Operation() { }
        /// <summary>
        /// 通过权限对象代码和权限操作代码构造
        /// </summary>
        /// <param name="targetCode">权限对象编码</param>
        /// <param name="code">权限操作编码</param>
        public Operation(string targetCode, string code)
        {
            this.TargetCode = targetCode;
            this.Code = code;
        }
        /// <summary>
        /// 构造一个 Operation
        /// </summary>
        /// <param name="targetCode">对象编码</param>
        /// <param name="code">操作编码</param>
        /// <param name="name">操作名称</param>
        public Operation(string targetCode, string code, string name)
        {
            this.TargetCode = targetCode;
            this.Code = code;
            this.Name = name;
        }
        /// <summary>
        /// 构造一个 Operation
        /// </summary>
        /// <param name="targetCode">对象编码</param>
        /// <param name="code">操作编码</param>
        /// <param name="name">操作名称</param>
        /// <param name="summary">操作描述</param>
        public Operation(string targetCode, string code, string name, string summary)
        {
            this.TargetCode = targetCode;
            this.Code = code;
            this.Name = name;
            this.Summary = summary;
        }
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppKey { get; set; }
        /// <summary>
        /// TargetCode 用于指定操作属于那个权限对象，必须指定
        /// </summary>
        public string TargetCode { get; set; }
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
    }
}
