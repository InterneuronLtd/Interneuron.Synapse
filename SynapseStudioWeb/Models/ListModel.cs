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


﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class ListNamespaceModel
    {
        public string listnamespace { get; set; }
        [Required(ErrorMessage = "Please enter a name for the new list namespace")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Namespace can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyListNamespace", controller: "List")]
        public string ListNamespace { get; set; }
        public string ListNamespaceDescription { get; set; }
        public List<ListNamespaceDto> ListNamespaceDto { get; set; }
    }
    public class ListNamespaceDto
    {
        public string listnamespaceid { get; set; }
        public string listnamespace { get; set; }

    }
    public class ListModel
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public List<ListDto> ListDto { get; set; }
        public List<QuestionDto> QuestionDto { get; set; }
        public List<QuestionCollectioDto> QuestionCollectioDto { get; set; }

    }
    public class ListDto
    {
        public string List_Id { get; set; }
        public string ListName { get; set; }

    }
    public class ListDetailDto
    {
        public string ListDetail { get; set; }
        public string ListDescription { get; set; }

    }
    public class QuestionDto
    {
        public string Question_Id { get; set; }

        public string QuestionQuickName { get; set; }
        public string DefaultContextFieldName { get; set; }
    }
    public class QuestionCollectioDto
    {
        public string QuestionOptionCollection_Id { get; set; }

        public string QuestionOptionCollectionName { get; set; }
    }

    public class ListManagerNewModel
    {
        public string NamespaceId { get; set; }
        public string ListId { get; set; }
        [Required(ErrorMessage = "Please enter a name for the new List")]
        [Remote(action: "VerifyListName", controller: "List")]
        public string ListName { get; set; }
        public string ListComments { get; set; }
        [Required(ErrorMessage = "Please select the baseview namespace")]
        public string BaseViewNamespaceId { get; set; }
        [Required(ErrorMessage = "Please select the associated baseview for this list")]
        public string BaseViewId { get; set; }
        [Required(ErrorMessage = "Please select the entity that defines the default context")]
        public string DefaultContextId { get; set; }
        [Required(ErrorMessage = "Please select the field from the baseview that matches the default context key defined")]
        public string MatchContextFieldId { get; set; }
        public string PatientBannerFieldId { get; set; }
        public string RowCSSFieldId { get; set; }
        public string TableClass { get; set; }
        public string TableHeaderClass { get; set; }
        public string DefaultTableRowCSS { get; set; }
        public string PersonaContextFieldId { get; set; }
        public string BaseviewFieldId { get; set; }
        public string WardPersonaContextFieldId { get; set; }
        public string CUPersonaContextFieldId { get; set; }
        public string SpecialtyPersonaContextFieldId { get; set; }
        public string TeamPersonaContextFieldId { get; set; }

        public string SnapshotLine1Id { get; set; }
        public string SnapshotLine2Id { get; set; }
        public string SnapshotBadgeId { get; set; }
        public string DefaultSortColumnId { get; set; }
        public string DefaultSortOrderId { get; set; }
        [Required(ErrorMessage = "Please select the field from the baseview that matches the date context key defined")]
        public string DateContextField { get; set; }
        public List<ListDetailDto> ListDetailDto { get; set; }
        public List<PersonaListFilter> PersonaListFilters { get; set; }
    }
    public class PersonaListFilter
    {
        public string displayname { get; set; }
        public string persona_id { get; set; }
        public string field { get; set; }
        public string list_id { get; set; }
    }
    public class ListQuestionNewModel
    {
        public string QuestionID { get; set; }
        [Required(ErrorMessage = "Please enter a new name")]
        public string QuickName { get; set; }
        [Required(ErrorMessage = "Please select the default context")]
        public string DefaultContextId { get; set; }
        [Required(ErrorMessage = "Please select the type of question")]
        public string QuestionTypeId { get; set; }
        [Required(ErrorMessage = "Please select the internal option collection")]
        public string OptionTypeId { get; set; }
        public string OptionCollectionId { get; set; }
        public string DefaultValueText { get; set; }
        public string DefaultContextFieldName { get; set; }
        [Required(ErrorMessage = "Please enter the label text")]
        public string LabelText { get; set; }
        [Required(ErrorMessage = "Please enter the HTML snippet for the option's flag")]
        public string CustomHTML { get; set; }
        public string CustomHTMLAlt { get; set; }
        public string DefaultValueDate { get; set; }
        [Required(ErrorMessage = "Please enter the custom SQL statement to load the options")]
        public string OptionSQLStatement { get; set; }
    }
    public class ListOptionCollectionNewModel
    {
        public string OptionCollectionID { get; set; }
        [Required(ErrorMessage = "Please enter collection name")]
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }
        public List<ListOptionDto> ListOptionDto { get; set; }
    }
    public class ListOptionDto
    {

        public string OptionDisplayText { get; set; }
        public string OptionFlag { get; set; }
        public string QuestionOption_Id { get; set; }
    }

    public class ListOptionNewModel
    {
        public string OptionID { get; set; }
        public string OptionCollectionID { get; set; }
        public string OptionCollectionText { get; set; }
        [Required(ErrorMessage = "Please enter the value that you want to assocaite with the option")]
        public string OptionValueText { get; set; }
        [Required(ErrorMessage = "Please enter the value that you want to display for the option")]
        public string OptionDisplayText { get; set; }
        public string OptionFlag { get; set; }
        public string OptionFlagAlt { get; set; }

    }

    public class AvailableAttribute
    {
        public string baseviewattribute_id { get; set; }
        public string baseviewnamespaceid { get; set; }
        public string baseviewnamespace { get; set; }
        public string baseview_id { get; set; }
        public string baseviewname { get; set; }
        public string attributename { get; set; }
        public string datatype { get; set; }
        public int ordinalposition { get; set; }
        public Boolean isselected { get; set; }
    }

    public class SelectedAttribute
    {
        public string listattribute_id { get; set; }
        public string list_id { get; set; }
        public string baseviewattribute_id { get; set; }
        public string attributename { get; set; }
        public string datatype { get; set; }
        public string displayname { get; set; }
        public int ordinalposition { get; set; }
        public string defaultcssclassname { get; set; }
    }

    public class AvailableQuestion
    {
        public string question_id { get; set; }
        public string questiondisplay { get; set; }
        public Boolean isselected { get; set; }
    }



    public class SelectedQuestion
    {
        public string listquestion_id { get; set; }
        public string list_id { get; set; }
        public string question_id { get; set; }
        public string questiondisplay { get; set; }
    }
}
