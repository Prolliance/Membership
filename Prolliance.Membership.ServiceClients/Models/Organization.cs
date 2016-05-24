
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// ��֯��ģ�� (Ψһ������Code)
    /// </summary>
    public class Organization : ModelBase
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

        /// <summary>
        /// ��չ����
        /// </summary>
        public Dictionary<string, object> Extensions { get; set; }  
        #endregion

        internal const string SERVICE_TYPE = "organization";

        #region ��̬����
        /// <summary>
        /// ��ȡ������֯�б�
        /// </summary>
        /// <returns>��֯�б�</returns>
        public static List<Organization> GetAllOrganizationList()
        {
            return ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetAllOrganizationList", null);
        }
        /// <summary>
        /// ͨһ�� Id ��ȡһ����֯
        /// </summary>
        /// <param name="orgId">��֯Id</param>
        /// <returns></returns>
        public static Organization GetOrganizationById(string orgId)
        {
            return ServiceClient.Get<Organization>(SERVICE_TYPE, "GetOrganizationById", new
            {
                orgId = orgId
            });
        }

        /// <summary>
        /// ��ȡ��֯��չ��
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetTableColumns()
        {
            return ServiceClient.Get<Dictionary<string, string>>(SERVICE_TYPE, "GetTableColumns", null);
        }

        /// <summary>
        /// ͨһ�� Code ��ȡһ����֯
        /// </summary>
        /// <param name="orgCode">��֯Code</param>
        /// <returns></returns>
        public static Organization GetOrganization(string orgCode)
        {
            return ServiceClient.Get<Organization>(SERVICE_TYPE, "GetOrganization", new
            {
                orgCode = orgCode
            });
        }

        /// <summary>
        /// ������༭��֯
        /// </summary>
        /// <param name="organization"></param>
        public static void Save(Organization organization)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Save", new
            {
                organizationInfo = organization
            });
        }

        /// <summary>
        /// ɾ����֯
        /// </summary>
        /// <param name="organId"></param>
        public static void Delete(string organId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "Delete", new
            {
                organId = organId
            });
        }

        #endregion

        #region ��Ա����

        List<Organization> _ChildOrganizationList;
        /// <summary>
        /// ��ȡ��������֯
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetChildOrganizationList()
        {
            if (_ChildOrganizationList == null)
            {
                _ChildOrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetChildOrganizationList", new
                {
                    orgCode = this.Code
                });
            }
            return _ChildOrganizationList;
        }

        List<Organization> _DeepChildOrganizationList;
        /// <summary>
        /// ��ȡ��������֯(�������в㼶�Ӽ�)
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetDeepChildOrganizationList()
        {
            if (_DeepChildOrganizationList == null)
            {
                _DeepChildOrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetDeepChildOrganizationList", new
                {
                    orgCode = this.Code
                });
            }
            return _DeepChildOrganizationList;
        }

        List<User> _UserList;
        /// <summary>
        /// ��ȡ�����������û�
        /// </summary>
        /// <returns></returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _UserList;
        }

        List<User> _DeepUserList;
        /// <summary>
        /// ��ȡ�����������û�(������������֯�û�)
        /// </summary>
        /// <returns></returns>
        public List<User> GetDeepUserList()
        {
            if (_DeepUserList == null)
            {
                _DeepUserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetDeepUserListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _DeepUserList;
        }

        List<Position> _PositionList;
        /// <summary>
        /// ��ȡ�����µ�����ְλ
        /// </summary>
        /// <returns>ְλ�б�</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                _PositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _PositionList;
        }

        List<Position> _DeepPositionList;
        /// <summary>
        /// ��ȡ�����µ�����ְλ (��������֯��ְλ)
        /// </summary>
        /// <returns>ְλ�б�</returns>
        public List<Position> GetDeepPositionList()
        {
            if (_DeepPositionList == null)
            {
                _DeepPositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetDeepPositionListByOrgId", new
                {
                    orgId = this.Id
                });
            }
            return _DeepPositionList;
        }

        /// <summary>
        /// ����֯���������Ա
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User AddUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "AddUser", new { orgId = this.Id, user = user });
        }

        /// <summary>
        /// ����֯�����Ƴ���Ա
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        public User RemoveUser(User user)
        {
            return ServiceClient.Post<User>(SERVICE_TYPE, "RemoveUser", new { orgId = this.Id, user = user });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role GiveRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "GiveRole", new { organId = this.Id, roleInfo = role });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public Role CancelRole(Role role)
        {
            return ServiceClient.Post<Role>(SERVICE_TYPE, "CancelRole", new { organId = this.Id, roleInfo = role });
        }
        #endregion
    }
}
