using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib;
using System;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class Agreement : InstallControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void next_Click(object sender, EventArgs e)
        {
            InstallContext.Clear();
            this.InstallPage.NextStep();
        }
    }
}