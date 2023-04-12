var DayYesterday = 0;
var DayToday = 1;
var DayLast7day = 2;
var DayLast30day = 3;
var DayLast60day = 4;
var DayLast90day = 5;
var DayLast120day = 6;
var WeekLast = 7;
var WeekCurrent = 8;
var WeekLastCurrent = 9;
var MonthLast = 10;
var MonthCurrent = 11;
var MonthLastCurrent = 12;
var CurrentQuarter = 13;
var LastQuarter = 14;

function getTimeFrame(objGDDL, objTxtFrom, objTxtTo)
{
    var ddlTime;
    var TimeVal;

    var fromDate;
    var toDate;

    var strFromDate = '';
    var strToDate = '';
    var month;
    var frommonth, tomonth, year;
    fromDate = new Date();
    toDate = new Date();

    ddlTime = document.getElementById(objGDDL);
    TimeVal = ddlTime.options[ddlTime.selectedIndex].value;

    if (TimeVal == DayToday) {
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayYesterday) {
        fromDate.setDate(fromDate.getDate() - 1);
        toDate.setDate(toDate.getDate() - 1);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayLast7day) {
        fromDate.setDate(fromDate.getDate() - 7);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayLast30day) {
        fromDate.setDate(fromDate.getDate() - 30);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayLast60day) {
        fromDate.setDate(fromDate.getDate() - 60);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayLast90day) {
        fromDate.setDate(fromDate.getDate() - 90);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == DayLast120day) {
        fromDate.setDate(fromDate.getDate() - 120);
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == WeekLast) {
        fromDate.setDate(fromDate.getDate() - (fromDate.getDay() + 7));
        toDate.setDate(toDate.getDate() - (toDate.getDay() + 1));
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == WeekCurrent) {
        fromDate.setDate(fromDate.getDate() - fromDate.getDay());
        toDate.setDate(toDate.getDate() + (6 - toDate.getDay()));
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == WeekLastCurrent) {
        fromDate.setDate(fromDate.getDate() - (fromDate.getDay() + 7));
        toDate.setDate(toDate.getDate() + (6 - toDate.getDay()));
        strFromDate = fromDate.getDate() + '-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = toDate.getDate() + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == MonthCurrent) {
        strFromDate = '1-' + getMonth(fromDate.getMonth() + 1) + '-' + fromDate.getFullYear();
        strToDate = daysInMonth(toDate.getMonth(), toDate.getFullYear()) + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == MonthLast) {
        strFromDate = '1-' + getMonth(fromDate.getMonth()) + '-' + fromDate.getFullYear();
        strToDate = daysInMonth(toDate.getMonth()-1, toDate.getFullYear()) + '-' + getMonth(toDate.getMonth()) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == MonthLastCurrent) {
        year = fromDate.getFullYear();
        month = fromDate.getMonth()
        if (fromDate.getMonth() == 0) {
            year = year - 1;
            month = 12;
        }
        strFromDate = '1-' + getMonth(month) + '-' + year;
        strToDate = daysInMonth(toDate.getMonth(), toDate.getFullYear()) + '-' + getMonth(toDate.getMonth() + 1) + '-' + toDate.getFullYear();
    }
    else if (TimeVal == CurrentQuarter) {
        month = fromDate.getMonth();
        year = fromDate.getFullYear();
        if (month == 0) {
            month = 12;
            year = year - 1;
        }
        frommonth = Math.floor(month / 3) * 3 + 1;
        tomonth = frommonth + 2;

        strFromDate = '1-' + getMonth(frommonth) + '-' + year;
        strToDate = daysInMonth(tomonth - 1, year) + '-' + getMonth(tomonth) + '-' + year;
    }
    else if (TimeVal == LastQuarter) {
        month = fromDate.getMonth();
        year = fromDate.getFullYear();

        if (month == 0) {
            month = 12;
            year = year - 1;
        }

        frommonth = Math.floor((month / 3) - 1) * 3 + 1;
        tomonth = frommonth + 2;
        strFromDate = '1-' + getMonth(frommonth) + '-' + year;
        strToDate = daysInMonth(tomonth - 1, year) + '-' + getMonth(tomonth) + '-' + year;
    }

    document.getElementById(objTxtFrom).value = strFromDate;    
    document.getElementById(objTxtTo).value = strToDate;
}
function daysInMonth(iMonth, iYear) {
    return 32 - new Date(iYear, iMonth, 32).getDate();
}
function getMonth(objMonth) {

    if (objMonth == 1) {
        return 'Jan';
    }
    else if (objMonth == 2) {
        return 'Feb';
    }
    else if (objMonth == 3) {
        return 'Mar';
    }
    else if (objMonth == 4) {
        return 'Apr';
    }
    else if (objMonth == 5) {
        return 'May';
    }
    else if (objMonth == 6) {
        return 'Jun';
    }
    else if (objMonth == 7) {
        return 'Jul';
    }
    else if (objMonth == 8) {
        return 'Aug';
    }
    else if (objMonth == 9) {
        return 'Sep';
    }
    else if (objMonth == 10) {
        return 'Oct';
    }
    else if (objMonth == 11) {
        return 'Nov';
    }
    else if (objMonth == 12) {
        return 'Dec';
    }
    return '';
}
