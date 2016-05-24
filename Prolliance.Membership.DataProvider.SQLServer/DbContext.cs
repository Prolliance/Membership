using Prolliance.Membership.Common;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Prolliance.Membership.DataProvider.SQLServer
{
    public class DbContext
    {
        private static XmlMappingSource _Mapping = null;
        private static XmlMappingSource Mapping
        {
            get
            {
                if (_Mapping == null)
                {
                    string xmlBuffer = ResHelper.ReadTextFromRes(typeof(DbContext).Assembly, "Prolliance.Membership.DataProvider.SQLServer.Mapping.config");
                    _Mapping = XmlMappingSource.FromXml(xmlBuffer);
                }
                return _Mapping;
            }
        }
        internal static string ConnectionString { get; set; }
        /// 链接数据库
        /// </summary>
        /// <param name="dbSetting"></param>
        /// <returns></returns>
        public static DataContext Create()
        {
            DataContext dataContext = new DataContext(ConnectionString, Mapping);
           
            return dataContext;
        }
    }
}
