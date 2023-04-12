<%@ Page Language="VB" AutoEventWireup="false" CodeFile="frmCTMMedExInfoHdrDtl_New.aspx.vb"
    Inherits="frmCTMMedExInfoHdrDtl_New" Title=":: Subject Attributes Detail :: Lambda Therapeutic Research ."
    ValidateRequest="false" EnableEventValidation="false" %>

<%@ Register Assembly="System.Web.Extensions" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="App_Themes/StyleBlue/StyleBlue.css" rel="stylesheet" type="text/css" />
    <link href="App_Themes/sweetalert.css" rel="stylesheet" type="text/css"/>

    <script src="Script/Validation.js" language="javascript" type="text/javascript"></script>

    <script src="Script/General.js" language="javascript" type="text/javascript"></script>

    <script src="Script/popcalendar.js" language="javascript" type="text/javascript"></script>

    <script type="text/javascript" src="Script/jquery.min.js"></script>

    <script src="Script/DD_roundies_0.0.2a.js" type="text/javascript"></script>
    <script type="text/javascript" src="Script/sweetalert.js"></script>

    <script language="javascript" type="text/javascript">

        var ControlId;
        var PreviousValue;
        var cMONTHNAMES = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug',
                   'Sep', 'Oct', 'Nov', 'Dec'];

        DD_roundies.addRule('.updateProgress', '15px');

        $(document).ready(function()
        {
            $('#canal').css('display', 'none');
        })

        $(document).ready(function()
        {
            $('#divActivityLegends').css('display', 'none');
        })

        var currTab;

        function closewindow(e)
        {
            msgConfirmDeleteAlert(null, "Are you sure want to Exit ?", function (isConfirmed) {
                if (isConfirmed) {
                    var parWin = window.opener;
                    if (parWin != null && typeof (parWin) != 'undefined') {
                        parWin.RefreshPage();
                        self.close();
                    }
                    __doPostBack(e.name, '');
                    return true;
                } else {

                    return false;
                }
            });
            return false;


        }

        //        function DivHideTemp()
        //        {
        //            document.getElementById('div1').style.display = "none";
        //        }
        //        function ShowHideDiv(Divid, ImageId)
        //        {
        //            var e = document.getElementById(Divid);
        //            var f = document.getElementById(ImageId);
        //            if (e.style.display == 'none')
        //            {
        //                e.style.display = 'block';
        //                f.src = "images/collapse.jpg";
        //            }
        //            else
        //            {
        //                e.style.display = 'none';
        //                f.src = "images/expand.jpg";
        //            }
        ////            if (document.getElementById('HFActivateTab').value != "")
        ////            {
        ////                e.style.display = 'block';
        ////                f.src = "images/collapse.jpg";
        ////            }
        //        }

        function ValidateTextbox(checktype, txt, msg, HighRange, LowRange, AlertOn, AlertMsg, length, IsNotNull, BtnUpdateId)
        {
            var ShowUpdate = true;
            txt.style.borderColor = '';
            if (IsNotNull == 'Y' && txt.value.trim() == '')
            {
                msgalert('Field can not be left blank !');
                txt.style.borderColor = "Red";
                if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                {
                    return;
                }
                ShowUpdate = false;
                //CheckToUpdateValue(BtnUpdateId);
            }

            var result;

            if (checktype != 0)
            {
                switch (checktype)
                {
                    case 1:
                        result = CheckInteger(txt.value);
                        break;
                    case 2:
                        result = CheckDecimal(txt.value);
                        break;
                    case 3:
                        result = CheckIntegerOrBlank(txt.value);
                        break;
                    case 4:
                        result = CheckDecimalOrBlank(txt.value);
                        break;
                    case 5:
                        result = CheckAlphabet(txt.value);
                        break;
                    case 6:
                        result = CheckAlphaNumeric(txt.value);
                        break;
                    default: break;          //alert("oh u have all rights ");
                }

                if (result == false)
                {
                    txt.value = '';
                    txt.focus();
                    msgalert(msg);
                    txt.style.borderColor = "Red";
                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    ShowUpdate = false;
                    //CheckToUpdateValue(BtnUpdateId);
                }

            }

            if (HighRange != 0)
            {
                if (txt.value > HighRange)
                {
                    msgalert('Out Of Range! Value Must be Less Then ' + HighRange);
                    txt.style.borderColor = "Red";
                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    ShowUpdate = false;
                    //CheckToUpdateValue(BtnUpdateId);
                }
            }
            if (LowRange != 0)
            {
                if (txt.value < LowRange)
                {
                    msgalert('Out Of Range! Value Must be Greater Then ' + LowRange);
                    txt.style.borderColor = "Red";
                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    ShowUpdate = false;
                    //CheckToUpdateValue(BtnUpdateId);
                }
            }
            if (txt.value.trim() != '')
            {
                if (txt.value.toUpperCase() == AlertOn.toUpperCase())
                {
                    msgalert(AlertMsg);
                    //txt.style.borderColor = "Red";
                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    ShowUpdate = false;
                    //CheckToUpdateValue(BtnUpdateId);
                }
            }
            if (length != 0)
            {
                if (txt.value.length > length)
                {
                    msgalert('Length Exceeded! Lengh Should be Less Then Or Equal To ' + length);
                    txt.style.borderColor = "Red";
                    if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                }
            }
            if (txt.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return;
            }
            if (ShowUpdate == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
            else
            {
                txt.focus()
            }
            return;
        }
        function C2F(txtCelsiusID, txtFahrenheitID)
        {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result = true;

            if (!(CheckDecimalOrBlank(txtCelsius.value)))
            {
                msgalert('Please Enter Valid Temperature in Celsius !');
                txtCelsius.focus();
                if (txtCelsius.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                {
                    return false;
                }
                result = false;
                //CheckToUpdateValue(BtnUpdateId);
                return false;
            }
            txtFahrenheit.value = c2f(txtCelsius.value);
            if (txtCelsius.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return true;
            }
            if (result == false)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
            return true;
        }

        function F2C(txtFahrenheitID, txtCelsiusID, BtnUpdateId)
        {
            var txtCelsius = document.getElementById(txtCelsiusID);
            var txtFahrenheit = document.getElementById(txtFahrenheitID);
            var result = true;

            if (!(CheckDecimalOrBlank(txtFahrenheit.value)))
            {
                msgalert('Please Enter Valid Temperature in Fahrenheit !');
                txtFahrenheit.focus();
                if (txtFahrenheit.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                {
                    return false;
                }
                //CheckToUpdateValue(BtnUpdateId);
                result = false;
                return false;
            }
            txtCelsius.value = f2c(txtFahrenheit.value);
            if (txtFahrenheit.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return true;
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
            return true;
        }
        function ddlAlerton(objId, alerton, alertmsg, BtnUpdateId)
        {
            document.getElementById(objId).style.color = "Navy";
            if (alerton != '')
            {
                if (document.getElementById(objId).value.toUpperCase() == alerton.toUpperCase())
                {
                    //document.getElementById(objId).style.color = "Red";
                    msgalert(alertmsg);
                }
            }
            if (document.getElementById(objId).value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return;
            }
            CheckToUpdateValue(BtnUpdateId);
        }
        function checkddlNotNull(objId, IsNotNull, BtnUpdateId)
        {
            var result = true;
            document.getElementById(objId).style.backgroundColor = '';
            if (IsNotNull == 'Y' && document.getElementById(objId).value.trim() == '')
            {
                document.getElementById(objId).style.backgroundColor = "Red";
                msgalert('Field can not be left blank !');
                result = false;
            }
            if (document.getElementById(objId).value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return;
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function checkRBLNotNull(tblName, IsNotNull, BtnUpdateId)
        {
            var result = true;
            if (IsNotNull == 'Y')
            {
                var tblRdo = $get(tblName);
                var name = tblRdo.id;
                name = name.replace(/_/g, '$');
                var rdos = document.getElementsByName(name);
                var i;
                tblRdo.style.color = "Navy";
                for (i = 0; i < rdos.length; i++)
                {
                    if (rdos[i].checked)
                    {
                        break;
                    }
                }
                result = false;
                tblRdo.style.color = "Red";
                msgalert('Field can not be left blank !');
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function Alerton(tblName, alertOn, alertMsg, BtnUpdateId, Value)
        {
            ControlId = tblName;
            PreviousValue = Value;
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;
            for (i = 0; i < rdos.length; i++)
            {
                tblRdo.style.color = "Navy";
                if (rdos[i].checked && rdos[i].value.toUpperCase() == alertOn.toUpperCase())
                {
                    //tblRdo.style.color = "Red";
                    msgalert(alertMsg);
                    break;
                }
                if (rdos[i].checked && rdos[i].value.toUpperCase() == Value && BtnUpdateId != null)
                {
                    return;
                }
            }
            CheckToUpdateValue(BtnUpdateId);
        }

        function RemoveSelection(tblName, BtnUpdateId)
        {
            var tblRdo = $get(tblName);
            var name = tblRdo.id;
            name = name.replace(/_/g, '$');
            var rdos = document.getElementsByName(name);
            var i;
            for (i = 0; i < rdos.length; i++)
            {
                rdos[i].checked = false;
            }
            document.getElementById('HFRadioButtonValue').value = 'NULL';
            CheckToUpdateValue(BtnUpdateId);
        }

        function checkCBLNotNull(chklst, IsNotNull, BtnUpdateId)
        {
            var result = true;
            if (IsNotNull == 'Y')
            {
                chklst.style.color = "Navy";
                var chks;
                var result = false;
                var i;
                if (chklst != null && typeof (chklst) != 'undefined')
                {
                    chks = chklst.getElementsByTagName('input');
                    for (i = 0; i < chks.length; i++)
                    {
                        var val = chks[i].nextSibling.innerText;
                        if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked)
                        {
                            break;
                        }
                    }
                    chklst.style.color = "Red";
                    msgalert('Field can not be left blank !');
                    result = false;
                }
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
        }
        function AlertonCheckBox(chklst, alertOn, alertMsg, BtnUpdateId, Value)
        {
            ControlId = chklst.id;
            PreviousValue = Value;
            chklst.style.color = "Navy";
            var chks;
            var result = false;
            var i;
            if (chklst != null && typeof (chklst) != 'undefined')
            {
                chks = chklst.getElementsByTagName('input');
                for (i = 0; i < chks.length; i++)
                {
                    var val = chks[i].nextSibling.innerText;
                    if (chks[i].type.toUpperCase() == 'CHECKBOX' && chks[i].checked && val == alertOn)
                    {
                        msgalert(alertMsg);
                        //chklst.style.color = "Red";
                        break;

                    }
                }
            }
            CheckToUpdateValue(BtnUpdateId);
        }

        function DateValidation(ParamDate, txtdate, IsNotNull, BtnUpdateId)
        {
            var result = true;
            txtdate.style.borderColor = "";
            if (IsNotNull == 'Y')
            {
                if (ParamDate.trim() == '')
                {
                    result = false;
                    msgalert('Field can not be left blank !');
                    txtdate.style.borderColor = "Red";
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    return;
                }
            }
            if (ParamDate.trim() != '')
            {
                var flg = false;
                flg = DateConvert(ParamDate, txtdate);
                if (flg == true && !CheckDateLessThenToday(txtdate.value))
                {
                    txtdate.value = "";
                    txtdate.focus();
                    msgalert('Date should be less than Current Date !');
                    result = false;
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return false;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    return false;
                }
            }
            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return true;
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }
            return true;
        }

        function DateValidationForCTM(ParamDate, txtdate, IsNotNull, BtnUpdateId)
        {
            var result = true;
            txtdate.style.borderColor = "";
            if (IsNotNull == 'Y')
            {
                if (ParamDate.trim() == '')
                {
                    msgalert('Field can not be left blank !');
                    result = false;
                    txtdate.style.borderColor = "Red";
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    return;
                }
            }
            if (ParamDate.trim() != '')
            {
                //Format Change Start
                var dt = ParamDate.trim().toUpperCase();
                var tempdt;
                if (dt.indexOf('UK') >= 0 || dt.indexOf('UNK') >= 0 || dt.indexOf('UKUK') >= 0)
                {

                    if (dt.length < 8)
                    {
                        msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only !');
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                        {
                            return false;
                        }
                        result = false;
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    var day;
                    var month;
                    var year;
                    if (dt.indexOf('-') >= 0)
                    {
                        var arrDate = dt.split('-');
                        day = arrDate[0];
                        month = arrDate[1];
                        year = arrDate[2];
                    }
                    else
                    {
                        day = dt.substr(0, 2);
                        month = dt.substr(2, 2);
                        year = dt.substr(4, 4);
                        if (dt.indexOf('UNK') >= 0)
                        {
                            month = dt.substr(2, 3);
                            year = dt.substr(5, 4);
                        }
                    }

                    if (day.length > 2 && day.length != 0)
                    {
                        msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only !');
                        result = false;
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                        {
                            return false;
                        }
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    if (month.length > 3 && month.length != 3)
                    {
                        msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only !');
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                        {
                            return false;
                        }
                        result = false;
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    if (year.length > 4 && year.length != 4)
                    {
                        msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only !');
                        txtdate.value = "";
                        txtdate.focus();
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                        {
                            return false;
                        }
                        result = false;
                        //CheckToUpdateValue(BtnUpdateId);
                        return false;
                    }
                    if (day == 'UK')
                    {
                        tempdt = '01';
                    }
                    else
                    {
                        tempdt = day;
                    }
                    if (dt.indexOf('-') >= 0)
                    {
                        tempdt += '-';
                    }
                    if (month == 'UNK')
                    {
                        tempdt += '01';
                    }
                    else
                    {
                        tempdt += month;
                    }
                    if (dt.indexOf('-') >= 0)
                    {
                        tempdt += '-';
                    }
                    if (year == 'UKUK')
                    {
                        tempdt += '1800';
                    }
                    else
                    {
                        tempdt += year;
                    }
                    var chk = false;
                    chk = DateConvert(tempdt, txtdate);
                    if (chk == true)
                    {
                        if (isNaN(month))
                        {
                            txtdate.value = day + '-' + month + '-' + year;
                        }
                        else
                        {
                            txtdate.value = day + '-' + cMONTHNAMES[month - 1] + '-' + year;
                        }
                        if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                        {
                            return true;
                        }
                        // CheckToUpdateValue(BtnUpdateId);
                        return true;
                    }
                    //alert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only.');
                    txtdate.value = "";
                    txtdate.focus();
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return false;
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    return false;
                }
                //End Format change
                var flg = false;
                flg = DateConvert(ParamDate, txtdate);
                if (flg == true && !CheckDateLessThenToday(txtdate.value))
                {
                    msgalert('Date should be less than Current Date !');
                    txtdate.value = "";
                    txtdate.focus();
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return false;
                    }
                    result = false;
                    //CheckToUpdateValue(BtnUpdateId);
                    return false;
                }
                else if (flg == false)
                {
                    msgalert('Please Enter Date in DDMMYYYY or dd-Mon-YYYY format only !');
                    txtdate.value = "";
                    txtdate.focus();
                    if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return false;
                    }
                    result = false;
                    //CheckToUpdateValue(BtnUpdateId);
                    return false;
                }
                else if (flg == true)
                {
                    dt = txtdate.value.toUpperCase();
                    var Year = dt.substring(dt.lastIndexOf('-') + 1);
                    if (Year.length == 2)
                    {
                        if (parseInt(Year) <= cCUTOFFYEAR)
                        {
                            Year = "20" + Year.toString();
                        }
                        else
                        {
                            Year = "19" + Year.toString();
                        }
                    }
                    var Day = dt.substring(0, dt.indexOf('-'));
                    var Month = dt.substring(dt.indexOf('-') + 1, dt.lastIndexOf('-'));
                    Month = ConvertMonthToInt(Month);
                    Month = parseFloat(Month);
                    Month = Month - 1;
                    var startDate = new Date();
                    startDate.setFullYear(Year, Month, Day);
                    txtdate.value = startDate.format('dd-MMM-yyyy');
                }
            }
            if (txtdate.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return true;
            }
            if (result == true)
            {
                CheckToUpdateValue(BtnUpdateId);
            }

            //            CheckToUpdateValue(BtnUpdateId);
            return true;
        }

        function AutoTimeConvert(ParamTime, txtTime, IsNotNull, BtnUpdateId)
        {
            var result = true;
            txtTime.style.borderColor = "";
            if (IsNotNull == 'Y')
            {
                if (ParamTime.trim() == '')
                {
                    msgalert('Field can not be left blank !');
                    txtTime.style.borderColor = "Red";
                    if (txtTime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
                    {
                        return;
                    }
                    result = false;
                    if (result == true)
                    {
                        CheckToUpdateValue(BtnUpdateId);
                    }
                    //CheckToUpdateValue(BtnUpdateId);
                    return;
                }
            }
            TimeConvert(ParamTime, txtTime);
            if (txtTime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return;
            }
            CheckToUpdateValue(BtnUpdateId);
        }

        function Next(NoneDivId)
        {
            var arrDiv = NoneDivId.split(',');
            var isShow = false;
            for (i = 0; i < arrDiv.length; i++)
            {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                document.getElementById(disBtn).style.color = '#FFffff';
            }
            for (i = 0; i < arrDiv.length; i++)
            {
                if (isShow)
                {
                    currTab = arrDiv[i];
                    isShow = false;
                    break;
                }
                if (arrDiv[i].toLowerCase() == currTab.toLowerCase())
                {
                    isShow = true;
                }
            }
            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function Previous(NoneDivId)
        {
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++)
            {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                document.getElementById(disBtn).style.color = '#FFffff';
            }
            for (i = 0; i < arrDiv.length; i++)
            {
                if (arrDiv[i].toLowerCase() == currTab.toLowerCase())
                {
                    if (i > 0)
                    {
                        currTab = arrDiv[i - 1];
                        break;
                    }
                }
            }
            var currBtn = currTab.replace('Div', 'BtnDiv');
            document.getElementById(currTab).style.display = 'block';
            document.getElementById(currBtn).style.color = '#FFC300';
            return false;
        }

        function DisplayDiv(BlockDivId, NoneDivId)
        {
            var selBtn = BlockDivId.replace('Div', 'BtnDiv');
            var arrDiv = NoneDivId.split(',');
            for (i = 0; i < arrDiv.length; i++)
            {
                document.getElementById(arrDiv[i]).style.display = 'none';
                var disBtn = arrDiv[i].replace('Div', 'BtnDiv');
                document.getElementById(disBtn).style.color = '#FFffff';
                if (selBtn.toLowerCase() == disBtn.toLowerCase())
                {
                    currTab = arrDiv[i];
                }
            }
            document.getElementById(BlockDivId).style.display = 'block';
            document.getElementById('HFActivateTab').value = BlockDivId;
            document.getElementById(selBtn).style.color = '#FFC300';

            return false;
        }

        function HistoryDivShowHide(Type, MedexCode, BlockDivId, NoneDivId, CRFDtlNo)
        {

            document.getElementById('hfMedexCode').value = MedexCode;
            document.getElementById('HFCRFDtlNo').value = CRFDtlNo;

            var btnAudit = document.getElementById('btnAudittrail');

            if (Type == 'S')
            {
                return false;
            }
            else if (Type == 'H')
            {
                document.getElementById('divHistoryDtl').style.display = 'none';
                return false;
            }
            else if (Type == 'A')
            {
                btnAudit.click();
                return false;
            }
            else if (Type == 'SN')
            {
                document.getElementById('divHistoryDtl').style.display = 'block';
                SetCenter('divHistoryDtl');
                return DisplayDiv(BlockDivId, NoneDivId);
            }
            return true;

        }

        function AuditDivShowHide(Type, MedexCode, buttonId, CRFDtlNo)
        {
            var btnE = document.getElementById('btnEdit');
            var btnD = document.getElementById('btnDCF');
            document.getElementById('hfMedexCode').value = MedexCode;
            document.getElementById('HFCRFDtlNo').value = CRFDtlNo;

            if (Type == 'E')
            {
                document.getElementById(MedexCode).disabled = false;
                document.getElementById(MedexCode).removeAttribute('readOnly'); //Enhancement in EDC

                var chklst = document.getElementById(MedexCode);
                var chks;
                var i;
                if (chklst != null && typeof (chklst) != 'undefined')
                {
                    chks = chklst.getElementsByTagName('input');
                    for (i = 0; i < chks.length; i++)
                    {
                        if (chks[i].type.toUpperCase() == 'CHECKBOX')
                        {
                            chks[i].disabled = false;
                        }
                    }
                }

                document.getElementById('btnUpdate' + buttonId).disabled = false;
                if (document.getElementById('btnBrowse' + buttonId) != null)
                {
                    document.getElementById('btnBrowse' + buttonId).disabled = false;
                }
                //document.getElementById('btnEdit' + buttonId).disabled = true;
                document.getElementById(MedexCode).focus();
                return false;
            }
            if (Type == 'U')
            {
                document.getElementById('btnUpdate' + buttonId).disabled = true;
                document.getElementById('btnEdit' + buttonId).disabled = false;
                btnE.click();
                return false;
            }
            if (Type == 'D')
            {
                btnD.click();
                return false;
            }
            if (Type == 'S')
            {
                var btnS = document.getElementById('btnSaveRunTime');
                btnS.click();
                return false;
            }
            return true;
        }
        function AnyDivShowHide(Type)
        {
            if (Type == 'S')
            {
                document.getElementById('divForEditAttribute').style.display = 'block';
                SetCenter('divForEditAttribute');
                return false;
            }
            else if (Type == 'H')
            {
                document.getElementById('divForEditAttribute').style.display = 'none';
                return false;
            }
            else if (Type == 'DCFSHOW')
            {
                document.getElementById('divDCF').style.display = 'block';
                SetCenter('divDCF');
                return false;
            }
            else if (Type == 'DCFHIDE')
            {
                document.getElementById('divDCF').style.display = 'none';
                return false;
            }
            else if (Type == 'DIVQUERIESSHOW')
            {
                document.getElementById('divQueries').style.display = 'block';
                SetCenter('divQueries');
                return false;
            }
            else if (Type == 'DIVQUERIESHIDE')
            {
                document.getElementById('divQueries').style.display = 'none';
                return false;
            }
            return true;
        }
        function DivAuthenticationHideShow(Type)
        {
            if (Type == 'S')
            {
                document.getElementById('divAuthentication').style.display = 'block';
                SetCenter('divAuthentication');
                document.getElementById('txtPassword').value = '';
                document.getElementById('txtPassword').focus();
                return false;
            }
            else if (Type == 'H')
            {
                document.getElementById('divAuthentication').style.display = 'none';
                return false;
            }
            return true;
        }

        function ValidationForAuthentication()
        {
            if (document.getElementById('txtPassword').value.trim() == '')
            {
                document.getElementById('txtPassword').value = '';
                msgalert('Please Enter Password For Authentication !');
                document.getElementById('txtPassword').focus();
                return false;
            }
            return true;
        }
        function MeddraBrowser(MedExCode)
        {
            document.getElementById('hfMedexCode').value = MedExCode;
            //            var btn = document.getElementById('btnMeddraBrowse');
            //            btn.click();
            MedExCode = MedExCode.substring(0, MedExCode.indexOf('R'));
            window.open('frmCTMMeddraBrowse.aspx?MedExCode=' + MedExCode);
            return false;
        }

        function SetMeddra(objLLT)
        {
            var MedExCode = document.getElementById('hfMedexCode').value;
            var control = document.getElementById(MedExCode);
            if (control != null && typeof (control) != 'undefined')
            {
                control.value = objLLT.Meddra;
                control.focus();
            }
        }

        function ValidateReview()
        {
            var chk = document.getElementById('chkReviewCompleted');
            var chkAll = document.getElementById('chkReviewAll');
            if (!chk.checked && !chkAll.checked)
            {
                msgalert('Select the check box and then click on OK !');
                return false;
            }
            var DCFCount = document.getElementById('HFDCFCount').value;
            if (DCFCount > 0)
            {
                var Result = confirm(DCFCount + ' Discrepancy Pending, You Can Not Review.');
                return false;
            }
            //DivAuthenticationHideShow('S');
            document.getElementById('BtnAuthentication').click();
            document.getElementById('txtPassword').focus();
            //            PageMethods.GetServerDateTimeFor(function(Result)
            //            {
            //                document.getElementById('lblSignDateTime').innerHTML = Result;
            //            }, function(eerror) { alert(eerror); });

            return false;
        }

        function CheckTheEnterKey(e)
        {
            var unicode = e.charCode ? e.charCode : e.keyCode
            if (unicode == 13)
            {
                document.getElementById('btnAuthenticate').click();
                return false;
            }
            return true;
        }
        function MedExFormula(MedExCode, formula)
        {
            document.getElementById('hfMedexCode').value = MedExCode;
            document.getElementById('HFMedExFormula').value = formula;
            var btn = document.getElementById('btnAutoCalculate');
            btn.click();
            return false;
        }
        function SetFormulaResult(result)
        {
            var MedExCode = document.getElementById('hfMedexCode').value;
            var control = document.getElementById(MedExCode);
            if (control != null && typeof (control) != 'undefined')
            {
                control.value = result;
            }
        }

        function SetDosingTime(medexcode, BtnUpdateId)
        {
            var txtdosingtime = document.getElementById(medexcode);
            var d = new Date();
            var curr_hour = d.getHours();
            var curr_min = d.getMinutes();
            var curr_sec = d.getSeconds();
            txtdosingtime.value = curr_hour + ":" + curr_min + ":" + curr_sec;
            if (txtdosingtime.value.toUpperCase() == PreviousValue.toUpperCase() && BtnUpdateId != null)
            {
                return;
            }
            CheckToUpdateValue(BtnUpdateId);
            return true;
        }

        function ValidationForUpdate()
        {
            if (document.getElementById('txtRemarkForAttributeEdit').value.trim() == '')
            {
                msgalert('Please enter remarks !');
                document.getElementById('txtRemarkForAttributeEdit').value = '';
                document.getElementById('txtRemarkForAttributeEdit').focus();
                return false;
            }
            return true;
        }

        function ValidationForEditOrDelete()
        {

            if (document.getElementById('DdlEditRemarks').selectedIndex == '0')
            {
                if (document.getElementById('txtRemarkForAttributeEdit').value == '')
                {
                    msgalert('Select Either Reason Or Specify Remarks !');
                    document.getElementById('DdlEditRemarks').selectedIndex = 0;
                    document.getElementById('txtRemarkForAttributeEdit').value = '';
                    return false;
                }
                else
                {
                    document.getElementById('HdReasonDesc').value = document.getElementById('txtRemarkForAttributeEdit').value;
                    return true;
                }

            }
            else if (document.getElementById('txtRemarkForAttributeEdit').value != '')
            {
                msgalert('Select Either Reason Or Specify Remarks !');
                document.getElementById('DdlEditRemarks').selectedIndex = 0;
                document.getElementById('txtRemarkForAttributeEdit').value = '';
                return false;
            }
            else
            {
                document.getElementById('HdReasonDesc').value = document.getElementById('DdlEditRemarks').options[document.getElementById('DdlEditRemarks').selectedIndex].innerText;
                return true;

            }
        }

        function ValidationForDiscrepancy()
        {
            if (document.getElementById('txtDiscrepancyRemarks').value.trim() == '')
            {
                msgalert('Please enter remarks !');
                document.getElementById('txtDiscrepancyRemarks').value = '';
                document.getElementById('txtDiscrepancyRemarks').focus();
                return false;
            }
            return true;
        }

        var count = 0;
        var element;
        var prev;
        var result;

        function ValidationsForSave()
        {
            count = 0;
            jQuery('.Required').each(validateControls);
            if (count > 0)
            {
                var conf = confirm('' + count + ' Field(s) Are Blank, Do You Still Want To Save?');
                if (conf)
                {
                    document.getElementById('BtnSave').style.display = 'none';
                    document.getElementById('btnSaveAndContinue').style.display = 'none';
                }
                else
                {
                    return false;
                }
            }
            document.getElementById('BtnSave').style.display = 'none';
            document.getElementById('btnSaveAndContinue').style.display = 'none';
            jQuery('input[type=checkbox]', $get('UpPlaceHolder')).attr('disabled', false).parents('table').attr('disabled', false);
            //            if (jQuery('input[type=checkbox]', $get('UpPlaceHolder')).parents('table').length > 0)
            //                jQuery('input[type=checkbox]', $get('UpPlaceHolder')).parents('table')[0].disabled = false;
            onUpdating();
            return true;
        }

        function validateControls(index)
        {
            element = jQuery('.Required')[index];
            switch (element.type)
            {
                case 'text':
                    document.getElementById(element.id).style.borderColor = '';
                    if (element.value.trim().length <= 0)
                    {
                        document.getElementById(element.id).style.borderColor = 'Red';
                        count = count + 1;
                        return;
                    }
                    break;

                case 'select-one':
                    document.getElementById(element.id).style.backgroundColor = '';
                    if (element.value.trim().length <= 0)
                    {
                        document.getElementById(element.id).style.backgroundColor = 'Red';
                        count = count + 1;
                        return;
                    }
                    break;

                case 'radio':
                    if (prev != element.name)
                    {
                        prev = element.name;
                        var rdos = document.getElementsByName(element.name);
                        var j;
                        result = false;
                        for (j = 0; j < rdos.length; j++)
                        {
                            rdos[j].style.color = 'Navy';
                            if (rdos[j].checked)
                            {
                                result = true;
                                return;
                            }
                        }
                        if (result == false)
                        {
                            for (j = 0; j < rdos.length; j++)
                            {
                                rdos[j].style.color = 'Red';
                            }
                            count = count + 1;
                            return;
                        }
                    }
                    break;

                case 'checkbox':
                    if (prev != element.name)
                    {
                        prev = element.name;
                        var chks;
                        var j;
                        result = false;
                        if (element != null && typeof (element) != 'undefined')
                        {
                            if (document.getElementById(element.id).canHaveChildren == true)
                            {
                                chks = element.getElementsByTagName('input');
                                for (j = 0; j < chks.length; j++)
                                {
                                    chks[j].style.backgroundColor = '';
                                    if (chks[j].type.toUpperCase() == 'CHECKBOX' && chks[j].checked)
                                    {
                                        result = true;
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                element.style.backgroundColor = '';
                                if (element.checked)
                                {
                                    result = true;
                                    return;
                                }
                            }
                        }
                        if (result == false)
                        {
                            if (document.getElementById(element.id).canHaveChildren == true)
                            {
                                chks = element.getElementsByTagName('input');
                                for (j = 0; j < chks.length; j++)
                                {
                                    chks[j].style.backgroundColor = 'Red';
                                }
                            }
                            else
                            {
                                element.style.backgroundColor = 'Red';
                            }
                            count = count + 1;
                            return;
                        }
                    }
                    break;
            }
        }

        function CheckToUpdateValue(BtnUpdateId)
        {
            var btnUpdate = document.getElementById(BtnUpdateId);
            if (btnUpdate != null)
            {
                document.getElementById('BtnForEditAttribute').click();
            }
        }
        function SetValue(Id, Value)
        {
            if (ControlId != Id && PreviousValue == Value)
            {
                ControlId = Id;
                PreviousValue = Value;
            }
            else if (ControlId != Id)
            {
                ControlId = Id;
                PreviousValue = Value;
            }
        }

        function ResetValue()
        {
            //document.getElementById(ControlId).value = PreviousValue;
            element = document.getElementById(ControlId);
            if (element.type != 'undefined')
            {
                if (element.type == 'text')
                {
                    document.getElementById(ControlId).value = PreviousValue;
                }

                else if (element.type == 'select-one')
                {
                    document.getElementById(ControlId).value = PreviousValue;
                }

                else
                {
                    var tblRdo = $get(ControlId);
                    if (tblRdo != null)
                    {
                        var name = tblRdo.id;
                        name = name.replace(/_/g, '$');
                        var rdos = document.getElementsByName(name);
                        var i;
                        for (i = 0; i < rdos.length; i++)
                        {
                            if (rdos[i].nextSibling != null)
                            {
                                rdos[i].checked = false;
                                var values = PreviousValue.split('##');
                                var j;
                                for (j = 0; j < values.length; j++)
                                {
                                    if (rdos[i].nextSibling.innerText.toUpperCase() == values[j].toUpperCase())
                                    {
                                        rdos[i].checked = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        function GetCurrentDate(CtrlId)
        {
            var curr_Time = new Date();
            var curr_Dt = curr_Time.getDate();
            if (curr_Dt.toString().length < 2)
            {
                curr_Dt = '0' + curr_Dt;
            }
            var curr_mon = curr_Time.toString().substring(4, 7);
            if (curr_mon.toString().length < 2)
            {
                curr_mon = '0' + curr_mon;
            }
            var curr_yr = curr_Time.getFullYear();
            curr_Time.value = curr_Dt + "-" + curr_mon + "-" + curr_yr;

            document.getElementById(CtrlId).value = curr_Dt + "-" + curr_mon + "-" + curr_yr;
            return false;
        }
        function GetCurrentTime(CtrlId)
        {
            var curr_Time = new Date();
            var curr_hour = curr_Time.getHours();
            var curr_min = curr_Time.getMinutes();
            var curr_sec = curr_Time.getSeconds();
            if (curr_hour.toString().length < 2)
            {
                curr_hour = '0' + curr_hour;
            }
            if (curr_min.toString().length < 2)
            {
                curr_min = '0' + curr_min;
            }
            if (curr_sec.toString().length < 2)
            {
                curr_sec = '0' + curr_sec;
            }
            curr_Time.value = curr_hour + ":" + curr_min + ":" + curr_sec;

            //now = now.format("H:i:s");
            document.getElementById(CtrlId).value = curr_hour + ":" + curr_min + ":" + curr_sec;
            return false;
        }
        function ClearVal(CtrlId)
        {
            document.getElementById(CtrlId).value = '';
            return false;
        }
    </script>

</head>
<body leftmargin="0" topmargin="0">
    <form id="form1" runat="server" method="post">
    <div align="center">
        <center>
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="1000"
                EnablePageMethods="True">
                <Services>
                    <asp:ServiceReference Path="AutoComplete.asmx" />
                </Services>
            </asp:ScriptManager>
            <table border="0" cellspacing="0" style="border-collapse: collapse" bordercolor="#111111"
                width="998" id="AutoNumber1" cellpadding="0">
                <tr height="65">
                    <td style="vertical-align: bottom; width: 1000; height: 65; background-image: url(images/topheader.jpg);
                        background-repeat: no-repeat;" align="left">
                        <div align="right">
                            <table width="45%" border="0" align="right">
                                <tr>
                                    <td style="white-space: nowrap">
                                        <asp:Label ID="lblWelcome" runat="server" CssClass="Label" Text="Welcome, " ForeColor="#FFffff"></asp:Label><asp:Label
                                            ID="lblUserName" runat="server" CssClass="Label" ForeColor="#ffc300"></asp:Label>
                                        , &nbsp;
                                        <asp:Label ID="lblLoginTime1" runat="server" CssClass="Label" Text="Login Time: "
                                            ForeColor="#FFffff"></asp:Label><asp:Label ID="lblTime" runat="server" CssClass="Label"
                                                ForeColor="#ffc300"></asp:Label>
                                        <asp:Label runat="server" ID="lblSessionTimeCap" class="Label" Style="color: #FFFFFF"
                                            Text="Session Expires:"></asp:Label>
                                        <b><span class="Label" style="color: #FFFFFF" id="timerText"></span></b>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td height="8">
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 95%">
                        <div align="center">
                            <table border="1" cellspacing="1" bordercolor="#1560a1" width="95%" cellpadding="0">
                                <tr>
                                    <td align="center" style="width: 98%">
                                        <asp:Panel ID="Pan_Hdr" runat="server" Width="100%" CssClass="InnerTable" BackColor="White">
                                            <asp:Panel ID="Pan_Child" runat="server" BackColor="Window" Width="100%">
                                                <div style="text-align: center" id="Header Label" class="Div" align="center">
                                                    <table style="width: 100%" align="center">
                                                        <tbody>
                                                            <tr align="center">
                                                                <td align="center" width="890">
                                                                    <strong style="font-weight: bold; font-size: 20px"></strong>
                                                                    <table style="width: 90%">
                                                                        <tbody>
                                                                            <tr>
                                                                                <td style="width: 100%" align="center">
                                                                                    <asp:Label ID="lblHeader" runat="server" SkinID="lblHeading"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                    <hr style="background-image: none; width: 982px; color: #ffcc66; background-color: #ffcc66" />
                                                                </td>
                                                            </tr>
                                                            <tr align="center">
                                                                <td align="left" width="890">
                                                                    <table>
                                                                        <tbody>
                                                                            <tr id="Tr1" runat="server">
                                                                                <td style="width: 980px; white-space: nowrap; text-align: left" class="Label" align="left">
                                                                                    <table style="width: 100%; white-space: nowrap; text-align: left" class="Label" align="center">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="text-align: left; white-space: nowrap;">
                                                                                                    <asp:DropDownList ID="ddlActivities" runat="server" CssClass="Label" Font-Bold="true"
                                                                                                        Font-Size="Small" AutoPostBack="True" Width="400px">
                                                                                                    </asp:DropDownList>
                                                                                                    <asp:Label ID="lblLastReviewedBy" runat="server"></asp:Label>
                                                                                                </td>
                                                                                                <td style="text-align: left;" align="left">
                                                                                                    <%--<asp:UpdatePanel ID="UPnlQuery" runat="server">
                                                                                                        <ContentTemplate>--%>
                                                                                                    <asp:Button Visible="false" ID="btnShowquery" runat="server" Text="Show Queries"
                                                                                                        CssClass="btn btnnew"  OnClientClick="return AnyDivShowHide('DIVQUERIESSHOW');">
                                                                                                    </asp:Button>
                                                                                                    <asp:Button Visible="false" ID="btnReviewHistory" runat="server" Text="Show Review History"
                                                                                                        CssClass="btn btnnew" ></asp:Button>
                                                                                                    <%--</ContentTemplate>
                                                                                                        <Triggers>
                                                                                                            <asp:AsyncPostBackTrigger ControlID="btnSaveRemarksForAttribute" EventName="Click">
                                                                                                            </asp:AsyncPostBackTrigger>
                                                                                                        </Triggers>
                                                                                                    </asp:UpdatePanel>--%>
                                                                                                </td>
                                                                                                <td style="text-align: right;">
                                                                                                    &nbsp;Legends&nbsp;<img id="imgActivityLegends" src="images/question.gif" runat="server" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="vertical-align: middle; width: 3px; background-repeat: no-repeat; white-space: nowrap;
                                                                                    background-color: transparent" align="left">
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="Tr2" runat="server">
                                                                                <td style="width: 980px; white-space: nowrap;" class="Label" valign="middle" align="right">
                                                                                    <div style="border-right: black 2px outset; border-top: black 2px outset; display: none;
                                                                                        font-size: 8pt; border-left: black 2px outset; border-bottom: black 2px outset;
                                                                                        height: auto; background-color: white; text-align: left" id="divActivityLegends"
                                                                                        runat="server">
                                                                                        <asp:Label ID="lblRed" runat="Server" BackColor="red">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                                                        Entry Pending,
                                                                                        <asp:Label ID="lblOrange" runat="Server" BackColor="orange">&nbsp;&nbsp;&nbsp;</asp:Label>-Data
                                                                                        Entry Continue,
                                                                                        <asp:Label ID="lblBlue" runat="Server" BackColor="blue">&nbsp;&nbsp;&nbsp;</asp:Label>-Ready
                                                                                        For Review,
                                                                                        <asp:Label ID="lblYellow" runat="Server" BackColor="#50C000">&nbsp;&nbsp;&nbsp;</asp:Label>-First
                                                                                        Review Done,
                                                                                        <asp:Label ID="lblGreen" runat="Server" BackColor="#006000">&nbsp;&nbsp;&nbsp;</asp:Label>-Second
                                                                                        Review Done,
                                                                                        <asp:Label ID="lblGray" runat="Server" BackColor="gray">&nbsp;&nbsp;&nbsp;</asp:Label>-Reviewed
                                                                                        & Freeze
                                                                                    </div>
                                                                                </td>
                                                                                <td style="vertical-align: middle; width: 3px; background-repeat: no-repeat; white-space: nowrap;
                                                                                    background-color: transparent" align="left">
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trReviewCompleted" runat="server">
                                                                                <td style="width: 894px" class="Label" align="center">
                                                                                    <asp:CheckBox ID="chkReviewCompleted" runat="server" Text="Review Completed"></asp:CheckBox>
                                                                                    <asp:Button ID="btnOk" runat="server" Text="Ok" CssClass="btn btnnew" OnClientClick="return ValidateReview();">
                                                                                    </asp:Button>
                                                                                    <asp:CheckBox ID="chkReviewAll" runat="server" Text="Review All" />
                                                                                </td>
                                                                                <td style="vertical-align: middle; width: 3px; background-repeat: no-repeat; white-space: nowrap;
                                                                                    background-color: transparent" align="left">
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 894px" class="Label" align="left">
                                                                                    <asp:DropDownList ID="ddlRepeatNo" runat="server" CssClass="Label" AutoPostBack="true">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td style="vertical-align: middle; width: 3px; background-repeat: no-repeat; white-space: nowrap;
                                                                                    background-color: transparent" align="left">
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trimageinformation" align="right" runat="server" visible="false">
                                                                                <td style="width: 980px; white-space: nowrap" class="Label" valign="middle" align="right">
                                                                                    Legends
                                                                                    <img id="imgShow" src="images/question.gif" runat="server" />
                                                                                    <div style="border-right: black 2px outset; border-top: black 2px outset; display: none;
                                                                                        font-size: 8pt; border-left: black 2px outset; border-bottom: black 2px outset;
                                                                                        height: auto; background-color: white; text-align: left" id="canal" runat="server">
                                                                                        <img id="imgedit" src="images/Edit2.gif" runat="server" />Edit Field Value
                                                                                        <img id="imgupdate" src="images/save.gif" runat="server" />Save
                                                                                        <img id="imgdiscrepancy" src="images/Discrepancy.png" runat="server" />Discrepancy
                                                                                        <img id="imgaudittrail" src="images/paste.png" runat="server" />Audit trail
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left" colspan="2">
                                                                                    <table width="100%">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td valign="top" align="left">
                                                                                                    <asp:UpdatePanel ID="UpPlaceHolder" runat="server">
                                                                                                        <ContentTemplate>
                                                                                                            <asp:Panel ID="PnlPlaceMedex" runat="server" Width="100%">
                                                                                                                <asp:PlaceHolder ID="PlaceMedEx" runat="server" EnableViewState="true"></asp:PlaceHolder>
                                                                                                            </asp:Panel>
                                                                                                            <button id="BtnForAuditTrail" runat="server" style="display: none;" />
                                                                                                            <cc1:ModalPopupExtender ID="MpeAuditTrail" runat="server" PopupControlID="divHistoryDtl"
                                                                                                                PopupDragHandleControlID="lblMedexDescription" BackgroundCssClass="modalBackground"
                                                                                                                TargetControlID="BtnForAuditTrail" CancelControlID="ImgPopUpCloseAuditTrail">
                                                                                                            </cc1:ModalPopupExtender>
                                                                                                            <div style="display: none; left: 391px; width: 650px; top: 528px; height: 250px;
                                                                                                                text-align: left" id="divHistoryDtl" class="DIVSTYLE2" runat="server">
                                                                                                                <table style="width: 650px">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td valign="middle">
                                                                                                                                <strong style="white-space: nowrap; vertical-align: middle;">History Of Attribute :
                                                                                                                                    <asp:Label ID="lblMedexDescription" runat="server"></asp:Label></strong>
                                                                                                                            </td>
                                                                                                                            <td align="right">
                                                                                                                                <img id="ImgPopUpCloseAuditTrail" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                                    float: right; right: 5px;" />
                                                                                                                                <%--<img style="width: 21px; height: 15px" onclick="HistoryDivShowHide('H','','','');"
                                                                                                                                    src="images/close.gif" />
                                                                                                                                &nbsp;--%>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td colspan="2" style="text-align: center;">
                                                                                                                                <asp:Panel ID="pnlHistoryDtl" runat="server" Width="630px" ScrollBars="Auto" Height="220px">
                                                                                                                                    <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                                    <asp:GridView ID="GVHistoryDtl" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                                                        Font-Size="Small" SkinID="grdViewSmlAutoSize">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:BoundField DataField="iTranNo" HeaderText="Sr. No." />
                                                                                                                                            <asp:BoundField DataField="vMedExResult" HeaderText="Value" />
                                                                                                                                            <asp:BoundField DataField="vModificationRemark" HeaderText="Reason" />
                                                                                                                                            <asp:BoundField DataField="CRFSubDtlChangedBy" HeaderText="Modify By">
                                                                                                                                                <ItemStyle Wrap="False" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="dModifyOnSubDtl" HeaderText="Modify On" DataFormatString="{0:dd-MMM-yyyy HH:mm tt}">
                                                                                                                                                <ItemStyle Wrap="False" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                                </asp:Panel>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <button id="BtnForEditAttribute" runat="server" style="display: none;" />
                                                                                                            <cc1:ModalPopupExtender ID="MpeEditAttribute" runat="server" PopupControlID="divForEditAttribute"
                                                                                                                PopupDragHandleControlID="lblReasonForEdit" BackgroundCssClass="modalBackground"
                                                                                                                TargetControlID="BtnForEditAttribute" CancelControlID="ImgPopUpCloseEditAttribute">
                                                                                                            </cc1:ModalPopupExtender>
                                                                                                            <div style="display: none; left: 391px; width: 400px; top: 528px; height: 210px;
                                                                                                                text-align: left" id="divForEditAttribute" class="DIVSTYLE2" runat="server">
                                                                                                                <table style="width: 350px">
                                                                                                                    <tbody>
                                                                                                                        <tr align="center" style="height: 20px">
                                                                                                                            <td class="Label" align="center" colspan="2">
                                                                                                                                <strong style="white-space: nowrap">
                                                                                                                                    <asp:Label ID="lblReasonForEdit" runat="server" Text="Reason For Edit"></asp:Label>
                                                                                                                                </strong>&nbsp;&nbsp;
                                                                                                                                <img id="ImgPopUpCloseEditAttribute" onclick="ResetValue();" alt="Close" src="images/Sqclose.gif"
                                                                                                                                    style="position: relative; float: right; right: 5px;" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <%--Added By Dharmesh H.Salla on 11-May-2011--%>
                                                                                                                        <tr>
                                                                                                                            <td style="white-space: nowrap; text-align: left;" class="Label">
                                                                                                                                Reason* :
                                                                                                                            </td>
                                                                                                                            <td class="Label" align="left">
                                                                                                                                <asp:DropDownList ID="DdlEditRemarks" runat="server">
                                                                                                                                </asp:DropDownList>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="white-space: nowrap" class="Label" align="LEFT">
                                                                                                                                OR
                                                                                                                            </td>
                                                                                                                            <td class="Label" align="left">
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <%---------------------------------------------------%>
                                                                                                                        <tr>
                                                                                                                            <td style="white-space: nowrap" class="Label" align="right">
                                                                                                                                Remark* :
                                                                                                                            </td>
                                                                                                                            <td class="Label" align="left">
                                                                                                                                <asp:TextBox ID="txtRemarkForAttributeEdit" runat="Server" Text="" CssClass="textbox"
                                                                                                                                    Width="240px" Height="100px"> </asp:TextBox>
                                                                                                                                <asp:HiddenField ID="HdReasonDesc" runat="server" Value="" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr align="center">
                                                                                                                            <td align="right" class="Label" style="white-space: nowrap">
                                                                                                                            </td>
                                                                                                                            <td align="left" class="Label">
                                                                                                                                <asp:Button ID="btnSaveRemarksForAttribute" runat="server" CssClass="btn btnsave" OnClientClick="return ValidationForEditOrDelete();"
                                                                                                                                    Text="Save" />
                                                                                                                                <%--<asp:Button ID="btnCloseRemarksForAttribute" runat="server" Text="Close" CssClass="button"
                                                                                                                                    OnClientClick="AnyDivShowHide('H');">
                                                                                                                                    </asp:Button>--%>
                                                                                                                                <input class="button" onclick="$get('ImgPopUpCloseEditAttribute').click();" type="button"
                                                                                                                                    value="Close" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <button id="BtnForDCF" runat="server" style="display: none;" />
                                                                                                            <cc1:ModalPopupExtender ID="MpeDCF" runat="server" PopupControlID="divDCF" PopupDragHandleControlID="lblAttributeDCF"
                                                                                                                BackgroundCssClass="modalBackground" TargetControlID="BtnForDCF" CancelControlID="ImgPopUpCloseDCF">
                                                                                                            </cc1:ModalPopupExtender>
                                                                                                            <div style="display: none; left: 391px; width: 650px; top: 460px; height: 450px;
                                                                                                                text-align: left" id="divDCF" class="DIVSTYLE2" runat="server">
                                                                                                                <table style="width: 650px">
                                                                                                                    <tbody>
                                                                                                                        <tr align="center">
                                                                                                                            <td style="text-align: center;" align="center" colspan="2">
                                                                                                                                <strong style="white-space: nowrap">Discrepancy Of Attribute :
                                                                                                                                    <asp:Label ID="lblAttributeDCF" runat="server"></asp:Label></strong>
                                                                                                                                <img id="ImgPopUpCloseDCF" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                                    float: right; right: 5px;" />
                                                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td align="right" style="text-align: right">
                                                                                                                                Remarks For Discrepancy :
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left">
                                                                                                                                <asp:TextBox ID="txtDiscrepancyRemarks" runat="Server" Text="" CssClass="textBox"
                                                                                                                                    Width="226px" TextMode="MultiLine" Height="59px"></asp:TextBox>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td align="right" style="text-align: right">
                                                                                                                                Status :
                                                                                                                            </td>
                                                                                                                            <td class="Label" align="left" style="text-align: left">
                                                                                                                                <asp:DropDownList ID="ddlDiscrepancyStatus" runat="server" CssClass="Label">
                                                                                                                                    <asp:ListItem Text="Generated" Value="N" Selected="True"></asp:ListItem>
                                                                                                                                    <asp:ListItem Text="Answered" Value="O"></asp:ListItem>
                                                                                                                                    <asp:ListItem Text="Resolved" Value="R"></asp:ListItem>
                                                                                                                                    <asp:ListItem Text="Internally Resolved" Value="I"></asp:ListItem>
                                                                                                                                </asp:DropDownList>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td>
                                                                                                                            </td>
                                                                                                                            <td style="text-align: left" align="right">
                                                                                                                                <asp:Button ID="btnSaveDiscrepancy" runat="server" Text="Add" CssClass="btn btnsave" OnClientClick="return ValidationForDiscrepancy();">
                                                                                                                                </asp:Button>
                                                                                                                                <asp:Button ID="btnUpdateDiscrepancy" runat="server" Text="Update" CssClass="btn btnsave"
                                                                                                                                    OnClientClick="return ValidationForDiscrepancy();" Width="133px"></asp:Button>
                                                                                                                                <%--<asp:Button ID="btnCloseDiscrepency" runat="server" Text="Close" CssClass="button"
                                                                                                                                OnClientClick="$get('ImgPopUpCloseDCF').click();" />--%>
                                                                                                                                <%--OnClientClick="return AnyDivShowHide('DCFHIDE');"></asp:Button>--%>
                                                                                                                                <input type="button" value="Close" class="button" onclick="$get('ImgPopUpCloseDCF').click();" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="height: 260px" colspan="2">
                                                                                                                                <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                                <asp:Panel ID="pnlDCFGrid" runat="server" Width="640px" ScrollBars="Auto" Height="250px">
                                                                                                                                    <br />
                                                                                                                                    <asp:GridView ID="GVWDCF" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                                                        Font-Size="Small" SkinID="grdViewSmlAutoSize">
                                                                                                                                        <Columns>
                                                                                                                                            <asp:BoundField DataField="nDCFNo" HeaderText="DCFNo"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="nCRFDtlNo" HeaderText="CRFDtlNo"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="iSrNo" HeaderText="SrNo"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="vMedExCode" HeaderText="MedExCode"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="cDCFType" HeaderText="DCF Type">
                                                                                                                                                <ItemStyle Wrap="False" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="iDCFBy" HeaderText="iDCFBy" />
                                                                                                                                            <asp:BoundField DataField="vCreatedBy" HeaderText="CreatedBy" />
                                                                                                                                            <asp:BoundField DataField="dDCFDate" HeaderText="DCF Date" HtmlEncode="False">
                                                                                                                                                <ItemStyle Wrap="False" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="vDiscrepancy" HeaderText="Discrepancy" />
                                                                                                                                            <asp:BoundField DataField="vSourceResponse" HeaderText="Remarks"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="cDCFStatus" HeaderText="Status"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="vUpdatedBy" HeaderText="Updated By"></asp:BoundField>
                                                                                                                                            <asp:BoundField DataField="dStatusChangedOn" HeaderText="Updated On" HtmlEncode="False">
                                                                                                                                                <ItemStyle Wrap="False" />
                                                                                                                                            </asp:BoundField>
                                                                                                                                            <asp:TemplateField HeaderText="Update">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:LinkButton ID="lnkbtnUpdate" runat="server" CssClass="LinkButton" Text="Update"
                                                                                                                                                        CommandName="Update" Style="white-space: nowrap"></asp:LinkButton>
                                                                                                                                                </ItemTemplate>
                                                                                                                                            </asp:TemplateField>
                                                                                                                                            <asp:BoundField DataField="vUserTypeCode" HeaderText="UserTypeCode" />
                                                                                                                                            <%-- <asp:TemplateField HeaderText="Internally Resolved">
                                                                                                                                                <ItemTemplate>
                                                                                                                                                    <asp:LinkButton ID="LnkBtnInternallyResolved" runat="server" CssClass="LinkButton" Text="Internally Resolved"
                                                                                                                                                        CommandName="IR" ></asp:LinkButton>
                                                                                                                                                </ItemTemplate>
                                                                                                                                            </asp:TemplateField>--%>
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                                </asp:Panel>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <asp:Button Style="display: none" ID="btnEdit" OnClick="btnEdit_Click" runat="server"
                                                                                                                Text="Edit" CssClass="btn btnsave"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="btnDCF" OnClick="btnDCF_Click" runat="server"
                                                                                                                Text="DCF" CssClass="btn btnnew"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="btnAudittrail" OnClick="btnAudittrail_Click"
                                                                                                                runat="server" Text="btn btnaudit" CssClass="button"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="btnSaveRunTime" OnClick="btnSaveRunTime_Click"
                                                                                                                runat="server" Text="SaveRunTime" CssClass="btn btnsave"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="btnMeddraBrowse" OnClick="btnMeddraBrowse_Click"
                                                                                                                runat="server" Text="MedDRA Browser" CssClass="btn btnnew"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="btnAutoCalculate" OnClick="btnAutoCalculate_Click"
                                                                                                                runat="server" Text="Auto Calculate" CssClass="btn btnnew"></asp:Button>
                                                                                                            <asp:Button Style="display: none" ID="BtnDosingTime" OnClick="BtnDosingTime_Click"
                                                                                                                runat="server" Text="DosingTime" CssClass="btn btnnew"></asp:Button>
                                                                                                            <div style="display: none; left: 391px; width: 650px; top: 460px; height: 450px;
                                                                                                                text-align: left" id="divQueries" class="DIVSTYLE2" runat="server">
                                                                                                                <table style="width: 650px">
                                                                                                                    <tbody>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: center" align="center">
                                                                                                                                <strong style="white-space: nowrap">Queries Of Activity</strong>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="text-align: center" align="center">
                                                                                                                                <asp:Button ID="btnCloseQuery" runat="server" Text="Close" CssClass="btn btnclose" OnClientClick="return AnyDivShowHide('DIVQUERIESHIDE');">
                                                                                                                                </asp:Button>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td style="height: 270px">
                                                                                                                                <asp:Panel ID="pnlQuery" runat="server" Width="640px" ScrollBars="Auto" Height="250px">
                                                                                                                                    <hr style="background-image: none; color: #ffcc66; background-color: #ffcc66" />
                                                                                                                                    <br />
                                                                                                                                    <asp:GridView ID="gvwQueries" runat="server" AutoGenerateColumns="False" BorderColor="Peru"
                                                                                                                                        Font-Size="Small" SkinID="grdViewSmlAutoSize">
                                                                                                                                        <Columns>
                                                                                                                                            <%--<asp:BoundField DataField="vQueryValue" HeaderText="Query Value" />--%>
                                                                                                                                            <asp:BoundField DataField="vRemarks" HeaderText="Query Remarks" />
                                                                                                                                        </Columns>
                                                                                                                                    </asp:GridView>
                                                                                                                                </asp:Panel>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </tbody>
                                                                                                                </table>
                                                                                                            </div>
                                                                                                            <asp:HiddenField ID="HFWorkspaceId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFActivityId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFParentActivityId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFNodeId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFParentNodeId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFPeriodId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFSubjectId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFMySubjectNo" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFScreenNo" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFMedexInfoDtlTranNo" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFCRFHdrNo" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFCRFDtlNo" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="hfMedexCode" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFCRFDtlLockStatus" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFMedExFormula" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFRadioButtonValue" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFReviewedWorkFlowId" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFDCFCount" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFActivateTab" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFSessionFlg" runat="server"></asp:HiddenField>
                                                                                                            <asp:HiddenField ID="HFImportedDataWorkFlowId" runat="server"></asp:HiddenField>
                                                                                                        </ContentTemplate>
                                                                                                    </asp:UpdatePanel>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="width: 950px; height: 40px" class="Label" align="center">
                                                                                    <table style="width: 940px">
                                                                                        <tbody>
                                                                                            <tr>
                                                                                                <td style="width: 50%; height: 13px" align="left">
                                                                                                    <asp:Button ID="BtnPrevious" runat="server" Text="<< Previous" CssClass="btn btnsave"></asp:Button>
                                                                                                </td>
                                                                                                <td style="white-space: nowrap; height: 13px" class="Label" align="center">
                                                                                                    <asp:Button ID="btnSaveAndContinue" runat="server" Text="Save & Continue" CssClass="btn btnsave"
                                                                                                       OnClientClick="return ValidationsForSave();"></asp:Button>&nbsp;
                                                                                                    <asp:Button ID="BtnSave" runat="server" Text="Submit" CssClass="btn btnsave" OnClientClick="return ValidationsForSave();">
                                                                                                    </asp:Button>
                                                                                                    &nbsp;
                                                                                                    <%--<input id="BtnExit" class="btn btnexit" onclick="return closewindow(this);" type="button" value="Exit" />--%>
                                                                                                    <asp:Button ID="BtnExit" runat="server" Text="Exit" CssClass="btn btnexit" OnClientClick="return closewindow(this);"/>
                                                                                                </td>
                                                                                                <td style="width: 50%; height: 13px" align="right">
                                                                                                    <asp:Button ID="BtnNext" runat="server" Text="Next >>" CssClass="btn btnsave">
                                                                                                    </asp:Button>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <button id="BtnAuthentication" runat="server" style="display: none;" />
                                                                                                    <cc1:ModalPopupExtender ID="MpeAuthentication" runat="server" PopupControlID="divAuthentication"
                                                                                                        PopupDragHandleControlID="lblAttributeDCF" BackgroundCssClass="modalBackground"
                                                                                                        TargetControlID="BtnAuthentication" CancelControlID="ImgPopUpCloseAuthentication">
                                                                                                    </cc1:ModalPopupExtender>
                                                                                                    <div style="display: none; left: 391px; width: 450px; top: 528px; height: 200px;
                                                                                                        text-align: left" id="divAuthentication" class="DIVSTYLE2" runat="server">
                                                                                                        <table style="width: 400px;" align="center">
                                                                                                            <tbody>
                                                                                                                <tr style="height: 10px;">
                                                                                                                </tr>
                                                                                                                <tr align="center">
                                                                                                                    <td class="Label" align="center" colspan="3">
                                                                                                                        <strong style="white-space: nowrap">User Authentication</strong>
                                                                                                                        <img id="ImgPopUpCloseAuthentication" alt="Close" src="images/Sqclose.gif" style="position: relative;
                                                                                                                            float: right;" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 10px;">
                                                                                                                </tr>
                                                                                                                <tr runat="server" id="trName">
                                                                                                                    <td style="white-space: nowrap" class="Label" align="left">
                                                                                                                        Name
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;:&nbsp
                                                                                                                    </td>
                                                                                                                    <td class="Label" align="left">
                                                                                                                        <asp:Label runat="server" ID="lblSignername" Text=""></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 5px;">
                                                                                                                </tr>
                                                                                                                <tr runat="server" id="trDesignation">
                                                                                                                    <td style="white-space: nowrap" class="Label" align="left">
                                                                                                                        Designation
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;:&nbsp
                                                                                                                    </td>
                                                                                                                    <td class="Label" align="left">
                                                                                                                        <asp:Label runat="server" ID="lblSignerDesignation" Text=""></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 5px;">
                                                                                                                </tr>
                                                                                                                <%--<tr>
                                                                                                                    <td style="white-space: nowrap" class="Label" align="left">
                                                                                                                        Date & Time
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;:&nbsp
                                                                                                                    </td>
                                                                                                                    <td class="Label" align="left">
                                                                                                                        <asp:Label runat="server" ID="lblSignDateTime" Text=""></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 5px;"></tr>--%>
                                                                                                                <tr runat="server" id="trRemarks">
                                                                                                                    <td style="white-space: nowrap" class="Label" align="left">
                                                                                                                        Remarks
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;:&nbsp
                                                                                                                    </td>
                                                                                                                    <td class="Label" align="left">
                                                                                                                        <asp:Label runat="server" ID="lblSignRemarks" Text=""></asp:Label>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 5px;">
                                                                                                                </tr>
                                                                                                                <tr>
                                                                                                                    <td style="white-space: nowrap" class="Label" align="left">
                                                                                                                        Password
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;:&nbsp
                                                                                                                    </td>
                                                                                                                    <td class="Label" align="left">
                                                                                                                        <asp:TextBox ID="txtPassword" TabIndex="21" onkeydown="return CheckTheEnterKey(event);"
                                                                                                                            runat="Server" Text="" CssClass="textbox" TextMode="Password"> </asp:TextBox>
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                                <tr style="height: 10px;">
                                                                                                                </tr>
                                                                                                                <tr align="center">
                                                                                                                    <td align="right" class="Label" style="white-space: nowrap">
                                                                                                                    </td>
                                                                                                                    <td>
                                                                                                                        &nbsp;&nbsp;
                                                                                                                    </td>
                                                                                                                    <td align="left" class="Label">
                                                                                                                        <asp:Button ID="btnAuthenticate" runat="server" CssClass="btn btnsave" OnClientClick="return ValidationForAuthentication();"
                                                                                                                            TabIndex="22" Text="Authenticate" />
                                                                                                                        <%--<asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" OnClientClick="return DivAuthenticationHideShow('H');"></asp:Button>--%>
                                                                                                                        <input class="button" onclick="$get('ImgPopUpCloseAuthentication').click();" type="button"
                                                                                                                            value="Close" />
                                                                                                                    </td>
                                                                                                                </tr>
                                                                                                            </tbody>
                                                                                                        </table>
                                                                                                    </div>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </tbody>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </tbody>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#1560a1" valign="middle" height="18">
                        <p align="center">

                            <script type="text/javascript">
                                var copyright;
                                var update;
                                copyright = new Date();
                                update = copyright.getFullYear();
                                document.write("<font face=\"verdana\" size=\"1\" color=\"white\">© Copyright " + update + ", Lambda Therapeutic Research. </font>");
                            </script>
                    </td>
                </tr>
                <tr>
                    <td style="height: 19px">
                        <div id="updateProgress" class="updateProgress" style="display: none; vertical-align: middle">
                            <div align="center">
                                <table>
                                    <tr>
                                        <td style="height: 130px">
                                            <font class="updateText">Please Wait....</font>
                                        </td>
                                        <td style="height: 130px">
                                            <div title="Wait" class="updateImage">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
            <asp:UpdatePanel ID="upTest" runat="server">
                <ContentTemplate>
                    <table>
                        <tbody>
                            <tr>
                                <td>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddlActivities" EventName="SelectedIndexChanged">
                    </asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnOk" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnAuthenticate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnSaveRemarksForAttribute" EventName="Click"></asp:AsyncPostBackTrigger>--%>
                    <%--<asp:AsyncPostBackTrigger ControlID="btnAudittrail" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnAuthenticate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnAutoCalculate" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnDCF" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnMeddraBrowse" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="BtnSave" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveAndContinue" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveDiscrepancy" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveRemarksForAttribute" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="btnUpdateDiscrepancy" EventName="Click"></asp:AsyncPostBackTrigger>
                    <asp:AsyncPostBackTrigger ControlID="rblRepeatNo" EventName="SelectedIndexChanged"></asp:AsyncPostBackTrigger>--%>
                </Triggers>
            </asp:UpdatePanel>
        </center>
    </div>

                <asp:Button ID="btnmdl" runat="server" Style="display: none;" />
    <cc1:ModalPopupExtender ID="mdlSessionTimeoutWarning" runat="server" PopupControlID="divSessionTimeoutWarning"
        BackgroundCssClass="modalBackground" BehaviorID="mdlSessionTimeoutWarning" TargetControlID="btnmdl">
    </cc1:ModalPopupExtender>
    <div id="divSessionTimeoutWarning" runat="server" class="centerModalPopup" style="display: none;
        overflow: auto; width: 35%; height: auto; max-height: 25%; min-height: auto;">
        <asp:UpdatePanel ID="HM_Home_upnlSession" runat="server" UpdateMode="Conditional"
            RenderMode="Inline">
            <ContentTemplate>
                <table width="350px" align="center">
                    <tr>
                        <td>
                            <img id="Img1" src="~/Images/showQuery.png" runat="server" alt="Confirmation" />
                        </td>
                        <td class="Label" style="text-align: left;">
                            Your session will expire within 5 mins.
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btnContinueWorking" runat="server" Text="Extend" CssClass="btn btnnew" Style="display: none;" />
                            <asp:Button ID="BtnSessionDivClose" runat="server" Text="Close" CssClass="btn btnnew"
                                OnClientClick="closeSessionDiv();" />
                            <asp:Button ID="btnLogout" runat="server" Text="" CssClass="btn btngo" Style="display: none;" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_initializeRequest(onUpdating);
        prm.add_endRequest(onUpdated);

        var ACTUAL_SESSIONTIME = "<%= Session.Timeout %>", timerId, sessionFlag = true;
        SessionTimeSet();

        function onUpdating(sender, args)
        {
            var updateProgressDiv = $get('updateProgress');

            createDiv();
            updateProgressDiv.style.display = '';
            setLayerPosition();
        }

        function createDiv()
        {
            var divTag = document.createElement('div');

            divTag.id = 'shadow';
            divTag.setAttribute('align', 'center');
            divTag.style.position = 'absolute';
            divTag.style.top = '0px';
            divTag.style.left = '0px';
            divTag.style.opacity = '0.6';
            divTag.style.filter = 'alpha(opacity=30)';
            divTag.style.backgroundColor = '#000000';
            divTag.style.zIndex = '1000';

            document.body.appendChild(divTag);
        }

        function onUpdated(sender, args)
        {
            // get the update progress div
            var updateProgressDiv = $get('updateProgress');
            // make it invisible
            updateProgressDiv.style.display = 'none';
            document.body.removeChild(document.getElementById('shadow'));
            //clearTimeout(tld);
        }

        function setLayerPosition()
        {

            var winScroll = BodyScrollHeight();
            var updateProgressDivBounds =

              Sys.UI.DomElement.getBounds($get('updateProgress'));
            var shadow = document.getElementById('shadow');
            var bws = GetWindowBounds();

            if (!shadow)
            {
                return;
            }

            shadow.style.width = bws.Width + "px";
            shadow.style.height = bws.Height + "px";
            shadow.style.top = winScroll.yScr;
            shadow.style.left = winScroll.xScr;

            x = Math.round((bws.Width - updateProgressDivBounds.width) / 2);
            y = Math.round((bws.Height - updateProgressDivBounds.height) / 2);

            x += winScroll.xScr;
            y += winScroll.yScr;

            Sys.UI.DomElement.setLocation($get('updateProgress'), parseInt(x), parseInt(y));
        }
        window.onresize = setLayerPosition;
        window.onscroll = setLayerPosition;

        function pageLoad()
        {
            jQuery('#<%= btnShowQuery.ClientID %>').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow').fadeToggle('slow');
        }

        function show_popup(str)
        {
            var arg = 'resizable=no, toolbar=no,location=no,directories=no,addressbar=no,scrollbars=no,status=no,menubar=no,top=100,left=250';
            window.open(str, "_blank", arg);
            return false;
        }
    </script>

    </form>
</body>
</html>
