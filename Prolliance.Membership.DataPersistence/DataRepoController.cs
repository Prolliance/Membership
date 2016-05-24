using Amuse;

namespace Prolliance.Membership.DataPersistence
{
    public class DataRepoController
    {
        internal static readonly string DATA_PRIVATER = "data-provider";
        internal static IDataProvider Provider = Container.Create().Get<IDataProvider>(DATA_PRIVATER);

        public static void CreateDataRepo()
        {
            Provider.CreateDataRepo();
        }
    }
}
