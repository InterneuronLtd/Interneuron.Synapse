//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.


﻿

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SynapseStudio
{
    public partial class ListQuestionNew : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //string id = "";
                //try
                //{
                //    id = Request.QueryString["id"].ToString();
                //}
                //catch
                //{
                //    Response.Redirect("Error.aspx");
                //}


                //if (String.IsNullOrEmpty(id))
                //{
                //    Response.Redirect("Error.aspx");
                //}

                //this.hdnNamespaceID.Value = id;

                //string local = "";
                //try
                //{
                //    local = Request.QueryString["local"].ToString();
                //}
                //catch
                //{
                //}

                //this.hdnLocalNamespaceID.Value = local;



                //string name = "";
                //try
                //{
                //    name = SynapseHelpers.GetListNamespaceNameFromID(id);
                //}
                //catch { }

                //try
                //{
                //    this.hdnUserName.Value = Session["userFullName"].ToString();
                //}
                //catch { }

                //if (String.IsNullOrEmpty(name))
                //{
                //    Response.Redirect("Error.aspx");
                //}



                BindDropDownList(this.ddlDefaultContext, "SELECT entityid, synapsenamespacename || '.' || entityname as entitydisplayname, keycolumn FROM entitysettings.entitymanager order by 2", "entityid", "entitydisplayname", 1, null);
                BindDropDownList(this.ddlQuestionType, "SELECT questiontype_id, questiontypetext, htmltemplate FROM listsettings.questiontype WHERE isenabled = true ORDER BY displayorder;", "questiontype_id", "questiontypetext", 1, null);
                BindDropDownList(this.ddlOptionType, "SELECT questionoptiontype_id, questionoptiontypename FROM listsettings.questionoptiontype ORDER BY displayorder;", "questionoptiontype_id", "questionoptiontypename", 0, null);
                BindDropDownList(this.ddlOptionCollection, "SELECT questionoptioncollection_id, questionoptioncollectionname, questionoptioncollectiondescription FROM listsettings.questionoptioncollection ORDER BY questionoptioncollectionname;", "questionoptioncollection_id", "questionoptioncollectionname", 1, null);
                //BindBaseViewContextFields();



                CheckQuestionTypeSelection();
                CheckOptionTypeSelection();


                this.lblError.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
                this.btnSave.Visible = false;
                this.btnCancel.Visible = false;
            }
        }


        //Drop Down Lists
        private void BindDropDownList(DropDownList ddl, string sql, string valueField, string displayField, int addPleaseSelect, List<KeyValuePair<string, string>> parameters = null)
        {
            DataSet ds = DataServices.DataSetFromSQL(sql, parameters);
            ddl.DataSource = ds;
            ddl.DataValueField = valueField;
            ddl.DataTextField = displayField;
            ddl.DataBind();

            if (addPleaseSelect == 1)
            {
                ListItem[] items = new ListItem[1];
                items[0] = new ListItem("Please select ...", "0");
                ddl.Items.Insert(0, items[0]);
            }
        }

        private void SetDDLSource(DropDownList ddl, string val)
        {
            if (val.Length > 0)
            {
                int idx = 9999;

                try
                {
                    idx = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
                }
                catch
                {
                    idx = 9999;
                }

                if (idx == 9999 || idx < 0)
                {
                    ListItem[] items = new ListItem[1];
                    items[0] = new ListItem(val + " (old value)", val);
                    ddl.Items.Insert(1, items[0]);
                }
            }

            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(val));
        }


        protected void ddlDefaultContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lblDefaultContextField.Text = SynapseHelpers.GetKeyColumnForEntity(this.ddlDefaultContext.SelectedValue);
            // BindBaseViewContextFields();
        }

        protected void ddlQuestionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckQuestionTypeSelection();

        }

        private void CheckOptionTypeSelection()
        {
            //"e9e6feda-f02d-4388-8c5b-9fc97558c684"  "Internal Option Collection"
            //"638dadd6-fca7-4f9b-b25f-692c45172524"  "Custom SQL Staetement"

            //switch (this.ddlOptionType.SelectedValue)
            //{
            //    case "e9e6feda-f02d-4388-8c5b-9fc97558c684": //  "Internal Option Collection"
            //        break;
            //    case "638dadd6-fca7-4f9b-b25f-692c45172524": //  "Custom SQL Staetement"
            //        break;
            //    default:
            //        break;
            //}

            this.fgOptionCollection.Visible = false;
            this.fgOptionSQLStatement.Visible = false;

            switch (this.ddlOptionType.SelectedValue)
            {
                case "e9e6feda-f02d-4388-8c5b-9fc97558c684": //  "Internal Option Collection"
                    this.fgOptionCollection.Visible = true;
                    this.fgOptionSQLStatement.Visible = false;
                    break;
                case "638dadd6-fca7-4f9b-b25f-692c45172524": //  "Custom SQL Staetement"
                    this.fgOptionCollection.Visible = false;
                    this.fgOptionSQLStatement.Visible = true;
                    break;
                default:
                    break;
            }
        }


        private void CheckQuestionTypeSelection()
        {
            //"bbc7acbc-b968-4dad-b9d2-ee22ce943a35"  "Text Box (Limit 255)"                1
            //"feb547a3-3b84-40c7-8007-547c9fe267e9"  "Text Area (No limit)"                2
            //"3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf"  "HTML Tag (Label, Custom HTML)"       3
            //"fc1f2643-b491-4889-8d1a-910619b65722"  "Drop Down List"                      4
            //"3d236e17-e40e-472d-95a5-5e45c5e02faf"  "Check Box List"                      5
            //"6c166d07-53d0-4cd3-80f4-801cadcc88eb"  "Calendar Control"                    6
            //"83d4fd68-ac33-4996-bd2a-8b6338526520"  "Time Picker"                         7
            //"4f31c02d-fa36-4033-8977-8f25bef33d52"  "Auto-complete Selection List"        8


            //switch (this.ddlQuestionType.SelectedValue)
            //{
            //    case "bbc7acbc-b968-4dad-b9d2-ee22ce943a35":  //"Text Box (Limit 255)"                1

            //        break;
            //    case "feb547a3-3b84-40c7-8007-547c9fe267e9":  //"Text Area (No limit)"                2

            //        break;
            //    case "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf":  //"HTML Tag (Label, Custom HTML)"       3

            //        break;
            //    case "fc1f2643-b491-4889-8d1a-910619b65722":  //"Drop Down List"                      4

            //        break;
            //    case "3d236e17-e40e-472d-95a5-5e45c5e02faf":  //"Check Box List"                      5

            //        break;
            //    case "6c166d07-53d0-4cd3-80f4-801cadcc88eb":  //"Calendar Control"                    6

            //        break;
            //    case "83d4fd68-ac33-4996-bd2a-8b6338526520":  //"Time Picker"                         7

            //        break;
            //    case "4f31c02d-fa36-4033-8977-8f25bef33d52":  //"Auto-complete Selection List"        8

            //        break;
            //    default:

            //        break;

            //}


            this.fgLabelText.Visible = false;
            this.fgCustomHTML.Visible = false;
            this.fgDefaultValueDate.Visible = false;
            this.fgDefaultValueText.Visible = false;
            this.pnlOptions.Visible = false;
            this.fgCustomHTMLAlt.Visible = false;
            this.lblCustomHTML.Text = "* * enter the custom HTML that you wish to display";

            switch (this.ddlQuestionType.SelectedValue)
            {
                case "bbc7acbc-b968-4dad-b9d2-ee22ce943a35":  //"Text Box (Limit 255)"                1
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = false;
                    break;
                case "feb547a3-3b84-40c7-8007-547c9fe267e9":  //"Text Area (No limit)"                2
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = false;
                    break;
                case "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf":  //"HTML Tag (Label, Custom HTML)"       3
                    this.fgLabelText.Visible = false;
                    this.fgCustomHTML.Visible = true;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = false;
                    this.pnlOptions.Visible = false;
                    break;
                case "fc1f2643-b491-4889-8d1a-910619b65722":  //"Drop Down List"                      4
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = true;
                    CheckOptionTypeSelection();
                    break;

                case "ca1f1b24-b490-4e57-8921-9f680819e47c": // "Radio Button List"
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = true;
                    CheckOptionTypeSelection();
                    break;

                case "71490eff-a54b-455a-86b1-a4d5ab676f32": // "Radio Button Image List"
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = true;
                    CheckOptionTypeSelection();
                    break;

                case "3d236e17-e40e-472d-95a5-5e45c5e02faf":  //"Check Box List"                      5
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = true;
                    CheckOptionTypeSelection();
                    break;
                case "6c166d07-53d0-4cd3-80f4-801cadcc88eb":  //"Calendar Control"                    6
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = false;
                    this.pnlOptions.Visible = false;
                    break;
                case "83d4fd68-ac33-4996-bd2a-8b6338526520":  //"Time Picker"                         7
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = false;
                    this.pnlOptions.Visible = false;
                    break;
                case "4f31c02d-fa36-4033-8977-8f25bef33d52":  //"Auto-complete Selection List"        8
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = false;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = true;
                    this.pnlOptions.Visible = true;
                    CheckOptionTypeSelection();
                    break;
                case "164c31d5-d32e-4c97-91d6-a0d01822b9b6":  //"Single Checkbox (Binary)"        9
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = true;
                    this.fgCustomHTMLAlt.Visible = true;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = false;
                    this.pnlOptions.Visible = false;
                    //CheckOptionTypeSelection();
                    this.lblCustomHTML.Text = "* enter the custom html you want to display if the value has been selected";
                    break;
                case "221ca4a0-3a39-42ff-a0f4-885ffde0f0bd":  //"Checkbox Image (Binary)"        10
                    this.fgLabelText.Visible = true;
                    this.fgCustomHTML.Visible = true;
                    this.fgCustomHTMLAlt.Visible = true;
                    this.fgDefaultValueDate.Visible = false;
                    this.fgDefaultValueText.Visible = false;
                    this.pnlOptions.Visible = false;
                    //CheckOptionTypeSelection();
                    this.lblCustomHTML.Text = "* enter the custom html you want to display if the value has been selected";
                    break;

                default:

                    break;

            }
        }

        protected void ddlOptionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckOptionTypeSelection();
        }

        protected void btnValidateList_Click(object sender, EventArgs e)
        {

            string haserr = "form-group has-error";
            string noerr = "form-group";


            this.lblError.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.fgQuickName.CssClass = noerr;
            this.fgDefaultContext.CssClass = noerr;
            this.fgQuestionType.CssClass = noerr;
            this.fgLabelText.CssClass = noerr;
            this.fgOptionCollection.Attributes["class"] = noerr;
            this.fgCustomHTML.Attributes["class"] = noerr;
            this.fgOptionSQLStatement.Attributes["class"] = noerr;

            if (string.IsNullOrEmpty(this.txtQuickName.Text.ToString()))
            {
                this.lblError.Text = "Please enter a new name";
                this.txtQuickName.Focus();
                this.lblError.Visible = true;
                this.fgQuickName.CssClass = haserr;
                return;
            }

            if (this.ddlDefaultContext.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the default context";
                this.ddlDefaultContext.Focus();
                this.lblError.Visible = true;
                this.fgDefaultContext.CssClass = haserr;
                return;
            }

            if (this.ddlQuestionType.SelectedIndex == 0)
            {
                this.lblError.Text = "Please select the type of question";
                this.ddlQuestionType.Focus();
                this.lblError.Visible = true;
                this.fgQuestionType.CssClass = haserr;
                return;
            }

            if (this.ddlQuestionType.SelectedValue != "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"HTML Tag (Label, Custom HTML)"       3
            {
                if (string.IsNullOrEmpty(this.txtLabelText.Text.ToString()))
                {
                    this.lblError.Text = "Please enter the label text";
                    this.txtLabelText.Focus();
                    this.lblError.Visible = true;
                    this.fgLabelText.CssClass = haserr;
                    return;
                }
            }



            if (this.ddlQuestionType.SelectedValue == "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"HTML Tag (Label, Custom HTML)"       3
            {
                if (string.IsNullOrEmpty(this.txtCustomHTML.Text.ToString()))
                {
                    this.lblError.Text = "Please enter the HTML snippet for the option's flag";
                    this.txtCustomHTML.Focus();
                    this.lblError.Visible = true;
                    this.fgCustomHTML.Attributes["class"] = haserr;
                    return;
                }
            }


            if (this.ddlQuestionType.SelectedValue == "fc1f2643-b491-4889-8d1a-910619b65722" ||
                this.ddlQuestionType.SelectedValue == "3d236e17-e40e-472d-95a5-5e45c5e02faf" ||
                this.ddlQuestionType.SelectedValue == "4f31c02d-fa36-4033-8977-8f25bef33d52" ||
                this.ddlQuestionType.SelectedValue == "ca1f1b24-b490-4e57-8921-9f680819e47c" ||
                this.ddlQuestionType.SelectedValue == "71490eff-a54b-455a-86b1-a4d5ab676f32"
                )
            // "Drop Down List"                      4
            // "Check Box List"                      5
            // "Auto-complete Selection List"        8
            // "Radio Button List"
            // "Radio Button Image List"
            {
                if (this.ddlOptionType.SelectedValue == "e9e6feda-f02d-4388-8c5b-9fc97558c684" && this.ddlOptionCollection.SelectedIndex == 0)//Internal Option Collection
                {
                    this.lblError.Text = "Please select the internal option collection";
                    this.ddlOptionType.Focus();
                    this.lblError.Visible = true;
                    this.fgOptionCollection.Attributes["class"] = haserr;
                    return;
                }


                if (this.ddlOptionType.SelectedValue == "638dadd6-fca7-4f9b-b25f-692c45172524" && string.IsNullOrEmpty(this.txtOptionSQLStatement.Text)) //Custom SQL Statement
                {
                    this.lblError.Text = "Please enter the custom SQL statement to load the options";
                    this.txtOptionSQLStatement.Focus();
                    this.lblError.Visible = true;
                    this.fgOptionSQLStatement.Attributes["class"] = haserr;
                    return;
                }
            }

            //if (this.ddlQuestionType.SelectedValue == "164c31d5-d32e-4c97-91d6-a0d01822b9b6")  //"Single Checkbox (Binary)"        9
            //{

            //}


            this.txtCustomHTML.Enabled = false;
            this.txtCustomHTMLAlt.Enabled = false;
            this.txtDefaultValueDate.Enabled = false;
            this.txtDefaultValueText.Enabled = false;
            this.txtLabelText.Enabled = false;
            this.txtOptionSQLStatement.Enabled = false;
            this.txtQuickName.Enabled = false;
            this.ddlDefaultContext.Enabled = false;
            this.ddlOptionCollection.Enabled = false;
            this.ddlOptionType.Enabled = false;
            this.ddlQuestionType.Enabled = false;


            this.btnSave.Visible = true;
            this.btnCancel.Visible = true;
            this.btnValidateList.Visible = false;

            this.lblSuccess.Text = "Validation succeeded";
            this.lblSuccess.Visible = true;



            //switch (this.ddlQuestionType.SelectedValue)
            //{
            //    case "bbc7acbc-b968-4dad-b9d2-ee22ce943a35":  //"Text Box (Limit 255)"                1

            //        break;
            //    case "feb547a3-3b84-40c7-8007-547c9fe267e9":  //"Text Area (No limit)"                2

            //        break;
            //    case "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf":  //"HTML Tag (Label, Custom HTML)"       3

            //        break;
            //    case "fc1f2643-b491-4889-8d1a-910619b65722":  //"Drop Down List"                      4

            //        break;
            //    case "3d236e17-e40e-472d-95a5-5e45c5e02faf":  //"Check Box List"                      5

            //        break;
            //    case "6c166d07-53d0-4cd3-80f4-801cadcc88eb":  //"Calendar Control"                    6

            //        break;
            //    case "83d4fd68-ac33-4996-bd2a-8b6338526520":  //"Time Picker"                         7

            //        break;
            //    case "4f31c02d-fa36-4033-8977-8f25bef33d52":  //"Auto-complete Selection List"        8

            //        break;
            //    default:

            //        break;

            //}
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

            this.txtCustomHTML.Enabled = true;
            this.txtDefaultValueDate.Enabled = true;
            this.txtDefaultValueText.Enabled = true;
            this.txtLabelText.Enabled = true;
            this.txtOptionSQLStatement.Enabled = true;
            this.txtQuickName.Enabled = true;
            this.ddlDefaultContext.Enabled = true;
            this.ddlOptionCollection.Enabled = true;
            this.ddlOptionType.Enabled = true;
            this.ddlQuestionType.Enabled = true;
            this.txtCustomHTMLAlt.Enabled = true;


            this.btnSave.Visible = false;
            this.btnCancel.Visible = false;
            this.btnValidateList.Visible = true;

            this.lblSuccess.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string sql = "INSERT INTO listsettings.question(question_id, defaultcontext, defaultcontextfieldname, questiontype_id, questiontypetext, labeltext, defaultvaluetext, defaultvaluedatetime, questionquickname, questionview_id, questionviewname, questionviewsql, optiontype, questionoptioncollection_id, questionoptionsqlstatement, questioncustomhtml, questioncustomhtmlalt) values (@question_id, @defaultcontext, @defaultcontextfieldname, @questiontype_id, @questiontypetext, @labeltext, @defaultvaluetext, null, @questionquickname, @questionview_id, @questionviewname, @questionviewsql, @optiontype, @questionoptioncollection_id, @questionoptionsqlstatement, @questioncustomhtml, @questioncustomhtmlalt);";


            //, , , , , , , , , , , , , , 

            string question_id = System.Guid.NewGuid().ToString();
            string defaultcontext = this.ddlDefaultContext.SelectedValue;
            string defaultcontextfieldname = this.lblDefaultContextField.Text;
            string questiontype_id = this.ddlQuestionType.SelectedValue;
            string questiontypetext = this.ddlQuestionType.SelectedItem.Text;
            string labeltext = "";
            string questioncustomhtml = "";
            string defaultvaluetext = "";
            string questioncustomhtmlalt = "";

            if (this.ddlQuestionType.SelectedValue != "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"Not HTML Tag (Label, Custom HTML)"       3
            {

                defaultvaluetext = this.txtDefaultValueText.Text;
                labeltext = this.txtLabelText.Text;

            }

            if (this.ddlQuestionType.SelectedValue == "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf") //"HTML Tag (Label, Custom HTML)"       3
            {
                questioncustomhtml = this.txtCustomHTML.Text;
            }

            if (this.ddlQuestionType.SelectedValue == "164c31d5-d32e-4c97-91d6-a0d01822b9b6" || this.ddlQuestionType.SelectedValue == "221ca4a0-3a39-42ff-a0f4-885ffde0f0bd")  //"Single Checkbox (Binary)" or "Checkbox Image (Binary)" 
            {
                questioncustomhtml = this.txtCustomHTML.Text;
                questioncustomhtmlalt = this.txtCustomHTMLAlt.Text;
            }



            string questionquickname = this.txtQuickName.Text;
            string questionview_id = "";
            string questionviewname = "";
            string questionviewsql = "";


            string optiontype = "";
            string questionoptioncollection_id = "";
            string questionoptionsqlstatement = "";


            if (this.ddlQuestionType.SelectedValue == "fc1f2643-b491-4889-8d1a-910619b65722" ||
                 this.ddlQuestionType.SelectedValue == "3d236e17-e40e-472d-95a5-5e45c5e02faf" ||
                 this.ddlQuestionType.SelectedValue == "4f31c02d-fa36-4033-8977-8f25bef33d52" ||
                 this.ddlQuestionType.SelectedValue == "ca1f1b24-b490-4e57-8921-9f680819e47c" ||
                 this.ddlQuestionType.SelectedValue == "71490eff-a54b-455a-86b1-a4d5ab676f32"
                 )
            // "Drop Down List"                      4
            // "Check Box List"                      5
            // "Auto-complete Selection List"        8
            // "Radio Button List"
            // "Radio Button Image List"
            {
                optiontype = this.ddlOptionType.SelectedValue;
                if (this.ddlOptionType.SelectedValue == "e9e6feda-f02d-4388-8c5b-9fc97558c684")//Internal Option Collection
                {
                    questionoptioncollection_id = this.ddlOptionCollection.SelectedValue;
                }


                if (this.ddlOptionType.SelectedValue == "638dadd6-fca7-4f9b-b25f-692c45172524") //Custom SQL Statement
                {
                    questionoptionsqlstatement = this.txtCustomHTML.Text;
                }
            }


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("question_id", question_id),
                new KeyValuePair<string, string>("defaultcontext", defaultcontext),
                new KeyValuePair<string, string>("defaultcontextfieldname", defaultcontextfieldname),
                new KeyValuePair<string, string>("questiontype_id", questiontype_id),
                new KeyValuePair<string, string>("questiontypetext", questiontypetext),
                new KeyValuePair<string, string>("labeltext", labeltext),
                new KeyValuePair<string, string>("questioncustomhtml", questioncustomhtml),
                new KeyValuePair<string, string>("questionquickname", questionquickname),
                new KeyValuePair<string, string>("defaultvaluetext", defaultvaluetext),
                new KeyValuePair<string, string>("questionview_id", questionview_id),
                new KeyValuePair<string, string>("questionviewname", questionviewname),
                new KeyValuePair<string, string>("questionviewsql", questionviewsql),
                new KeyValuePair<string, string>("optiontype", optiontype),
                new KeyValuePair<string, string>("questionoptioncollection_id", questionoptioncollection_id),
                new KeyValuePair<string, string>("questionoptionsqlstatement", questionoptionsqlstatement),
                new KeyValuePair<string, string>("questioncustomhtmlalt", questioncustomhtmlalt)
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                this.lblError.Text = "Error creating question: " + System.Environment.NewLine + ex.ToString();
                this.lblError.Visible = true;
                return;
            }


            Response.Redirect("ListManagerList.aspx?id=" + question_id);

        }
    }
}