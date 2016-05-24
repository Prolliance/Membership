using System.Web.UI;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class AjaxControlBase : UserControl
    {
        public AjaxPageBase AjaxPage
        {
            get
            {
                return (AjaxPageBase)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }
        public void GotoUrl(string url)
        {
            this.AjaxPage.GotoUrl(url);
        }
    }
}
