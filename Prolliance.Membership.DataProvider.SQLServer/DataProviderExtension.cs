using System;
using System.Data;
using System.Data.SqlClient;
using System.Management.Instrumentation;
using System.Reflection.Emit;
using Amuse.Extends;
using Membership.DataProvider.SQLServer;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Prolliance.Membership.DataProvider.SQLServer
{
    public class DataProviderExtension : IDataProvider
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

        public Info Add<Info>(Info info) where Info : class, IModel, new()
        {
            if (info == null) return info;
            var type = typeof(Info);
            if (checkIsExtension(type))
            {
                return AddExtension(info, type);
            }
            using (DataContext context = DbContext.Create())
            {
                context.GetTable<Info>().InsertOnSubmit(info);
                context.SubmitChanges();
                return info;
            }
        }

        /// <summary>
        /// 如果是添加可扩展表
        /// </summary>
        /// <typeparam name="Info"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        private Info AddExtension<Info>(Info info, Type type)
        {

            string insertSql = "insert into dbo.[{0}]({1}) values({2})";
            List<SqlParameter> parmlist = new List<SqlParameter>();
            List<string> cols = new List<string>();
            List<string> parmNameList = new List<string>();
            foreach (var p in type.GetProperties())
            {
                if (p.Name == "Extensions" && p.PropertyType == typeof(Dictionary<string, object>))
                {
                    var dic = p.GetValue(info) as Dictionary<string, object>;
                    if (dic != null)
                    {
                        foreach (var key in dic.Keys)
                        {
                            cols.Add(key);
                            parmlist.Add(getParameter(string.Format("@{0}", key), dic[key]));
                            parmNameList.Add(string.Format("@{0}", key));
                        }
                    }
                    continue;
                }
                parmNameList.Add(string.Format("@{0}", p.Name));
                parmlist.Add(getParameter(string.Format("@{0}", p.Name), p.GetValue(info)));
                cols.Add(p.Name);
            }

            insertSql = string.Format(insertSql, type.Name.Remove(type.Name.Length - 4, 4), string.Join(",", cols), string.Join(",", parmNameList));
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, insertSql, parmlist.ToArray());
            return info;
        }

        private SqlParameter getParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, (value == null || string.IsNullOrEmpty(value.ToString())) ? DBNull.Value : TryConvertDate(value));
        }

        private object TryConvertDate(object value)
        {
            DateTime dt;
            if (DateTime.TryParse(value.ToString(), out dt))
            {
                return dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return value;
        }

        private bool checkIsExtension(Type type)
        {
            return !string.IsNullOrEmpty(AppSettings.ExtensionModel) &&
                   AppSettings.ExtensionModel.Split(',').Any(p => p == type.Name);
        }


        public List<Info> Read<Info>() where Info : class,IModel, new()
        {

            var type = typeof(Info);
            var ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.Text, string.Format("select * from dbo.[{0}]", type.Name.Remove(type.Name.Length - 4, 4)));
            var list = new List<Info>();

            if (checkIsExtension(type))
            {
                return ReadExtensionInfo<Info>(ds);
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var inst = CreateReturnValue<Info>(type);
                foreach (var pr in type.GetProperties())
                {
                    pr.SetValue(inst, dr[pr.Name] == DBNull.Value ? null : dr[pr.Name]);
                }

                list.Add(inst);
            }
            return list;

        }
        /// <summary>
        /// 动态读取扩展表
        /// </summary>
        /// <typeparam name="Info"></typeparam>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<Info> ReadExtensionInfo<Info>(DataSet ds)
        {
            var type = typeof(Info);
            var list = new List<Info>();
            var constFeilds = type.GetProperties().Select(p => p.Name);
            var cols = ds.Tables[0].Columns;
            var listextension = new List<string>();
            var isext =
                type.GetProperties()
                    .FirstOrDefault(p => p.Name == "Extensions" && p.PropertyType == typeof(Dictionary<string, object>));

            foreach (DataColumn col in cols)
            {
                if (!constFeilds.Any(p => p == col.ColumnName))
                {
                    listextension.Add(col.ColumnName);
                }
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                var inst = CreateReturnValue<Info>(type);
                foreach (var pr in type.GetProperties())
                {
                    if (pr.Name == "Extensions")
                    {
                        continue;
                    }
                    pr.SetValue(inst, dr[pr.Name] == DBNull.Value ? null : dr[pr.Name]);
                }


                if (isext != null)
                {
                    var dic = new Dictionary<string, object>();
                    foreach (var ex in listextension)
                    {
                        dic.Add(ex, dr[ex]);
                    }
                    isext.SetValue(inst, dic);
                }

                list.Add(inst);
            }
            return list;
        }

        private T CreateReturnValue<T>(Type type)
        {
            var construct = type.GetConstructor(new Type[] { });
            return (T)construct.Invoke(null);
        }

        public Info Update<Info>(Info info) where Info : class,IModel, new()
        {
            if (info == null) return info;
            /*
            Delete<Info>(info);
            return Add<Info>(info);
            */
            var type = typeof(Info);
            if (checkIsExtension(type))
            {
                return UpdateExtension(info, type);
            }
            using (DataContext context = DbContext.Create())
            {
                Table<Info> table = context.GetTable<Info>();
                var list  = table.ToList();
                  Info oldInfo=list.FirstOrDefault(x => x.Id == info.Id);
                if (oldInfo != null)
                {
                    table.DeleteOnSubmit(oldInfo);
                    table.InsertOnSubmit(info);

                    context.SubmitChanges();
                }
                return info;
            }
        }

        private Info UpdateExtension<Info>(Info info, Type type) where Info : class,IModel, new()
        {

            string insertSql = "update dbo.[{0}] set {1}  where Id=@Id";
            List<SqlParameter> parmlist = new List<SqlParameter>();
            List<string> cols = new List<string>();

            foreach (var p in type.GetProperties())
            {
                if (p.Name.ToLower() == "id")
                {
                    continue;
                }
                if (p.Name == "Extensions" && p.PropertyType == typeof(Dictionary<string, object>))
                {
                    var dic = p.GetValue(info) as Dictionary<string, object>;
                    if (dic != null)
                    {
                        foreach (var key in dic.Keys)
                        {
                            parmlist.Add(getParameter(string.Format("@{0}", key), dic[key]));
                            cols.Add(string.Format("{0}=@{0}", key));
                        }
                    }
                    continue;
                }

                cols.Add(string.Format("{0}=@{0}", p.Name));
                parmlist.Add(getParameter(string.Format("@{0}", p.Name), p.GetValue(info)));

            }
            parmlist.Add(new SqlParameter("@Id", info.Id));

            insertSql = string.Format(insertSql, type.Name.Remove(type.Name.Length - 4, 4), string.Join(",", cols));
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, insertSql, parmlist.ToArray());
            return info;
        }

        public Info Delete<Info>(Info info) where Info : class,IModel, new()
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

            Dictionary<string, string> fields = new Dictionary<string, string>();
            string sql = string.Format(@"SELECT a.name as f_name, b.value from 
sys.syscolumns a LEFT JOIN sys.extended_properties b on a.id=b.major_id AND a.colid=b.minor_id AND b.name='MS_Description' 
WHERE object_id('{0}')=a.id ORDER BY a.colid", tableName);

            SqlDataReader dr = SqlHelper.ExecuteReader(ConnectionString, CommandType.Text, sql);
            while (dr.Read())
            {
                fields.Add(dr.GetString(0), dr.IsDBNull(1) ? "" : dr.GetString(1));

            }
            dr.Close();
            return fields;


        }

        public string AddField(ExtensionField field)
        {
            try
            {
                string sql = string.Format(@"alter table dbo.[{0}] add {1} {2} {3} ", field.TableName, field.ColumnName,
                    getDBType(field.Type, field.Length), getDefault(field.Type, field.Defaultval));
                string dessql =
                    string.Format(
                        @"EXECUTE sp_addextendedproperty N'MS_Description', '{0}', N'user', N'dbo', N'TABLE', N'{1}', N'column', N'{2}'",
                        field.Des, field.TableName, field.ColumnName);
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, sql);
                SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, dessql);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return string.Empty;

        }

        private string getDBType(ColumnType type, int length)
        {
            switch (type)
            {
                case ColumnType.Date:
                    return "DateTime";
                case ColumnType.String:
                    return string.Format("nvarchar({0})", length);
                case ColumnType.Number:
                    return length == 0 ? "int" : string.Format("decimal(18,{0})", length);
                default:
                    return string.Empty;
            }
        }

        private string getDefault(ColumnType type, string defaultVal)
        {
            if (string.IsNullOrEmpty(defaultVal))
            {
                return string.Empty;
            }
            return string.Format("default {0}", type == ColumnType.String ? "'" + defaultVal + "'" : defaultVal);
        }

        public void ExecuteDynamicSql(string dynamicSql)
        {
            SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, dynamicSql);
        }
    }
}
