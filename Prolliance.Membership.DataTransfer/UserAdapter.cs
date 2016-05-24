using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.DataTransfer
{
    public static class UserAdapter
    {
        public static UserPhotoInfo GetUserPhotoByAccount(string account)
        {
            var userPhoto = UserPhoto.GetUserPhotoByAccount(account);
            if (userPhoto == null) return null;
            return userPhoto.MappingTo<UserPhotoInfo>();
        }

        #region 针对当前会话状态用户的方法
        public static UserStateInfo Login(AuthParameterInfo loginInfo)
        {
            AuthParameter parameter = loginInfo.MappingTo<AuthParameter>();
            UserState state = User.CreateState(parameter);
            if (state == null) return null;
            return state.MappingTo<UserStateInfo>();
        }

        public static void Logout(string token)
        {
            User.CancelState(token);
        }

        public static UserStateInfo GetState(string token)
        {
            UserState state = User.GetState(token);
            return state.MappingTo<UserStateInfo>();
        }

        public static void ChangePassword(string token, string newPassword)
        {
            User user = User.GetUserByToken(token);
            if (user == null) return;
            user.SetPassword(newPassword);
        }

        public static UserInfo GetUser(string token)
        {
            User user = User.GetUserByToken(token);
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static Dictionary<string, string> GetTableColumns()
        {
            return User.GetTableColumns();
        }

        public static List<UserInfo> GetUserList(string token)
        {
            return User.GetUserList().MappingToList<UserInfo>();
        }

        public static List<UserInfo> GetUserListOfOrganization(string token)
        {
            return User.GetUserList()
                .Where(user => user.OrganizationList != null && user.OrganizationList.Count > 0)
                .ToList()
                .MappingToList<UserInfo>();
        }

        public static bool CheckPermission(string token, OperationInfo operation)
        {
            User user = User.GetUserByToken(token);
            if (user == null) return false;
            //必须查询出来，确保 Operation 存在
            Operation _operation = Operation.GetOperation(operation.AppKey, operation.TargetCode, operation.Code);
            if (_operation == null) return false;
            return user.CheckPermission(_operation);
        }

        public static List<TargetInfo> GetTargetList(string token)
        {
            User user = User.GetUserByToken(token);
            if (user == null) return null;
            return user.CalculatedDeapTargetList.MappingToList<TargetInfo>();//new List<string> { "OperationList" }
        }

        public static List<OperationInfo> GetOperationList(string token)
        {
            User user = User.GetUserByToken(token);
            if (user == null) return null;
            return user.DeepOperationList.MappingToList<OperationInfo>();
        }

        public static void CreateUser(UserInfo userInfo)
        {
            var user = userInfo.MappingTo<User>();
            user.Save();
        }

        public static void Save(UserInfo userInfo)
        {
            var user = userInfo.MappingTo<User>();
            user.Save();
        }

        public static void Delete(string userId)
        {
            var user = User.GetUserById(userId);
            user.Delete();
        }

        public static void LoadUserRemoteUser()
        {
            User.LoadUserRemoteUser();
        }
        #endregion

        #region 针对所有用户实例的方法
        public static List<RoleInfo> GetRoleListByUserId(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null) return null;
            return user.DeepRoleList.MappingToList<RoleInfo>();
        }

        public static List<PositionInfo> GetPositionListByUserId(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null) return null;
            return user.PositionList.MappingToList<PositionInfo>();
        }

        public static List<PositionInfo> GetReportToListByUserId(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null || user.ReportToList == null) return null;
            return user.ReportToList.MappingToList<PositionInfo>();
        }

        public static List<UserInfo> GetReportToUsers(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null || user.ReportToUserList == null) return null;
            return user.ReportToUserList.MappingToList<UserInfo>();
        } 

        public static List<OrganizationInfo> GetOrganizationListByUserId(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null) return null;
            return user.OrganizationList.MappingToList<OrganizationInfo>();
        }

        public static List<OrganizationInfo> GetDeepOrganizationListByUserId(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null) return null;
            return user.DeepOrganizationList.MappingToList<OrganizationInfo>();
        }
        public static UserInfo GetUserById(string userId)
        {
            User user = User.GetUserById(userId);
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }
        public static string ParseExprByUserId(string userId, string expr)
        {
            User user = User.GetUserById(userId);
            if (user == null) return string.Empty;
            return user.ParseExpr(expr);
        }
        public static UserInfo GetUserByAccount(string account)
        {
            User user = User.GetUser(account);
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static void ResetPassword(string account, string newPassword)
        {
            User user = User.GetUser(account);
            if (user == null) return;
            user.SetPassword(newPassword);
        }

        public static RoleInfo GiveRole(string userId, RoleInfo roleInfo)
        {
            var user = User.GetUserById(userId);
            var role = user.GiveRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }

        public static RoleInfo CancelRole(string userId, RoleInfo roleInfo)
        {
            var user = User.GetUserById(userId);
            var role = user.CancelRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }
        #endregion

    }
}
