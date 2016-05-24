using AjaxEngine.Utils;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Models;
using Prolliance.Membership.DataTransfer.Utils;
using Prolliance.Membership.ServicePoint.Lib.Services;
using System;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.Service
{
    /**
     * TODO: 未来视需求可调整
     * 目前 User 服务下，大部份方法只支持针对当前登录用户
     * 如果将来要支持，非常当前登录用户，相关方法需要将通过 Token 拿到用户对象调整为通过请求方传递过来的 UserId
     * 界时需要为要调整的相关方法添加 userId 参数，当然也可以支持用 accout 的方法
     */
    /// <summary>
    /// Service 的摘要说明
    /// </summary>
    [ServiceAuth(Type = ServiceAuthType.AppOrToken, IgnoreMethods = new string[] { "Login", "GetUserPhotoByAccount" })]
    [Summary(Name = "用户服务", Description = "用户相关 API，必须以某一用户的身份访问。")]
    public class UserService : ServiceBase
    {
        [AjaxMethod]
        [Summary(Description = "获取所有用户的列表")]
        public ServiceResult<List<UserInfo>> GetAllUserList()
        {
            return new ServiceResult<List<UserInfo>>(UserAdapter.GetUserList(this.Token));
        }

        [AjaxMethod]
        [Summary(Description = "获取所有用户的列表")]
        public ServiceResult<List<UserInfo>> GetUserListOfOrganization()
        {
            return new ServiceResult<List<UserInfo>>(UserAdapter.GetUserListOfOrganization(this.Token));
        }

        [Summary(Description = "通过组织结构编码范围，获取某用户的部门", Parameters = "userAccount:用户账号,orgcode:组织 Id")]
        [AjaxMethod]
        public ServiceResult<List<OrganizationInfo>> GetRangeOrganization(string userAccount, string orgcode)
        {
            return new ServiceResult<List<OrganizationInfo>>(OrganizationAdapter.GetRangeOrganization(userAccount, orgcode));
        }

        [Summary(Description = "获取用户照片", Parameters="account:用户帐号，必传，format:返回值类型base64，默认直接返回图片")]
        [AjaxMethod]
        public ServiceResult<UserPhotoInfo> GetUserPhotoByAccount(string account, string format)
        {
            var userPhoto = UserAdapter.GetUserPhotoByAccount(account);
            if (userPhoto == null)
            {
                userPhoto = new UserPhotoInfo() { Account = account, PhotoExt = ".jpg" };
                string imagePath = this.Context.Server.MapPath("..\\static\\defaultuser.jpg");
                System.IO.FileStream stream = new System.IO.FileStream(imagePath, System.IO.FileMode.Open);
                userPhoto.PhotoBinary = new byte[stream.Length];
                stream.Read(userPhoto.PhotoBinary, 0, (int)stream.Length);
                stream.Close();
            }

            if (format.ToUpper() == "BASE64")
            {
                return new ServiceResult<UserPhotoInfo>(userPhoto);
            }
            else
            {
                byte[] image_bytes = userPhoto.PhotoBinary;
                string image_ext = userPhoto.PhotoExt;

                if(format.IndexOf("*")>-1)
                {
                    int newW = Convert.ToInt32(format.Split('*')[0]);
                    int newH = Convert.ToInt32(format.Split('*')[1]);
                    System.IO.MemoryStream msSource = new System.IO.MemoryStream(image_bytes);
                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromStream(msSource);
                    msSource.Close();

                    System.Drawing.Bitmap bpmTarget = new System.Drawing.Bitmap(newW, newH);
                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bpmTarget);   
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; 
                    g.DrawImage(bmp, new System.Drawing.Rectangle(0, 0, newW, newH), new System.Drawing.Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.GraphicsUnit.Pixel);  
                    g.Dispose();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    bpmTarget.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    image_bytes = ms.ToArray();
                    ms.Close();
                }

                this.Context.Response.ContentType = image_ext == ".jpg" ? "image/jpeg" : image_ext == "gif" ? "image/gif" : "image/x-png";
                this.Context.Response.BinaryWrite(image_bytes);
                this.Context.Response.End();
            }
            return new ServiceResult<UserPhotoInfo>(null);
        }

        #region 针对当前会话状态用户的方法
        [AjaxMethod]
        [Summary(Description = "登录", Parameters = "parameter:登录参数")]
        public ServiceResult<UserStateInfo> Login(AuthParameterInfo parameter)
        {
            parameter.AppIp = Context.Request.UserHostAddress;
            parameter.DeviceId = parameter.DeviceId ?? parameter.Ip;
            UserStateInfo state = UserAdapter.Login(parameter);
            if (state != null)
            {
                return new ServiceResult<UserStateInfo>(state);
            }
            else
            {
                return new ServiceResult<UserStateInfo>(ServiceState.InvalidUserCredentials);
            }
        }

        [AjaxMethod]
        [Summary(Description = "登出")]
        public ServiceResult<object> Logout()
        {
            UserAdapter.Logout(this.Token);
            return new ServiceResult<object>();
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户状态")]
        public ServiceResult<UserStateInfo> GetState()
        {
            return new ServiceResult<UserStateInfo>(UserAdapter.GetState(this.Token));
        }

        [Summary(Description = "获取人员扩展列")]
        [AjaxMethod]
        public ServiceResult<Dictionary<string, string>> GetTableColumns()
        {
            return new ServiceResult<Dictionary<string, string>>(UserAdapter.GetTableColumns());
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户")]
        public ServiceResult<UserInfo> GetUser()
        {
            return new ServiceResult<UserInfo>(UserAdapter.GetUser(this.Token));
        }

        [AjaxMethod]
        [Summary(Description = "检查当前用户权限", Parameters = "operation:权限操作结构体")]
        public ServiceResult<bool> CheckPermission(OperationInfo operation)
        {
            if (operation == null) return new ServiceResult<bool>(false);
            operation.AppKey = operation.AppKey ?? this.AppKey;
            return new ServiceResult<bool>(UserAdapter.CheckPermission(this.Token, operation));
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户的权限对象列表")]
        public ServiceResult<List<TargetInfo>> GetTargetList()
        {
            return new ServiceResult<List<TargetInfo>>(UserAdapter.GetTargetList(this.Token));
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户的权限操作列表")]
        public ServiceResult<List<OperationInfo>> GetOperationList()
        {
            return new ServiceResult<List<OperationInfo>>(UserAdapter.GetOperationList(this.Token));
        }

        [AjaxMethod]
        [Summary(Description = "更改密码")]
        public ServiceResult<object> ChangePassword(string token, string newPassword)
        {
            UserAdapter.ChangePassword(this.Token, newPassword);
            return new ServiceResult<object>(null);
        }

      

        [Summary(Description = "创建用户")]
        [AjaxMethod]
        public ServiceResult<object> CreateUser(UserInfo user)
        {
            UserAdapter.CreateUser(user);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "保存用户")]
        [AjaxMethod]
        public static ServiceResult<object> Save(UserInfo userInfo)
        {
            UserAdapter.Save(userInfo);
            return new ServiceResult<object>();
        }

        [Summary(Description = "删除用户")]
        [AjaxMethod]
        public static ServiceResult<object> Delete(string userId)
        {
            UserAdapter.Delete(userId);
            return new ServiceResult<object>();
        }

        [Summary(Description = "同步用户")]
        [AjaxMethod]
        public static ServiceResult<object> LoadUserRemoteUser()
        {
            UserAdapter.LoadUserRemoteUser();
            return new ServiceResult<object>();
        }
        #endregion

        #region 针对所有用户实例的方法
        [AjaxMethod]
        [Summary(Description = "获取当前用户的角色列表")]
        public ServiceResult<List<RoleInfo>> GetRoleList(string userId)
        {
            return new ServiceResult<List<RoleInfo>>(UserAdapter.GetRoleListByUserId(userId));
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户的职位列表")]
        public ServiceResult<List<PositionInfo>> GetPositionList(string userId)
        {
            return new ServiceResult<List<PositionInfo>>(UserAdapter.GetPositionListByUserId(userId));
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户汇报上级职位列表")]
        public ServiceResult<List<PositionInfo>> GetReportToList(string userId)
        {
            return new ServiceResult<List<PositionInfo>>(UserAdapter.GetReportToListByUserId(userId));
        }

        [AjaxMethod]
        [Summary(Description = "获取当前用户汇报上级职位列表")]
        public ServiceResult<List<UserInfo>> GetReportToUsers(string userId)
        {
            return new ServiceResult<List<UserInfo>>(UserAdapter.GetReportToUsers(userId));
        }


        [AjaxMethod]
        [Summary(Description = "获取当前用户所属组织列表")]
        public ServiceResult<List<OrganizationInfo>> GetOrganizationList(string userId)
        {
            return new ServiceResult<List<OrganizationInfo>>(UserAdapter.GetOrganizationListByUserId(userId));
        }

        [AjaxMethod]
        [Summary(Description = "通过id获取用户")]
        public ServiceResult<UserInfo> GetUserById(string userId)
        {
            return new ServiceResult<UserInfo>(UserAdapter.GetUserById(userId));
        }

        [AjaxMethod]
        [Summary(Description = "通过账号获取一个用户")]
        public ServiceResult<UserInfo> GetUserByAccount(string account)
        {
            return new ServiceResult<UserInfo>(UserAdapter.GetUserByAccount(account));
        }

        [AjaxMethod]
        [Summary(Description = "以当前用户身份解释一段表达式")]
        public ServiceResult<string> ParseExprByUserId(string userId, string expr)
        {
            return new ServiceResult<string>(UserAdapter.ParseExprByUserId(userId, expr));
        }

        [Summary(Description = "重置用户密码")]
        [AjaxMethod]
        public ServiceResult<object> ResetPassword(string account, string newPassword)
        {
            UserAdapter.ResetPassword(account, newPassword);
            return new ServiceResult<object>(null);
        }

        [Summary(Description = "授权")]
        [AjaxMethod]
        public ServiceResult<RoleInfo> GiveRole(string userId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(UserAdapter.GiveRole(userId, roleInfo));
        }

        [Summary(Description = "取消授权")]
        [AjaxMethod]
        public ServiceResult<RoleInfo> CancelRole(string userId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(UserAdapter.CancelRole(userId, roleInfo));
        }
        #endregion
    }
}