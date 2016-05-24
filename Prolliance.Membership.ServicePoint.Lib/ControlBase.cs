
namespace Prolliance.Membership.ServicePoint.Lib
{
    public class ControlBase : AjaxControlBase
    {
        public PageBase CurrentPage
        {
            get
            {
                return (PageBase)base.Page;
            }
            set
            {
                base.Page = value;
            }
        }
    }
}
