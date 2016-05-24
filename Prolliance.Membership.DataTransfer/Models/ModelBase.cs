using Prolliance.Membership.Common;

namespace Prolliance.Membership.DataTransfer.Models
{
    public abstract class ModelBase
    {
        public ModelBase()
        {
            this.Id = StringFactory.NewGuid();
        }
        public string Id { get; set; }
    }
}
