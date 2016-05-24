using AjaxEngine.Utils;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Models;
using Prolliance.Membership.DataTransfer.Utils;
using Prolliance.Membership.ServicePoint.Lib.Services;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.service
{
    /// <summary>
    /// OrganizationService 的摘要说明
    /// </summary>
    [Summary(Name = "组织服务", Description = "组织相关 API，可以用户或应用身份访问。")]
    [ServiceAuth(Type = ServiceAuthType.AppOrToken)]
    public class OrganizationService : ServiceBase
    {
        [Summary(Description = "获取组织列表")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetAllOrganizationList()
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetAllOrganizationList());
        }

        [Summary(Description = "通过 Id 获取组织", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public ServiceResult<OrganizationInfo> GetOrganizationById(string orgId)
        {
            return new ServiceResult<OrganizationInfo>(OrganizationAdapter.GetOrganizationById(orgId));
        }

        [Summary(Description = "获取组织扩展列")]
        [AjaxMethod]
        public ServiceResult<Dictionary<string, string>> GetTableColumns()
        {
            return new ServiceResult<Dictionary<string, string>>(OrganizationAdapter.GetTableColumns());
        }

        [Summary(Description = "通过 Code 获取组织", Parameters = "orgCode:组织 Code")]
        [AjaxMethod]
        public ServiceResult<OrganizationInfo> GetOrganization(string orgCode)
        {
            return new ServiceResult<OrganizationInfo>(OrganizationAdapter.GetOrganization(orgCode));
        }

        [Summary(Description = "通过 Code 获取子级组织", Parameters = "orgCode:组织 Code")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetChildOrganizationList(string orgCode)
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetChildOrganizationList(orgCode));
        }

        [Summary(Description = "通过 Code 获取子级组织", Parameters = "orgCode:组织 Code")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetDeepChildOrganizationList(string orgCode)
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetDeepChildOrganizationList(orgCode));
        }

        [Summary(Description = "通过 Id 获取子级组织", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetChildOrganizationListById(string orgId)
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetChildOrganizationListById(orgId));
        }

        

        [Summary(Description = "通过 Id 获取子级组织", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetDeepChildOrganizationListById(string orgId)
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetDeepChildOrganizationListById(orgId));
        }

        [Summary(Description = "通过 Id 获取组织下用户", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public static ServiceResult<List<UserInfo>> GetUserListByOrgId(string orgId)
        {
            return new ServiceResult<List<UserInfo>>(OrganizationAdapter.GetUserListByOrgId(orgId));
        }

        [Summary(Description = "通过 Id 获取组织下用户", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public static ServiceResult<List<UserInfo>> GetDeepUserListByOrgId(string orgId)
        {
            return new ServiceResult<List<UserInfo>>(OrganizationAdapter.GetDeepUserListByOrgId(orgId));
        }

        [Summary(Description = "通过 Id 获取组织下所有职位", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public static ServiceResult<List<PositionInfo>> GetPositionListByOrgId(string orgId)
        {
            return new ServiceResult<List<PositionInfo>>(OrganizationAdapter.GetPositionListByOrgId(orgId));
        }

        [Summary(Description = "通过 Id 获取组织下所有职位", Parameters = "orgId:组织 Id")]
        [AjaxMethod]
        public static ServiceResult<List<PositionInfo>> GetDeepPositionListByOrgId(string orgId)
        {
            return new ServiceResult<List<PositionInfo>>(OrganizationAdapter.GetDeepPositionListByOrgId(orgId));
        }

        [Summary(Description = "在组织下面添加人员")]
        [AjaxMethod]
        public static ServiceResult<UserInfo> AddUser(string orgId, UserInfo user)
        {
            return new ServiceResult<UserInfo>(OrganizationAdapter.AddUser(orgId, user));
        }

        [Summary(Description = "在组织下面移除人员")]
        [AjaxMethod]
        public static ServiceResult<UserInfo> RemoveUser(string orgId, UserInfo user)
        {
            return new ServiceResult<UserInfo>(OrganizationAdapter.RemoveUser(orgId, user));
        }

        [Summary(Description = "保存组织机构")]
        [AjaxMethod]
        public static ServiceResult<object> Save(OrganizationInfo organizationInfo)
        {
            OrganizationAdapter.Save(organizationInfo);
            return new ServiceResult<object>();
        }

        [Summary(Description = "删除组织机构")]
        [AjaxMethod]
        public static ServiceResult<object> Delete(string organId)
        {
            OrganizationAdapter.Delete(organId);
            return new ServiceResult<object>();
        }

        [Summary(Description = "")]
        [AjaxMethod]
        public static ServiceResult<RoleInfo> GiveRole(string organId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(OrganizationAdapter.GiveRole(organId, roleInfo));
        }

        [Summary(Description = "")]
        [AjaxMethod]
        public static ServiceResult<RoleInfo> CancelRole(string organId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(OrganizationAdapter.CancelRole(organId, roleInfo));
        }
    }
}