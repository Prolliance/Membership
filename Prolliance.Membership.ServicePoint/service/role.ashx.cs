using AjaxEngine.Utils;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Models;
using Prolliance.Membership.DataTransfer.Utils;
using Prolliance.Membership.ServicePoint.Lib.Services;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.Service
{
    /// <summary>
    /// Postion 的摘要说明
    /// </summary>
    [Summary(Name = "角色服务", Description = "角色相关 API，可以用户或应用身份访问。")]
    [ServiceAuth(Type = ServiceAuthType.AppOrToken)]
    public class RoleService : ServiceBase
    {
        [Summary(Description = "获取所有角色列表")]
        [AjaxMethod]
        public ServiceResult<List<RoleInfo>> GetRoleList()
        {
            return new ServiceResult<List<RoleInfo>>(RoleAdapter.GetRoleList());
        }

        [Summary(Description = "通过角色ID获取角色")]
        [AjaxMethod]
        public ServiceResult<RoleInfo> GetRoleById(string roleId)
        {
            return new ServiceResult<RoleInfo>(RoleAdapter.GetRoleById(roleId));
        }

        [Summary(Description = "新增角色")]
        [AjaxMethod]
        public ServiceResult<object> AddRole(RoleInfo roleInfo)
        {
            RoleAdapter.AddRole(roleInfo);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "修改角色")]
        [AjaxMethod]
        public ServiceResult<object> UpdateRole(RoleInfo roleInfo)
        {
            RoleAdapter.UpdateRole(roleInfo);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "删除角色")]
        [AjaxMethod]
        public ServiceResult<object> DeleteRole(string roleId)
        {
            RoleAdapter.DeleteRole(roleId);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "获取用户列表")]
        [AjaxMethod]
        public ServiceResult<List<UserInfo>> GetUserList(string roleId)
        {
            return new ServiceResult<List<UserInfo>>(RoleAdapter.GetUserList(roleId));
        }

        [Summary(Description = "获取组织列表")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetOrganizationList(string roleId)
        {
            return new ServiceResult<List<OrganizationInfo>>(RoleAdapter.GetOrganizationList(roleId));
        }

        [Summary(Description = "获取权限列表")]
        [AjaxMethod]
        public ServiceResult<List<OperationInfo>> GetOperationList(string roleId)
        {
            return new ServiceResult<List<OperationInfo>>(RoleAdapter.GetOperationList(roleId));
        }

        [Summary(Description = "获取职位列表")]
        [AjaxMethod]
        public ServiceResult<List<PositionInfo>> GetPositionList(string roleId)
        {
            return new ServiceResult<List<PositionInfo>>(RoleAdapter.GetPositionList(roleId));
        }

        [Summary(Description = "角色下添加用户")]
        [AjaxMethod]
        public ServiceResult<object> AddUserToRole(string roleId, string account)
        {
            RoleAdapter.AddUserToRole(roleId, account);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "给角色授权")]
        [AjaxMethod]
        public ServiceResult<object> GivePermission(string roleId, OperationInfo operation)
        {
            RoleAdapter.GivePermission(roleId, operation);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "取消角色")]
        [AjaxMethod]
        public ServiceResult<object> CancelPermission(string roleId, OperationInfo operation)
        {
            RoleAdapter.CancelPermission(roleId, operation);
            return new ServiceResult<object>(null);
        }
    }
}