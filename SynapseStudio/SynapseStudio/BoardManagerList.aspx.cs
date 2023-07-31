 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

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
    public partial class BoardManagerList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            BindBedBoardList();
            BindLocatorBoardList();
        }

        private void BindBedBoardList()
        {
            string sql = "SELECT bedboard_id, bedboardname FROM eboards.bedboard ORDER BY bedboardname;";
            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql);
            }
            catch(Exception ex)
            {
                return;
            }

            DataTable dt = ds.Tables[0];
            this.dgBedBoards.DataSource = dt;
            this.dgBedBoards.DataBind();
            this.lblBedBoardCount.Text = dt.Rows.Count.ToString();

        }

        private void BindLocatorBoardList()
        {
            string sql = "SELECT locatorboard_id, locatorboardname FROM eboards.locatorboard ORDER BY locatorboardname;";
            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql);
            }
            catch (Exception ex)
            {
                return;
            }

            DataTable dt = ds.Tables[0];
            this.dgLocatorBoards.DataSource = dt;
            this.dgLocatorBoards.DataBind();
            this.lblLocatorBoardCount.Text = dt.Rows.Count.ToString();

        }

        protected void btnNewBedBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect("BedBoardManagerNew.aspx");
        }

        protected void btnNewLocator_Click(object sender, EventArgs e)
        {
            Response.Redirect("LocatorBoardManagerNew.aspx");
        }

        protected void btnNewLInpatientManagement_Click(object sender, EventArgs e)
        {

        }

        protected void btnBedBoardDevices_Click(object sender, EventArgs e)
        {
            Response.Redirect("BedBoardDeviceList.aspx");
        }

        protected void ButtbtnLocatorBoardDeviceson1_Click(object sender, EventArgs e)
        {
            Response.Redirect("LocatorBoardDeviceList.aspx");
        }
    }
}