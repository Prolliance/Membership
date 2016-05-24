using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class Position : ModelBase
    {
        internal static DataRepo<PositionInfo> PositionInfoRepo = new DataRepo<PositionInfo>();
        internal static DataRepo<PositionReportToInfo> PositionReportToInfoRepo = new DataRepo<PositionReportToInfo>();
        internal static DataRepo<PositionUserInfo> PositionUserInfoRepo = new DataRepo<PositionUserInfo>();
        internal static DataRepo<UserInfo> UserInfoRepo = new DataRepo<UserInfo>();
        internal static DataRepo<RoleInfo> RoleInfoRepo = new DataRepo<RoleInfo>();
        internal static DataRepo<RolePositionInfo> RolePositionInfoRepo = new DataRepo<RolePositionInfo>();

        #region 属性
        /// <summary>
        /// 组织Code
        /// </summary>
        public string OrganizationId { get; set; }
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        public string Type { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public string OrganizationCode
        {
            get
            {
                Organization org = this.GetOrganization();
                if (org != null)
                {
                    return org.Code;
                }
                return "";
            }
        }
        #endregion

        #region 静态方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public static Position Create()
        {
            Position position = new Position();
            return position;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        public static List<Position> GetPositionList()
        {
            return PositionInfoRepo.Read()
                .OrderByDescending(p => p.Sort)
                .MappingToList<Position>();
        }

        public static Position GetPosition(string organizationCode, string positionCode)
        {
            Organization org = Organization.GetOrganization(organizationCode);
            if (org == null)
                return null;
            return GetPositionList()
                .FirstOrDefault(p => p.OrganizationId == org.Id
                    && p.Code == positionCode);
        }

        public static Position GetPositionById(string positionId)
        {
            return GetPositionList().FirstOrDefault(p => p.Id == positionId);
        }

        #endregion

        #region 实列方法
        /// <summary>
        /// 新增
        /// </summary>
        public void Save()
        {
            PositionInfo positionInfo = PositionInfoRepo.Read()
                .FirstOrDefault(po => po.OrganizationId == this.OrganizationId
                    && po.Code == this.Code);
            if (positionInfo != null && positionInfo.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            PositionInfoRepo.Save(this.MappingTo<PositionInfo>());
        }

        /// <summary>
        /// 移除
        /// </summary>
        public void Delete()
        {
            PositionInfoRepo.Delete(this.MappingTo<PositionInfo>());
            //移除岗位用户关系
            PositionUserInfoRepo.Read()
                .Where(pu => pu.PositionId == this.Id)
                .ToList()
                .ForEach(pu => PositionUserInfoRepo.Delete(pu));
        }
        #endregion

        #region 汇报关系
        List<Position> _ReportToList;
        /// <summary>
        /// 根据汇报类型查询岗位上级
        /// </summary>
        /// <returns></returns>
        public List<Position> ReportToList
        {
            get
            {
                if (_ReportToList == null)
                {
                    var result = from position in PositionInfoRepo.Read()
                                 from prt in PositionReportToInfoRepo.Read()
                                 where prt.PositionId == this.Id
                                        && prt.HigherOrganizationId == position.OrganizationId
                                        && prt.HigherPositionId == position.Id
                                        && position.IsActive
                                 orderby position.Sort descending
                                 select position;
                    _ReportToList = result.MappingToList<Position>();
                }
                return _ReportToList;
            }
        }

        /// <summary>
        /// 新增汇报上级
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position AddReportTo(Position position)
        {
            if (!PositionInfoRepo.Exists(position.MappingTo<PositionInfo>())
                || !PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            PositionReportToInfo reportToInfo = new PositionReportToInfo();
            reportToInfo.OrganizationId = this.OrganizationId;
            reportToInfo.PositionId = this.Id;
            reportToInfo.HigherOrganizationId = position.OrganizationId;
            reportToInfo.HigherPositionId = position.Id;
            PositionReportToInfoRepo.Save(reportToInfo);
            return position;
        }

        /// <summary>
        /// 移除汇报上级
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Position RemoveReportTo(Position position)
        {
            if (!PositionInfoRepo.Exists(position.MappingTo<PositionInfo>())
                || !PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            PositionReportToInfo reportToInfo =
                PositionReportToInfoRepo.Read()
                    .FirstOrDefault(
                        p =>
                            p.PositionId == this.Id && p.HigherOrganizationId == position.OrganizationId &&
                            p.HigherPositionId == position.Id
                            && p.OrganizationId == this.OrganizationId);
            //reportToInfo.OrganizationId = this.OrganizationId;
            //reportToInfo.PositionId = this.Id;
            //reportToInfo.HigherOrganizationId = position.OrganizationId;
            //reportToInfo.HigherPositionId = position.Id;
            PositionReportToInfoRepo.Delete(reportToInfo);
            return position;
        }

        #endregion

        #region 部门相关
        Organization _Organization;
        public Organization GetOrganization()
        {
            if (_Organization == null)
            {
                _Organization = Organization.GetOrganizationById(this.OrganizationId);
            }
            return _Organization;
        }
        #endregion

        #region 用户相关
        List<User> _UserList;
        public List<User> UserList
        {
            get
            {
                if (_UserList == null)
                {
                    var userInfoList = UserInfoRepo.Read();
                    var positionUserInfoList = PositionUserInfoRepo.Read();
                    var list = from user in userInfoList
                               from positionUser in positionUserInfoList
                               where positionUser.OrganizationId == this.OrganizationId
                                    && positionUser.PositionId == this.Id
                                    && positionUser.UserId == user.Id
                                    && user.IsActive
                               orderby positionUser.Sort descending
                               select user;
                    _UserList = list.ToList().MappingToList<User>();
                }
                return _UserList;
            }
        }
        public User AddUser(User user)
        {
            if (!PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            if (!User.UserInfoRepo.Exists(user.MappingTo<UserInfo>()))
            {
                throw new Exception("用户不存在");
            }
            PositionUserInfo pu = new PositionUserInfo();
            pu.UserId = user.Id;
            pu.OrganizationId = this.OrganizationId;
            pu.PositionId = this.Id;
            PositionUserInfoRepo.Save(pu);
            return user;
        }
        public User RemoveUser(User user)
        {
            if (!PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            List<PositionUserInfo> piList = PositionUserInfoRepo.Read()
                .Where(pu => pu.UserId == user.Id
                && pu.OrganizationId == this.OrganizationId
                && pu.PositionId == this.Id)
                .ToList();
            foreach (var pi in piList)
            {
                PositionUserInfoRepo.Delete(pi);
            }
            return user;
        }
        #endregion

        #region 角色相关
        List<Role> _RoleList;
        public List<Role> RoleList
        {
            get
            {
                if (_RoleList == null)
                {
                    var rolelist = from role in RoleInfoRepo.Read()
                                   from rolePosition in RolePositionInfoRepo.Read()
                                   where rolePosition.OrganizationId == this.OrganizationId
                                         && rolePosition.PositionId == this.Id
                                         && rolePosition.RoleId == role.Id
                                   orderby role.Sort descending
                                   select role;
                    _RoleList = rolelist.ToList().MappingToList<Role>();
                }
                return _RoleList;
            }
        }

        List<Role> _DeepRoleList;
        public List<Role> DeepRoleList
        {
            get
            {
                if (_DeepRoleList == null)
                {
                    List<Role> roleList = this.RoleList;
                    Organization org = this.GetOrganization();
                    if (org != null)
                    {
                        roleList.AddRange(org.DeepRoleList);
                    }
                    _DeepRoleList = roleList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _DeepRoleList;
            }
        }
        public Role GiveRole(Role role)
        {
            if (!PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            if (!Role.RoleInfoRepo.Exists(role.MappingTo<RoleInfo>()))
            {
                throw new Exception("角色不存在");
            }
            if (RolePositionInfoRepo.Read().Exists(ro => ro.RoleId == role.Id
                && ro.OrganizationId == this.OrganizationId
                && ro.PositionId == this.Id))
            {
                return role;
            }

            //检查互斥,除检查自身外，需检查所有上级组件层叠过来的角色（DeepRoleList 包括上级组件的角色）
            List<Role> linkRoleList = this.DeepRoleList;
            foreach (Role linkRole in linkRoleList)
            {
                if (RoleMutex.CheckMutex(role, linkRole, RoleMutexType.Static))
                {
                    throw new Exception("授予的角色和当前岗位或其所在组织的角色存在静态互斥关系");
                }
            }

            RolePositionInfo rolePositionInfo = new RolePositionInfo();
            rolePositionInfo.RoleId = role.Id;
            rolePositionInfo.OrganizationId = this.OrganizationId;
            rolePositionInfo.PositionId = this.Id;
            RolePositionInfoRepo.Add(rolePositionInfo);
            return role;
        }
        public Role CancelRole(Role role)
        {
            if (!PositionInfoRepo.Exists(this.MappingTo<PositionInfo>()))
            {
                throw new Exception("岗位不存在");
            }
            RolePositionInfo rolePositionInfo = new RolePositionInfo();
            rolePositionInfo.RoleId = role.Id;
            rolePositionInfo.OrganizationId = this.OrganizationId;
            rolePositionInfo.PositionId = this.Id;
            RolePositionInfoRepo.Delete(rolePositionInfo);
            return role;
        }
        #endregion

        #region 权限操作
        List<Operation> _OperationList;
        public List<Operation> OperationList
        {
            get
            {
                if (_OperationList == null)
                {
                    List<Operation> operationList = new List<Operation>();
                    List<Role> roleList = this.RoleList;
                    foreach (var role in roleList)
                    {
                        operationList.AddRange(role.OperationList);
                    }
                    _OperationList = operationList;
                }
                return _OperationList;
            }
        }

        List<Operation> _DeepOperationList;
        public List<Operation> DeepOperationList
        {
            get
            {
                if (_DeepOperationList == null)
                {
                    List<Operation> operationList = new List<Operation>();
                    List<Role> roleList = this.DeepRoleList;
                    foreach (var role in roleList)
                    {
                        operationList.AddRange(role.OperationList);
                    }
                    _DeepOperationList = operationList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _DeepOperationList;
            }
        }
        #endregion

        #region 权限对象
        List<Target> _TargetList;
        public List<Target> TargetList
        {
            get
            {
                if (_TargetList == null)
                {
                    List<Target> targetList = new List<Target>();
                    List<Role> roleList = this.RoleList;
                    foreach (var role in roleList)
                    {
                        targetList.AddRange(role.TargetList);
                    }
                    _TargetList = targetList;
                }
                return _TargetList;
            }
        }

        List<Target> _DeapTargetList;
        public List<Target> DeapTargetList
        {
            get
            {
                if (_DeapTargetList == null)
                {
                    List<Target> targetList = new List<Target>();
                    List<Role> roleList = this.DeepRoleList;
                    foreach (var role in roleList)
                    {
                        targetList.AddRange(role.TargetList);
                    }
                    _DeapTargetList = targetList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _DeapTargetList;
            }
        }
        #endregion

    }
}