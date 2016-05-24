using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServiceClients.Demo.AuthManifests
{
    [App]
    public class AppManifest : AppManifestBase
    {
        [TestManifest]
        public TestManifest Test { get; set; }
    }
}
