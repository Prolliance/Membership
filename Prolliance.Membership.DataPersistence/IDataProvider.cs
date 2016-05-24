using System.Collections.Generic;

namespace Prolliance.Membership.DataPersistence
{
    /// <summary>
    /// 数据提供程序接口，通过实现该接口，可以将系统签移动任务数据库
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 创建数据库
        /// </summary>
        void CreateDataRepo();

        /// <summary>
        /// 添加一个表模型数据
        /// 因为每个表的主键都是程序生成的，插入数据时，需注意不能出来重复的主键，特别在并发插入时，其实在业务层有做主键检查，但还是需要注意一下。
        /// </summary>
        /// <typeparam name="Info">表模型的类型</typeparam>
        /// <param name="info">表模型对象实例</param>
        /// <returns>表模型对象实例</returns>
        Info Add<Info>(Info info) where Info : class,IModel, new();

        /// <summary>
        /// 读到所有表模型数据
        /// </summary>
        /// <typeparam name="Info">表模型的类型</typeparam>
        /// <returns>表模型对象实例列表</returns>
        List<Info> Read<Info>() where Info : class,IModel, new();

        /// <summary>
        /// 更新一个表模型数据
        /// 因为每个表的主键都是程序生成的，更新数据时，需注意不能出来重复的主键，特别在更新插入时
        /// 注意如果采用 delete -> add 实现 update ，会有并发风险，因为即使用 delete 、add 都有数据库锁
        /// 但是 update 方法没有整体锁，如果用 delete -> add 实现 update ,请为 update 方法用代码加锁。
        /// delete,add —> delete,add -是期望的，delete,delete -> add,add 是错误的，因为 Membeship 中的表主键是程序生成的
        /// 会导到主键重复的异常，即使主键是数据库自动Id 也会导致多余的重复记录
        /// </summary>
        /// <typeparam name="Info">表模型的类型</typeparam>
        /// <param name="info">表模型对象实例</param>
        /// <returns>表模型对象实例</returns>
        Info Update<Info>(Info info) where Info : class,IModel, new();

        /// <summary>
        /// 删除一个表模型数据
        /// </summary>
        /// <typeparam name="Info">表模型的类型</typeparam>
        /// <param name="info">表模型对象实例</param>
        /// <returns>表模型对象实例</returns>
        Info Delete<Info>(Info info) where Info : class,IModel, new();

        /// <summary>
        /// 获取表的结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        Dictionary<string, string> GetTableFields(string tableName);


        /// <summary>
        /// 获取表的结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        string AddField(ExtensionField field);

        /// <summary>
        /// 动态执行SQL
        /// </summary>
        /// <param name="dynamicSql"></param>
        void ExecuteDynamicSql(string dynamicSql);
    }
}
