using Prolliance.Membership.ServicePoint.Lib;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Prolliance.Membership.ServicePoint.Install
{
    public partial class Index : InstallPageBase
    {
        public override string StepPageUrl
        {
            get
            {
                return "./";
            }
        }
        public override string StepPath
        {
            get
            {
                return "./step/";
            }
        }
        public override Panel StepPanel
        {
            get
            {
                return this.stepArea;
            }
        }
        public override List<string> Steps
        {
            get
            {
                return new List<string> { "agreement", "data-repo", "passport", "account", "init-data", "done" };
            }
        }

    }
}