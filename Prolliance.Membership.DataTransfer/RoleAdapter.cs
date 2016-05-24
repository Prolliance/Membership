using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolliance.Membership.DataTransfer
{
    public static class RoleAdapter
    {
        public static List<RoleInfo> GetRoleList()
        {
            List<Role> roleList = Role.GetRoleList();
            if (roleList == null) new List<RoleInfo>();
            return roleList.MappingToList<RoleInfo>();
        }

        /// <summary>
        /// 通过角色ID获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public static RoleInfo GetRoleById(string roleId)
        {
            var role = Role.GetRoleById(roleId);
            if(role == null)
            {
                return null;
            }
            return role.MappingTo<RoleInfo>();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>用户列表</returns>
        public static List<UserInfo> GetUserList(string roleId)
        {
            Role role = Role.GetRoleById(roleId);
            if (role == null) return new List<UserInfo>();
            List<User> userList = role.GetUserList();
            if (userList == null) return new List<UserInfo>();
            return userList.MappingToList<UserInfo>();
        }

        /// <summary>
        /// 获取组织列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>组织列表</returns>
        public static List<OrganizationInfo> GetOrganizationList(string roleId)
        {
            Role role = Role.GetRoleById(roleId);
            if (role == null) return new List<OrganizationInfo>();
            List<Organization> orgList = role.GetOrganizationList();
            if (orgList == null) return new List<OrganizationInfo>();
            return orgList.MappingToList<OrganizationInfo>();
        }

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static List<OperationInfo> GetOperationList(string roleId)
        {
            Role role = Role.GetRoleById(roleId);
            if (role == null) return new List<OperationInfo>();
            var operList = role.OperationList;
            if (operList == null) return new List<OperationInfo>();
            return operList.MappingToList<OperationInfo>();
        }

        /// <summary>
        /// 获取职位列表
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>职位列表</returns>
        public static List<PositionInfo> GetPositionList(string roleId)
        {
            Role role = Role.GetRoleById(roleId);
            if (role == null) return new List<PositionInfo>();
            List<Position> positionList = role.GetPositionList();
            if (positionList == null) return new List<PositionInfo>();
            return positionList.MappingToList<PositionInfo>();
        }

        /// <summary>
        /// 给角色授权
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="operationInfo">权限对象</param>
        public static void GivePermission(string roleId, OperationInfo operationInfo)
        {
            Role role = Role.GetRoleById(roleId);
            role.GivePermission(operationInfo.MappingTo<Operation>(new List<string>() { "AppKey", "TargetCode" }));
        }

        /// <summary>
        /// 取消角色授权
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="operationInfo">权限对象</param>
        public static void CancelPermission(string roleId,OperationInfo operationInfo)
        {
            Role role = Role.GetRoleById(roleId);
            role.CancelPermission(operationInfo.MappingTo<Operation>(new List<string>() { "AppKey", "TargetCode" }));
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleInfo">角色实体</param>
        public static void AddRole(RoleInfo roleInfo)
        {
            var role = Role.Create();
            role.Code = roleInfo.Code;
            role.Name = roleInfo.Name;
            role.Summary = roleInfo.Summary;
            role.IsActive = roleInfo.IsActive;
            role.Sort = roleInfo.Sort;
            role.Save();
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="roleInfo">角色实体</param>
        public static void UpdateRole(RoleInfo roleInfo)
        {
            var role = Role.GetRoleById(roleInfo.Id);
            role.Code = roleInfo.Code;
            role.Name = roleInfo.Name;
            role.Summary = roleInfo.Summary;
            role.IsActive = roleInfo.IsActive;
            role.Sort = roleInfo.Sort;
            role.Save();
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public static void DeleteRole(string roleId)
        {
            var role = Role.GetRoleById(roleId);
            role.Delete();
        }

        public static void AddUserToRole(string roleId,string userAccout)
        {
            var user = User.GetUser(userAccout);
            user.GiveRole(Role.GetRoleById(roleId));
            
        }
    }
}
