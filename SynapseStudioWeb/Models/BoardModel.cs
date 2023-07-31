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


﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class BoardModel
    {

        public string BedBoardId { get; set; }
        [Required(ErrorMessage = "Enter a name for the bed board")]
        public string BedBoardName { get; set; }
        public string BedBoardDescription { get; set; }
        [Required(ErrorMessage = "Please select the baseview namespace")]
        public string BaseViewNamespaceId { get; set; }
        [Required(ErrorMessage = "Please select the baseview")]
        public string BaseViewId { get; set; }
        [Required(ErrorMessage = "Please select the PersonID field from the baseview")]
        public string PersonIDField { get; set; }
        [Required(ErrorMessage = "Please select the EncounterID field from the baseview")]
        public string EncounterIDField { get; set; }
        [Required(ErrorMessage = "Please select the Ward Code field from the baseview")]
        public string WardField { get; set; }
        [Required(ErrorMessage = "Please select the Bed Code field from the baseview")]
        public string BedField { get; set; }
        [Required(ErrorMessage = "Please select the top section setting")]
        public string TopSetting { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Top Section")]
        public string TopField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Top Left Section")]
        public string TopLeftField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Top Right Section")]
        public string TopRightField { get; set; }
        [Required(ErrorMessage = "Please select the Middle section setting")]
        public string MiddleSetting { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Middle Section")]
        public string MiddleField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Middle Left Section")]
        public string MiddleLeftField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Middle Right Section")]
        public string MiddleRightField { get; set; }
        [Required(ErrorMessage = "Please select the Bottom section setting")]
        public string BottomSetting { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Bottom Section")]
        public string BottomField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Bottom Left Section")]
        public string BottomLeftField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Bottom Right Section")]
        public string BottomRightField { get; set; }

        public List<BedBoardDto> BedBoardDto { get; set; }
        public List<LocatorBoardDto> LocatorBoardDto { get; set; }


    }
    public class LocatorBoardModel
    {

        public string LocatorBoardId { get; set; }
        [Required(ErrorMessage = "Enter a name for the locator board")]
        public string LocatorBoardName { get; set; }
        public string LocatorBoardDescription { get; set; }
        [Required(ErrorMessage = "Please select the underlying list for the board")]
        public string ListId { get; set; }
        [Required(ErrorMessage = "Please select the location id field for the list")]
        public string ListLocationField { get; set; }

        [Required(ErrorMessage = "Please select the location specific information baseview")]
        public string BaseViewNamespaceId { get; set; }
        [Required(ErrorMessage = "Please select the location specifice baseview")]
        public string BaseViewId { get; set; }
        [Required(ErrorMessage = "Please select the Bed Code field from the baseview")]
        public string LocationIDField { get; set; }
        [Required(ErrorMessage = "Please select the Heading column from the baseview")]
        public string Heading { get; set; }
        public string TopLeftField { get; set; }
        [Required(ErrorMessage = "Please select the field column from the baseview for the Top Right Section")]
        public string TopRightField { get; set; }
    }

    public class BedBoardDeviceModel
    {

        public string DeviceId { get; set; }
        [Required(ErrorMessage = "Enter a device name")]
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        [Required(ErrorMessage = "Please select the bed board")]
        public string BedBoardId { get; set; }
        [Required(ErrorMessage = "Please select the ward")]
        public string WardId { get; set; }
        [Required(ErrorMessage = "Please select the bay room")]
        public string BayRoomId { get; set; }
        [Required(ErrorMessage = "Please select the bed")]
        public string BedId { get; set; }
    }

    public class LocatorDeviceModel
    {
        public string DeviceId { get; set; }
        [Required(ErrorMessage = "Enter a device name")]
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        [Required(ErrorMessage = "Please select the locator board")]
        public string LocatorBoardId { get; set; }
        public string LocationCode { get; set; }
    }


    public class BedBoardDto
    {
        public string Bedboard_Id { get; set; }
        public string BedboardName { get; set; }

    }
    public class BedBoardDeviceDto
    {
        public string BedboardDevice_Id { get; set; }
        public string BedboardDeviceName { get; set; }
        public string DeviceipAddress { get; set; }
        public string BedboardName { get; set; }
        public string WardLocationDisplayName { get; set; }
        public string BedLocationDisplayName { get; set; }
        public string BayRoomLocationDisplayName { get; set; }
    }
    public class LocatorBoardDto
    {
        public string Locatorboard_Id { get; set; }
        public string LocatorboardName { get; set; }

    }

    public class LocatorBoardDeviceDto
    {
        public string LocatorboardDevice_Id { get; set; }
        public string LocatorboardDeviceName { get; set; }
        public string DeviceipAddress { get; set; }
        public string LocatorboardName { get; set; }
        public string LocationId { get; set; }
    }
}
