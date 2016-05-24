using AjaxEngine.Utils;
using Prolliance.Membership.DataTransfer;
using Prolliance.Membership.DataTransfer.Models;
using Prolliance.Membership.DataTransfer.Utils;
using Prolliance.Membership.ServicePoint.Lib.Services;
using System.Collections.Generic;

namespace Prolliance.Membership.ServicePoint.Service
{
    /// <summary>
    /// Postion 的摘要说明
    /// </summary>
    [Summary(Name = "职位服务", Description = "职位相关 API，可以用户或应用身份访问。")]
    [ServiceAuth(Type = ServiceAuthType.AppOrToken)]
    public class PositionService : ServiceBase
    {
        [Summary(Description = "获取职位列表")]
        [AjaxMethod]
        public ServiceResult<List<PositionInfo>> GetPositionList()
        {
            return new ServiceResult<List<PositionInfo>>(PositionAdapter.GetPositionList());
        }

        [Summary(Description = "获取当前职位汇报链的上级职位列表")]
        [AjaxMethod]
        public ServiceResult<List<PositionInfo>> GetReportToListById(string positionId)
        {
            return new ServiceResult<List<PositionInfo>>(PositionAdapter.GetReportToListById(positionId));
        }

        [Summary(Description = "获取汇报关系")]
        [AjaxMethod]
        public ServiceResult<List<PositionInfo>> ReportToList(string positionId)
        {
            return new ServiceResult<List<PositionInfo>>(PositionAdapter.ReportToList(positionId));
        }

        [Summary(Description = "移除汇报关系")]
        [AjaxMethod]
        public ServiceResult<PositionInfo> RemoveReportTo(string positionId, string toPositionId)
        {
            return new ServiceResult<PositionInfo>(PositionAdapter.RemoveReportTo(positionId, toPositionId));
        }

        [Summary(Description = "添加汇报关系")]
        [AjaxMethod]
        public ServiceResult<PositionInfo> AddReportTo(string positionId, string toPositionId)
        {
            return new ServiceResult<PositionInfo>(PositionAdapter.AddReportTo(positionId, toPositionId));
        }

        [Summary(Description = "获取职位下用户列表")]
        [AjaxMethod]
        public ServiceResult<List<UserInfo>> GetUserList(string positionId)
        {
            return new ServiceResult<List<UserInfo>>(PositionAdapter.GetUserList(positionId));
        }

        [Summary(Description = "通过 id 获取一个职位")]
        [AjaxMethod]
        public static ServiceResult<PositionInfo> GetPositionById(string positionId)
        {
            return new ServiceResult<PositionInfo>(PositionAdapter.GetPositionById(positionId));
        }

        [Summary(Description = "通过组织代码、职位代码获取一个职位")]
        [AjaxMethod]
        public static ServiceResult<PositionInfo> GetPosition(string organizationCode, string positionCode)
        {
            return new ServiceResult<PositionInfo>(PositionAdapter.GetPosition(organizationCode, positionCode));
        }

        [Summary(Description = "在职位下面添加人员")]
        [AjaxMethod]
        public static ServiceResult<UserInfo> AddUser(string positionId, UserInfo user)
        {
            return new ServiceResult<UserInfo>(PositionAdapter.AddUser(positionId, user));
        }

        [Summary(Description = "在职位下面移除人员")]
        [AjaxMethod]
        public static ServiceResult<UserInfo> RemoveUser(string positionId, UserInfo user)
        {
            return new ServiceResult<UserInfo>(PositionAdapter.RemoveUser(positionId, user));
        }

        [Summary(Description = "保存组织机构")]
        [AjaxMethod]
        public static ServiceResult<object> Save(PositionInfo positionInfo)
        {
            PositionAdapter.Save(positionInfo);
            return new ServiceResult<object>();
        }

        [Summary(Description = "删除组织机构")]
        [AjaxMethod]
        public static ServiceResult<object> Delete(string positionId)
        {
            PositionAdapter.Delete(positionId);
            return new ServiceResult<object>();
        }

        [Summary(Description = "授权")]
        [AjaxMethod]
        public static ServiceResult<RoleInfo> GiveRole(string positionId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(PositionAdapter.GiveRole(positionId,roleInfo));
        }

        [Summary(Description = "取消授权")]
        [AjaxMethod]
        public static ServiceResult<RoleInfo> CancelRole(string positionId, RoleInfo roleInfo)
        {
            return new ServiceResult<RoleInfo>(PositionAdapter.CancelRole(positionId, roleInfo));
        }
    }
}