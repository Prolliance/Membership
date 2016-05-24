using Prolliance.EasyCache;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataPersistence.Utils;
using System;
using System.Collections.Generic;

namespace Prolliance.Membership.DataPersistence
{
    /// <summary>
    /// 数据表模型仓库
    /// </summary>
    /// <typeparam name="Info">表模型类型</typeparam>
    public class DataRepo<Info> where Info : class,IModel, new()
    {
        #region 静态公共私有成员
        private static ICache Cache = CacheManager.Create();
        private static IDataProvider Provider = DataRepoController.Provider;
        
        #endregion

        public DataRepo()
        {
            
        } 

        private string CacheKey = typeof(Info).FullName;

        public List<Info> Build()
        {
            Cache.Remove(CacheKey);
            return Read();
        }

       

        public Info Add(Info info)
        {
            if (info == null) return info;
            ModelIdHelper.GenerateId<Info>(info);
            if (Exists(info))
            {
                throw new Exception(string.Format("列表‘{0}’添加对象时，发现对象已存在", typeof(Info).Name));
            }
            Info rs = Provider.Add<Info>(info);
            Build();
            CacheKey.ReBuildCache();
            return rs;
        }

        public List<Info> Read()
        {
            if (Cache.Get<List<Info>>(CacheKey) == null)
            {
                List<Info> infoList = Provider.Read<Info>();
                foreach (Info info in infoList)
                {
                    ModelIdHelper.GenerateId<Info>(info);
                }
                Cache.Add(CacheKey, infoList);
            }
            return Cache.Get<List<Info>>(CacheKey);
        }

        public Info Update(Info info)
        {
            if (info == null) return info;
            ModelIdHelper.GenerateId<Info>(info);
            if (!Exists(info))
            {
                throw new Exception(string.Format("更新列表‘{0}’对象时，发现对象不存在", typeof(Info).Name));
            }
            Info rs = Provider.Update<Info>(info);
            Build();
            CacheKey.ReBuildCache();
            return rs;
        }

        public bool Exists(Info info)
        {
            if (info == null) return false;
            ModelIdHelper.GenerateId<Info>(info);
            return Read()
                .Exists(item => item != null
                    && item.Id == info.Id);
        }

        public Info Save(Info info)
        {
            if (Exists(info))
            {
                Update(info);
            }
            else
            {
                Add(info);
            }
            Build();
            CacheKey.ReBuildCache();
            return info;
        }

        public Info Delete(Info info)
        {
            if (info == null) return info;
            ModelIdHelper.GenerateId<Info>(info);
            Info rs = Provider.Delete<Info>(info);
            Build();
            CacheKey.ReBuildCache();
            return rs;
        }

        public Dictionary<string, string> GetTableStruct(string tableName)
        {
            return Provider.GetTableFields(tableName);
        }

        public string AddField(ExtensionField field)
        {
            return Provider.AddField(field);
        }

        public void ExecuteDynamicSql(string dynamicSql)
        {
            Provider.ExecuteDynamicSql(dynamicSql);
        }
    }
}
