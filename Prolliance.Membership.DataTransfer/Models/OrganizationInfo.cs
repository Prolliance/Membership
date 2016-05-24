
using System.Collections.Generic;

namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// ��֯��ģ�� (Ψһ������Code)
    /// </summary>
    public class OrganizationInfo : ModelBase
    {
        #region ��ͨ����
        /// <summary>
        /// ����
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ȫ��
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// �ϼ�ID
        /// </summary>
        public string ParentId { set; get; }

        /// <summary>
        /// ����
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; }  

        #endregion

    }
}
