using AjaxEngine.AjaxHandlers;
using AjaxEngine.Extends;
using Prolliance.Membership.Common;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Utils;
using System;
using System.Linq;

namespace Prolliance.Membership.ServicePoint.Lib.Services
{
    public class ServiceBase : AjaxHandlerBase
    {
        #region 公共参数
        public string Token
        {
            get
            {
                return this.Context.Request["token"] ?? "";
            }
        }
        public string AppKey
        {
            get
            {
                return this.Context.Request["appKey"] ?? "";
            }
        }
        public string Secret
        {
            get
            {
                return this.Context.Request["secret"] ?? "";
            }
        }
        private Logger _Logger = null;
        public Logger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    _Logger = new Logger(AppSettings.LogPath);
                }
                return _Logger;
            }
        }
        #endregion

        #region 全局入口
        protected override object InvokeEntityMethod(string methodName, string httpMethod)
        {
            ServiceAuth serviceAuth = this.GetType().GetAttribute<ServiceAuth>();
            if (serviceAuth == null)
            {
                return new ServiceResult<object>(ServiceState.Error);
            }
            if (serviceAuth.IgnoreMethods == null
                || !serviceAuth.IgnoreMethods.Contains(methodName))
            {
                if (serviceAuth.Type == ServiceAuthType.Token
                    && UserAdapter.GetState(this.Token) == null)
                {
                    return new ServiceResult<object>(ServiceState.InvalidToken);
                }
                else if (serviceAuth.Type == ServiceAuthType.App
                   && !AppAdapter.CheckApp(this.AppKey, this.Secret))
                {
                    return new ServiceResult<object>(ServiceState.InvalidAppCredentials);
                }
                else if (serviceAuth.Type == ServiceAuthType.AppAndToken)
                {
                    if (!AppAdapter.CheckApp(this.AppKey, this.Secret))
                    {
                        return new ServiceResult<object>(ServiceState.InvalidAppCredentials);
                    }
                    if (UserAdapter.GetState(this.Token) == null)
                    {
                        return new ServiceResult<object>(ServiceState.InvalidToken);
                    }
                }
                else if (serviceAuth.Type == ServiceAuthType.AppOrToken)
                {
                    if (!AppAdapter.CheckApp(this.AppKey, this.Secret)
                        && UserAdapter.GetState(this.Token) == null)
                    {
                        return new ServiceResult<object>(ServiceState.InvalidAppCredentials);
                    }
                }
            }
#if !DEBUG
            try
            {
#endif
                return base.InvokeEntityMethod(methodName, httpMethod);
#if !DEBUG
            }
            catch (Exception ex)
            {
                this.Logger.Error(ex);
                return new ServiceResult<object>(ServiceState.Error);
            }
#endif
        }

        #endregion
    }
}
