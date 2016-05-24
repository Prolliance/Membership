
using System.Collections.Generic;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 组织表模型 (唯一条件：Code)
    /// </summary>
    public class OrganizationInfo : IModel
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; } 

    }
}
