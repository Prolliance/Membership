using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Linq;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class TargetTest
    {
        #region 静态方法
        [TestMethod]
        public void Create()
        {
            Target target = Target.Create();
            Assert.IsNotNull(target);
        }
        [TestMethod]
        public void GetTargetById()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1);

            target1.Delete();
        }
        [TestMethod]
        public void GetTarget()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Save();

            app.AddTarget(target);

            Target target1 = Target.GetTarget(app.Key, target.Code);
            Assert.IsNotNull(target1);

            app.Delete();
            app.RemoveTarget(target);
            target1.Delete();
        }
        [TestMethod]
        public void GetTargetList()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Save();

            var target1 = Target.GetTargetList();
            Assert.IsNotNull(target1);
            target.Delete();
        }
        #endregion

        #region 普通实例方法
        [TestMethod]
        public void Save()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();

            Target target2 = Target.GetTargetById(target.Id);
            Assert.IsNull(target2);
            target.Save();

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1);

            target.Delete();
        }

        [TestMethod]
        public void Delete()
        {
            App app = App.Create();
            app.Save();

            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Name = "test-target-add";
            app.AddTarget(target);

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1);

            Operation operation = new Operation();
            operation.Code = StringFactory.NewGuid();
            target1.AddOperation(operation);

            target.Delete();

            Assert.AreEqual(0, Operation.GetOperationList().Where(o => o.Id == operation.Id).ToList().Count());

            app.Delete();
        }

        #endregion

        #region 有关App
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
        #endregion

        #region 有关操作
        [TestMethod]
        public void OperationList()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Name = "test-target-add";
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";
            target.AddOperation(targetoperation);

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1.OperationList);

            target.Delete();
            target.RemoveOperation(targetoperation);
        }
        [TestMethod]
        public void AddOperation()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Name = "test-target-add";
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation targetoperation = new Operation();
            targetoperation.Name = "test-add-RoleTargetOperation";
            targetoperation.Code = StringFactory.NewGuid();
            target.AddOperation(targetoperation);

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1.OperationList);
            target.Delete();
            target.RemoveOperation(targetoperation);
        }
        [TestMethod]
        public void RemoveOperation()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();
            target.Name = "test-target-add";
            target.AppId = StringFactory.NewGuid();
            target.Save();

            Operation targetoperation = new Operation();
            targetoperation.Name = "test-add-RoleTargetOperation";
            targetoperation.Code = StringFactory.NewGuid();
            target.AddOperation(targetoperation);

            Target target1 = Target.GetTargetById(target.Id);
            Assert.AreEqual(1, target1.OperationList.Count);
            target.RemoveOperation(targetoperation);

            Target target2 = Target.GetTargetById(target1.Id);
            Assert.AreEqual(0, target2.OperationList.Count);

            target.Delete();
        }
        #endregion
    }
}
