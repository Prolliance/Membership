
namespace Prolliance.Membership.DataPersistence
{
    /// <summary>
    /// 实体表模型接口
    /// </summary>
    public interface IModel
    {
        string Id { get; set; }
    }

    /// <summary>
    /// 关系表模型接口
    /// </summary>
    public interface ICorrelate : IModel
    {

    }
}
