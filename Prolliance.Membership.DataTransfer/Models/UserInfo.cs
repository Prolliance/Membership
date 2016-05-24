
using System.Collections.Generic;

namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// 用户表模型（唯一条件：Account）
    /// </summary>
    public class UserInfo : ModelBase
    {
        #region 属性

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 办公电话
        /// </summary>
        public string OfficePhone { get; set; }

        /// <summary>
        /// 简拼
        /// </summary>
        public string JianPin { get; set; }

        /// <summary>
        /// 全拼
        /// </summary>
        public string QuanPin { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 全局排序
        /// </summary>
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; }  
        #endregion

    }
}
