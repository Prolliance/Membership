using AjaxEngine.AjaxHandlers;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Utils;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class ServiceBase : AjaxHandlerBase
    {
        #region Static Member
        private static List<string> IgnoreAuthList = new List<string>{
            "login"
        };
        #endregion

        #region Member
        public string Token
        {
            get
            {
                return this.Context.Request["token"] ?? "";
            }
        }
        #endregion

        #region 全局入口
        protected override object InvokeEntityMethod(string methodName, string httpMethod)
        {
            //如果不是忽略身份验证的方法,并且 Token 验证没有成功
            if (UserAdapter.GetState(this.Token) == null
                && !IgnoreAuthList.Contains(this.InvokeMethodName.ToLower()))
            {
                return new ServiceResult<object>(ServiceState.InvalidToken);
            }
            return base.InvokeEntityMethod(methodName, httpMethod);
        }

        #endregion
    }
}
