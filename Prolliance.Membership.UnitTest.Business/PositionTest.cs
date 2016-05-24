using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class PositionTest
    {
        #region 静态方法
        [TestMethod]
        public void Create()
        {
            Position position = Position.Create();
            Assert.IsNotNull(position);
        }
        [TestMethod]
        public void GetPositionList()
        {
            List<Position> list = Position.GetPositionList();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void GetPosition()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            Position position1 = Position.GetPosition(org.Code, position.Code);
            Assert.IsNotNull(position1);

            Position position2 = Position.GetPosition("", "");
            Assert.IsNull(position2);

            position.Delete();
            org.Delete();

        }

        [TestMethod]
        public void GetPositionById()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            Position position1 = Position.GetPositionById(position.Id);
            Assert.IsNotNull(position1);

            Position position2 = Position.GetPosition("", "");
            Assert.IsNull(position2);

            position.Delete();
            org.Delete();
        }
        #endregion

        #region 实列方法
        [TestMethod]
        public void Save()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            Position position1 = Position.GetPosition(org.Code, position.Code);
            Assert.IsNull(position1);
            position.Save();

            Position position2 = Position.GetPosition(org.Code, position.Code);
            Assert.IsNotNull(position2);

            position.Delete();
            position2.Delete();
            org.Delete();
        }

        [TestMethod]
        public void Delete()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            User user = User.Create();
            user.Account = "test-add-user" + StringFactory.NewGuid().Substring(0, 5);
            user.Save();

            position.AddUser(user);

            Position postion1 = Position.GetPosition(org.Code, position.Code);
            Assert.IsNotNull(postion1);
            postion1.Delete();

            Position position2 = Position.GetPosition(org.Code, postion1.Code);
            Assert.IsNull(position2);

            org.Delete();
            user.Delete();

        }
        #endregion

        #region 汇报关系
        [TestMethod]
        public void ReportToList()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = StringFactory.NewGuid();
            position.Name = "test-add-Postion";
            position.IsActive = true;
            position.Save();

            Position position1 = Position.Create();
            position1.Code = StringFactory.NewGuid();
            position1.OrganizationId = org.Id;
            position1.Name = "test-add-Postion";
            position1.IsActive = true;
            position1.Save();

            position1.AddReportTo(position);

            Position newpostion = Position.GetPosition(org.Code, position1.Code);
            Assert.AreEqual(1, newpostion.ReportToList.Count);

            position1.RemoveReportTo(position);
            position1.Delete();
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void AddReportTo()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = StringFactory.NewGuid();
            position.Name = "test-add-Postion";
            position.IsActive = true;
            position.Save();

            Position position1 = Position.Create();
            position1.Code = StringFactory.NewGuid();
            position1.OrganizationId = org.Id;
            position1.Name = "test-add-Postion";
            position1.IsActive = true;
            position1.Save();

            position1.AddReportTo(position);

            Position newpostion = Position.GetPosition(org.Code, position1.Code);
            Assert.AreEqual(1, newpostion.ReportToList.Count);

            position1.RemoveReportTo(position);
            position1.Delete();
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void RemoveReportTo()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = StringFactory.NewGuid();
            position.Name = "test-add-Postion";
            position.IsActive = true;
            position.Save();

            Position position1 = Position.Create();
            position1.Code = StringFactory.NewGuid();
            position1.OrganizationId = org.Id;
            position1.Name = "test-add-Postion";
            position1.IsActive = true;
            position1.Save();

            position1.AddReportTo(position);

            Position newpostion = Position.GetPosition(org.Code, position1.Code);
            Assert.AreEqual(1, newpostion.ReportToList.Count);
            position1.RemoveReportTo(position);

            Position newpostion1 = Position.GetPosition(org.Code, position1.Code);
            Assert.AreEqual(0, newpostion1.ReportToList.Count);

            org.Delete();
            position1.Delete();
            position.Delete();
        }
        #endregion

        #region 部门相关
        [TestMethod]
        public void OrganizationTest()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            Position postion1 = Position.GetPosition(org.Code, position.Code);

            Organization org1 = postion1.GetOrganization();
            Assert.IsNotNull(org1);

            org.Delete();
            position.Delete();
        }
        #endregion

        #region 用户相关
        [TestMethod]
        public void UserList()
        {
            User user = new User();
            user.Account = "test-adduser-" + StringFactory.NewGuid().Substring(0, 4);
            user.Name = "test-add-user";
            user.IsActive = true;
            user.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.AddUser(user);

            var userInfo = position.UserList.FirstOrDefault(u => u.Account == user.Account);

            Assert.IsNotNull(userInfo);

            user.Delete();
            position.RemoveUser(user);
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void AddUser()
        {
            User user = new User();
            user.Account = "test-adduser-" + StringFactory.NewGuid().Substring(0, 4);
            user.Name = "test-add-user";
            user.IsActive = true;
            user.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.AddUser(user);

            var userInfo = position.UserList.FirstOrDefault(u => u.Account == user.Account);

            Assert.IsNotNull(userInfo);

            user.Delete();
            position.RemoveUser(user);
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void RemoveUser()
        {
            User user = new User();
            user.Account = "test-adduser-" + StringFactory.NewGuid().Substring(0, 4);
            user.Name = "test-add-user";
            user.IsActive = true;
            user.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Code;
            position.Name = "test-add-Postion";
            position.Save();

            position.AddUser(user);

            var positionUserInfo = position.UserList.FirstOrDefault(p => p.Account == user.Account);
            Assert.IsNotNull(positionUserInfo);
            position.RemoveUser(user);

            var newpositionUserInfo = position.UserList.FirstOrDefault(p => p.Account == user.Account);
            Assert.IsNull(newpositionUserInfo);

            user.Delete();
            org.Delete();
            position.Delete();
        }
        #endregion

        #region 角色相关
        [TestMethod]
        public void RoleList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.RoleList.Count);

            role.Delete();
            position.CancelRole(role);
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void DeepRoleList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.DeepRoleList.Count);

            role.Delete();
            position.CancelRole(role);
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void GiveRole()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.RoleList.Count);

            role.Delete();
            position.CancelRole(role);
            position.Delete();
            org.Delete();
        }
        [TestMethod]
        public void CancelRole()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.RoleList.Count);

            position.CancelRole(role);

            var position2 = Position.GetPosition(org.Code, position1.Code);
            Assert.AreEqual(0, position2.RoleList.Count);

            role.Delete();
            org.Delete();
            position.Delete();
        }
        #endregion


        #region 权限操作
        [TestMethod]
        public void OperationList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            Operation operation = new Operation();
            operation.AppId = StringFactory.NewGuid();
            operation.Code = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            operation.Save();
            role.GivePermission(operation);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.OperationList.Where(op => op.Id == operation.Id).ToList().Count);

            role.CancelPermission(operation);
            operation.Delete();
            position.CancelRole(role);
            position.Delete();
            role.Delete();
            org.Delete();

        }
        [TestMethod]
        public void DeepOperationList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.IsActive = true;
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            Operation operation = new Operation();
            operation.AppId = StringFactory.NewGuid();
            operation.Code = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            operation.Save();
            role.GivePermission(operation);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.DeepOperationList.Count);

            role.CancelPermission(operation);
            role.Delete();
            position.CancelRole(role);
            position.Delete();
            position.Delete();
        }
        #endregion


        #region 权限对象
        [TestMethod]
        public void TargetList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target.AddOperation(operation);

            role.GivePermission(operation);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.TargetList.Count);

            role.Delete();
            position.CancelRole(role);
            position.Delete();
            target.Delete();
            target.RemoveOperation(operation);
            org.Delete();
        }
        [TestMethod]
        public void DeapTargetList()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org.Id;
            position.Name = "test-add-Postion";
            position.Save();

            position.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target.AddOperation(operation);

            role.GivePermission(operation);

            var position1 = Position.GetPosition(org.Code, position.Code);
            Assert.AreEqual(1, position1.DeapTargetList.Count);

            role.Delete();
            position.CancelRole(role);
            position.Delete();
            target.Delete();
            target.RemoveOperation(operation);
            org.Delete();
        }
        #endregion
    }
}
