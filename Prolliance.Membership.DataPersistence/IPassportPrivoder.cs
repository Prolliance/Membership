
using Prolliance.Membership.DataPersistence.Models;
using System.Collections.Generic;
namespace Prolliance.Membership.DataPersistence
{
    public interface IPassportProvider
    {
        /// <summary>
        /// 身份验证
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        bool Validate(string account, string password);

        /*
        //暂不使用这种在业务层就完成验证的方式。
        //因为目前业务层本为开放式的，不必要密码修改再次鉴权，再交由UI或Service层程序控制
        /// <summary>
        /// 重置密码（用于管理员重置基他用户密码）
        /// </summary>
        /// <param name="administrator">账号</param>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        void SetPassword(string adminAccount, string adminPassword, string account, string password);

        /// <summary>
        /// 修改密码（用于用户自行修改密码）
        /// </summary>
        /// <param name="account"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        void ChangePassword(string account, string oldPassword, string newPassword);
        */

        void SetPassword(string account, string password);

        List<UserInfo> LoadUser();

    }
}
