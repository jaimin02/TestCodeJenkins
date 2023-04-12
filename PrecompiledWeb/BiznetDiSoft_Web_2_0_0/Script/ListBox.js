// JScript File

function FillSelectedValueFromSourceToDestination(Source, Destination)
{   
	var j= Source.length; 
	var k=0;		

    if(Source.selectedIndex < 0)
	{
	    return false;    
	}
		
	for( i = 0; i < j ; i++)
	{
		if(Source[i].selected)
		{
		    st = new Option(Source[i].text,Source[i].value);
		    
		    Destination.add(st,(Destination.length));
		    Source.remove(i);
		    
		    break;
	    }
    }        
			
	return false;    	    
}


function UpDownListItem(listbox,action)
{
    var text1,old_position,new_position;
    var val1;
    
    if (listbox.selectedIndex < 0)
    {
        return false;
    }
    
    for (i =0 ;i<listbox.length;i++)
    {    
        if(listbox[i].selected)
        {
            text1 = listbox[i].text;
            val1 = listbox[i].value;
            old_position = i;
            break;
        } 
    }
    
    if (action == 'UP')
    {
        new_position= old_position - 1;
    }
    else
    {
        new_position= old_position + 1;
    }
    
    if ((new_position < 0 ) || (new_position >= listbox.length))
    {
        return false;
    }
    
    listbox.remove(old_position);
	
	st = new Option(text1,val1);
	
	listbox.add(st,new_position);
	listbox.selectedIndex = new_position;
	return true;
    
    
}
