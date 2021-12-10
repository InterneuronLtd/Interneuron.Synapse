

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavBar.ascx.cs" Inherits="SynapseStudio.NavBar" %>

<ul class="nav navbar-nav navbar-right navbar-user">   
    <li class="dropdown user-dropdown">
        <a href="#" class="dropdown-toggle" data-toggle="dropdown"><i class="fa fa-user"></i>&nbsp;<asp:Label ID="lblUserFullName" runat="server"></asp:Label><b class="caret"></b></a>
        <ul class="dropdown-menu">
            <li><a href="logout.aspx"><i class="fa fa-power-off"></i>&nbsp;Log Out</a></li>

        </ul>
    </li>
</ul>
