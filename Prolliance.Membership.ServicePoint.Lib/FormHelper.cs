/*
 * 版本: 0.1
 * 描述: 表单辅助类。
 * 创建: Houfeng
 * 邮件: houzf@prolliance.cn
 * 
 * 修改记录:
 * 2011-11-7,Houfeng,添加文件说明，更新版本号为0.1
 */

using AjaxEngine.Extends;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using WebUI = System.Web.UI;

namespace Prolliance.Membership.ServicePoint.Lib
{
    public class FormHelper
    {
        #region Web
        public static string IdFormat = "ctl_{0}";

        /// <summary>
        /// 填充model
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="control"></param>
        public static void FillModel(Object entity, WebUI.Control control)
        {
            if (entity == null || control == null)
                return;
            NameValueCollection formData = HttpContext.Current.Request.Form;
            PropertyInfo[] propertyList = entity.GetProperties();
            foreach (PropertyInfo pi in propertyList)
            {
                string ctlId = string.Format(IdFormat, pi.Name);
                WebUI.Control ctl = control.FindControl(ctlId);
                if (ctl == null)
                {
                    #region 处理HMTL标签
                    if (formData[ctlId] != null)
                        entity.SetPropertyValue(pi.Name, formData[ctlId]);
                    #endregion
                    continue;
                }
                Type ctlType = ctl.GetType();

                #region 处理服务器控件
                if (ctlType == typeof(WebUI.WebControls.TextBox))//文本框
                {
                    entity.SetPropertyValue(pi.Name, ((WebUI.WebControls.TextBox)ctl).Text);
                }
                else if (ctlType == typeof(WebUI.WebControls.Image))//图片
                {
                    entity.SetPropertyValue(pi.Name, ((WebUI.WebControls.Image)ctl).ImageUrl);
                }
                else if (ctlType == typeof(WebUI.WebControls.DropDownList))//选择框
                {
                    entity.SetPropertyValue(pi.Name, ((WebUI.WebControls.DropDownList)ctl).SelectedValue);
                }
                else if (ctlType == typeof(WebUI.WebControls.HiddenField))//隐藏域
                {
                    entity.SetPropertyValue(pi.Name, ((WebUI.WebControls.HiddenField)ctl).Value);
                }
                else if (ctlType == typeof(WebUI.WebControls.RadioButton))//单选框
                {
                    WebUI.WebControls.RadioButton rb = (WebUI.WebControls.RadioButton)ctl;

                    if (rb.Checked)
                        entity.SetPropertyValue(pi.Name, rb.Text);
                    else
                        entity.SetPropertyValue(pi.Name, "");
                }
                else if (ctlType == typeof(WebUI.WebControls.CheckBox))//复选框
                {
                    WebUI.WebControls.CheckBox ck = (WebUI.WebControls.CheckBox)ctl;
                    if (ck.Checked)
                        entity.SetPropertyValue(pi.Name, ck.Text);
                    else
                        entity.SetPropertyValue(pi.Name, "");
                }
                else if (ctlType == typeof(WebUI.WebControls.CheckBoxList))//复选框列表
                {
                    WebUI.WebControls.CheckBoxList ck = (WebUI.WebControls.CheckBoxList)ctl;
                    string rs = "";
                    foreach (WebUI.WebControls.ListItem li in ck.Items)
                    {
                        if (li.Selected)
                            rs += "," + li.Value;
                    }
                    if (rs.Length > 1)
                    {
                        rs = rs.Substring(1);
                    }
                    entity.SetPropertyValue(pi.Name, rs);
                }
                else if (ctlType == typeof(WebUI.WebControls.RadioButtonList))//单框列表
                {
                    WebUI.WebControls.RadioButtonList ck = (WebUI.WebControls.RadioButtonList)ctl;
                    entity.SetPropertyValue(pi.Name, ck.SelectedValue);
                }
                else if (ctlType == typeof(WebUI.WebControls.ListBox))//列表框
                {
                    WebUI.WebControls.ListBox ck = (WebUI.WebControls.ListBox)ctl;
                    string rs = "";
                    foreach (WebUI.WebControls.ListItem li in ck.Items)
                    {
                        if (li.Selected)
                            rs += "," + li.Value;
                    }
                    if (rs.Length > 1)
                    {
                        rs = rs.Substring(1);
                    }
                    entity.SetPropertyValue(pi.Name, rs);
                }
                #endregion
                #region 处理不同Html控件
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputText))//文本域
                {
                    WebUI.HtmlControls.HtmlInputText ct = (WebUI.HtmlControls.HtmlInputText)ctl;
                    entity.SetPropertyValue(pi.Name, ct.Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlTextArea))//文本域
                {
                    WebUI.HtmlControls.HtmlTextArea ct = (WebUI.HtmlControls.HtmlTextArea)ctl;
                    entity.SetPropertyValue(pi.Name, ct.Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlSelect))//选择域
                {
                    WebUI.HtmlControls.HtmlSelect ct = (WebUI.HtmlControls.HtmlSelect)ctl;
                    entity.SetPropertyValue(pi.Name, ct.Items[ct.SelectedIndex].Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputHidden))////隐藏域
                {
                    WebUI.HtmlControls.HtmlInputHidden ct = (WebUI.HtmlControls.HtmlInputHidden)ctl;
                    entity.SetPropertyValue(pi.Name, ct.Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputRadioButton))//单选域
                {
                    WebUI.HtmlControls.HtmlInputRadioButton rb = (WebUI.HtmlControls.HtmlInputRadioButton)ctl;
                    if (rb.Checked)
                        entity.SetPropertyValue(pi.Name, rb.Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputCheckBox))//复选域
                {
                    WebUI.HtmlControls.HtmlInputCheckBox ck = (WebUI.HtmlControls.HtmlInputCheckBox)ctl;
                    if (ck.Checked)
                        entity.SetPropertyValue(pi.Name, ck.Value);
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputPassword))//密码域
                {
                    WebUI.HtmlControls.HtmlInputPassword ck = (WebUI.HtmlControls.HtmlInputPassword)ctl;
                    entity.SetPropertyValue(pi.Name, ck.Value);
                }
                #endregion
            }
        }
        /// <summary>
        /// 填充表单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="control"></param>
        public static void FillForm(WebUI.Control control, Object entity)
        {
            if (entity == null || control == null)
                return;
            PropertyInfo[] propertyList = entity.GetProperties();
            foreach (PropertyInfo pi in propertyList)
            {
                WebUI.Control ctl = control.FindControl(string.Format(IdFormat, pi.Name));
                if (ctl == null)
                    continue;
                Type ctlType = ctl.GetType();
                #region 处理服务器控件
                if (ctlType == typeof(WebUI.WebControls.TextBox))//文本框
                {
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ((WebUI.WebControls.TextBox)ctl).Text = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.WebControls.Image))//图片
                {
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        string imageUrl = entity.GetPropertyValue(pi.Name).ToString();
                        if (!string.IsNullOrEmpty(imageUrl))
                            ((WebUI.WebControls.Image)ctl).ImageUrl = imageUrl;
                    }
                }
                else if (ctlType == typeof(WebUI.WebControls.DropDownList))//选择框
                {
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ((WebUI.WebControls.DropDownList)ctl).SelectedValue = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.WebControls.HiddenField))//隐藏域
                {
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ((WebUI.WebControls.HiddenField)ctl).Value = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.WebControls.RadioButton))//单选框
                {
                    WebUI.WebControls.RadioButton rb = (WebUI.WebControls.RadioButton)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        rb.Checked = entity.GetPropertyValue(pi.Name).ToString() == rb.Text ? true : false;
                    }
                }
                else if (ctlType == typeof(WebUI.WebControls.CheckBox))//复选框
                {
                    WebUI.WebControls.CheckBox ck = (WebUI.WebControls.CheckBox)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        ck.Checked = entity.GetPropertyValue(pi.Name).ToString() == ck.Text ? true : false;
                    }
                }
                else if (ctlType == typeof(WebUI.WebControls.CheckBoxList))//复选框列表
                {
                    WebUI.WebControls.CheckBoxList ck = (WebUI.WebControls.CheckBoxList)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        string sel = entity.GetPropertyValue(pi.Name).ToString();
                        foreach (WebUI.WebControls.ListItem li in ck.Items)
                        {
                            if (sel.IndexOf(",") > -1 && (sel.IndexOf(li.Value + ",") > -1 || sel.IndexOf("," + li.Value) > -1))
                            {
                                li.Selected = true;
                            }
                            else if (sel.IndexOf(",") == -1 && sel == li.Value)
                            {
                                li.Selected = true;
                            }
                            else
                            {
                                li.Selected = false;
                            }
                        }
                    }
                }
                else if (ctlType == typeof(WebUI.WebControls.RadioButtonList))//单框列表
                {
                    WebUI.WebControls.RadioButtonList ck = (WebUI.WebControls.RadioButtonList)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ck.SelectedValue = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.WebControls.ListBox))//列表框
                {
                    WebUI.WebControls.ListBox ck = (WebUI.WebControls.ListBox)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        string sel = entity.GetPropertyValue(pi.Name).ToString();
                        foreach (WebUI.WebControls.ListItem li in ck.Items)
                        {
                            if (sel.IndexOf(",") > -1 && (sel.IndexOf(li.Value + ",") > -1 || sel.IndexOf("," + li.Value) > -1))
                            {
                                li.Selected = true;
                            }
                            else if (sel.IndexOf(",") == -1 && sel == li.Value)
                            {
                                li.Selected = true;
                            }
                            else
                            {
                                li.Selected = false;
                            }
                        }
                    }
                }
                #endregion
                #region 处理不同Html控件
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputText))//文本域
                {
                    WebUI.HtmlControls.HtmlInputText ct = (WebUI.HtmlControls.HtmlInputText)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ct.Value = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlTextArea))//文本域
                {
                    WebUI.HtmlControls.HtmlTextArea ct = (WebUI.HtmlControls.HtmlTextArea)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ct.Value = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlSelect))//选择域
                {
                    WebUI.HtmlControls.HtmlSelect ct = (WebUI.HtmlControls.HtmlSelect)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                    {
                        for (int i = 0; i < ct.Items.Count; i++)
                        {
                            if (ct.Items[i].Value == entity.GetPropertyValue(pi.Name).ToString())
                                ct.SelectedIndex = i;
                        }
                    }
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputHidden))////隐藏域
                {
                    WebUI.HtmlControls.HtmlInputHidden ct = (WebUI.HtmlControls.HtmlInputHidden)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ct.Value = entity.GetPropertyValue(pi.Name).ToString();
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputRadioButton))//单选域
                {
                    WebUI.HtmlControls.HtmlInputRadioButton rb = (WebUI.HtmlControls.HtmlInputRadioButton)ctl;
                    if (rb.Checked && entity.GetPropertyValue(pi.Name) != null)
                    {
                        rb.Checked = entity.GetPropertyValue(pi.Name).ToString() == rb.Value ? true : false;
                    }
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputCheckBox))//复选域
                {
                    WebUI.HtmlControls.HtmlInputCheckBox ck = (WebUI.HtmlControls.HtmlInputCheckBox)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        if (entity.GetPropertyValue(pi.Name).ToString().IndexOf("," + ck.Value) != -1)
                            ck.Checked = true;
                }
                else if (ctlType == typeof(WebUI.HtmlControls.HtmlInputPassword))//密码域
                {
                    WebUI.HtmlControls.HtmlInputPassword ck = (WebUI.HtmlControls.HtmlInputPassword)ctl;
                    if (entity.GetPropertyValue(pi.Name) != null)
                        ck.Value = entity.GetPropertyValue(pi.Name).ToString();
                }
                #endregion
            }
        }

        public static void FillExtension(WebUI.Control control,Dictionary<string, object> extensions)
        {
            foreach (var pi in extensions.Keys)
            {
                WebUI.Control ctl = control.FindControl(string.Format(IdFormat, pi));
                if (ctl == null)
                    continue;
                Type ctlType = ctl.GetType();



                if (ctlType == typeof (WebUI.WebControls.TextBox)) //文本框
                {
                    if (extensions[pi] != null)
                        ((WebUI.WebControls.TextBox)ctl).Text = GetFormatStr(extensions[pi]);
                }

            }
        }

        private static string GetFormatStr(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            DateTime dt;
            if (DateTime.TryParse(obj.ToString(),out dt))
            {
                return dt.ToString("yyyy-MM-dd");
            }
            return obj.ToString();

        }
        /// <summary>
        /// 清理表单
        /// </summary>
        /// <param name="control"></param>
        public static void ClearForm(WebUI.Control control)
        {
            if (control == null)
                return;
            foreach (WebUI.Control ctl in control.Controls)
            {
                Type type = ctl.GetType();
                #region 处理服务器控件
                if (type == typeof(WebUI.WebControls.TextBox))//文本框
                {
                    WebUI.WebControls.TextBox box = ((WebUI.WebControls.TextBox)ctl);
                    box.Text = "";
                    if (box.Attributes["isNumber"] != null)
                        box.Text = "0";
                }
                else if (type == typeof(WebUI.WebControls.DropDownList))//选择框
                {
                    ((WebUI.WebControls.DropDownList)ctl).SelectedIndex = -1;
                }
                else if (type == typeof(WebUI.WebControls.HiddenField))//隐藏域
                {
                    ((WebUI.WebControls.HiddenField)ctl).Value = "";
                }
                else if (type == typeof(WebUI.WebControls.RadioButton))//单选框
                {
                    WebUI.WebControls.RadioButton rb = (WebUI.WebControls.RadioButton)ctl;
                    rb.Checked = false;
                }
                else if (type == typeof(WebUI.WebControls.CheckBox))//复选框
                {
                    WebUI.WebControls.CheckBox ck = (WebUI.WebControls.CheckBox)ctl;
                    ck.Checked = false;
                }
                else if (type == typeof(WebUI.WebControls.CheckBoxList))//复选框列表
                {
                    WebUI.WebControls.CheckBoxList ck = (WebUI.WebControls.CheckBoxList)ctl;
                    foreach (WebUI.WebControls.ListItem li in ck.Items)
                    {
                        li.Selected = false;
                    }
                }
                else if (type == typeof(WebUI.WebControls.RadioButtonList))//单框列表
                {
                    WebUI.WebControls.RadioButtonList ck = (WebUI.WebControls.RadioButtonList)ctl;
                    foreach (WebUI.WebControls.ListItem li in ck.Items)
                    {
                        li.Selected = false;
                    }
                }
                else if (type == typeof(WebUI.WebControls.ListBox))//列表框
                {
                    WebUI.WebControls.ListBox ck = (WebUI.WebControls.ListBox)ctl;
                    foreach (WebUI.WebControls.ListItem li in ck.Items)
                    {
                        li.Selected = false;
                    }
                }
                #endregion
                #region 处理不同Html控件
                else if (type == typeof(WebUI.HtmlControls.HtmlInputText))//文本域
                {
                    WebUI.HtmlControls.HtmlInputText ct = (WebUI.HtmlControls.HtmlInputText)ctl;
                    ct.Value = "";
                    if (ct.Attributes["isNumber"] != null)
                        ct.Value = "0";
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlTextArea))//文本域
                {
                    WebUI.HtmlControls.HtmlTextArea ct = (WebUI.HtmlControls.HtmlTextArea)ctl;
                    ct.Value = "";
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlSelect))//选择域
                {
                    WebUI.HtmlControls.HtmlSelect ct = (WebUI.HtmlControls.HtmlSelect)ctl;
                    ct.SelectedIndex = -1;
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlInputHidden))////隐藏域
                {
                    WebUI.HtmlControls.HtmlInputHidden ct = (WebUI.HtmlControls.HtmlInputHidden)ctl;
                    ct.Value = "";
                    if (ct.Attributes["isNumber"] != null)
                        ct.Value = "0";
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlInputRadioButton))//单选域
                {
                    WebUI.HtmlControls.HtmlInputRadioButton rb = (WebUI.HtmlControls.HtmlInputRadioButton)ctl;
                    rb.Checked = false;
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlInputCheckBox))//复选域
                {
                    WebUI.HtmlControls.HtmlInputCheckBox ck = (WebUI.HtmlControls.HtmlInputCheckBox)ctl;
                    ck.Checked = false;
                }
                else if (type == typeof(WebUI.HtmlControls.HtmlInputPassword))//密码域
                {
                    WebUI.HtmlControls.HtmlInputPassword ck = (WebUI.HtmlControls.HtmlInputPassword)ctl;
                    ck.Value = "";
                }
                #endregion
            }
        }
        #endregion

        public static List<WebUI.Control> GetByLink(WebUI.Control holder, string linkId)
        {
            List<WebUI.Control> controlList = new List<Control>();
            foreach (Control control in holder.Controls)
            {
                if (control is IAttributeAccessor)
                {
                    IAttributeAccessor attributeAccessor = (IAttributeAccessor)control;
                    var _linkId = attributeAccessor.GetAttribute("data-link");
                    if (!string.IsNullOrWhiteSpace(_linkId)
                        && _linkId.Split(',').Contains(linkId))
                    {
                        controlList.Add(control);
                    }
                }
            }
            return controlList;
        }
        public static void ShowByLink(WebUI.Control holder, string linkId)
        {
            List<WebUI.Control> controlList = GetByLink(holder, linkId);
            foreach (Control control in controlList)
            {
                control.Visible = true;
            }
        }
        public static void HideByLink(WebUI.Control holder, string linkId)
        {
            List<WebUI.Control> controlList = GetByLink(holder, linkId);
            foreach (Control control in controlList)
            {
                control.Visible = false;
            }
        }
    }
}