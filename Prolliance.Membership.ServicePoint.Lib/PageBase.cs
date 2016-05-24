using System.Threading.Tasks;
using AjaxEngine.Extends;
using AjaxEngine.Serializes;
using Prolliance.Membership.Business;
using Prolliance.Membership.Install.Lib;
using Prolliance.Membership.ServicePoint.Lib.AuthManifests;
using Prolliance.Membership.ServicePoint.Lib.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using SDK = Prolliance.Membership.ServiceClients.Models;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class PageBase : AjaxPageBase
    {
        #region 成员
        public UserState CurrentState
        {
            get
            {
                return (UserState)Session["user-state"];
            }
            set
            {
                Session["user-state"] = value;
            }
        }
        public User CurrentUser
        {
            get
            {
                if (this.CurrentState == null) return null;
                return Business.User.GetUser(this.CurrentState.Account);
            }
        }
        private ControlPanelManifest _AuthManifest { get; set; }
        public ControlPanelManifest AuthManifest
        {
            get
            {
                if (_AuthManifest == null)
                {
                    _AuthManifest = new ControlPanelManifest();
                }
                return _AuthManifest;
            }
        }
        #endregion

        private JsonSerializer _Serializer { get; set; }
        public JsonSerializer Serializer
        {
            get
            {
                if (_Serializer == null)
                {
                    _Serializer = new JsonSerializer();
                }
                return _Serializer;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!InstallContext.Installed)
            {
                this.PageEngine.GotoUrl(this.ResolveUrl("~/install"));
                return;
            }
            this.VerifyPageAuth();
            base.OnLoad(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.VerifyControlAuth();
            base.Render(writer);
        }

        #region 验证状态及权限
        public bool IgnoreState
        {
            get
            {
                Type type = this.GetType();
                return type.GetAttribute<IgnoreSessionState>() != null;
            }
        }

        private List<SDK.Operation> CalculateSDKOperationList(List<SDK.Operation> srcList)
        {
            List<SDK.Operation> dstList = new List<SDK.Operation>();
            ControlPanelManifest manifest = new ControlPanelManifest();
            var manifestOperationList = manifest.GetOperationList();
            foreach (SDK.Operation operation in srcList)
            {
                operation.TargetCode = operation.TargetCode ?? "*";
                operation.Code = operation.Code ?? "*";
                var bufferList = new List<SDK.Operation>();
                if (operation.TargetCode == "*" && operation.Code == "*")
                {
                    bufferList.AddRange(manifestOperationList);
                }
                else if (operation.TargetCode == "*" && operation.Code != "*")
                {
                    bufferList.AddRange(manifestOperationList
                        .Where(op => op.Code == operation.Code)
                        .ToList());
                }
                else if (operation.TargetCode != "*" && operation.Code == "*")
                {
                    bufferList.AddRange(manifestOperationList
                       .Where(op => op.TargetCode == operation.TargetCode)
                       .ToList());
                }
                else
                {
                    bufferList.Add(manifestOperationList
                        .FirstOrDefault(op => op.TargetCode == operation.TargetCode
                            && op.Code == operation.Code));
                }
                //如果计算结果不存在就把，原 “操作” 放入 buffer
                if (bufferList.Count < 1)
                {
                    bufferList.Add(operation);
                }
                dstList.AddRange(bufferList);
            }
            return dstList;
        }

        private List<SDK.Operation> _SDKOperationList { get; set; }

        public List<SDK.Operation> SDKOperationList
        {
            get
            {
                if (_SDKOperationList == null)
                {
                    Type type = this.GetType();
                    _SDKOperationList = CalculateSDKOperationList(type.GetAttributes<SDK.Operation>());
                }
                return _SDKOperationList;
            }
        }

        protected List<SDK.Operation> GetSDKOperationListFromControl(Control control)
        {
            List<SDK.Operation> sdkOpearationList = new List<SDK.Operation>();
            if (control is IAttributeAccessor)
            {
                IAttributeAccessor attributeAccessor = (IAttributeAccessor)control;
                var list = this.Serializer.Deserialize<List<SDK.Operation>>(attributeAccessor.GetAttribute("data-auth") ?? "[]");
                sdkOpearationList.AddRange(list);
            }
            return CalculateSDKOperationList(sdkOpearationList);
        }

        public bool ApplyAuth(Control control)
        {
            if (control is IAttributeAccessor)
            {
                IAttributeAccessor attributeAccessor = (IAttributeAccessor)control;
                return string.IsNullOrWhiteSpace(attributeAccessor.GetAttribute("data-auth"));
            }
            return false;
        }

        private List<Control> GetDeepControls(Control parent)
        {
            var deepChildControls = new List<Control>();
            foreach (Control childControl in parent.Controls)
            {
                deepChildControls.Add(childControl);
                deepChildControls.AddRange(GetDeepControls(childControl));
            }
            return deepChildControls;
        }

        public List<Control> DeepControls
        {
            get
            {
                return this.GetDeepControls(this.Form);
            }
        }

        public void VerifyControlAuth()
        {
            var controls = this.DeepControls;
            //Parallel.ForEach(controls, (control) =>
            //{
            //    var controlSDKOperationList = this.GetSDKOperationListFromControl(control);
            //    //控件鉴权
            //    if (controlSDKOperationList == null || controlSDKOperationList.Count <= 0)
            //    {
            //        return;
            //    }
            //    control.Visible = false;
            //    foreach (SDK.Operation sdkOperation in controlSDKOperationList)
            //    {
            //        if (this.CurrentUser != null && this.CurrentUser.CheckPermissionBySDK(sdkOperation))
            //        {
            //            control.Visible = true;
            //            return;
            //        }
            //    }
            //});
            var currentUser = this.CurrentUser;
            if (currentUser == null)
            {
                return;
            }
            var userOpCodeList = currentUser.OperationList;
            foreach (Control control in controls)
            {
                var controlSDKOperationList = this.GetSDKOperationListFromControl(control);
                //控件鉴权
                if (controlSDKOperationList == null || controlSDKOperationList.Count <= 0)
                {
                    continue;
                }
                control.Visible = false;
                foreach (SDK.Operation sdkOperation in controlSDKOperationList)
                {
                    if (this.CurrentUser != null && userOpCodeList.Any(p => p.Code == sdkOperation.Code))
                    {
                        control.Visible = true;
                        break;
                    }
                    //if (this.CurrentUser != null && this.CurrentUser.CheckPermissionBySDK(sdkOperation))
                    //{
                    //    control.Visible = true;
                    //    break;
                    //}
                }
            }

            //foreach (Control control in controls)
            //{
            //    var controlSDKOperationList = this.GetSDKOperationListFromControl(control);
            //    //控件鉴权
            //    if (controlSDKOperationList == null || controlSDKOperationList.Count <= 0)
            //    {
            //        continue;
            //    }
            //    control.Visible = false;
            //    foreach (SDK.Operation sdkOperation in controlSDKOperationList)
            //    {
            //        if (this.CurrentUser != null && this.CurrentUser.CheckPermissionBySDK(sdkOperation))
            //        {
            //            control.Visible = true;
            //            break;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 权限验证，采用 “除非明确授予否则拒绝的策略”
        /// </summary>
        public void VerifyPageAuth()
        {
            Type type = this.GetType();
            //检查是否忽略状态
            if (this.IgnoreState)
            {
                return;
            }
            //检查状态
            if (!this.IgnoreState && this.CurrentUser == null)
            {
                this.GotoLogin();
                return;
            }
            //鉴权
            if (this.SDKOperationList != null)
            {
                foreach (SDK.Operation sdkOperation in this.SDKOperationList)
                {
                    if (this.CurrentUser.CheckPermissionBySDK(sdkOperation))
                    {
                        return;
                    }
                }
            }
            this.GotoUnAuth();
        }
        #endregion

        #region 常用方法
        public void GotoLogin()
        {
            this.PageEngine.GotoUrl(this.ResolveUrl("~/mgr/login.aspx"));
        }
        public void GotoError()
        {
            this.PageEngine.GotoUrl(this.ResolveUrl("~/error.aspx"));
        }
        public void GotoUnAuth()
        {
            this.PageEngine.GotoUrl(this.ResolveUrl("~/unauth.aspx"));
        }
        public void GotoHome()
        {
            this.PageEngine.GotoUrl(this.ResolveUrl("~/mgr/frame.aspx"));
        }
        public void FillForm(object model)
        {
            FormHelper.FillForm(this.Form, model);
        }
        public void FillModel(object model)
        {
            FormHelper.FillModel(model, this.Form);
        }
        #endregion

    }
}