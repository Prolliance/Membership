using AjaxEngine.Extends;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Prolliance.Membership.Common
{
    public class Mapper
    {
        public static Tag Clone<Tag>(object src, List<string> ignoreList = null) where Tag : new()
        {
            if (src == null) return default(Tag);
            Tag tag = new Tag();
            Clone(src, ref tag, ignoreList);
            return tag;
        }

        public static object Clone(object src, Type tagType, List<string> ignoreList = null)
        {
            if (src == null) return null;
            var tag = Activator.CreateInstance(tagType);
            Clone(src, ref tag, ignoreList);
            return tag;
        }

        public static void Clone<Src, Tag>(Src src, Tag tag, List<string> ignoreList = null)
        {
            Clone(src, ref tag, ignoreList);
        }

        public static void Clone<Src, Tag>(Src src, ref Tag tag, List<string> ignoreList = null)
        {
            if (src == null || tag == null) { return; }

            PropertyInfo[] tagPropertyInfoList = tag.GetProperties();
            PropertyInfo[] srcPropertyInfoList = src.GetProperties();
            if (tagPropertyInfoList == null || srcPropertyInfoList == null)
            { return; }
            foreach (PropertyInfo property in tagPropertyInfoList)
            {
                var propertyName = property.Name;
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }
                if (ignoreList != null && ignoreList.Contains(propertyName))
                {
                    continue;
                }
                if (srcPropertyInfoList.FirstOrDefault(p => p.Name == propertyName) == null)
                {
                    continue;
                }
                var srcPropertyValue = src.GetPropertyValue(propertyName);
                if (srcPropertyValue == null
                    || srcPropertyValue is String
                    || srcPropertyValue is Int32
                    || srcPropertyValue is DateTime
                    || srcPropertyValue is Boolean
                    || srcPropertyValue is Double
                    || srcPropertyValue is float
                    || srcPropertyValue is Int16
                    || srcPropertyValue is Int64
                    || srcPropertyValue is Decimal
                    || srcPropertyValue is Int32?
                    || srcPropertyValue is DateTime?
                    || srcPropertyValue is Boolean?
                    || srcPropertyValue is Double?
                    || srcPropertyValue is float?
                    || srcPropertyValue is Int16?
                    || srcPropertyValue is Int64?
                    || srcPropertyValue is Decimal?
                    || srcPropertyValue is Dictionary<string,object>
                    || srcPropertyValue is byte[])
                {
                    tag.SetPropertyValue(propertyName, srcPropertyValue);
                }
                else
                {
                    var tagPropertyValue = Activator.CreateInstance(property.PropertyType);
                    //TODO: 将来还可以扩展对字典子对象的支持
                    //如果属性是一个列表子对象
                    if (tagPropertyValue is IList)
                    {
                        IList tagList = (IList)tagPropertyValue;
                        var tagType = tagPropertyValue.GetType().GenericTypeArguments[0];
                        CloneList((IList)srcPropertyValue, ref tagList, tagType, ignoreList);
                        tag.SetPropertyValue(propertyName, tagList);
                    }
                    else //普通子对象属性
                    {
                        Clone<object, object>(srcPropertyValue, ref tagPropertyValue, ignoreList);
                        tag.SetPropertyValue(propertyName, tagPropertyValue);
                    }
                }
            }
        }

        public static void CloneList<Src, Tag>(IEnumerable<Src> srcList, ref IList<Tag> tagList, List<string> ignoreList = null) where Tag : new()
        {
            if (tagList == null || srcList == null) return;

            foreach (object src in srcList)
            {
                tagList.Add(Clone<Tag>(src, ignoreList));
            }
        }

        public static void CloneList(IList srcList, ref IList tagList, Type tagType, List<string> ignoreList = null)
        {
            if (tagList == null || srcList == null) return;

            foreach (object src in srcList)
            {
                tagList.Add(Clone(src, tagType, ignoreList));
            }
        }

        public static List<Tag> CloneList<Src, Tag>(IEnumerable<Src> srcList, List<string> ignoreList = null) where Tag : new()
        {
            if (srcList == null) return null;

            List<Tag> tagList = new List<Tag>();
            foreach (object src in srcList)
            {
                tagList.Add(Clone<Tag>(src, ignoreList));
            }
            return tagList;
        }

        public static List<Tag> CloneList<Tag>(IEnumerable<object> srcList, List<string> ignoreList = null) where Tag : new()
        {
            if (srcList == null) return null;

            List<Tag> tagList = new List<Tag>();
            foreach (object src in srcList)
            {
                tagList.Add(Clone<Tag>(src, ignoreList));
            }
            return tagList;
        }

    }
}
