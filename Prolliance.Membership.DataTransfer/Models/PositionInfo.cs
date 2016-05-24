
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// ��λ��ģ�� (Ψһ������OrgCode + Code)
    /// </summary>
    public class PositionInfo : ModelBase
    {
        #region ����
        /// <summary>
        /// ��֯Code
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// �Ƿ�����
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }

        public string OrganizationCode { get; set; }
        #endregion
    }
}
