

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sidebar.ascx.cs" Inherits="SynapseStudio.Sidebar" %>

<ul id="active" class="nav navbar-nav side-nav">
    <li style="height: 10px;">&nbsp;</li>
    <li><a href="Default.aspx"><i class="fa fa-home"></i>&nbsp;Home</a></li>
    <li><a href="EntityManagerList.aspx"><i class="fa fa-database"></i>&nbsp;Entity Manager</a></li>
    <li><a href="BaseViewManagerList.aspx"><i class="fa fa-anchor"></i>&nbsp;Baseviews</a></li>

    <li><a href="ListManagerList.aspx"><i class="fa fa-list"></i>&nbsp;Lists</a></li>
    <li><a href="BoardManagerList.aspx"><i class="fa fa-columns"></i>&nbsp;Boards</a></li>
    <li><a href="SchemaExport.aspx"><i class="fa fa-refresh"></i>&nbsp;Schema Migration</a></li>
    <!--<li><a href="DevOpsDashboard.aspx"><i class="fa fa-server"></i>&nbsp;DevOps</a></li>-->
    <li class="dropdown">
        <a class="dropdown-toggle" data-toggle="dropdown" href="#">&nbsp;DevOps &nbsp;&nbsp;<span class="caret"></span></a>
        <ul class="dropdown-menu">
            <li><a href="DevOpsDashboard.aspx"><i class="fa fa-dashboard"></i>&nbsp;Dashboard</a> </li>
            <li><a href="#" onclick="javascript:window.open('<%= ConfigurationManager.AppSettings["JenkinsURL"] %>');return false;" target="_blank">
                <i class="fa fa-handshake"></i>&nbsp;CI \ CD</a> </li>
        </ul>
    </li>
    <li><a href="SecurityManager.aspx"><i class="fa fa-server"></i>&nbsp;Security</a></li>
    <li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">&nbsp;Applications &nbsp;&nbsp;<span class="caret"></span></a>
        <ul class="dropdown-menu">
            <li><a href="ApplicationManager.aspx"><i class="fa fa-map-marker"></i>&nbsp;Applications Module</a> </li>
            <li><a href="ApplicationListManager.aspx"><i class="fa fa-map-marker"></i>&nbsp;Applications List</a> </li>
        </ul>
    </li>

    <li><a href="logout.aspx"><i class="fa fa-power-off"></i>&nbsp;Log Out</a></li>
</ul>
