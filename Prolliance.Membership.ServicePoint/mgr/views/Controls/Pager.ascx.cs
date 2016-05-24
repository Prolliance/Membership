using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Prolliance.Membership.ServicePoint.Mgr.Views.Controls
{
    public delegate void PagingHandler();

    public partial class Pager : System.Web.UI.UserControl
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize
        {
            get
            {
                return string.IsNullOrEmpty(this.pagesize.Text) ? 0 : Convert.ToInt32(this.pagesize.Text);
            }
            set
            {
                this.pagesize.Text = value.ToString();
            }
        }
        /// <summary>
        /// 记录总数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return string.IsNullOrEmpty(this.totalcount.Text) ? 0 : Convert.ToInt32(this.totalcount.Text);
            }
            set
            {
                this.totalcount.Text = value.ToString();
            }
        }
        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get
            {
                if (string.IsNullOrEmpty(this.curpage.Text)) return 0;
                int icurpage = Convert.ToInt32(this.curpage.Text);
                if (icurpage <= 0) return 0;
                return (icurpage - 1);
            }
            set
            {
                if (value < 0) this.curpage.Text = "0";
                else this.curpage.Text = (value + 1).ToString();
            }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                return string.IsNullOrEmpty(this.totalpages.Text) ? 0 : Convert.ToInt32(this.totalpages.Text);
            }
            private set
            {
                this.totalpages.Text = value.ToString();
            }
        }
        /// <summary>
        /// 翻页事件
        /// </summary>
        public event PagingHandler Paging;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshPager();
            }
        }

        protected void firstpage_Click(object sender, EventArgs e)
        {
            if (PageIndex > 0)
            {
                PageIndex = 0;
                SetPagerEnable();
                Paging();
            }
        }

        protected void previouspage_Click(object sender, EventArgs e)
        {
            if (PageIndex > 0)
            {
                PageIndex -= 1;
                SetPagerEnable();
                Paging();
            }
        }

        protected void nextpage_Click(object sender, EventArgs e)
        {
            if ((PageIndex + 1) < TotalPages)
            {
                PageIndex += 1;
                SetPagerEnable();
                Paging();
            }
        }

        protected void lastpage_Click(object sender, EventArgs e)
        {
            if ((PageIndex + 1) < TotalPages)
            {
                PageIndex = TotalPages - 1;
                SetPagerEnable();
                Paging();
            }
        }

        public void RefreshPager()
        {
            PageIndex = 0;
            PageSize = PageSize > 0 ? PageSize : 20;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            SetPagerEnable();
        }

        private void SetPagerEnable()
        {
            this.firstpage.Enabled = (PageIndex > 0);
            this.previouspage.Enabled = (PageIndex > 0);
            this.nextpage.Enabled = ((PageIndex + 1) < TotalPages);
            this.lastpage.Enabled = ((PageIndex + 1) < TotalPages);
        }
    }
}