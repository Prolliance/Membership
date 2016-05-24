using System.Linq;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer.Models;
using System.Collections.Generic;

namespace Prolliance.Membership.DataTransfer
{
    public static class OrganizationAdapter
    {
        public static List<OrganizationInfo> GetAllOrganizationList()
        {
            return Organization.GetOrganizationList().MappingToList<OrganizationInfo>();
        }

        public static List<OrganizationInfo> GetRangeOrganization(string account, string orgCode)
        {
            var lstOrgInfo = new List<Organization>();
            var user = User.GetUser(account);
            if (user == null)
            {
                return new List<OrganizationInfo>();
            }
            var userOrgList = user.OrganizationList;

            var cmpOU = Organization.GetOrganization(orgCode);
            if (cmpOU == null)
            {
                return new List<OrganizationInfo>();
            }
            if (userOrgList.Any(p => p.Code == cmpOU.Code))
            {
                lstOrgInfo.Add(cmpOU);
            }
            lstOrgInfo.AddRange(cmpOU.DeepChildren.Where(organization => userOrgList.Any(p => p.Code == organization.Code)));
            return lstOrgInfo.MappingToList<OrganizationInfo>();
        } 

        public static List<OrganizationInfo> GetChildOrganizationListById(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null || org.Children == null) return null;
            return org.Children.MappingToList<OrganizationInfo>();
        }

        public static List<OrganizationInfo> GetChildOrganizationList(string orgCode)
        {
            Organization org = Organization.GetOrganization(orgCode);
            if (org == null || org.Children == null) return null;
            return org.Children.MappingToList<OrganizationInfo>();
        }

        public static List<OrganizationInfo> GetDeepChildOrganizationListById(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null || org.Children == null) return null;
            return org.DeepChildren.MappingToList<OrganizationInfo>();
        }

        public static List<OrganizationInfo> GetDeepChildOrganizationList(string orgCode)
        {
            Organization org = Organization.GetOrganization(orgCode);
            if (org == null || org.Children == null) return null;
            return org.DeepChildren.MappingToList<OrganizationInfo>();
        }

        public static OrganizationInfo GetOrganizationById(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            return org.MappingTo<OrganizationInfo>();
        }

        public static Dictionary<string, string> GetTableColumns()
        {
            return Organization.GetTableColumns();
        }

        public static OrganizationInfo GetOrganization(string orgCode)
        {
            Organization org = Organization.GetOrganization(orgCode);
            if (org == null) return null;
            return org.MappingTo<OrganizationInfo>();
        }

        public static List<UserInfo> GetUserListByOrgId(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            return org.UserList.MappingToList<UserInfo>();
        }

        public static List<UserInfo> GetDeepUserListByOrgId(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            return org.DeepUserList.MappingToList<UserInfo>();
        }

        public static List<PositionInfo> GetPositionListByOrgId(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            return org.PositionList.MappingToList<PositionInfo>();
        }
        public static List<PositionInfo> GetDeepPositionListByOrgId(string orgId)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            return org.DeepPositionList.MappingToList<PositionInfo>();
        }

        public static UserInfo AddUser(string orgId, UserInfo userInfo)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            var user = org.AddUser(userInfo.MappingTo<User>());
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static UserInfo RemoveUser(string orgId, UserInfo userInfo)
        {
            Organization org = Organization.GetOrganizationById(orgId);
            if (org == null) return null;
            var user = org.RemoveUser(userInfo.MappingTo<User>());
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static void Save(OrganizationInfo organizationInfo)
        {
            var organization = organizationInfo.MappingTo<Organization>();
            organization.Save();
        }

        public static void Delete(string organid)
        {
            var organization = Organization.GetOrganizationById(organid);
            organization.Delete();
        }

        public static RoleInfo GiveRole(string organId, RoleInfo roleInfo)
        {
            var organization = Organization.GetOrganizationById(organId);
            var role = organization.GiveRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }

        public static RoleInfo CancelRole(string organId, RoleInfo roleInfo)
        {
            var organization = Organization.GetOrganizationById(organId);
            var role = organization.CancelRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }
    }
}
