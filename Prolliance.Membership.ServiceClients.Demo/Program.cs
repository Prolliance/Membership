using Prolliance.Membership.ServiceClients.Demo.AuthManifests;
using Prolliance.Membership.ServiceClients.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Prolliance.Membership.ServiceClients.Demo
{
    public static class Program
    {
        public static void Main()
        {
            Stopwatch wacth = new Stopwatch();
            wacth.Start();

            /**
             * SDK 使用的第一步需要先初始化。
             * 通过 “服务 URI + 应用标识 + 应用密钥” 初始化 ServiceClient
             */
            ServiceClient.Init(new ServiceOptions
            {
                ServiceUri = "http://192.168.1.63:33221",
                AppKey = "94cfa8de6a5649458171b1ef4dbe9b4d",
                Secret = "081aed8812a04d3ca4651e8eb6ccd9bb"
            });

            /**
             * 当前示例应用的权限清单
             */
            AppManifest manifest = new AppManifest();

            /**
             * 可以直接向 Membership 导入权限清单，也可以在 Control Panel 手工导入
             * 此方法对 Membership 及上层应用，都将消耗更新的资源，一般在应用程启动时调用一次。
             */
            //App.ImportManifest(manifest);
            
            wacth.Restart();

            Organization o1 = new Organization();
            o1.Code = "hehe";
            o1.Name = "呵呵";
            o1.IsActive = true;
            Organization.Save(o1);

            List<Organization> orgList = Organization.GetAllOrganizationList();
            if (orgList != null && orgList.Count > 0)
            {
                Console.WriteLine(string.Format("组织数量: {0}", orgList.Count));
                var org = orgList[0];
                Console.WriteLine(string.Format("组织子级: {0} 下有 {1} 个子组织", org.Name, org.GetChildOrganizationList().Count));
                var userList = org.GetUserList();
                if (userList != null && userList.Count > 0)
                {
                    Console.WriteLine(string.Format("组织用户: {0} 下有 {1} 个用户", org.Name, userList.Count));
                    var theUser = userList[0];
                    Console.WriteLine(string.Format("首个用户: {0}", theUser.Account));
                    Console.WriteLine(string.Format("    用户角色: {0} 个", theUser.GetRoleList().Count));
                    Console.WriteLine(string.Format("    用户部门: {0} 个", theUser.GetOrganizationList().Count));
                    Console.WriteLine(string.Format("    用户职位: {0} 个", theUser.GetPositionList().Count));
                }
                else
                {
                    Console.WriteLine(string.Format("组织用户: {0} 下有 {1} 个用户", org.Name, 0));
                }
            }
            else
            {
                Console.WriteLine(string.Format("组织数量: {0}", 0));
            }
            App.RemoveAllCache();
            orgList = Organization.GetAllOrganizationList();

            List<Position> positionList = Position.GetPositionList();
            Console.WriteLine(string.Format("岗位数量: {0}", positionList.Count));

            User user1 = User.GetUserByAccount("admin");
            if (user1 != null)
            {
                Console.WriteLine(string.Format("获取用户: {0} 成功", user1.Name));
                Console.WriteLine(string.Format("执行表达式: {0}", user1.ParseExpr("<%= this.Name=='admin' %>")));
            }
            else
            {
                Console.WriteLine(string.Format("获取用户: {0} 失败", "admin"));
            }

            /**
             * 登陆时需要实例一个 AuthParameter 作为参数。
             * 适用于 Service 的认证方式有两种:
             * 1. Password : 通过当前用户的凭据（账号和密码）进行认证。
             * 2. Clinet : 仅利用应用凭据进行认证，此方式 Membership 会验证来源 IP。
             * 两种方式在验证时都要求 AppKey 和 Secret
             */
            var parameter = new AuthParameter();
            parameter.Type = AuthType.PASSWORD;
            parameter.Account = "admin";
            parameter.Password = "123456";

            var result = User.GetUserPhotoByAccount("admin", "base64");

            /**
             * 通过 try ... catch 捕获异常信息
             */
            UserState state = null;
            try
            {
                state = User.Login(parameter);
            }
            catch (ServiceErrorException ex)
            {
                var serviceEx = (ServiceErrorException)ex;
                Console.WriteLine(string.Format("服务错误: {0}，{1}", (int)serviceEx.State, serviceEx.Message));
                Console.Read();
            }

            Console.WriteLine(string.Format("登录用时: {0}", wacth.ElapsedMilliseconds));
            Console.WriteLine(string.Format("登陆状态: {0}", state != null));

            User user = state.GetUser();
            Console.WriteLine(string.Format("当前密钥: {0}", state.Token));
            Console.WriteLine(string.Format("用户账号: {0}", user.Account));
            Console.WriteLine(string.Format("用户姓名: {0}", user.Name));
            Console.WriteLine(string.Format("用户邮箱: {0}", user.Email));

            wacth.Restart();

            /**
             * User.CheckPermission 可以传入一个直接构造的 Operation
             * 也可以参照片 SysManifest 建立一个权限清单
             */
            Console.WriteLine(string.Format("鉴权结果: {0}", user.CheckPermission(manifest.Test.Read)));
            Console.WriteLine(string.Format("鉴权用时: {0}", wacth.ElapsedMilliseconds));

            /**
             * 获取用户相关对象
             */
            Console.WriteLine(string.Format("权象操作: {0} 个", user.GetOperationList().Count));
            Console.WriteLine(string.Format("权象对象: {0} 个", user.GetTargetList().Count));
            Console.WriteLine(string.Format("所属组织: {0} 个", user.GetOrganizationList().Count));
            Console.WriteLine(string.Format("所属职位: {0} 个", user.GetPositionList().Count));
            Console.WriteLine(string.Format("汇报职位: {0} 个", user.GetReportToList().Count));
            Console.WriteLine(string.Format("所属角色: {0} 个", user.GetRoleList().Count));

            try
            {
                user.ChangePassword("123456");
            }
            catch (ServiceErrorException ex)
            {
                Console.WriteLine(string.Format("服务错误: {0}，{1}", (int)ex.State, ex.Message));
            }

            wacth.Restart();
            user.Logout();
            Console.WriteLine(string.Format("注销用时: {0}", wacth.ElapsedMilliseconds));

            Console.Read();
        }
    }
}