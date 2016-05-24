using Prolliance.Membership.Common;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib.Extends
{
    public static class UserExtends
    {
        public static bool CheckPermissionBySDK(this Business.User user, SDK.Operation sdkOperation)
        {
            if (user == null || sdkOperation == null) return false;
            sdkOperation.AppKey = AppSettings.Name;
            Business.Operation operation = Business.Operation.GetOperation(sdkOperation.AppKey, sdkOperation.TargetCode, sdkOperation.Code);
            if (operation == null)
                return false;
            else
                return user.CheckPermission(operation);
        }
    }
}
