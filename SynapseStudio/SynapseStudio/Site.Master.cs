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
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // DataTable dt = null;
            //try
            //{
            //    dt = (Session["userFullName"]) as DataTable;
            //}
            //catch
            //{
            //    dt = null;
            //}

            string fullname = null;
            try
            {
                fullname = (Session["userFullName"]) as string;
            }
            catch
            {
                fullname = null;
            }


            if (fullname == null && !Request.Url.AbsolutePath.ToLower().Contains("logout"))
            {
                Response.Redirect("Logout.aspx?oidccallback=true");
            }



        }
    }
}