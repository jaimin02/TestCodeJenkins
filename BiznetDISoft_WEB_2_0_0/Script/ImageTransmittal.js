'use strict';
class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = 
            {  value: '',
                ProjectOpt:[{title:'',Prefix:''}],
                HProjectId:'',
                HProjeName:'',
                HStudyNo:'',
                HParentWorkSpaceId:'',
                Visitddl:[],
                Subjectddl:[],
                UserId:'',
                Modalityddl:[],
                Anatomyddl:[],
                rows: []
            };
        this.bytesToSize = this.bytesToSize.bind(this);
        this.onKeyDownHandler = this.onKeyDownHandler.bind(this);
        this.FillGridData = this.FillGridData.bind(this);
        this.ProjectClientShowing = this.ProjectClientShowing.bind(this);
        this.ProjectOnItemSelected = this.ProjectOnItemSelected.bind(this);
        this.bindSubject = this.bindSubject.bind(this);
        this.bindVisit = this.bindVisit.bind(this);
        this.bindModality=this.bindModality.bind(this);
        this.bindAnatomy=this.bindAnatomy.bind(this);
        this.handleVisitChange = this.handleVisitChange.bind(this);
        this.saveData = this.saveData.bind(this);
        this.initVariable = this.initVariable.bind(this);
        this.fuButton = this.fuButton.bind(this);
        this.fileUploadFn = this.fileUploadFn.bind(this);
        this.checkDate = this.checkDate.bind(this);
        this.SaveDataAPI = this.SaveDataAPI.bind(this);
        this.ValidationForAuthentication = this.ValidationForAuthentication.bind(this);
        this.SignAuthModalClose = this.SignAuthModalClose.bind(this);
        this.FileUploadDataAPI = this.FileUploadDataAPI.bind(this);
        this.ProgressBar = this.ProgressBar.bind(this);
        this.parseFile = this.parseFile.bind(this);
        this.processFileData = this.processFileData.bind(this);
        this.GetVisitIsReviewedGetMAXiImageTranNoAPI = this.GetVisitIsReviewedGetMAXiImageTranNoAPI.bind(this);
        //this.CopyFile = this.CopyFile.bind(this);
        this.setProject = this.setProject.bind(this);
        this.componentDidMount = this.componentDidMount.bind(this);
        this.cancleData = this.cancleData.bind(this);
        this.onClearFn = this.onClearFn.bind(this);
        this.handleModalityChange = this.handleModalityChange.bind(this);
        this.VisitValidation = this.VisitValidation.bind(this);
        this.DicomTagChanges=this.DicomTagChanges.bind(this);
        this.ChangeRadioButton = this.ChangeRadioButton.bind(this);
        this.ChangeRadioButton1 = this.ChangeRadioButton1.bind(this);
        this.DomCreate = this.DomCreate.bind(this);
        this.RemoveDom = this.RemoveDom.bind(this);
        this.removeSpecialCharacter = this.removeSpecialCharacter.bind(this);
        this.handleVisitSet = this.handleVisitSet.bind(this);
        this.sendEmail = this.sendEmail.bind(this);
        
    }
    componentDidMount = () =>
        { 
            var queryStr = window.location.href; 
            var arrstrvalue = queryStr.split('?');
            if(arrstrvalue.length > 1)
            {
                var paramPairs = arrstrvalue[1].split('&');
                var values =[];
                for(var i=0;i< paramPairs.length;i++)
                {
                    var pera = paramPairs[i].split('=');
                    values.push({title:pera[0],Prefix:pera[1]})
                }
   
                this.ProjectName = values[0].Prefix.split('%20').join(' ');
                document.getElementById('HProjectId').value = values[1].Prefix;
                document.getElementById('HSubjectId').value = values[2].Prefix;
                this.HProjectIdURL =values[1].Prefix ;	
                this.SubjectIdURL =values[2].Prefix;
                this.iNodeIdURL = values[3].Prefix;
                this.bindSubject(this.HProjectIdURL);
                this.setProject(this.HProjectIdURL);
            }

        debugger;
        document.getElementById('lblImageTransmittal').innerHTML = "Image Transmittal";
        document.getElementById('lblImageTransmittalDetails').innerHTML = "Image Transmittal Details";
        this.filesize=0;
        this.rowCount=0;
        
        var dtToday = new Date(); 
        var maxMonth = dtToday.getMonth() + 1;
        var minMonth = dtToday.getMonth() - 0;
        var day = dtToday.getDate();
        var maxyear = dtToday.getFullYear();
        var minyear = dtToday.getFullYear() - 5;
        if(maxMonth < 10)
            maxMonth = '0' + maxMonth.toString();
        if(minMonth == '0')
        {
            minyear = dtToday.getFullYear() - 5;
            minMonth ='07';
        }
        if(minMonth < 10)
            minMonth = '0' + minMonth.toString();
        if(day < 10)
            day = '0' + day.toString();
        var maxDate = maxyear + '-' + maxMonth + '-' + day;
        var MinDate = minyear + '-' + minMonth + '-' + day;
        $('#ExaminationDate').attr('max', maxDate);
        $('#ExaminationDate').attr('min', MinDate);
        $('#DOB').attr('max', maxDate);
        //document.getElementById("ExaminationDate").value = new Date().format("yyyy-MM-dd");
        $("form").keypress(function(e) {
            //Enter key
            if (e.which == 13) {
                return false;
            }
        });
        //document.getElementById("randomizedNo").checked == true
        var bydefault = document.getElementsByName("randomized")
        bydefault[0].checked = true;
        
        this.ChangeRadioButton();
        this.ChangeRadioButton1();
    }

//Hide and show the tab
Display = (control, divName)  =>
{
    if (control.target.src.toString().toUpperCase().search("EXPAND") != -1) {
        $("#" + divName).slideToggle(600);
        control.target.src = "images/panelcollapse.png";
    }
    else {
        $("#" + divName).slideToggle(600);
        control.target.src = "images/panelexpand.png";
    }
}

onKeyDownHandler = e => 
{
    //if(e.target.value.length >= 3)
    {
        const data = { prefixText: e.target.value, count: 10, contextKey : ""}, me = this;
        axios({
            method: 'Post',
            url: 'frmImageTransmittal.aspx/GetMyChildProjectCompletionList',
            data: data
        }).then(
            function (response) {
            // handle success
            if(response.data) {
                me.ProjectClientShowing(response.data.d);
            }                         
        }).catch(
            function (error) {
                // handle error
                console.log(error);
        });
    }
}

ProjectClientShowing =(textBox) =>
{
    var items = textBox;
    var  tempArray =[{title:'',Prefix:''}];
    var searchText = this.state.value;
    for (var i = 0; i < items.length; i++) 
    {
        var value = items[i];
        var startIndex;
        var len;
        var strValue;
        var tempValue;
        startIndex = value.toUpperCase().indexOf(searchText.toUpperCase());
        len = searchText.length;
        strValue = value.substring(0, startIndex);
        strValue = strValue + value.substring(startIndex, parseInt(strValue.length) + parseInt(len));
        tempValue = strValue;
        tempValue = tempValue.replace('<span style="color:red"><b>', '');
        tempValue = tempValue.replace('</b></span>', '');
        strValue += value.substring(tempValue.length);
        strValue = strValue.replace('\'', '');
        var arrstrvalue = strValue.split('#');
        if (arrstrvalue.length == 3)
        {
            tempArray.push({title:''+'[' + arrstrvalue[1] + ']' + arrstrvalue[2],Prefix:value});
        }
        else if (arrstrvalue.length == 4)
        {
            tempArray.push({title: arrstrvalue[1] + arrstrvalue[2],Prefix:value});
        }

        else if (arrstrvalue.length == 5) 
        {
            tempArray.push({title:arrstrvalue[1] + arrstrvalue[2] + '-' + arrstrvalue[3] + '-' + arrstrvalue[4],Prefix:value});
        }
        else if (arrstrvalue.length > 5)
        {
            tempArray.push({title:'[' + arrstrvalue[1] + ']' + arrstrvalue[2] + '-'+ arrstrvalue[3] + '-' + arrstrvalue[4] + '[' + arrstrvalue[5] + ']',Prefix:value});
        }
    }
    this.setState({ProjectOpt:tempArray})
}

                                                                                              
//Create a function to call Web API to set 2 hidden filed ,Project Name, Projectwork spaceId   //                                                                                                 //
ProjectOnItemSelected = (perfix) =>
{
    this.MaxImageiTranNo="0";
    var strvalue = perfix;
    strvalue = strvalue.replace('\'', '');
    var arrStrValue = strvalue.split('#');
    if (arrStrValue.length == 3) {
        // txt.value = '[' + arrStrValue[1] + '] ' + arrStrValue[2];
        this.setState({ HProjectId: arrStrValue[0] })
        document.getElementById('HProjectId').value = arrStrValue[0];
        this.setProject(arrStrValue[0]);
    }
} //End of  ProjectOnItemSelected function

//set the set 2 hidden filed ,Project Name, Projectwork spaceId
setProject = (HProjectId) =>
{
    const data = { HProjectId:HProjectId},me=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/btnSetProject',
        data: data
    }).
    then(
        function (response) {// handle success  
            var arrstrvalue =response.data.d.split('#')
            document.getElementById('HProjeName').value = eval(arrstrvalue[0])[0].vProjectNo;
            document.getElementById('HParentWorkSpaceId').value = eval(arrstrvalue[0])[0].vParentWorkspaceId;
            document.getElementById('HExternalProjectNo').value = eval(arrstrvalue[0])[0].vExternalProjectNo;
            document.getElementById('HUserId').value = arrstrvalue[2];
            document.getElementById('HStudyNo').value = eval(arrstrvalue[0])[0].vStudyNo;
            document.getElementById('lblSignAuthUserName').innerHTML = "<label >"+arrstrvalue[1]+"</label>";
            document.getElementById('lblSignAuthDateTime').innerHTML = "<label >"+ new Date().format('dd-MMM-yyyy HH:mm:ss tt')+"</label>";
            
            me.bindSubject(HProjectId);
            
        })
    .catch(
        function (error) {
            // handle error
            console.log(error);
        });
} // End of setProject function

//bind Subject 
bindSubject =(HProjectId) =>
{
    debugger;
    this.MaxImageiTranNo="0"
    const FillSubjectDropDown ={ HProjectId:HProjectId},me=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/FillSubjectDropDown',
        data: FillSubjectDropDown
    }).
    then(
    function (response) {// handle success  
        me.setState({Subjectddl:eval(response.data.d)}) 
        var  SubjectSelect=  document.getElementById("ddlSubject");
        document.getElementById("ddlVisitNo").options.length = 0;
        document.getElementById("ddlSubject").options.length = 0;
        me.myOption = document.createElement("option");
        me.myOption.text ="Select Subject";
        me.myOption.value = "0";
        SubjectSelect.appendChild(me.myOption);
        for(var i=0;i < eval(response.data.d).length; i++)
        {
            me.myOption = document.createElement("option");
            me.myOption.text =eval(response.data.d)[i].vRandomizationNo =="" ? eval(response.data.d)[i].vInitials +'/'+ eval(response.data.d)[i].vMySubjectNo : eval(response.data.d)[i].vRandomizationNo +'/'+ eval(response.data.d)[i].vMySubjectNo;
            me.myOption.value = eval(response.data.d)[i].vSubjectId;
            SubjectSelect.appendChild(me.myOption);
        }
        if(me.SubjectIdURL !="" && me.SubjectIdURL !=null)
        {
            document.getElementById("ddlSubject").value = me.SubjectIdURL;
            document.getElementById("ddlSubject").disabled = true;
            const result =  eval(response.data.d).filter(model => model.vSubjectId == me.SubjectIdURL);
            document.getElementById('HSubjectName').value = result[0].vInitials;
            document.getElementById('HSubjectId').value = result[0].vSubjectId;
            document.getElementById('HSubjectNo').value = result[0].vMySubjectNo;
            document.getElementById('HRandomizationNo').value = result[0].vRandomizationNo;

            me.bindVisit(HProjectId);
        }
        me.FillGridData();
    })
    .catch(
    function (error) {
        // handle error
        console.log(error);
    });

} // End of bindSubject function

//Fill the visit Drop-down list
bindVisit = (HProjectId) =>
{
    this.MaxImageiTranNo="0"
    this.MarkNodeId = 0
    const FillVisitDropDown ={ HProjectId: HProjectId,SubjectId : HSubjectId.value},mee=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/FillVisitDropDown',
        data: FillVisitDropDown
    }).
    then(
    function (response) {// handle success    
        mee.setState({Visitddl:eval(response.data.d)})
        const resultSubject =  mee.state.Subjectddl.filter(model => model.vSubjectId == ddlSubject.value);
        document.getElementById("ddlVisitNo").options.length = 0;
        var SubjectVisit=  document.getElementById("ddlVisitNo");
        if(resultSubject.length > 0){
            document.getElementById("DOB").value = resultSubject[0].dBirthDate1;
        }
        mee.myOption = document.createElement("option")
        mee.myOption.text ="Select Visit";
        mee.myOption.value = "0";
        SubjectVisit.appendChild(mee.myOption);
        mee.bindAnatomy();
        mee.bindModality();
        for(var i=0;i < eval(response.data.d).length; i++)
        {
            mee.myOption = document.createElement("option");
            mee.myOption.text =eval(response.data.d)[i].vNodeDisplayName;
            mee.myOption.value = eval(response.data.d)[i].iNodeId;
            SubjectVisit.appendChild(mee.myOption);
        }
        if(mee.iNodeIdURL !="" && mee.iNodeIdURL !=null)
        {
            var arrstrvalue = mee.iNodeIdURL.split('#');
            // mee.handleVisitChange(arrstrvalue[0]);
            document.getElementById("ddlVisitNo").value =arrstrvalue[0] ;
            document.getElementById('HNodeId').value = arrstrvalue[0];
           
            document.getElementById('HParentNodeId').value = result[0].iParentNodeId;
            document.getElementById('HActivityId').value = result[0].vActivityId;
            document.getElementById('HParentActivityId').value = result[0].vParentActivityId;
            document.getElementById('HVisit').value = result[0].vNodeDisplayName;
            mee.handleVisitSet(result[0].vNodeDisplayName);
            mee.tabsValidation(result[0].vNodeDisplayName);
        }
        document.getElementById('lblImageTransmittal').innerHTML = "Image Transmittal";
        document.getElementById('lblImageTransmittalDetails').innerHTML = "Image Transmittal Details";
       
        for(var i=0;i < eval(response.data.d).length; i++)
        {
            if(eval(response.data.d)[i].vNodeDisplayName == 'MARK'){
                mee.MarkNodeId = eval(response.data.d)[i].iNodeId;
                mee.MarkiParentNodeId = eval(response.data.d)[i].iParentNodeId;
                mee.MarkvActivityId = eval(response.data.d)[i].vActivityId;
                mee.MarkvParentActivityId = eval(response.data.d)[i].vParentActivityId;
                continue;
            }
            else if(eval(response.data.d)[i].vNodeDisplayName == 'GLOBAL RESPONSE' || eval(response.data.d)[i].vNodeDisplayName == 'ADJUDICATOR'){
                continue;
            }
            else{
                mee.myOption = document.createElement("option");
                mee.myOption.text =eval(response.data.d)[i].vNodeDisplayName;
                mee.myOption.value = eval(response.data.d)[i].iNodeId;
                SubjectVisit.appendChild(mee.myOption);
                document.getElementById('HNodeId').value = eval(response.data.d)[i].iNodeId;
                document.getElementById('HParentNodeId').value = eval(response.data.d)[i].iParentNodeId;
                document.getElementById('HActivityId').value = eval(response.data.d)[i].vActivityId;
                document.getElementById('HParentActivityId').value = eval(response.data.d)[i].vParentActivityId;
                document.getElementById('HVisit').value = eval(response.data.d)[i].vNodeDisplayName;
            }
        }
        document.getElementById('lblImageTransmittal').innerHTML = "Image Transmittal";
        document.getElementById('lblImageTransmittalDetails').innerHTML = "Image Transmittal Details";

        mee.bindAnatomy();
        mee.bindModality();
    })
    .catch(
    function (error) {
        // handle error
        console.log(error);
    });

} // End of bindVisit function

//Fill the modality Drop-down list
bindModality = () =>
{
    const me=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/FillModalityDropDown',
        data: {}
    }).
    then(
    function (response) {// handle success   
        debugger;
        me.setState({Modalityddl:eval(response.data.d)})
        if(me.rowCount > 0){
            document.getElementById(`ddlModality`+me.rowCount).options.length = 0;
            var SubjectModality=  document.getElementById(`ddlModality`+me.rowCount);
        }else{
            document.getElementById("ddlModality").options.length = 0;
            var SubjectModality=  document.getElementById("ddlModality");
        }
        
        
        me.myOption = document.createElement("option")
        me.myOption.text ="Select Modality";
        me.myOption.value = "0";
        SubjectModality.appendChild(me.myOption);
        for(var i=0;i < eval(response.data.d).length; i++)
        {
            me.myOption = document.createElement("option");
            me.myOption.text =eval(response.data.d)[i].vModalityDesc;
            me.myOption.value = eval(response.data.d)[i].nModalityNo;
            SubjectModality.appendChild(me.myOption);
            
        }
        //var SubjectModality=  document.getElementById("ddlModality");
        //var ModalityTemplate="<ul><li><input type='checkbox' id='SelectAllModalities' onClick='SelectAllModality(" + eval(response.data.d).length + "," + response.data.d + "," + 0 + "," + 1 + ")' name='Modality'>Select All</li><br>";
        
        //for(var i=0 ; i < eval(response.data.d).length ; i++)
        //{
        //    ModalityTemplate += "<li><input type='checkbox' id='" + eval(response.data.d)[i].nModalityNo + "' onClick='SelectAllModality(" + eval(response.data.d).length + "," + response.data.d + "," + eval(response.data.d)[i].nModalityNo + "," + 0 + ")' name='Modality' />" + eval(response.data.d)[i].vModalityDesc + "</li><br>";
        //}
        //ModalityTemplate += "</ul>";
        //SubjectModality.innerHTML=ModalityTemplate;
        //document.getElementById('lblImageTransmittal').innerHTML = "Image Transmittal";
        //document.getElementById('lblImageTransmittalDetails').innerHTML = "Image Transmittal Details";
    })
    .catch(
    function (error) {
        // handle error
        console.log(error);
    });
} // End of bindModality function 

//Fill the anatomy Drop-down list
bindAnatomy = () =>
{
    const me=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/FillAnatomyDropDown',
        data: {}
    }).
    then(
    function (response) {// handle success  
        me.setState({Anatomyddl:eval(response.data.d)})
        var SubjectAnatomy = "";
        if(me.rowCount > 0){
            SubjectAnatomy=  document.getElementById(`ddlAnatomy`+me.rowCount);
        }else{
            SubjectAnatomy=  document.getElementById("ddlAnatomy");
        }

        //var SubjectAnatomy=  document.getElementById("ddlAnatomy");
        var AnatomyTemplate="<ul><li><input type='checkbox' id='SelectAll_"+ me.rowCount +"' onClick='SelectAllAnatomy(" + eval(response.data.d).length + "," + response.data.d + ",this," + 1 + "," + me.rowCount + ")' name='Anatomy'>Select All</li><br>";
        
        for(var i=0 ; i < eval(response.data.d).length ; i++)
        {
            if(eval(response.data.d)[i].vAnatomyDesc.toUpperCase().match("OTHER")){
                AnatomyTemplate += "<li><input type='checkbox' id='" + eval(response.data.d)[i].nAnatomyNo + "_" + me.rowCount + "' onClick='SelectAllAnatomy(" + eval(response.data.d).length + "," + response.data.d + ",this," + 0 + "," + me.rowCount + ")' name='"+ eval(response.data.d)[i].vAnatomyDesc +"' />" + eval(response.data.d)[i].vAnatomyDesc + "</li><br>";
            }
            else{
                AnatomyTemplate += "<li><input type='checkbox' id='" + eval(response.data.d)[i].nAnatomyNo + "_" + me.rowCount + "' onClick='SelectAllAnatomy(" + eval(response.data.d).length + "," + response.data.d + ",this," + 0 + "," + me.rowCount + ")' name='"+ eval(response.data.d)[i].vAnatomyDesc +"' />" + eval(response.data.d)[i].vAnatomyDesc + "</li><br>";
            }
        }
        AnatomyTemplate += "</ul>";
        SubjectAnatomy.innerHTML=AnatomyTemplate;
        //document.getElementById('lblImageTransmittal').innerHTML = "Image Transmittal";
        //document.getElementById('lblImageTransmittalDetails').innerHTML = "Image Transmittal Details";
    })
    .catch(
    function (error) {
        // handle error
        console.log(error);
    });
} // End of bindAnatomy function

//filter the Modality the 
handleModalityChange = (ModalityId, rowNum) =>{
    debugger;
    const result =  this.state.Modalityddl.filter(model => model.nModalityNo == ModalityId);
    //$("#dynamicTxt").empty();
    //if(rowNum > 1){
    //for (var i = 0 ; i < length ; i++) {
        
            if (result[0].vModalityDesc.toUpperCase().match("OTHER")) {
                if(rowNum > 1){
                    $("#OtherModality" + rowNum).removeAttr("disabled");
                    $("#OtherModality" + rowNum).focus();
                }else{
                    $("#OtherModality").removeAttr("disabled");
                    $("#OtherModality").focus();
                }
                     
            }else{
                if(rowNum > 1){
                    $("#OtherModality" + rowNum).attr("disabled", "disabled");
                    $("#OtherModality" + rowNum).val('');
                  }else{
                    $("#OtherModality").attr("disabled", "disabled");
                    $("#OtherModality").val('');
                }
           }
    
    this.VisitValidation();
} // End of handleModalityChange function

//filter the visit the 
handleVisitChange = (VisitId) =>{
    this.MaxImageiTranNo="0";
    const result =  this.state.Visitddl.filter(model => model.iNodeId == VisitId);
    document.getElementById('HNodeId').value = result[0].iNodeId;
    document.getElementById('HParentNodeId').value = result[0].iParentNodeId;
    document.getElementById('HActivityId').value = result[0].vActivityId;
    document.getElementById('HParentActivityId').value = result[0].vParentActivityId;
    document.getElementById('HVisit').value = result[0].vNodeDisplayName;
    this.handleVisitSet(result[0].vNodeDisplayName);
    var randomizedd = document.getElementsByName("randomized")
    if(result[0].vNodeDisplayName == "BL"){
        randomizedd[0].checked = true;
        randomizedd[0].disabled = true;
        randomizedd[1].disabled = true;
        document.getElementById("randomizationNo").disabled = true;
        document.getElementById('randomizationNo').value  = '';
    }
    else {
        if(!document.getElementById('HRandomizationNo').value){
            randomizedd[0].checked = true;
            randomizedd[0].disabled = false;
            randomizedd[1].disabled = false;
            document.getElementById('randomizationNo').value  = '';
        }else{
            randomizedd[1].checked = true;
            randomizedd[0].disabled = true;
            randomizedd[1].disabled = true;
            document.getElementById("randomizationNo").disabled = true;
            document.getElementById('randomizationNo').value  = document.getElementById('HRandomizationNo').value;
        }
    }
} // End of handleVisitChange function

//Filter the subject and stored the data in hiden filed's
HandleSubjectChange = (SubjectId) =>{
    debugger;
    this.MaxImageiTranNo="0";
    const result =  this.state.Subjectddl.filter(model => model.vSubjectId == SubjectId);
    document.getElementById('HSubjectName').value = result[0].vInitials;
    document.getElementById('HSubjectId').value = result[0].vSubjectId;
    document.getElementById('HSubjectNo').value = result[0].vMySubjectNo;
    document.getElementById('HRandomizationNo').value = result[0].vRandomizationNo;
    document.getElementById('HDob').value = result[0].dBirthDate1;

    if(result[0].dBirthDate1 != ""){
        document.getElementById('DOB').value = result[0].dBirthDate1;
    }

    var randomized = document.getElementsByName("randomized")
    var crandomized = -1
    if(result[0].vRandomizationNo != "" ){
        randomized[1].checked = true;
        randomized[0].disabled = true;
        randomized[1].disabled = true;
        document.getElementById("randomizationNo").disabled = true;
        document.getElementById('randomizationNo').value  = result[0].vRandomizationNo;
    }
    else{
        randomized[0].checked = true;
        randomized[0].disabled = false;
        randomized[1].disabled = false;
        document.getElementById('randomizationNo').value  = '';
    }

    this.bindVisit(HProjectId.value);
    this.FillGridData();

} // End of HandleSubjectChange function


//Create a Function for stored to particule Name wise
initVariable = ()=> 
{
    this.IsUpload=false;
    this.HProjectId=document.getElementById('HProjectId');
    this.HProjeName = document.getElementById('HProjeName');
    this.HParentWorkSpaceId = document.getElementById('HParentWorkSpaceId');
    this.HExternalProjectNo = document.getElementById('HExternalProjectNo');
    this.HUserId = document.getElementById('HUserId');
    this.HParentActivityId = document.getElementById('HParentActivityId');
    this.HParentNodeId=document.getElementById("HParentNodeId");
    this.HActivityId=document.getElementById("HActivityId");
    this.ddlSubject = document.getElementById('ddlSubject');
    this.ddlVisitNo = document.getElementById('ddlVisitNo');
    this.DOB = document.getElementById('DOB');
    this.contrastYes=document.getElementById('contrastYes');
    this.contrastNo=document.getElementById('contrastNo');
    this.contrastNA=document.getElementById('contrastNA');
    this.OralcontrastYes=document.getElementById('OralcontrastYes');
    this.OralcontrastNo=document.getElementById('OralcontrastNo');
    this.OralcontrastNA=document.getElementById('OralcontrastNA');
    this.daviationNo=document.getElementById('daviationNo');
    this.daviationYes=document.getElementById('daviationYes');
    this.ddlModality=document.getElementById('ddlModality');
    this.OtherModality=document.getElementById("OtherModality");
    this.ddlAnatomy=document.getElementById('ddlAnatomy');
    this.OtherAnatomy=document.getElementById("OtherAnatomy");
    this.txtInstruction=document.getElementById("txtInstruction");
    this.txtDaviation=document.getElementById("txtDaviation");
    this.ExaminationDate=document.getElementById("ExaminationDate");
    this.randomizedNo = document.getElementById("randomizedNo");
    this.randomizedYes = document.getElementById("randomizedYes");
    this.randomizationNumber = document.getElementById("randomizationNo");
    this.DicomTagChangeYes = document.getElementById("DicomTagChangeYes");
    this.DicomTagChangeNo = document.getElementById("DicomTagChangeNo");
    this.fileUpload = {files:[]};
    this.FileListSaveData=[];
    this.FileListSaveDataMark=[];
    this.FileList=[];
    this.FileCountSuccess = 0;
    this.const_Count = 1;
    this.flagCount = this.const_Count;
    this.SrNo = 0;
    this.AllFileUpload = [];
    this.ImageSeq = 0;
} // End of initVariable function

//byte size convertion 
bytesToSize =(bytes) =>{
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return '0 Byte';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
} // End of bytesToSize function

//Create a OnClick to Save the data in DataBase.
saveData = () => {
    if (!this.validateData()) {
        return false;
    }
    document.getElementById('myModalSignAuth').style.display  = "block";
    return false;
} // End of saveData function

// SignAuthModalClose for close model box
SignAuthModalClose = () => {
    document.getElementById('myModalSignAuth').style.display = 'none';
    document.getElementById('myModalProgress').style.display = 'none';
    document.getElementById('AuditTrailDiv').style.display = 'none';
} // End of SignAuthModalClose function

QSignAuthModalClose = () => {
    document.getElementById('myModalSignAuth').style.display = 'none';
    document.getElementById('myModalProgress').style.display = 'none';
    document.getElementById('EditQueryDiv').style.display = 'none';
} 
//Create a Function for validation to check the filed's
validateData = () => 
{   debugger;
    this.initVariable();
    var fileData = document.getElementById('fileUpload');
    var strAnatomy = document.getElementById('ddlAnatomy').getElementsByTagName('input');
    
    var countAnatomy = 0;
    var countModality =0;
    for(var h = 0 ; h < strAnatomy.length ; h++){
        if(!strAnatomy[h].checked){
            countAnatomy++;
        }
    }
    //for(var i = 0 ; i < strModality.length ; i++){
    //    if(!strModality[i].checked){
    //        countModality++;
    //    }
    //}
    if(HProjectId.value==""){
        HProjectId.focus();
        alert("Please select site no!");
        return false;
    }
    else if(ddlSubject.value == "" || ddlSubject.value == "0"){
        alert("Please select subject!");
        return false;
    }
    else if(ddlVisitNo.value == "" || ddlVisitNo.value == "0"){
        alert("Please select visit no!");
        return false;
    }
    else if(daviationYes.checked == true && txtDaviation.value == ""){
        txtDaviation.focus();
        alert("Please enter Daviation!");
        return false;
    }
    else if(randomizedYes.checked == true && this.randomizationNumber.value == ""){
        randomizationNumber.focus();
        alert("Please add randomization number!")
        return false
    }
    else if (ExaminationDate.value == "" || ExaminationDate.value == null) {
        alert("Please enter Examination Date!");
        return false;
    }
    else if(countAnatomy == strAnatomy.length){
        alert("Please select Anatomy 1!")
        return false;
    }
    else if(ddlModality.value == "0"){
        alert("Please select modality 1!");
        return false;
    }
    else if (fileData.files.length == 0) {
        alert("Please first select the files for Modality 1!");
        return false;
    }
    else if(!contrastYes.checked && !contrastNo.checked && !contrastNA.checked){
        alert("Please Select IV Contrast!");
        return false;
    }
    else if(!OralcontrastYes.checked && !OralcontrastNo.checked && !OralcontrastNA.checked ){
        alert("Please Select Oral Contrast!");
        return false;
    }
    
    if(ddlVisitNo.value != "" || ddlVisitNo.value != "0")
    {
        var VisitPara1 = document.getElementById('HVisit').value
        this.handleVisitSet(VisitPara1);
        if(this.setFlag == 1)
        {
            return false;
        }
    }
    if(this.rowCount > 1){
        debugger;
        for(var o=2;o<=this.rowCount;o++){
            var strAnatomyy = document.getElementById('ddlAnatomy'+o).getElementsByTagName('input');
            var CountAnatomies = 0;
            var fileData1 = document.getElementById(`fileUpload`+o);
            
            //----Anatomy validation
            for(var dd = 0 ; dd < strAnatomyy.length ; dd++){
                if(!strAnatomyy[dd].checked){
                    CountAnatomies++;
                }
            }
            if(CountAnatomies == strAnatomyy.length){
                alert("Please select Anatomy "+ o +"!")
                return false;
            }

            //------Modality validation
            if(document.getElementById(`ddlModality`+o).value == "0"){
                alert("Please select modality "+ o +" !");
                return false;
            }
            if (fileData1.files.length == 0) {
                alert("Please first select the files for Modality "+ o +" !");
                return false;
            }
        }
    }

    document.getElementById('lblSignAuthDateTime').innerHTML = "<label >"+ new Date().format('dd-MMM-yyyy HH:mm:ss tt') +"</label>";
    this.strAnatomyNo = "";
    this.strModalityNo = "";
    var fileUpload = document.getElementById('fileUpload');
    this.filesize = fileUpload.files.length;
    this.iModalityNo="";
    var FilenameAnatomyWise = "";
    this.FolderNameModalityWise = "";
    this.strVisitWithMark = "";
    this.OtherComment="";
    this.SelectedModalities = "";
    this.SelectedAnatomies = "";

    //-----------Modality coma saperated----------- 
    if(ddlModality.value != "0"){
        this.strModalityNo = document.getElementById('ddlModality').value;
        this.FolderNameModalityWise = document.getElementById('ddlModality').value;
        if(ddlModality.options[ddlModality.selectedIndex].outerText.toUpperCase().match("OTHER")){
            this.SelectedModalities = document.getElementById("OtherModality").value;
        }else{
            this.SelectedModalities = ddlModality.options[ddlModality.selectedIndex].outerText;
        }
        //if(document.getElementById('ddlModality').value == "18"){
        //    this.OtherComment = document.getElementById('OtherModality1').value
        //}
        if(this.rowCount > 1){
            for(var l=2;l<=this.rowCount;l++){
                if(document.getElementById("ddlModality"+l).options[document.getElementById("ddlModality"+l).selectedIndex].outerText.toUpperCase().match("OTHER")){
                    this.SelectedModalities1 = document.getElementById("OtherModality" + this.rowCount).value;
                }else{
                    this.SelectedModalities1 = document.getElementById("ddlModality"+l).options[document.getElementById("ddlModality"+l).selectedIndex].outerText
                }
                this.SelectedModalities = this.SelectedModalities + ", " + this.SelectedModalities1
                
                this.strModalityNo = this.strModalityNo + "," + document.getElementById(`ddlModality`+l).value;
                this.FolderNameModalityWise = this.FolderNameModalityWise  + "_" + document.getElementById(`ddlModality`+l).value;
            }
        }
    }
    this.GetVisitIsReviewedGetMAXiImageTranNoAPI();
    
    //-----------Anatomy coma saperated----------- 
    //strAnatomy=ddlAnatomy.value;
    if(strAnatomy != null || strAnatomy.length != 0){
        for( var u=1 ; u < strAnatomy.length ; u++ ){
            if(this.strAnatomyNo=="" || this.strAnatomyNo==null)
            {
                if(strAnatomy[u].checked){
                    this.strAnatomyNo = strAnatomy[u].id.split("_")[0];
                    if(strAnatomy[u].name.toUpperCase().match("OTHER")){
                        this.SelectedAnatomies = document.getElementById("OtherAnatomy").value;}
                    else{
                        this.SelectedAnatomies = document.getElementById('ddlAnatomy').getElementsByTagName('input')[u].name;
                    }
                    FilenameAnatomyWise= strAnatomy[u].id.split("_")[0];
                }
            }
            else{
                if(strAnatomy[u].checked){
                    this.strAnatomyNo = this.strAnatomyNo + "," + strAnatomy[u].id.split("_")[0];
                    if(strAnatomy[u].name.toUpperCase().match("OTHER")){
                        this.SelectedAnatomies = this.SelectedAnatomies + ", " + document.getElementById("OtherAnatomy").value;
                    }
                    else
                    {
                    this.SelectedAnatomies = this.SelectedAnatomies + ", " + document.getElementById('ddlAnatomy').getElementsByTagName('input')[u].name;
                    }
                    FilenameAnatomyWise=FilenameAnatomyWise + "_" + strAnatomy[u].id.split("_")[0];
                }
            }
        }
    }

    if(fileData.files.length >= 1){
        debugger;
        var FileExt = SupportedFileExt.split(',');
        var FilesName,now,file;
        for (var iindexx = 0; iindexx < fileData.files.length; iindexx++) {
            this.ImageSeq++;
            var SingleFile = fileData.files[iindexx];
            const name = SingleFile.name;
            const lastDot = name.lastIndexOf('.');
            const ext = name.substring(lastDot + 1);
            var FileExists = FileExt.filter(x => x == ext)
            if(FileExists.length == 0 && SingleFile.name != 'Thumbs.db')
            {
                alert(name + " Image is not valid Format.\n It's support this format .DCM");
                return false;
            }
            
            if(SingleFile.name != 'Thumbs.db')
            {
                FilesName = this.ImageSeq + name.substring(0, lastDot) + "_" + FilenameAnatomyWise +"."+ext
                fileUpload.files[iindexx].filename = FilesName 
                file = fileUpload.files[iindexx];

                this.FileList.push({
                    FileName: FilesName
                });
                this.FileListSaveData.push({
                    vFileName: FilesName,
                    vServerPath: "/" + HProjectId.value + "/" + ddlSubject.value + "/" + ddlVisitNo.value + "/" + this.FolderNameModalityWise + "/" + this.MaxImageiTranNo + "/Uploaded/" + FilesName,
                    vFileType:ext,
                    vSize: this.bytesToSize(file.size),
                    dScheduledDate:new Date().format('MM-dd-yyyy HH:mm'),
                    vAnatomyNo: this.strAnatomyNo,
                    vModalityNo: document.getElementById('ddlModality').value,
                    iImgSeqNo: this.ImageSeq
                });
                this.FromPath = "\\" + HProjectId.value + "\\" + ddlSubject.value + "\\" + ddlVisitNo.value + "\\" + this.FolderNameModalityWise + "\\" + this.MaxImageiTranNo + "\\Uploaded\\";
                this.fileUpload.files.push(SingleFile);
            }
        }
        this.AllFileUpload.push.apply(this.AllFileUpload, this.fileUpload.files);
        this.fileUpload = {files:[]};
    }

    if(this.rowCount > 1){
        debugger;
        for(var i = 2 ; i <= this.rowCount ; i++){
            //if(document.getElementById('ddlModality'+i).value == "18"){
            //    if(this.OtherComment == ""){
            //        this.OtherComment = document.getElementById('OtherModality'+i).value
            //    }else{
            //        this.OtherComment = this.OtherComment + "," + document.getElementById('OtherModality'+i).value
            //    }
            //}
            var strAnatomy = document.getElementById('ddlAnatomy'+i).getElementsByTagName('input');
            var AnatomyImageWise = "";
            if(strAnatomy != null || strAnatomy.length != 0){
                
                for( var ana=1 ; ana < strAnatomy.length ; ana++ ){
                    
                    if(this.strAnatomyNo=="" || this.strAnatomyNo==null)
                    {
                        if(strAnatomy[ana].checked){
                            this.strAnatomyNo = strAnatomy[ana].id.split("_")[0];
                            this.SelectedAnatomies = document.getElementById('ddlAnatomy').getElementsByTagName('input')[ana].name;
                            AnatomyImageWise = strAnatomy[ana].id.split("_")[0];
                            FilenameAnatomyWise= strAnatomy[ana].id.split("_")[0];
                        }
                    }
                    else{
                        if(strAnatomy[ana].checked){
                            this.strAnatomyNo = this.strAnatomyNo + "," + strAnatomy[ana].id.split("_")[0];
                            if(strAnatomy[ana].name.toUpperCase().match("OTHER")){
                                this.SelectedAnatomies1 = document.getElementById("OtherAnatomy"+this.rowCount).value;
                            }else{
                                this.SelectedAnatomies1 = document.getElementById('ddlAnatomy').getElementsByTagName('input')[ana].name;
                            }
                            this.SelectedAnatomies = this.SelectedAnatomies + ", " + this.SelectedAnatomies1;
                            if(AnatomyImageWise != "")
                                AnatomyImageWise = AnatomyImageWise + "," + strAnatomy[ana].id.split("_")[0];
                            else
                                AnatomyImageWise = strAnatomy[ana].id.split("_")[0];
                            FilenameAnatomyWise=FilenameAnatomyWise + "_" + strAnatomy[ana].id.split("_")[0];
                        }
                    }
                }
            }


            var fileData = document.getElementById("fileUpload"+i);
            //var fileUpload = document.getElementById('fileUpload');
            this.filesize = this.filesize + fileData.files.length;
            if(fileData.files.length >= 1){
                debugger;
                var FileExt = SupportedFileExt.split(',');
                var FilesName,now,file;
                for (var iindex = 0; iindex < fileData.files.length; iindex++) {
                    this.ImageSeq++;
                    var SingleFile = fileData.files[iindex];
                    const name = SingleFile.name;
                    const lastDot = name.lastIndexOf('.');
                    const ext = name.substring(lastDot + 1);
                    var FileExists = FileExt.filter(x => x == ext)
                    if(FileExists.length == 0 && SingleFile.name != 'Thumbs.db')
                    {
                        alert(name + " Image is not valid Format.\n It's support this format .DCM,.JPG,.JPEG,.PNG,.TIF,.TIFF,.BMP,.ZIP");
                        return false;
                    }
            
                    if(SingleFile.name != 'Thumbs.db')
                    {
                        FilesName = this.ImageSeq + name.substring(0, lastDot) + "_" + FilenameAnatomyWise +"."+ext
                        fileData.files[iindex].filename = FilesName 
                        file = fileData.files[iindex];

                        this.FileList.push({
                            FileName: FilesName
                        });
                        this.FileListSaveData.push({
                            vFileName: FilesName,
                            vServerPath: "/" + HProjectId.value + "/" + ddlSubject.value + "/" + ddlVisitNo.value + "/" + this.FolderNameModalityWise + "/" + this.MaxImageiTranNo + "/Uploaded/" + FilesName,
                            vFileType:ext,
                            vSize: this.bytesToSize(file.size),
                            dScheduledDate:new Date().format('MM-dd-yyyy HH:mm'),
                            vAnatomyNo: AnatomyImageWise,
                            vModalityNo: document.getElementById('ddlModality'+i).value,
                            iImgSeqNo: this.ImageSeq
                        });
                        this.FromPath = "\\" + HProjectId.value + "\\" + ddlSubject.value + "\\" + ddlVisitNo.value + "\\" + this.FolderNameModalityWise + "\\" + this.MaxImageiTranNo + "\\Uploaded\\";
                        this.fileUpload.files.push(SingleFile);
                    }
                }
                this.AllFileUpload.push.apply(this.AllFileUpload, this.fileUpload.files);
                this.fileUpload = {files:[]};
            }
        }
    }

    return true;
} // End of validateData function


// File function for the chunck function..
processFileData = (loopCount,flagCount_1) => {
    //for(var j=0 ; j<this.AllFileUpload.Allfiles.length ; j++){
        for (var i = loopCount; i < flagCount_1; i++) 
        {
            var file = this.AllFileUpload[i];
            var LocalFilePath = "/" + HProjectId.value + "/" + ddlSubject.value + "/" + ddlVisitNo.value + "/" + this.FolderNameModalityWise + "/" + this.MaxImageiTranNo + "/Uploaded/";
            this.parseFile(file, LocalFilePath, this.FileUploadDataAPI);
        }
   // }
    
} // End of processFileData 


//  Create a Function for do the part of file usind chunk reader and pass the file save on path.
parseFile = (file, LocalFilePath, callback ) => {
    var fileSize = file.size;   
    var fileName = file.filename;
    var offset = 0, self = this; // we need a reference to the current object
    var chunkReaderBlock = null;

    var readEventHandler = function (evt) {
        if (evt.target.error == null) {
            offset += chunkSize;
            var LineLoder = offset * 100/file.size;
            callback(evt.target.result.split(",")[1], fileName, LocalFilePath,  LineLoder,0); // callback for handling read chunk
                                                    
        } else {
            console.log("Read error: " + evt.target.error);
            return;
        }
        if (offset >= fileSize) {
            //debugger;
            console.log("Done reading file");
            //if(self.ToPath != null){
            //if(DicomTagChangeYes.checked == true){
                self.DicomTagChanges(fileName, self.FromPath, HSubjectNo.value);
            //}
                //self.CopyFile(fileName, self.FromPath, self.ToPath);
            //}
            callback("", fileName, LocalFilePath, LineLoder,1); // callback for handling read chunk
            return;
        }

        // of to the next chunk
        chunkReaderBlock(offset, chunkSize, file);
    }

    chunkReaderBlock = function (_offset, length, _file) {
        var r = new FileReader();
        var blob = _file.slice(_offset, length + _offset);
        r.onload = readEventHandler;
        //r.readAsText(blob);
        r.readAsDataURL(blob);
        //alert("success");  
    }
    // now let's start the read with the first block
    chunkReaderBlock(offset, chunkSize, file);
} // End of parseFile function

DicomTagChanges = (FileName, FromPath, SubjectValue) =>
{
    const data ={ fileName: FileName, fromPath: DicomFilePath + FromPath, subjectValue: SubjectValue }; const me=this;
    //axios({
    //    method: "Post",
    //    url: "frmImageTransmittal.aspx/DicomTagChange",
    //    data: data,
    //}).then(
    //    function (response){
    //        console.log(response);
    //    }
    //).catch(
    //    function(error){
    //        console.log(error);
    //    }
    //);
    $.ajax({
        type: "POST",
        url: DISoftAPIURL + "SetData/DicomTagChange",
        data: data,
        async: false,
        dataType: "json",
        success: function (result) {
            //me.MaxImageiTranNo=result.Table[0].iImageTranNo;
        },
        failure: function (error) {
            alert(error._message);
            window.onUpdated();
            return false;
        }
    });
}


// call the API for the get the visit review and maxImage iTronNo
GetVisitIsReviewedGetMAXiImageTranNoAPI = () => {
    var strModality = document.getElementById('ddlModality').getElementsByTagName('input');
    var strModalityNo = this.strModalityNo;
    
    var now, FilesName,
    GetreviewStatus = {
        vWorkSpaceID: HProjectId.value,
        vSubjectId: ddlSubject.value,
        iNodeID: ddlVisitNo.value,
        vModalityNo: strModalityNo,
    }
    
    const me = this
    $.ajax({
        type: "POST",
        url: DISoftAPIURL + "CommonData/MAXiImageTranNo",
        data: GetreviewStatus,
        async: false,
        success: function (result) {
            me.MaxImageiTranNo=result.Table[0].iImageTranNo;
        },
        failure: function (error) {
            alert(error._message);
            window.onUpdated();
            return false;
        }
    });

} // End of GetVisitIsReviewedGetMAXiImageTranNoAPI function


//  Create a Function for stored the file on location path,
FileUploadDataAPI = (result, FileName, LocalFilePath, LineLoder,IsUploadComplete) => {
    var File = {
        vFileName: FileName,
        vServerPath: LocalFilePath,
        Content: result
    },me = this;
    $.ajax({
        type: "post",
        url: DISoftAPIURL + "SetData/ImageTransmittalFileUpload",
        data: File,
        async: false,
        dataType: "json",
        success: function (data) { 
            me.ProgressBar(FileName,LineLoder);
            if(IsUploadComplete==1 && (++me.FileCountSuccess == me.flagCount) && me.filesize > me.FileCountSuccess)
            {
                me.flagCount+=me.const_Count;

                if( me.flagCount > me.filesize) {
                    me.flagCount = me.filesize;
                }   
                me.processFileData(me.FileCountSuccess, me.flagCount);
            }

            if(IsUploadComplete==1  && me.FileCountSuccess ==  me.filesize) {
                me.filepath = [];
                me.FileListSaveData = [];
                me.sendEmail();
                //alert("All Files uploaded Sucessfully!");
                //location.reload();
            }
        },
        failure: function (response) {
            msgalert(response.d);
        },
        error: function (response) {
            msgalert("All files are not successfully uploaded!");
            location.reload();
        }
    });
} // End of FileUploadDataAPI function

        //Send email 
sendEmail = () =>
{
    //var ddlTechnician = document.getElementById('ddlTechnician'+this.imageType)
    //var ddlModel = document.getElementById('ddlModel'+this.imageType)
    const SendData = {
        vWorkSpaceID: HProjectId.value ,
        ProjectNo: HProjeName.value ,
        ScreenNo: HSubjectNo.value,
        vParentWorkSpaceId:HParentWorkSpaceId.value,
        VisitNo: HVisit.value , 
        PreformedDate: ExaminationDate.value , 
        PationtInitial:HSubjectName.value,
        study: HStudyNo.value,
        vRemark: document.getElementById("txtInstruction").value,
        //ImagingSystem:ddlModel.item(ddlModel.selectedIndex).text,
        //TechnicianName:ddlTechnician.item(ddlTechnician.selectedIndex).text,
        //ImageType:this.imageType,
        iImgTransmittalDtlId:this.iImgTransmittalDtlId,
        RandomizationNo:HRandomizationNo.value },me = this;
    //console.log(SendData);
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/SendEmail',
        data: SendData
    }).
    then(
        function (response) {// handle success    
         if(response.data.d =="A")
            {
                me.unZipFile();
            }
            else
            {
                alert(response.data.d);
                location.reload();
            }
     //location.reload();
 })
 .catch(
 function (error) {
     // handle error
     console.log(error);
 });
}

//list of file and display ProgressBar
ProgressBar = (fileName,LineLoder)  =>
{
    var tr = document.createElement("TR"),td,div;
    var DivFileName = document.getElementById(fileName);
    var LineBarWidth = LineLoder < 100 ?  LineLoder.toFixed(2):100;
    if(DivFileName == null || DivFileName.id != fileName)
    {
        this.SrNo = this.SrNo + 1;
        td = document.createElement("TD");
        td.innerHTML = this.SrNo;
        td.className = "trBorder";
        tr.appendChild(td);
        td = document.createElement("TD");
        td.innerHTML = fileName;
        td.className = "trBorder";
        tr.appendChild(td);
        td = document.createElement("TD");
        td.style.width = '50%'
        div = document.createElement("DIV");
        div.id = fileName
        div.style.textAlign = 'center';
        div.style.width = LineBarWidth+"%"
        div.style.backgroundColor = 'dodgerblue';
        div.innerText = LineBarWidth+"%";
        td.appendChild(div);
        td.className = "trBorder";
        tr.appendChild(td);
        //td = document.createElement("TD");
        //td.style.display = 'none';
        //td.innerHTML = "<input type='button' id='btnResume' className='btn btncance' value=' Resume '  style= 'margin: 6px; display:none/>";
        //td.className = "trBorder";
        //tr.appendChild(td);
        document.getElementById('ProgressLine').prepend(tr);
    }
    else{
        document.getElementById(fileName).style.width = LineBarWidth+"%";
        document.getElementById(fileName).innerText  =  LineBarWidth+"%";
    }

    //document.getElementById(fileName).style.backgroundColor = 'dodgerblue';
    //document.getElementById(fileName).style.width = LineBarWidth+"%";
    //document.getElementById(fileName).innerText  =  LineBarWidth+"%";
} // End of ProgressBar function 

//File upload 
fuButton = (FileId) =>
{
    if (HProjectId.value == "") {
        HProjectId.focus();
        alert("Please Select Site No!");
        return false;
    }
    document.getElementById(FileId).click()
} // End of fuButton function 

//Count of the file Upload label
fileUploadFn = (fileUploadPara, lblfilesPara, divPara, ttPara) =>
{
    var fileUpload = document.getElementById(fileUploadPara);
    var fileList = [];
    var titleName = '';
    for (var i = 0; i < fileUpload.files.length; i++) {
        if(fileUpload.files[i].name != 'Thumbs.db')
        {
            fileList.push({
                FileName: fileUpload.files[i].name
            });
        }
    }
    document.getElementById(lblfilesPara).innerHTML =fileList.length + ' files';
    for(var g = 0 ; g < fileUpload.files.length ; g++){
        titleName += fileUpload.files[g].name + ", ";
    }
    
    document.getElementById(ttPara).innerText = titleName;
    $("#"+divPara).addClass('tooltip tooltip-scroll');
} // End of fileUploadFn function

//onchange date perform
checkDate = (ctl) =>
{
    var Month,day;
    if(Date.parse(ctl.value) < Date.parse(ctl.min) || Date.parse(ctl.value) > Date.parse(ctl.max) || ctl.value == ''){
        if(new Date().getMonth() < 10)
            Month =  new Date().getMonth() == '0'  ? '01' : '0' + new Date().getMonth() + 1
        if(new Date().getDate() < 10){
            day = '0' + new Date().getDate();}
        else{
            day =new Date().getDate();}
        ctl.value = new Date().getFullYear() + "-" + Month + "-" + day
    }
} // End of checkDate function

//Create a Function for check the popup password
ValidationForAuthentication = () => 
{
    this.txtPasswords = document.getElementById('txtPasswords');
    var VisitPara2 = document.getElementById('HVisit').value;
    this.handleVisitSet(VisitPara2);
    if (txtPasswords.value.trim() == '') {
        txtPasswords.value = '';
        alert('Please Enter Password For Authentication.');
        txtPasswords.focus();
        return false;
    }
    const data = {Password: txtPasswords.value},me=this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/ValidatePassword',
        data: data
    }).
    then(
        function (response) {// handle success  
            if(response.data.d == false){
                alert(" Password Authentication Fails!");
                return false;
            }
            else
            {  document.getElementById("btnSignAuthOK").style.display = "none";
                window.onUpdating();
            me.SaveDataAPI();}
            return true;
        })
    .catch(
        function (error) {
            // handle error
            console.log(error);
        });
    return true;
} // End of ValidationForAuthentication function


//  Create a Function for stored the varaible,create a  path,create file,
// Save the data in DataBase By API call.
SaveDataAPI = () => {
    debugger;
    var IVContrast = "";
    var OralContrast = "";
    if(document.getElementById("ExaminationDate").value != null || document.getElementById("ExaminationDate").value != "" || document.getElementById("ExaminationDate").value !=undefined){
        if(this.IsUpload == false){
            
            if(contrastYes.checked == true){
                IVContrast = "1";
            }
            else if(contrastNo.checked == true){
                IVContrast = "2";
            }
            else if(contrastNA.checked == true){
                IVContrast = "3";
            }

            if(OralcontrastYes.checked == true){
                OralContrast = "1";
            }
            else if(OralcontrastNo.checked == true){
                OralContrast = "2";
            }
            else if(OralcontrastNA.checked == true){
                OralContrast = "3";
            }
        }
    }
    
    var dav="";
    if(daviationNo.checked == true){
        dav="2";
    }
    else if(daviationYes.checked == true){
        dav="1"
    }
    var RandomizationNumber = "";
    if(randomizedNo.checked == true){
        RandomizationNumber = "";
    }
    else if(randomizedYes.checked = true){
        RandomizationNumber = document.getElementById("randomizationNo").value;
    }
    var multiVisit = this.strVisitWithMark.split(',');
    //var j = 1;
    //for(var index=0 ; index < multiVisit.length ; index ++ ){
    //    if(j==2){
            
    //        HParentNodeId.value = this.MarkiParentNodeId;
    //        HActivityId.value = this.MarkvActivityId;
    //        HParentActivityId.value = this.MarkvParentActivityId;
    //    }
        var saveData = {
            iImgTransmittalHdrId: null,
            iImgTransmittalDtlId: null,
            iImgTransmittalImgDtlId: null,
            vWorkspaceId: HProjectId.value,
            vProjectNo:HProjeName.value,
            vSubjectId: ddlSubject.value,
            vRandomizationNo: RandomizationNumber,
            vMySubjectNo: HSubjectNo.value,
            vDOB: DOB.value,
            iNodeId: ddlVisitNo.value,
            iModalityNo: "1",
            iAnatomyNo: "1",
            vAnatomyNo: this.strAnatomyNo,
            cIVContrast: IVContrast,
            cDeviation: dav,
            nvDeviationReason: document.getElementById("txtDaviation").value,
            nvInstructions: document.getElementById("txtInstruction").value,
            dExaminationDate: ExaminationDate.value,
            iNoImages: this.filesize,
            vRemark: "",
            iModifyBy: parseInt(HUserId.value),
            iImageMode: "1",
            vParentWorkspaceId: HParentWorkSpaceId.value,
            vParentActivityId: HParentActivityId.value,
            iParentNodeId: HParentNodeId.value,
            vActivityId: HActivityId.value,
            cOralContrast: OralContrast,
            vModalityNo: this.strModalityNo,
            vModalityDesc: this.SelectedModalities,
            vAnatomyDesc: this.SelectedAnatomies
        }
        console.log(saveData);
        //if(j==2){
        //    saveData["_SaveImageTransmittal"] = this.FileListSaveDataMark;
        //}
        //else{
            saveData["_SaveImageTransmittal"] = this.FileListSaveData;
        //}
        const me = this;
        $.ajax({
            type: "POST",
            url: DISoftAPIURL + "SetData/SaveImageTransmittal",
            data: saveData,
            async: false,
            success: function (data) {
                //var res = eval("(" + data + ")");
                me.iImgTransmittalDtlId = data.toString(); 
                window.onUpdated();
                document.getElementById('myModalSignAuth').style.display = 'none';
                //if(j==1){
                    document.getElementById('myModalProgress').style.display = 'block';
                    me.processFileData(0,me.const_Count);
                //}
            },
            failure: function (error) {
                alert(error._message);
                window.onUpdated();
                return false;
            },
            error: function (error) {
                msgalert("Error while saving data.." + error._message);
            }
        });
        //j++;
    //}
} // End of SaveDataAPI function

//File copy from 
//CopyFile = (FileName, FromPath, ToPath) =>
//{
//    var CopyFile ={ fileName: FileName, fromPath: FromPath, toPath: ToPath }; const me=this;
//    $.ajax({
//        type: "Post",
//        url: DISoftAPIURL + "SetData/CopyFiles",
//        data: CopyFile,
//        async: false,
//        success: function(data){
//            console.log(data);
//        },
//        failure: function(error){
//            alert(error._message);
//            return false;
//        }
//    });
//}


//start FillGridData function
FillGridData =() =>
{     
    debugger;
    const FillGridData ={ WorkspaceId:  HProjectId.value,SubjectId : ddlSubject.value},me = this;
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/FillGrid',
        data: FillGridData
    }).
    then(
    function (response) {// handle success 
        //me.responseData = response.data;
        var tempTable = "";
        tempTable = " <thead><tr class='trBorder'><th class='trBorder'>Audit Query</th><th class='trBorder'>Total Image</th>"+
                    " <th class='trBorder'>Visit</th><th class='trBorder'>Change On</th><th class='trBorder'>Review status</th><th class='trBorder'>Audit Trail</th></tr></thead></tbody>";
        if(response.data.d != null){
            for(var i=0;i < eval(response.data.d).length; i++)
            { 
                if(eval(response.data.d)[i].cQueryStatus == null){
                    tempTable +="<tr class='trBorder'><td><center></center></td><td class='trBorder'>" 
                    + eval(response.data.d)[i].iNoImages + "</td><td class='trBorder'>" 
                    + eval(response.data.d)[i].vNodeDisplayName + "</td><td class='trBorder'>" 
                    + eval(response.data.d)[i].ChangeOn + "</td><td class='trBorder'>"
                    + eval(response.data.d)[i].cReviewStatus + "</td>"
                    + "<td class ='trBorder'> <center><img alt='submit' style ='cursor: pointer' src='images/audit.png' onclick='HandleAuditTrail(" +  eval(response.data.d)[i].iImgTransmittalHdrId + ")'></img></center></td>" ;

                }
                else if(eval(response.data.d)[i].cQueryStatus.trim(0) == "QC1R" || eval(response.data.d)[i].cQueryStatus.trim(0) == "QC2R" || eval(response.data.d)[i].cQueryStatus.trim(0) == "CA1R" ){
                tempTable +="<tr class='trBorder'><td><center><img alt='submit' style ='cursor: pointer' src='images/Upload.jpg' onclick='HandleEditQuery(" + eval(response.data.d)[i].nQueryNo + ")'></img><img alt='submit' style ='cursor: pointer' src='images/audit.png' onclick='HandleEditQuery(" + eval(response.data.d)[i].nQueryNo + ")'></img></center></td><td class='trBorder'>" 
                    + eval(response.data.d)[i].iNoImages + "</td><td class='trBorder'>" 
                    + eval(response.data.d)[i].vNodeDisplayName + "</td><td class='trBorder'>" 
                    + eval(response.data.d)[i].ChangeOn + "</td><td class='trBorder'>"
                    + eval(response.data.d)[i].cReviewStatus + "</td>"
                    + "<td class ='trBorder'> <center><img alt='submit' style ='cursor: pointer' src='images/audit.png' onclick='HandleAuditTrail(" +  eval(response.data.d)[i].iImgTransmittalHdrId + ")'></img></center></td>" ;
                }
                else if(eval(response.data.d)[i].cQueryStatus.trim(0) == "QC1A" || eval(response.data.d)[i].cQueryStatus.trim(0) == "QC2A" || eval(response.data.d)[i].cQueryStatus.trim(0) == "CA1A" ){
                    tempTable +="<tr class='trBorder'><td><center><img alt='submit' style ='cursor: pointer' src='images/Edit2.gif' onclick='HandleEditQuery(" + eval(response.data.d)[i].nQueryNo + ")'></img><img alt='submit' style ='cursor: pointer' src='images/audit.png' onclick='HandleEditQuery(" + eval(response.data.d)[i].nQueryNo + ")'></img></center></td><td class='trBorder'>" 
                        + eval(response.data.d)[i].iNoImages + "</td><td class='trBorder'>" 
                        + eval(response.data.d)[i].vNodeDisplayName + "</td><td class='trBorder'>" 
                        + eval(response.data.d)[i].ChangeOn + "</td><td class='trBorder'>"
                        + eval(response.data.d)[i].cReviewStatus + "</td>"
                        + "<td class ='trBorder'> <center><img alt='submit' style ='cursor: pointer' src='images/audit.png' onclick='HandleAuditTrail(" +  eval(response.data.d)[i].iImgTransmittalHdrId + ")'></img></center></td>" ;
                }
            }
        }
        else{tempTable += "<tr class='trBorder'><td colSpan='7'>No Record Found..!</td></tr>"}
        tempTable += "</tbody>"
        document.getElementById('gvwImageTransmittalMst').innerHTML = tempTable;
 
    })
    .catch(
    function (error) {
        // handle error
        console.log(error);
    });

} // End of FillGridData function


//On clear function to clear the some control
onClearFn = () =>
{
    document.getElementById('DOB').value = "";
    document.getElementById('daviationNo').checked = true;
    document.getElementById('txtDaviation').value = "";
    document.getElementById('txtInstruction').value = "";
    document.getElementById('contrastYes').checked = true;
    document.getElementById('lblfiles').value = "";
    document.getElementById("fileUpload").value ="";
    //document.getElementById("ddlSetNo").value ="0";
    //document.getElementById('ddlCertified').value = "N";
    //document.getElementById("fileCerificate").value ="";
    //document.getElementById("fileUpload").value ="";
}

//Cancle button function clear all the fileds.
//

cancleData =()=>
{
    document.getElementById('ddlSubject').value = "0";
    document.getElementById('ddlVisitNo').value = "0";
    document.getElementById('ddlModality').value = "0";
    document.getElementById('ddlAnatomy').value = "0";
    this.onClearFn();
    $("#SelectAll").prop("checked", false);
} // End of Cancle button function

VisitValidation = () => {
    debugger;
    const VisitValidateData ={ iProjectId:  HProjectId.value, iSubjectId : ddlSubject.value, visitName: ddlVisitNo.options[ddlVisitNo.selectedIndex].outerText}
    axios({
        method: 'Post',
        url: 'frmImageTransmittal.aspx/VisitValidation',
        data: VisitValidateData
    }).then(
    function (response){

        for (var i = 0; i < 1; i++) {

            if (ddlVisitNo.options[ddlVisitNo.selectedIndex].outerText == eval(response.data.d)[i].vNodeDisplayName && ddlModality.options[ddlModality.selectedIndex].outerText == eval(response.data.d)[i].vModalityDesc && eval(response.data.d)[i].cReviewStatus == "Approved")
            {
                alert("Visit '" + eval(response.data.d)[i].vNodeDisplayName + "' and Modality '" + eval(response.data.d)[i].vModalityDesc + "' are under the review");
                document.getElementById('ddlModality').value = "0";
            }
        }
    }).catch(
    function (error) {
        // handle error
        console.log(error);
    });
}

ChangeRadioButton=()=>{
    debugger;
    if(document.getElementById("daviationNo").checked == true){
        document.getElementById("txtDaviation").disabled = true;
        document.getElementById("txtDaviation").value = null;
    }else if(document.getElementById("daviationYes").checked == true){
        document.getElementById("txtDaviation").disabled = false;
    }
}

ChangeRadioButton1=()=>{
    debugger;
    if(document.getElementById("randomizedNo").checked == true){
        document.getElementById("randomizationNo").disabled = true;
        document.getElementById("randomizationNo").value = "";
    }else if(document.getElementById("randomizedYes").checked == true){
        document.getElementById("randomizationNo").disabled = false;
    }
}

// VisitSet Validation
handleVisitSet = (VisitName) =>
{
    this.result  = [];
    this.setFlag =0;
    if(VisitName != '')
    {
        const data = {HProjectId : HProjectId.value,SubjectId : HSubjectId.value,VisitName : VisitName},me=this;
        axios({
            method: 'Post',
            url: 'frmImageTransmittal.aspx/CheckVisitStatus',
            data: data
        }).
        then(
            function (response) {// handle success  
                me.setFlag =0;
                var JsonObj = JSON.parse(response.data.d).Table;
                if(JsonObj.length > 0)
                {
                    if(JsonObj[0].ImageReviewStatus.trim() == "QC1A" || JsonObj[0].ImageReviewStatus.trim() == "QC2A" || JsonObj[0].ImageReviewStatus.trim() == "CA1A" || JsonObj[0].ImageReviewStatus.trim() == "R1" || JsonObj[0].ImageReviewStatus.trim() == "R2"){
                        me.setFlag =1;
                        alert(VisitName +" visit under the review..!");
                        document.createElement("ddlVisitNo").value ="Select Visit";
                        document.getElementById("ddlVisitNo").value = "0";
                        document.getElementById('myModalSignAuth').style.display = 'none';
                    }else if(JsonObj[0].ImageReviewStatus.trim() == "AA"){
                        me.setFlag =1;
                        alert(VisitName +" visit Approved!");
                        document.createElement("ddlVisitNo").value ="Select Visit";
                        document.getElementById("ddlVisitNo").value = "0";
                        document.getElementById('myModalSignAuth').style.display = 'none';
                    }
                }
            })
        .catch(
            function (error) {
                // handle error
                console.log(error);
            });
    
    }
}

DomCreate = () =>{
    if(HProjectId.value==""){
        HProjectId.focus();
        alert("Please select site no!");
        return false;
    }
    if(document.getElementById('ddlSubject').value == "" || document.getElementById('ddlSubject').value == "0"){
        alert("Please select subject!");
        return false;
    }
    var table = document.getElementById("tblModality");
    this.rowCount = table.rows.length;
    var rowCountt = table.rows.length;

    this.rowCount = this.rowCount+1;
    rowCountt = rowCountt+1;
    this.bindAnatomy();
    this.bindModality(); 
    if(rowCountt>2){
        var x = rowCountt - 1;
        $("#dltRow"+x).hide();
    }
    var joined = this.state.rows.concat(

        <tr id={rowCountt}>
            <td style={{textAlign: 'center'}}>{rowCountt}</td>
            <td style={{textAlign: 'left',width: '50%'}} className="Label" colSpan='2'>
                Anatomy*: 
                <div id={`ddlAnatomy`+rowCountt} className="dropDownList" style={{width:'40%', height:'80px', overflowY: 'auto',overflowX: 'auto'}}></div>
                
                <input type='text' id={`OtherAnatomy`+rowCountt} className="textBox" disabled="disabled" placeholder="Add Other Anatomy" style={{width: '40%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}} />

            </td>
            <td style={{textAlign: 'left',width: '50%'  }}>
                Modality*: <select  id={`ddlModality`+rowCountt}  className="dropDownList" style={{width:'40%'}} onChange={e => {this.handleModalityChange(e.target.value, rowCountt)}}>
                </select>
                 <input type='text' id={`OtherModality`+rowCountt} className="textBox" disabled="disabled" placeholder="Add Other Modality" style={{width: '40%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}} />
                <input type="button"  style={{width:"120px",height:"30px"}} value=" Choose Files " onClick={e =>{this.fuButton(`fileUpload`+rowCountt)}}/>
                <input type="file" id={`fileUpload`+rowCountt} style={{display:"none"}} webkitdirectory="true" mozdirectory="true" onChange={e => {this.fileUploadFn(`fileUpload`+rowCountt, `lblfiles`+rowCountt, `div`+rowCountt, `tt`+rowCountt, )}} />
                
                <lable className="Label" style = {{paddingLeft: '10px'}} id={`div`+rowCountt}>
                    <lable id={`lblfiles`+rowCountt}>No file choosen</lable>
                    <div className="wrapper"> 
                        <span id={`tt`+rowCountt} className="tooltip-text"></span>
                    </div>
                </lable>
                
            </td>
            <td style={{textAlign: 'center', width:"30px" }} ><input id={`dltRow`+rowCountt} type="image" src="images/delete_small.png" value=" Delete " onClick={e =>{this.RemoveDom(rowCountt)}} /></td>
        </tr>
    
      );
     
    this.setState({ rows: joined })
}

RemoveDom = (num) =>{
    const TblArr = [];
    for(var k = 0 ; k < document.getElementById("tblModality").getElementsByTagName("tr").length ; k++){
        TblArr.push(document.getElementById("tblModality").getElementsByTagName("tr")[k].id);
    }
    const number = element => element == num
    const index = TblArr.findIndex(number);
    document.getElementById("tblModality").deleteRow(index);

    var y = num - 1
    $("#dltRow"+y).show();
    this.rowCount = this.rowCount - 1;
    //document.getElementById("tblModality").deleteRow(index);
}

removeSpecialCharacter = () =>{
    if(document.getElementById("randomizationNo") != null){
        var removedSpecialChar = document.getElementById("randomizationNo");
        document.getElementById("randomizationNo").value = removedSpecialChar.value.replace(/[&/\[\]\/\\#,+()$~%.'":*?<>{}]/gi, '');
    }
    if(document.getElementById("txtDaviation") != null){
        var Daviation = document.getElementById("txtDaviation")
        document.getElementById("txtDaviation").value = Daviation.value.replace(/[&/\[\]\/\\#,+()$~%.'":*?<>{}\r\n]/gm, '');
    }
    if(document.getElementById("txtInstruction") != null){
        var Instruction = document.getElementById("txtInstruction")
        document.getElementById("txtInstruction").value = Instruction.value.replace(/[&/\[\]\/\\#,+()$~%.'":*?<>{}\r\n]/gm, '');
    }
}


render(){ 
    return(
        <div> 
            <table style={{width:'100%',cellPadding: '5px'}}>
                <tbody>
                    <tr>
                        <td>
                            <fieldset className="FieldSetBox" style={{display: 'block',width: '96%', margin: 'auto', textAlign: 'left' ,border: '#aaaaaa 1px solid'}}>
                                <legend className="LegendText" style={{color: 'Black', fontSize: '12px'}}>
                                    <img id="img1" alt="Equipment Data" src="images/panelcollapse.png" onClick={e => this.Display(e,'divImageTransmittal')} style={{marginRight: '2px' }}/>
                                    <span id="lblImageTransmittalDetails" className="Label"></span>
                                </legend>
                                <div id="divImageTransmittal">
                                    <table id="gvwImageTransmittalMst" style={{width: 'auto', margin: 'auto'}}>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table style={{width:'100%',cellPadding: '5px'}}>
                <tbody>
                    <tr>
                        <td>
                            <fieldset className="FieldSetBox" style={{display: 'block', width: '96%', margin: 'auto', textAlign: 'left' ,border: '#aaaaaa 1px solid'}}>
                                <legend className="LegendText" style={{color: 'Black',fontSize: '12px'}}>
                                    <img id="img2" alt="Equipment Details" src="images/panelcollapse.png" onClick = {e => this.Display(e,'divImageTransmittalDetails')} style={{marginRight: '2px'}}/>
                                    <span id="lblImageTransmittal" className="Label"> </span>
                                </legend>
                                <div id="divImageTransmittalDetails">
                                    <table style={{width: '99%', margin: 'auto'}}>
                                        <tbody>
                                            <tr>
                                                <td style={{textAlign: 'right', width: '30%'}} className="Label">
                                                    <label id="txtProject">Site No * :</label>
                                                </td>
                                                <td style={{textAlign: 'left',width: '70%'}} colSpan="3">
                                                    <ReactAutocomplete  className="textBox"  inputProps={{ style: { width: '100%'}}} wrapperStyle={{ width:'100%',position: 'relative', display: 'inline-block' }} getItemValue={(item) => item.title} items={this.state.ProjectOpt} 
                                                        renderItem={(item, isHighlighted) =>
                                                        <div key={item.title} id="racSiteId" style={{width:'100%',maxHeight: '300px',background: isHighlighted ? 'lightgray' : 'white' }}>
                                                            {item.title}
                                                        </div>
                                                        }
                                                        value={this.ProjectName ? this.ProjectName :this.state.value}
                                                        onChange={e => {this.setState({ value: e.target.value }), e.target.value.length >=1 ? this.onKeyDownHandler(e):""}}
                                                        onSelect={(value,val) => {this.setState({ value}),this.ProjectOnItemSelected(val.Prefix)}}
                                                        />
                                                        <input type="hidden" id="HProjectId" />
                                                        <input type="hidden" id="HParentWorkSpaceId" />
                                                        <input type="hidden" id="HProjeName" />
                                                        <input type="hidden" id="HUserId" />
                                                        <input type="hidden" id="HExternalProjectNo" />
                                                        <input type="hidden" id="HStudyNo" />
                                                </td>
                                            </tr>
                                            <tr style={{height: '2pc'}}>
                                                <td style={{textAlign: 'right', width: '15%'}} className="Label ">
                                                    <label id="lblScreening">Screening No *:</label>
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <select  id="ddlSubject"  className="dropDownList" style={{width:'100%'}} onChange={e => {this.HandleSubjectChange(e.target.value)}} >
                                                    </select>
                                                    <input type="hidden" id="HSubjectName" />
                                                    <input type="hidden" id="HSubjectId" />
                                                    <input type="hidden" id="HSubjectNo" />
                                                    <input type="hidden" id="HRandomizationNo" />
                                                    <input type="hidden" id="HDob" />
                                                </td>
                                                <td style={{textAlign: 'right'}} className="Label ">
                                                    <label id="lblVisit">Visit *:</label>
                                                </td>
                                                <td style={{textAlign: 'left',width: '30%'  }}>
                                                    <select  id="ddlVisitNo"  className="dropDownList" style={{width:'100%'}}  onChange={e => {this.handleVisitChange(e.target.value)}}>
                                                    </select>
										    		 <input type="hidden" id="HNodeId" />							
                                                    <input type="hidden" id="HParentNodeId" />
                                                    <input type="hidden" id="HActivityId" />
                                                    <input type="hidden" id="HParentActivityId" />
                                                    <input type="hidden" id="HVisit" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style={{textAlign: 'right'}} className="Label ">
                                                    <label id="lblScreening">DOB *:</label>
                                                    </td>
                                                    <td style={{textAlign: 'left'}}>
                                                    <input type="date" id="DOB" name="DOB" />
                                                </td>
                                            </tr>
                                            <tr style={{height: '2pc'}}>
                                                <td style={{textAlign: 'right'}}className="Label ">
                                                    Deviation :
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <label style={{color: 'red'}}><input type="radio" id="daviationNo" name="daviation" style={{paddingLeft: '5%'}} onChange={e => {this.ChangeRadioButton()}} defaultChecked /> No</label>
                                                    <label style={{color: 'green'}}><input type="radio" id="daviationYes" name="daviation" style={{margin:'revert'}} onChange={e => {this.ChangeRadioButton()}} /> Yes </label>
                                                    <textarea  rows="4" cols="50" id="txtDaviation" className="textBox" style={{width: 'auto', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}}/>
                                                </td>
                                                <td style={{textAlign: 'right'}}className="Label ">
                                                    Instruction to CRC :
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <textarea  rows="4" cols="50" id="txtInstruction" className="textBox" style={{width: '100%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}}/>
                                                </td>
                                            </tr>
                                            <tr style={{height: '2pc'}}>
                                                <td style={{textAlign: 'right'}}className="Label ">
                                                    Is Patient Randomized?
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <label style={{color: 'red'}}><input type="radio" id="randomizedNo" name="randomized" style={{paddingLeft: '5%'}} onClick={e => {this.ChangeRadioButton1()}}/> No</label>
                                                    <label style={{color: 'green'}}><input type="radio" id="randomizedYes" name="randomized" style={{margin:'revert'}} onClick={e => {this.ChangeRadioButton1()}} /> Yes </label>
                                                </td>
                                            </tr>
                                            <tr style={{height: '2pc'}}>
                                                <td style={{textAlign: 'right'}}className="Label ">
                                                    Add Randomization No
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <input type='text' id='randomizationNo' className="textBox" placeholder="Add Randomization No." style={{width: '100%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}} />
                                                </td>
                                            </tr>
                                            
                                            
                                            <tr style={{height: '2pc'}}>
                                                <td style={{textAlign: 'right'}}>
                                                    IV Contrast *:
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <label style={{color: 'green'}}><input type="radio" id="contrastYes" name="contrast" style={{margin:'revert'}} /> Yes </label>
                                                    <label style={{color: 'red'}}><input type="radio" id="contrastNo" name="contrast" style={{paddingLeft: '5%'}} /> No</label>
                                                    <label><input type="radio" id="contrastNA" name="contrast" style={{paddingLeft: '5%'}} /> NA</label>
                                                </td>

                                                <td style={{textAlign: 'right'}}>
                                                    Oral Contrast :
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <label style={{color: 'green'}}><input type="radio" id="OralcontrastYes" name="Oralcontrast" style={{margin:'revert'}} /> Yes </label>
                                                    <label style={{color: 'red'}}><input type="radio" id="OralcontrastNo" name="Oralcontrast" style={{paddingLeft: '5%'}} /> No</label>
                                                    <label><input type="radio" id="OralcontrastNA" name="Oralcontrast" style={{paddingLeft: '5%'}} /> NA</label>
                                                </td>
                                                
                                            </tr>

                                            <tr style={{height: '2pc'}}>
                                                
                                                <td style={{textAlign: 'right'}} className="Label ">
                                                    Examination Date *:
                                                </td>
                                                <td style={{textAlign: 'left'}}>
                                                    <input type="date" id="ExaminationDate" name="ExaminationDate" onChange={e => {this.checkDate(e.target)}} />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colSpan='4' style={{width: '100%'}}>
                                                    <br />
                                                    <center><input type="button" id="addUpload" className="btn btnsave" value=" Add new row" onClick ={this.DomCreate} /></center>
                                                    <table id="tblModality" border='1px' style={{width: '100%'}}>
                                                        <tr>
                                                            <td style={{textAlign: 'center',width: '5%'}}>1</td>
                                                            <td style={{textAlign: 'left',width: '50%'}} className="Label" colSpan='2'>
                                                                Anatomy*:
                                                                
                                                                <div id="ddlAnatomy" className="dropDownList" style={{width:'40%', height:'80px', overflowY: 'auto',overflowX: 'auto'}} ></div>
                                                               <input type='text' id='OtherAnatomy' className="textBox" disabled="disabled" placeholder="Add Other Anatomy" style={{width: '40%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}} />

                                                            </td>
                                                            
                                                            <td style={{textAlign: 'left',width: '50%'  }} colSpan='2'>
                                                                Modality*: <select  id="ddlModality"  className="dropDownList" style={{width:'40%'}} onChange={e => {this.handleModalityChange(e.target.value, 1)}}>
                                                                </select>
                                                                <input type='text' id='OtherModality' className="textBox" disabled="disabled" placeholder="Add Other Modality" style={{width: '40%', height: 'auto'}} onBlur={e => {this.removeSpecialCharacter()}} />
                                                                <input type="button"  style={{width:"120px",height:"30px"}} value=" Choose Files " onClick={e =>{this.fuButton('fileUpload')}}/>
                                                                <input type="file" id="fileUpload" style={{display:"none"}} webkitdirectory="true" mozdirectory="true" onChange={e => {this.fileUploadFn("fileUpload", "lblfiles", "div", "tt")}} />
                                                                <lable id="div">
                                                                    <lable id="lblfiles">No file choosen</lable>
                                                                    <div className="wrapper"> 
                                                                        <span id="tt" className="tooltip-text"></span>
                                                                    </div>
                                                                </lable>
                                                                <br />
                                                            </td>
                                                            
                                                        </tr>
                                                        {this.state.rows}
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style={{textAlign: 'center',whiteSpace: 'nowrap'}} className="Label" colSpan="4">
                                                        <input type="button" id="btnSave" className="btn btnsave" value=" Save " onClick ={this.saveData}/>
                                                        <input type="button" id="btnCancel" className="btn btncancel" value=" Cancel " style={{margin: '6px'}} onClick ={this.cancleData}/>
                                                        <input type="button" id="btnClose" className="btn btnclose" value=" Exit " style={{margin:'0px'}}/>
                                                </td>
                                            </tr>
                                            <tr style={{height: '2pc'}}>
                                                <td colSpan="4">
                                                    <table style={{width: '100%', margin: 'auto',color:'black'}}>
                                                        <tbody  id="fileUploadTable">
                                                        </tbody>
                                                    </table>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </fieldset>
                        </td>
                    </tr>
                </tbody>
            </table>
            <div id="myModalSignAuth" className="modal" style={{display : 'none'}} >
                <div className="modal-content" style={{top: "20%", width: "38%"}}>
                    <div className="modal-header" style={{textAlign: "left"}}>
                        <img alt="Close1" src="images/Sqclose.gif" className="close modalCloseImage"  onClick={this.SignAuthModalClose} />
                        <h3 style={{textAlign: "center"}}>
                             <label id="Label4">Signature Authentication</label>
                        </h3>
                        <table style={{width: '100%'}}>
                            <tbody>
                                <tr>
                                    <td align="right" style={{width: '16%', fontWeight: 'bold',fontSize : '14px'}}>User Name :</td>
                                    <td align="left" style={{width: '15%', fontSize: '14px' }}><div id="lblSignAuthUserName" className="Label"></div></td>
                                    <td align="right" style={{width: '16%',fontWeight: 'bold',fontSize: '10px'}}>Date & Time :</td>
                                    <td align="left" style={{width: '26%'}}><div id="lblSignAuthDateTime" className="Label"></div></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div className='modal-body'>
                        <table style={{width:'100%'}}>
                            <tbody>
                                <tr>
                                    <td align='center' colSpan='2'><label id='Label5'> We will remove below tags from image if present:</label></td>
                                </tr>
                                <tr>
                                    <td align='center' colSpan='2'><label><b>Patient Name, Accession Number, Institution Address, Referring Physician name, Performing Physician name, Other patient name, Other patient IDs, Patient Address</b></label></td>
                                </tr>
                                
                                <tr>
                                    <td align='right'><label id='Label5'>Password* :</label></td>
                                    <td align='left'>
                                        <input type ='password' id='txtPasswords'  style={{Width: '59%'}} className='textbox'/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align='center' colSpan='2'>
                                        <label id='Label7' >I hereby confirm signing of this record electronically.</label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style={{textAlign: "center"}} colSpan="2">
                                         <input type="button" id="btnSignAuthOK" className="btn btnsave" value="OK"  style={{width:'105px'}} onClick={ this.ValidationForAuthentication} />
                                        <input type="button" id="btnSignAuthCancel"  className="btn btncancel" value="Cancel" style={{width:'105px',margin: '6px'}} onClick={ this.SignAuthModalClose} />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div id="myModalProgress" className="modal" style={{display : 'none'}} >
                <div className="modal-content" style={{top: "20%", width: "90%", left:"5%"}}>
                    <div className="modal-header" style={{textAlign: "left"}}>
                        <h3 style={{textAlign: "center"}}>
                            <label id="Label4">File Upload</label>
                        </h3>
                    </div>
                    <div className='modal-body'  style={{height:'300px', overflowY: 'auto',overflowX: 'auto'}}>
                        <table style={{width:'100%',borderCollapse: 'collapse'}} >
                            <tbody id="ProgressLine" style={{width:'auto'}} className='trBorder'>
                        
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

            <div id="AuditTrailDiv" className="modal" style={{display : 'none',}} >
                <div className="modal-content" style={{top: "20%",left:"0%", width: "100%"}}>
                    <div className="modal-header">
                        <img alt="Close1" src="images/Sqclose.gif" className="close modalCloseImage"  onClick={this.SignAuthModalClose} />
                        <h3 style={{textAlign: "center"}}>
                        <label id="Label4">Audit Trail</label>
                        </h3>
                    </div>
                    <div className='modal-body' style={{height:'300px',overflowY: 'auto',overflowX: 'auto'}}>
                        <table style={{width:'100%', borderCollapse: 'collapse'}} className="table" >
                            <tbody id="AuditTrailTable" style={{width:'auto'}}>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>

              <div id="EditQueryDiv" className="modal" style={{display : 'none',}} >
                <div className="modal-content" style={{top: "20%",left:"0%", width: "100%"}}>
                    <div className="modal-header">
                        <img alt="Close1" src="images/Sqclose.gif" className="close modalCloseImage"  onClick={this.QSignAuthModalClose} />
                        <h3 style={{textAlign: "center"}}>
                        <label id="Label4">Edit Query</label>
                        </h3>
                    </div>
                    <div className='modal-body' style={{height:'300px',overflowY: 'auto',overflowX: 'auto'}}>
                        <table style={{width:'100%', borderCollapse: 'collapse'}} className="table" >
                            <tbody id="EditQueryTable" style={{width:'auto'}}>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        );
    }
}
ReactDOM.render(<App />, document.getElementById('ImageTransmittal_container'));