using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Linq;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class OperationTest
    {
        [TestMethod]
        public void GetTarget()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = app.Id;

            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);

            Operation operation = new Operation();
            target1.AddOperation(operation);

            Operation operation1 = Operation.GetOperationById(operation.Id);
            Assert.IsNotNull(operation1);

            Target target2 = operation1.GetTarget();
            Assert.IsNotNull(target2);


            app.RemoveTarget(target);
            app.Delete();
            target.RemoveOperation(operation);
        }

        [TestMethod]
        public void GetApp()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = app.Key;

            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);

            Operation operation = new Operation();
            target1.AddOperation(operation);

            Operation operation1 = Operation.GetOperationById(operation.Id);
            Assert.IsNotNull(operation1);

            App app1 = operation1.GetApp();
            Assert.IsNotNull(app1);


            app.RemoveTarget(target);
            app.Delete();
            target.RemoveOperation(operation);

        }

        [TestMethod]
        public void Delete()
        {
            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";
            targetoperation.Save();

            role.GivePermission(targetoperation);

            Role role1 = Role.GetRoleById(role.Id);
            var roletarget = role1.OperationList;
            Assert.IsNotNull(roletarget);

            Operation operation1 = Operation.GetOperationById(targetoperation.Id);
            operation1.Delete();

            Operation operation2 = Operation.GetOperationById(targetoperation.Id);
            Assert.IsNull(operation2);

            Role role2 = Role.GetRoleById(role.Id);
            var roletarget1 = role2.OperationList;
            Assert.AreEqual(0, roletarget1.Count);

            role.Delete();
        }

        [TestMethod]
        public void Save()
        {
            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            operation.Save();
            Operation operation1 = Operation.GetOperationById(operation.Id);
            Assert.IsNotNull(operation1);
            operation.Delete();
        }
        [TestMethod]
        public void GetOperation()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = app.Id;

            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target1.AddOperation(operation);

            Operation operation1 = Operation.GetOperation(app.Key, target1.Code, operation.Code);
            Assert.IsNotNull(operation1);

            app.RemoveTarget(target);
            app.Delete();
            target.RemoveOperation(operation);
        }

        [TestMethod]
        public void GetOperationById()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = app.Id;

            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);

            Operation operation = new Operation();
            target1.AddOperation(operation);

            Operation operation1 = Operation.GetOperationById(operation.Id);
            Assert.IsNotNull(operation1);

            app.RemoveTarget(target);
            app.Delete();
            target.RemoveOperation(operation);
        }

        [TestMethod]
        public void GetOperationList()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = app.Id;

            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);

            Operation operation = new Operation();
            target1.AddOperation(operation);

            var list = Operation.GetOperationList().Where(o => o.AppId == app.Id && o.TargetId == target.Id).ToList();
            Assert.AreEqual(1, list.Count());

            app.RemoveTarget(target);
            app.Delete();
            target.RemoveOperation(operation);
        }
    }
}
