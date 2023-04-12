
function FreezeTableHeader($ele, options)
        {
if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
 //test for MSIE x.x;
 var ieversion=new Number(RegExp.$1) // capture x.x portion and store as a number
 if (ieversion>=8)
{
            var $div = $ele.parent().parent();
            var ddiv = $('<div>').prependTo($div.parent());

            $div.scroll(function()
            {
                ddiv[0].scrollLeft = this.scrollLeft;
            });
            ddiv.css({ overflow: 'hidden', width: $div.width() });
            var header = $('<table>', { "BORDER": "#add7ea 1px solid;" }).prependTo(ddiv);
            header.border = '1px';
            var w = new Array();
            $('tr:first th', $ele).each(function()
            {
                w.push(this.clientWidth);
            });
           
            $('tr:first', $ele).appendTo(header);
            for (var i = 0; i < w.length; i++)
            {
                $('tr:first th:nth-child(' + (i + 1) + ')', header).css({ minWidth: w[i] + 4 + 'px' });
                $('tr:first td:nth-child(' + (i + 1) + ')', $ele).css({ minWidth: w[i] + 'px' });
            }
            $('<th>').css({ minWidth: getScrollBarWidth() }).appendTo($('tr:first', header)); ;
            var $td = $('#' + $ele.get(0).id + ' > tbody > tr:last > td');
            if ($td.attr('colspan') == w.length)
            {
                // this grid has paging...
                $('<div align="center">').appendTo($div.parent()).append($('table', $td)).css({ width: $div.width() });
            }
}                  
        }
        

        function getScrollBarWidth()
        {
            var inner = document.createElement('p');
            inner.style.width = "100%";
            inner.style.height = "200px";

            var outer = document.createElement('div');
            outer.style.position = "absolute";
            outer.style.top = "0px";
            outer.style.left = "0px";
            outer.style.visibility = "hidden";
            outer.style.width = "200px";
            outer.style.height = "150px";
            outer.style.overflow = "hidden";
            outer.appendChild(inner);

            document.body.appendChild(outer);
            var w1 = inner.offsetWidth;
            outer.style.overflow = 'scroll';
            var w2 = inner.offsetWidth;
            if (w1 == w2) w2 = outer.clientWidth;

            document.body.removeChild(outer);

            return (w1 - w2);
        };