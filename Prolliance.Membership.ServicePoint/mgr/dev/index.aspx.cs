using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Web.UI;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Dev
{
    [SDK.Operation(TargetCode = "*", Code = "*")]
    public partial class Index : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}