using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Prolliance.Membership.Common
{
    public static class ObjectExtends
    {
        public static T MappingTo<T>(this object entity, List<string> ignoreList) where T : new()
        {
            return Mapper.Clone<T>(entity, ignoreList);
        }

        public static T MappingTo<T>(this object entity) where T : new()
        {
            return Mapper.Clone<T>(entity);
        }

        public static List<T> MappingToList<T>(this IEnumerable<object> entity) where T : new()
        {
            return Mapper.CloneList<T>(entity);
        }

        public static List<T> MappingToList<T>(this IEnumerable<object> entity, List<string> ignoreList) where T : new()
        {
            return Mapper.CloneList<T>(entity, ignoreList);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void ReBuildCache(this string cachekey)
        {
            try
            {
                if (string.IsNullOrEmpty(cachekey))
                {
                    return;
                }
                string service = "service/cache.ashx";
                var nodes = AppSettings.MembershipNodes;
               
                foreach (var node in nodes)
                {
                      var url = string.Format("{0}/{1}?cachekey={2}", node.TrimEnd("/\\".ToCharArray()), service, cachekey);
                    var task = new Task(() =>
                    {
                        WebClient client = new WebClient();
                        client.DownloadString(new Uri(url));

                    });
                    task.Start();

                }
            }
            catch (Exception ex)
            {
               
            }
        }
    }
}
