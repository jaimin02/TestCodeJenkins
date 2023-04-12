var startXmlTag = String.fromCharCode(254);
var endXmlTag = String.fromCharCode(255);


function ConvertXml2Excel(xmlValue_1, ExcelFile_1)
{
    xmlValue_1 = xmlValue_1.replace(new RegExp(startXmlTag,'g'),"<");
    xmlValue_1 = xmlValue_1.replace(new RegExp(endXmlTag,'g'),">");
    
	var xmldoc=new ActiveXObject("MSXML2.DOMDocument");
	xmldoc.loadXML(xmlValue_1);
	var xmlNodeList = xmldoc.getElementsByTagName("row");

	try
	{
		var oExcel = new ActiveXObject("Excel.Application");
        
        if (oExcel == null)
        {
            return;
        }
        		
		oExcel.workbooks.Add();
		var oWs = oExcel.ActiveSheet;
	
		for(row=1; row<=xmlNodeList.length; row++)
		{
			for(col=1; col<=xmlNodeList[row-1].attributes.length;col++)
			{
				oWs.Cells(row, col).Value =xmlNodeList[row-1].attributes[col-1].value;
			}
		}
	
		var fname ="file://" + ExcelFile_1;
		oExcel.ActiveWorkbook.saveas(fname);
		
	}
	catch(e)
	{
		if (oExcel!=null)
		{
			oExcel.ActiveWorkbook.close();
		}
	}

	oExcel.Quit();
	oWs = null;
	oExcel= null;
	xmldoc = null;
	xmlNodeList =null;

}
function ConvertExcel2Xml(ExcelFile_1, noofColumn4read)
{

	var BlankColumn;
	var oExcel = new ActiveXObject("Excel.Application");
    
    if (oExcel ==  null)
    {
        return ;
    }
   
	try
	{
	
	    try
	    {
		    oExcel.workbooks.open(ExcelFile_1);
		}
		catch(e)
		{
		    alert(e.message);
		}
		    
		
		var oWs = oExcel.ActiveSheet;
		var retustr="";
	
		for(row=1; row<=oExcel.Rows.count; row++)
		{
			var tStr="";
			BlankColumn=0;

			for(col=1; col<=noofColumn4read; col++)
			{
				if(oWs.Cells(row,col).value == null || oWs.Cells(row,col).value=="")
				{
					BlankColumn ++;
				}
				else
				{
					tStr += ReplaceSpecialCharacter(oWs.Cells(1,col).value) + " = " + '"' +  ReplaceSpecialCharacter(oWs.Cells(row,col).value) + '"' + " " ;
				}
			}
			
			if (BlankColumn != noofColumn4read)
			{

				retustr += startXmlTag + "row " + tStr + "/" + endXmlTag + " "; 
			}
			else if(IsLastRow(oWs,row, noofColumn4read))
			{
				break;
			}
		}
		
	}
	catch(e)
	{
		oExcel.ActiveWorkbook.close();
	}

	oExcel.Quit();
	oExcel = null;
	oWs = null;
    
    retustr = startXmlTag + "dataset" + endXmlTag + " " + retustr + " " + startXmlTag + "/dataset" + endXmlTag + " "; 
	return retustr;
}
function IsLastRow(oWs, startRow, noofColumn4read)
{
	startRow +=1;
	
	for(row=startRow; row<=oWs.Rows.count; row++)
	{
		for(col=1; col<=noofColumn4read; col++)
		{
		    if (Null2String(oWs.Cells(row,col).value) != "")
		    {
			    return false;
			}  
		}
		if (row == startRow + 50)
		{
			break;
		}
	}

	return true;
}
function Null2String(StrVal_1)
{
    try
    {
        if (StrVal_1==null)
        {
            return "";
        }
        return new String(StrVal_1).toString() ;
    }
    catch(e)
    {
        return "";
    }
}
function ReplaceSpecialCharacter(Str_1)
{
    var s = new String(Str_1);
    
    s = s.replace('&','&amp;');
    s = s.replace('"','&quot;');
    s = s.replace("'",'&apos;');
    
    return s
}