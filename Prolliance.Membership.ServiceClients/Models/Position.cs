
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// ��λ��ģ�� (Ψһ������OrgCode + Code)
    /// </summary>
    public class Position : ModelBase
    {
        internal const string SERVICE_TYPE = "position";

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
        /// <summary>
        /// ���� 
        /// </summary>
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

        #region ��̬����
        /// <summary>
        /// ��ȡ����ְλ�б�
        /// </summary>
        /// <returns>ְλ</returns>
        public static List<Position> GetPositionList()
        {
            return ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionList", new { });
        }

        /// <summary>
        /// ͨ����֯�����ְλ�����ȡְλ
        /// </summary>
        /// <param name="organizationCode">��֯����</param>
        /// <param name="positionCode">ְλ����</param>
        /// <returns>ְλ</returns>
        public static Position GetPosition(string organizationCode, string positionCode)
        {
            return ServiceClient.Get<Position>(SERVICE_TYPE, "GetPosition", new
            {
                organizationCode = organizationCode,
                positionCode = positionCode
            });
        }

        /// <summary>
        /// ͨ��ְλid����ְλ
        /// </summary>
        /// <param name="positionId">ְλid</param>
        /// <returns>ְλ</returns>
        public static Position GetPositionById(string positionId)
        {
            return ServiceClient.Get<Position>(SERVICE_TYPE, "GetPositionById", new
            {
                positionId = positionId
            });
        }

        /// <summary>
        /// ������༭��֯
        /// </summary>
        /// <param name="position"></param>
        public static void Save(Position position)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Save", new
            {
                positionInfo = position
            });
        }

        /// <summary>
        /// ɾ����֯
        /// </summary>
        /// <param name="positionId"></param>
        public static void Delete(string positionId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Delete", new
            {
                positionId = positionId
            });
        }
        #endregion

        #region ʵ������

        List<Position> _ReportToList;
        /// <summary>
        /// ��ȡ�㱨�����ϼ�ְλ�б�
        /// </summary>
        /// <returns>ְλ�б�</returns>
        public List<Position> GetReportToList()
        {
            if (_ReportToList == null)
            {
                _ReportToList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetReportToListById", new
                {
                    positionId = this.Id
                });
            }
            return _ReportToList;
        }

        List<User> _UserList;

        /// <summary>
        /// ��ȡ�㱨��ϵ
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public List<Position> ReportToList()
        {
            var list = ServiceClient.Post<List<Position>>(SERVICE_TYPE, "ReportToList", new
            {
                positionId = this.Id
            });
            return list;
        }

        /// <summary>
        /// �Ƴ��㱨��ϵ
        /// </summary>
        /// <param name="toPositionId"></param>
        /// <returns></returns>
        public Position RemoveReportTo(string toPositionId)
        {
            var position = ServiceClient.Post<Position>(SERVICE_TYPE, "RemoveReportTo", new
            {
                positionId = this.Id,
                toPositionId = toPositionId
            });
            return position;
        }

        /// <summary>
        /// ��ӻ㱨��ϵ
        /// </summary>
        /// <param name="toPositionId"></param>
        /// <returns></returns>
        public Position AddReportTo(string toPositionId)
        {
            var position = ServiceClient.Post<Position>(SERVICE_TYPE, "AddReportTo", new
            {
                positionId = this.Id,
                toPositionId = toPositionId
            });
            return position;
        }

        /// <summary>
        /// ��ȡ��ǰְλ���û��б�
        /// </summary>
        /// <returns>ְλ�б�</returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserList", new
                {
                    positionId = this.Id
                });
            }
            return _UserList;
        }

        /// <summary>
        /// ��ְλ���������Ա
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User AddUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "AddUser", new { positionId = this.Id, user = user });
        }

        /// <summary>
        /// ��ְλ�����Ƴ���Ա
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User RemoveUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "RemoveUser", new { positionId = this.Id, user = user });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role GiveRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "GiveRole", new { positionId = this.Id, roleInfo = role });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role CancelRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "CancelRole", new { positionId = this.Id, roleInfo = role });
        }
        #endregion
    }
}
