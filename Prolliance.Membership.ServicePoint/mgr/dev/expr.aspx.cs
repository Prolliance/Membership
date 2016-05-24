using Prolliance.Membership.Business.Utils;
using Prolliance.Membership.ServicePoint.Lib;
using System;
using System.Diagnostics;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Mgr.Dev
{
    [SDK.Operation(TargetCode = "*", Code = "*")]
    public partial class Expr : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnExec_Click(object sender, EventArgs e)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            try
            {
                this.boxResult.Text = ExpressionCalculator.Calculate(this.boxExpr.Text, this.CurrentUser);
            }
            catch (Exception ex)
            {
                this.boxResult.Text = ex.Message;
            }
            this.lblTime.Text = string.Format("用时: {0}ms", watch.ElapsedMilliseconds);
            watch.Stop();
            this.PageEngine.UpdateControlRender(this.boxResult);
            this.PageEngine.UpdateControlRender(this.lblTime);
        }

        protected void btnClearSpace_Click(object sender, EventArgs e)
        {
            this.boxExpr.Text = this.boxExpr.Text.Replace("\n", " ").Replace("\r", "");
            this.PageEngine.UpdateControlRender(this.boxExpr);
        }
    }
}