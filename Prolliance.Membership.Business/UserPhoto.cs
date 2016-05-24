using Prolliance.Membership.DataPersistence;
using Prolliance.Membership.DataPersistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prolliance.Membership.Business.Utils;
using Prolliance.Membership.Common;

namespace Prolliance.Membership.Business
{
    /// <summary>
    /// 用户照片类
    /// </summary>
    public class UserPhoto : ModelBase
    {
        internal static DataRepo<UserPhotoInfo> UserPhotoRepo = new DataRepo<UserPhotoInfo>();

        /// <summary>
        /// 用户帐号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 照片Binary
        /// </summary>
        public byte[] PhotoBinary { get; set; }

        /// <summary>
        /// 照片扩展名
        /// </summary>
        public string PhotoExt { get; set; }

     

        /// <summary>
        /// 通过帐号获取照片
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static UserPhoto GetUserPhotoByAccount(string account)
        {
            return UserPhotoRepo.Read().FirstOrDefault(u => u.Account == account).MappingTo<UserPhoto>();
        }

        public static UserPhoto Create()
        {
            UserPhoto userPhoto = new UserPhoto();
            return userPhoto;
        }

        public void Save()
        {
            UserPhotoRepo.Save(this.MappingTo<UserPhotoInfo>());
        }
    }
}
