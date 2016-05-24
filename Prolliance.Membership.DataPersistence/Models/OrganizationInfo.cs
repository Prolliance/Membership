
using System.Collections.Generic;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// ��֯��ģ�� (Ψһ������Code)
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
