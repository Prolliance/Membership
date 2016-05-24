using Amuse.Extends;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;

namespace Prolliance.Membership.DataProvider.MongoDB
{
    /// <summary>
    /// 暂用不到
    /// </summary>
    public static class QueryFactory
    {
        public static IMongoQuery Create<Info>(Info info)
        {
            var typeName = typeof(Info).Name;
            switch (typeName)
            {
                case "AppInfo":
                    {
                        return Query.EQ("Key", new BsonString(Convert.ToString(info.GetPropertyValue("Key"))));
                    }
                case "RoleInfo":
                    {
                        IMongoQuery query1 = Query.EQ("Code", new BsonString(Convert.ToString(info.GetPropertyValue("Code"))));
                        return Query.And(new IMongoQuery[] { query1 });
                    }
                case "RoleUserInfo":
                    {
                        IMongoQuery query1 = Query.EQ("RoleCode", new BsonString(Convert.ToString(info.GetPropertyValue("RoleCode"))));
                        IMongoQuery query2 = Query.EQ("Account", new BsonString(Convert.ToString(info.GetPropertyValue("Account"))));
                        return Query.And(query1, query2);
                    }
                case "RoleOrganizationInfo":
                    {
                        IMongoQuery query1 = Query.EQ("RoleCode", new BsonString(Convert.ToString(info.GetPropertyValue("RoleCode"))));
                        IMongoQuery query2 = Query.EQ("OrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OrganizationCode"))));
                        return Query.And(query1, query2);
                    }
                case "RolePositionInfo":
                    {
                        IMongoQuery query1 = Query.EQ("RoleCode", new BsonString(Convert.ToString(info.GetPropertyValue("RoleCode"))));
                        IMongoQuery query2 = Query.EQ("OrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OrganizationCode"))));
                        IMongoQuery query3 = Query.EQ("PositionCode", new BsonString(Convert.ToString(info.GetPropertyValue("PositionCode"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3 });
                    }
                case "RoleOperationInfo":
                    {
                        IMongoQuery query1 = Query.EQ("RoleCode", new BsonString(Convert.ToString(info.GetPropertyValue("RoleCode"))));
                        IMongoQuery query2 = Query.EQ("AppKey", new BsonString(Convert.ToString(info.GetPropertyValue("AppKey"))));
                        IMongoQuery query3 = Query.EQ("TargetCode", new BsonString(Convert.ToString(info.GetPropertyValue("TargetCode"))));
                        IMongoQuery query4 = Query.EQ("OperationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OperationCode"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3, query4 });
                    }
                case "RoleMutexInfo":
                    {
                        IMongoQuery query1 = Query.EQ("Group", new BsonString(Convert.ToString(info.GetPropertyValue("Group"))));
                        IMongoQuery query2 = Query.EQ("Type", new BsonInt32(Convert.ToInt32(info.GetPropertyValue("Type").ToString())));
                        IMongoQuery query3 = Query.EQ("RoleCode", new BsonString(Convert.ToString(info.GetPropertyValue("RoleCode"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3 });
                    }
                case "UserInfo":
                    {
                        return Query.EQ("Account", new BsonString(Convert.ToString(info.GetPropertyValue("Account"))));
                    }
                case "UserStateInfo":
                    {
                        IMongoQuery query1 = Query.EQ("Account", new BsonString(Convert.ToString(info.GetPropertyValue("Account"))));
                        IMongoQuery query2 = Query.EQ("DeviceId", new BsonString(Convert.ToString(info.GetPropertyValue("DeviceId"))));
                        return Query.And(query1, query2);
                    }
                case "OrganizationInfo":
                    {
                        return Query.EQ("Code", new BsonString(Convert.ToString(info.GetPropertyValue("Code"))));
                    }
                case "TargetInfo":
                    {
                        IMongoQuery query1 = Query.EQ("AppKey", new BsonString(Convert.ToString(info.GetPropertyValue("AppKey"))));
                        IMongoQuery query2 = Query.EQ("Code", new BsonString(Convert.ToString(info.GetPropertyValue("Code"))));
                        return Query.And(query1, query2);
                    }
                case "OperationInfo":
                    {
                        IMongoQuery query1 = Query.EQ("AppKey", new BsonString(Convert.ToString(info.GetPropertyValue("AppKey"))));
                        IMongoQuery query2 = Query.EQ("TargetCode", new BsonString(Convert.ToString(info.GetPropertyValue("TargetCode"))));
                        IMongoQuery query3 = Query.EQ("Code", new BsonString(Convert.ToString(info.GetPropertyValue("Code"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3 });
                    }
                case "PositionInfo":
                    {
                        IMongoQuery query1 = Query.EQ("OrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OrganizationCode"))));
                        IMongoQuery query2 = Query.EQ("Code", new BsonString(Convert.ToString(info.GetPropertyValue("Code"))));
                        return Query.And(new IMongoQuery[] { query1, query2 });
                    }
                case "PositionReportToInfo":
                    {
                        IMongoQuery query1 = Query.EQ("OrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OrganizationCode"))));
                        IMongoQuery query2 = Query.EQ("PositionCode", new BsonString(Convert.ToString(info.GetPropertyValue("PositionCode"))));
                        IMongoQuery query3 = Query.EQ("HigherOrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("HigherOrganizationCode"))));
                        IMongoQuery query4 = Query.EQ("HigherPositionCode", new BsonString(Convert.ToString(info.GetPropertyValue("HigherPositionCode"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3 });
                    }
                case "PositionUserInfo":
                    {
                        IMongoQuery query1 = Query.EQ("OrganizationCode", new BsonString(Convert.ToString(info.GetPropertyValue("OrganizationCode"))));
                        IMongoQuery query2 = Query.EQ("PositionCode", new BsonString(Convert.ToString(info.GetPropertyValue("PositionCode"))));
                        IMongoQuery query3 = Query.EQ("Account", new BsonString(Convert.ToString(info.GetPropertyValue("Account"))));
                        return Query.And(new IMongoQuery[] { query1, query2, query3 });
                    }
            }
            return null;
        }
    }
}
