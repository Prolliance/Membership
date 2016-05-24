using Microsoft.VisualStudio.TestTools.UnitTesting;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.UnitTest.DataPersitence
{

    [TestClass]
    public class DataProvider
    {
        [TestMethod]
        public void AddAndRead()
        {
            DataRepo<AppInfo> repo = new DataRepo<AppInfo>();
            //添加
            AppInfo info = new AppInfo();
            info.Id = StringFactory.NewGuid();
            info.Key = "test";
            info.Secret = "test";
            info.Name = "测试App";
            repo.Add(info);
            //通过读取验证添加是否成功
            List<AppInfo> infoList = repo.Read();
            Assert.IsNotNull(infoList);
            Assert.IsTrue(infoList.Count > 0);
            repo.Delete(info);
        }
        [TestMethod]
        public void Delete()
        {
            DataRepo<AppInfo> repo = new DataRepo<AppInfo>();
            //添加
            AppInfo appInfo = new AppInfo();
            appInfo.Id = StringFactory.NewGuid();
            appInfo.Key = "$deleteKey";
            appInfo.Secret = "test";
            appInfo.Name = "测试删除App";
            repo.Add(appInfo);
            repo.Delete(appInfo);
            Assert.IsTrue(repo.Read().Where(a => a.Key == "$deleteKey").Count() < 1);
        }
    }
}
