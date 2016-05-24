using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.AuthManifests
{
    [Target(Code = "role", Name = "角色管理", Group = "常规功能")]
    public class RoleManifest : TargetManifestBase
    {
        [Operation(Code = "view", Name = "查看")]
        public Operation View { get; set; }

        [Operation(Code = "new", Name = "新建")]
        public Operation New { get; set; }

        [Operation(Code = "edit", Name = "编辑")]
        public Operation Edit { get; set; }

        [Operation(Code = "delete", Name = "删除")]
        public Operation Delete { get; set; }

        [Operation(Code = "auth-config", Name = "配置权限")]
        public Operation AuthConfig { get; set; }

        [Operation(Code = "sys-auth-config", Name = "配置 Control Panel 权限")]
        public Operation SystemAuthConfig { get; set; }

        [Operation(Code = "mutex", Name = "互斥关系")]
        public Operation Mutex { get; set; }
    }
}
