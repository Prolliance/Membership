using Prolliance.Membership.Common;
using System;

namespace Prolliance.Membership.ServiceClients.Models
{
    public abstract class ModelBase : Attribute
    {
        public ModelBase()
        {
            this.Id = StringFactory.NewGuid();
        }
        public string Id { get; set; }
    }
}
