using Prolliance.Membership.ServiceClients.Manifests;
using Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServiceClients.Demo.AuthManifests
{
    /// <summary>
    /// 每一个权限清单类，对应一个 “Target (权限许可对象)”
    /// 使用权限清单类，可以让编码是更舒服，重构也更加方便
    /// 但是对于 “会动态增加或减少的权限对象” 还需要在通过 “构造 Operation 的方式”
    /// 权限清单类需要继承抽象基类 “AuthManifestBase”
    /// </summary>
    [Target(Name = "测试功能点", Code = "test", Group = "测试分组")]
    public class TestManifest : TargetManifestBase
    {
        [Operation(Name = "读取", Code = "read")]
        public Operation Read { get; set; }

        [Operation(Name = "写入", Code = "write")]
        public Operation Write { get; set; }
    }
}
