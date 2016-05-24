using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.AuthManifests
{
    [Target(Code = "user", Name = "用户管理", Group = "常规功能")]
    public class UserManifest : TargetManifestBase
    {
        [Operation(Code = "view", Name = "查看")]
        public Operation View { get; set; }

        [Operation(Code = "new", Name = "新建")]
        public Operation New { get; set; }

        [Operation(Code = "add", Name = "添加现有")]
        public Operation Add { get; set; }

        [Operation(Code = "edit", Name = "编辑")]
        public Operation Edit { get; set; }

        [Operation(Code = "delete", Name = "彻底删除")]
        public Operation Delete { get; set; }

        [Operation(Code = "remove", Name = "从组织移除")]
        public Operation Remove { get; set; }

        [Operation(Code = "role", Name = "配置角色")]
        public Operation Role { get; set; }

        [Operation(Code = "password", Name = "重置密码")]
        public Operation Password { get; set; }
    }
}
