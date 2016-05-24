using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib;
using Prolliance.Membership.ServicePoint.Lib.AuthManifests;
using System;

namespace Prolliance.Membership.ServicePoint.Install.Step
{
    public partial class InitData : InstallControlBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void next_Click(object sender, EventArgs e)
        {
#if !DEBUG
            try
            {
#endif
                InstallContext.CreateDataRepo();
                ControlPanelManifest manifest = new ControlPanelManifest();
                InstallContext.InitData(manifest.ExportManifestText());
                this.InstallPage.NextStep();
#if !DEBUG
            }
            catch (Exception ex)
            {
                this.AjaxPage.PageEngine.ShowMessageBox(ex.Message);
                this.AjaxPage.PageEngine.UpdateControlRender(this);
            }
#endif
        }
    }
}