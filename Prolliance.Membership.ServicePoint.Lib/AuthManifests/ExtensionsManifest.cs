using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.AuthManifests
{
  
    [Target(Code = "ext", Name = "扩展信息管理", Group = "常规功能")]
    public class ExtensionsManifest : TargetManifestBase
    {
   

        [Operation(Code = "new", Name = "新建")]
        public Operation New { get; set; }

    }
}
