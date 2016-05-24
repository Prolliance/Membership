using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 角色
    /// </summary> 
    public class Role : ModelBase
    {
        internal static DataRepo<RoleInfo> RoleInfoRepo = new DataRepo<RoleInfo>();
        internal static DataRepo<RoleOperationInfo> RoleOperationInfoRepo = new DataRepo<RoleOperationInfo>();
        internal static DataRepo<TargetInfo> TargetInfoRepo = new DataRepo<TargetInfo>();
        internal static DataRepo<RoleOrganizationInfo> RoleOrganizationInfoRepo = new DataRepo<RoleOrganizationInfo>();
        internal static DataRepo<RoleUserInfo> RoleUserInfoRepo = new DataRepo<RoleUserInfo>();
        internal static DataRepo<RolePositionInfo> RolePositionInfoRepo = new DataRepo<RolePositionInfo>();
        internal static DataRepo<OperationInfo> OperationInfoRepo = new DataRepo<OperationInfo>();

        #region 属性
        /// <summary>
        /// 主键
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        #endregion

        #region  静态方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public static Role Create()
        {
            Role role = new Role();
            return role;
        }

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <returns></returns>
        public static List<Role> GetRoleList()
        {
            return RoleInfoRepo.Read()
                .ToList()
                .OrderByDescending(role => role.Sort)
                .MappingToList<Role>();
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Role GetRole(string code)
        {
            return RoleInfoRepo.Read()
                .FirstOrDefault(role => role.Code == code)
                .MappingTo<Role>();
        }
        public static Role GetRoleById(string id)
        {
            return RoleInfoRepo.Read()
                .FirstOrDefault(role => role.Id == id)
                .MappingTo<Role>();
        }
        #endregion

        #region 普通实列方法
        /// <summary>
        /// 新增
        /// </summary>
        public void Save()
        {
            Role role = GetRole(this.Code);
            if (role != null && role.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            RoleInfoRepo.Save(this.MappingTo<RoleInfo>());
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Delete()
        {
            RoleInfo roleInfo = Mapper.Clone<RoleInfo>(this);
            RoleInfoRepo.Delete(roleInfo);
            //移除角色部门关系
            RoleOrganizationInfoRepo.Read()
                .Where(ro => ro.RoleId == this.Id)
                .ToList()
                .ForEach(ro => RoleOrganizationInfoRepo.Delete(ro));
            //移除角色用户关系
            RoleUserInfoRepo.Read()
                .Where(ru => ru.RoleId == this.Id)
                .ToList()
                .ForEach(ru => RoleUserInfoRepo.Delete(ru));
            //移除角色岗位关系
            RolePositionInfoRepo.Read()
                .Where(rp => rp.RoleId == this.Id)
                .ToList()
                .ForEach(rp => RolePositionInfoRepo.Delete(rp));
            //移除角色权限操作关系
            RoleOperationInfoRepo.Read()
                .Where(ro => ro.RoleId == this.Id)
                .ToList()
                .ForEach(rto => RoleOperationInfoRepo.Delete(rto));
        }
        #endregion

        #region 权限相关方法
        /// <summary>
        /// 授予权限
        /// </summary>
        /// <param name="operation">操作</param>
        /// <returns>操作</returns>
        public Operation GivePermission(Operation operation)
        {
            RoleOperationInfo info = new RoleOperationInfo();
            info.RoleId = this.Id;
            info.AppId = operation.AppId;
            info.TargetId = operation.TargetId;
            info.OperationId = operation.Id;
            RoleOperationInfoRepo.Save(info);
            return operation;
        }

        /// <summary>
        /// 取消权限
        /// </summary>
        /// <param name="operation">操作</param>
        /// <returns>操作</returns>
        public Operation CancelPermission(Operation operation)
        {
            RoleOperationInfo info = new RoleOperationInfo();
            info.RoleId = this.Id;
            info.AppId = operation.AppId;
            info.TargetId = operation.TargetId;
            info.OperationId = operation.Id;
            RoleOperationInfoRepo.Delete(info);
            return operation;
        }
        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="operation">操作</param>
        /// <returns>状态</returns>
        public bool CheckPermission(Operation operation)
        {
            if (!OperationInfoRepo.Exists(operation.MappingTo<OperationInfo>()))
            {
                throw new Exception("对象不存在");
            }
            return RoleOperationInfoRepo.Read()
                .Exists(ro =>
                 ro.RoleId == this.Id
                 && ro.AppId == operation.AppId
                 && ro.TargetId == operation.TargetId
                 && ro.OperationId == operation.Id);
        }

        List<Operation> _OperationList;
        /// <summary>
        /// 操作列表
        /// </summary>
        public List<Operation> OperationList
        {
            get
            {
                if (_OperationList == null)
                {
                    List<RoleOperationInfo> roList = RoleOperationInfoRepo.Read();
                    List<OperationInfo> opList = OperationInfoRepo.Read();
                    var operationInfoList = from opInfo in opList
                                            from roInfo in roList
                                            where roInfo.RoleId == this.Id
                                                && opInfo.Id == roInfo.OperationId
                                            select opInfo;
                    _OperationList = operationInfoList.MappingToList<Operation>();
                }
                return _OperationList;
            }
        }

        List<Target> _TargetList;
        /// <summary>
        /// 对象列表
        /// </summary>
        public List<Target> TargetList
        {
            get
            {
                if (_TargetList == null)
                {
                    var roList = RoleOperationInfoRepo.Read();
                    var targeInfotList = TargetInfoRepo.Read();
                    var roleTargetList = from target in targeInfotList
                                         from ro in roList
                                         where ro.RoleId == this.Id
                                         && target.AppId == ro.AppId
                                         && target.Id == ro.TargetId
                                         select target;
                    List<Target> targetList = roleTargetList.MappingToList<Target>();
                    _TargetList = targetList;
                }
                return _TargetList;
            }
        }

        List<User> _UserList;
        /// <summary>
        /// 获取属于这个角色的所有用户
        /// </summary>
        /// <returns>用户列表</returns>
        public List<User> GetUserList()
        {
            if (_UserList == null)
            {
                var userList = User.GetUserList();
                if (userList != null && userList.Count > 0)
                {
                    _UserList = userList
                        .Where(user => user
                            .RoleList
                            .Exists(role => role.Id == this.Id))
                        .ToList();
                    return _UserList;
                }
                _UserList = new List<User>();
            }
            return _UserList;
        }

        List<Organization> _OrganizationList;
        /// <summary>
        /// 获取属于这个角色的所有组织
        /// </summary>
        /// <returns>组织列表</returns>
        public List<Organization> GetOrganizationList()
        {
            if (_OrganizationList == null)
            {
                var orgList = Organization.GetOrganizationList();
                if (orgList != null && orgList.Count > 0)
                {
                    _OrganizationList = orgList
                        .Where(org => org
                            .RoleList
                            .Exists(role => role.Id == this.Id))
                        .ToList();
                }
                else { _OrganizationList = new List<Organization>(); }
                
            }
            return _OrganizationList;
        }

        List<Position> _PositionList;
        /// <summary>
        /// 获取属于这个角色的所有职位
        /// </summary>
        /// <returns>职位列表</returns>
        public List<Position> GetPositionList()
        {
            if (_PositionList == null)
            {
                var positionList = Position.GetPositionList();
                if (positionList != null && positionList.Count > 0)
                {
                    _PositionList = positionList
                        .Where(position => position
                            .RoleList
                            .Exists(role => role.Id == this.Id))
                        .ToList();
                    return _PositionList;
                }
                _PositionList = new List<Position>();
            }
            return _PositionList;
        }

        #endregion
    }
}
