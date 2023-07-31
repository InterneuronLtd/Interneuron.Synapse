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


﻿namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public class DmdVmpIngredientDTO
    {
        public string Isid { get; set; }
        public long? BasisStrntcd { get; set; }
        public string BsSubid { get; set; }
        public decimal? StrntNmrtrVal { get; set; }
        public string StrntNmrtrUomcd { get; set; }
        public decimal? StrntDnmtrVal { get; set; }
        public string StrntDnmtrUomcd { get; set; }
    }
}
