using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Collections.Generic;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class RoleTest
    {
        #region  静态方法
        [TestMethod]
        public void Create()
        {
            Role role = Role.Create();
            Assert.IsNotNull(role);
        }
        [TestMethod]
        public void GetRoleList()
        {
            List<Role> list = Role.GetRoleList();
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public void GetRole()
        {
            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-role-add";
            role.Save();

            Role role1 = Role.GetRole(role.Code);
            Assert.IsNotNull(role1);
            role.Delete();
        }
        [TestMethod]
        public void GetRoleById()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Role role1 = Role.GetRoleById(role.Id);
            Assert.IsNotNull(role1);
            role.Delete();
        }
        #endregion

        #region 普通实列方法
        [TestMethod]
        public void Save()
        {
            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-role-add";
            Role role2 = Role.GetRole(role.Code);
            Assert.IsNull(role2);
            role.Save();

            Role role1 = Role.GetRole(role.Code);
            Assert.IsNotNull(role1);
            role.Delete();
        }

        [TestMethod]
        public void Delete()
        {
            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-role-add";
            role.Save();

            Role role1 = Role.GetRole(role.Code);
            Assert.IsNotNull(role1);
            role1.Delete();

            Role role2 = Role.GetRole(role1.Code);
            Assert.IsNull(role2);

            role.Delete();

        }
        #endregion

        #region 权限相关方法

        [TestMethod]
        public void GivePermission()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";

            role.GivePermission(targetoperation);

            var roletarget = role.OperationList;
            Assert.IsNotNull(roletarget);

            role.Delete();
            role.CancelPermission(targetoperation);
        }
        [TestMethod]
        public void CancelPermission()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";

            role.GivePermission(targetoperation);

            var roletarget = role.OperationList;
            Assert.IsNotNull(roletarget);

            role.CancelPermission(targetoperation);

            var newroletarget = role.OperationList;
            Assert.AreEqual(0, newroletarget.Count);

            role.Delete();
        }
        [TestMethod]
        public void CheckPermission()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";
            targetoperation.Code = StringFactory.NewGuid();
            role.GivePermission(targetoperation);

            var roletarget = role.CheckPermission(targetoperation);
            Assert.IsTrue(roletarget);

            role.Delete();
            role.CancelPermission(targetoperation);

        }

        [TestMethod]
        public void OperationList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";

            role.GivePermission(targetoperation);

            var roletarget = role.OperationList;
            Assert.IsNotNull(roletarget);

            role.Delete();
        }
        [TestMethod]
        public void TargetList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-role-add";
            role.Save();

            org1.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation operation = new Operation();
            operation.AppId = StringFactory.NewGuid();
            operation.Code = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            target.AddOperation(operation);

            role.GivePermission(operation);

            Role role1 = Role.GetRole(role.Code);
            var list1 = role1.TargetList;
            Assert.IsNotNull(list1);


            role.Delete();
            org1.Delete();
            org1.CancelRole(role);
            target.Delete();
            target.RemoveOperation(operation);
            role.CancelPermission(operation);
        }
        #endregion

    }
}
