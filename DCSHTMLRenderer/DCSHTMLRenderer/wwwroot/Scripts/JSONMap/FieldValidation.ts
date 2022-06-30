//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2022  Interneuron CIC

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
﻿import { CompareDateValues } from "../Enumerations/CompareDateValues";
import { Comparators } from "../Enumerations/Comparator";

export class FieldValidation {
    validationtype: string;
    fieldValidationPattern: string;
    validationErrorMsg: string;
    compareField: string;
    comparator: Comparators;
    compareDateValue: CompareDateValues[];
    compareValue: number[];
    minLength: number;
    maxLength: number;
}