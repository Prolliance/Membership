
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// ��ɫ��ģ�ͣ�Ψһ������Code��
    /// </summary>
    public class RoleInfo : ModelBase
    {
        #region ����
        /// <summary>
        /// ����
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }
        #endregion
    }
}
