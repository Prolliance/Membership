using Amuse.Extends;
using Prolliance.Membership.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Prolliance.Membership.DataPersistence.Utils
{
    /// <summary>
    /// 此类的意义在于帮助表模型生成唯一的 _id
    /// _id 数据驱动可以自行选择是否存储 _id
    /// 如果表模型存在添加 ModelIdKey 特性的的属性，会根据这些属性的值生成 _id
    /// 如果表模型中没有 ModelIdKey 特性的的属性，需说明模型的 _id 上层业务类自行处理
    /// TODO: [2014-8-27]
    /// 当并发时操作这个通过 ModelIdKey 生成 Id 的表模型，因为 Id 有可能相同，而产生插入数据主键重复异常。
    /// 调整方案: 取消使用 ModelIdKey 所有表模型都采用生成的 GUID 的方式，带来的问题是，所有更新或删除操作必须先查询
    /// 目前 ModelIdKey 仅在 “关系表” 模型中有使用，实体表模型不存在这个问题，本身就是先查询，再更新或删除的
    /// 即使关系表，删除记录时，仅指定相关联实体 Id 进行删除的也不多，但一需要在 BLL 层中先仔细排查一遍。
    /// 然后，将 ModelIdHelper,ModelIdKeyAttribute 类删除，并将 DataRepo 中的相关调用移除即可
    /// </summary>
    internal static class ModelIdHelper
    {
        private static Dictionary<string, string> IdCache = new Dictionary<string, string>();
        private static string Hash(string text)
        {
            text = text ?? "";
            //进行 Hash
            if (!IdCache.ContainsKey(text))
            {
                IdCache[text] = StringFactory.Hash(text);
            }
            return IdCache[text];
        }
        private static string GetModelIdKeys<T>(T info) where T : IModel
        {
            StringBuilder buffer = new StringBuilder();
            PropertyInfo[] propertyList = info.GetProperties();
            foreach (PropertyInfo property in propertyList)
            {
                if (property.Name != "_id" && property.GetAttribute<ModelIdKeyAttribute>() != null)
                {
                    string val = Convert.ToString(info.GetPropertyValue(property.Name));
                    buffer.Append(val);
                }
            }
            return buffer.ToString();
        }
        public static void GenerateId<T>(T info) where T : IModel
        {
            if (!string.IsNullOrWhiteSpace(info.Id)) return;
            string idKeys = GetModelIdKeys<T>(info);
            if (!string.IsNullOrWhiteSpace(idKeys))
            {
                info.Id = Hash(idKeys);
            }
        }
    }
}
