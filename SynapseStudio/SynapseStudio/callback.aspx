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

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="callback.aspx.cs" Inherits="EBoards.callback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="js/jquery-3.3.1.js"></script>
    <script src="Scripts/oidc/oidc-client.js"></script>
    <script src="Scripts/oidc/callback.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hTkn" runat="server" />
            Please wait...
            <div style="display: none">
                <asp:Button ID="btnPostback" runat="server" Text="" OnClick="btnPostback_Click" />
            </div>
            <div id="error"></div>
        </div>
    </form>
</body>
</html>
