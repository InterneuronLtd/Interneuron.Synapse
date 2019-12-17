<%--
Interneuron Synapse

Copyright (C) 2019  Interneuron CIC



This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  

See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
--%>

<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="SynapseStudio.logout" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="GlobalSettings.js"></script>
    <script src="Scripts/oidc/oidc-client.js"></script>
    <script src="Scripts/oidc/OidcLoginHelper.js"></script>

    <link href="bootstrap/css/bootstrap.css" rel="stylesheet" />
    <link href="font-awesome/css/font-awesome.css" rel="stylesheet" />
    <style>
        .div-form {
            border: 4px solid #343762;
            padding: 15px;
        }

        .form-signin .checkbox {
            font-weight: 400;
        }

        .form-signin .form-control {
            position: relative;
            box-sizing: border-box;
            height: auto;
            padding: 10px;
            font-size: 16px;
        }

            .form-signin .form-control:focus {
                z-index: 2;
            }

        .form-signin input[type="email"] {
            margin-bottom: -1px;
            border-bottom-right-radius: 0;
            border-bottom-left-radius: 0;
        }

        .form-signin input[type="password"] {
            margin-bottom: 10px;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
        }
    </style>
</head>
<body class="text-center">
    <form id="form1" runat="server" class="form-signin">

        <div style="padding-top: 80px;">

            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-3">
                        &nbsp;
                    </div>
                    <div class="col-md-6 div-form">

                        <asp:Panel ID="pnlLogin" runat="server">


                           <h1>Synapse Studio</h1>


                            <div id="divLoggingOut" visible="false" runat="server" class="alert alert-danger">
                                We are logging you out, please wait.
                            </div>

                            <div id="divLoggedOut" visible="false" runat="server" class="alert alert-danger">
                                <h3>Logged out successfully</h3>

                                Your session has ended, please click on the link below to login again.

                                   <a href="LoginOidc.aspx" class="btn btn-lg btn-primary btn-block">Click here to login again</a>
                            </div>
                            <%--<button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>--%>
                        </asp:Panel>




                        <div class="col-md-3">
                            &nbsp;
                        </div>

                    </div>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
