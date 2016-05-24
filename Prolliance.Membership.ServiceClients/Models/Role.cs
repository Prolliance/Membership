
using System.Collections.Generic;
namespace Prolliance.Membership.ServiceClients.Models
{
    /// <summary>
    /// ��ɫ��ģ�ͣ�Ψһ������Code��
    /// </summary>
    public class Role : ModelBase
    {
        internal const string SERVICE_TYPE = "role";

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

        #region ��̬����
        /// <summary>
        /// ��ȡ���н�ɫ�б�
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoleList()
        {
            return ServiceClient.Get<List<Role>>(SERVICE_TYPE, "GetRoleList", null);
        }

        /// <summary>
        /// ͨ����ɫID��ȡ��ɫ
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static Role GetRoleById(string roleId)
        {
            return ServiceClient.Get<Role>(SERVICE_TYPE, "GetRoleById", new { roleId = roleId });
        }

        /// <summary>
        /// ������ɫ
        /// </summary>
        /// <param name="role">��ɫʵ��</param>
        public static void AddRole(Role role)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddRole", new { roleInfo = role });
        }

        /// <summary>
        /// �޸Ľ�ɫ
        /// </summary>
        /// <param name="role">��ɫʵ��</param>
        public static void UpdateRole(Role role)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "UpdateRole", new { roleInfo = role });
        }

     
        /// <summary>
        /// ɾ����ɫ
        /// </summary>
        /// <param name="roleId">��ɫID</param>
        public static void DeleteRole(string roleId)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "DeleteRole", new { roleId = roleId });
        }
        #endregion

        #region ʵ������
        List<User> _UserList;
        /// <summary>
        /// ��ȡ���������ɫ�������û�
        /// </summary>
        /// <returns>�û��б�</returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                _UserList = ServiceClient.Get<List<User>>(SERVICE_TYPE, "GetUserList", new
                {
                    roleId = this.Id
                });
            }
            return _UserList;
        }

        List<Organization> _OrganizationList;
        /// <summary>
        /// ��ȡ���������ɫ��������֯
        /// </summary>
        /// <returns>��֯�б�</returns>
        public List<Organization> GetOrganizationList()
        {
            if (_OrganizationList == null)
            {
                _OrganizationList = ServiceClient.Get<List<Organization>>(SERVICE_TYPE, "GetOrganizationList", new
                {
                    roleId = this.Id
                });
            }
            return _OrganizationList;
        }

        List<Operation> _OperationList;
        /// <summary>
        /// ��ȡ���������ɫ������Ȩ��
        /// </summary>
        /// <returns>Ȩ���б�</returns>
        public List<Operation> GetOperationList()
        {
            if(_OperationList == null)
            {
                _OperationList = ServiceClient.Get<List<Operation>>(SERVICE_TYPE, "GetOperationList", new
                {
                    roleId = this.Id
                });
            }
            return _OperationList;
        }

        List<Position> _PositionList;
        /// <summary>
        /// ��ȡ���������ɫ������ְλ
        /// </summary>
        /// <returns>ְλ�б�</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                _PositionList = ServiceClient.Get<List<Position>>(SERVICE_TYPE, "GetPositionList", new
                {
                    roleId = this.Id
                });
            }
            return _PositionList;
        }

        /// <summary>
        /// ����û��˺ŵ���ɫ��
        /// </summary>
        /// <param name="account"></param>
        public void AddUserToRole(string account)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "AddUserToRole", new { roleId = this.Id, account = account });
        }
        public void GivePermission(Operation operation)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "GivePermission", new { roleId = this.Id, operation = operation });
        }

        public void CancelPermission(Operation operation)
        {
            ServiceClient.Post<object>(SERVICE_TYPE, "CancelPermission", new { roleId = this.Id, operation = operation });
        }
        #endregion
    }
}
