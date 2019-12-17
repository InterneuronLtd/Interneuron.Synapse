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

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sidebar.ascx.cs" Inherits="SynapseStudio.Sidebar" %>

<ul id="active" class="nav navbar-nav side-nav">
    <li style="height: 10px;">&nbsp;</li>
    <li><a href="Default.aspx"><i class="fa fa-home"></i>&nbsp;Home</a></li>
    <li><a href="EntityManagerList.aspx"><i class="fa fa-database"></i>&nbsp;Entity Manager</a></li>
    <li><a href="BaseViewManagerList.aspx"><i class="fa fa-anchor"></i>&nbsp;Baseviews</a></li>
    
    <li><a href="ListManagerList.aspx"><i class="fa fa-list"></i>&nbsp;Lists</a></li>
    <li><a href="BoardManagerList.aspx"><i class="fa fa-columns"></i>&nbsp;Boards</a></li>
    <li><a href="SchemaExport.aspx"><i class="fa fa-refresh"></i>&nbsp;Schema Migration</a></li>
    <li><a href="DevOpsDashboard.aspx"><i class="fa fa-server"></i>&nbsp;DevOps</a></li>
    <li><a href="SecurityManager.aspx"><i class="fa fa-server"></i>&nbsp;Security</a></li>
     <li class="dropdown"><a class="dropdown-toggle" data-toggle="dropdown" href="#">&nbsp;Applications &nbsp;&nbsp;<span class="caret"></span></a>
        <ul class="dropdown-menu">
         <li><a href="ApplicationManager.aspx"><i class="fa fa-map-marker"></i>&nbsp;Applications Module</a> </li>
            <li><a href="ApplicationListManager.aspx"><i class="fa fa-map-marker"></i>&nbsp;Applications List</a> </li>
        </ul>
      </li>
    
    <li><a href="logout.aspx"><i class="fa fa-power-off"></i>&nbsp;Log Out</a></li>
</ul>
