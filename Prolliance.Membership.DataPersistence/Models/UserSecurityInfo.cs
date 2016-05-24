using Prolliance.Membership.DataPersistence.Utils;

namespace Prolliance.Membership.DataPersistence.Models
{
    public class UserSecurityInfo : IModel
    {
        public string Id { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }

    }
}
