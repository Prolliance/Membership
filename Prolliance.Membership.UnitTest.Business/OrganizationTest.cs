using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class OrganizationTest
    {

        #region 静态方法
        [TestMethod]
        public void Create()
        {
            Organization org = Organization.Create();
            Assert.IsNotNull(org);
        }
        [TestMethod]
        public void GetOrganizationList()
        {
            List<Organization> list = Organization.GetOrganizationList();
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public void GetOrganization()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Organization org1 = Organization.GetOrganization(org.Code);
            Assert.IsNotNull(org1);

            Organization org2 = Organization.GetOrganization("");
            Assert.IsNull(org2);

            org.Delete();
            org1.Delete();

        }
        [TestMethod]
        public void GetOrganizationById()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Organization org1 = Organization.GetOrganizationById(org.Id);
            Assert.IsNotNull(org1);

            Organization org2 = Organization.GetOrganizationById("");
            Assert.IsNull(org2);

            org.Delete();
            org1.Delete();
        }
        #endregion

        #region 普通实例方法
        [TestMethod]
        public void Save()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            Organization org2 = Organization.GetOrganization(org.Code);
            Assert.IsNull(org2);
            org.Save();

            Organization org1 = Organization.GetOrganization(org.Code);
            Assert.IsNotNull(org1);
            org.Delete();
            org1.Delete();
        }

        [TestMethod]
        public void Delete()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Save();

            org1.GiveRole(role);//添加角色组织关系

            Position position = Position.Create();
            position.Code = StringFactory.NewGuid();
            position.IsActive = true;
            position.Save();

            org1.AddPosition(position);//添加组织下的岗位

            User user = User.Create();
            user.Account = "test-add-user" + StringFactory.NewGuid().Substring(0, 5);
            user.Save();

            org1.AddUser(user);//添加组织人员关系

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);

            org2.Delete();

            var positionlist = Position.GetPositionList().Where(po => po.Id == position.Id).ToList();
            Assert.AreEqual(0, positionlist.Count());

            Organization org3 = Organization.GetOrganization(org2.Code);
            Assert.IsNull(org3);

            org1.Delete();
            user.Delete();
            role.Delete();
            position.Delete();

        }

        #endregion

        #region 部门相关
        [TestMethod]
        public void AddChild()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            org.AddChild(org1);

            Organization org2 = Organization.GetOrganization(org.Code);
            Assert.IsNotNull(org2.Children.FirstOrDefault(o => o.Code == org1.Code));

            org.Delete();
            org1.Delete();
            org2.Delete();
        }
        [TestMethod]
        public void RemoveChild()
        {
            Organization org = Organization.Create();
            org.Code = StringFactory.NewGuid();
            org.Name = "org-test-name";
            org.FullName = "org-test-fullname";
            org.Save();

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();

            org.AddChild(org1);

            Organization org2 = Organization.GetOrganization(org.Code);
            Assert.IsNotNull(org2.Children.FirstOrDefault(o => o.Code == org1.Code));

            org.RemoveChild(org1);

            Organization org4 = Organization.GetOrganization(org.Code);
            var a = org4.Children.Where(o => o.Code == org1.Code).ToList();
            Assert.AreEqual(0, a.Count);

            org.Delete();
            org1.Delete();
        }

        [TestMethod]
        public void GetParent()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Organization org2 = Organization.Create();
            org2.Code = StringFactory.NewGuid();
            org2.Name = "org-test-name";
            org2.FullName = "org-test-fullname";
            org2.Save();

            org1.AddChild(org2);

            Organization org3 = org1.AddChild(org2);
            Assert.IsNotNull(org3);
            Assert.IsNotNull(org3.GetParent());

            org1.Delete();
            org1.RemoveChild(org2);
            org2.Delete();
            org3.Delete();
        }
        [TestMethod]
        public void SetParent()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Organization org2 = Organization.Create();
            org2.Code = StringFactory.NewGuid();
            org2.Name = "org-test-name";
            org2.FullName = "org-test-fullname";
            org2.Save();

            org2.SetParent(org1);

            Organization org3 = Organization.GetOrganization(org2.Code);
            Assert.IsNotNull(org3.GetParent());

            org1.Delete();
            org1.RemoveChild(org2);
            org2.Delete();
            org3.Delete();
        }
        [TestMethod]
        public void DepParentList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Organization org2 = Organization.Create();
            org2.Code = StringFactory.NewGuid();
            org2.Name = "org-test-name";
            org2.FullName = "org-test-fullname";
            org2.Save();

            org1.AddChild(org2);

            Organization org3 = Organization.GetOrganization(org2.Code);
            Assert.IsNotNull(org3);
            Assert.IsNotNull(org3.DeepParentList.Where(de => de.Id == org1.Id).ToList());

            org1.Delete();
            org1.RemoveChild(org2);
            org2.Delete();
            org3.Delete();
        }
        [TestMethod]
        public void ChildrenOrg()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Organization org2 = Organization.Create();
            org2.Code = StringFactory.NewGuid();
            org2.Name = "org-test-name";
            org2.FullName = "org-test-fullname";
            org2.Save();

            org1.AddChild(org2);

            Organization org3 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org3);
            Assert.IsNotNull(org3.Children);

            org1.Delete();
            org1.RemoveChild(org2);
            org2.Delete();
            org3.Delete();
        }

        [TestMethod]
        public void DeepChildrenOrg()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Organization org2 = Organization.Create();
            org2.Code = StringFactory.NewGuid();
            org2.Name = "org-test-name";
            org2.FullName = "org-test-fullname";
            org2.Save();
            org1.AddChild(org2);

            Organization org3 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org3);
            Assert.IsNotNull(org3.DeepChildren.Where(de => de.Id == org1.Id).ToList());

            org1.Delete();
            org1.RemoveChild(org2);
            org2.Delete();
            org3.Delete();
        }

        #endregion

        #region 岗位相关
        [TestMethod]
        public void AddPosition()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Code;

            org1.AddPosition(position);

            Organization org2 = Organization.GetOrganization(org1.Code);

            Assert.IsNotNull(org2.PositionList);

            org1.Delete();
            org1.RemovePosition(position);
        }
        [TestMethod]
        public void RemovePosition()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Code;

            org1.AddPosition(position);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2.PositionList);
            org1.RemovePosition(position);

            Organization org3 = Organization.GetOrganization(org2.Code);
            Assert.AreEqual(0, org3.PositionList.Count);

            org1.Delete();

        }
        [TestMethod]
        public void GetPositionList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Code;

            org1.AddPosition(position);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreNotEqual(0, org2.PositionList.Count);

            org1.Delete();
            org1.RemovePosition(position);
            org2.Delete();
        }
        [TestMethod]
        public void DeepPositionList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Code;

            org1.AddPosition(position);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreNotEqual(0, org2.DeepPositionList.Count);


            org1.Delete();
            org1.RemovePosition(position);
            org2.Delete();
        }

        #endregion

        #region 用户相关
        [TestMethod]
        public void AddUser()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            User user1 = new User();
            user1.Account = "test-user-add" + StringFactory.NewGuid().Substring(0, 4);
            user1.Name = "test-user-name";
            user1.IsActive = true;
            user1.Save();

            org1.AddUser(user1);

            var positionuser = org1.UserList.Where(pu => pu.Account == user1.Account).ToList();
            Assert.AreEqual(1, positionuser.Count);

            user1.Delete();
            org1.Delete();
            org1.RemoveUser(user1);
        }
        [TestMethod]
        public void RemoveUser()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            User user1 = new User();
            user1.Account = "test-user-add" + StringFactory.NewGuid().Substring(0, 4);
            user1.IsActive = true;
            user1.Save();

            org1.AddUser(user1);

            var positionuser = org1.UserList.Where(pu => pu.Account == user1.Account).ToList();
            Assert.AreEqual(1, positionuser.Count);

            org1.RemoveUser(user1);

            var positionuser1 = org1.UserList.Where(pu => pu.Account == user1.Account).ToList();
            Assert.AreEqual(0, positionuser1.Count);


            org1.Delete();
            user1.Delete();

        }
        [TestMethod]
        public void GetUserList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            User user1 = new User();
            user1.Account = "test-user-add" + StringFactory.NewGuid().Substring(0, 4);
            user1.IsActive = true;
            user1.Save();

            org1.AddUser(user1);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreNotEqual(0, org2.UserList.Count);

            org1.Delete();
            org1.RemoveUser(user1);
            user1.Delete();
        }
        [TestMethod]
        public void DeepUserList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            User user1 = new User();
            user1.Account = "test-user-add" + StringFactory.NewGuid().Substring(0, 4);
            user1.IsActive = true;
            user1.Save();

            org1.AddUser(user1);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreNotEqual(0, org2.DeepUserList.Count);

            org1.Delete();
            org1.RemoveUser(user1);
            user1.Delete();
        }

        #endregion

        #region 角色相关
        [TestMethod]
        public void GiveRole()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-add-role";
            role.Save();

            org1.GiveRole(role);

            var roleorg = org1.RoleList.FirstOrDefault(ro => ro.Code == role.Code);
            Assert.IsNotNull(roleorg);

            org1.Delete();
            org1.CancelRole(role);
            role.Delete();
        }
        [TestMethod]
        public void CancelRole()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-add-role";
            role.Save();

            org1.GiveRole(role);
            var roleorg = org1.RoleList.FirstOrDefault(ro => ro.Code == role.Code);
            Assert.IsNotNull(roleorg);

            org1.CancelRole(role);
            var roleorg1 = org1.RoleList.FirstOrDefault(ro => ro.Code == role.Code);
            Assert.IsNull(roleorg1);

            org1.Delete();
            role.Delete();
        }
        [TestMethod]
        public void GetRoleList()
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

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreEqual(1, org2.RoleList.Where(ro => ro.Code == role.Code).ToList().Count);

            org1.Delete();
            role.Delete();
            org1.CancelRole(role);
        }
        [TestMethod]
        public void GetDeepRoleList()
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

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.AreEqual(1, org2.DeepRoleList.Where(ro => ro.Code == role.Code).ToList().Count);

            org1.Delete();
            org1.CancelRole(role);
            role.Delete();
        }
        /*[TestMethod]
        public void GetLinkRoleList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Add();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Add();

            org1.GiveRole(role);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2.LinkRoleList);

            org1.Remove();
            org1.CancelRole(role);
            role.Remove();
        }*/

        #endregion

        #region 权限操作
        [TestMethod]
        public void GetTargetOperationList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Save();

            org1.GiveRole(role);

            Operation operation = new Operation();
            operation.AppId = StringFactory.NewGuid();
            operation.Code = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            role.GivePermission(operation);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.IsNotNull(org2.OperationList);

            org1.Delete();
            org1.CancelRole(role);
            role.CancelPermission(operation);
            role.Delete();

        }
        [TestMethod]
        public void GetDeepTargetOperationList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Save();

            org1.GiveRole(role);

            Operation operation = new Operation();
            operation.AppId = StringFactory.NewGuid();
            operation.Code = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            role.GivePermission(operation);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.IsNotNull(org2.DeepOperationList);

            org1.Delete();
            org1.CancelRole(role);
            role.CancelPermission(operation);
            role.Delete();
        }
        #endregion

        #region 权限对象
        [TestMethod]
        public void GetTargetList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            org1.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target.AddOperation(operation);

            role.GivePermission(operation);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.IsNotNull(org2.TargetList);

            org1.Delete();
            org1.CancelRole(role);
            role.Delete();
            target.Delete();
            target.RemoveOperation(operation);
        }
        [TestMethod]
        public void GetDeepTargetList()
        {
            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            org1.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target.AddOperation(operation);

            role.GivePermission(operation);

            Organization org2 = Organization.GetOrganization(org1.Code);
            Assert.IsNotNull(org2);
            Assert.IsNotNull(org2.TargetList);

            org1.Delete();
            org1.CancelRole(role);
            role.Delete();
            target.RemoveOperation(operation);
            target.Delete();
        }
        #endregion
    }
}
