using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolliance.Membership.DataTransfer.Models
{
    public class UserPhotoInfo
    {
        public string Account { get; set; }

        public byte[] PhotoBinary { get; set; }

        public string PhotoExt { get; set; }
    }
}
