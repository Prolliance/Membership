
using System.Collections.Generic;
namespace Prolliance.Membership.DataTransfer.Models
{
    /// <summary>
    /// Ȩ�޶����ģ�ͣ�Ψһ������AppKey + Code��
    /// </summary>
    public class TargetInfo : ModelBase
    {

        #region ����

        /// <summary>
        /// Key
        /// </summary>
        public string AppId { get; set; }

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
        /// ����
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// ��������
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// ������̬���ʽ
        /// </summary>
        public string Scope { get; set; }
        public string AppKey { get; set; }

        #endregion

        public List<OperationInfo> OperationList { get; set; }
    }
}
