using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class InstallPageBase : AjaxPageBase
    {
        public virtual List<string> Steps
        {
            get
            {
                return new List<string>();
            }
        }
        public virtual string CurrentStep
        {
            get
            {
                return this.Request["step"];
            }
        }
        public virtual string StepPath
        {
            get
            {
                return "";
            }
        }
        public virtual Panel StepPanel
        {
            get
            {
                return null;
            }
        }
        public virtual void GoStep(string step)
        {
            this.GotoUrl(string.Format("{0}?step={1}", this.StepPageUrl, step));
        }
        public virtual string StepPageUrl
        {
            get
            {
                return this.Request.Url.ToString().Split('?')[0];
            }
        }
        public virtual void FirstStep()
        {
            this.GoStep(this.Steps[0]);
        }
        public virtual void NextStep()
        {
            var currentIndex = this.Steps.IndexOf(this.CurrentStep);
            var nextIndex = currentIndex + 1;
            if (nextIndex >= this.Steps.Count - 1) nextIndex = this.Steps.Count - 1;
            var nextStep = this.Steps[nextIndex];
            this.GoStep(nextStep);
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var ip = this.Request.UserHostAddress;
            if (ip != "localhost" && ip != "127.0.0.1" && ip != "::1")
            {
                this.Response.Write(string.Format("禁止从 \"{0}\" 访问", ip));
                this.Response.End();
            }
            if (this.Steps == null || this.Steps.Count < 1)
            {
                throw new Exception("没有找到任何安装步骤");
            }
            if (!string.IsNullOrWhiteSpace(this.CurrentStep))
            {
                Control stepControl = this.LoadControl(this.ResolveUrl(string.Format("{0}{1}.ascx", this.StepPath, this.CurrentStep)));
                this.StepPanel.Controls.Add(stepControl);
            }
            else
            {
                this.FirstStep();
            }
        }

    }
}
