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
    public partial class DevOpsDashboard : System.Web.UI.Page
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
            }
            catch { }


        }

        private void BindDevOpsDashboardGrids()
        {

            // Database Activity
            string dbActivitySQL = "SELECT * FROM pg_stat_activity WHERE datname='synapse';";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsActivity = DataServices.DataSetFromSQL(dbActivitySQL, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionPostgresDB);
            DataTable dtActivity = dsActivity.Tables[0];

            this.dataGridDatabaseActivity.DataSource = dtActivity;
            this.dataGridDatabaseActivity.DataBind();

            //Replication Status
            string replStatusSQL = "select pid, usename, client_addr, client_port, backend_start, sent_location, write_location, flush_location, replay_location from pg_stat_replication;";

            DataSet dsReplStatus = DataServices.DataSetFromSQL(replStatusSQL, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionPostgresDB);
            DataTable dtReplStatus = dsReplStatus.Tables[0];

            this.dgReplStatus.DataSource = dtReplStatus;
            this.dgReplStatus.DataBind();




            //Update Refreshed Label
            this.lblDashboardRefreshed.Text = "Lasted refreshed @ " + DateTime.Now.ToShortTimeString();

        }

        protected void btnRefreshDashboard_Click(object sender, EventArgs e)
        {
            BindDevOpsDashboardGrids();
        }


    }
}