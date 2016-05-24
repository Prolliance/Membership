using Prolliance.Membership.Common;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 业务模型抽象基类（用来统一生成 Id）
    /// </summary>
    public abstract class ModelBase
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ModelBase()
        {
            this.Id = StringFactory.NewGuid();
        }

        /// <summary>
        /// 对象Id
        /// </summary>
        public string Id { get; set; }
    }
}
