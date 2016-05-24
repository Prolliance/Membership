using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Linq;
using System.Threading;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class UserTest
    {
        #region 普通实列方法
        [TestMethod]
        public void Save()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            User user2 = User.GetUser(user.Account);
            Assert.IsNull(user2);
            user.Save();

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);

            user.Delete();
        }

        [TestMethod]
        public void Delete()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);
            user1.Delete();

            User user2 = User.GetUser(user1.Account);
            Assert.IsNull(user2);
            user.Delete();
        }

        #endregion

        #region 静态方法
        [TestMethod]
        public void GetUser()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);

            user.Delete();
        }
        [TestMethod]
        public void GetUserList()
        {
            var list = User.GetUserList();
            Assert.IsNotNull(list);
        }
        [TestMethod]
        public void Create()
        {
            User user = User.Create();
            Assert.IsNotNull(user);
        }
        [TestMethod]
        public void GetUserById()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            User user1 = User.GetUserById(user.Id);
            Assert.IsNotNull(user1);

            user.Delete();
        }
        #endregion

        #region 有关组织
        [TestMethod]
        public void OrganizationList()
        {
            User user = new User();
            user.Account = "test-add-user" + StringFactory.NewGuid().Substring(0, 4);
            user.Save();

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.IsActive = true;
            org1.Save();

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Id;
            position.Save();

            position.AddUser(user);

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);
            Assert.AreEqual(1, user1.OrganizationList.Count);

            user.Delete();
            position.RemoveUser(user);
            position.Delete();
            org1.Delete();
        }
        [TestMethod]
        public void DeepOrganizationList()
        {
            User user = new User();
            user.Account = "test-add-user" + StringFactory.NewGuid().Substring(0, 4);
            user.Save();

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.IsActive = true;
            org1.Save();

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Id;
            position.Save();

            position.AddUser(user);

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);
            Assert.AreEqual(1, user1.DeepOrganizationList.Count);

            user.Delete();
            position.RemoveUser(user);
            position.Delete();
            org1.Delete();
        }

        #endregion

        #region 有关岗位

        [TestMethod]
        public void PositionList()
        {
            User user = new User();
            user.Account = "test-add-user" + StringFactory.NewGuid().Substring(0, 4);
            user.Save();

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.IsActive = true;
            org1.Save();

            Position position = Position.Create();
            position.Name = "test-Position-add";
            position.Code = StringFactory.NewGuid();
            position.OrganizationId = org1.Id;
            position.IsActive = true;
            position.Save();

            position.AddUser(user);

            User user1 = User.GetUser(user.Account);
            Assert.IsNotNull(user1);
            Assert.AreEqual(1, user1.PositionList
                .Where(o => o.OrganizationId == org1.Id)
                .ToList().Count);

            user.Delete();
            position.RemoveUser(user);
            position.Delete();
            org1.Delete();
        }
        #endregion

        #region 有关权限
        [TestMethod]
        public void CheckPermission()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Operation target = new Operation();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.TargetId = StringFactory.NewGuid();
            target.Name = "test-add-RoleTargetOperation";

            bool b = user.CheckPermission(target);
            Assert.IsFalse(b);

            User user1 = new User();
            user1.Name = "test-add-user";
            user1.Account = StringFactory.NewGuid();
            user1.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            role.GivePermission(target);
            user.GiveRole(role);

            User user2 = User.GetUser(user.Account);
            var b1 = user2.CheckPermission(target);
            Assert.IsTrue(b1);

            role.Delete();
            user1.CancelRole(role);
            role.CancelPermission(target);
            user.Delete();
            user1.Delete();
        }
        #endregion

        #region 有关角色
        [TestMethod]
        public void GiveRole()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);
            User user1 = User.GetUser(user.Account);
            var user2 = user1.RoleList;
            Assert.AreEqual(1, user2.Count);

            role.Delete();
            user1.CancelRole(role);
            user.Delete();
        }
        [TestMethod]
        public void CancelRole()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Code = StringFactory.NewGuid();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);
            User user1 = User.GetUser(user.Account);
            var user2 = user1.RoleList.Where(r => r.Code == role.Code).ToList();
            Assert.AreEqual(1, user2.Count);

            user1.CancelRole(role);
            var user3 = user1.RoleList.Where(r => r.Code == role.Code).ToList();
            Assert.AreEqual(0, user3.Count);

            role.Delete();
            user.Delete();
        }
        [TestMethod]
        public void RoleList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);
            User user1 = User.GetUser(user.Account);
            var user2 = user1.RoleList.Where(r => r.Code == role.Code); ;
            Assert.IsNotNull(user2);

            user1.CancelRole(role);
            role.Delete();
            user.Delete();
        }
        [TestMethod]
        public void DeepRoleList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);
            User user1 = User.GetUser(user.Account);
            var user2 = user1.DeepRoleList.Where(r => r.Code == role.Code); ;
            Assert.IsNotNull(user2);

            user1.CancelRole(role);
            role.Delete();
            user.Delete();
        }
        #endregion

        #region 权限操作
        [TestMethod]
        public void TargetOperationList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            operation.AppId = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            operation.Name = "test-add-RoleTargetOperation";
            operation.Save();

            User user1 = new User();
            user1.Name = "test-add-user";
            user1.Account = StringFactory.NewGuid();
            user1.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            role.GivePermission(operation);
            user.GiveRole(role);

            User user2 = User.GetUser(user.Account);
            Assert.AreEqual(1, user2.OperationList.Count);

            role.Delete();
            user1.CancelRole(role);
            role.CancelPermission(operation);
            operation.Delete();
            user.Delete();
            user1.Delete();
        }
        [TestMethod]
        public void DeepTargetOperationList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            operation.AppId = StringFactory.NewGuid();
            operation.TargetId = StringFactory.NewGuid();
            operation.Name = "test-add-RoleTargetOperation";
            operation.Save();

            User user1 = new User();
            user1.Name = "test-add-user";
            user1.Account = StringFactory.NewGuid();
            user1.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            role.GivePermission(operation);
            user.GiveRole(role);

            User user2 = User.GetUser(user.Account);
            Assert.AreEqual(1, user2.DeepOperationList.Count);

            user1.CancelRole(role);
            role.CancelPermission(operation);
            operation.Delete();
            user.Delete();
            user1.Delete();
            role.Delete();
        }

        #endregion

        #region 权限对象
        [TestMethod]
        public void TargetList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            org1.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            target.AddOperation(targetoperation);

            role.GivePermission(targetoperation);


            User user1 = User.GetUser(user.Account);

            Assert.IsNotNull(user.TargetList);

            user.Delete();
            role.Delete();
            org1.Delete();
            org1.CancelRole(role);
            target.Delete();
            target.RemoveOperation(targetoperation);
            role.CancelPermission(targetoperation);

        }
        [TestMethod]
        public void DeapTargetList()
        {
            User user = new User();
            user.Name = "test-add-user";
            user.Account = StringFactory.NewGuid();
            user.Save();

            Role role = Role.Create();
            role.Name = "test-giverole-role";
            role.Save();

            user.GiveRole(role);

            Organization org1 = Organization.Create();
            org1.Code = StringFactory.NewGuid();
            org1.Name = "org-test-name";
            org1.FullName = "org-test-fullname";
            org1.Save();
            Assert.IsNotNull(org1);

            org1.GiveRole(role);

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Code = StringFactory.NewGuid();
            target.AddOperation(targetoperation);

            role.GivePermission(targetoperation);


            User user1 = User.GetUser(user.Account);

            Assert.IsNotNull(user.DeapTargetList);

            user.Delete();
            role.Delete();
            org1.Delete();
            org1.CancelRole(role);
            target.Delete();
            target.RemoveOperation(targetoperation);
            role.CancelPermission(targetoperation);
        }
        #endregion

        #region 身份验证
        //TODO  身份验证相关单元测试待完善
        [TestMethod]
        public void CheckLogin()
        {
            AutoResetEvent execEvent = new AutoResetEvent(false);
            var count = 10;
            for (var i = count; i > 0; i--)
            {
                Thread thread = new Thread(new ThreadStart(delegate
                {
                    UserState result = User.GetState("f39e1e4f0238177012581c78c53e15de");
                    count--;
                    if (count <= 0)
                    {
                        execEvent.Set();
                    }
                }));
                thread.Start();
            }
            execEvent.WaitOne();
        }
        [TestMethod]
        public void Login()
        {
            AuthParameter stateParamters = new AuthParameter();
            //loginParamters.Account=
        }
        #endregion
    }
}