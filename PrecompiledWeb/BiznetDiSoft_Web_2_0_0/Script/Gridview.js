// JScript File
// Created By   : Ravi Patel
// Created On   : 29-march-2008

// This function is used for check whether atleast one checkbox is selected

//This variable is used when mouse over on gridview rows
var curSelColor;
var overColor='Silver';


function CheckOne(gv)
{    
    var tableElement = document.getElementById(gv);
    
    if (tableElement != null)
    {
        var CheckBoxes = tableElement.getElementsByTagName('input');            
         
        for (i=0; i < CheckBoxes.length; i++)
        {
            if (CheckBoxes[i].type=='checkbox' || CheckBoxes[i].type=='CHECKBOX')
            {
                if (CheckBoxes[i].checked == true)
                {
                    return true;
                }
            }
        }      
    }  
    return false;
}



// This function is used for check whether atleast one checkbox is selected

function CheckUnCheckAll(gv,checked_1)
{
    var tableElement = document.getElementById(gv);
   
    if (tableElement != null)
    {
        var CheckBoxes = tableElement.getElementsByTagName('input');            
        
        for (i=0; i < CheckBoxes.length; i++)
        {
            if (CheckBoxes[i].type=='checkbox' || CheckBoxes[i].type=='CHECKBOX')
            {
                CheckBoxes[i].checked = checked_1;
            }
        }
    }
    return false;
}

function gridViewMouseOver(gvr)
{
    curSelColor = gvr.style.backgroundColor;
    gvr.style.backgroundColor = overColor;
}

function gridViewMouseOut(gvr)
{
    gvr.style.backgroundColor = curSelColor;
}

