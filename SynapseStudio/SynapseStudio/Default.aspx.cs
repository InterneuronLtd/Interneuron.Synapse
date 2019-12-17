//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SynapseStudio
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            DataTable dt = null;
            try
            {
                dt = (Session["UserDetailsSxn"]) as DataTable;
            }
            catch
            {
                dt = null;
            }

            string userType = "";
            try
            {
                userType = Session["userType"].ToString().ToLower();
            } catch { }

            BindGrids();


        }

        private void BindGrids()
        {
            string coreSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  'd8851db1-68f8-45ee-be9a-628666512431' ORDER BY entityname; ";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsCore = DataServices.DataSetFromSQL(coreSQL, paramList);
            DataTable dtCore = dsCore.Tables[0];

            this.lblCoreEntityCount.Text = dtCore.Rows.Count.ToString();
            this.dgCoreEnties.DataSource = dtCore;
            this.dgCoreEnties.DataBind();

            string extendedSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  'e8b78b52-641d-46eb-bb8b-16e2feb86fe7' ORDER BY entityname; ";

            DataSet dsExtended = DataServices.DataSetFromSQL(extendedSQL, paramList);
            DataTable dtExtended = dsExtended.Tables[0];

            this.lblExtendedCount.Text = dtExtended.Rows.Count.ToString();
            this.dgExtendedEntities.DataSource = dtExtended;
            this.dgExtendedEntities.DataBind();

            string LocalSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  '468ac87d-f6a3-4d20-8800-58742b8952b6' ORDER BY entityname; ";

            DataSet dsLocal = DataServices.DataSetFromSQL(LocalSQL, paramList);
            DataTable dtLocal = dsLocal.Tables[0];

            this.lblLocalCount.Text = dtLocal.Rows.Count.ToString();
            this.dgLocalEntities.DataSource = dtLocal;
            this.dgLocalEntities.DataBind();

            string metaSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  '76ce099d-0ab1-40c5-8b37-d0933b84ec8c' ORDER BY entityname; ";

            DataSet dsMeta = DataServices.DataSetFromSQL(metaSQL, paramList);
            DataTable dtMeta = dsMeta.Tables[0];

            this.lblMetaEntityCount.Text = dtMeta.Rows.Count.ToString();
            this.dgMetaEntities.DataSource = dtMeta;
            this.dgMetaEntities.DataBind();
        }

        protected void btnNewCoreEntity_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerNew.aspx?id=d8851db1-68f8-45ee-be9a-628666512431");
        }

        protected void btnNewExtendedEntity_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerNewExtended.aspx?id=e8b78b52-641d-46eb-bb8b-16e2feb86fe7");
        }

        protected void btnNewLocalEntity_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerNewLocal.aspx?id=468ac87d-f6a3-4d20-8800-58742b8952b6");
        }

        protected void btnNewMetaEntity_Click(object sender, EventArgs e)
        {
            Response.Redirect("EntityManagerNewMeta.aspx?id=76ce099d-0ab1-40c5-8b37-d0933b84ec8c");
        }
    }
}