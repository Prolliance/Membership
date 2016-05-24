
namespace Prolliance.Membership.ServicePoint.Lib
{

    public class InstallControlBase : AjaxControlBase
    {
        public InstallPageBase InstallPage
        {
            get
            {
                return (InstallPageBase)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }
    }
}
