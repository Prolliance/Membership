using System;
using System.Data;
using System.Data.SqlClient;
using System.Management.Instrumentation;
using Amuse.Extends;
using Membership.DataProvider.SQLServer;
using Prolliance.Membership.DataPersistence;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Prolliance.Membership.DataProvider.SQLServer
{
    public class DataProvider : IDataProvider
    {
        public string ConnectionString
        {
            get
            {
                return DbContext.ConnectionString;
            }
            set
            {
                DbContext.ConnectionString = value;
            }
        }

        public void CreateDataRepo()
        {
            using (DataContext context = DbContext.Create())
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                }
            }
        }

        public virtual Info Add<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            using (DataContext context = DbContext.Create())
            {
                context.GetTable<Info>().InsertOnSubmit(info);
                context.SubmitChanges();
                return info;
            }
        }

        public virtual List<Info> Read<Info>() where Info : class,IModel, new()
        {
            using (DataContext context = DbContext.Create())
            {
                context.ObjectTrackingEnabled = false;
                Table<Info> infoTable = context.GetTable<Info>();
                var infoList = from info in infoTable
                               select info;
                return infoList.ToList();
            }

        }


        public virtual Info Update<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            /*
            Delete<Info>(info);
            return Add<Info>(info);
            */
            using (DataContext context = DbContext.Create())
            {
                Table<Info> table = context.GetTable<Info>();
                Info oldInfo = table.FirstOrDefault(x => x.Id == info.Id);
                if (oldInfo != null)
                {
                    table.DeleteOnSubmit(oldInfo);
                    table.InsertOnSubmit(info);
                    context.SubmitChanges();
                }
                return info;
            }
        }

        public virtual Info Delete<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            using (DataContext context = DbContext.Create())
            {
                //context.ObjectTrackingEnabled = false;
                Table<Info> table = context.GetTable<Info>();
                Info oldInfo = table.FirstOrDefault(x => x.Id == info.Id);
                if (oldInfo != null)
                {
                    //table.Attach(oldInfo);
                    table.DeleteOnSubmit(oldInfo);
                    context.SubmitChanges();
                }
                return info;
            }
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
