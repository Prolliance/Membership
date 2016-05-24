using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class RoleMutexTest
    {
        #region 静态方法
        [TestMethod]
        public void GetRoleMutexList()
        {
            List<RoleMutex> rolemutex = RoleMutex.GetRoleMutexList();
            Assert.IsNotNull(rolemutex);
        }
        [TestMethod]
        public void GetRoleMutexListBygroup()
        {
            RoleMutex rolemutex = new RoleMutex();
            rolemutex.Group = "test-add-rolemutex";
            rolemutex.Save();

            List<RoleMutex> rolemutex1 = RoleMutex.GetRoleMutexList(rolemutex.Group);
            Assert.IsNotNull(rolemutex1);

            rolemutex.Delete();
        }
        [TestMethod]
        public void GetGroupList()
        {
            List<string> list = RoleMutex.GetGroupList();
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public void RemoveGroup()
        {
            RoleMutex rolemutex = new RoleMutex();
            rolemutex.Group = "test-add-rolemutex";
            rolemutex.RoleId = StringFactory.NewGuid();
            rolemutex.Save();

            List<RoleMutex> list = RoleMutex.GetRoleMutexList();
            Assert.IsTrue(list.Where(r => r.Group == rolemutex.Group).ToList().Count > 0);

            RoleMutex.RemoveGroup(rolemutex.Group);

            List<RoleMutex> newlist = RoleMutex.GetRoleMutexList();
            Assert.AreEqual(0, newlist.Where(r => r.Group == rolemutex.Group).ToList().Count);
            rolemutex.Delete();
        }
        [TestMethod]
        public void CheckMutex()
        {
            RoleMutex rolemutex = new RoleMutex();
            rolemutex.Group = "test-add-rolemutex";
            rolemutex.Save();

            Role role1 = Role.Create();
            role1.Name = "test-check-rolemutex";
            role1.Save();

            Role role2 = Role.Create();
            role2.Name = "test-check-rolemutex2";
            role2.Save();

            bool b = RoleMutex.CheckMutex(role1, role2, RoleMutexType.Static);
            Assert.IsFalse(b);

            rolemutex.Delete();
            role1.Delete();
            role2.Delete();
        }
        #endregion

        #region 实列方法
        [TestMethod]
        public void Save()
        {
            RoleMutex rolemutex = new RoleMutex();
            rolemutex.Group = StringFactory.NewGuid();
            rolemutex.Type = RoleMutexType.Static;
            List<RoleMutex> list1 = RoleMutex.GetRoleMutexList();
            Assert.AreEqual(0, list1.Where(r => r.Group == rolemutex.Group).ToList().Count);
            rolemutex.Save();

            List<RoleMutex> list = RoleMutex.GetRoleMutexList();
            Assert.IsNotNull(list.Where(r => r.Group == rolemutex.Group).ToList());

            rolemutex.Delete();
        }
        [TestMethod]
        public void Delete()
        {
            RoleMutex rolemutex = new RoleMutex();
            rolemutex.Group = StringFactory.NewGuid();
            rolemutex.Type = RoleMutexType.Static;
            rolemutex.RoleId = StringFactory.NewGuid();
            rolemutex.Save();

            List<RoleMutex> list = RoleMutex.GetRoleMutexList();
            Assert.IsNotNull(list.Where(r => r.Group == rolemutex.Group).ToList());

            rolemutex.Delete();

            var rolemutex1 = RoleMutex.GetRoleMutexList().FirstOrDefault(r => r.Group == rolemutex.Group);
            Assert.IsNull(rolemutex1);
        }
        #endregion
    }
}
