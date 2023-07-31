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
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class BoardController : Controller
    {
        public IActionResult BoardManagerList()
        {
            BoardModel boardModel = new BoardModel();
            string sql = "SELECT bedboard_id, bedboardname FROM eboards.bedboard ORDER BY bedboardname;";
            DataSet ds = DataServices.DataSetFromSQL(sql);
            string sqlLocator = "SELECT locatorboard_id, locatorboardname FROM eboards.locatorboard ORDER BY locatorboardname;";
            DataSet dsLocator = DataServices.DataSetFromSQL(sqlLocator);
            boardModel.BedBoardDto = ds.Tables[0].ToList<BedBoardDto>();
            boardModel.LocatorBoardDto = dsLocator.Tables[0].ToList<LocatorBoardDto>();
            return View(boardModel);
        }
        public IActionResult BedBoardDeviceList()
        {

            string sql = "SELECT * FROM eboards.v_bedboarddevice ORDER BY bedboarddevicename;";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };
            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            List<BedBoardDeviceDto> BedBoardDeviceDto = ds.Tables[0].ToList<BedBoardDeviceDto>();
            return View(BedBoardDeviceDto);
        }
        public IActionResult LocatorBoardDeviceList()
        {
            string sql = "SELECT * FROM eboards.v_locatorboarddevice ORDER BY locatorboarddevicename;";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            List<LocatorBoardDeviceDto> LocatorBoardDeviceDto = ds.Tables[0].ToList<LocatorBoardDeviceDto>();
            return View(LocatorBoardDeviceDto);
        }

        public IActionResult BedBoardManagerNew(string id)
        {
            DataSet dsBaseview = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseview.Tables[0], "baseviewnamespaceid", "baseviewnamespace");
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Please Select...",
                Value = ""
            });
            ViewBag.AttributeList = new SelectList(list, "Value", "Text");
            List<SelectListItem> areaSetting = new List<SelectListItem>();
            areaSetting.Add(new SelectListItem()
            {
                Text = "Please select . . .",
                Value = ""
            });
            areaSetting.Add(new SelectListItem()
            {
                Text = "Single Section",
                Value = "1"
            });
            areaSetting.Add(new SelectListItem()
            {
                Text = "Two Sections",
                Value = "2"
            });
            ViewBag.AreaSetting = new SelectList(areaSetting, "Value", "Text");


            return View();
        }
        public IActionResult LocatorBoardManagerNew(string id)
        {
            DataSet dsBaseview = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseview.Tables[0], "baseviewnamespaceid", "baseviewnamespace");

            DataSet dsList = DataServices.DataSetFromSQL("SELECT * FROM listsettings.listmanager ORDER BY listname");
            ViewBag.List = ToSelectList(dsList.Tables[0], "list_id", "listname");


            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Text = "Please Select...",
                Value = ""
            });
            ViewBag.AttributeList = new SelectList(list, "Value", "Text");
            return View();
        }
        public IActionResult BedBoardDeviceNew()
        {
            DataSet dsBedBoard = DataServices.DataSetFromSQL("SELECT bedboard_id, bedboardname FROM eboards.bedboard");
            ViewBag.BedBoard = ToSelectList(dsBedBoard.Tables[0], "bedboard_id", "bedboardname");

            DataSet dsWard = DataServices.DataSetFromSQL("SELECT wardcode, warddisplay FROM entitystorematerialised.meta_ward ORDER BY warddisplay");
            ViewBag.Ward = ToSelectList(dsWard.Tables[0], "wardcode", "warddisplay");
            return View();
        }
        public IActionResult LocatorBoardDeviceNew()
        {
            DataSet dsLocatorBoard = DataServices.DataSetFromSQL("SELECT Locatorboard_id, Locatorboardname FROM eboards.Locatorboard;");
            ViewBag.LocatorBoard = ToSelectList(dsLocatorBoard.Tables[0], "Locatorboard_id", "Locatorboardname");
            return View();
        }


        public IActionResult BedBoardDeviceView(string id)
        {
            DataSet dsBedBoard = DataServices.DataSetFromSQL("SELECT bedboard_id, bedboardname FROM eboards.bedboard");
            ViewBag.BedBoard = ToSelectList(dsBedBoard.Tables[0], "bedboard_id", "bedboardname");

            DataSet dsWard = DataServices.DataSetFromSQL("SELECT wardcode, warddisplay FROM entitystorematerialised.meta_ward ORDER BY warddisplay");
            ViewBag.Ward = ToSelectList(dsWard.Tables[0], "wardcode", "warddisplay");

            string sql = "SELECT * FROM eboards.bedboarddevice WHERE bedboarddevice_id = @bedboarddevice_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", id)
            };
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            BedBoardDeviceModel model = new BedBoardDeviceModel();
            model.DeviceId = id;
            try
            {
                model.DeviceName = dt.Rows[0]["bedboarddevicename"].ToString();
            }
            catch { }

            try
            {
                model.IPAddress = dt.Rows[0]["deviceipaddress"].ToString();
            }
            catch { }
            model.BedBoardId = dt.Rows[0]["bedboard_id"].ToString();
            model.WardId = dt.Rows[0]["locationward"].ToString(); //dt.Rows[0]["bedboard_id"].ToString();

            DataSet dsBayRoom = DataServices.DataSetFromSQL("SELECT baycode, baydisplay FROM entitystorematerialised.meta_wardbay WHERE wardcode = '" +
                dt.Rows[0]["locationward"].ToString() + "' ORDER BY baydisplay;");
            ViewBag.BayRoom = ToSelectList(dsBayRoom.Tables[0], "baycode", "baydisplay");

            model.BayRoomId = dt.Rows[0]["locationbayroom"].ToString();

            DataSet dsBed = DataServices.DataSetFromSQL("SELECT wardbaybed_id, beddisplay FROM entitystorematerialised.meta_wardbaybed WHERE " +
                "wardcode = '" + dt.Rows[0]["locationward"].ToString() + "' AND baycode = '" + dt.Rows[0]["locationbayroom"].ToString()  + "' ORDER BY beddisplay;");
            ViewBag.Bed = ToSelectList(dsBed.Tables[0], "wardbaybed_id", "beddisplay");

            model.BedId = dt.Rows[0]["locationbed"].ToString();

            string uri = SynapseHelpers.GetEBoardURL();
            ViewBag.BoardUrl = uri + "DynamicBedBoard.aspx";
            return View(model);
        }
        public IActionResult LocatorBoardDeviceView(string id)
        {
            DataSet dsLocatorBoard = DataServices.DataSetFromSQL("SELECT Locatorboard_id, Locatorboardname FROM eboards.Locatorboard;");
            ViewBag.LocatorBoard = ToSelectList(dsLocatorBoard.Tables[0], "Locatorboard_id", "Locatorboardname");
            string sql = "SELECT * FROM eboards.locatorboarddevice WHERE locatorboarddevice_id = @locatorboarddevice_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("locatorboarddevice_id", id)
            };
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            LocatorDeviceModel model = new LocatorDeviceModel();
            model.DeviceId = id;
            try
            {
                model.DeviceName = dt.Rows[0]["Locatorboarddevicename"].ToString();
            }
            catch { }

            try
            {
                model.IPAddress = dt.Rows[0]["deviceipaddress"].ToString();
            }
            catch { }
            model.LocatorBoardId = dt.Rows[0]["Locatorboard_id"].ToString();
            try
            {
                model.LocationCode = dt.Rows[0]["locationid"].ToString();
            }
            catch { }

            return View(model);
        }
        public IActionResult BedBoardManagerView(string id)
        {
            DataSet dsBaseviewNamespace = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseviewNamespace.Tables[0], "baseviewnamespaceid", "baseviewnamespace");
            List<SelectListItem> areaSetting = new List<SelectListItem>();
            areaSetting.Add(new SelectListItem()
            {
                Text = "Please select . . .",
                Value = ""
            });
            areaSetting.Add(new SelectListItem()
            {
                Text = "Single Section",
                Value = "1"
            });
            areaSetting.Add(new SelectListItem()
            {
                Text = "Two Sections",
                Value = "2"
            });
            ViewBag.AreaSetting = new SelectList(areaSetting, "Value", "Text");
            ViewBag.PreviewURL = GetPreviewURL(id);
            string sql = "SELECT * FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboard_id", id)
            };
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            BoardModel model = new BoardModel();
            model.BedBoardId = id;
            try
            {
                model.BedBoardName = dt.Rows[0]["bedboardname"].ToString();
            }
            catch { }

            try
            {
                model.BedBoardDescription = dt.Rows[0]["bedboarddescription"].ToString();
            }
            catch { }
            model.BedBoardId = id;
            model.BaseViewNamespaceId = dt.Rows[0]["baseviewnamespace_id"].ToString();
            model.BaseViewId = dt.Rows[0]["baseview_id"].ToString();
            model.PersonIDField = dt.Rows[0]["baseviewpersonidfield"].ToString();
            model.EncounterIDField = dt.Rows[0]["baseviewencounteridfield"].ToString();
            model.WardField = dt.Rows[0]["baseviewwardfield"].ToString();
            model.BedField = dt.Rows[0]["baseviewbedfield"].ToString();
            model.TopSetting = dt.Rows[0]["topsetting"].ToString();
            model.TopField = dt.Rows[0]["topfield"].ToString();
            model.TopLeftField = dt.Rows[0]["topleftfield"].ToString();
            model.TopRightField = dt.Rows[0]["toprightfield"].ToString();
            model.MiddleSetting = dt.Rows[0]["middlesetting"].ToString();
            model.MiddleField = dt.Rows[0]["middlefield"].ToString();
            model.MiddleLeftField = dt.Rows[0]["middleleftfield"].ToString();
            model.MiddleRightField = dt.Rows[0]["middlerightfield"].ToString();
            model.BottomSetting = dt.Rows[0]["bottomsetting"].ToString();
            model.BottomField = dt.Rows[0]["bottomfield"].ToString();
            model.BottomLeftField = dt.Rows[0]["bottomleftfield"].ToString();
            model.BottomRightField = dt.Rows[0]["bottomrightfield"].ToString();

            string sqlBaseView = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramListBaseView = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", model.BaseViewNamespaceId)
            };
            DataSet dsBaseView = DataServices.DataSetFromSQL(sqlBaseView, paramListBaseView);
            ViewBag.BaseView = ToSelectList(dsBaseView.Tables[0], "baseview_id", "baseviewname");

            string sqlAttribute = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramListAttribute = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", model.BaseViewId)
            };
            DataSet dsAttributey = DataServices.DataSetFromSQL(sqlAttribute, paramListAttribute);
            ViewBag.AttributeList = ToSelectList(dsAttributey.Tables[0], "attributename", "attributename");



            return View(model);
        }
        public IActionResult LocatorBoardManagerView(string id)
        {
            DataSet dsBaseview = DataServices.DataSetFromSQL("SELECT * FROM listsettings.baseviewnamespace ORDER BY baseviewnamespace");
            ViewBag.BaseviewNamespace = ToSelectList(dsBaseview.Tables[0], "baseviewnamespaceid", "baseviewnamespace");

            DataSet dsList = DataServices.DataSetFromSQL("SELECT * FROM listsettings.listmanager ORDER BY listname");
            ViewBag.List = ToSelectList(dsList.Tables[0], "list_id", "listname");

            string sql = "SELECT * FROM eboards.locatorboard WHERE locatorboard_id = @locatorboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("locatorboard_id", id)
            };
            ds = DataServices.DataSetFromSQL(sql, paramList);

            DataTable dt = ds.Tables[0];
            LocatorBoardModel model = new LocatorBoardModel();
            model.LocatorBoardId = id;
            try
            {
                model.LocatorBoardName = dt.Rows[0]["locatorboardname"].ToString();
            }
            catch { }

            try
            {
                model.LocatorBoardDescription = dt.Rows[0]["locatorboarddescription"].ToString();
            }
            catch { }
            model.BaseViewNamespaceId = dt.Rows[0]["locationbaseviewnamespace_id"].ToString();
            model.ListId = dt.Rows[0]["list_id"].ToString();
            model.ListLocationField = dt.Rows[0]["listlocationfield"].ToString();
            model.BaseViewId = dt.Rows[0]["locationbaseview_id"].ToString();
            model.LocationIDField = dt.Rows[0]["locationidfield"].ToString();
            model.Heading = dt.Rows[0]["locationdisplayfield"].ToString();
            model.TopLeftField = dt.Rows[0]["topleftfield"].ToString();
            model.TopRightField = dt.Rows[0]["toprightfield"].ToString();
            ViewBag.PreviewURL = SynapseHelpers.GetEBoardURL() + "locatorboard.aspx?id=" + id + "&Location=XXXXX";

            string sqlBaseView = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramListBaseView = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", model.BaseViewNamespaceId)
            };
            DataSet dsBaseView = DataServices.DataSetFromSQL(sqlBaseView, paramListBaseView);
            ViewBag.BaseView = ToSelectList(dsBaseView.Tables[0], "baseview_id", "baseviewname");

            string sqlAtt = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramListAtt = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", model.BaseViewId)
            };
            DataSet dsAtt = DataServices.DataSetFromSQL(sqlAtt, paramListAtt);
            List<SelectListItem> list = new List<SelectListItem>();
            ViewBag.AttributeList = ToSelectList(dsAtt.Tables[0], "attributename", "attributename");
            return View(model);
        }


        [HttpPost]
        public IActionResult BedBoardManagerNewSave(BoardModel model)
        {
            string sql = @"INSERT INTO eboards.bedboard(
                                _createdby, 
	                            bedboard_id, 
	                            bedboardname, 
	                            bedboarddescription, 
	                            baseviewnamespace_id, 
	                            baseview_id, 
	                            baseviewpersonidfield, 
	                            baseviewencounteridfield, 
	                            baseviewwardfield, 
	                            baseviewbedfield, 
	                            topsetting, 
	                            middlesetting, 
	                            bottomsetting, 
	                            topfield, 
	                            topleftfield, 
	                            toprightfield, 
	                            middlefield, 
	                            middleleftfield, 
	                            middlerightfield, 
	                            bottomfield, 
	                            bottomleftfield, 
	                            bottomrightfield
                            )
                            VALUES(
                                @_createdby,
                                @bedboard_id,
                                @bedboardname,
                                @bedboarddescription,
                                @baseviewnamespace_id,
                                @baseview_id,
                                @baseviewpersonidfield,
                                @baseviewencounteridfield,
                                @baseviewwardfield,
                                @baseviewbedfield,
                                @topsetting,
                                @middlesetting,
                                @bottomsetting,
                                @topfield,
                                @topleftfield,
                                @toprightfield,
                                @middlefield,
                                @middleleftfield,
                                @middlerightfield,
                                @bottomfield,
                                @bottomleftfield,
                                @bottomrightfield
                            )";

            string newID = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("bedboard_id", newID),
                new KeyValuePair<string, string>("bedboardname", model.BedBoardName),
                new KeyValuePair<string, string>("bedboarddescription", model.BedBoardDescription),
                new KeyValuePair<string, string>("baseviewnamespace_id", model.BaseViewNamespaceId),
                new KeyValuePair<string, string>("baseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("baseviewpersonidfield", model.PersonIDField),
                new KeyValuePair<string, string>("baseviewencounteridfield", model.EncounterIDField),
                new KeyValuePair<string, string>("baseviewwardfield", model.WardField),
                new KeyValuePair<string, string>("baseviewbedfield", model.BedField),
                new KeyValuePair<string, string>("topsetting", model.TopSetting),
                new KeyValuePair<string, string>("middlesetting", model.MiddleSetting),
                new KeyValuePair<string, string>("bottomsetting", model.BottomSetting),
                new KeyValuePair<string, string>("topfield", model.TopField),
                new KeyValuePair<string, string>("topleftfield", model.TopLeftField),
                new KeyValuePair<string, string>("toprightfield", model.TopRightField),
                new KeyValuePair<string, string>("middlefield", model.MiddleField),
                new KeyValuePair<string, string>("middleleftfield", model.MiddleLeftField),
                new KeyValuePair<string, string>("middlerightfield", model.MiddleRightField),
                new KeyValuePair<string, string>("bottomfield", model.BottomField),
                new KeyValuePair<string, string>("bottomleftfield", model.BottomLeftField),
                new KeyValuePair<string, string>("bottomrightfield", model.BottomRightField)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult LocatorBoardManagerNewSave(LocatorBoardModel model)
        {
            string sql = @"INSERT INTO eboards.LocatorBoard(
                                _createdby, 
	                            LocatorBoard_id, 
	                            LocatorBoardname, 
	                            LocatorBoarddescription, 
	                            list_id,
                                listlocationfield,
                                locationbaseviewnamespace_id, 
	                            locationbaseview_id,              
                                locationidfield,
                                locationdisplayfield,
	                            topleftfield, 
	                            toprightfield
                            )
                            VALUES(
                                @_createdby, 
	                            @LocatorBoard_id, 
	                            @LocatorBoardname, 
	                            @LocatorBoarddescription, 
	                            @list_id,
                                @listlocationfield,
                                @locationbaseviewnamespace_id, 
	                            @locationbaseview_id,       
                                @locationidfield,
                                @locationdisplayfield,
	                            @topleftfield, 
	                            @toprightfield
                            )";

            string newID = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("LocatorBoard_id", newID),
                new KeyValuePair<string, string>("LocatorBoardname", model.LocatorBoardName),
                new KeyValuePair<string, string>("LocatorBoarddescription", model.LocatorBoardDescription),

                new KeyValuePair<string, string>("list_id", model.ListId),
                new KeyValuePair<string, string>("listlocationfield", model.ListLocationField),

                new KeyValuePair<string, string>("locationbaseviewnamespace_id", model.BaseViewNamespaceId),
                new KeyValuePair<string, string>("locationbaseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("locationidfield", model.LocationIDField),
                new KeyValuePair<string, string>("locationdisplayfield", model.Heading),
                new KeyValuePair<string, string>("topleftfield", model.TopLeftField),
                new KeyValuePair<string, string>("toprightfield", model.TopRightField)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult BedBoardManagerViewSave(BoardModel model)
        {
            string sql = @"UPDATE eboards.bedboard SET                                
	                            baseview_id = @baseview_id, 
	                            bedboardname = @bedboardname, 
	                            bedboarddescription = @bedboarddescription, 
	                            baseviewnamespace_id = @baseviewnamespace_id, 	                            
	                            baseviewpersonidfield = @baseviewpersonidfield, 
	                            baseviewencounteridfield = @baseviewencounteridfield, 
	                            baseviewwardfield = @baseviewwardfield, 
	                            baseviewbedfield = @baseviewbedfield, 
	                            topsetting = @topsetting, 
	                            middlesetting = @middlesetting, 
	                            bottomsetting = @bottomsetting, 
	                            topfield = @topfield, 
	                            topleftfield = @topleftfield, 
	                            toprightfield = @toprightfield, 
	                            middlefield = @middlefield, 
	                            middleleftfield = @middleleftfield, 
	                            middlerightfield = @middlerightfield, 
	                            bottomfield = @bottomfield, 
	                            bottomleftfield = @bottomleftfield, 
	                            bottomrightfield = @bottomrightfield
                        WHERE bedboard_id = @bedboard_id";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("bedboard_id", model.BedBoardId),
                new KeyValuePair<string, string>("bedboardname", model.BedBoardName),
                new KeyValuePair<string, string>("bedboarddescription", model.BedBoardDescription),
                new KeyValuePair<string, string>("baseviewnamespace_id", model.BaseViewNamespaceId),
                new KeyValuePair<string, string>("baseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("baseviewpersonidfield", model.PersonIDField),
                new KeyValuePair<string, string>("baseviewencounteridfield", model.EncounterIDField),
                new KeyValuePair<string, string>("baseviewwardfield", model.WardField),
                new KeyValuePair<string, string>("baseviewbedfield", model.BedField),
                new KeyValuePair<string, string>("topsetting", model.TopSetting),
                new KeyValuePair<string, string>("middlesetting", model.MiddleSetting),
                new KeyValuePair<string, string>("bottomsetting", model.BottomSetting),
                new KeyValuePair<string, string>("topfield", model.TopField),
                new KeyValuePair<string, string>("topleftfield", model.TopLeftField),
                new KeyValuePair<string, string>("toprightfield", model.TopRightField),
                new KeyValuePair<string, string>("middlefield", model.MiddleField),
                new KeyValuePair<string, string>("middleleftfield", model.MiddleLeftField),
                new KeyValuePair<string, string>("middlerightfield", model.MiddleRightField),
                new KeyValuePair<string, string>("bottomfield", model.BottomField),
                new KeyValuePair<string, string>("bottomleftfield", model.BottomLeftField),
                new KeyValuePair<string, string>("bottomrightfield", model.BottomRightField)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult LocatorBoardManagerViewSave(LocatorBoardModel model)
        {
            string sql = @"UPDATE eboards.LocatorBoard SET
                                _createdby = @_createdby, 	                             
	                            LocatorBoardname = @LocatorBoardname, 
	                            LocatorBoarddescription = @LocatorBoarddescription, 
	                            list_id = @list_id,
                                listlocationfield = @listlocationfield,
                                locationbaseviewnamespace_id = @locationbaseviewnamespace_id, 
	                            locationbaseview_id = @locationbaseview_id,              
                                locationidfield = @locationidfield,
                                locationdisplayfield = @locationdisplayfield,
	                            topleftfield = @topleftfield, 
	                            toprightfield = @toprightfield
                            WHERE LocatorBoard_id = @LocatorBoard_id";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("LocatorBoard_id", model.LocatorBoardId),
                new KeyValuePair<string, string>("LocatorBoardname", model.LocatorBoardName),
                new KeyValuePair<string, string>("LocatorBoarddescription", model.LocatorBoardDescription),

                new KeyValuePair<string, string>("list_id", model.ListId),
                new KeyValuePair<string, string>("listlocationfield", model.ListLocationField),

                new KeyValuePair<string, string>("locationbaseviewnamespace_id", model.BaseViewNamespaceId),
                new KeyValuePair<string, string>("locationbaseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("locationidfield", model.LocationIDField),
                new KeyValuePair<string, string>("locationdisplayfield", model.Heading),
                new KeyValuePair<string, string>("topleftfield", model.TopLeftField),
                new KeyValuePair<string, string>("toprightfield", model.TopRightField)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }

        [HttpPost]
        public IActionResult BedBoardDeviceNewSave(BedBoardDeviceModel model)
        {
            string sql = @"INSERT INTO eboards.bedboarddevice(
                        bedboarddevice_id, bedboarddevicename, bedboard_id, deviceipaddress, locationward, locationbayroom, locationbed)
	                    VALUES(@bedboarddevice_id, @bedboarddevicename, @bedboard_id, @deviceipaddress, @locationward, @locationbayroom, @locationbed);";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", System.Guid.NewGuid().ToString()),
                new KeyValuePair<string, string>("bedboarddevicename", model.DeviceName),
                new KeyValuePair<string, string>("bedboard_id", model.BedBoardId),
                new KeyValuePair<string, string>("deviceipaddress", model.IPAddress),
                new KeyValuePair<string, string>("locationward", model.WardId),
                new KeyValuePair<string, string>("locationbayroom", model.BayRoomId),
                new KeyValuePair<string, string>("locationbed", model.BedId)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }

        [HttpPost]
        public IActionResult LocatorBoardDeviceNewSave(LocatorDeviceModel model)
        {
            string sql = @"INSERT INTO eboards.Locatorboarddevice(
                        Locatorboarddevice_id, Locatorboarddevicename, Locatorboard_id, deviceipaddress, locationid)
	                    VALUES(@Locatorboarddevice_id, @Locatorboarddevicename, @Locatorboard_id, @deviceipaddress, @locationid);";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("Locatorboarddevice_id", System.Guid.NewGuid().ToString()),
                new KeyValuePair<string, string>("Locatorboarddevicename", model.DeviceName),
                new KeyValuePair<string, string>("Locatorboard_id", model.LocatorBoardId),
                new KeyValuePair<string, string>("deviceipaddress", model.IPAddress),
                new KeyValuePair<string, string>("locationid", model.LocationCode) };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }


        [HttpPost]
        public IActionResult BedBoardDeviceViewSave(BedBoardDeviceModel model)
        {
            string sql = @"UPDATE eboards.bedboarddevice SET
                        bedboarddevicename = @bedboarddevicename, bedboard_id = @bedboard_id, deviceipaddress = @deviceipaddress, locationward = @locationward, locationbayroom = @locationbayroom, locationbed = @locationbed
	                    WHERE bedboarddevice_id = @bedboarddevice_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboarddevice_id", model.DeviceId),
                new KeyValuePair<string, string>("bedboarddevicename", model.DeviceName),
                new KeyValuePair<string, string>("bedboard_id", model.BedBoardId),
                new KeyValuePair<string, string>("deviceipaddress", model.IPAddress),
                new KeyValuePair<string, string>("locationward", model.WardId),
                new KeyValuePair<string, string>("locationbayroom", model.BayRoomId),
                new KeyValuePair<string, string>("locationbed", model.BedId)
            };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }

        [HttpPost]
        public IActionResult LocatorBoardDeviceViewSave(LocatorDeviceModel model)
        {
            string sql = @"UPDATE eboards.Locatorboarddevice SET
                        Locatorboarddevicename = @Locatorboarddevicename, Locatorboard_id = @Locatorboard_id, deviceipaddress = @deviceipaddress, locationid = @locationid
	                    WHERE Locatorboarddevice_id = @Locatorboarddevice_id;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("Locatorboarddevice_id", model.DeviceId),
                new KeyValuePair<string, string>("Locatorboarddevicename", model.DeviceName),
                new KeyValuePair<string, string>("Locatorboard_id", model.LocatorBoardId),
                new KeyValuePair<string, string>("deviceipaddress", model.IPAddress),
                new KeyValuePair<string, string>("locationid", model.LocationCode) };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }

        public IActionResult DeleteBedBoard(string bedBoardId)
        {
            string sql = @"DELETE FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            string newID = bedBoardId;
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboard_id", newID)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }

        public IActionResult DeleteLocatorBoard(string locatorBoardId)
        {
            string sql = @"DELETE FROM eboards.LocatorBoard WHERE LocatorBoard_id = @LocatorBoard_id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("LocatorBoard_id", locatorBoardId)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        [HttpPost]
        public IActionResult BedBoardManagerCloneSave(BoardModel model)
        {
            string sql = @"INSERT INTO eboards.bedboard(
                                _createdby, 
	                            bedboard_id, 
	                            bedboardname, 
	                            bedboarddescription, 
	                            baseviewnamespace_id, 
	                            baseview_id, 
	                            baseviewpersonidfield, 
	                            baseviewencounteridfield, 
	                            baseviewwardfield, 
	                            baseviewbedfield, 
	                            topsetting, 
	                            middlesetting, 
	                            bottomsetting, 
	                            topfield, 
	                            topleftfield, 
	                            toprightfield, 
	                            middlefield, 
	                            middleleftfield, 
	                            middlerightfield, 
	                            bottomfield, 
	                            bottomleftfield, 
	                            bottomrightfield
                            )
                            VALUES(
                                @_createdby,
                                @bedboard_id,
                                @bedboardname,
                                @bedboarddescription,
                                @baseviewnamespace_id,
                                @baseview_id,
                                @baseviewpersonidfield,
                                @baseviewencounteridfield,
                                @baseviewwardfield,
                                @baseviewbedfield,
                                @topsetting,
                                @middlesetting,
                                @bottomsetting,
                                @topfield,
                                @topleftfield,
                                @toprightfield,
                                @middlefield,
                                @middleleftfield,
                                @middlerightfield,
                                @bottomfield,
                                @bottomleftfield,
                                @bottomrightfield
                            )";

            string newID = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("_createdby", HttpContext.Session.GetString(SynapseSession.FullName)),
                new KeyValuePair<string, string>("bedboard_id", newID),
                new KeyValuePair<string, string>("bedboardname", model.BedBoardName + "_Clone"),
                new KeyValuePair<string, string>("bedboarddescription", model.BedBoardDescription),
                new KeyValuePair<string, string>("baseviewnamespace_id", model.BaseViewNamespaceId),
                new KeyValuePair<string, string>("baseview_id", model.BaseViewId),
                new KeyValuePair<string, string>("baseviewpersonidfield", model.PersonIDField),
                new KeyValuePair<string, string>("baseviewencounteridfield", model.EncounterIDField),
                new KeyValuePair<string, string>("baseviewwardfield", model.WardField),
                new KeyValuePair<string, string>("baseviewbedfield", model.BedField),
                new KeyValuePair<string, string>("topsetting", model.TopSetting),
                new KeyValuePair<string, string>("middlesetting", model.MiddleSetting),
                new KeyValuePair<string, string>("bottomsetting", model.BottomSetting),
                new KeyValuePair<string, string>("topfield", model.TopField),
                new KeyValuePair<string, string>("topleftfield", model.TopLeftField),
                new KeyValuePair<string, string>("toprightfield", model.TopRightField),
                new KeyValuePair<string, string>("middlefield", model.MiddleField),
                new KeyValuePair<string, string>("middleleftfield", model.MiddleLeftField),
                new KeyValuePair<string, string>("middlerightfield", model.MiddleRightField),
                new KeyValuePair<string, string>("bottomfield", model.BottomField),
                new KeyValuePair<string, string>("bottomleftfield", model.BottomLeftField),
                new KeyValuePair<string, string>("bottomrightfield", model.BottomRightField)
            };
            DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            return Json("OK");
        }
        public IActionResult RandomBedBoard(string bedBoardId)
        {
            string url = GetPreviewURL(bedBoardId);
            return Json(url);
        }
        private string GetPreviewURL(string bedBoardId)
        {
            string sql = "SELECT * FROM eboards.bedboard WHERE bedboard_id = @bedboard_id;";
            DataSet ds = new DataSet();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("bedboard_id", bedBoardId)
            };
            ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];
            string baseview_id = "";
            try
            {
                baseview_id = dt.Rows[0]["baseview_id"].ToString();
            }
            catch { }
            string baseviewname = SynapseHelpers.GetBaseViewNameAndNamespaceFromID(baseview_id);
            string PersonIDField = "";
            try
            {
                PersonIDField = dt.Rows[0]["baseviewpersonidfield"].ToString();
            }
            catch { }

            string EncounterIDField = "";
            try
            {
                EncounterIDField = dt.Rows[0]["baseviewencounteridfield"].ToString();
            }
            catch { }

            string WardField = "";
            try
            {
                WardField = dt.Rows[0]["baseviewwardfield"].ToString();
            }
            catch { }

            string BedField = "";
            try
            {
                BedField = dt.Rows[0]["baseviewbedfield"].ToString();
            }
            catch { }


            string sqlBoard = "SELECT " + WardField + " as WardField, " + BedField + " as BedField" +
                              " FROM baseview." + baseviewname + " order by random() LIMIT 1;";

            var paramListbOARD = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet dsBoard = DataServices.DataSetFromSQL(sqlBoard, paramListbOARD);

            DataTable dtBoard = dsBoard.Tables[0];

            string ward = "";
            try
            {
                ward = dtBoard.Rows[0]["WardField"].ToString();
            }
            catch { }

            string bed = "";
            try
            {
                bed = dtBoard.Rows[0]["BedField"].ToString();
            }
            catch { }

            string apiURL = SynapseHelpers.GetAPIURL();

            string uri = SynapseHelpers.GetEBoardURL() + "bedboard.aspx?BedBoardID=" + bedBoardId + "&Ward=" + ward + "&Bed=" + bed;

            return uri;

        }
        public JsonResult BayRoomJsonList(string wardId)
        {
            string sql = "SELECT baycode, baydisplay FROM entitystorematerialised.meta_wardbay WHERE wardcode = @id ORDER BY baydisplay;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", wardId)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            return Json(dsEntity.Tables[0]);
        }
        public JsonResult BedJsonList(string wardId, string bayRoomId)
        {
            string sql = "SELECT wardbaybed_id, beddisplay FROM entitystorematerialised.meta_wardbaybed WHERE wardcode = @id AND baycode = @bayroomlocation_id ORDER BY beddisplay;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", wardId),
                new KeyValuePair<string, string>("bayroomlocation_id", bayRoomId)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);

            return Json(dsEntity.Tables[0]);
        }
        public JsonResult BaseViewJsonList(string id)
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseviewnamespaceid = @id ORDER BY baseviewname;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            return Json(dsEntity.Tables[0]);
        }
        public JsonResult BaseViewContextFieldsJson(string id)
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id = @id ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            return Json(dsEntity.Tables[0]);
        }
        public JsonResult ListBaseViewContextFieldsJson(string id)
        {
            string sql = "SELECT attributename FROM listsettings.baseviewattribute WHERE baseview_id IN (SELECT baseview_id FROM listsettings.listmanager WHERE list_id =  @id) ORDER BY attributename;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };
            DataSet dsEntity = DataServices.DataSetFromSQL(sql, paramList);
            return Json(dsEntity.Tables[0]);
        }
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    }
}

