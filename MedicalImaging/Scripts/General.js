var ApiURL;
var WebURL;
var ChkLockInspect = "N";
var ViewOnlyQAUser = 0130;

$(document).ready(function () {
    //view onlyprofile
    var url = window.location.href;
    UserTypeCode = $("#hdnUserTypeCode").val();
    if (UserTypeCode == ViewOnlyQAUser) {
        HideEditColumn();
        HideInActiveColumn();
        HideAllMainAddButton();
    }
    $("#userName").html($("#hdnUserNameWithProfile").val());
    $("#userDept").html($("#hdnDeptName").val());
    $("#userLocation").html("<small>" + $("#hdnLocationName").val() + "</small>");
    $("#userFullName").html($("#hdnUserFullName").val());

});

/*Start*/

function HideAllMainAddButton() {
    $("#btnSetProject").hide(); //remove();
}

function HideAllSaveButton() {
    var UserTypeCode = $("#hdnUserTypeCode").val();
    if (UserTypeCode == ViewOnlyQAUser) {
        $(".btn-primary").hide();
    }
}

function HideInActiveColumn() {
    setTimeout(function () {
        $("table.dataTable thead tr th").each(function () {
            if ($(this).text() === "Inactive") {
                $(this).remove(); //css('display', 'none');  
            }
        });
        $("table.dataTable tbody tr td").each(function () {
            if ($(this).text() === " Inactive") {
                $(this).remove(); //css('display', 'none');
            }
        });
    }, 500);
}

function HideEditColumn() {

    setTimeout(function () {
        $("table.dataTable thead tr th").each(function () {
            if ($(this).text() === "Edit") {
                $(this).remove(); //css('display', 'none'); 
            }
            if ($(this).text() === "Data Entry Continue") { // For ProductReceipt
                $(this).remove(); //css('display', 'none'); 
            }
        });
        $("table.dataTable tbody tr td").each(function () {
            if ($(this).text() === "Edit") {
                //$(this).hide(); //css('display', 'none');
                $(this).remove(); //css('display', 'none');
            }
        });
    }, 500);

}

function HideAllAddButton() {
    var UserTypeCode = $("#hdniWorkflowStageId").val();
    if (UserTypeCode == ViewOnlyUser) {
        $(".margin").hide();
    }

}

/*End*/

function UnLockInspect() {
    debugger;
    ChkLockInspect = "Y";
}

/////For Disbale Inspect///////////////

    //f12
    $(document).keydown(function (e) {
        if (ChkLockInspect != "Y") {
            if (e.which === 123) {
                return false;
            }
        }
    });

    //Right click
    $(document).bind("contextmenu", function (e) {
        if (ChkLockInspect != "Y") {
            e.preventDefault();
        }
    });

//allow copy paste allow Phase 3 changes
    //Ctrl Key
    //$(document).bind('keydown', function (e) {
    //    if (ChkLockInspect != "Y") {
    //        if (e.ctrlKey) {
    //            e.preventDefault();
    //            return false;
    //        }
    //    }
    //});


///////////////////////////////////////

function AlertBox(type, name, message) {    
    var divdata = document.getElementById('msgPopUp')
    while (divdata.hasChildNodes()) {
        divdata.removeChild(divdata.lastChild);
    }
    var data = "";
    type = type.toUpperCase();
    if (type == "SUCCESS") {
        data += "<div class='alert alert-success alert-dismissible'>";
        data += " <h4><i class='icon fa fa-check'></i>" + name + "</h4>";
    }
    else if (type == "WARNING") {
        data += "<div class='alert alert-warning alert-dismissible'>";
        data += "<h4><i class='icon fa fa-warning'></i>" + name + "</h4>";
    }
    else if (type == "INFORMATION") {
        data += "<div class='alert alert-info alert-dismissible'>";
        data += "<h4><i class='icon fa fa-info'></i>" + name + "</h4>";
    }
    else if (type == "ERROR") {
        data += "<div class='alert alert-danger alert-dismissible'>";
        data += "<h4><i class='icon fa fa-ban'></i>" + name + "</h4>";
    }
   // data += "<button type='button' class='close' id='btnClose' data-dismiss='alert' aria-hidden='true'>X</button>";
   
    data += message;
    data += "</div>";
    $("#msgPopUp").append(data);
    $("#msgPopUp").modal('show');
    $("#btnClose").focus();
}

function Label(id, value, classval) {
    var label = null;
    return label = '<label id="' + id + '" for="'+""+'" class="'+classval+'">' + value + '</label>';
}

function TextBox(type, classVal, id, placeHolder, tabIndex, value) {    
    if (value == "" || value == '' || value == null) {
        var textbox = null;
        return textbox = '<input type="' + type + '" class="' + classVal + '" id="' + id + '" placeholder="' + placeHolder + '" tabindex="' + tabIndex + '">';
    }
    else {

        var textbox = "";
        //textbox = '<div class="input-group">';
        //textbox += '<input type="' + type + '" class="' + classVal + '" id="' + id + '" placeholder="' + placeHolder + '" tabindex="' + tabIndex + '" value="' + value + '" aria-label="">';
        //textbox += '<span class="input-group-addon">';
        ////textbox += '<span class="glyphicon glyphicon-heart"></span>';
        //textbox += '';
        //textbox += '</span>';
        //return textbox += '</div>';

        var textbox = null;
        //return textbox = '<input type="' + type + '" value="' + value + '">';
        return textbox = '<input type="' + type + '" class="' + classVal + '" id="' + id + '" placeholder="' + placeHolder + '" tabindex="' + tabIndex + '" value="' + value + '" style= "font-size: smaller;">';
        //textbox.disabled = true;
    }
    
}

function TextBoxWithCheckBox(type1, type2, classVal1, classVal2, id1, id2, placeHolder, tabIndex, value) {    
    if (value == "" || value == '' || value == null) {
        var textbox = null;
        textbox = '<div class="input-group">';
        textbox += '<span class="input-group-addon">';
        textbox += '<input type="' + type1 + '" class="' + classVal1 + '" id="' + id1 + '" aria-label="" tabindex="' + tabIndex + '" refid="'+ id2 +'">';
        textbox += '</span>';
        if (value == "" || value == '' || value == null) {
            textbox += '<input type="' + type2 + '" class="' + classVal2 + '" id="' + id2 + '" placeholder="' + placeHolder + '"  aria-label="">';
        }
        else {
            textbox += '<input type="' + type2 + '" class="' + classVal2 + '" id="' + id2 + '" placeholder="' + placeHolder + '"  value="' + value + '" aria-label="">';
        }
        textbox += '<span class="input-group-addon">';
        //textbox += '<span class="glyphicon glyphicon-heart"></span>';
        textbox += 'mm';
        textbox += '</span>';
        return textbox += '</div>';
    }
    else {
        //var textbox = "";
        //textbox = '<div class="input-group">';
        //textbox += '<input type="' + type2 + '" class="' + classVal2 + '" id="' + id2 + '" placeholder="' + placeHolder + '"  value="' + value + '" aria-label="">';
        //textbox += '<span class="input-group-addon">';
        ////textbox += '<span class="glyphicon glyphicon-heart"></span>';
        //textbox += 'mm';
        //textbox += '</span>';
        //return textbox += '</div>';

        var textbox = null;
        textbox = '<div class="input-group">';
        textbox += '<span class="input-group-addon">';
        //textbox += '<input type="' + type1 + '" class="' + classVal1 + '" id="' + id1 + '" aria-label="" tabindex="' + tabIndex + '" refid="' + id2 + '">';        
        textbox += '<input type="' + type1 + '" class="' + classVal1 + '" id="' + id1 + '" aria-label="" tabindex="' + tabIndex + '" refid="' + id2 + '">';
        textbox += '</span>';
        if (value == "" || value == '' || value == null) {
            textbox += '<input type="' + type2 + '" class="' + classVal2 + '" id="' + id2 + '" placeholder="' + placeHolder + '"  aria-label="">';
        }
        else {
            textbox += '<input type="' + type2 + '" class="' + classVal2 + '" id="' + id2 + '" placeholder="' + placeHolder + '"  value="' + value + '" aria-label="">';
        }
        textbox += '<span class="input-group-addon">';
        //textbox += '<span class="glyphicon glyphicon-heart"></span>';
        textbox += 'mm';
        textbox += '</span>';
        return textbox += '</div>';

    }
              
}

function DropDownWithCheckBox(type1, classVal1, classVal2, id1, id2, option, tabIndex, forVal, lesionType, value) {    
    var dropdown = '';
    var val = option.split(',');
        
    dropdown = '<div class="input-group">';
    //Do Not Delete Commebnted Code It is Fore Reference and Future Use.
    //dropdown += '<span class="input-group-addon">';
    //dropdown += '<input type="' + type1 + '" class="' + classVal1 + '" id="' + id1 + '" aria-label="" tabindex="' + tabIndex + '" refid="' + id2 + '">';
    //dropdown += '</span>';
    dropdown += '<select class="' + classVal2 + '" id="' + id2 + '" tabindex="' + tabIndex + '" name="' + forVal + '" >';
    for (var v = 0 ; v < val.length ; v++) {
        if (value == "" || value == null) {
            dropdown += '<option value="' + id2 + "_" + v + '">' + val[v] + '</option>';
        }
        else{
            if (value.toUpperCase() == val[v].toUpperCase()) {                
                dropdown += '<option value="' + id2 + "_" + v + '" selected>' + val[v] + '</option>';
            }
            else{
                dropdown += '<option value="' + id2 + "_" + v + '">' + val[v] + '</option>';
            }
        }  
    }
    dropdown += '</select>';
    dropdown += '<span class="input-group-addon">';
    //dropdown += '<span class="glyphicon glyphicon-heart"></span>';
    dropdown += lesionType;
    dropdown += '</span>';
    return dropdown += '</div>';
}

function TextArea(type, classVal, id, placeHolder, tabIndex) {    
    var textArea = null;
    return textArea = '<input type="' + type + '" class="' + classVal + '" id="' + id + '" placeholder="' + placeHolder + '" tabindex="' + tabIndex + '">';
}

function DropDown(classVal, id, option, tabIndex, forVal, value) {    
    var dropdown = '';
    var val = option.split(',');
    dropdown += '<select class="' + classVal + '" id="' + id + '" tabindex="' + tabIndex + '" name="' + forVal + '" >';
    for (var v = 0 ; v < val.length ; v++) {
        if (value == "" || value == null) {
            dropdown += '<option value="' + id + "_" + v + '">' + val[v] + '</option>';
        }
        else {
            if (value.trim().toUpperCase() == val[v].trim().toUpperCase()){                
                dropdown += '<option value="' + id + "_" + v + '" selected>' + val[v] + '</option>';
            }
            else {
                dropdown += '<option value="' + id + "_" + v + '">' + val[v] + '</option>';
            }
        }
    }  
    return dropdown += '</select>';
}

function Radio(type, classVal, id, name, tabIndex, value, controlVal, checked) {
    var val = controlVal.split(',');
    var radio = '';
    for (var v = 0 ; v < val.length ; v++) {
        //radio += '<input type="' + type + '" name="' + name + '" id="' + id + "_" + v + '" value="' + value + '" checked="' + checked + '" class="' + classVal + '" tabindex="' + tabIndex + '" >';
        //radio += '<input type="' + type + '" name="' + name + "_" + value + '"  value="' + val[v] + '"  class="' + classVal + '" tabindex="' + tabIndex + '" checked="' + "" + '" >';
        radio += '<input type="' + type + '" name="' + name + "_" + value + '"  value="' + val[v] + '"  class="' + classVal + '" tabindex="' + tabIndex + '" id="' + id + "_" + v + '">';
        radio += ' <label for="' + "" + '" id="' + id + "_" + v + '">' + val[v] + '</label>';
    }
    return radio;
}

function CheckBox() {
}

function DateGet() {
}

function TimeGet() {
}

function ComboGlobalDictionary() {
}

function CrfTerm() {
}

function Table(name, id, classVal, data) {
    var table = '';
    var key = [];
    var value = [];
    var final = []
    
    for (var v = 0 ; v < Object.keys(data[0]).length ; v++) {
        key.push(Object.keys(data[0])[v]);        
    }

    for (var v = 0 ; v < data.length ; v++) {
        for (var k = 0 ; k < Object.values(data[v]).length ; k++) {
            value.push(Object.values(data[v])[k]);
        }
        final.push(value);
    }

    table += '<table name="'+name+'" id="'+id+'" class="'+classVal+'">';
    table += '<thead>';
    table += '<tr>';
    for (var v = 0 ; v < key.length ; v++) {
        table += '<th>' + key[v] + '</th>';
    }
    table += '</tr>';
    table += '</thead>';
    table += '<tbody>';
    for (var v = 0 ; v < final.length ; v++) {
        table += '<tr>';
        for (var k = 0; k < final[v].length; k++) {
            table += '<td>' + final[v][k] + '</td>';
        }
        table += '</tr>';
    }
    table += '</tbody>';
    table += '</table>';
    return table;
}

function TableToDataTable(id) {
    
    $(id).dataTable({
        "bJQueryUI": true,
        "sPaginationType": "full_numbers",
        "bLengthChange": true,
        "iDisplayLength": 10,
        "bProcessing": true,
        "bSort": true,
        "autoWidth": false,
        "bInfo": true,
        "bDestroy": true,
        "sScrollX": "100%",
        "oLanguage": {
            "sEmptyTable": "No Record Found",
        },
    });
}

function createDiv() {
    
    if (typeof shadow == "undefined") {
        var divTag = document.createElement('div');
        divTag.id = 'shadow';
        divTag.setAttribute('align', 'center');
        //divTag.style.position = 'absolute';
        divTag.style.position = 'fixed';
        divTag.style.top = '0px';
        divTag.style.width = '100%';
        divTag.style.height = '100%';
        divTag.style.left = '0px';
        divTag.style.opacity = '0.6';
        divTag.style.zIndex = '999999';
        divTag.style.filter = 'alpha(opacity=30)';
        divTag.style.backgroundColor = '#000000';
        document.body.appendChild(divTag);       
    }
    else {
        shadow.style.display = "";
    }
}

function removeDiv() {    
    if (typeof shadow != "undefined") {
        $("#shadow").remove();
    }
}


