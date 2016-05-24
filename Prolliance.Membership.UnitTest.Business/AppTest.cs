using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Business;
using Prolliance.Membership.Common;
using System.Collections.Generic;

namespace Prolliance.Membership.UnitTest.Business
{
    [TestClass]
    public class AppTest
    {
        [TestMethod]
        public void Create()
        {
            App app = App.Create();
            Assert.IsNotNull(app);
        }

        [TestMethod]
        public void GetAppList()
        {
            List<App> listapp = App.GetAppList();
            Assert.IsNotNull(listapp);
        }

        [TestMethod]
        public void GetApp()
        {
            App app1 = App.Create();
            app1.Key = StringFactory.NewGuid();
            app1.Name = "app-test";
            app1.Secret = "app-test-secret";
            app1.Save();
            App app2 = App.GetApp(app1.Key);
            Assert.IsNotNull(app2);
            App app3 = App.GetApp("");
            Assert.IsNull(app3);
            app2.Delete();
        }
        [TestMethod]
        public void GetAppById()
        {
            App app1 = App.Create();
            app1.Key = StringFactory.NewGuid();
            app1.Name = "app-test";
            app1.Secret = "app-test-secret";
            app1.Save();
            App app2 = App.GetAppById(app1.Id);
            Assert.IsNotNull(app2);
            App app3 = App.GetAppById("");
            Assert.IsNull(app3);
            app2.Delete();
        }

        //验证实例方法
        [TestMethod]
        public void Save()
        {
            App app1 = App.Create();
            app1.Key = StringFactory.NewGuid();
            app1.Name = "app-test";
            app1.Secret = "app-test-secret";
            app1.Save();
            App app2 = App.GetApp(app1.Key);
            Assert.IsNotNull(app2);
            app1.Delete();
        }

        /// <summary>
        /// Disable
        /// </summary>
        [TestMethod]
        public void Delete()
        {
            App app = App.Create();
            app.Save();

            //验证Target是否存在
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();

            app.AddTarget(target);

            App app1 = App.GetApp(app.Key);
            Assert.IsNotNull(app1);
            Assert.AreEqual(1, app1.TargetList.Count);


            Role role = Role.Create();
            role.Name = "test-role-add";
            role.Save();

            Operation targetoperation = new Operation();
            targetoperation.Code = StringFactory.NewGuid();
            targetoperation.AppId = StringFactory.NewGuid();
            targetoperation.TargetId = StringFactory.NewGuid();
            targetoperation.Name = "test-add-RoleTargetOperation";
            targetoperation.Save();

            target.AddOperation(targetoperation);

            Target target1 = Target.GetTargetById(target.Id);
            Assert.IsNotNull(target1.OperationList);

            App app2 = App.GetApp(app1.Key);
            Assert.IsNotNull(app2);

            app2.Delete();

            App app3 = App.GetApp(app1.Key);
            Assert.IsNull(app3);

            Target target2 = Target.GetTargetById(target.Id);
            Assert.IsNull(target2);

            app.Delete();
            app.RemoveTarget(target);
            target.RemoveOperation(targetoperation);
            role.Delete();
            targetoperation.Delete();
        }
        [TestMethod]
        public void TargetList()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();

            App app = App.Create();
            app.Save();

            app.AddTarget(target);

            App app1 = App.GetApp(app.Key);
            Assert.IsNotNull(app1);
            Assert.AreEqual(1, app1.TargetList.Count);

            app1.RemoveTarget(target);

            app.Delete();
            target.Delete();
        }

        [TestMethod]
        public void AddTarget()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();

            App app = App.Create();
            app.Save();

            app.AddTarget(target);

            App app1 = App.GetApp(app.Key);
            Assert.IsNotNull(app1);
            Assert.AreEqual(1, app1.TargetList.Count);

            app1.RemoveTarget(target);

            app.Delete();
            target.Delete();
        }

        [TestMethod]
        public void RemoveTarget()
        {
            Target target = Target.Create();
            target.Code = StringFactory.NewGuid();

            App app = App.Create();
            app.Save();

            app.AddTarget(target);

            App app1 = App.GetApp(app.Key);
            Assert.AreEqual(1, app1.TargetList.Count);

            app1.RemoveTarget(target);

            App app2 = App.GetApp(app1.Key);
            Assert.AreEqual(0, app1.TargetList.Count);

            app.Delete();
            target.Delete();
        }
    }
}
