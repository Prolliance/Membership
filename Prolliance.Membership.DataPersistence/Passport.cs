using Amuse;

namespace Prolliance.Membership.DataPersistence
{

    public static class Passport
    {
        private static readonly string PASSPORT = "passport";

        public static IPassportProvider Cteate()
        {
            return Container.Create().Get<IPassportProvider>(PASSPORT);
        }
    }
}
