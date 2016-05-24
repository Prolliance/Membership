using Amuse.Extends;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Prolliance.Membership.DataPersistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Prolliance.Membership.DataProvider.MongoDB
{
    public class DataProvider : IDataProvider
    {
        private string _ConnectionString;

        private MongoDatabase Db { get; set; }

        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = value;
                this.Db = DbContext.Create(_ConnectionString);
            }
        }

        public void CreateDataRepo()
        {
            return;
        }

        private MongoCollection GetCollection<Info>() where Info : IModel, new()
        {
            return Db.GetCollection(typeof(Info).Name.Replace("Info", ""));
        }

        /**
         * 因为每个表的主键都是程序生成的，插入数据时，需注意不能出来重复的主键，特别在并发插入时
         * 其实在业务层有做主键检查，但还是需要注意一下。
         */
        public Info Add<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            GetCollection<Info>().Insert(info);
            return info;
        }

        public List<Info> Read<Info>() where Info : class,IModel, new()
        {
            List<Info> itemList = new List<Info>();
            MongoCursor<BsonDocument> cursor = GetCollection<Info>().FindAllAs<BsonDocument>();
            List<BsonDocument> docList = cursor.ToList<BsonDocument>();
            foreach (BsonDocument doc in docList)
            {
                Info item = new Info();
                foreach (string name in doc.Names)
                {
                    if (string.IsNullOrWhiteSpace(name)) continue;
                    var _name = name.ToLower();
                    if (_name == "_id" || _name == "id")
                    {
                        item.SetPropertyValue("Id", doc[name].ToString());
                    }
                    else if (doc[name].IsBsonBinaryData)
                    {
                        item.SetPropertyValue(name, doc[name].AsByteArray);
                    }
                    else
                    {
                        item.SetPropertyValue(name, doc[name].ToString());
                    }
                }
                itemList.Add(item);
            }
            return itemList;
        }

        /**
         * 因为每个表的主键都是程序生成的，更新数据时，需注意不能出来重复的主键，特别在更新插入时
         */
        public Info Update<Info>(Info info) where Info : class,IModel, new()
        {
            /*
             * 虽然 Delete 、Add 在操作数据库时都是有锁的
             * 但是 Update 如果采用先 Delete 再 Add 的方法，并发时，会导致 n 个 Delete 都执行完，再执行 n 个Add。
             * 1,2 —> 1,2 -是期望的，1,1 -> 2,2 是错误的，原因是因为 Update 方法是没有 delete->add 的整体锁的。
             * 又因为 Membership 的主键会程序生成的 ，因此，带来插入数据时的 “主键重复的异常”。
             * 即使并发，正常的程序也不会插入 “相同主键的记录”，Add 方法在业务层有做主键重复检查，
             * 但是，在 Update 中调用 Add 却可以白绕过检查，导到问题
             */
            if (info == null) return info;
            /*错误用法
            Delete<Info>(info);
            return Add<Info>(info);
            */
            GetCollection<Info>().Save(info);
            return info;
        }

        public Info Delete<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            //利用条件工厂生成条件，然后按条件删除
            //GetCollection<Info>().Remove(QueryFactory.Create<Info>(info));
            //利用记录Id删除
            IMongoQuery query = Query.Or(Query.EQ("_id", info.Id), Query.EQ("Id", info.Id));
            GetCollection<Info>().Remove(query);
            return info;
        }
        public Dictionary<string, string> GetTableFields(string tableName)
        {
            return new Dictionary<string, string>();
        }

        public string AddField(ExtensionField field)
        {
            return string.Empty;
        }

        public void ExecuteDynamicSql(string dynamicSql)
        {
            throw new NotImplementedException();
        }
    }
}
