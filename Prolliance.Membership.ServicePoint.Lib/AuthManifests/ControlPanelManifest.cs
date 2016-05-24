using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.AuthManifests
{
    [App(Key = "Membership")]
    public class ControlPanelManifest : AppManifestBase
    {
        [AppManifest]
        public AppManifest App { get; set; }

        [RoleManifest]
        public RoleManifest Role { get; set; }

        [OrganizationManifest]
        public OrganizationManifest Organzation { get; set; }

        [PositionManifest]
        public PositionManifest Position { get; set; }

        [UserManifest]
        public UserManifest User { get; set; }

        [ExtensionsManifest]
        public ExtensionsManifest Ext { get; set; }
    }
}
