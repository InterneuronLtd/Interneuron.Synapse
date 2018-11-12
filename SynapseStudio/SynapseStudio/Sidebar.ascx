<%--
Interneuron Synapse

Copyright (C) 2018  Interneuron CIC



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
    <li><a href="SchemaExport.aspx"><i class="fa fa-columns"></i>&nbsp;Schema Migration</a></li>
    <li><a href="logout.aspx"><i class="fa fa-power-off"></i>&nbsp;Log Out</a></li>
</ul>
