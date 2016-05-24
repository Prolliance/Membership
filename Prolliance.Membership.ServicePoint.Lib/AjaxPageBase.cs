using System.Web.UI;
using sps = AjaxEngine.AjaxPages.StatePersisters;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class AjaxPageBase : AjaxEngine.AjaxPages.AjaxPageBase
    {
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                return new sps.CachePageStatePersister(this);
            }
        }
        public void GotoUrl(string url)
        {
            this.PageEngine.GotoUrl(url);
        }
        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);
            this.PageEngine.RegisterClientScriptInclude(this.ResolveUrl("~/static/dialog.js"));
        }
    }
}
