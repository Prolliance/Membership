using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.AuthManifests
{
    [Target(Code = "position", Name = "职位管理", Group = "常规功能")]
    public class PositionManifest : TargetManifestBase
    {
        [Operation(Code = "view", Name = "查看")]
        public Operation View { get; set; }

        [Operation(Code = "new", Name = "新建")]
        public Operation New { get; set; }

        [Operation(Code = "edit", Name = "编辑")]
        public Operation Edit { get; set; }

        [Operation(Code = "delete", Name = "删除")]
        public Operation Delete { get; set; }

        [Operation(Code = "role", Name = "配置角色")]
        public Operation Role { get; set; }

        [Operation(Code = "report", Name = "汇报关系")]
        public Operation Report { get; set; }
    }
}
