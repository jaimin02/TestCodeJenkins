<%@ page title="" language="VB" masterpagefile="~/MasterPage.master" autoeventwireup="false" inherits="frmUploadCertificateWeb, App_Web_ybumpksz" enableEventValidation="false" theme="StyleBlue" viewStateEncryptionMode="Always" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPHLAMBDA" Runat="Server">
    <script src="Script/General.js" language="javascript" type="text/javascript"></script>
    <script type="text/javascript" src="Script/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="Script/Gridview.js"></script>
    <script type="text/javascript" src="Script/scrollablegrid.js"></script>
    <style>
        .disabled{
    pointer-events:none;
    opacity:0.4;
}
        .trBorder {
            border: 1px solid black;
        }

        .tab-content {
            display: none;
            color: white;
            display: none;
            padding: 20px 20px;
            height: 100%;
        }

            .tab-content:target {
                display: block;
            }

        ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            font-family: "Helvetica Neue", Helvetica, Roboto, Arial, sans-serif;
        }

        li {
            display: inline;
        }

            li a {
                display: block;
                color: white;
                text-align: center;
                padding: 16px;
                text-decoration: none;
                padding: 4px 10px;
                margin-left: -1px;
                position: relative;
                left: 1px;
            }

        /* Style tab links */
        .tablink {
            display: block;
            color: white;
            float: left;
            border: none;
            outline: none;
            cursor: pointer;
            padding: 14px 16px;
            text-decoration: none;
            padding: 4px 10px;
            margin-left: -1px;
            position: relative;
            left: 1px;
        }

            .tablink:hover {
                background-color: #777;
            }

        /* Style the tab content (and add height:100% for full page content) */
        .tabcontent {
            color: white;
            display: none;
            padding: 100px 20px;
            height: 100%;
        }

        .wrapper{
            position:relative;
        }
        .tooltip {
            transform: none;
            margin: 50px;    
        }
        
        .tooltip:hover > .tooltip-text, .tooltip:hover > .wrapper {
            position:relative;
            pointer-events: auto;
            opacity: 1.0;
        }
        
        .tooltip > .tooltip-text, .tooltip >.wrapper {
            display: block;
            position: absolute;
            z-index: 6000;
            overflow: visible;
            padding: 5px 8px;
            margin-top: 10px;
            line-height: 16px;
            border-radius: 4px;
            text-align: left;
            color: #fff;
            background: #000;
            pointer-events: none;
            opacity: 0.0;
            -o-transition: all 0.3s ease-out;
            -ms-transition: all 0.3s ease-out;
            -moz-transition: all 0.3s ease-out;
            -webkit-transition: all 0.3s ease-out;
            transition: all 0.3s ease-out;
        }
        
        /* Arrow */
        .tooltip > .tooltip-text:before, .tooltip > .wrapper:before  {
            display: inline;
            top: -5px;
            content: "";
            position: absolute;
            border: solid;
            border-color: rgba(0, 0, 0, 1) transparent;
            border-width: 0 .5em .5em .5em;
            z-index: 6000;
            left: 20px;
        }
        
        /* Invisible area so you can hover over tooltip */
        .tooltip > .tooltip-text:after, .tooltip > .wrapper:after  {
            top: -20px;
            content: " ";
            display: block;
            height: 20px;
            position: absolute;
            width: 60px;
            left: 20px;
        }
        
        .wrapper > .tooltip-text {
            overflow-y: auto;
            max-height: 100px;
            display: block;
        }

    </style>
        <script type="text/javascript" src="Script/ReactJS/react.development.js"></script>
        <script type="text/javascript" src="Script/ReactJS/react-dom.development.js"></script>
        <script src="Script/ReactJS/react-autocomplete.min.js"></script>
        <script src="Script/ReactJS/axios.min.js"></script>
        <script src="Script/ReactJS/babel.min.js"></script>
    <!--Load our React Component-->
    <script type="text/babel" src="Script/UploadCertificate.js"></script>
    <contenttemplate>
        <div id="ImageTransmittal_container"></div>
    </contenttemplate>
    <script type="text/javascript" language="javascript">
        var SupportedFileExt = "<%=ConfigurationManager.AppSettings.Item("SupportedFileExt").Trim()%>";
        var DISoftAPIURL = "<%=ConfigurationManager.AppSettings.Item("DISoftAPIURL").Trim()%>";
        var chunkSize = parseInt("<%=ConfigurationManager.AppSettings.Item("chunkSize").Trim()%>");
        var DicomFilePath = "<%=ConfigurationManager.AppSettings.Item("DicomFilePath").Trim()%>";

        function HandleAuditTrail(iCertificateMasterID) {
            debugger;
            document.getElementById('AuditTrailDiv').style.display = "block";
            $.ajax({
                type: "post",
                url: "frmUploadCertificateWeb.aspx/AuditTrail",
                data: '{"iCertificateMasterId":"' + iCertificateMasterID + '"}',
                contentType: "application/json; charset=utf-8",
                datatype: JSON,
                success: function (response) {
                    this.responseData = response.data;
                    var tempTable = "";

                    tempTable = "<tr class='trBorder'><th class ='trBorder'>Sr. No</th>" +
                                "<th class ='trBorder'>Modality Desc.</th>" +
                                "<th class ='trBorder'>Anatomy Desc.</th>" +
                                "<th class ='trBorder'>IVContrast Desc.</th>" +
                                "<th class ='trBorder'>Examination Date</th>" +
                                "<th class ='trBorder'>Total Image</th>" +
                                "<th class ='trBorder'>Change On(Uploader Date)</th>";

                    for (var i = 0; i < eval(response.d).length; i++) {
                        tempTable += "<tr class ='trBorder'><td class ='trBorder'>" + parseInt(i + 1) + "</td>" +
                            " <td class ='trBorder'>" + eval(response.d)[i].vModalityDesc + "</td>" +
                            " <td class ='trBorder'>" + eval(response.d)[i].vAnatomyDesc + "</td>" +
                            " <td class ='trBorder'>" + eval(response.d)[i].cIVContrast + "</td>" +
                            " <td class ='trBorder'>" + eval(response.d)[i].dExaminationDate + "</td>" +
                            " <td class ='trBorder'>" + eval(response.d)[i].iNoImages + "</td> " +
                            " <td class ='trBorder'>" + eval(response.d)[i].ChangeOn + "</td></tr>"
                    }

                    document.getElementById('AuditTrailTable').innerHTML = tempTable;
                },
                failure: function (error) {
                    alert(error._message);
                    return false;
                }
            });
        }

        function SelectAllAnatomy(length, data, e, flag, rowNum) {
            debugger;
            count = 0;
            if (flag == 1) {
                if (document.getElementById("SelectAll_" + rowNum).checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nAnatomyNo + "_" + rowNum).prop("checked", true);
                    }
                }
                else if (!document.getElementById("SelectAll_" + rowNum).checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nAnatomyNo + "_" + rowNum).prop("checked", false);
                    }
                }
            }
            else {

                if (document.getElementById(e.id).checked) {
                    $("#" + e.id).prop("checked", true);
                }
                else {
                    $("#" + e.id).prop("checked", false);
                    if (document.getElementById("SelectAll_" + rowNum).checked) {
                        $("#SelectAll_" + rowNum).prop("checked", false);
                    }
                }
            }

            for (var i = 0 ; i < length ; i++) {
                if (document.getElementById(eval(data)[i].nAnatomyNo + "_" + rowNum).checked) {
                    count++;
                }
                if ((data[i].nAnatomyNo == 20)) {
                    if (document.getElementById(e.id).checked) {
                        if(e.id == '20_0'){
                    $("#OtherAnatomy").removeAttr("disabled");
                    $("#OtherAnatomy").focus();
                        }
                    }
                } else {
                    $("#OtherAnatomy").attr("disabled", "disabled");
                }

            }
            if (count == length) {
                $("#SelectAll_" + rowNum).prop("checked", true);
            }
            
            
        }

        function SelectOther(e, rowCount, ddlId) {
            debugger;
            if (rowCount > 0) {
                if (document.getElementById("ddlAnatomy" + rowCount).id == ddlId.id) {
                    if (e.id == '18_' + rowCount) {
                        if (document.getElementById(e.id).checked) {
                            var textbox = $(document.createElement('div')).attr("id", 'divTxt' + rowCount);
                            textbox.html('Other Anatomy <input id="txt' + rowCount + '" name = "OtherAnatomy" type="text" />');
                            if (rowCount == 0) {
                                textbox.appendTo("#OtherAnatomy");
                            }
                            else {
                                textbox.appendTo("#OtherAnatomy" + rowCount);
                            }
                        }
                        else {
                            if (rowCount == 0) {
                                document.getElementById("OtherAnatomy").innerHTML = "";
                            }
                            else {
                                document.getElementById("OtherAnatomy" + rowCount).innerHTML = "";
                            }
                        }
                    }
                    else if (e.id == "SelectAll_"+ rowCount) {
                        var textbox = $(document.createElement('div')).attr("id", 'AnatomydivTxt' + rowCount);
                        textbox.html('Other Anatomy <input id="OtherAnatomy' + rowCount + '" name = "OtherAnatomy" type="text" />');
                        if (document.getElementById(e.id).checked) {
                            var textbox = $(document.createElement('div')).attr("id", 'divTxt' + rowCount);
                            textbox.html('Other Anatomy <input id="txt' + rowCount + '" name = "OtherAnatomy" type="text" />');
                            if (rowCount == 0) {
                                textbox.appendTo("#OtherAnatomy");
                            }
                            else {
                                textbox.appendTo("#OtherAnatomy" + rowCount);
                            }
                        }
                        else {
                            if (rowCount == 0) {
                                document.getElementById("OtherAnatomy").innerHTML = "";
                            }
                            else {
                                document.getElementById("OtherAnatomy" + rowCount).innerHTML = "";
                            }
                        }
                    }
                }
            }
            else {
               
                if (e.id == '18_0') {
                    if (document.getElementById(e.id).checked) {
                        var textbox = $(document.createElement('div')).attr("id", 'AnatomydivTxt' + rowCount);
                        textbox.html('Other Anatomy <input id="OtherAnatomy' + rowCount + '" name = "OtherAnatomy" type="text" />');
                        if (rowCount == 0) {
                            textbox.appendTo("#OtherAnatomy");
                        }
                    }
                    else {
                        if (rowCount == 0) {
                            document.getElementById("OtherAnatomy").innerHTML = "";
                        }
                    }
                }
                else if (e.id == "SelectAll_0") {
                    var textbox = $(document.createElement('div')).attr("id", 'AnatomydivTxt' + rowCount);
                    textbox.html('Other Anatomy <input id="OtherAnatomy' + rowCount + '" name = "OtherAnatomy" type="text" />');
                    if (document.getElementById(e.id).checked) {
                        var textbox = $(document.createElement('div')).attr("id", 'AnatomydivTxt' + rowCount);
                        textbox.html('Other Anatomy <input id="OtherAnatomy' + rowCount + '" name = "OtherAnatomy" type="text" />');
                        if (rowCount == 0) {
                            textbox.appendTo("#OtherAnatomy");
                        }
                    }
                    else {
                        if (rowCount == 0) {
                            document.getElementById("OtherAnatomy").innerHTML = "";
                        }
                    }
                }
            }
        }

        function SelectAllModality(length, data, id, flag) {
            
            count = 0;
            if (flag == 1) {
                if (document.getElementById("SelectAllModalities").checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nModalityNo).prop("checked", true);
                    }
                }
                else if (!document.getElementById("SelectAllModalities").checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nModalityNo).prop("checked", false);
                    }
                }
            }
            else {

                if (document.getElementById(id).checked) {
                    $("#" + id).prop("checked", true);
                }
                else {
                    $("#" + id).prop("checked", false);
                    if (document.getElementById("SelectAllModalities").checked) {
                        $("#SelectAllModalities").prop("checked", false);
                    }
                }
            }

            for (var i = 0 ; i < length ; i++) {
                if (document.getElementById(eval(data)[i].nModalityNo).checked) {
                    count++;
                }
            }
            if (count == length) {
                $("#SelectAllModalities").prop("checked", true);
            }
        }

        function SelectAllAnatomy(length, data, e, flag, rowNum) {
            debugger;
            count = 0;
            if (flag == 1) {
                if (document.getElementById("SelectAll_" + rowNum).checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nAnatomyNo + "_" + rowNum).prop("checked", true);
                    }
                }
                else if (!document.getElementById("SelectAll_" + rowNum).checked) {
                    for (var i = 0 ; i < length ; i++) {
                        $("#" + eval(data)[i].nAnatomyNo + "_" + rowNum).prop("checked", false);
                    }
                }
            }
            else {

                if (document.getElementById(e.id).checked) {
                    $("#" + e.id).prop("checked", true);
                }
                else {
                    $("#" + e.id).prop("checked", false);
                    if (document.getElementById("SelectAll_" + rowNum).checked) {
                        $("#SelectAll_" + rowNum).prop("checked", false);
                        $("#OtherAnatomy" + rowNum).val('');
                    }
                    if (e.name.toUpperCase().match("OTHER")) {
                        if (rowNum > 1) {
                            $("#OtherAnatomy" + rowNum).val('');
                        } else {
                            $("#OtherAnatomy").val('');
                        }
                    }
                }
            }

            for (var i = 0 ; i < length ; i++) {
                if (document.getElementById(eval(data)[i].nAnatomyNo + "_" + rowNum).checked) {
                    count++;
                }
                if (data[i].vAnatomyDesc.toUpperCase().match("OTHER")) { //text.match("OTHER");
                    if (document.getElementById(e.id).checked) {
                        if (e.name.toUpperCase().match("OTHER")) {
                            if (rowNum > 1) {
                                $("#OtherAnatomy" + rowNum).removeAttr("disabled");
                                $("#OtherAnatomy" + rowNum).focus();
                            } else {
                                $("#OtherAnatomy").removeAttr("disabled");
                                $("#OtherAnatomy").focus();
                            }

                        }
                    }
                } else {
                    if (rowNum > 1) {
                        $("#OtherAnatomy" + rowNum).attr("disabled", "disabled");
                    }
                    else {
                        $("#OtherAnatomy").attr("disabled", "disabled");
                    }
                }

            }
            if (count == length) {
                $("#SelectAll_" + rowNum).prop("checked", true);
            }


        }

    </script>
</asp:Content>
