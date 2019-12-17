//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

function GenerateFilterData(filterList, paramList, selectStatement, ordergroupbyStatement) {

    var filterobject = new Object();
    filterobject.filters = filterList;

    var paramobject = new Object();
    paramobject.filterparams = paramList;

    var selectobject = new Object();
    selectobject.selectstatement = selectStatement;

    var ordergroupbyobject = new Object();
    ordergroupbyobject.ordergroupbystatement = ordergroupbyStatement;

    var jdata = [];
    jdata.push(filterobject);
    jdata.push(paramobject);
    jdata.push(selectobject);
    jdata.push(ordergroupbyobject);
    var json = JSON.stringify(jdata);
    //console.log(json);
    return json;
}

function calculateAge(birthday) { // birthday is a date
    var ageDifMs = Date.now() - birthday.getTime();
    var ageDate = new Date(ageDifMs); // miliseconds from epoch            
    return Math.abs(ageDate.getUTCFullYear() - 1970);
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function convertDate(inputFormat) {
    function pad(s) { return (s < 10) ? '0' + s : s; }
    var d = new Date(inputFormat);
    return [pad(d.getDate()), pad(d.getMonth() + 1), d.getFullYear()].join('/');
}

function htmlEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function htmlDecode(value) {
    return $('<div/>').html(value).text();
}


function getCurrentDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = dd + '-' + mm + '-' + yyyy;
    return today;
}

function Left(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else
        return String(str).substring(0, n);
}

function Right(str, n) {
    if (n <= 0)
        return "";
    else if (n > String(str).length)
        return str;
    else {
        var iLen = String(str).length;
        return String(str).substring(iLen, iLen - n);
    }
}


function checkIsValidDate(dt) {

    var parts = "";

    try {
        parts = dt.split('-');
    }
    catch (ex) {
        return false;
    }
    //please put attention to the month (parts[0]), Javascript counts months from 0:
    // January - 0, February - 1, etc
    var newDT = new Date(parts[2], parts[1] - 1, parts[0]);


    return (!isNaN(Date.parse(newDT)));

}


function getDateFromString(dt) {

    var parts = "";
    try {
        parts = dt.split('-');
    }
    catch (ex) {
        return "";
    }



    //please put attention to the month (parts[0]), Javascript counts months from 0:
    // January - 0, February - 1, etc
    var dt = new Date(parts[2], parts[1] - 1, parts[0]);
    return dt;

}


function isInt(value) {
    return !isNaN(value) &&
        parseInt(Number(value)) == value &&
        !isNaN(parseInt(value, 10));
}


function formatDate(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    //return date.getDate() + "/" + date.getMonth() + 1 + "/" + date.getFullYear() + "  " + strTime;
    var day = date.getDate() + "/";
    var month = date.getMonth() + 1 + "/";

    return day + month + date.getFullYear() + "  " + strTime;
}


function reverseDate(date) {
    var newdate = date.split("-").reverse().join("-");
    //console.log("nd:" + newdate);
    return newdate;
}


function returnemptystringifnull(data) {
    var ret = "";
    if (data !== null && data !== '' && typeof data != 'undefined') {
        ret = data;
    }
    else {
        ret = 'nodata';
    }
    return ret;
}