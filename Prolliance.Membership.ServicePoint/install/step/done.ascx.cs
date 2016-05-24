using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class Done : InstallControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void next_Click(object sender, EventArgs e)
        {
            this.GotoUrl(this.ResolveUrl("~/"));
        }
    }
}