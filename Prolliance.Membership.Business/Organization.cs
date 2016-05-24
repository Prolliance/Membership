using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 组织 
    /// </summary>
    public class Organization : ModelBase
    {
        internal static DataRepo<OrganizationInfo> OrganizationInfoRepo = new DataRepo<OrganizationInfo>();
        internal static DataRepo<UserInfo> UserInfoRepo = new DataRepo<UserInfo>();
        internal static DataRepo<PositionInfo> PositionInfoRepo = new DataRepo<PositionInfo>();
        internal static DataRepo<PositionUserInfo> PositionUserInfoRepo = new DataRepo<PositionUserInfo>();
        internal static DataRepo<RoleOrganizationInfo> RoleOrganizationInfoRepo = new DataRepo<RoleOrganizationInfo>();
        internal static DataRepo<RoleInfo> RoleInfoRepo = new DataRepo<RoleInfo>();
        internal static DataRepo<RoleMutexInfo> RoleMutexRepo = new DataRepo<RoleMutexInfo>();

        #region 普通属性
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public string ParentId
        {
            internal set;
            get;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public Dictionary<string, object> Extensions { get; set; }  

        #endregion

        #region 静态方法
        /// <summary>
        /// 构造函数
        /// </summary>
        public static Organization Create()
        {
            Organization organization = new Organization();
            return organization;
        }

        /// <summary>
        /// 获得所有组织机构
        /// </summary>
        /// <returns></returns>
        public static List<Organization> GetOrganizationList()
        {
            return OrganizationInfoRepo.Read()
                .OrderByDescending(org => org.Sort)
                .ToList()
                .MappingToList<Organization>();
        }

        /// <summary>
        /// 获得一个组织
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static Organization GetOrganization(string code)
        {
            OrganizationInfo orgInfo = OrganizationInfoRepo
                .Read()
                .FirstOrDefault(org => org.Code == code);
            return orgInfo.MappingTo<Organization>();
        }
        public static Organization GetOrganizationById(string id)
        {
            OrganizationInfo orgInfo = OrganizationInfoRepo
                .Read()
                .FirstOrDefault(org => org.Id == id);
            return orgInfo.MappingTo<Organization>();
        }
        public static Dictionary<string, string> GetTableColumns()
        {
            return OrganizationInfoRepo.GetTableStruct("Organization");
        }

        public static string AddExtensionField(ExtensionField field)
        {
            return OrganizationInfoRepo.AddField(field);
        }
        #endregion

        #region 普通实例方法
        //保存
        public void Save()
        {
            var orgInfo = OrganizationInfoRepo
                .Read()
                .FirstOrDefault(org => org.Code == this.Code);
            if (orgInfo != null && orgInfo.Id != this.Id)
            {
                throw new Exception(string.Format("保存‘{0}’对象时，发现唯一键冲突", this.GetType().Name));
            }
            OrganizationInfoRepo.Save(this.MappingTo<OrganizationInfo>());
        }
        /// <summary>
        /// 删除
        /// </summary>
        public void Delete()
        {
            OrganizationInfoRepo.Delete(this.MappingTo<OrganizationInfo>());

            //移除角色组织关系
            List<RoleOrganizationInfo> roList = RoleOrganizationInfoRepo
                .Read()
                .Where(ro => ro.OrganizationId == this.Id)
                .ToList();
            foreach (RoleOrganizationInfo ro in roList)
            {
                RoleOrganizationInfoRepo.Delete(ro);
            }
            //移除组织下的岗位
            List<PositionInfo> poList = PositionInfoRepo
                .Read()
                .Where(po => po.OrganizationId == this.Id)
                .ToList();
            foreach (PositionInfo po in poList)
            {
                PositionInfoRepo.Delete(po);
            }
            //移除组织人员关系
            List<PositionUserInfo> puList = PositionUserInfoRepo
                .Read()
                .Where(pu => pu.OrganizationId == this.Id)
                .ToList();
            foreach (PositionUserInfo pu in puList)
            {
                PositionUserInfoRepo.Delete(pu);
            }
        }
        #endregion

        #region 部门相关
        /// <summary>
        /// 新增子节点
        /// </summary>
        /// <param name="organization"></param>
        public Organization AddChild(Organization organization)
        {
            //设置上级id
            if (organization.ParentId != this.Id)
            {
                //检查互斥
                List<Role> roleList1 = this.LinkRoleList;
                List<Role> roleList2 = organization.LinkRoleList;
                foreach (Role role1 in roleList1)
                {
                    foreach (Role role2 in roleList1)
                    {
                        if (RoleMutex.CheckMutex(role1, role2, RoleMutexType.Static))
                        {
                            throw new Exception("当前组织或其父级或其子级的角色存在静态互斥关系");
                        }
                    }
                }
                organization.ParentId = this.Id;
                organization.Save();
            }
            return organization;
        }
        public Organization RemoveChild(Organization organization)
        {
            if (organization.ParentId == this.Id)
            {
                organization.Delete();
            }
            return organization;
        }

        Organization _Parent;
        /// <summary>
        /// 上级部门
        /// </summary>
        public Organization GetParent()
        {
            if (_Parent == null)
            {
                _Parent = GetOrganizationById(this.ParentId);
            }
            return _Parent;
        }
        public Organization SetParent(Organization parent)
        {
            parent.AddChild(this);
            _Parent = parent;
            return parent;
        }
        private List<Organization> GetDeepParentList(Organization organization)
        {
            List<Organization> organizationList = new List<Organization>();
            if (organization != null && organization.GetParent() != null)
            {
                organizationList.AddRange(GetDeepParentList(organization.GetParent()));
                organizationList.Add(organization.GetParent());
            }
            return organizationList;
        }

        List<Organization> _DeepParentList;
        public List<Organization> DeepParentList
        {
            get
            {
                if (_DeepParentList == null)
                {
                    _DeepParentList = this.GetDeepParentList(this)
                        .DistinctBy(item => item.Id)
                        .OrderByDescending(org => org.Sort)
                        .ToList();
                }
                return _DeepParentList;
            }
        }

        List<Organization> _Children;
        /// <summary>
        /// 子部门
        /// </summary>
        public List<Organization> Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = OrganizationInfoRepo.Read()
                        .Where(org => org.ParentId == this.Id)
                        .OrderByDescending(org => org.Sort)
                        .ToList()
                        .MappingToList<Organization>();
                }
                return _Children;
            }
        }

        /// <summary>
        /// 获得当前组织子节点
        /// </summary>
        /// <param name="orgCode"></param>
        /// <returns></returns>
        private List<Organization> GetDeepChildren(Organization organization)
        {
            //返回数据
            List<Organization> list = new List<Organization>();
            //获得下级部门
            List<Organization> childrenOrganization = organization.Children;
            if (childrenOrganization != null)
            {
                foreach (Organization childOrganization in childrenOrganization)
                {
                    list.Add(childOrganization);
                    list.AddRange(GetDeepChildren(childOrganization));
                }
            }
            return list;
        }

        List<Organization> _DeepChildren;
        /// <summary>
        /// 获得所有下级部门
        /// </summary>
        public List<Organization> DeepChildren
        {
            get
            {
                if (_DeepChildren == null)
                {
                    _DeepChildren = this.GetDeepChildren(this)
                            .DistinctBy(item => item.Id)
                            .OrderByDescending(org => org.Sort)
                            .ToList();
                }
                return _DeepChildren;
            }
        }

        #endregion

        #region 岗位相关
        /// <summary>
        /// 新增岗位
        /// </summary>
        /// <param name="positionId"></param>
        public Position AddPosition(Position position)
        {
            position.OrganizationId = this.Id;
            position.Save();
            return position;
        }
        /// <summary>
        /// 删除岗位
        /// </summary>
        /// <param name="position"></param>
        public Position RemovePosition(Position position)
        {
            position.OrganizationId = this.Id;
            position.Delete();
            return position;
        }

        List<Position> _PositionList;
        /// <summary>
        /// 岗位列表
        /// </summary>
        public List<Position> PositionList
        {
            get
            {
                if (_PositionList == null)
                {
                    _PositionList = PositionInfoRepo.Read()
                        .Where(p => p.OrganizationId == this.Id)
                        .ToList()
                        .OrderByDescending(p => p.Sort)
                        .MappingToList<Position>();
                }
                return _PositionList;
            }
        }

        List<Position> _DeepPositionList;
        public List<Position> DeepPositionList
        {
            get
            {
                if (_DeepPositionList == null)
                {
                    List<Position> positionList = this.PositionList;
                    List<Organization> children = this.DeepChildren;
                    foreach (Organization child in children)
                    {
                        positionList.AddRange(child.PositionList);
                    }
                    _DeepPositionList = positionList
                        .DistinctBy(item => item.Id)
                        .OrderByDescending(p => p.Sort)
                        .ToList();
                }
                return _DeepPositionList;
            }
        }
        #endregion

        #region 用户相关
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        public User AddUser(User user)
        {
            if (user == null) return user;
            var positionUserList = PositionUserInfoRepo.Read();
            if (!positionUserList.Exists(pu => pu.OrganizationId == this.Code && pu.UserId == user.Account))
            {
                PositionUserInfo positionUserInfo = new PositionUserInfo();
                positionUserInfo.PositionId = "";
                positionUserInfo.UserId = user.Id;
                positionUserInfo.OrganizationId = this.Id;
                PositionUserInfoRepo.Save(positionUserInfo);
            }
            return user;
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="user"></param>
        public User RemoveUser(User user)
        {
            var positionUserList = PositionUserInfoRepo.Read().Where(pu => pu.OrganizationId == this.Id && pu.UserId == user.Id);
            foreach (var item in positionUserList)
            {
                PositionUserInfoRepo.Delete(item);
            }
            return user;
        }

        List<User> _UserList;
        /// <summary>
        /// 组织下人员列表
        /// </summary>
        public List<User> UserList
        {
            get
            {
                if (_UserList == null)
                {
                    var userInfoList = UserInfoRepo.Read();
                    var positionUserInfoList = PositionUserInfoRepo.Read().Where(p=>p.OrganizationId==this.Id).ToList();
                    var list = from user in userInfoList
                               from positionUser in positionUserInfoList
                               where  positionUser.UserId == user.Id
                                    && user.IsActive
                               orderby positionUser.Sort descending
                               select user;
                    _UserList = list
                        .DistinctBy(item => item.Id)
                        .ToList()
                        .MappingToList<User>();
                }
                return _UserList;
            }
        }

        /// <summary>
        /// 对用户进行排序
        /// </summary>
        /// <param name="list"></param>
        public void SortUser(List<string> list)
        {
            var positionUserInfoList = PositionUserInfoRepo.Read()
                .Where(pu => pu.OrganizationId == this.Id
                        && list.Contains(pu.UserId))
                .ToList();
            var sortIndex = list.Count();
            foreach (var userId in list)
            {
                var positionUserInfo = positionUserInfoList
                    .FirstOrDefault(pu => pu.UserId == userId);
                positionUserInfo.Sort = sortIndex;
                PositionUserInfoRepo.Save(positionUserInfo);
                sortIndex--;
            }
            _UserList = null;
        }

        List<User> _DeepUserList;
        /// <summary>
        /// 这个列表使用全局排序
        /// </summary>
        public List<User> DeepUserList
        {
            get
            {
                if (_DeepUserList == null)
                {
                    List<User> userList = this.UserList;
                    List<Organization> children = this.DeepChildren;
                    foreach (Organization child in children)
                    {
                        userList.AddRange(child.UserList);
                    }
                    _DeepUserList = userList
                        .DistinctBy(item => item.Id)
                        .OrderByDescending(user => user.Sort)
                        .ToList();
                }
                return _DeepUserList;
            }
        }

        #endregion

        #region 角色相关
        /// <summary>
        /// 授予角色
        /// </summary>
        /// <param name="role"></param>
        public Role GiveRole(Role role)
        {
            if (!Role.GetRoleList().Exists(r => r.Id == role.Id))
            {
                throw new Exception("授予的角色不存在");
            }
            if (RoleOrganizationInfoRepo.Read().Exists(ro => ro.RoleId == role.Id && ro.OrganizationId == this.Id))
            {
                return role;
            }

            //检查互斥
            List<Role> linkRoleList = this.LinkRoleList;
            foreach (Role linkRole in linkRoleList)
            {
                if (RoleMutex.CheckMutex(role, linkRole, RoleMutexType.Static))
                {
                    throw new Exception("授予的角色和当前组织或其父级或其子级的角色存在静态互斥关系");
                }
            }

            RoleOrganizationInfo roleOrganizationInfo = new RoleOrganizationInfo();
            roleOrganizationInfo.RoleId = role.Id;
            roleOrganizationInfo.OrganizationId = this.Id;
            RoleOrganizationInfoRepo.Save(roleOrganizationInfo);
            return role;
        }

        /// <summary>
        /// 取消角色
        /// </summary>
        /// <param name="role"></param>
        public Role CancelRole(Role role)
        {
            RoleOrganizationInfo roleOrganizationInfo = new RoleOrganizationInfo();
            roleOrganizationInfo.RoleId = role.Id;
            roleOrganizationInfo.OrganizationId = this.Id;
            RoleOrganizationInfoRepo.Delete(roleOrganizationInfo);
            return role;
        }

        List<Role> _RoleList;
        /// <summary>
        /// 角色列表
        /// </summary>
        public List<Role> RoleList
        {
            get
            {
                if (_RoleList == null)
                {
                    var rolelist = from role in RoleInfoRepo.Read()
                                   from roleOrganization in RoleOrganizationInfoRepo.Read()
                                   where roleOrganization.OrganizationId == this.Id
                                        && roleOrganization.RoleId == role.Id
                                   select role;
                    _RoleList = rolelist
                        .ToList()
                        .OrderByDescending(role => role.Sort)
                        .ToList()
                        .MappingToList<Role>();
                }
                return _RoleList;
            }
        }

        List<Role> _DeepRoleList;
        /// <summary>
        /// 所有角色列表
        /// </summary>
        public List<Role> DeepRoleList
        {
            get
            {
                if (_DeepRoleList == null)
                {
                    List<Role> roleList = this.RoleList;
                    List<Organization> orgList = new List<Organization>();
                    orgList.AddRange(this.DeepParentList);
                    foreach (Organization org in orgList)
                    {
                        roleList.AddRange(org.RoleList);
                    }
                    _DeepRoleList = roleList
                        .OrderByDescending(r => r.Sort)
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _DeepRoleList;
            }
        }

        List<Role> _LinkRoleList;
        /// <summary>
        /// 向上及向下和这个组织相关的角色列表（用来检查互斥）
        /// </summary>
        internal List<Role> LinkRoleList
        {
            get
            {
                if (_LinkRoleList == null)
                {
                    List<Role> roleList = this.RoleList;
                    List<Organization> orgList = new List<Organization>();
                    /*
                    【当前组织】所有的【上级组织的角色】会层叠到自身。
                     所以检查互斥关系时除【自身角色】需向上找到所有【上级组织角色】
                    */
                    orgList.AddRange(this.DeepChildren);
                    /*
                    【当前组织】的【自身角色】会层叠到【所有下级组织】。
                     所以检查互斥关系时除【自身角色】需向下找到所有【下级组织的角色】
                    */
                    orgList.AddRange(this.DeepParentList);
                    foreach (Organization org in orgList)
                    {
                        roleList.AddRange(org.RoleList);
                    }
                    /*
                    【当前组织】的【自身角色】会层叠到【所有下级组织中的岗位】。
                     所以检查互斥关系时，需向下找到所有【下级组织的岗位的角色】
                    */
                    List<Position> positionList = this.DeepPositionList;
                    foreach (Position position in positionList)
                    {
                        roleList.AddRange(position.DeepRoleList);
                    }
                    _LinkRoleList = roleList
                        .DistinctBy(item => item.Id)
                        .ToList();
                }
                return _LinkRoleList;
            }
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