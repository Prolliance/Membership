using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer.Models;
using System.Collections.Generic;

namespace Prolliance.Membership.DataTransfer
{
    public static class PositionAdapter
    {
        public static List<PositionInfo> GetPositionList()
        {
            return Position.GetPositionList().MappingToList<PositionInfo>();
        }

        public static List<PositionInfo> GetReportToListById(string positionId)
        {
            var position = Position.GetPositionById(positionId);
            if (position == null) return null;
            if (position.ReportToList == null) return null;
            return position.ReportToList.MappingToList<PositionInfo>();
        }

        public static PositionInfo GetPositionById(string positionId)
        {
            var position = Position.GetPositionById(positionId);
            if (position == null) return null;
            return position.MappingTo<PositionInfo>();
        }

        public static PositionInfo GetPosition(string organizationCode, string positionCode)
        {
            var position = Position.GetPosition(organizationCode, positionCode);
            if (position == null) return null;
            return position.MappingTo<PositionInfo>();
        }

        public static List<PositionInfo> ReportToList(string positionId)
        {
            var position = Position.GetPositionById(positionId);
            if (position == null) return null;
            return position.ReportToList.MappingToList<PositionInfo>();
        }

        public static PositionInfo RemoveReportTo(string positionId, string toPositionId)
        {
            var position = Position.GetPositionById(positionId);
            var toPosition = Position.GetPositionById(toPositionId);

            if (position == null || toPosition == null) return null;
            return position.RemoveReportTo(toPosition).MappingTo<PositionInfo>();
        }

        public static PositionInfo AddReportTo(string positionId, string toPositionId)
        {
            var position = Position.GetPositionById(positionId);
            var toPosition = Position.GetPositionById(toPositionId);

            if (position == null || toPosition == null) return null;
            return position.AddReportTo(toPosition).MappingTo<PositionInfo>();
        }

        public static List<UserInfo> GetUserList(string positionId)
        {
            var position = Position.GetPositionById(positionId);
            if (position == null) return null;
            if (position.UserList == null) return null;
            return position.UserList.MappingToList<UserInfo>();
        }

        public static UserInfo AddUser(string positionId, UserInfo userInfo)
        {
            Position position = Position.GetPositionById(positionId);
            if (position == null) return null;
            var user = position.AddUser(userInfo.MappingTo<User>());
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static UserInfo RemoveUser(string positionId, UserInfo userInfo)
        {
            Position position = Position.GetPositionById(positionId);
            if (position == null) return null;
            var user = position.RemoveUser(userInfo.MappingTo<User>());
            if (user == null) return null;
            return user.MappingTo<UserInfo>();
        }

        public static void Save(PositionInfo positionInfo)
        {
            var position = positionInfo.MappingTo<Position>(new List<string>() { "OrganizationCode" });
            position.Save();
        }

        public static void Delete(string positionId)
        {
            var position = Position.GetPositionById(positionId);
            position.Delete();
        }

        public static RoleInfo GiveRole(string positionId, RoleInfo roleInfo)
        {
            var position = Position.GetPositionById(positionId);
            var role = position.GiveRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }

        public static RoleInfo CancelRole(string positionId, RoleInfo roleInfo)
        {
            var position = Position.GetPositionById(positionId);
            var role = position.CancelRole(roleInfo.MappingTo<Role>());
            return role.MappingTo<RoleInfo>();
        }
    }
}
