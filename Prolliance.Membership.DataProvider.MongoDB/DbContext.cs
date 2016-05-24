using MongoDB.Driver;
using Prolliance.Membership.Common;

namespace Prolliance.Membership.DataProvider.MongoDB
{
    public class DbContext
    {
        /// 链接数据库
        /// </summary>
        /// <param name="dbSetting"></param>
        /// <returns></returns>
        public static MongoDatabase Create(string connectionString)
        {
            var dbSettings = connectionString.Split('|');
            var dbServer = dbSettings[0];
            var dbName = AppSettings.Name;
            if (dbSettings.Length > 1)
                dbName = dbSettings[1];
            MongoClient client = new MongoClient(dbServer);
            //client.Settings.ConnectionMode = ConnectionMode.ReplicaSet;
            MongoServer server = client.GetServer();
            return server.GetDatabase(dbName);
        }
    }
}
