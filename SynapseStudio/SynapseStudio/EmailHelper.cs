//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using MailKit;

namespace SynapseStudio
{
    public class EmailHelper
    {

        public static int SendMail(string messageBody, string messageSubject, string emailTo, out string msg)
        {

            string emailhost = "";
	        string emailuser = "";
            string emailpassword = "";
            Int16 emailport = 0;
            bool emailusetls = false;
            string emailfromaddress = "";
            string emailfromname = "Physical Health App";

            string sql = "SELECT * FROM systemsettings.systemsetup WHERE systemsetupid = 1;";

            DataSet ds = DataServices.DataSetFromSQL(sql, null);
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                try { emailhost = dt.Rows[0]["emailhost"].ToString(); } catch { }
                try { emailuser = dt.Rows[0]["emailuser"].ToString(); } catch { }
                try { emailpassword = dt.Rows[0]["emailpassword"].ToString(); } catch { }
                try { emailport = System.Convert.ToInt16(dt.Rows[0]["emailport"].ToString()); } catch { }
                try { emailusetls = System.Convert.ToBoolean(dt.Rows[0]["emailusetls"].ToString()); } catch { }
                try { emailfromaddress = dt.Rows[0]["emailfromaddress"].ToString(); } catch { }
                try { emailfromname = dt.Rows[0]["emailfromname"].ToString(); } catch { }
            }


            if(string.IsNullOrEmpty(emailhost))
            {
                msg = "Email not configured";
                return 0;
            }

            MailMessage Message = new MailMessage();

            Message.Subject = messageSubject;
            Message.Body = messageBody;
            Message.From = new System.Net.Mail.MailAddress(emailfromaddress, emailfromname);
            Message.ReplyToList.Add(Message.From);
            Message.IsBodyHtml = true;
            Message.To.Add(new MailAddress(emailTo));

            SmtpClient client = new SmtpClient();
            client.Host = emailhost;
            client.Port = emailport;
            client.UseDefaultCredentials = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = emailusetls;
            client.Credentials = new NetworkCredential(emailuser, emailpassword);
            try
            {
                client.Send(Message);
                msg = "Email sent successfully";
                return 1;
            }
            catch (Exception ex)
            {
                msg = "Problem with email account: " + ex.ToString();
                return 0;
            }
        }



    }
}