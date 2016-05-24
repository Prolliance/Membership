using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 互斥关系对像
    /// </summary>
    public class RoleMutex : ModelBase
    {

        internal static DataRepo<RoleMutexInfo> RoleMutexInfoRepo = new DataRepo<RoleMutexInfo>();

        #region 属性
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 互斥类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 互斥关系组名
        /// </summary>
        public string Group { get; set; }
        #endregion

        #region 静态方法
        /// <summary>
        /// 获取互斥列表
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static List<RoleMutex> GetRoleMutexList()
        {
            return RoleMutexInfoRepo.Read().MappingToList<RoleMutex>();
        }
        /// <summary>
        /// 根据 group 获取互斥列表
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        public static List<RoleMutex> GetRoleMutexList(string group)
        {
            return GetRoleMutexList().Where(rm => rm.Group == group).ToList();
        }
        public static List<string> GetGroupList()
        {
            List<RoleMutex> mutexList = GetRoleMutexList();
            List<string> groupList = new List<string>();
            foreach (var mutex in mutexList)
            {
                if (!groupList.Contains(mutex.Group))
                {
                    groupList.Add(mutex.Group);
                }
            }
            return groupList;
        }
        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="group">关系组名</param>
        public static void RemoveGroup(string group)
        {
            var list = RoleMutexInfoRepo.Read().Where(r => r.Group == group).ToList();
            foreach (var item in list)
            {
                RoleMutexInfoRepo.Delete(item);
            }
        }
        public static bool CheckMutex(Role role1, Role role2, int type)
        {
            List<string> groupList = GetGroupList();
            List<RoleMutex> mutexList = GetRoleMutexList();
            foreach (var group in groupList)
            {
                var rs1 = mutexList.Exists(rm => rm.Group == group && rm.RoleId == role1.Id && rm.Type == type);
                var rs2 = mutexList.Exists(rm => rm.Group == group && rm.RoleId == role2.Id && rm.Type == type);
                if (rs1 && rs2)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 实列方法

        /// <summary>
        /// 新增
        /// </summary>
        public void Save()
        {
            if (RoleMutexInfoRepo.Read()
                .Exists(rm => rm.Group == this.Group
                && rm.Type == this.Type
                && rm.RoleId == this.RoleId))
            {
                return;
            }
            RoleMutexInfoRepo.Save(this.MappingTo<RoleMutexInfo>());
        }

        /// <summary>
        /// 移除
        /// </summary>
        public void Delete()
        {
            RoleMutexInfoRepo.Delete(this.MappingTo<RoleMutexInfo>());
        }
        #endregion
    }
}
