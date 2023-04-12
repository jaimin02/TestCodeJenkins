<%@ control language="VB" autoeventwireup="false" inherits="UserControls_MultiSelectDropDown, App_Web_cfmlkvsy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<style type="text/css">
    /*.textbox
    {
        -webkit-appearance: button;
        -moz-appearance: button;
        appearance: button;
        -webkit-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        -moz-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
        color: #555;
        font-size: inherit;
        margin: 0;
        overflow: hidden; /*PADDING-TOP: 2px;PADDING-BOTTOM: 2px;*//*  line-height: 165%;
        text-overflow: ellipsis;
        white-space: nowrap;
    }
    .webkit textbox
    {
        -webkit-border-radius: 2px;
        -webkit-padding-end: 20px;
        -webkit-padding-start: 2px;
        background-image: url(     "images/select.png" ), -webkit-linear-gradient(#fafafa, #f4f4f4 40%, #e5e5e5);
        background-position: center right;
        background-repeat: no-repeat;
        border: 1px solid #aaa;
    }
    .mozilla textbox
    {
        -moz-border-radius: 2px;
        -moz-padding-start: 2px;
        background-image: url(     "images/select.png" ), -moz-linear-gradient(#fafafa, #f4f4f4 40%, #e5e5e5);
        background-position: center right;
        background-repeat: no-repeat;
        border: 1px solid #aaa;
    }   */textbox:enabled:hover
    {
        -webkit-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
        -moz-box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
        box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2); /*COLOR: #333;*/
    }
    .webkit textbox:enabled:hover, .mozilla textbox:enabled:hover
    {
        background-image: url(      "images/select.png" ), -webkit-linear-gradient(#fefefe, #f8f8f8 40%, #e9e9e9);
        background-image: url(      "images/select.png" ), -moz-linear-gradient(#fefefe, #f8f8f8 40%, #e9e9e9);
    }
    textbox:enabled:active
    {
        -webkit-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.2);
        -moz-box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.2);
        box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.2); /*COLOR: #444;*/
    }
    .webkit textbox:enabled:active, .mozilla textbox:enabled:active
    {
        background-image: url(      "images/select.png" ), -webkit-linear-gradient(#f4f4f4, #efefef 40%, #dcdcdc);
        background-image: url(      "images/select.png" ), -moz-linear-gradient(#f4f4f4, #efefef 40%, #dcdcdc);
    }
</style>

<script type="text/javascript">
    function pageLoad() {
        if ($.browser.webkit) {
            $('html').addClass('webkit');
        }
        else if ($.browser.mozilla) {
            $('html').addClass('mozilla');
        }
    }
    function AssingWidth() {
        $get('<%=Panel111.ClientID %>').style.width = $get('<%=txtCombo.ClientID %>').clientWidth;
        $get('<%=cbList.ClientID %>').style.width = $get('<%=txtCombo.ClientID %>').clientWidth;
        $get('DivChk').style.width = $get('<%=txtCombo.ClientID %>').clientWidth;
        return false;
    }
    function CheckItem(checkBoxList, cIsFromHdr) {
        var options = checkBoxList.getElementsByTagName('input');
        var arrayOfCheckBoxLabels = checkBoxList.getElementsByTagName("label");
        var s = "";
        var select = $('input[type="checkbox"]', checkBoxList.parentElement.previousElementSibling)[0];
        var checkedElements = 0, totalElements = options.length;

        for (i = 0; i < options.length; i++) {
            var opt = options[i];
            if (opt.checked) {
                s += (s == "") ? "" : ",";
                s += arrayOfCheckBoxLabels[i].innerHTML;
                checkedElements++;
            }
        }
        //        if (s.length > 0) {
        //            s = s.substring(2, s.length);
        //        }
        $get(checkBoxList.id.split('_')[0] + '_' + checkBoxList.id.split('_')[1] + '_' + checkBoxList.id.split('_')[2] + '_txtCombo').value = s;
        if (s == "") {
            $get(checkBoxList.id.split('_')[0] + '_' + checkBoxList.id.split('_')[1] + '_' + checkBoxList.id.split('_')[2] + '_txtCombo').value = "Please Select " + checkBoxList.id.split('_')[2]
        }
        $get(checkBoxList.id.split('_')[0] + '_' + checkBoxList.id.split('_')[1] + '_' + checkBoxList.id.split('_')[2] + '_hidVal').value = s;

        if (!cIsFromHdr) {
            select.checked = totalElements > 0 && checkedElements === totalElements;
            select.indeterminate = !select.checked && checkedElements > 0 ? true : false;
        }
    }
    function SelectChkList(e) {
        var elements = e.parentNode.parentNode.getElementsByTagName('input');
        for (var i = 0; i < elements.length; i++) {
            elements[i].checked = e.checked;
            CheckItem($get(e.id.split('_')[0] + '_' + e.id.split('_')[1] + '_' + e.id.split('_')[2] + '_chkList'), true)
        }
    }
    //    $('#' + $('table[id*="chkList"]')[0].id.split('_')[0] + '_' + $('table[id*="chkList"]')[0].id.split('_')[1] + '_' + $('table[id*="chkList"]')[0].id.split('_')[2] + '_txtCombo' ).click(function(){
    //        var checkBoxList=$('table[id*="chkList"]')[0];
    //        $('#txtCombo').val($get(checkBoxList.id.split('_')[0] + '_' + checkBoxList.id.split('_')[1] + '_' + checkBoxList.id.split('_')[2] + '_hidVal').value);
    //    });
    function txtFilterOnChange(objtxtFilter) {
        var totalelements = $(objtxtFilter).parent().next().next().next().find('table').find('label');
        for (var i = 0; i < totalelements.length; i++) {
            if (totalelements[i].innerHTML.toUpperCase().indexOf($(objtxtFilter).val().toUpperCase()) == -1) {
                $(totalelements[i]).parent().parent().css("display", "none");
            }
            else {
                $(totalelements[i]).parent().parent().css("display", "");
            }
            if ($(objtxtFilter).parent().next().next().next().find('div').find('td').not('[style]').find('input[type="checkbox"]:checked').length > 0) {
                if ($(objtxtFilter).parent().next().next().next().find('div').find('td').not('[style]').find('input[type="checkbox"]:checked').length == $(objtxtFilter).parent().next().next().next().find('div').find('td').not('[style]').find('input[type="checkbox"]').length) {
                    $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').get(0).indeterminate = false
                    $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').attr('checked', true);
                }
                else {
                    $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').get(0).checked = false;
                    $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').get(0).indeterminate = true;
                }
            }
            else {
                $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').get(0).checked = false;
                $(objtxtFilter).parent().next().next().next().find('span').find('input[type="checkbox"]').get(0).indeterminate = false;
            }
        }
    }
</script>

<asp:TextBox ID="txtCombo" CssClass="textbox" runat="server" ReadOnly="false" Font-Size="X-Small"
    Enabled="true" OnKeyUp="txtFilterOnChange(this);"></asp:TextBox>
<%--<asp:DropDownList ID="txtCombo" runat="server" Width="250">
</asp:DropDownList>--%>
<cc1:TextBoxWatermarkExtender ID="txtWaterMark" runat="server" TargetControlID="txtCombo"
    WatermarkText="  ">
</cc1:TextBoxWatermarkExtender>
<cc1:DropDownExtender ID="extender" runat="server" DropDownControlID="Panel111" TargetControlID="txtCombo"
    Enabled="false">
</cc1:DropDownExtender>
<cc1:PopupControlExtender ID="PopupControlExtender111" runat="server" TargetControlID="txtCombo"
    PopupControlID="Panel111" Position="Bottom">
</cc1:PopupControlExtender>
<input type="hidden" name="hidVal" id="hidVal" runat="server" />
<asp:Panel ID="Panel111" runat="server" ScrollBars="Vertical" Style="max-height: 150px;"
    BackColor="AliceBlue" BorderColor="Gray" BorderWidth="1" align="left">
    <asp:CheckBox ID="cbList" runat="server" Text="Select All/None" CssClass="checkBox"
        Style="display: block; line-height: 13px; vertical-align: top; border-bottom: none;"
        OnClick="SelectChkList(this);" Visible="false" />
    <div style="border: gray thin solid; overflow-x: hidden; overflow-y: auto; max-height: 150;"
        align="left" id="DivChk">
        <asp:CheckBoxList ID="chkList" runat="server" Style="max-height: 150px;" onclick="CheckItem(this)">
        </asp:CheckBoxList>
    </div>
</asp:Panel>