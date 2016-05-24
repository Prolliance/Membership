using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolliance.Membership.DataPersistence.Models
{
    public class UserPhotoInfo : IModel
    {
        public string Id { get; set; }

        public string Account { get; set; }

        public byte[] PhotoBinary { get; set; }

        public string PhotoExt { get; set; }
    }
}
