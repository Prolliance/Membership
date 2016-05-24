using System;
using System.Collections.Generic;

namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// 权限对象
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Target : ModelBase
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 作用域动态表达式
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// 构造一个对象
        /// </summary>
        /// <param name="code">对象编码</param>
        /// <param name="name">对象名称</param>
        /// <param name="group">分组</param>
        /// <param name="summary">描述</param>
        public Target(string code, string name, string group, string summary)
        {
            this.Code = code;
            this.Name = name;
            this.Group = group;
            this.Name = summary;
        }

        /// <summary>
        /// 默认构造
        /// </summary>
        public Target() { }

        public List<Operation> OperationList { get; set; }
    }
}

