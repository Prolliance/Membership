using System.Web.Script.Serialization;

namespace Prolliance.Membership.Common.Serializes
{
    /// <summary>
    /// Josn序列化
    /// </summary>
    public static class JsonSerializer
    {
        private static JavaScriptSerializer Serializer = new JavaScriptSerializer();
        public static string Serialize(object obj)
        {
            return Serializer.Serialize(obj);
        }

        public static T Deserialize<T>(string jsonDatas)
        {
            if (string.IsNullOrEmpty(jsonDatas))
                return default(T);

            return Serializer.Deserialize<T>(jsonDatas);
        }
    }
}
