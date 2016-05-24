
using System.Collections.Generic;

namespace Prolliance.Membership.DataPersistence.Models
{
    /// <summary>
    /// 用户表模型（唯一条件：Account）
    /// </summary>
    public class UserInfo : IModel
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string JianPin { get; set; }
        public string QuanPin { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; } 
    }
}
