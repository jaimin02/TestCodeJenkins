$(document).bind("ajaxSend", function () {
    createDiv();
    $(".spinner").show();
}).bind("ajaxComplete", function () {
    removeDiv();
    $(".spinner").hide();
}).bind("ajaxError", function () {
    removeDiv();
    $(".spinner").hide();
});
//var textboxId = "";

if (typeof cornerstone === 'undefined') {
    cornerstone = {};
}

if (typeof cornerstoneTools === 'undefined') {
    cornerstoneTools = {
        referenceLines: {},
        orientation: {}
    };
}

var moveForce = 30; // max popup movement in pixels
var rotateForce = 20; // max popup rotation in deg
var isVisit = 'N';
var R_UserId = $("#hdniUserIdR1").val();
var R_UserType = $("#hdnvUserTypeCodeR1").val();
var R2_UserId = $("#hdniUserIdR2").val();
var R2_UserType = $("#hdnvUserTypeCodeR2").val();
var radiologist = $("#hdnUserType").val();
var radiologist2 = $("#hdnUserTypeR2").val();
var WorkFlowStageId = $("#hdnWorkFlowStageId").val();
var ActivityData = [];
var SubActivityData = [];
var SubActivityNameData = [];
var ActivityListForMeasurement = [];
var bttitle = '';
var subActivity = '';
$(document).mousemove(function (e) {
    var docX = $(document).width();
    var docY = $(document).height();

    var moveX = (e.pageX - docX / 2) / (docX / 2) * -moveForce;
    var moveY = (e.pageY - docY / 2) / (docY / 2) * -moveForce;

    var rotateY = (e.pageX / docX * rotateForce * 2) - rotateForce;
    var rotateX = -((e.pageY / docY * rotateForce * 2) - rotateForce);

    $('.popup')
        .css('left', moveX + 'px')
        .css('top', moveY + 'px')
        .css('transform', 'rotateX(' + rotateX + 'deg) rotateY(' + rotateY + 'deg)');
});

$(document).ajaxStart(function () {
    var query = window.location.search.substring(1);
    if (query == '' || query == "" || query == null) {

    }
    else {
        var ActivityID, NodeID, ActivityDef;
        var parms = query.split('&');
        for (var i = 0; i < parms.length; i++) {
            var pos = parms[i].indexOf('=');
            if (pos > 0) {
                var key = parms[i].substring(0, pos);
                var val = parms[i].substring(pos + 1);
                if (key == 'Uid') {
                    $("#hdnuserid").val(val);
                }
            }
        }
    }
    var userLoginDetails = {
        iUserId: $("#hdnuserid").val(),
        vIPAddress: $("#hdnIpAddress").val(),
        DATAOPMODE: 4
    }
    if (query == '' || query == "" || query == null) {
        var ajaxData = {
            url: ApiURL + "SetData/save_UserLoginDetails",
            type: 'POST',
            async: false,
            data: userLoginDetails,
            success: successuserLoginDetails,
            error: erroruserLoginDetails
        }
    }
    else {
        var ajaxData = {
            url: ApiURL + "SetData/save_UserLoginDetails",
            type: 'POST',
            async: false,
            data: userLoginDetails,
            success: successuserLoginDetails,
            error: erroruserLoginDetails
        }
    }

    $.ajax({
        url: ajaxData.url,
        type: ajaxData.type,
        data: ajaxData.data,
        async: ajaxData.async,
        success: ajaxData.success,
        error: ajaxData.error
    });

    function successuserLoginDetails(jsonData) {
        if (jsonData.length == 0) {
            logOut();
            var url = $("#RedirectToLogin").val();
            location.href = url;
        }
    }

    function erroruserLoginDetails() {
        AlertBox('error', 'MI', 'Error While Saving User Activity!')
    }

    function logOut() {
        var userLoginDetails = {
            iUserId: $("#hdnuserid").val(),
            vIPAddress: $("#hdnIpAddress").val(),
            DATAOPMODE: 3
        }

        var ajaxData = {
            url: ApiURL + "SetData/save_UserLoginDetails",
            type: 'POST',
            async: false,
            data: userLoginDetails,
            success: successlogOutUserDetails,
            error: errorlogOutUserDetails
        }

        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: ajaxData.data,
            async: ajaxData.async,
            success: ajaxData.success,
            error: ajaxData.error
        });

        function successlogOutUserDetails(jsonData) {
            var url = $("#RedirectToLogin").val();
            location.href = url;
        }

        function errorlogOutUserDetails() {
            AlertBox('error', 'MI', 'Error While LogOut User!')
        }
    }
});

$(document).ajaxComplete(function () {
});

$(document).ajaxError(function () {
});

var imageType;
var MIClearLesionDataFlag = false;
var UserId, HdrId, DtlId;
var lineLength;
var NTLMark;
var currentImageIndex = 0;
var imageIds = []
var element = $('#dicomImage').get(0);
var container = element.parentNode;
var loaded = false;
var stack = {
    currentImageIdIndex: 0,
    imageIds: imageIds
};
var toolSelector = $("#toolSelector");
var loopCheckbox = $("#loop");

// Initialize range input
var range, max, slice, currentValueSpan;
range = document.getElementById('slice-range');

// Set minimum and maximum value
range.min = 0;
range.step = 1;
range.max = imageIds.length - 1;

// Set current value
range.value = stack.currentImageIdIndex;

//var imageId = 'example://1';

var config = {
    // invert: true,
    minScale: 0.25,
    maxScale: 20.0,
    preventZoomOutsideImage: true
};

cornerstoneTools.toolStyle.setToolWidth(1.5);
cornerstoneTools.toolColors.setToolColor("#ffcc33");
cornerstoneTools.toolColors.setActiveColor("#0099ff");
cornerstoneTools.toolColors.setFillColor("#0099ff");

var magLevelRange = $("#magLevelRange")
magLevelRange.on("change", function () {
    var config = cornerstoneTools.magnify.getConfiguration();
    config.magnificationLevel = parseInt(magLevelRange.val(), 10);
});

var magSizeRange = $("#magSizeRange")
magSizeRange.on("change", function () {
    var config = cornerstoneTools.magnify.getConfiguration();
    config.magnifySize = parseInt(magSizeRange.val(), 10)
    var magnify = $(".magnifyTool").get(0);
    magnify.width = config.magnifySize;
    magnify.height = config.magnifySize;
});

var config = {
    magnifySize: parseInt(magSizeRange.val(), 10),
    magnificationLevel: parseInt(magLevelRange.val(), 10),
    drawAllMarkers: true
};

var loopCheckbox = $("#loop");

loopCheckbox.on('change', function () {
    var playClipToolData = cornerstoneTools.getToolState(element, 'playClip');
    playClipToolData.data[0].loop = loopCheckbox.is(":checked");
})

// Listen for changes to the viewport so we can update the text overlays in the corner
function onViewportUpdated(e) {
    var viewport = cornerstone.getViewport(e.target)
    $('#wwwc').text("WW/WC " + Math.round(viewport.voi.windowWidth) + "/" + Math.round(viewport.voi.windowCenter));
    $('#wwwc').css("color", "red")
    $('#zoomText').text("Zoom: " + viewport.scale.toFixed(2));
    $('#zoomText').css("color", "red")

    
    //DicomDataAnnotation = ""
    for (var vd = 0 ; vd < DicomDataAnnotation.length ; vd++) {
        var Annotation = DicomDataAnnotation[vd];

        var enabledImage = cornerstone.getEnabledElement(element);
        var dicomFileName = enabledImage.image.imageId.toUpperCase().substring(enabledImage.image.imageId.toUpperCase().lastIndexOf('/') + 1, enabledImage.image.imageId.toUpperCase().indexOf('?'));
        var dicomServerPath = enabledImage.image.imageId.toUpperCase().substring(0, enabledImage.image.imageId.toUpperCase().lastIndexOf('?'));

        if (Annotation.vServerPath.toUpperCase() == dicomServerPath.toUpperCase()) {
            if (Annotation.vFileName.toUpperCase() == dicomFileName.toUpperCase()) {
                if (Annotation.vAnnotationType.toUpperCase() == "LENGTH") {
                    // we have tool data for this element - iterate over each one and draw it
                    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    context.setTransform(1, 0, 0, 1, 0, 0);
                    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    var config = cornerstoneTools.length.getConfiguration();

                    context.save();

                    // configurable shadow
                    if (config && config.shadow) {
                        context.shadowColor = config.shadowColor || '#000000';
                        context.shadowOffsetX = config.shadowOffsetX || 1;
                        context.shadowOffsetY = config.shadowOffsetY || 1;
                    }

                    //var AnnotationData = JSON.parse(Annotation.nvDicomAnnotation);
                    //for (var dv = 0 ; dv < AnnotationData.length ; dv++) {
                    //var data = AnnotationData[dv];

                    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);

                    var color = cornerstoneTools.toolColors.getColorIfActive(data.active);

                    // Get the handle positions in canvas coordinates
                    var handleStartCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], data.handles.start);
                    var handleEndCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], data.handles.end);

                    // Draw the measurement line
                    context.beginPath();
                    context.strokeStyle = color;
                    context.lineWidth = lineWidth;
                    context.moveTo(handleStartCanvas.x, handleStartCanvas.y);
                    context.lineTo(handleEndCanvas.x, handleEndCanvas.y);
                    context.stroke();

                    // Draw the handles
                    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    // Draw the text
                    context.fillStyle = color;

                    // Set rowPixelSpacing and columnPixelSpacing to 1 if they are undefined (or zero)
                    var dx = (data.handles.end.x - data.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (data.handles.end.y - data.handles.start.y) * (eventData.image.rowPixelSpacing || 1);

                    // Calculate the length, and create the text variable with the millimeters or pixels suffix            
                    var length = Math.sqrt(dx * dx + dy * dy);

                    // Set the length text suffix depending on whether or not pixelSpacing is available
                    var suffix = ' mm';
                    if (!eventData.image.rowPixelSpacing || !eventData.image.columnPixelSpacing) {
                        //suffix = ' pixels';
                        suffix = ' mm';
                    }

                    // Store the length measurement text                    
                    if (data.text == "" || data.text == undefined) {
                        var text = '' + length.toFixed(2) + suffix
                    }
                    else {
                        var text = '' + length.toFixed(2) + suffix + ' ' + data.text;
                    }


                    // Place the length measurement text next to the right-most handle
                    var fontSize = cornerstoneTools.textStyle.getFontSize();
                    var textCoords = {
                        x: Math.max(handleStartCanvas.x, handleEndCanvas.x),
                    };

                    // Depending on which handle has the largest x-value, 
                    // set the y-value for the text box
                    if (textCoords.x === handleStartCanvas.x) {
                        textCoords.y = handleStartCanvas.y;
                    } else {
                        textCoords.y = handleEndCanvas.y;
                    }

                    // Move the textbox slightly to the right and upwards
                    // so that it sits beside the length tool handle
                    textCoords.x += 10;
                    textCoords.y -= fontSize / 2 + 7;
                    
                    // Draw the textbox
                    cornerstoneTools.drawTextBox(context, text, textCoords.x, textCoords.y, color);

                    //}
                }

                else if (Annotation.vAnnotationType.toUpperCase() == "RECTANGLEROI") {
                    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    context.setTransform(1, 0, 0, 1, 0, 0);
                    context.save();

                    //activation color
                    var color;
                    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    var font = cornerstoneTools.textStyle.getFont();
                    var fontHeight = cornerstoneTools.textStyle.getFontSize();

                    //var AnnotationData = JSON.parse(Annotation.nvDicomAnnotation);

                    //for (var dv = 0 ; dv < AnnotationData.length ; dv++) {
                    //var data = AnnotationData[dv];
                    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);

                    color = cornerstoneTools.toolColors.getToolColor();

                    // draw the rectangle
                    var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
                    var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

                    var widthCanvas = Math.abs(handleStartCanvas.x - handleEndCanvas.x);
                    var heightCanvas = Math.abs(handleStartCanvas.y - handleEndCanvas.y);
                    var leftCanvas = Math.min(handleStartCanvas.x, handleEndCanvas.x);
                    var topCanvas = Math.min(handleStartCanvas.y, handleEndCanvas.y);
                    var centerX = (handleStartCanvas.x + handleEndCanvas.x) / 2;
                    var centerY = (handleStartCanvas.y + handleEndCanvas.y) / 2;

                    context.beginPath();
                    context.strokeStyle = color;
                    context.lineWidth = lineWidth;
                    context.rect(leftCanvas, topCanvas, widthCanvas, heightCanvas);
                    context.stroke();

                    // draw the handles
                    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    // Calculate the mean, stddev, and area
                    // TODO: calculate this in web worker for large pixel counts...

                    var width = Math.abs(data.handles.start.x - data.handles.end.x);
                    var height = Math.abs(data.handles.start.y - data.handles.end.y);
                    var left = Math.min(data.handles.start.x, data.handles.end.x);
                    var topData = Math.min(data.handles.start.y, data.handles.end.y);
                    var pixels = cornerstone.getPixels(eventData.element, left, topData, width, height);

                    var ellipse = {
                        left: left,
                        top: topData,
                        width: width,
                        height: height
                    };

                    var meanStdDev = calculateMeanStdDev(pixels, ellipse);
                    var area = (width * eventData.image.columnPixelSpacing) * (height * eventData.image.rowPixelSpacing);
                    var areaText = 'Area: ' + area.toFixed(2) + ' mm^2';
                    var NTL = NTLMark;

                    // Draw text
                    context.font = font;

                    var textSize = context.measureText(area);

                    var textX = centerX < (eventData.image.columns / 2) ? centerX + (widthCanvas / 2) : centerX - (widthCanvas / 2) - textSize.width;
                    var textY = centerY < (eventData.image.rows / 2) ? centerY + (heightCanvas / 2) : centerY - (heightCanvas / 2);

                    context.fillStyle = color;

                    if (data.text == "" || data.text == undefined) {
                        cornerstoneTools.drawTextBox(context, 'Mean: ' + meanStdDev.mean.toFixed(2), textX, textY - fontHeight - 5, color);
                        cornerstoneTools.drawTextBox(context, 'StdDev: ' + meanStdDev.stdDev.toFixed(2), textX, textY, color);
                    }
                    else {
                        cornerstoneTools.drawTextBox(context, 'NTL: ' + data.text, textX, textY - fontHeight - 25, color);
                        cornerstoneTools.drawTextBox(context, 'Mean: ' + meanStdDev.mean.toFixed(2), textX, textY - fontHeight - 5, color);
                        cornerstoneTools.drawTextBox(context, 'StdDev: ' + meanStdDev.stdDev.toFixed(2), textX, textY, color);
                    }

                    context.restore();
                    //}

                    function calculateMeanStdDev(sp, ellipse) {
                        
                        // TODO: Get a real statistics library here that supports large counts

                        var sum = 0;
                        var sumSquared = 0;
                        var count = 0;
                        var index = 0;

                        for (var y = ellipse.top; y < ellipse.top + ellipse.height; y++) {
                            for (var x = ellipse.left; x < ellipse.left + ellipse.width; x++) {
                                sum += sp[index];
                                sumSquared += sp[index] * sp[index];
                                count++;
                                index++;
                            }
                        }

                        if (count === 0) {
                            return {
                                count: count,
                                mean: 0.0,
                                variance: 0.0,
                                stdDev: 0.0
                            };
                        }

                        var mean = sum / count;
                        var variance = sumSquared / count - mean * mean;

                        return {
                            count: count,
                            mean: mean,
                            variance: variance,
                            stdDev: Math.sqrt(variance)
                        };
                    }
                }

                    //Logic from Simple Angle for Ortho

                    //else if (Annotation.vAnnotationType.toUpperCase() == "ORTHO") {
                    //    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    //    context.setTransform(1, 0, 0, 1, 0, 0);

                    //    //activation color 
                    //    var color;
                    //    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    //    var font = cornerstoneTools.textStyle.getFont();
                    //    var config = cornerstoneTools.ortho.getConfiguration();

                    //    if (config && config.shadow) {
                    //        context.shadowColor = config.shadowColor || '#000000';
                    //        context.shadowOffsetX = config.shadowOffsetX || 1;
                    //        context.shadowOffsetY = config.shadowOffsetY || 1;
                    //    }

                    //    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    //    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);

                    //    color = cornerstoneTools.toolColors.getToolColor();

                    //    // draw the line
                    //    context.beginPath();
                    //    context.strokeStyle = color;
                    //    context.lineWidth = lineWidth;

                    //    var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
                    //    var handleMiddleCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.middle);

                    //    context.moveTo(handleStartCanvas.x, handleStartCanvas.y);
                    //    context.lineTo(handleMiddleCanvas.x, handleMiddleCanvas.y);
                    //    context.stroke();

                    //    var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

                    //    //context.lineTo(handleEndCanvas.x, handleEndCanvas.y);
                    //    context.stroke();

                    //    // draw the handles
                    //    //cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    //    //Perpendicular Angle

                    //    var midX = (handleStartCanvas.x + handleMiddleCanvas.x) / 2
                    //    var midY = (handleStartCanvas.y + handleMiddleCanvas.y) / 2

                    //    var dx = (handleMiddleCanvas.x - handleStartCanvas.x) / 2
                    //    var dy = (handleMiddleCanvas.y - handleStartCanvas.y) / 2

                    //    context.moveTo(midX, midY);
                    //    context.lineTo(midX + dy, midY - dx);
                    //    context.stroke();
                    //    context.lineTo(midX - dy, midY + dx);
                    //    context.stroke();

                    //    // Draw the text
                    //    context.fillStyle = color;

                    //    // Default to isotropic pixel size, update suffix to reflect this
                    //    var columnPixelSpacing = eventData.image.columnPixelSpacing || 1;
                    //    var rowPixelSpacing = eventData.image.rowPixelSpacing || 1;
                    //    var suffix = '';
                    //    if (!eventData.image.rowPixelSpacing || !eventData.image.columnPixelSpacing) {
                    //        suffix = ' (isotropic)';
                    //    }

                    //    var sideA = {
                    //        x: (Math.ceil(data.handles.middle.x) - Math.ceil(data.handles.start.x)) * columnPixelSpacing,
                    //        y: (Math.ceil(data.handles.middle.y) - Math.ceil(data.handles.start.y)) * rowPixelSpacing
                    //    };

                    //    var sideB = {
                    //        x: (Math.ceil(data.handles.end.x) - Math.ceil(data.handles.middle.x)) * columnPixelSpacing,
                    //        y: (Math.ceil(data.handles.end.y) - Math.ceil(data.handles.middle.y)) * rowPixelSpacing
                    //    };

                    //    var sideC = {
                    //        x: (Math.ceil(data.handles.end.x) - Math.ceil(data.handles.start.x)) * columnPixelSpacing,
                    //        y: (Math.ceil(data.handles.end.y) - Math.ceil(data.handles.start.y)) * rowPixelSpacing
                    //    };

                    //    //var sideALength = length(sideA);
                    //    //var sideBLength = length(sideB);
                    //    //var sideCLength = length(sideC);

                    //    // Cosine law
                    //    //var angle = Math.acos((Math.pow(sideALength, 2) + Math.pow(sideBLength, 2) - Math.pow(sideCLength, 2)) / (2 * sideALength * sideBLength));
                    //    //angle = angle * (180 / Math.PI);

                    //    //Drow the Perpendicular Line that is always at 90 degree

                    //    //var rAngle = cornerstoneTools.roundToDecimal(angle, 2);                    
                    //    var rAngle = 90;

                    //    if (rAngle) {
                    //        var str = '00B0'; // degrees symbol
                    //        var text = rAngle.toString() + String.fromCharCode(parseInt(str, 16)) + suffix;

                    //        //var distance = 15;
                    //        var distance = 5;

                    //        var textX = handleMiddleCanvas.x + distance;
                    //        var textY = handleMiddleCanvas.y + distance;

                    //        context.font = font;
                    //        var textWidth = context.measureText(text).width;

                    //        if ((handleMiddleCanvas.x - handleStartCanvas.x) < 0) {
                    //            textX = handleMiddleCanvas.x - distance - textWidth - 10;
                    //        } else {
                    //            textX = handleMiddleCanvas.x + distance;
                    //        }

                    //        textY = handleMiddleCanvas.y;
                    
                    //        cornerstoneTools.drawTextBox(context, text, textX, textY, color);
                    //    }
                    //    context.restore();
                    //}

                    //Logic from Line For Ortho

                else if (Annotation.vAnnotationType.toUpperCase() == "ORTHO") {
                    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    context.setTransform(1, 0, 0, 1, 0, 0);

                    //activation color 
                    var color;
                    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    var font = cornerstoneTools.textStyle.getFont();
                    var config = cornerstoneTools.ortho.getConfiguration();

                    if (config && config.shadow) {
                        context.shadowColor = config.shadowColor || '#000000';
                        context.shadowOffsetX = config.shadowOffsetX || 1;
                        context.shadowOffsetY = config.shadowOffsetY || 1;
                    }

                    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);
                    color = cornerstoneTools.toolColors.getToolColor();

                    // Get the handle positions in canvas coordinates
                    var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
                    var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

                    // Draw the measurement line
                    context.beginPath();
                    context.strokeStyle = color;
                    context.lineWidth = lineWidth;
                    context.moveTo(handleStartCanvas.x, handleStartCanvas.y);
                    context.lineTo(handleEndCanvas.x, handleEndCanvas.y);
                    context.stroke();

                    //Perpendicular Angle                   

                    var vhandleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.orthoStart);
                    var vhandleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.orthoEnd);

                    context.moveTo(vhandleStartCanvas.x, vhandleStartCanvas.y);
                    context.lineTo(vhandleEndCanvas.x, vhandleEndCanvas.y);
                    context.stroke();
                    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    // Draw the text
                    context.fillStyle = color;

                    // Set rowPixelSpacing and columnPixelSpacing to 1 if they are undefined (or zero)
                    var dx = (data.handles.end.x - data.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (data.handles.end.y - data.handles.start.y) * (eventData.image.rowPixelSpacing || 1);

                    var Wdx = (data.handles.orthoEnd.x - data.handles.orthoStart.x) * (eventData.image.columnPixelSpacing || 1);
                    var Wdy = (data.handles.orthoEnd.y - data.handles.orthoStart.y) * (eventData.image.rowPixelSpacing || 1);

                    // Calculate the length, and create the text variable with the millimeters or pixels suffix            
                    var length = Math.sqrt(dx * dx + dy * dy);
                    var Wlength = Math.sqrt(Wdx * Wdx + Wdy * Wdy);

                    // Set the length text suffix depending on whether or not pixelSpacing is available
                    var suffix = ' mm';
                    if (!eventData.image.rowPixelSpacing || !eventData.image.columnPixelSpacing) {
                        //suffix = ' pixels';
                        suffix = ' mm';
                    }

                    // Store the length measurement text                     
                    if (data.text == "" || data.text == undefined) {
                        //var text = '' + length.toFixed(2) + suffix
                        var str = '00B0'; // degrees symbol
                        var text = 90 + String.fromCharCode(parseInt(str, 16));
                        var lengthtext = 'L: ' + length.toFixed(2) + suffix;
                        var Wlengthtext = 'W: ' + Wlength.toFixed(2) + suffix;
                        var dataText = ''
                    }
                    else {
                        //var text = '' + length.toFixed(2) + suffix + ' ' + data.text;                
                        var str = '00B0'; // degrees symbol
                        var text = '90' + String.fromCharCode(parseInt(str, 16));
                        var lengthtext = 'L: ' + length.toFixed(2) + suffix;
                        var Wlengthtext = 'W: ' + Wlength.toFixed(2) + suffix + ' ' + data.text;
                        var dataText = data.text;
                    }

                    // Place the length measurement text next to the right-most handle
                    var fontSize = cornerstoneTools.textStyle.getFontSize();
                    var textCoords = {
                        x: Math.max(handleStartCanvas.x, handleEndCanvas.x),
                    };

                    // Depending on which handle has the largest x-value, 
                    // set the y-value for the text box
                    if (textCoords.x === handleStartCanvas.x) {
                        textCoords.y = handleStartCanvas.y;
                    } else {
                        textCoords.y = handleEndCanvas.y;
                    }

                    // Move the textbox slightly to the right and upwards
                    // so that it sits beside the length tool handle
                    textCoords.x += 10;
                    textCoords.y -= fontSize / 2 + 7;


                    //-------rinkal
                    var distance = 10;


                    var textX = handleStartCanvas.x + distance;
                    var textY = handleStartCanvas.y + distance;

                    context.font = font;
                    var textWidth = context.measureText(text).width;

                    if ((handleEndCanvas.x - handleStartCanvas.x) < 0) {
                        textX = handleStartCanvas.x - distance - textWidth - 10;
                    } else {
                        textX = handleStartCanvas.x + distance;
                    }

                    textY = handleStartCanvas.y;
                    cornerstoneTools.drawTextBox(context, text, textX, textY, color);

                    // Draw the textbox
                    cornerstoneTools.drawTextBox(context, lengthtext, textCoords.x + 1, textCoords.y + 1, color);
                    cornerstoneTools.drawTextBox(context, Wlengthtext, textCoords.x + 1, textCoords.y + 15, color);
                    //cornerstoneTools.drawTextBox(context, dataText, textCoords.x + 1, textCoords.y + 30, color);
                }


                else if (Annotation.vAnnotationType.toUpperCase() == "PERPENDICULAR") {
                    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    context.setTransform(1, 0, 0, 1, 0, 0);

                    //activation color 
                    var color;
                    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    var font = cornerstoneTools.textStyle.getFont();
                    var config = cornerstoneTools.ortho.getConfiguration();

                    if (config && config.shadow) {
                        context.shadowColor = config.shadowColor || '#000000';
                        context.shadowOffsetX = config.shadowOffsetX || 1;
                        context.shadowOffsetY = config.shadowOffsetY || 1;
                    }

                    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);
                    color = cornerstoneTools.toolColors.getToolColor();

                    // Get the handle positions in canvas coordinates
                    var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
                    var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

                    // Draw the measurement line
                    context.beginPath();
                    context.strokeStyle = color;
                    context.lineWidth = lineWidth;
                    context.moveTo(handleStartCanvas.x, handleStartCanvas.y);
                    context.lineTo(handleEndCanvas.x, handleEndCanvas.y);
                    context.stroke();

                    // Draw the handles
                    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    // Draw the text
                    context.fillStyle = color;

                    // Set rowPixelSpacing and columnPixelSpacing to 1 if they are undefined (or zero)
                    var dx = (data.handles.end.x - data.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (data.handles.end.y - data.handles.start.y) * (eventData.image.rowPixelSpacing || 1);

                    // Calculate the length, and create the text variable with the millimeters or pixels suffix            
                    var length = Math.sqrt(dx * dx + dy * dy);

                    // Set the length text suffix depending on whether or not pixelSpacing is available
                    var suffix = ' mm';
                    if (!eventData.image.rowPixelSpacing || !eventData.image.columnPixelSpacing) {
                        //suffix = ' pixels';
                        suffix = ' mm';
                    }

                    //---------rinkal
                    var newText = "";
                    var newContext = "";
                    var newX = "";
                    var newY = "";
                    var newColor = "";

                    // Store the length measurement text                    
                    if (data.text == "" || data.text == undefined) {
                        var text = '' + length.toFixed(2) + suffix
                    }
                    else {
                        var text = '' + length.toFixed(2) + suffix + ' ' + data.text;
                    }

                    newText = text
                    newContext = context
                    newX = textCoords.x
                    newY = textCoords.y
                    newColor = color




                    // Place the length measurement text next to the right-most handle
                    var fontSize = cornerstoneTools.textStyle.getFontSize();
                    var textCoords = {
                        x: Math.max(handleStartCanvas.x, handleEndCanvas.x),
                    };

                    // Depending on which handle has the largest x-value, 
                    // set the y-value for the text box
                    if (textCoords.x === handleStartCanvas.x) {
                        textCoords.y = handleStartCanvas.y;
                    } else {
                        textCoords.y = handleEndCanvas.y;
                    }

                    //------rinkal
                    if (data.handles.end.perpendicular == true && data.handles.end.perpendicular == true) {
                        //var str = '00B0'; // degrees symbol
                        var text = 90 + String.fromCharCode(parseInt(str, 16));
                    }
                    else {
                        var text = '';
                    }

                    var distance = 10;


                    var textX = handleStartCanvas.x + distance;
                    var textY = handleStartCanvas.y + distance;

                    context.font = font;
                    var textWidth = context.measureText(text).width;

                    if ((handleEndCanvas.x - handleStartCanvas.x) < 0) {
                        textX = handleStartCanvas.x - distance - textWidth - 10;
                    } else {
                        textX = handleStartCanvas.x + distance;
                    }
                    textY = handleEndCanvas.y;
                    //// Move the textbox slightly to the right and upwards
                    //// so that it sits beside the length tool handle
                    //textCoords.x += 10;
                    //textCoords.y -= fontSize / 2 + 7;
                    // Draw the textbox

                    cornerstoneTools.drawTextBox(context, text, textX, textY, color);

                    //---rinkal
                    cornerstoneTools.drawTextBox(newContext, newText, textCoords.x, textCoords.y, newColor);

                }
                    //--- Added by Vijay Rathod , For display ellipticalroi Tools data
                else if (Annotation.vAnnotationType.toUpperCase() == "ELLIPTICALROI") {
                    var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
                    context.setTransform(1, 0, 0, 1, 0, 0);

                    //activation color 
                    var color;
                    var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
                    var font = cornerstoneTools.textStyle.getFont();
                    var fontHeight = cornerstoneTools.textStyle.getFontSize();
                    var config = cornerstoneTools.ellipticalRoi.getConfiguration();

                    if (config && config.shadow) {
                        context.shadowColor = config.shadowColor || '#000000';
                        context.shadowOffsetX = config.shadowOffsetX || 1;
                        context.shadowOffsetY = config.shadowOffsetY || 1;
                    }

                    var data = JSON.parse(Annotation.nvDicomAnnotation);
                    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);
                    color = cornerstoneTools.toolColors.getToolColor();

                    // Get the handle positions in canvas coordinates
                    var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
                    var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

                    var widthCanvas = Math.abs(handleStartCanvas.x - handleEndCanvas.x);
                    var heightCanvas = Math.abs(handleStartCanvas.y - handleEndCanvas.y);
                    var leftCanvas = Math.min(handleStartCanvas.x, handleEndCanvas.x);
                    var topCanvas = Math.min(handleStartCanvas.y, handleEndCanvas.y);
                    var centerX = (handleStartCanvas.x + handleEndCanvas.x) / 2;
                    var centerY = (handleStartCanvas.y + handleEndCanvas.y) / 2;

                    context.beginPath();
                    context.strokeStyle = color;
                    context.lineWidth = lineWidth;
                    cornerstoneTools.drawEllipse(context, leftCanvas, topCanvas, widthCanvas, heightCanvas);
                    context.closePath();

                    // draw the handles
                    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

                    context.font = font;

                    var textX,
                        textY,
                        area,
                        meanStdDev;

                    if (!data.invalidated) {
                        textX = data.textX;
                        textY = data.textY;
                        meanStdDev = data.meanStdDev;
                        area = data.area;
                    } else {
                        // TODO: calculate this in web worker for large pixel counts...
                        var width = Math.abs(data.handles.start.x - data.handles.end.x);
                        var height = Math.abs(data.handles.start.y - data.handles.end.y);
                        var left = Math.min(data.handles.start.x, data.handles.end.x);
                        var top = Math.min(data.handles.start.y, data.handles.end.y);

                        var pixels = cornerstone.getPixels(eventData.element, left, top, width, height);

                        var ellipse = {
                            left: left,
                            top: top,
                            width: width,
                            height: height
                        };

                        // Calculate the mean, stddev, and area
                        meanStdDev = calculateMeanStdDev(pixels, ellipse);
                        area = Math.PI * (width * eventData.image.columnPixelSpacing / 2) * (height * eventData.image.rowPixelSpacing / 2);

                        if (!isNaN(area)) {
                            data.area = area;
                        }

                        if (!isNaN(meanStdDev.mean) && !isNaN(meanStdDev.stdDev)) {
                            data.meanStdDev = meanStdDev;
                        }
                    }

                    // Draw text

                    var areaText,
                        areaTextWidth = 0;
                    if (area !== undefined) {
                        areaText = 'Area: ' + area.toFixed(2) + ' mm' + String.fromCharCode(178);
                        areaTextWidth = context.measureText(areaText).width;
                    }

                    var meanText = 'Mean: ' + meanStdDev.mean.toFixed(2);
                    var meanTextWidth = context.measureText(meanText).width;

                    var stdDevText = 'StdDev: ' + meanStdDev.stdDev.toFixed(2);
                    var stdDevTextWidth = context.measureText(stdDevText).width;

                    var longestTextWidth = Math.max(meanTextWidth, areaTextWidth, stdDevTextWidth);

                    textX = centerX < (eventData.image.columns / 2) ? centerX + (widthCanvas / 2) + longestTextWidth : centerX - (widthCanvas / 2) - longestTextWidth - 15;
                    textY = centerY < (eventData.image.rows / 2) ? centerY + (heightCanvas / 2) : centerY - (heightCanvas / 2);
                    
                    context.fillStyle = color;
                    if (meanStdDev) {
                        cornerstoneTools.drawTextBox(context, meanText, textX, textY - fontHeight - 5, color);
                        cornerstoneTools.drawTextBox(context, stdDevText, textX, textY, color);
                    }

                    // Char code 178 is a superscript 2 for mm^2
                    if (area !== undefined && !isNaN(area)) {
                        cornerstoneTools.drawTextBox(context, areaText, textX, textY + fontHeight + 5, color);
                    }


                    if (data.text != "" && data.text != undefined) {
                        cornerstoneTools.drawTextBox(context, 'NL: ' + data.text, textX, textY + fontHeight + 25, color);
                    }

                    context.restore();
                }
                //END OF ELLIPTICALROI
            }
        }
    }

    

    ///////////////////////////////////////************************************************///////////////////////////////
    //************************************To check the Static Annotation Marking for Length*****************************//
    ///////////////////////////////////////********************************************///////////////////////////////////


    //// if we have no toolData for this element, return immediately as there is nothing to do
    //// var toolData = cornerstoneTools.getToolState(e.currentTarget, toolType);

    //toolType = "length"
    //// var toolData = cornerstoneTools.getToolState($("#dicomImage")[0], toolType);	
    //var toolData, handles;
    //var data = []
    //var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);

    //handles = {
    //    start: { x: 69.96899224806202, y: 259.968992248062, highlight: true, active: false },
    //    end: { x: 262.4651162790698, y: 386.9767441860465, highlight: true, active: false }
    //}

    //handles2 = {
    //    start: { x: 89.96899224806202, y: 359.968992248062, highlight: true, active: false },
    //    end: { x: 362.4651162790698, y: 486.9767441860465, highlight: true, active: false }
    //}


    //var data1 = { active: true, handles: handles, text: "TL1", visible: true }

    //var data2 = { active: true, handles: handles2, text: "TL2", visible: true }

    //data.push(data1)
    //data.push(data2)

    //toolData = { data: data }
    ////toolData.data.push(data)
    //if (!toolData) {
    //    //return;
    //}    

    //var MIDicomAnnotation = {
    //    vFileName: "a",
    //    vServerPath: "b",
    //    nvDicomAnnotation: JSON.stringify(toolData.data),
    //    UserID: $("#hdnuserid").val()
    //}

    
    ////$.ajax({
    ////    url: WebURL + "MIDicomViewer/SubmitDicomAnnotation",
    ////    type: 'POST',
    ////    //crossDomain: true,
    ////    data: MIDicomAnnotation,
    ////    //contentType: 'application/json; charset=utf-8',
    ////    //dataType: 'text',
    ////    async: false,
    ////    success: function (data) {
    ////        var v = data;
    ////        //var result = JSON.parse(data)
    ////    },
    ////    error: function (e) {
    ////        var error = e
    ////    }
    ////});

    //// we have tool data for this element - iterate over each one and draw it
    ////var context = eventData.canvasContext.canvas.getContext('2d');
    //var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');

    //context.setTransform(1, 0, 0, 1, 0, 0);

    //// var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
    //var lineWidth = 1
    //var config = cornerstoneTools.length.getConfiguration();

    // for (var i = 0; i < toolData.data.length; i++) {
    ////for (var i = 0; i < 1; i++) {
    //    context.save();

    //    // configurable shadow
    //    if (config && config.shadow) {
    //        context.shadowColor = config.shadowColor || '#000000';
    //        context.shadowOffsetX = config.shadowOffsetX || 1;
    //        context.shadowOffsetY = config.shadowOffsetY || 1;
    //    }

    //    var data = toolData.data[i];
    //    // var handles = {	
    //    // start : {x: 69.96899224806202, y: 259.968992248062, highlight: true, active: false},
    //    // end : {x: 262.4651162790698, y: 386.9767441860465, highlight: true, active: true}
    //    // }

    //    // data = {
    //    // active:true,
    //    // handles: handles,
    //    // text:"TL1",
    //    // visible:true
    //    // }

    //    //start = {x: 69.96899224806202, y: 259.968992248062, highlight: true, active: false}
    //    //end = {x: 262.4651162790698, y: 386.9767441860465, highlight: true, active: true}

    //    //var handles = start
    //    //handles += end

    //    //var color = cornerstoneTools.toolColors.getColorIfActive(data.active);
    //    //var color = "#0099ff"
    //    var color = cornerstoneTools.toolColors.getColorIfActive(data.active);

    //    // Get the handle positions in canvas coordinates
    //    //var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
    //    // var handleStartCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], start);
    //    var handleStartCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], data.handles.start);

    //    // var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);
    //    // var handleEndCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], end);
    //    var handleEndCanvas = cornerstone.pixelToCanvas($("#dicomImage")[0], data.handles.end);

    //    // Draw the measurement line
    //    context.beginPath();
    //    context.strokeStyle = color;
    //    context.lineWidth = lineWidth;
    //    context.moveTo(handleStartCanvas.x, handleStartCanvas.y);
    //    context.lineTo(handleEndCanvas.x, handleEndCanvas.y);
    //    context.stroke();

    //    // Draw the handles
    //    // cornerstoneTools.drawHandles(context, eventData, data.handles, color);

    //    cornerstoneTools.drawHandles(context, eventData, data.handles, color);

    //    // Draw the text
    //    context.fillStyle = color;

    //    // Set rowPixelSpacing and columnPixelSpacing to 1 if they are undefined (or zero)
    //    var dx = (data.handles.end.x - data.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
    //    var dy = (data.handles.end.y - data.handles.start.y) * (eventData.image.rowPixelSpacing || 1);

    //    // Calculate the length, and create the text variable with the millimeters or pixels suffix            
    //    var length = Math.sqrt(dx * dx + dy * dy);

    //    // Set the length text suffix depending on whether or not pixelSpacing is available
    //    var suffix = ' mm';
    //    if (!eventData.image.rowPixelSpacing || !eventData.image.columnPixelSpacing) {
    //        //suffix = ' pixels';//Changed By Vivek Patel
    //        suffix = ' mm';
    //    }

    //    // Store the length measurement text // Chaged by Vivek Patel
    //    //var text = '' + length.toFixed(2) + suffix;
    //    //var text = '' + length.toFixed(2) + suffix + ' ' + NTLMark;
    //    //console.log(data.text)
    //    if (data.text == "" || data.text == undefined) {
    //        var text = '' + length.toFixed(2) + suffix
    //    }
    //    else {
    //        var text = '' + length.toFixed(2) + suffix + ' ' + data.text;
    //    }


    //    // Place the length measurement text next to the right-most handle
    //    var fontSize = cornerstoneTools.textStyle.getFontSize();
    //    var textCoords = {
    //        x: Math.max(handleStartCanvas.x, handleEndCanvas.x),
    //    };

    //    // Depending on which handle has the largest x-value, 
    //    // set the y-value for the text box
    //    if (textCoords.x === handleStartCanvas.x) {
    //        textCoords.y = handleStartCanvas.y;
    //    } else {
    //        textCoords.y = handleEndCanvas.y;
    //    }

    //    // Move the textbox slightly to the right and upwards
    //    // so that it sits beside the length tool handle
    //    textCoords.x += 10;
    //    textCoords.y -= fontSize / 2 + 7;
    
    //    // Draw the textbox
    //    cornerstoneTools.drawTextBox(context, text, textCoords.x, textCoords.y, color);

    //    /////////////////////////////////context.restore();

    //    //Added By Vivek Patel
    //    //var sizecontrol = document.getElementsByClassName('25041');
    //    //if (sizecontrol.length != 0) {
    //    //    for (var v = 0 ; v < sizecontrol.length ; v++) {
    //    //        if (sizecontrol[v].value == '' || sizecontrol[v].value == "" || sizecontrol[v].value == null) {
    //    //            sizecontrol[v].value = lineLength;
    //    //            sizecontrol[v].disabled = true;
    //    //            return true;
    //    //        }
    //    //    }
    //    //}
    // }


    ///////////////////////////////////////*******************************************************/////////////////////////
    //************************************To check the Static Annotation Marking for Ractangle ROI**********************//
    ///////////////////////////////////////********************************************///////////////////////////////////


    
    // var toolData;
    ////var context = eventData.canvasContext.canvas.getContext('2d');
    // var context = cornerstone.getEnabledElement($("#dicomImage")[0]).canvas.getContext('2d');
    // context.setTransform(1, 0, 0, 1, 0, 0);


    // var toolData, handles;
    // var data = []
    // var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);


    // handles = {
    //     start: { x: 175.1472868217054, y: 210.35658914728683, highlight: true, active: false },
    //     end: { x: 332.91472868217056, y: 268.8992248062016, highlight: true, active: false }
    // }

    // handles2 = {
    //     start: { x: 275.1472868217054, y: 310.35658914728683, highlight: true, active: false },
    //     end: { x: 432.91472868217056, y: 368.8992248062016, highlight: true, active: false }
    // }


    // var data1 = { active: true, handles: handles, text: "NTL1", visible: true }

    // var data2 = { active: true, handles: handles2, text: "NTL2", visible: true }

    // data.push(data1)
    // data.push(data2)

    // toolData = { data: data }

    ////activation color 

    // var color;
    // var lineWidth = cornerstoneTools.toolStyle.getToolWidth();
    // var font = cornerstoneTools.textStyle.getFont();
    // var fontHeight = cornerstoneTools.textStyle.getFontSize();

    // for (var i = 0; i < toolData.data.length; i++) {
    //     //	for (var i = 0; i < 1; i++) {
    //     context.save();

    //     var data = toolData.data[i];
    //     //var data = toolData.data;

    //     //differentiate the color of activation tool
    //     // if (data.active) {
    //     // color = cornerstoneTools.toolColors.getActiveColor();
    //     // } else {
    //     // color = cornerstoneTools.toolColors.getToolColor();
    //     // }

    //     color = cornerstoneTools.toolColors.getToolColor();


    //     // draw the rectangle
    //     var handleStartCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.start);
    //     var handleEndCanvas = cornerstone.pixelToCanvas(eventData.element, data.handles.end);

    //     var widthCanvas = Math.abs(handleStartCanvas.x - handleEndCanvas.x);
    //     var heightCanvas = Math.abs(handleStartCanvas.y - handleEndCanvas.y);
    //     var leftCanvas = Math.min(handleStartCanvas.x, handleEndCanvas.x);
    //     var topCanvas = Math.min(handleStartCanvas.y, handleEndCanvas.y);
    //     var centerX = (handleStartCanvas.x + handleEndCanvas.x) / 2;
    //     var centerY = (handleStartCanvas.y + handleEndCanvas.y) / 2;

    //     context.beginPath();
    //     context.strokeStyle = color;
    //     context.lineWidth = lineWidth;
    //     context.rect(leftCanvas, topCanvas, widthCanvas, heightCanvas);
    //     context.stroke();

    //     // draw the handles
    //     cornerstoneTools.drawHandles(context, eventData, data.handles, color);

    //     // Calculate the mean, stddev, and area
    //     // TODO: calculate this in web worker for large pixel counts...

    //     var width = Math.abs(data.handles.start.x - data.handles.end.x);
    //     var height = Math.abs(data.handles.start.y - data.handles.end.y);
    //     var left = Math.min(data.handles.start.x, data.handles.end.x);
    //     var topData = Math.min(data.handles.start.y, data.handles.end.y);
    //     var pixels = cornerstone.getPixels(eventData.element, left, topData, width, height);

    //     var ellipse = {
    //         left: left,
    //         top: topData,
    //         width: width,
    //         height: height
    //     };

    //     var meanStdDev = calculateMeanStdDev(pixels, ellipse);
    //     var area = (width * eventData.image.columnPixelSpacing) * (height * eventData.image.rowPixelSpacing);
    //     var areaText = 'Area: ' + area.toFixed(2) + ' mm^2';
    //     var NTL = NTLMark;

    //     // Draw text
    //     context.font = font;

    //     var textSize = context.measureText(area);

    //     var textX = centerX < (eventData.image.columns / 2) ? centerX + (widthCanvas / 2) : centerX - (widthCanvas / 2) - textSize.width;
    //     var textY = centerY < (eventData.image.rows / 2) ? centerY + (heightCanvas / 2) : centerY - (heightCanvas / 2);

    //     context.fillStyle = color;
    
    //     //Changed By Vivek Patel
    //     //cornerstoneTools.drawTextBox(context, 'vivek: ' + 'vivek', textX, textY - fontHeight - 25, color);
    //     //if (NTLMark != "") {
    //     //    cornerstoneTools.drawTextBox(context, 'NTL: ' + NTL, textX, textY - fontHeight - 25, color);
    //     //}
    //     if (data.text == "" || data.text == undefined) {
    //         cornerstoneTools.drawTextBox(context, 'Mean: ' + meanStdDev.mean.toFixed(2), textX, textY - fontHeight - 5, color);
    //         cornerstoneTools.drawTextBox(context, 'StdDev: ' + meanStdDev.stdDev.toFixed(2), textX, textY, color);
    //     }
    //     else {
    //         cornerstoneTools.drawTextBox(context, 'NTL: ' + data.text, textX, textY - fontHeight - 25, color);
    //         cornerstoneTools.drawTextBox(context, 'Mean: ' + meanStdDev.mean.toFixed(2), textX, textY - fontHeight - 5, color);
    //         cornerstoneTools.drawTextBox(context, 'StdDev: ' + meanStdDev.stdDev.toFixed(2), textX, textY, color);
    //     }
    //     //cornerstoneTools.drawTextBox(context, areaText, textX, textY + fontHeight + 5, color);
    //     context.restore();
    // }
    ////}


    // function calculateMeanStdDev(sp, ellipse) {
    
    //     // TODO: Get a real statistics library here that supports large counts

    //     var sum = 0;
    //     var sumSquared = 0;
    //     var count = 0;
    //     var index = 0;

    //     for (var y = ellipse.top; y < ellipse.top + ellipse.height; y++) {
    //         for (var x = ellipse.left; x < ellipse.left + ellipse.width; x++) {
    //             sum += sp[index];
    //             sumSquared += sp[index] * sp[index];
    //             count++;
    //             index++;
    //         }
    //     }

    //     if (count === 0) {
    //         return {
    //             count: count,
    //             mean: 0.0,
    //             variance: 0.0,
    //             stdDev: 0.0
    //         };
    //     }

    //     var mean = sum / count;
    //     var variance = sumSquared / count - mean * mean;

    //     return {
    //         count: count,
    //         mean: mean,
    //         variance: variance,
    //         stdDev: Math.sqrt(variance)
    //     };
    // }

};

function onMagnify() {
    document.getElementById("lblmagLevelRange").style.display = "block";
    document.getElementById("magLevelRange").style.display = "block";
    document.getElementById("lblmagSizeRange").style.display = "block";
    document.getElementById("magSizeRange").style.display = "block";
}

function offMagnify() {
    document.getElementById("lblmagLevelRange").style.display = "none";
    document.getElementById("magLevelRange").style.display = "none";
    document.getElementById("lblmagSizeRange").style.display = "none";
    document.getElementById("magSizeRange").style.display = "none";
}

function onPlay() {
    document.getElementById("lblloop").style.display = "block";
    document.getElementById("loop").style.display = "block";
    document.getElementById("slice-range").style.display = "block";
}

function offPlay() {
    document.getElementById("lblloop").style.display = "none";
    document.getElementById("loop").style.display = "none";
    document.getElementById("slice-range").style.display = "none";
}

function onRotation() {
    $('#rotation').css("color", "red")
    document.getElementById("rotation").style.display = "block";
}

function offRotation() {
    document.getElementById("rotation").style.display = "none";
}

function rotateMarker(div, rotation) {
    var rotationCSS = {
        "-webkit-transform-origin": "center center",
        "-moz-transform-origin": "center center",
        "-o-transform-origin": "center center",
        "transform-origin": "center center",
        "transform": "rotate(" + rotation + "deg)"
    };

    var oppositeRotationCSS = {
        "-webkit-transform-origin": "center center",
        "-moz-transform-origin": "center center",
        "-o-transform-origin": "center center",
        "transform-origin": "center center",
        "transform": "rotate(" + -rotation + "deg)"
    };

    div.css(rotationCSS);
    div.find(".orientationMarkerDiv").css(oppositeRotationCSS);
}

function calculateOrientationMarkers(element, viewport) {
    var elementDiv = $(element);
    var enabledElement = cornerstone.getEnabledElement(element);
    var imagePlaneMetaData = cornerstoneTools.metaData.get('imagePlane', enabledElement.image.imageId);
    console.log(imagePlaneMetaData);

    var rowString = cornerstoneTools.orientation.getOrientationString(imagePlaneMetaData.rowCosines);
    var columnString = cornerstoneTools.orientation.getOrientationString(imagePlaneMetaData.columnCosines);

    var oppositeRowString = cornerstoneTools.orientation.invertOrientationString(rowString);
    var oppositeColumnString = cornerstoneTools.orientation.invertOrientationString(columnString);

    var markers = {
        top: oppositeColumnString,
        bottom: columnString,
        left: oppositeRowString,
        right: rowString
    }

    var topMid = elementDiv.find('.mrtopmiddle .orientationMarker');
    var bottomMid = elementDiv.find('.mrbottommiddle .orientationMarker');
    var rightMid = elementDiv.find('.mrrightmiddle .orientationMarker');
    var leftMid = elementDiv.find('.mrleftmiddle .orientationMarker');

    topMid.text(markers.top);
    bottomMid.text(markers.bottom);
    rightMid.text(markers.right);
    leftMid.text(markers.left);
}

function updateOrientationMarkers(element, viewport) {
    var elementDiv = $(element);
    var width = elementDiv.width();
    var height = elementDiv.width();

    //Apply rotations
    var orientationMarkers = elementDiv.find('.orientationMarkers');
    rotateMarker(orientationMarkers, viewport.rotation);
}

function onNewImage(e, data) {
    var newImageIdIndex = stack.currentImageIdIndex;
    var slider = document.getElementById('slice-range');
    slider.value = newImageIdIndex;
    var currentValueSpan = document.getElementById("sliceText");
    currentValueSpan.textContent = "Image " + (newImageIdIndex + 1) + "/" + imageIds.length;
    var playClipToolData = cornerstoneTools.getToolState(element, 'playClip');
    if (playClipToolData !== undefined && !$.isEmptyObject(playClipToolData.data)) {
        $("#frameRate").text("FPS: " + Math.round(data.frameRate));
        $('#frameRate').css("color", "red")
    } else {
        if ($("#frameRate").text().length > 0) {
            $("#frameRate").text("");
            $('#frameRate').css("color", "white")
        }
    }

    var imagearray = imageIds[newImageIdIndex].split('/');
    var imageNameTemp = imagearray[imagearray.length - 1].split('?');
    var imageName = imageNameTemp[0].toUpperCase();
    document.getElementById("imagename").textContent = imageName;

    if ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1") {
        if (imageType.toUpperCase().match("DCM")) {
            cornerstone.loadAndCacheImage(imageIds[newImageIdIndex]).then(function (image) {
                if (image.data.intString('x00201041') != undefined || image.data.intString('x00201041') != null) {
                    document.getElementById("sliceInterval").style.display = "block";
                    $("#sliceInterval").text("Slice Location : " + image.data.string('x00201041'))
                }
            
                if (image.data.intString('x00180050') != undefined || image.data.intString('x00180050') != null) {
                    document.getElementById("sliceThickness").style.display = "block";
                    $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))
                }
            });
        }
    }

    query = window.location.search.substring(1);

    if (!query == "" || !query == null) {
        if (imageType.toUpperCase().match("DCM")) {
            cornerstone.loadAndCacheImage(imageIds[newImageIdIndex]).then(function (image) {
                if (image.data.intString('x00201041') != undefined || image.data.intString('x00201041') != null) {
                    document.getElementById("sliceInterval").style.display = "block";
                    $("#sliceInterval").text("Slice Location : " + image.data.string('x00201041'))
                }

                if (image.data.intString('x00180050') != undefined || image.data.intString('x00180050') != null) {
                    document.getElementById("sliceThickness").style.display = "block";
                    $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))
                }

                 //document.getElementById("sliceInterval").style.display = "block"; // Change by sachin to show slice location instead of interval
                //$("#sliceInterval").text("Slice Interval : " + image.data.uint16('x00180088'))
                //$("#sliceInterval").text("Slice Interval : " + image.data.string('x00180088'))

                document.getElementById("acquisitionDate").style.display = "block";
                $("#acquisitionDate").text("Acquisition Date : " + image.data.string('x00080022'))

                document.getElementById("subjectName").style.display = "block";
                $("#subjectName").text("Subject Name : " + image.data.string('x00100010'))
            });
        }
    }
    ShowToolTable();
}

function selectImage(event) {
    var targetElement = document.getElementById("dicomImage");
    var newImageIdIndex = parseInt(event.currentTarget.value, 10);
    var stackToolDataSource = cornerstoneTools.getToolState(targetElement, 'stack');
    if (stackToolDataSource === undefined) {
        return;
    }
    var stackData = stackToolDataSource.data[0];
    if (newImageIdIndex !== stackData.currentImageIdIndex && stackData.imageIds[newImageIdIndex] !== undefined) {
        cornerstone.loadAndCacheImage(stackData.imageIds[newImageIdIndex]).then(function (image) {
            var viewport = cornerstone.getViewport(targetElement);
            stackData.currentImageIdIndex = newImageIdIndex;
            cornerstone.displayImage(targetElement, image, viewport);
        });
    }
}

function updateTheImage(imageIndex) {
    currentImageIndex = imageIndex;
    cornerstone.loadAndCacheImage(imageIds[currentImageIndex]).then(function (image) {
        var viewport = cornerstone.getDefaultViewportForImage(element, image);
        cornerstone.displayImage(element, image, viewport);
        var currentValueSpan = document.getElementById("sliceText");
        currentValueSpan.textContent = "Image " + (currentImageIndex + 1) + "/" + imageIds.length;
        $('#sliceText').css("color", "red")


        var newImageIdIndex = stack.currentImageIdIndex;
        var slider = document.getElementById('slice-range');
        slider.value = newImageIdIndex;
        var currentValueSpan = document.getElementById("sliceText");
        currentValueSpan.textContent = "Image " + (newImageIdIndex + 1) + "/" + imageIds.length;

        var playClipToolData = cornerstoneTools.getToolState(element, 'playClip');
        if (playClipToolData !== undefined && !$.isEmptyObject(playClipToolData.data)) {
            $("#frameRate").text("FPS: " + Math.round(data.frameRate));
            $('#frameRate').css("color", "red")
        } else {
            if ($("#frameRate").text().length > 0) {
                $("#frameRate").text("");
                $('#frameRate').css("color", "white")
            }
        }
    });
};

function handleDragOver(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    evt.dataTransfer.dropEffect = 'copy'; // Explicitly show this is a copy.
}

function handleFileSelect(evt) {
    evt.stopPropagation();
    evt.preventDefault();
    var files = evt.dataTransfer.files;
    file = files[0];
    var imageIds = cornerstoneWADOImageLoader.fileManager.add(file);
    loadAndViewImage(imageIds);
}

function activate(id) {
    $('a').removeClass('active');
    $(id).addClass('active');
}

$('#chkshadow').on('change', function () {
    cornerstoneTools.length.setConfiguration({ shadow: this.checked });
    cornerstoneTools.angle.setConfiguration({ shadow: this.checked });
    cornerstone.updateImage(element);
});

cornerstoneTools.magnify.setConfiguration(config);
cornerstoneTools.orientationMarkers.setConfiguration(config);
cornerstoneTools.zoom.setConfiguration(config);

$(element).on("CornerstoneNewImage", onNewImage);

$(element).on("CornerstoneImageRendered", onViewportUpdated);

$('#dicomImage').on("CornerstoneImageRendered", onImageRendered);

$("#slice-range").on("input", selectImage);

// Listen for changes to the viewport so we can update the text overlays in the corner
function onImageRendered(e) {
    var viewport = cornerstone.getViewport(e.target)
    updateOrientationMarkers(e.target, viewport);
    //$('#rotation').text("Rotation: " + Math.round(viewport.rotation) + "�");
    $('#rotation').text("Rotation: " + Math.round(viewport.rotation) + String.fromCharCode(176));
};

cornerstoneWebImageLoader.configure({
    beforeSend: function (xhr) {
        // Add custom headers here (e.g. auth tokens)
        //xhr.setRequestHeader('x-auth-token', 'my auth token');
    }
});

function loadAndViewImage(imageIds) {
    try {
        //uncomment
        var start = new Date().getTime();
        // image enable the dicomImage element
        cornerstone.enable(element);
        cornerstone.loadAndCacheImage(imageIds).then(function (image) {
            var viewport = cornerstone.getDefaultViewportForImage(element, image);
            if (loaded === false) {
                cornerstone.displayImage(element, image, viewport);
                cornerstoneTools.mouseInput.enable(element);
                cornerstoneTools.mouseWheelInput.enable(element);
                // Enable all tools we want to use with this element
                cornerstoneTools.wwwc.activate(element, 1); // ww/wc is the default tool for left mouse button
                cornerstoneTools.pan.activate(element, 2); // pan is the default tool for middle mouse button
                cornerstoneTools.zoom.activate(element, 4); // zoom is the default tool for right mouse button
                //cornerstoneTools.zoomWheel.activate(element); // zoom is the default tool for middle mouse wheel
                cornerstoneTools.probe.enable(element);
                cornerstoneTools.length.enable(element);
                cornerstoneTools.ellipticalRoi.enable(element);
                cornerstoneTools.rectangleRoi.enable(element);
                cornerstoneTools.angle.enable(element);
                cornerstoneTools.highlight.enable(element);
                cornerstoneTools.addStackStateManager(element, ['stack', 'playClip']);
                cornerstoneTools.addToolState(element, 'stack', stack);
                cornerstoneTools.stackScrollWheel.activate(element);
                cornerstoneTools.scrollIndicator.enable(element);

                activate("#enableWindowLevelTool");
                loaded = true;
            }

            function getTransferSyntax() {
                var value = image.data.string('x00020010');
                return value + ' [' + uids[value] + ']';
            }

            function getSopClass() {
                var value = image.data.string('x00080016');
                return value + ' [' + uids[value] + ']';
            }

            function getPixelRepresentation() {
                var value = image.data.uint16('x00280103');
                if (value === undefined) {
                    return;
                }
                return value + (value === 0 ? ' (unsigned)' : ' (signed)');
            }

            function getPlanarConfiguration() {
                var value = image.data.uint16('x00280006');
                if (value === undefined) {
                    return;
                }
                return value + (value === 0 ? ' (pixel)' : ' (plane)');
            }

            $(element).mousemove(function (event) {
                var pixelCoords = cornerstone.pageToPixel(element, event.pageX, event.pageY);
                var x = event.pageX;
                var y = event.pageY;
                $('#pagecoords').text("PageX=" + event.pageX + ", PageY=" + event.pageY);
                $('#pixelcoords').text("PixelX=" + pixelCoords.x + ", PixelY=" + pixelCoords.y);
            });

            //Dicom Details
            document.getElementById("mrtopleft").style.display = "block";
            document.getElementById("mrbottomright").style.display = "block";

            if ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1") {
                //uncomment
                if (imageType.toUpperCase().match("DCM")) {
                    //if (image.data.intString('x00180050') != undefined || image.data.intString('x00180050') != null) {
                    //    document.getElementById("sliceThickness").style.display = "block";
                    //    $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))
                    //}

                    $('#transferSyntax').text(getTransferSyntax());
                    $('#sopClass').text(getSopClass());
                    $('#samplesPerPixel').text(image.data.uint16('x00280002'));
                    $('#photometricInterpretation').text(image.data.string('x00280004'));
                    $('#numberOfFrames').text(image.data.string('x00280008'));
                    $('#planarConfiguration').text(getPlanarConfiguration());
                    $('#rows').text(image.data.uint16('x00280010'));
                    $('#columns').text(image.data.uint16('x00280011'));
                    $('#pixelSpacing').text(image.data.string('x00280030'));
                    $('#bitsAllocated').text(image.data.uint16('x00280100'));
                    $('#bitsStored').text(image.data.uint16('x00280101'));
                    $('#highBit').text(image.data.uint16('x00280102'));
                    $('#pixelRepresentation').text(getPixelRepresentation());
                    $('#windowCenter').text(image.data.string('x00281050'));
                    $('#windowWidth').text(image.data.string('x00281051'));
                    $('#rescaleIntercept').text(image.data.string('x00281052'));
                    $('#rescaleSlope').text(image.data.string('x00281053'));
                    $('#basicOffsetTable').text(image.data.elements.x7fe00010.basicOffsetTable ? image.data.elements.x7fe00010.basicOffsetTable.length : '');
                    $('#fragments').text(image.data.elements.x7fe00010.fragments ? image.data.elements.x7fe00010.fragments.length : '');
                    $('#minStoredPixelValue').text(image.minPixelValue);
                    $('#maxStoredPixelValue').text(image.maxPixelValue);
                }
            }

            query = window.location.search.substring(1);

            if (!query == "" || !query == null) {
                //if (image.data.intString('x00180050') != undefined || image.data.intString('x00180050') != null) {
                //    document.getElementById("sliceThickness").style.display = "block";
                //    $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))
                //}

                //document.getElementById("sliceInterval").style.display = "none";
                ////$("#sliceInterval").text("Slice Interval : " + image.data.uint16('x00180088'))
                //$("#sliceInterval").text("Slice Interval : " + image.data.string('x00180088'))

                //document.getElementById("acquisitionDate").style.display = "block";
                //$("#acquisitionDate").text("Acquisition Date : " + image.data.string('x00080022'))

                //document.getElementById("subjectName").style.display = "block";
                //$("#subjectName").text("Subject Name : " + image.data.string('x00100010'))
            }

            var end = new Date().getTime();
            var time = end - start;
            $('#loadTime').text(time + "ms");

        });
    }
    catch (err) {
        AlertBox("error", " Dicom Viewer", "Error")
    }
}

var iBizImageStatus, ActivityID, NodeID, ActivityDef, vWorkspaceId, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId, SaveImageFlag;
var ReviewStatus;
var BizNetDicomSave = false;
var BizNetDicomPath;
var MIDicomSave = false;
var MIDicomPath;
var MISessionExpired = false;

function disableAllTools() {
    cornerstoneTools.wwwc.disable(element);
    cornerstoneTools.pan.activate(element, 2); // 2 is middle mouse button
    cornerstoneTools.zoom.activate(element, 4); // 4 is right mouse button        
    cornerstoneTools.probe.deactivate(element, 1);
    cornerstoneTools.dragProbe.deactivate(element, 1);
    cornerstoneTools.dragProbeTouch.deactivate(element);
    cornerstoneTools.length.deactivate(element, 1);
    cornerstoneTools.angle.deactivate(element, 1);
    cornerstoneTools.ellipticalRoi.deactivate(element, 1);
    cornerstoneTools.rectangleRoi.deactivate(element, 1);
    cornerstoneTools.stackScroll.deactivate(element, 1);
    cornerstoneTools.wwwcTouchDrag.deactivate(element);
    cornerstoneTools.zoomTouchDrag.deactivate(element);
    cornerstoneTools.panTouchDrag.deactivate(element);
    cornerstoneTools.stackScrollTouchDrag.deactivate(element);
    cornerstoneTools.arrowAnnotate.deactivate(element, 1)
    cornerstoneTools.arrowAnnotateTouch.deactivate(element);
    cornerstoneTools.simpleAngle.deactivate(element, 1);
    cornerstoneTools.freehand.deactivate(element, 1);
    cornerstoneTools.highlight.deactivate(element, 1);
    cornerstoneTools.magnify.disable(element);
    cornerstoneTools.magnifyTouchDrag.disable(element);
    cornerstoneTools.rotate.deactivate(element, 1);
    cornerstoneTools.rotateTouchDrag.deactivate(element);
    cornerstoneTools.ortho.deactivate(element, 1);
    cornerstoneTools.perpendicular.deactivate(element, 1);

    //cornerstoneTools.myDropdown.deactivate(element, 1);
    //cornerstoneTools..deactivate(element, 1);
    cornerstoneTools.Abdomen.deactivate(element, 1);
    cornerstoneTools.Angio.deactivate(element, 1);
    cornerstoneTools.Bone.deactivate(element, 1);
    cornerstoneTools.Brain.deactivate(element, 1);
    cornerstoneTools.Chest.deactivate(element, 1);
    cornerstoneTools.Lungs.deactivate(element, 1);




    //cornerstoneTools.Abdomen.disable(element);
    //cornerstoneTools.AbdomenTouchDrag.deactivate(element);
}

function toolenableWindowLevelTool() {
    activate('#enableWindowLevelTool')
    disableAllTools();
    cornerstoneTools.wwwc.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolinvert() {
    var viewport = cornerstone.getViewport(element);
    // Toggle invert
    if (viewport.invert === true) {
        viewport.invert = false;
    } else {
        viewport.invert = true;
    }
    cornerstone.setViewport(element, viewport);
    offMagnify();
    offPlay();
}

function toolpan() {
    activate('#pan')
    disableAllTools();
    cornerstoneTools.pan.activate(element, 3); // 3 means left mouse button and middle mouse button
    offMagnify();
    offPlay();
}

function toolstackscroll() {
    activate('#stackscroll')
    disableAllTools();
    cornerstoneTools.stackScroll.activate(element, 1);
    cornerstoneTools.stackScrollTouchDrag.activate(element);
    offMagnify();
    offPlay();
}

function toolzoom() {
    activate('#zoom')
    disableAllTools();
    cornerstoneTools.zoom.activate(element, 5); // 5 means left mouse button and right mouse button
    offMagnify();
    offPlay();
    offRotation();
}

function toolenableLength() {
    activate('#enableLength')
    disableAllTools();
    cornerstoneTools.length.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolanglemeasurement() {
    activate('#anglemeasurement')
    disableAllTools();
    cornerstoneTools.simpleAngle.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolpixelprobe() {
    activate('#pixelprobe')
    disableAllTools();
    cornerstoneTools.probe.activate(element, 1);
    offMagnify();
    offPlay();
}

function tooldragpixelprobe() {
    activate('#dragpixelprobe')
    disableAllTools();
    cornerstoneTools.dragProbe.activate(element, 1);
    cornerstoneTools.dragProbeTouch.activate(element);
    offMagnify();
    offPlay();
}

function toolcircleroi() {
    activate('#circleroi')
    disableAllTools();
    cornerstoneTools.ellipticalRoi.activate(element, 1);
    offMagnify();
}

function toolrectangleroi() {
    activate('#rectangleroi')
    disableAllTools();
    cornerstoneTools.rectangleRoi.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolplayclip() {
    var stackState = cornerstoneTools.getToolState(element, 'stack');
    var frameRate = stackState.data[0].frameRate;
    // Play at a default 10 FPS if the framerate is not specified
    if (frameRate === undefined) {
        frameRate = 10;
    }
    cornerstoneTools.playClip(element, frameRate);
    offMagnify();
    onPlay();
}

function toolstopclip() {
    cornerstoneTools.stopClip(element);
    offMagnify();
    onPlay();
}

function toolrotation() {
    activate('#rotation')
    disableAllTools();
    cornerstoneTools.rotate.activate(element, 1);
    cornerstoneTools.rotateTouchDrag.activate(element);
    offMagnify();
    offPlay();
}

function toolinterpolation() {
    activate('#interpolation')
    disableAllTools();
    var viewport = cornerstone.getViewport(element);
    if (viewport.pixelReplication === true) {
        viewport.pixelReplication = false;
    }
    else {
        viewport.pixelReplication = true;
    }
    cornerstone.updateImage(element);
    offMagnify();
    offPlay();
}

function toolanglemeasurement2() {
    activate('#anglemeasurement')
    disableAllTools();
    cornerstoneTools.angle.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolfreeformroi() {
    activate('#freeformroi')
    disableAllTools();
    cornerstoneTools.freehand.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolhighlight() {
    activate('#highlight')
    disableAllTools();
    cornerstoneTools.highlight.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolarrowannotation() {
    activate('#arrowannotation')
    disableAllTools();
    cornerstoneTools.arrowAnnotate.activate(element, 1);
    cornerstoneTools.arrowAnnotateTouch.activate(element);
    offMagnify();
    offPlay();
}

function toolmagnify() {
    activate('#magnify')
    disableAllTools();
    cornerstoneTools.magnify.activate(element, 1);
    cornerstoneTools.magnifyTouchDrag.activate(element);
    onMagnify();
    offPlay();
}

function toolresettools() {
    activate('#resettools')
    disableAllTools();
    cornerstoneTools.wwwc.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolcleartools() {
    activate('#cleartools')
    disableAllTools();
    cornerstoneTools.wwwc.activate(element, 1);
    var toolStateManager = cornerstoneTools.globalImageIdSpecificToolStateManager;
    toolStateManager.clear(element)
    cornerstone.updateImage(element);
    offMagnify();
    offPlay();
}

function toolresetview() {
    activate('#resetview')
    disableAllTools();
    var canvas = $('#dicomImage canvas').get(0);
    var enabledElement = cornerstone.getEnabledElement(element);
    var viewport = cornerstone.getDefaultViewport(canvas, enabledElement.image)
    cornerstoneTools.wwwc.activate(element, 1)
    cornerstone.setViewport(element, viewport);
    offMagnify();
    offPlay();
}

function toolfullscreen() {
    if (!document.fullscreenElement && !document.mozFullScreenElement &&
           !document.webkitFullscreenElement && !document.msFullscreenElement) {
        if (container.requestFullscreen) {
            container.requestFullscreen();
        } else if (container.msRequestFullscreen) {
            container.msRequestFullscreen();
        } else if (container.mozRequestFullScreen) {
            container.mozRequestFullScreen();
        } else if (container.webkitRequestFullscreen) {
            container.webkitRequestFullscreen();
        }
    }
    onMagnify();
    onPlay();
    onRotation();
}

function toolangle() {
    activate('#angle')
    disableAllTools();
    cornerstoneTools.angle.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolhighlight() {
    activate('#highlight')
    disableAllTools();
    cornerstoneTools.highlight.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolfreehand() {
    activate('#freehand')
    disableAllTools();
    cornerstoneTools.freehand.activate(element, 1);
    offMagnify()
    offPlay();
}

function toolsave() {
    $.ajax({
        url: WebURL + "MIDicomViewer/ClearSession",
        type: 'POST',
        data: '',
        //contentType: 'application/json; charset=utf-8',
        //dataType: 'json',
        async: false,
        success: function (msg) {
            var filename = 'dicom'
            cornerstoneTools.saveAs(element, filename);
        },
        error: function (msg) {
            AlertBox("error", " Dicom Viewer", "Error!")
        }
    });

    var query = window.location.search.substring(1);
    if (query == '' || query == "" || query == null) {
        $.ajax({
            url: WebURL + "MIDicomViewer/SaveDicom",
            type: 'POST',
            data: '',
            //contentType: 'application/json; charset=utf-8',
            //dataType: 'json',
            async: false,
            success: function (msg) {
                AlertBox("success", " Dicom Viewer", "Image saved successfully!")
            }
        });
    }
    //return false; 
}

function toolreferenceline() {
    //$('#toolreferenceline').click(function () {
    //    activate('#toolreferenceline')
    //    disableAllTools();
    //    cornerstoneTools.referenceLines.tool.enable(coronalElement);
    //    cornerstoneTools.referenceLines.tool.enable(axialElement);
    //});
}

function toolortho() {
    activate('#ortho')
    disableAllTools();
    cornerstoneTools.ortho.activate(element, 1);
    offMagnify();
    offPlay();
}

function toolperpendicular() {
    activate('#perpendicular')
    disableAllTools();
    cornerstoneTools.perpendicular.activate(element, 1);
    offMagnify();
    offPlay();
}

//Added by rinkal
function toolWWWCNew() {
    document.getElementById("myDropdown").classList.toggle("show");
}

function Abdomen() {

    activate('#toolAbdomen');
    disableAllTools();
    cornerstoneTools.Abdomen.activate(element, 1);
    cornerstoneTools.Abdomen.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

function Angio() {
    activate('#toolAngio');
    disableAllTools();
    cornerstoneTools.Angio.activate(element, 1);
    cornerstoneTools.Angio.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

function Bone() {
    activate('#toolBone');
    disableAllTools();
    cornerstoneTools.Bone.activate(element, 1);
    cornerstoneTools.Bone.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

function Brain() {
    activate('#toolBrain');
    disableAllTools();
    cornerstoneTools.Brain.activate(element, 1);
    cornerstoneTools.Brain.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

function Chest() {
    activate('#toolChest');
    disableAllTools();
    cornerstoneTools.Chest.activate(element, 1);
    cornerstoneTools.Chest.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

function Lungs() {
    activate('#toolLungs');
    disableAllTools();
    cornerstoneTools.Lungs.activate(element, 1);
    cornerstoneTools.Lungs.strategy();
    offMagnify();
    offPlay();
    document.getElementById("myDropdown").classList.toggle("show");
}

//$(document).click(function () {
//    document.getElementById("myDropdown").classList.toggle("show");
//    //$("#myDropdown").css("display", "none")
//});

//window.onclick = function (event) {
//    if (!event.target.matches('.dropbtn')) {
//        var dropdowns = document.getElementsByClassName("dropdown-content");
//        var i;
//        for (i = 0; i < dropdowns.length; i++) {
//            var openDropdown = dropdowns[i];
//                //document.getElementById("myDropdown").classList.toggle("show");
//            //$("#myDropdown").css("display", "none")
//        }
//    }
//}

//End
$(document).on("webkitfullscreenchange mozfullscreenchange fullscreenchange", function () {
    if (!document.fullscreenElement && !document.mozFullScreenElement &&
        !document.webkitFullscreenElement && !document.msFullscreenElement) {
        $(container).width(744);
        $(container).height(512);
        $(element).width(744);
        $(element).height(512);
    } else {
        $(container).width($(window).width());
        $(container).height($(window).height());
        $(element).width($(container).width());
        $(element).height($(container).height());
    }
    cornerstone.resize(element, true);
    offMagnify();
    offPlay();
})

$(window).on("resize orientationchange", function () {
    if (document.fullscreenElement || document.mozFullScreenElement ||
        document.webkitFullscreenElement || document.msFullscreenElement) {
        $(container).width($(window).width());
        $(container).height($(window).height());
        $(element).width($(container).width());
        $(element).height($(container).height());
        cornerstone.resize(element, true);
        offMagnify();
        offPlay();
    }
})

var DicomDataAnnotation = {}, DicomTLToolData = [], eCTDGridData = [];
var LesionSavedDetailsDATAFlag = false;
var arryActivity = [];
var remark = "";
var savedJsonData = [];
var savedJsonDataTL = [];
var savedJsonDataNTL = [];
var savedJsonDataNL = [];

$(document).ready(function () {
    debugger;
    isVisit = $("#hdnIsViist").val()
    var temp = decodeURI($("#hdnarryStorage").val());
   
    if (isVisit == null) {
        isVisit = ''
    }
    //debugger;  //uncomment
    //$.ajax({
    //    url: ApiURL + "GetData/RetriveDicomAnnotation",
    //    type: 'POST',
    //    //crossDomain: true,
    //    //data: MIDicomAnnotation,
    //    //contentType: 'application/json; charset=utf-8',
    //    //dataType: 'text',
    //    async: false,
    //    success: function (data) {
    //        DicomDataAnnotation = data;
    //        //var result = JSON.parse(data)
    //    },
    //    error: function (e) {
    //        var error = e
    //    }
    //});


    // action on key up
    $(document).keyup(function (e) {
        if (e.which == 17) {
            isCtrl = false;
        }
        if (e.which == 16) {
            isShift = false;
        }
    });
    // action on key down
    $(document).keydown(function (e) {

        //if (e.which == 27) {
        //    var msgPopUpVal = document.getElementById('msgPopUp')
        //    if (msgPopUpVal.hasChildNodes() == true) {
        //        $("#msgPopUp").empty()
        //    }	      
        //}

        if (e.which == 17) {
            isCtrl = true;
        }
        if (e.which == 16) {
            isShift = true;
        }
        if (e.which == 120 && isCtrl) {
            isCtrl = false;
            $("#btnMIFinalSaveLesion").trigger("click");
        }
        if (e.which == 120 && isShift) {
            isShift = false;
            $("#btnMIFinalSubmitLesion").trigger("click")
        }
    });


    $('#ModalMIeSignature').on('shown.bs.modal', function () {
        $('#txtPassword').focus();
    })

    $('#ModalMIRemarkPopUp').on('shown.bs.modal', function () {
        $('#txtRemarks').focus();
    })

    //debugger;     //uncomment only this line
    //success();

    createDiv();
    $(".spinner").show();

    query = window.location.search.substring(1);

    if (query == '' || query == "" || query == null) {
        if (!$("#hdnvActivityName").val().toUpperCase().match("MARK") && !$("#hdnvActivityName").val().toUpperCase().match("BL") && !$("#hdnvActivityName").val().toUpperCase().match("BASE") && !$("#hdnvActivityName").val().toUpperCase().match("BASELINE") && !$("#hdnvActivityName").val().toUpperCase().match("BASE LINE") && !$("#hdnvActivityName").val().toUpperCase().match("TP") && !$("#hdnvActivityName").val().toUpperCase().match("GLOBAL") && !$("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE") && !$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") && !$("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW") && !$("#hdnvActivityName").val().toUpperCase().match("IOV ASSESSMENT")) {
            AlertBox("ERROR", " Dicom Viewer", "NOT ALLOWED ! NO USER RIGHTS !");
            return false;
        }

        if (!$("#hdnvSubActivityName").val().toUpperCase().match("TL") && !$("#hdnvSubActivityName").val().toUpperCase().match("NTL") && !$("#hdnvSubActivityName").val().toUpperCase().match("NL") && !$("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR") && !$("#hdnvSubActivityName").val().toUpperCase().match("GLOBAL") && !$("#hdnvSubActivityName").val().toUpperCase().match("GLOBAL RESPONSE") && !$("#hdnvSubActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")) {
            AlertBox("ERROR", " Dicom Viewer", "NOT ALLOWED ! NO USER RIGHTS !");
            return false;
        }
    }

    setTimeout(function () {
        createDiv();
        $(".spinner").show();
    }, 0);


    if ($("#hdnvSkipVisit").val() == "Y") {
        //$("#divViewer").empty();
        //$("#divRow").empty();
    }

    $('#LesionModel').on('show', function () {
        this.focus();
    })

    $('#LesionModelNTL').on('show', function () {
        this.focus();
    })

    $('#LesionModelNL').on('show', function () {
        this.focus();
    })

    $('#LesionModelTL').on('show', function () {
        this.focus();
    })

    $('#GetDetailTL').on('show', function () {
        this.focus();
    })

    var divHeader = document.getElementById('divHeader')
    if ($("#hdnvActivityName").val() != "ADJUDICATOR") {
        while (divHeader.hasChildNodes()) {
            divHeader.removeChild(divHeader.lastChild);
        }
    }

    var divHeader = "";
    divHeader += '<h4>' + $("#hdnvActivityName").val() + " " + $("#hdnvSubActivityName").val() + '</h4>';
    $("#divHeader").append(divHeader);

    var iImgTransmittalHdrId;
    var iImgTransmittalDtlId;
    var iImageStatus;

    var vWorkSpaceID = $("#hdnvWorkspaceId").val();
    var iPeriod = 1;

    var activityDetailData = {
        vWorkSpaceID: vWorkSpaceID,
        iPeriod: iPeriod,
    }
    //debugger;     //uncomment
    if (query == '' || query == "" || query == null) {

        $("#hdnSelectedvParentNodeDisplayName").val($("#hdnvActivityName").val())
        $("#hdnSelectedvActivityId").val($("#hdnvActivityId").val())
        $("#hdnSelectediPeriod").val($("#hdniNodeId").val())

        $("#hdnSelectedvChildNodeDisplayName").val($("#hdnvSubActivityName").val())
        $("#hdnSelectedvSubActivityId").val($("#hdnvSubActivityId").val())
        $("#hdnSelectediSubNodeId").val($("#hdniSubNodeId").val())

        //Comment the Logc of Ajax Call taking to much time and one extra server trip 
        //that data is already available in Hidden Field from previous page post back

        //$.ajax({
        //    url: ApiURL + "GetData/ProjectActivityDetails",
        //    type: "POST",
        //    data: activityDetailData,
        //    async: false,
        //    success: function (jsonData) {
        //        if (jsonData.length > 0) {
        //            for (var v = 0 ; v < jsonData.length ; v++) {
        //                if (($("#hdnvActivityId").val() == jsonData[v].vActivityId) && ($("#hdniNodeId").val() == jsonData[v].iNodeId)) {
        //                    $("#hdnSelectedvParentNodeDisplayName").val(jsonData[v].vNodeDisplayName.toUpperCase())
        //                    $("#hdnSelectedvActivityId").val(jsonData[v].vActivityId)
        //                    $("#hdnSelectediPeriod").val(jsonData[v].iNodeId)
        //                }
        //                if (($("#hdnvSubActivityId").val() == jsonData[v].vActivityId) && ($("#hdniSubNodeId").val() == jsonData[v].iNodeId)) {
        //                    $("#hdnSelectedvChildNodeDisplayName").val(jsonData[v].vNodeDisplayName.toUpperCase())
        //                    $("#hdnSelectedvSubActivityId").val(jsonData[v].vActivityId)
        //                    $("#hdnSelectediSubNodeId").val(jsonData[v].iNodeId)
        //                }
        //            }
        //        }
        //        else {
        //            AlertBox("Error", " Dicom Viewer", "No Activity Node  Detail Found. Please Try Again Later!")
        //            return false;
        //        }
        //    },
        //    error: function (e) {
        //        AlertBox("Error", " Dicom Viewer", "Error While Retriving Activity Node Detail. Please Try Again Later!")
        //        return false;
        //    }
        //});
    }

    if (query == '' || query == "" || query == null) {
        iImgTransmittalHdrId = $("#hdniImgTransmittalHdrId").val();
        iImgTransmittalDtlId = $("#hdniImgTransmittalDtlId").val();
        iImageStatus = $("#hdniImageStatus").val();
    }
    else {
        var parms = query.split('&');

        //***********************************************************Query String Detail Info*******************************************************************//
        //var iBizImageStatus, ActivityID, NodeID, ActivityDef, vWorkspaceId, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId;
        //WId - workspaceid , SId - subjectid , PId - ProjectId , UId - userid , MId - modalityid , AId - AnatomyId , VId - visitid, Hdrid - ImgTransmittalHdrId, 
        //DtlId - ImgTransmittalDtlId, iIs - imagestatus, ActivityID - ActivityID, NodeID - NodeID, ActivityDef - TL / NTL, iMySubjectNo - iMySubjectNo, 
        //ScreenNo - ScreenNo, ParentWorkSpaceId - ParentWorkSpaceId, PeriodId - PeriodId
        //***********************************************************Query String Detail Info*******************************************************************//

        for (var i = 0; i < parms.length; i++) {
            var pos = parms[i].indexOf('=');
            if (pos > 0) {
                var key = parms[i].substring(0, pos);
                var val = parms[i].substring(pos + 1);
                if (key == 'Uid') {
                    UserId = val;
                    $("#hdnuserid").val(val);
                }
                else if (key == 'HdrId') {
                    iImgTransmittalHdrId = val;
                    $("#hdnImgTransmittalHdrId").val(val);
                }
                else if (key == 'DtlId') {
                    iImgTransmittalDtlId = val;
                    $("#hdnImgTransmittalDtlId").val(val);
                }
                else if (key == 'iIS') {
                    iBizImageStatus = val;
                    $("#hdnImageStatus").val(val);
                }
                else if (key == 'WId') {
                    vWorkspaceId = val;
                    $("#hdnWorkspaceId").val(val);
                }
                else if (key == 'ActivityID') {
                    ActivityID = val;
                    $("#hdnActivityID").val(val);
                }
                else if (key == 'NodeID') {
                    NodeID = val;
                    $("#hdnNodeID").val(val);
                }
                else if (key == 'ActivityDef') {
                    ActivityDef = val;
                    $("#hdnActivityDef").val(val);
                }
                else if (key == 'iMySubjectNo') {
                    iMySubjectNo = val;
                    $("#hdnMySubjectNo").val(val);
                }
                else if (key == 'ScreenNo') {
                    ScreenNo = val;
                    $("#hdnScreenNo").val(val);
                }
                else if (key == 'ParentWorkSpaceId') {
                    ParentWorkSpaceId = val;
                    $("#hdnParentWorkSpaceId").val(val);
                }
                else if (key == 'PeriodId') {
                    PeriodId = val;
                    $("#hdnPeriodId").val(val);
                }
                else if (key == 'SId') {
                    $("#hdnSubjectId").val(val);
                }
                else if (key == 'PId') {
                    $("#hdnProjectNo").val(val);
                }
                else if (key == 'MId') {
                    $("#hdnModalityNo").val(val);
                }
                else if (key == 'AId') {
                    $("#hdnAnatomyNo").val(val);
                }
                else if (key == 'VId') {
                    $("#hdnVisitId").val(val);
                }
            }
        }
        iImageStatus = iBizImageStatus;
    }

    if (iImageStatus == 1) {
        SaveImageFlag = "DICOM"
    }
    if (iImageStatus == 2) {
        SaveImageFlag = "PNG"
    }

    var subjectImageStudyDetailData;
    var subjectImageStudyDetailAjaxData;
    var cRadiologist;

    if (query == '' || query == "" || query == null) {
        if ($("#hdnvSubActivityName").val().split('-')[0] == "Adjudicator") {
            cRadiologist = $("#hdnUserType").val()
        }else{
            cRadiologist = $("#hdnvSubActivityName").val().split('-')[0];
        }
        
        if (cRadiologist == null || cRadiologist == "") {
            AlertBox("warning", "Dicom Study!", "Please Select Proper Sub Activity To Review Dicom!");
            return false
        }
    }

    //debugger;     //uncomment
    if ($("#hdnImageTransmittalImgDtl_iImageTranNo").val() == null || $("#hdnImageTransmittalImgDtl_iImageTranNo").val() == "null") {
        AlertBox("ERROR", " Dicom Viewer", "iTranNo for DICOM From ImageTransmittalImgDtl Not Found!")
    }

    if (query == '' || query == "" || query == null) {
        subjectImageStudyDetailData = {
            iImgTransmittalHdrId: iImgTransmittalHdrId,
            iImgTransmittalDtlId: iImgTransmittalDtlId,
            iImageStatus: iImageStatus,
            cRadiologist: cRadiologist,
            MODE: "1",
            ImageTransmittalImgDtl_iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val(),
            vParentActivityId: $("#hdnvActivityId").val(),
            iParentNodeId: $("#hdniNodeId").val()

        }
        subjectImageStudyDetailAjaxData = {
            //url: ApiURL + "GetData/SubjectImageStudyDetail",
            url: ApiURL + "GetData/SubjectImageStudyDetails",
            type: "POST",
            async: false,
            data: subjectImageStudyDetailData,
            success: successSubjectImageStudyDetail,
            error: errorSubjectImageStudyDetail
        }
    }
    else {
        subjectImageStudyDetailData = {
            iImgTransmittalHdrId: iImgTransmittalHdrId,
            iImgTransmittalDtlId: iImgTransmittalDtlId,
            iImageStatus: iImageStatus
            //cRadiologist: cRadiologist         
        }
        subjectImageStudyDetailAjaxData = {
            url: ApiURL + "GetData/SubjectImageStudyDetail",
            type: "POST",
            async: false,
            data: subjectImageStudyDetailData,
            success: successSubjectImageStudyDetail,
            error: errorSubjectImageStudyDetail
        }
    }

    fnSubjectImageStudyDetail(subjectImageStudyDetailAjaxData.url, subjectImageStudyDetailAjaxData.type, subjectImageStudyDetailAjaxData.data, subjectImageStudyDetailAjaxData.async, subjectImageStudyDetailAjaxData.success, subjectImageStudyDetailAjaxData.error)

    //From BizNET
    $("#btnSaveLesion").on("click", function () {
        SaveLession();
    });
    $("#btnSubmitLesion").on("click", function () {
        SubmitLesion();
    });
    //-------------------------------------


    //Not Used
    $("#btnMISaveLesion").on("click", function () {
        SaveMILession();
    });
    $("#btnMISubmitLesion").on("click", function () {
        SubmitMILesion();
    });
    //------------------------------

    // comment by Hiren Rami
    //$("#btnMIFinalSaveLesion").on("click", function () {
    //    debugger;
    //    SaveMIFinalLession();
    //});   

    //$("#btnMIFinalSubmitLesion").on("click", function () {
    //    debugger;
    //    var eSignData = "";
    //    eSignData += '<div class="modal fade" id="ModalMIeSignature" role="dialog">';
    //    eSignData += '<div class="modal-dialog">';
    //    eSignData += '<div class="modal-content">';
    //    eSignData += '<div class="modal-header">';
    //    eSignData += '<button type="button" class="btn btn-info btn-sm pull-right box-tools" data-widget="remove" data-dismiss="modal" data-toggle="tooltip" title="" data-original-title="Remove">';
    //    eSignData += '<i class="fa fa-times"></i>';
    //    eSignData += '</button>';
    //    eSignData += '<h4 class="modal-title">e-Signature Authentication</h4>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="modal-body">';
    //    eSignData += '<div class="row">';
    //    eSignData += '<div class="col-lg-6 col-xs-12 form-group">';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<label for="" id="lblUser">User :</label>';
    //    eSignData += '<label for="" id="lblUserName"></label>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="col-lg-6 col-xs-12 form-group">';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<label for="" id="lblDate">DateTime :</label>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<label for="" id="lblDateDetail"></label>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="col-lg-12 col-xs-12 form-group">';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<label for="txtPassword">Password*</label>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<input type="password" class="form-control" id="txtPassword" placeholder="e-Signature" tabindex="1">';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="col-lg-12 col-xs-12 form-group">';
    //    eSignData += '<div class="col-sm-12">';
    //    eSignData += '<label id="lbleSignature" for="">I Here by Confirm Signing of this Record Electronically.</label>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '<div class="modal-footer">';
    //    eSignData += '<button type="button" class="btn btn-default pull-left" data-dismiss="modal" id="btnCloseMIeSignature">Close</button>';
    //    eSignData += '<button type="button" class="btn btn-primary" id="btnMIeSignatureVerification">Submit</button>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';
    //    eSignData += '</div>';

    //    var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    //    var d = new Date();
    //    var curr_date = d.getDate();
    //    var curr_month = d.getMonth();
    //    var curr_year = d.getFullYear();
    //    var time = [d.getHours(), d.getMinutes(), d.getSeconds()].join(':');
    //    $("#lblUserName").html($("#hdnUserNameWithProfile").val())
    //    //$("#lblDateDetail").html(new Date($.now()))
    //    $("#lblDateDetail").html(curr_date + "-" + m_names[curr_month] + "-" + curr_year + " " + time)
    //    $("#txtPassword").val("")
    //    $("#txtPassword").focus();
    //});
    //------------------------------------------------------      

    $("#btnMIeSignatureVerification").on("click", function () {
        if ($("#txtPassword").val() == "" || $("#txtPassword").val() == null) {
            AlertBox("WARNING", " Dicom Viewer", "Please Enter eSignature!")
            return false;
        }

        $.ajax({
            url: WebURL + "MIDicomViewer/MIeSignatureVerification?password=" + $("#txtPassword").val(),
            type: "POST",
            async: false,
            success: function (data) {
                if (data != null) {
                    if (data.length > 0) {
                        if (data == "sessionexpired") {
                            AlertBox("Error", " Dicom Viewer", "Sessioin Expired!")
                            var url = $("#RedirectToLogin").val();
                            location.href = url;
                        }
                        else if (data == "success") {
                            createDiv();
                            $(".spinner").show();
                            SubmitMIFinalLesion();
                            removeDiv();
                            $(".spinner").hide();
                        }
                        else {
                            AlertBox("WARNING", " Dicom Viewer", "Please Enter Valid eSignature!")
                            $("#txtPassword").focus();
                        }
                    }
                }
                else {
                    AlertBox("ERROR", " Dicom Viewer", "Error While Verifying User eSignature")
                }
            },
            error: function () {
                AlertBox("ERROR", " Dicom Viewer", "Error While Verifying User eSignature")
            }
        });
    });

    setTimeout(function () {
        removeDiv();
        $(".spinner").hide();
    }, 0);

    removeDiv();
    $(".spinner").hide();
});

$("#txtPassword").keypress(function (e) {
    if (e.which == 13) {
        if ($("#txtPassword").val() != "") {
            $("#btnMIeSignatureVerification").trigger("click")
        }
    }
});


function success() {
    $.ajax({
        url: WebURL + "MIDicomViewer/SUCCESS",
        type: "POST",
        //async: false,
        success: function (data) { },
        error: function (e) { }
    });
}

//For Subject Image Study Detail
var fnSubjectImageStudyDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: async,
        success: success,
        error: error
    });
}

function successSubjectImageStudyDetail(jsonDetailData) {
    debugger;     //uncomment

    if (jsonDetailData.Table === undefined) {
        jsonData = jsonDetailData
    }
    else {
        jsonData = jsonDetailData.Table
        DicomDataAnnotation = jsonDetailData.Table1;
        eCTDGridData = jsonDetailData.Table1;
    }

    if (jsonData.length > 0) {

        var element = $('#dicomImage').get(0);
        var imgStatus = $("#hdniImageStatus").val()

        $("#hdncR1TLReviewStatus").val(jsonData[0].cR1TLReviewStatus)
        $("#hdncR1NTLReviewStatus").val(jsonData[0].cR1NTLReviewStatus)
        $("#hdncR2TLReviewStatus").val(jsonData[0].cR2TLReviewStatus)
        $("#hdncR2NTLReviewStatus").val(jsonData[0].cR2NTLReviewStatus)
        $("#hdncReviewStatusValue").val(jsonData[0].cReviewStatus);

        imageType = jsonData[0].vFileType;


        query = window.location.search.substring(1);

        if (query == '' || query == "" || query == null) {
            imgStatus = $("#hdniImageStatus").val()

        }
        else {
            var parms = query.split('&');
            var iBizImageStatus;
            for (var i = 0; i < parms.length; i++) {
                var pos = parms[i].indexOf('=');
                if (pos > 0) {
                    var key = parms[i].substring(0, pos);
                    var val = parms[i].substring(pos + 1);
                    if (key == 'iIS') {
                        imgStatus = val;
                    }
                }
            }
        }

        if ((jsonData[0].cReviewStatus == 'Y') && ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1") || $("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")))
        {
            if (isVisit == 'N' || isVisit == '') {
                VisitView();
            }
            else {
                document.getElementById("divVistView").style.display = "none";
            }
        }

        //*********************************For Reference use do not delete it.**************************//

        //imageIds.push("http://90.0.0.68/DICOM/DICOM/0000009471/AH15-01150/22/1/3/Updated/0000009471.png")                        
        //imageIds.push("dicomweb://90.0.0.68/DICOM/DICOM/0000009471/AH15-01150/22/1/3/Uploaded/CT0001.dcm")
        //imageIds.push("http://90.0.0.68/Dicom_Viewer2/1.png");

        //*********************************For Reference use do not delete it.**************************//
        
        if (imgStatus == 1) {
            for (var i = 0; i < jsonData.length; i++) {

                if (jsonData[i].vFileType.toUpperCase().match("DCM")) {
                    imageIds.push(DicomURL_1 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("PNG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("JPG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("JPEG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else {
                    imageIds.push(DicomURL_1 + jsonData[i].vServerPath + "?" + Math.random());
                }
                if (i < 1000) {
                    cornerstone.loadAndCacheImage(imageIds[i]);
                }
                //imageIds.push(DicomURL_1 + jsonData[i].vServerPath + "?" + Math.random());
                //cornerstone.loadAndCacheImage(imageIds[i]);

            }
        }
        if (imgStatus == 2) {
            for (var i = 0; i < jsonData.length; i++) {
                if (jsonData[i].vFileType.toUpperCase().match("DCM")) {
                    imageIds.push(DicomURL_1 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("PNG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("JPG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else if (jsonData[i].vFileType.toUpperCase().match("JPEG")) {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }
                else {
                    imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                }

                if (i < 1000) {
                    cornerstone.loadAndCacheImage(imageIds[i]);
                }
                //imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                //cornerstone.loadAndCacheImage(imageIds[i]);
            }
        }

        //updateTheImage(0);
        loadAndViewImage(imageIds[0]);

        var button = "";
        debugger;
        var buttondata = document.getElementById('dicomButtonGroup')
        while (buttondata.hasChildNodes()) {
            buttondata.removeChild(buttondata.lastChild)
        }
        if ((isVisit == 'N' || isVisit == '') || $("#hdnUserTypeName").val().toUpperCase().match("ADJUDICATOR")) {
            //button += '<style>.dropbtn {background-color: #3498DB;color: white;padding: 16px;font-size: 16px;border: none;cursor: pointer; .dropbtn:hover, .dropbtn:focus {background-color: #2980B9;} .dropdown {position: relative;display: inline-block;} .dropdown-content {display: none;position: absolute;background-color: #f1f1f1;min-width: 160px;overflow: auto;box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);z-index: 1;}.show {display: block;}.dropdown-content a {color: black;padding: 12px 16px;text-decoration: none;display: block;}.dropdown a:hover {background-color: #ddd;}</style>';
            button += '<!-- Orthogonal Ruler -->';
            button += '<button id="toolortho" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Orthogonal Ruler" onclick="toolortho()"><span class="fa fa-plus-square-o fa-lg"></span></button>';
            button += '<!-- WW/WL -->';
            button += '<button id="toolenableWindowLevelTool" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="WW/WC" data-original-title="WW/WC" onclick="toolenableWindowLevelTool()"><span class="fa fa-sun-o"></span></button>';
            button += '<!-- Invert -->';
            button += '<button id="toolinvert" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Invert" data-original-title="Invert" onclick="toolinvert()"><span class="fa fa-adjust"></span></button>';
            button += '<!-- Zoom -->';
            button += '<button id="toolzoom" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Zoom" data-original-title="Zoom" onclick="toolzoom()"><span class="fa fa-search"></span></button>';
            button += '<!-- Pan -->';
            button += '<button id="toolpan" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Pan" data-original-title="Pan" onclick="toolpan()"><span class="fa fa-arrows"></span></button>';
            button += '<!-- Stack scroll -->';
            button += '<button id="toolstackscroll" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Stack Scroll" data-original-title="Stack Scroll" onclick="toolstackscroll()"><span class="fa fa-bars"></span></button>';
            button += '<!-- Length measurement -->';
            button += '<button id="toolenableLength" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Length Measurement" data-original-title="Length Measurement" onclick="toolenableLength()"><span class="fa fa-arrows-v"></span></button>';
            button += '<!-- Angle measurement -->';
            button += '<button id="toolanglemeasurement" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Angle Measurement" data-original-title="Angle Measurement" onclick="toolanglemeasurement()"><span class="fa fa-angle-left"></span></button>';
            button += '<!-- Angle measurement -->';
            button += '<button id="toolanglemeasurement2" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Angle Measurement" data-original-title="Angle Measurement" onclick="toolanglemeasurement2()"><span class="fa fa-angle-right"></span></button>';
            button += '<!-- Pixel probe -->';
            button += '<button id="toolpixelprobe" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Pixel Probe" data-original-title="Pixel Probe" onclick="toolpixelprobe()"><span class="fa fa-dot-circle-o"></span></button>';
            button += '<!-- Drag Pixel probe -->';
            button += '<button id="tooldragpixelprobe" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Drag Pixel Probe" data-original-title="Drag Pixel Probe" onclick="tooldragpixelprobe()"><span class="fa fa-rebel"></span></button>';
            button += '<!-- Elliptical ROI -->';
            button += '<button id="toolcircleroi" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Elliptical ROI" data-original-title="Elliptical ROI" onclick="toolcircleroi()"><span class="fa fa-circle-o"></span></button>';
            button += '<!-- Rectangle ROI -->';
            button += '<button id="toolrectangleroi" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Rectangle ROI" data-original-title="Rectangle ROI" onclick="toolrectangleroi()"><span class="fa fa-square-o"></span></button>';
            button += '<!-- Reference Line -->';
        }
            //button += '@*<button id="toolreferenceline" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Rectangle ROI" data-original-title="Rectangle ROI"><span class="fa fa-square-o"></span></button>*@';
            button += '<!-- Play clip -->';
            button += '<button id="toolplayclip" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Play Clip" data-original-title="Play Clip" onclick="toolplayclip()"><span class="fa fa-play"></span></button>';
            button += '<!-- Stop clip -->';
            button += '<button id="toolstopclip" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Stop Clip" data-original-title="Stop Clip" onclick="toolstopclip()"><span class="fa fa-stop"></span></button>';
        //button += '@*<button id="toollayout" type="button" class="btn btn-sm btn-default dropdown-toggle" data-container="body" data-toggle="dropdown" aria-expanded="false" data-placement="top" title="Layout" data-original-title="Layout"><span class="fa fa-th-large"></span></button>';
        //button += '<ul class="pull-right dropdown-menu choose-layout" role="menu">';
        //button += '<li><a href="#">1x1</a></li>';
        //button += '<li><a href="#">2x1</a></li>';
        //button += '<li><a href="#">1x2</a></li>';
        //button += '<li><a href="#">2x2</a></li>';
        //button += '</ul>*@';
        if ((isVisit == 'N' || isVisit == '') || $("#hdnUserTypeName").val().toUpperCase().match("ADJUDICATOR")) {
            button += '<button id="toolrotation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Rotation" data-original-title="Rotation" onclick="toolrotation()"><span class="fa fa-repeat"></span></button>';
            button += '<button id="toolarrowannotation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Arrow Annotation" data-original-title="Arrow Annotation" onclick="toolarrowannotation()"><span class="fa fa-tags"></span></button>';
            button += '<button id="toolmagnify" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Magnify" data-original-title="Magnify" onclick="toolmagnify()"><span class="fa fa-search"></span></button>';
            button += '<button id="toolresettools" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Reset Tools" data-original-title="Reset Tools" onclick="toolresettools()"><span class="fa fa-refresh"></span></button>';
            //button += '<button id="toolcleartools" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Clear Tools" data-original-title="Clear Tools" onclick="toolcleartools()"><span class="fa fa-times"></span></button>';
            button += '<button id="toolresetview" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Reset View" data-original-title="Reset View" onclick="toolresetview()"><span class="fa fa-picture-o"></span></button>';
            //button += '@*<button id="toolinterpolation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Interpolation" data-original-title="Interpolation"><span class="fa fa-picture-o"></span></button>*@';
            button += '<button id="toolfreeformroi" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="FreeForm ROI" data-original-title="FreeForm ROI" onclick="toolfreeformroi()"><span class="fa fa-paint-brush"></span></button>';
            button += '<button id="toolhighlight" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Highlight" data-original-title="Highlight" onclick="toolhighlight()"><span class="fa fa-bookmark"></span></button>';
            button += '<button id="toolfullscreen" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Full Screen" onclick="toolfullscreen()"><span class="fa fa-arrows-alt"></span></button>';
            button += '<!-- Perpendicular Ruler -->';
            button += '<button id="toolperpendicular" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Perpendicular Ruler" onclick="toolperpendicular()"><span class="fa fa-plus-square-o fa-lg"></span></button>';
            //Added by rinkal
            button += '<button id="toolWWWCNew" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="WW/WC New" onclick="toolWWWCNew()"><span class="fa fa-caret-down"></span></button>';
            button += '<div id="myDropdown" class="dropdown-content"><a href="#" onclick="Abdomen()" id="toolAbdomen">CT Abdomen</a><a href="#" onclick="Angio()" id="toolAngio">CT Angio</a><a href="#" onclick="Bone()" id="toolBone">CT Bone</a><a href="#" onclick="Brain()" id="toolBrain">CT Brain</a><a href="#" onclick="Chest()" id="toolChest">CT Chest</a><a href="#" onclick="Lungs()" id="toolLungs">CT Lungs</a></div>'
        }
        //
        ReviewStatus = jsonData[0].cReviewStatus;

        if (ReviewStatus == 'N') {
            //button += '<button id="toolsave" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Save" data-original-title="Highlight" onclick="toolsave()"><span class="fa fa-file"></span></button>';
        }
        else {
            //button += '<button id="toolsave" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Save" data-original-title="Highlight" onclick="toolsave()"><span class="fa fa-file"></span></button>';
        }

        if (!$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
            $("#dicomButtonGroup").append(button);
        }

        if (query == '' || query == "" || query == null) {
            //debugger;     //uncomment
            if ($("#hdnEditPreviousVisit").val() == "true") {

                if ($("#hdniImageStatus").val() == "1") {
                    if (isVisit == 'N' || isVisit == '') {
                        if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                            //getUserProfile();
                            VisitView();
                        }
                    }
                    else {
                        document.getElementById("divVistView").style.display = "none";
                    }

                    SubActivityData = decodeURI($("#hdnSubActivityData").val());
                    SubActivityNameData = decodeURI($("#hdnSubActivityNameData").val());
                    if (SubActivityData.length > 0) {
                        debugger;
                        var btnLesion = "";
                        var vWorkspaceId = $("#hdnvWorkspaceId").val();
                        var vActivityId = $("#hdnvSubActivityId").val();
                        var iNodeId = $("#hdniSubNodeId").val();
                        SubActivityData = $.parseJSON(SubActivityData);
                        SubActivityNameData = $.parseJSON(SubActivityNameData);

                        if ($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW") || $("#hdnvActivityName").val().toUpperCase().match("MARK") || $("#hdnvActivityName").val().toUpperCase().match("BL") || ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) || $("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvActivityName").val().toUpperCase().match("IOV ASSESSMENT")) {                        
                            //localStorage.setItem("IsTimePoint", "false");

                            // 09-Dec-2022 if (!$("#hdnvActivityName").val().toUpperCase().match("BL")) Condition add
                            if (!$("#hdnvActivityName").val().toUpperCase().match("BL")) {
                                $("#hdnIsTimePoint").val("false");
                                var removeItem = vActivityId + "#" + iNodeId;
                                SubActivityData = jQuery.grep(SubActivityData, function (value) {
                                    return value == removeItem;
                                });

                                removeItem = $("#hdnvSubActivityName").val();
                                SubActivityNameData = jQuery.grep(SubActivityNameData, function (value) {
                                    return value == removeItem;
                                });

                                $("#hdnCurrentSubActivityName").val("ALL-ALL");
                            }
                            
                            btnLesion = "";
                            if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                //btnLesion += '<button style="text-align: right" type="button" id="TL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelTL,#MarkModel" title="TL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-TL</button>';
                                //btnLesion += '<button style="text-align: right" type="button" id="NTL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelNTL,#MarkModel" title="NTL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-NTL</button>';
                                //btnLesion += '<button style="text-align: right" type="button" id="NL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelNL,#MarkModel" title="NL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-NL</button>';
                                btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '." class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel,#MarkModel" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val().toUpperCase() + '</button>';
                                btnLesion += '<button style="text-align: right" type="button" id="Compare" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#GetDetailTL,#MarkModel" title="NL DETAIL" data-tooltip="tooltip" onclick="GetCompareDetail(this)">COMPARE</button>';
                            } else {
                                // 09-Dec-2022 if ($("#hdnvActivityName").val().toUpperCase().match("BL")) condition add And else part already there
                                if ($("#hdnvActivityName").val().toUpperCase().match("BL")) {
                                    $("#hdnIsTimePoint").val("true");
                                    $("#hdnCurrentSubActivityName").val("");
                                    btnLesion = "";
                                    for (var k = 0; k < SubActivityNameData.length; k++) {
                                        var next_k = parseInt(k) + 1;
                                        if (SubActivityNameData[k].split(" ").join("").toUpperCase().match("-TL")) {
                                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[k].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[k] + '" data-target="#LesionModel,#MarkModel" title="' + SubActivityNameData[k].split(" ").join("") + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[k].split(" ").join("") + '</button>';
                                        }
                                        else if (SubActivityNameData[k].split(" ").join("").toUpperCase().match("-NTL")) {
                                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[k].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[k] + '" data-target="#LesionModelNTL,#MarkModel" title="' + SubActivityNameData[k].split(" ").join("") + " DETAIL" + next_k + '" data-tooltip="tooltip">' + SubActivityNameData[k].split(" ").join("") + '</button>';
                                        }
                                        else {
                                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[k].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[k] + '" data-target="#LesionModelNL,#MarkModel" title="' + SubActivityNameData[k].split(" ").join("") + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[k].split(" ").join("") + '</button>';
                                        }
                                    }
                                }
                                else {
                                    btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel,#MarkModel" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL 1" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val().toUpperCase() + "1" + '</button>';
                                }
                            }
                            //btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[0].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[0] + '" data-target="#LesionModel,#MarkModel" title="' + SubActivityNameData[0].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[0].toUpperCase() + '</button>';
                        }
                        else {
                            //localStorage.setItem("IsTimePoint", "true");
                            $("#hdnIsTimePoint").val("true");
                            $("#hdnCurrentSubActivityName").val("");
                            btnLesion = "";
                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[0].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[0] + '" data-target="#LesionModel,#MarkModel" title="' + SubActivityNameData[0].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[0].toUpperCase() + '</button>';
                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[1].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[1] + '" data-target="#LesionModelNTL,#MarkModel" title="' + SubActivityNameData[1].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[1].toUpperCase() + '</button>';
                            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[2].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[2] + '" data-target="#LesionModelNL,#MarkModel" title="' + SubActivityNameData[2].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[2].toUpperCase() + '</button>';
                        }

                        for (var h = 0; h < SubActivityData.length; h++) {
                            //if ($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW") || $("#hdnvActivityName").val().toUpperCase().match("MARK") || $("#hdnvActivityName").val().toUpperCase().match("BL") || ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) || $("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvActivityName").val().toUpperCase().match("IOV ASSESSMENT")) {
                            //    if (SubActivityData.includes(vActivityId + "#" + iNodeId) == true) {
                            //        vActivityId = SubActivityData[h].split("#")[0];
                            //        iNodeId = SubActivityData[h].split("#")[1];
                            //        $("#hdnvSubActivityId").val(vActivityId);
                            //        $("#hdniSubNodeId").val(iNodeId);
                            //        $("#hdnvSubActivityName").val(SubActivityNameData[h]);
                            //        $("#hdnSelectedvChildNodeDisplayName").val(SubActivityNameData[h]);
                            //        $("#hdnSelectedvSubActivityId").val(vActivityId);
                            //        $("#hdnSelectediSubNodeId").val(iNodeId);
                            //    }
                            //    else {
                            //        continue;
                            //    }
                            //}
                            vActivityId = SubActivityData[h].split("#")[0];
                            iNodeId = SubActivityData[h].split("#")[1];
                            $("#hdnvSubActivityId").val(vActivityId);
                            $("#hdniSubNodeId").val(iNodeId);
                            $("#hdnvSubActivityName").val(SubActivityNameData[h]);
                            $("#hdnSelectedvChildNodeDisplayName").val(SubActivityNameData[h]);
                            $("#hdnSelectedvSubActivityId").val(vActivityId);
                            $("#hdnSelectediSubNodeId").val(iNodeId);

                            localStorage.setItem("ArrayNo", h);
                            var MILesionData;
                            var MILesionAjaxData;

                            MILesionData = {
                                vWorkspaceId: vWorkspaceId,
                                vActivityId: vActivityId,
                                iNodeId: iNodeId
                            }
                            MILesionAjaxData = {
                                url: ApiURL + "GetData/MILesionDetails",
                                type: "POST",
                                async: false,
                                data: MILesionData,
                                success: successMILesionDetail,
                                error: errorMILesionDetail
                            }
                            fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                        }
                        //var btnLesion = "";
                        //btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[0].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[0] + '" data-target="#LesionModel,#MarkModel" title="' + SubActivityNameData[0].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[0].toUpperCase() + '</button>';
                        ////btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel,#MarkModel" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val().toUpperCase() + '</button>';
                        //btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[1].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[1] + '" data-target="#LesionModelNTL,#MarkModel" title="' + SubActivityNameData[1].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[1].toUpperCase() + '</button>';
                        //btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + SubActivityNameData[2].split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" onclick="GetCurrentSubActivityName(this)" data-attribute="' + SubActivityData[2] + '" data-target="#LesionModelNL,#MarkModel" title="' + SubActivityNameData[2].toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + SubActivityNameData[2].toUpperCase() + '</button>';
                        $("#divLesion").append(btnLesion);
                    }
                }
                else {
                    var vWorkspaceId = $("#hdnvWorkspaceId").val();
                    var vActivityId = $("#hdnvSubActivityId").val();
                    var iNodeId = $("#hdniSubNodeId").val();

                    var MILesionData;
                    var MILesionAjaxData;

                    MILesionData = {
                        vWorkspaceId: vWorkspaceId,
                        vActivityId: vActivityId,
                        iNodeId: iNodeId
                    }
                    MILesionAjaxData = {
                        url: ApiURL + "GetData/MILesionDetails",
                        type: "POST",
                        async: false,
                        data: MILesionData,
                        success: successMILesionDetail,
                        error: errorMILesionDetail
                    }
                    fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                }
            }
            else {

                ///////////FOR R1
                if ($("#hdnvSubActivityName").val().toUpperCase().match("R1")) {

                    if ($("#hdnvSubActivityName").val().toUpperCase().match("R1-NTL")) {
                        if ($("#hdncR1NTLReviewStatus").val() == "N") {

                            if ($("#hdncR1TLReviewStatus").val() == "Y" && ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1")) {
                                AlertBox("WARNING", " Dicom Viewer", "Please Select Sub Sequent Image To Mark The Non Target Lesion Details!")
                            }
                            else {
                                var vWorkspaceId = $("#hdnvWorkspaceId").val();
                                var vActivityId = $("#hdnvSubActivityId").val();
                                var iNodeId = $("#hdniSubNodeId").val();

                                var MILesionData;
                                var MILesionAjaxData;

                                MILesionData = {
                                    vWorkspaceId: vWorkspaceId,
                                    vActivityId: vActivityId,
                                    iNodeId: iNodeId
                                }
                                MILesionAjaxData = {
                                    url: ApiURL + "GetData/MILesionDetails",
                                    type: "POST",
                                    async: false,
                                    data: MILesionData,
                                    success: successMILesionDetail,
                                    error: errorMILesionDetail
                                }

                                fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                            }
                        }
                        else {
                            AlertBox("WARNING", " Dicom Viewer", "Image Reviewed!")
                            if ($("#hdniImageStatus").val() == 2) {

                                var divdata = document.getElementById('divLesion')
                                while (divdata.hasChildNodes()) {
                                    divdata.removeChild(divdata.lastChild);
                                }

                                var btnLesion = "";
                                //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL 2" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + "2" + " DETAIL" + '</button>';
                                $("#divLesion").append(btnLesion);
                            }
                        }
                    }


                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("R1-TL")) {
                        if ($("#hdncR1TLReviewStatus").val() == "N") {

                            if ($("#hdncR1NTLReviewStatus").val() == "Y" && ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1")) {
                                AlertBox("WARNING", " Dicom Viewer", "Please Select Sub Sequent Image To Mark The Target Lesion Details!")
                            }
                            else {
                                var vWorkspaceId = $("#hdnvWorkspaceId").val();
                                var vActivityId = $("#hdnvSubActivityId").val();
                                var iNodeId = $("#hdniSubNodeId").val();

                                var MILesionData;
                                var MILesionAjaxData;

                                MILesionData = {
                                    vWorkspaceId: vWorkspaceId,
                                    vActivityId: vActivityId,
                                    iNodeId: iNodeId
                                }

                                MILesionAjaxData = {
                                    url: ApiURL + "GetData/MILesionDetails",
                                    type: "POST",
                                    async: false,
                                    data: MILesionData,
                                    success: successMILesionDetail,
                                    error: errorMILesionDetail
                                }

                                fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                            }
                        }
                        else {
                            AlertBox("WARNING", " Dicom Viewer", "Image Reviewed!")
                            if ($("#hdniImageStatus").val() == 2) {

                                var divdata = document.getElementById('divLesion')
                                while (divdata.hasChildNodes()) {
                                    divdata.removeChild(divdata.lastChild);
                                }

                                var btnLesion = "";
                                //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL 3" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + "3" + '</button>';
                                $("#divLesion").append(btnLesion);
                            }
                        }
                    }

                    else {
                        //Dicom is Reviewed
                        //For Image Flag 2 only Saved Detail Should Display

                        //alert("Image Reviewed")
                        if ($("#hdniImageStatus").val() == 2) {

                            var divdata = document.getElementById('divLesion')
                            while (divdata.hasChildNodes()) {
                                divdata.removeChild(divdata.lastChild);
                            }

                            var btnLesion = "";
                            //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                            btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL 4" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + "4" + '</button>';
                            $("#divLesion").append(btnLesion);
                        }
                    }
                }
                    ///// For R2
                else if ($("#hdnvSubActivityName").val().toUpperCase().match("R2")) {
                    debugger;

                    if ($("#hdnvSubActivityName").val().toUpperCase().match("R2-NTL")) {
                        if ($("#hdncR2NTLReviewStatus").val() == "N") {

                            if ($("#hdncR2TLReviewStatus").val() == "Y" && ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1")) {
                                AlertBox("WARNING", " Dicom Viewer", "Please Select Sub Sequent Image To Mark The Non Target Lesion Details!")
                            }
                            else {
                                var vWorkspaceId = $("#hdnvWorkspaceId").val();
                                var vActivityId = $("#hdnvSubActivityId").val();
                                var iNodeId = $("#hdniSubNodeId").val();

                                var MILesionData;
                                var MILesionAjaxData;

                                MILesionData = {
                                    vWorkspaceId: vWorkspaceId,
                                    vActivityId: vActivityId,
                                    iNodeId: iNodeId
                                }
                                MILesionAjaxData = {
                                    url: ApiURL + "GetData/MILesionDetails",
                                    type: "POST",
                                    async: false,
                                    data: MILesionData,
                                    success: successMILesionDetail,
                                    error: errorMILesionDetail
                                }

                                fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                            }
                        }
                        else {
                            AlertBox("WARNING", " Dicom Viewer", "Image Reviewed!")
                            if ($("#hdniImageStatus").val() == 2) {

                                var divdata = document.getElementById('divLesion')
                                while (divdata.hasChildNodes()) {
                                    divdata.removeChild(divdata.lastChild);
                                }

                                var btnLesion = "";
                                //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                                $("#divLesion").append(btnLesion);
                            }
                        }
                    }


                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("R2-TL")) {
                        if ($("#hdncR2TLReviewStatus").val() == "N") {

                            if ($("#hdncR2NTLReviewStatus").val() == "Y" && ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1")) {
                                AlertBox("WARNING", " Dicom Viewer", "Please Select Sub Sequent Image To Mark The Target Lesion Details!")
                            }
                            else {
                                var vWorkspaceId = $("#hdnvWorkspaceId").val();
                                var vActivityId = $("#hdnvSubActivityId").val();
                                var iNodeId = $("#hdniSubNodeId").val();

                                var MILesionData;
                                var MILesionAjaxData;

                                MILesionData = {
                                    vWorkspaceId: vWorkspaceId,
                                    vActivityId: vActivityId,
                                    iNodeId: iNodeId
                                }

                                MILesionAjaxData = {
                                    url: ApiURL + "GetData/MILesionDetails",
                                    type: "POST",
                                    async: false,
                                    data: MILesionData,
                                    success: successMILesionDetail,
                                    error: errorMILesionDetail
                                }

                                fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                            }
                        }
                        else {
                            AlertBox("WARNING", " Dicom Viewer", "Image Reviewed!")
                            if ($("#hdniImageStatus").val() == 2) {

                                var divdata = document.getElementById('divLesion')
                                while (divdata.hasChildNodes()) {
                                    divdata.removeChild(divdata.lastChild);
                                }

                                var btnLesion = "";
                                //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                                $("#divLesion").append(btnLesion);
                            }
                        }
                    }

                    else {
                        //Dicom is Reviewed
                        //For Image Flag 2 only Saved Detail Should Display

                        //alert("Image Reviewed")
                        if ($("#hdniImageStatus").val() == 2) {

                            var divdata = document.getElementById('divLesion')
                            while (divdata.hasChildNodes()) {
                                divdata.removeChild(divdata.lastChild);
                            }

                            var btnLesion = "";
                            //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                            btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                            $("#divLesion").append(btnLesion);
                        }
                    }


                }
                    ///////////////IF NOT R1 OR R2
                else {
                    //Dicom is Reviewed
                    //For Image Flag 2 only Saved Detail Should Display

                    //alert("Image Reviewed")
                    if ($("#hdniImageStatus").val() == 2) {

                        var divdata = document.getElementById('divLesion')
                        while (divdata.hasChildNodes()) {
                            divdata.removeChild(divdata.lastChild);
                        }

                        var btnLesion = "";
                        //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                        btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                        $("#divLesion").append(btnLesion);
                    }
                }
            }
        }
        else {
            if ($("#hdnActivityID").val() != "") {
                if ($("#hdnActivityID").val() != "undefined") {
                    var LesionData;
                    var LesionAjaxData;

                    LesionData = {
                        vActivityId: $("#hdnActivityID").val(),
                        iNodeId: $("#hdnNodeID").val(),
                        vWorkspaceId: $("#hdnWorkspaceId").val()
                    }

                    LesionAjaxData = {
                        url: ApiURL + "GetData/LesionDetails",
                        type: "POST",
                        async: false,
                        data: LesionData,
                        success: successLesionDetail,
                        error: errorLesionDetail
                    }
                    fnLesionDetail(LesionAjaxData.url, LesionAjaxData.type, LesionAjaxData.data, LesionAjaxData.async, LesionAjaxData.success, LesionAjaxData.error)
                }
            }
        }
    }

        //IF NO IMAGE FOUND IN THIS SUCCESS METHOD THEN SKIP VISIT  -- Workon
    else {
        debugger;
        if (($("#hdnvActivityName").val().toUpperCase().match("GLOBAL")) || ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR"))) {
            //Working
            var vWorkspaceId = $("#hdnvWorkspaceId").val();
            var vActivityId = $("#hdnvSubActivityId").val();
            var iNodeId = $("#hdniSubNodeId").val();

            $("#hdnIsTimePoint").val("false");
            $("#hdnCurrentSubActivityName").val("ALL-ALL");
            $("#hdnvSubActivityId").val(vActivityId);
            $("#hdniSubNodeId").val(iNodeId);
            //$("#hdnvSubActivityName").val(SubActivityNameData[h]);    // Already have value
            //$("#hdnSelectedvChildNodeDisplayName").val(SubActivityNameData[h]);   // Already have value
            $("#hdnSelectedvSubActivityId").val(vActivityId);
            $("#hdnSelectediSubNodeId").val(iNodeId);
          

            localStorage.setItem("ArrayNo", 0);

            var MILesionData;
            var MILesionAjaxData;

            MILesionData = {
                vWorkspaceId: vWorkspaceId,
                vActivityId: vActivityId,
                iNodeId: iNodeId
            }
            MILesionAjaxData = {
                url: ApiURL + "GetData/MILesionDetails",
                type: "POST",
                async: false,
                data: MILesionData,
                success: successMILesionDetail,
                error: errorMILesionDetail
            }
            fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
           
            btnLesion = "";
            //btnLesion += '<button style="text-align: right" type="button" id="TL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelTL,#MarkModel" title="TL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-TL</button>';
            //btnLesion += '<button style="text-align: right" type="button" id="NTL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelNTL,#MarkModel" title="NTL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-NTL</button>';
            //btnLesion += '<button style="text-align: right" type="button" id="NL" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModelNL,#MarkModel" title="NL DETAIL" data-tooltip="tooltip" onclick="GetDetail(this)">' + $("#hdnUserType").val() + '-NL</button>';
            btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '." class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel,#MarkModel" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val().toUpperCase() + '</button>';
            btnLesion += '<button style="text-align: right" type="button" id="Compare" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#GetDetailTL,#MarkModel" title="NL DETAIL" data-tooltip="tooltip" onclick="GetCompareDetail(this)">COMPARE</button>';
            $("#divLesion").append(btnLesion);
            //---------------------------------------------------
        }
        else {
            AlertBox("WARNING", " Dicom Viewer", "Dicom Image For Subject Not Found!")
            if ($("#hdnvSkipVisit").val() == "Y") {

                $("#divViewer").empty();
                $("#divRow").empty();

                var iModifyBy = $("#hdnuserid").val();
                var ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
                var vWorkspaceId = $("#hdnvWorkspaceId").val();
                var vSubjectId = $("#hdnvSubjectId").val();
                var vActivityId = $("#hdnvSubActivityId").val();
                var iNodeId = $("#hdniSubNodeId").val();
                var iMySubjectNo = $("#hdniMySubjectNo").val();
                var ScreenNo = $("#hdnvMySubjectNo").val();
                var PeriodId = $("#hdniPeriod").val();

                var ajaxdata = {
                    vParentWorkSpaceId: ParentWorkSpaceId,
                    vWorkspaceId: vWorkspaceId,
                    vSubjectId: vSubjectId,
                    iMySubjectNo: iMySubjectNo,
                    ScreenNo: ScreenNo,
                    vPeriodId: PeriodId,
                    vActivityId: vActivityId,
                    iNodeId: iNodeId
                }
                $.ajax({
                    url: ApiURL + "GetData/LesionSavedDetailsDATA",
                    //url: ApiURL + "GetData/LesionDetailsDATA",
                    type: "POST",
                    data: ajaxdata,
                    async: false,
                    success: function (jsonData) {

                        if (jsonData == null) {
                            LesionSavedDetailsDATAFlag = true;
                            //AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found
                            //No Lesion Detail So Create Control and Give User UI to submit data
                            var vWorkspaceId = $("#hdnvWorkspaceId").val();
                            var vActivityId = $("#hdnvSubActivityId").val();
                            var iNodeId = $("#hdniSubNodeId").val();

                            var MILesionData;
                            var MILesionAjaxData;

                            MILesionData = {
                                vWorkspaceId: vWorkspaceId,
                                vActivityId: vActivityId,
                                iNodeId: iNodeId
                            }
                            MILesionAjaxData = {
                                url: ApiURL + "GetData/MILesionDetails",
                                type: "POST",
                                async: false,
                                data: MILesionData,
                                success: successMILesionDetail,
                                error: errorMILesionDetail
                            }

                            fnMILesionDetail(MILesionAjaxData.url, MILesionAjaxData.type, MILesionAjaxData.data, MILesionAjaxData.async, MILesionAjaxData.success, MILesionAjaxData.error)
                        }
                        else {
                            //Lesion Detail Found So View Only Data
                            if (jsonData.length > 0) {

                                //This Logic is For To View CRF Table Temp Data in GridView that are also saved in BizNet
                                //Do Not Delete IT

                                AlertBox("WARNING", " Dicom Viewer", "Visit Completed.")

                                var modalData = document.getElementById('MILesionModelData')
                                while (modalData.hasChildNodes()) {
                                    modalData.removeChild(modalData.lastChild);
                                }

                                table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)
                                //var MiLesionDetailModel = $('<div class="modal-dialog">').append($('<div class="modal-content">')).append($('<div class="modal-header">'));

                                var MiLesionDetailModel = '';
                                //var table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)

                                //MiLesionDetailModel += '<div class="modal-dialog detail-dialog">';

                                MiLesionDetailModel += '<div class="modal-dialog" style="width:70%; !important">';
                                MiLesionDetailModel += '<div class="modal-content">';
                                MiLesionDetailModel += '<div class="modal-header">';
                                MiLesionDetailModel += '<button type="button" class="btn btn-info btn-sm pull-right box-tools" data-widget="remove" data-dismiss="modal" data-toggle="tooltip" title="" data-original-title="Remove">';
                                MiLesionDetailModel += '<i class="fa fa-times"></i>';
                                MiLesionDetailModel += '</button>';
                                MiLesionDetailModel += "<h4 class='modal-title'>" + $("#hdnvSubActivityName").val() + " DETAIL" + "</h4>";
                                MiLesionDetailModel += '</div>';
                                MiLesionDetailModel += '<div class="modal-body" id="MILesionModelBodyData" >';

                                //This Logic Is For To Create Dynalic Tabel for Data Do Not Delete It
                                //MiLesionDetailModel += table;


                                MiLesionDetailModel += "<div class='row'>";
                                for (var v = 0; v < jsonData.length; v++) {
                                    var id = null;
                                    var value = null;
                                    var classVal = null;
                                    var type = null;
                                    var placeHolder = null;
                                    var tabIndex = null;
                                    var option = null;
                                    var checked = null;
                                    var name = null;
                                    var controlVal = null;
                                    var forVal = null;

                                    // 08-Dec-2022 remove $("#hdnvActivityName").val().toUpperCase().match("BL") || condition
                                    //if ($("#hdnvActivityName").val().toUpperCase().match("BL") || $("#hdnvSubActivityName").val().toUpperCase().match("BASELINE")) {
                                    if ($("#hdnvSubActivityName").val().toUpperCase().match("BASELINE")) {
                                        if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                            MiLesionDetailModel += "<div class='col-lg-6 col-xs-12 form-group'>";
                                        }
                                        else {
                                            MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                                        }
                                    }
                                    else if ($("#hdnvActivityName").val().toUpperCase().match("MARK")) {
                                        if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                            MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                                        }
                                        else {
                                            MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                                        }
                                    }
                                    else {
                                        if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                            MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                                        }
                                        else {
                                            MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                                        }
                                    }

                                    MiLesionDetailModel += "<div class=col-sm-12>";
                                    //id = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v;
                                    id = jsonData[v].vMedExCode + "_" + v;
                                    value = jsonData[v].vMedExDesc;
                                    MiLesionDetailModel += Label(id, value);
                                    MiLesionDetailModel += "</div>"
                                    MiLesionDetailModel += "<div class=col-sm-12>";

                                    if (jsonData[v].vMedExType.match("TextBox")) {
                                        type = "text";
                                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                                        placeHolder = jsonData[v].vMedExDesc;
                                        tabIndex = v;
                                        value = jsonData[v].vMedExResult
                                        MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                                    }
                                    else if (jsonData[v].vMedExType.match("TextArea")) {
                                        type = "text";
                                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                                        placeHolder = jsonData[v].vMedExDesc;
                                        tabIndex = v;
                                        value = jsonData[v].vMedExResult
                                        MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                                    }
                                    else if (jsonData[v].vMedExType.match("ComboBox")) {
                                        type = "text";
                                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                                        placeHolder = jsonData[v].vMedExDesc;
                                        tabIndex = v;
                                        value = jsonData[v].vMedExResult
                                        MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                                    }
                                    else if (jsonData[v].vMedExType.match("Radio")) {
                                        type = "text";
                                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                                        placeHolder = jsonData[v].vMedExDesc;
                                        tabIndex = v;
                                        value = jsonData[v].vMedExResult
                                        MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                                    }
                                    else {
                                        type = "text";
                                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                                        placeHolder = jsonData[v].vMedExDesc;
                                        tabIndex = v;
                                        value = jsonData[v].vMedExResult
                                        MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                                    }
                                    MiLesionDetailModel += "</div>"
                                    MiLesionDetailModel += "</div>"
                                }

                                MiLesionDetailModel += '</div>';
                                MiLesionDetailModel += '</div>';
                                MiLesionDetailModel += '<div class="modal-footer">';
                                MiLesionDetailModel += '<button type="button" class="btn btn-default pull-left" data-dismiss="modal" id="btnMILesionModelDataClose">Close</button>';
                                MiLesionDetailModel += '</div>';
                                MiLesionDetailModel += '</div>';
                                MiLesionDetailModel += '</div>';

                                $("#MILesionModelData").append(MiLesionDetailModel);

                                var divdata = document.getElementById('divLesion')
                                while (divdata.hasChildNodes()) {
                                    divdata.removeChild(divdata.lastChild);
                                }

                                var btnLesion = "";
                                //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" data-toggle="modal" data-target="#MILesionModelData" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                                $("#divLesion").append(btnLesion);


                                $('.dynamic-control').each(function (index, control) {
                                    if (control.length != 0) {
                                        this.disabled = true;
                                    }
                                })

                            }
                        }
                    },
                    failure: function (response) {
                        AlertBox("error", " Dicom Viewer", "Error" + response.d)
                    },
                    error: function (response) {
                        AlertBox("error", " Dicom Viewer", "Error" + response.d)
                    }
                });

            }
        }
    }
    if (imageIds.length > 500) {
        for (var i = 1; i < 500; i++) {
            cornerstone.loadAndCacheImage(imageIds[i]);
        }
        setInterval(loadingimages(), 1000);
    }
    else {
        for (var i = 1; i < imageIds.length; i++) {
            cornerstone.loadAndCacheImage(imageIds[i]);
        }
    }
}

function errorSubjectImageStudyDetail() {
    AlertBox("error", " Dicom Viewer", "Error While Retriving Dicom Image For Subject!")
}

function loadingimages() {
    for (var i = 500; i < imageIds.length; i++) {
        setInterval(loadimageincachewithInterval(i), 10000);
    }
}

function loadimageincachewithInterval(i) {
    setInterval(function () { cornerstone.loadAndCacheImage(imageIds[i]); console.log(imageIds[i]); }, 20000);
}
//For Biznet
var fnLesionDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: false,
        success: success,
        error: error
    });
}

function successLesionDetail(jsonData) {
    debugger;
    if (jsonData.length > 0) {
        
        var modaldata = document.getElementById('LesionModel')
        while (modaldata.hasChildNodes()) {
            modaldata.removeChild(modaldata.lastChild);
        }

        var divdata = document.getElementById('divLesion')
        while (divdata.hasChildNodes()) {
            divdata.removeChild(divdata.lastChild);
        }

        var btnLesion = "";
        var data = "";
        data += '<div class="modal-dialog"  style="width: 70%; !important overflow: auto; margin: 30px auto;  max-height: 373px;">'
        data += "<div class='modal-content'>";
        data += "<div class='modal-header'>";
        data += "<button type='button' class='btn btn-info btn-sm pull-right box-tools' data-widget='remove' data-dismiss='modal' data-toggle='tooltip' title='' data-original-title='Remove'><i class='fa fa-times'></i></button>";
        if ($("#hdnActivityDef").val() == "TL") {
            data += "<h4 class='modal-title'>Target Lesion</h4>";
            btnLesion += '<button style="text-align: right" type="button" id="btnAddTargetLesion" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel" title="Target Lesion " data-tooltip="tooltip">TL</button>';

        }
        else if ($("#hdnActivityDef").val() == "NTL") {
            data += " <h4 class='modal-title'>Non Target Lesion</h4>";
            btnLesion += '<button style="text-align: right" type="button" id="btnAddNonTargetLesion" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel" title="Non Target Lesion " data-tooltip="tooltip">NTL</button>';
        }
        data += "</div>";
        data += " <div class='modal-body'>";
        data += "  <div class='row'>";
        for (var v = 0; v < jsonData.length; v++) {
            var id = null;
            var value = null;
            var classVal = null;
            var type = null;
            var placeHolder = null;
            var tabIndex = null;
            var option = null;
            var checked = null;
            var name = null;
            var controlVal = null;
            var forVal = null;

            //data += '<label id="' + jsonData[v].vMedExGroupCode + "_" + jsonData.vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v + '" for="' + jsonData[v].vMedExGroupCode + "_" + jsonData.vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v + '">' + jsonData[v].vMedExDesc + '</label>';
            //data += '<input type="text" class="form-control" id="' + jsonData[v].vMedExGroupCode + "_" + jsonData.vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v + '" placeholder="' + jsonData[v].vMedExDesc + '" tabindex="' + v + '">';

            data += "<div class='col-lg-6 col-xs-12 form-group'>";
            data += "<div class=col-sm-12>";
            id = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v;
            value = jsonData[v].vMedExDesc;
            data += Label(id, value);
            data += "</div>"
            data += "<div class=col-sm-12>";
            if (jsonData[v].vMedExType.match("TextBox")) {
                type = "text";
                classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                placeHolder = jsonData[v].vMedExDesc;
                tabIndex = v;
                data += TextBox(type, classVal, id, placeHolder, tabIndex);
            }
            else if (jsonData[v].vMedExType.match("ComboBox")) {
                classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                tabIndex = v;
                option = jsonData[v].vMedExValues
                forVal = jsonData[v].vMedExDesc;
                data += DropDown(classVal, id, option, tabIndex, forVal);
            }
            else if (jsonData[v].vMedExType.match("Radio")) {
                type = "radio";
                name = jsonData[v].vMedExDesc;
                classVal = "dynamic-control rGroup " + jsonData[v].vMedExCode;
                tabIndex = v;
                value = jsonData[v].vMedExCode;
                controlVal = jsonData[v].vMedExValues
                data += Radio(type, classVal, id, name, tabIndex, value, controlVal, checked);
            }

            else {
                type = "text";
                classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                placeHolder = jsonData[v].vMedExDesc;
                tabIndex = v;
                data += TextBox(type, classVal, id, placeHolder, tabIndex);
            }
            data += "</div>"
            data += "</div>"
        }
        //data += "<table id='tblLesionDetail' class='table table-bordered table-striped dataTable'> </table>";
        data += "</div>";
        data += "</div>";
        data += "<div class='modal-footer'>";
        data += "<button type='button' class='btn btn-default pull-left' data-dismiss='modal' id='btnClose'>Close</button>";
        data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='getLesionDetails()' data-toggle='modal' data-target='#LesionModelData'>Lesion Detail</button>";
        
        if ($("#hdnsubjectRejectionDtl").val() != 'Y') {

            data += "<button type='button' class='btn btn-primary' id='btnSaveLesion'>Save changes</button>";
            data += "<button type='button' class='btn btn-primary' id='btnSubmitLesion'>Submit</button>";

        }
        else {
            $.map(arryActivity, function (elementOfArray, indexInArray) {
                if (elementOfArray[0].toUpperCase() == $("#hdnvActivityName").val().toUpperCase()) {
                    if (elementOfArray[1].toUpperCase() == "FALSE") {
                        data += "<button type='button' class='btn btn-primary' id='btnSaveLesion'>Save changes</button>";
                        data += "<button type='button' class='btn btn-primary' id='btnSubmitLesion'>Submit</button>";
                    }
                }
            });
            //if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE") || $("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
            //    data += "<button type='button' class='btn btn-primary' id='btnSaveLesion'>Save changes</button>";
            //    data += "<button type='button' class='btn btn-primary' id='btnSubmitLesion'>Submit</button>";
            //}
        }
        data += "</div>";
        data += "</div>";
        data += "</div>";
        $("#LesionModel").append(data);
        $("#divLesion").append(btnLesion);

        var sizecontrol = document.getElementsByClassName('25041');//Added by Vivek Patel
        if (sizecontrol.length != 0) {
            sizecontrol[0].disabled = true;
        }
        //var sizecontrol = document.getElementsByClassName('25041');
        //sizecontrol[0].value = lineLength;        
    }
}

function errorLesionDetail() {
    AlertBox("ERROR", " Dicom Viewer", "Error While Retriving Lesion Details For BizNet!")
}



//For MI
var fnMILesionDetail = function (url, type, data, async, success, error) {
    $.ajax({
        url: url,
        type: type,
        data: data,
        async: false,
        success: success,
        error: error
    });
}

function successMILesionDetail(jsonData) {
    //alert('Test'); 
    //debugger;        //uncomment
    if (jsonData.length > 0) {

        var strClassName = "";
        var strOnlySubActivityName = "";
        var IsTimePoint = $("#hdnIsTimePoint").val();
        if (IsTimePoint == "true") {
            strClassName = "-" + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1];
            strOnlySubActivityName = "_" + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1];
        }
        else {
            strClassName = "-ALL";
            strOnlySubActivityName = "_ALL";
        }

        var ArrayNo = localStorage.getItem("ArrayNo");
        //alert(ArrayNo);

        //var savedJsonData = []
        var finalData = []
        var LesionData = []
        var MARKData = []
        var BLData = []
        debugger;
        var modaldata;
        if (IsTimePoint == "true") {
            if (ArrayNo == 0) {
                modaldata = document.getElementById('LesionModel');
            }
            else if (ArrayNo == 1) {
                modaldata = document.getElementById('LesionModelNTL');
            }
            else if (ArrayNo == 2) {
                modaldata = document.getElementById('LesionModelNL');
            }
            else if (ArrayNo == 3) {
                modaldata = document.getElementById("LesionModelTL");
            }
        }
        else {
            modaldata = document.getElementById('LesionModel');
        }

        while (modaldata.hasChildNodes()) {
            modaldata.removeChild(modaldata.lastChild);
        }

        ActivityData = decodeURI($("#hdnActivityData").val());

        if (!$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
            var divdata = document.getElementById('divLesion')
            while (divdata.hasChildNodes()) {
                divdata.removeChild(divdata.lastChild);
            }
        }

        if (ArrayNo == 0) {
            var temp = $("#hdnActivityArray").val().split(',');
            //var arryActivity = [];
            for (var r = 0; r < temp.length - 1; r += 2) {
                arryActivity.push([temp[r], temp[r + 1]])
            }
        }


        //For get Mark detail (For Left side)
        if ((!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) && (!($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {

                if (ActivityData.includes("MARK") == true) {
                    var vWorkspaceId = $("#hdnvWorkspaceId").val();
                    var vActivityId = $("#hdnvSubActivityId").val();
                    var iNodeId = $("#hdniSubNodeId").val();
                    var vSubjectId = $("#hdnvSubjectId").val();
                    var iMySubjectNo = $("#hdniMySubjectNo").val();
                    var ScreenNo = $("#hdnvMySubjectNo").val();
                    var vActivityName = "MARK";
                    var vSubActivityName;

                    vSubActivityName = $("#hdnSelectedvChildNodeDisplayName").val()

                    var MILesionMARKData;

                    MILesionMARKData = {
                        vWorkspaceId: vWorkspaceId,
                        vActivityId: vActivityId,
                        iNodeId: iNodeId,
                        vSubjectId: vSubjectId,
                        iMySubjectNo: iMySubjectNo,
                        ScreenNo: ScreenNo,
                        vActivityName: vActivityName,
                        vSubActivityName: vSubActivityName
                    }

                    $.ajax({
                        url: ApiURL + "GetData/MILesionMARKDetails",
                        type: "POST",
                        async: false,
                        data: MILesionMARKData,
                        success: function (MarkjsonData) {
                            if (MarkjsonData.length > 0) {
                                MARKData.push(MarkjsonData);
                            }
                            else {
                                AlertBox("WARNING", " Dicom Viewer", "No Mark Detail Found For " + $("#hdnvSubActivityName").val().toUpperCase() + "! \nFirst Fill the Mark Details For " + $("#hdnvSubActivityName").val().toUpperCase() + " !")
                                return false;
                            }
                        },
                        error: function (e) {
                            AlertBox("WARNING", " Dicom Viewer", "Error While Retriving Mark Details!")
                            return false;
                        }
                    });
                }
            }
        }
        //------------End of get Mark detail (For Left side)------------------


        // For Get BL Deatil (For Left side)
        if ((!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) && (!($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                if ((ActivityData.includes("MARK") == false) && (!($("#hdnvActivityName").val().toUpperCase().match("BL")))) {
                    var vWorkspaceId = $("#hdnvWorkspaceId").val();
                    var vActivityId = $("#hdnvSubActivityId").val();
                    var iNodeId = $("#hdniSubNodeId").val();
                    var vSubjectId = $("#hdnvSubjectId").val();
                    var iMySubjectNo = $("#hdniMySubjectNo").val();
                    var ScreenNo = $("#hdnvMySubjectNo").val();
                    var vActivityName = "BL";
                    var vSubActivityName;

                    vSubActivityName = $("#hdnSelectedvChildNodeDisplayName").val()

                    var MILesionBLData;

                    MILesionBLData = {
                        vWorkspaceId: vWorkspaceId,
                        vActivityId: vActivityId,
                        iNodeId: iNodeId,
                        vSubjectId: vSubjectId,
                        iMySubjectNo: iMySubjectNo,
                        ScreenNo: ScreenNo,
                        vActivityName: vActivityName,
                        vSubActivityName: vSubActivityName
                    }

                    $.ajax({
                        url: ApiURL + "GetData/MILesionMARKDetails",
                        type: "POST",
                        async: false,
                        data: MILesionBLData,
                        success: function (BLjsonData) {
                            if (BLjsonData.length > 0) {
                                BLData.push(BLjsonData);
                            }
                            else {
                                AlertBox("WARNING", " Dicom Viewer", "No BL Detail Found For " + $("#hdnvSubActivityName").val().toUpperCase() + "! \nFirst Fill the BL Details For " + $("#hdnvSubActivityName").val().toUpperCase() + " !")
                                return false;
                            }
                        },
                        error: function (e) {
                            AlertBox("WARNING", " Dicom Viewer", "Error While Retriving BL Details!")
                            return false;
                        }
                    });
                }
            }
        }
        //----------End of Get BL Deatil (For Left side)-------------------------

        //var btnLesion = "";
        var data = "";
        var markdata = ""

        //Fill data in Left side(MARK/BL)
        if ((!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) && (!($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {

                //SET MARK DETAIL FOR EVERY TIME POINT AND ALSO FOR BL
                if ((MARKData[0] != null) && (ActivityData.includes("MARK") == true)) {
                    if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
                        data += '<div class="modal-dialog"  style="width: 38%; float:left; margin: 30px auto; height: 91%;">'
                    }
                    else {
                        data += '<div class="modal-dialog"  style="width: 75%; overflow: auto; margin: 30px auto;  max-height: 800px;">'
                    }

                    data += "<div class='modal-content set-margin' style='height:100%'>";
                    data += "<div class='modal-header'>";
                    //data += "<span><button type='button' style='overflow:hidden !important;' class='btn btn-info btn-sm pull-right box-tools' data-widget='remove' data-dismiss='modal' data-toggle='tooltip' title='' data-original-title='Remove'><i class='fa fa-times'></i></button></span>";
                    data += " <h4 class='modal-title' style=''>" + "MARK  DETAIL" + "</h4>";
                    //btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel" title="' + $("#hdnvSubActivityName").val() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + '</button>';

                    data += "</div>";
                    data += " <div class='modal-body modpaddin' style='height:79%; overflow: auto;'>";
                    data += "  <div class='row'>";

                    for (var k = 0; k < MARKData[0].length; k++) {
                        var id = null;
                        var value = null;
                        var classVal = null;
                        var classvalue = null;
                        var type = null;
                        var placeHolder = null;
                        var tabIndex = null;
                        var option = null;
                        var checked = null;
                        var name = null;
                        var controlVal = null;
                        var forVal = null;
                        var vMedExResult = "";


                        //FOR MARK ACTIVITY AND FOR TARGET AND NON TARGET LESION
                        if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {

                            data += "<div class='col-lg-4 col-xs-12 form-group'>";
                            data += "<div class=col-sm-12>";
                            id = MARKData[0][k].vMedExCode + "_" + k;
                            value = MARKData[0][k].vMedExDesc;
                            data += Label(id, value);
                            data += "</div>"
                            data += "<div class=col-sm-12>";
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                classvalue = "dynamic-ntl-mark-control form-control " + MARKData[0][k].vMedExCode;
                            }
                            else if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                                classvalue = "dynamic-tl-mark-control form-control " + MARKData[0][k].vMedExCode;
                            }

                            if (MARKData[0][k].vMedExType.match("TextBox")) {

                                type = "text";
                                classVal = classvalue;
                                placeHolder = MARKData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = MARKData[0][k].vMedExResult
                                id += "_textbox_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (MARKData[0][k].vMedExType.match("TextArea")) {
                                type = "text";
                                classVal = classvalue;
                                placeHolder = MARKData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = MARKData[0][k].vMedExResult
                                id += "_textbox_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (MARKData[0][k].vMedExType.match("ComboBox")) {
                                type = "text";
                                classVal = classvalue;
                                placeHolder = MARKData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = MARKData[0][k].vMedExResult
                                id += "_dropdown_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (MARKData[0][k].vMedExType.match("Radio")) {
                                type = "radio";
                                name = MARKData[0][k].vMedExDesc;
                                classVal = classvalue;
                                tabIndex = k;
                                value = MARKData[0][k].vMedExCode;
                                controlVal = MARKData[0][k].vMedExValues
                                id += "_radio_" + k;
                                data += Radio(type, classVal, id, name, tabIndex, value, controlVal, checked);
                            }
                            data += "</div>"
                            data += "</div>"
                        }
                    }

                    data += "</div>";
                    data += "</div>";
                    data += "<div class='modal-footer'>";
                    data += "</div>";
                    data += "</div>";
                    data += "</div>";
                }
                //------END OF MARK DETAIL--------------------------------------------------------------------

                // SET BL DETAIL FOR EVERY TIME POINT
                if (BLData[0] != null && (ActivityData.includes("MARK") == false) && (!($("#hdnvActivityName").val().toUpperCase().match("BL")))) {
                    if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
                        data += '<div class="modal-dialog"  style="width: 38%; float:left; margin: 30px auto; height: 91%;">'
                    }
                    else {
                        data += '<div class="modal-dialog"  style="width: 75%; overflow: auto; margin: 30px auto;  max-height: 800px;">'
                    }

                    data += "<div class='modal-content set-margin' style='height:100%'>";
                    data += "<div class='modal-header'>";
                    data += " <h4 class='modal-title' style=''>" + "BL  DETAIL" + "</h4>";

                    data += "</div>";
                    data += " <div class='modal-body modpaddin' style='height:79%; overflow: auto;'>";
                    data += "  <div class='row'>";

                    for (var k = 0; k < BLData[0].length; k++) {
                        var id = null;
                        var value = null;
                        var classVal = null;
                        var classvalue = null;
                        var type = null;
                        var placeHolder = null;
                        var tabIndex = null;
                        var option = null;
                        var checked = null;
                        var name = null;
                        var controlVal = null;
                        var forVal = null;
                        var vMedExResult = "";

                        if (BLData[0][k].vMedExDesc.toUpperCase().match("BL-TL-ASSESSMENT")) {
                            continue;
                        }

                        //FOR BL ACTIVITY AND FOR TARGET AND NON TARGET LESION
                        if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {

                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                if (k == 0 || k == 4 || k == 9 || k == 14 || k == 19 || k == 24 || k == 29 || k == 34 || k == 39 || k == 44 || k == 49 || k == 54) {
                                    data += "<div class='fixed_widh'>";
                                }
                            }
                            else {
                                if (k == 0 || k == 5 || k == 10 || k == 15 || k == 20 || k == 25 || k == 30 || k == 35 || k == 40 || k == 45 || k == 50 || k == 55) {
                                    data += "<div class='fixed_widh'>";
                                }
                            }

                            //data += "<div class='col-lg-4 col-xs-12 form-group'>";
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                data += "<div class='innerboxNTL_width'>";
                            }
                            else {
                                data += "<div class='innerbox_width'>";
                            }
                            data += "<div class=col-sm-12>";
                            id = BLData[0][k].vMedExCode + "_" + k;
                            value = BLData[0][k].vMedExDesc;
                            data += Label(id, value);
                            data += "</div>"
                            data += "<div class=col-sm-12>";
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                classvalue = "dynamic-ntl-mark-control form-control " + BLData[0][k].vMedExCode;
                            }
                            else if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                                classvalue = "dynamic-tl-mark-control form-control " + BLData[0][k].vMedExCode;
                            }

                            if (BLData[0][k].vMedExType.match("TextBox")) {

                                type = "text";
                                classVal = classvalue;
                                placeHolder = BLData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = BLData[0][k].vMedExResult
                                id += "_textbox_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (BLData[0][k].vMedExType.match("TextArea")) {
                                type = "text";
                                classVal = classvalue;
                                placeHolder = BLData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = BLData[0][k].vMedExResult
                                id += "_textbox_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (BLData[0][k].vMedExType.match("ComboBox")) {
                                type = "text";
                                classVal = classvalue;
                                placeHolder = BLData[0][k].vMedExDesc;
                                tabIndex = k;
                                value = BLData[0][k].vMedExResult
                                id += "_dropdown_" + k;
                                data += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                            }
                            else if (BLData[0][k].vMedExType.match("Radio")) {
                                type = "radio";
                                name = BLData[0][k].vMedExDesc;
                                classVal = classvalue;
                                tabIndex = k;
                                value = BLData[0][k].vMedExCode;
                                controlVal = BLData[0][k].vMedExValues
                                id += "_radio_" + k;
                                data += Radio(type, classVal, id, name, tabIndex, value, controlVal, checked);
                            }
                            data += "</div>"
                            data += "</div>"

                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                if (k == 3 || k == 8 || k == 13 || k == 18 || k == 23 || k == 28 || k == 33 || k == 38 || k == 43 || k == 48 || k == 53 || k == 58) {
                                    data += "</div>"
                                }
                            }
                            else {
                                if (k == 4 || k == 9 || k == 14 || k == 19 || k == 24 || k == 29 || k == 34 || k == 39 || k == 44 || k == 49 || k == 54 || k == 59) {
                                    data += "</div>"
                                }
                            }
                        }
                    }
                    data += "</div>";
                    data += "</div>";
                    data += "<div class='modal-footer'>";
                    data += "</div>";
                    data += "</div>";
                    data += "</div>";
                    data += "</div>";   //Edited by Vasimkhan Pathan 
                }
                //------END OF BL DETAIL-------------------------------------------------------------------
            }
        }
        //------END OF Fill data in left side-------------------------------------------------------------------


        var iModifyBy = $("#hdnuserid").val();
        var ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
        var vWorkspaceId = $("#hdnvWorkspaceId").val();
        var vSubjectId = $("#hdnvSubjectId").val();
        var vActivityId = $("#hdnvSubActivityId").val();
        var iNodeId = $("#hdniSubNodeId").val();
        var iMySubjectNo = $("#hdniMySubjectNo").val();
        var ScreenNo = $("#hdnvMySubjectNo").val();
        var PeriodId = $("#hdniPeriod").val();

        var ajaxdata = {
            vParentWorkSpaceId: ParentWorkSpaceId,
            vWorkspaceId: vWorkspaceId,
            vSubjectId: vSubjectId,
            iMySubjectNo: iMySubjectNo,
            ScreenNo: ScreenNo,
            vPeriodId: PeriodId,
            vActivityId: vActivityId,
            iNodeId: iNodeId
        }


        //To Manage call of LesionSavedDetailsDATA for SkipVisit and Regular Visit
        if (!LesionSavedDetailsDATAFlag) {
            $.ajax({
                url: ApiURL + "GetData/LesionSavedDetailsDATA",
                type: "POST",
                data: ajaxdata,
                async: false,
                success: function (jsonSavedData) {

                    if (jsonSavedData == null) {
                        savedJsonData = [];
                        //AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found!")
                    }
                    else {
                        if (jsonSavedData.length > 0) {
                            savedJsonData = jsonSavedData

                            if (IsTimePoint == "true") {
                                if (ArrayNo == 0) {
                                    savedJsonDataTL = jsonSavedData;
                                }
                                else if (ArrayNo == 1) {
                                    savedJsonDataNTL = jsonSavedData;
                                }
                                else if (ArrayNo == 2) {
                                    savedJsonDataNL = jsonSavedData;
                                }
                            }
                        }
                    }
                    

                },
                error: function (e) {
                }
            });
        }
        else {
            //savedJsonData = null;
            savedJsonData = [];
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////khan
        if ((($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) || (($("#hdnvActivityName").val().toUpperCase().match("RESPONCE")))) {
            data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
        }
        else if ((($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")))) {
            data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
        }
        else if (!($("#hdnvActivityName").val().toUpperCase().match("MARK")) && !($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW"))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                if (($("#hdnvActivityName").val().toUpperCase().match("BL")) && (ActivityData.includes("MARK") == false)) {
                    data += '<div class="modal-dialog"  style="width: 100%; float:right;  margin: 30px auto; height: 91% !important;" tabindex="-1">'
                }
                else {
                    data += '<div class="modal-dialog"  style="width: 62%; float:right;  margin: 30px auto; height: 91% !important;" tabindex="-1">'
                }

            }
            else {
                data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
            }
        }
        else {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                data += '<div class="modal-dialog"  style="width: 70%; margin: 30px auto;  height: 85%;">'
            }
            else {
                data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
            }

        }

        if ($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")) {
            data += " <div class='modal-content set-margin' style='height:45% !important;'>";
        }
        else {
            data += "<div class='modal-content set-margin' style='height:100% !important;'>";
        }
        data += "<div class='modal-header'>";
        data += "<button type='button' class='btn btn-info btn-sm pull-right box-tools' data-widget='remove' data-dismiss='modal' data-toggle='tooltip' title='' data-original-title='Remove'><i class='fa fa-times'></i></button>";
        //data += " <h4 class='modal-title'>" + $("#hdnvSubActivityName").val() + " DETAIL" + "</h4>";
        if (bttitle.toUpperCase() == "") {
            data += " <h4 class='modal-title'>" + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + "</h4>";//khan
        }
        else {
            data += " <h4 class='modal-title'>" + bttitle.toUpperCase() + " DETAIL" + "</h4>";//khan 
        }


        data += "</div>";
        if (($("#hdnvActivityName").val().toUpperCase().match("IOV ASSESSMENT"))) {
            data += " <div class='modal-body padding' style='height:79% !important; overflow: auto !important; '>";
        }
        else {
            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK")) && !($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW"))) {
                data += " <div class='modal-body' style='height:79% !important; overflow: auto !important; '>";
            }
            else if ($("#hdnvActivityName").val().toUpperCase().match("ELIGIBILITY-REVIEW")) {
                data += " <div class='modal-body' style='height:50% !important; overflow: auto !important; '>";
            }
            else {
                data += " <div class='modal-body' style='height:77% !important; overflow: auto !important; '>";
            }
        }
        data += "  <div class='row'>";
        for (var v = 0; v < jsonData.length; v++) {
            var id = null;
            var value = null;
            var classVal = null;
            var classValue = null;
            var type = null;
            var placeHolder = null;
            var tabIndex = null;
            var option = null;
            var checked = null;
            var name = null;
            var controlVal = null;
            var forVal = null;

            if (jsonData[v].vMedExDesc.toUpperCase().match("BL-TL-ASSESSMENT")) {
                continue;
            }

            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            }

            //FOR MARK ACTIVITY AND TARGET LESION
            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            }

            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {

                if ($("#hdnvActivityName").val().toUpperCase().match("BL")) {
                    if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-3 col-xs-12 form-group'>";
                        }
                    }

                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            if (ActivityData.includes("MARK") == false) {
                                data += "<div class='col-lg-15 col-xs-12 form-group'>";
                            }
                            else {
                                data += "<div class='col-lg-4 col-xs-12 form-group'>";
                            }
                        }
                    }
                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-6 col-xs-12 form-group'>";
                        }
                    }
                    else {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-6 col-xs-12 form-group'>";
                        }
                    }
                }
                else if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                    if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                        data += "<div class='col-lg-3 col-xs-12 form-group'>";
                    }
                }
                else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                    if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                        data += "<div class='col-lg-6 col-xs-12 form-group'>";
                    }
                }


                else {
                    if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-4 col-xs-12 form-group'>";
                        }
                    }
                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-4 col-xs-12 form-group'>";
                        }
                    }
                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-6 col-xs-12 form-group'>";
                        }
                    }
                    else if ($("#hdnvSubActivityName").val().toUpperCase().match("-ELIGIBILITY-REVIEW") || $("#hdnvSubActivityName").val().toUpperCase().match("-REVIEW")) {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-4 col-xs-12 form-group'>";
                        }
                    }
                    else {
                        if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                            data += "<div class='col-lg-6 col-xs-12 form-group'>";
                        }
                    }
                }
            }
            else {
                if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                    data += "<div class='col-lg-4 col-xs-12 form-group'>";
                }
            }

            id = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v;

            var vMedEx = jsonData[v].vMedExDesc.replace(/ /g, "_").toUpperCase().split("_")
            if (savedJsonData != null) {
                if (savedJsonData.length > 0) {
                    for (var k = 0 ; k < savedJsonData.length ; k++) {
                        if (savedJsonData[k].vMedExCode == jsonData[v].vMedExCode) {
                            if (savedJsonData[k].vMedExResult != "" || savedJsonData[k].vMedExResult != null) {
                                vMedExResult = savedJsonData[k].vMedExResult;
                                break;
                            }
                        }
                    }
                }
            }

            //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
            if (MARKData[0] != null) {
                var b = 0;
                var arry = [];

                for (var a = 0; a < MARKData[0].length - 2; a += 3) {
                    b = b + 1;
                    var ORGANFlag = false, LOCATIONFlag = false;
                    if ('ORGAN' + b == MARKData[0][a + 1].vMedExDesc.toUpperCase()) {
                        if (MARKData[0][a + 1].vMedExResult != "" && MARKData[0][a + 1].vMedExResult != null) {
                            ORGANFlag = true;

                        }
                    }
                    if ('LOCATION' + b == MARKData[0][a + 2].vMedExDesc.toUpperCase()) {
                        if (MARKData[0][a + 2].vMedExResult != "" && MARKData[0][a + 1].vMedExResult != null) {
                            LOCATIONFlag = true;
                        }
                    }
                    arry.push([ORGANFlag, LOCATIONFlag]);
                }
            }
            //
            //debugger;
            if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                //rinkal
                //if (jsonData[v].vMedExDesc.match("Is image quality adequate?")) {
                //    data += "<div>"
                //    data += "<br>"
                //    data += "<br>"
                //    data += "</div>"
                //}
                value = jsonData[v].vMedExDesc;
                if (value.toUpperCase().match("EXTRA")) {
                    data += "<div class=col-sm-12>";
                    //var result = value.attr('class','extra')
                    //textboxId = id;
                    var classval = "extra"
                    data += Label(id, value, classval);
                }               
                else {
                    data += "<div class=col-sm-12>";
                    //Nikunj
                    if (value == "Remarks" || value == "Remark") {
                        var result = value.fontcolor("Aqua");
                        data += Label(id, result);
                    }
                    else if (value.match("-Assessment") && !$("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE")) {
                        var result = value.fontcolor("Aqua");
                        data += Label(id, result);
                    }
                    else if (value.match("Sum of Diameters")) {
                        var result = value.fontcolor("Aqua");
                        data += Label(id, result);
                    }
                    else if (value.match("-Overall Response")) {
                        var result = value.fontcolor("Aqua");
                        data += Label(id, result);
                    }
                    else if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE")) {
                        if (value.match("-Global Response") || value.match("BOR Response")) {
                            var result = value.fontcolor("Aqua");
                            data += Label(id, result);
                        }
                        else {
                            data += Label(id, value);
                        }
                    }
                    else {
                        data += Label(id, value);
                    }
                }
                //data += Label(id, value);
                data += "</div>"
                data += "<div class=col-sm-12>";
            }

            if (jsonData[v].vMedExType.match("TextBox")) {
                if (jsonData[v].vMedExDesc.toUpperCase().match("SIZE")) {
                    var type1 = "checkbox";
                    var type2 = "text";
                    tabIndex = v;
                    var id1 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "checkbox" + v;
                    var id2 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "textbox" + v;
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExDesc + " " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control" + strClassName + " form-control dynamic-size-textbox-control " + jsonData[v].vMedExDesc + " " + jsonData[v].vMedExCode + " textbox_" + v + "";
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                        textValue = parseFloat(textValue).toFixed(2);
                        classVal2 += " dynamic-saved-size-textbox-control"
                        classVal1 += " disableControl"
                    }

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal1 += " disableControl"
                                }
                                else {
                                    classVal1 += " enableControl"
                                }
                            }
                        }
                    }
                    //

                    data += TextBoxWithCheckBox(type1, type2, classVal1, classVal2, id1, id2, placeHolder, tabIndex, textValue);
                }
                else if (vMedEx.length >= 3) {
                    if (vMedEx.length > 3) {
                        if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val().toUpperCase())) {
                            classValue = "DIAMETERS-" + vMedEx[3].toUpperCase() + " " + "dynamic-diameter-sum-control"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && (vMedEx[3] != $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() || (!(vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val()))))) {
                            if (jsonData[v].vMedExDesc.toUpperCase().match("-IOV ASSESSMENT") && jsonData[v].vMedExDesc.toUpperCase().match("SUM OF DIAMETERS")) {
                                classValue = "DIAMETERS-" + vMedEx[3].toUpperCase() + " " + vMedEx[4].toUpperCase()
                            } else {
                                classValue = "DIAMETERS-" + vMedEx[3].toUpperCase()
                            }
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            if (jsonData[v].vMedExDesc.toUpperCase().match("-IOV ASSESSMENT") && jsonData[v].vMedExDesc.toUpperCase().match("SUM OF DIAMETERS")) {
                                classVal += " dynamic-diameter-sum-control";
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if (vMedEx[2].match("NADIR")) {
                            classValue = "NADIR " + vMedEx[3] + "-" + vMedEx[2]
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if (vMedEx[2].match("IOV")) {
                            classValue = vMedEx[2].split("(")[1] + " " + vMedEx[3].split(")")[0];
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else {
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                    }
                    else if (vMedEx.length > 2) {
                        if ((vMedEx[0].match("%") || vMedEx[1].match("CHANGE") || vMedEx[1].match("CHANGES")) && (vMedEx[2].match($("#hdnSelectedvParentNodeDisplayName").val()))) {
                            classValue = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val() + "-BL"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if ((vMedEx[0].match("%") || vMedEx[1].match("CHANGE") || vMedEx[1].match("CHANGES")) && (vMedEx[2] != $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() || (!(vMedEx[2].match($("#hdnSelectedvParentNodeDisplayName").val()))))) {
                            var v1k1 = jsonData[v].vMedExDesc
                            var v1 = v1k1.replace(/ /g, "_").toUpperCase().split("_");
                            var k1 = v1[2].substr(1, 3);
                            classValue = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + k1 + "-BL"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else {
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }

                    }
                    else {
                        type = "text";
                        classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                        placeHolder = jsonData[v].vMedExDesc;
                        tabIndex = v;
                        id += "_textbox_" + v;
                        var textValue = null;
                        if (vMedExResult != "" && vMedExResult != null) {
                            textValue = vMedExResult
                        }
                        data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                    }

                }
                else {
                    type = "text";
                    classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    id += "_textbox_" + v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                    }

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal += " disableControl"
                                }
                                else {
                                    classVal += " enableControl"
                                }
                            }
                        }
                    }
                    //
                    if (jsonData[v].vMedExDesc.toUpperCase().match("EXTRA")) {
                        classVal += " extra"
                    }
                    data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                }
            }

            else if (jsonData[v].vMedExType.match("TextArea")) {
                if (jsonData[v].vMedExDesc.toUpperCase().match("SIZE")) {
                    var type1 = "checkbox";
                    var type2 = "text";
                    tabIndex = v;
                    var id1 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "checkbox" + v;
                    var id2 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "textbox" + v;
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExDesc + " " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control" + strClassName + " form-control dynamic-size-textbox-control " + jsonData[v].vMedExCode + " textbox_" + v + "";
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                        textValue = parseFloat(textValue).toFixed(2);
                    }

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal1 += " disableControl"
                                }
                                else {
                                    classVal1 += " enableControl"
                                }
                            }
                        }
                    }
                    //

                    data += TextBoxWithCheckBox(type1, type2, classVal1, classVal2, id1, id2, placeHolder, tabIndex, textValue);
                }

                else if (vMedEx.length >= 3) {
                    if (vMedEx.length > 3) {
                        if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val().toUpperCase())) {
                            classValue = "DIAMETERS-" + vMedEx[3].toUpperCase() + " " + "dynamic-diameter-sum-control"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && (vMedEx[3] != $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() || (!(vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val()))))) {
                            classValue = "DIAMETERS-" + vMedEx[3].toUpperCase()
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if (vMedEx[2].match("NADIR")) {
                            classValue = "NADIR " + vMedEx[3] + "-" + vMedEx[2]
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else {
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                            }

                            //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                            var hasNumber = /\d/;
                            if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                                var indexOfArray = "";
                                if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                                    if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                        indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                                    }
                                }
                                else {
                                    indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                                }

                                if (arry != undefined) {
                                    if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                        if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                            classVal += " disableControl"
                                        }
                                        else {
                                            classVal += " enableControl"
                                        }
                                    }
                                }
                            }
                            //


                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                    }
                    else if (vMedEx.length > 2) {
                        if ((vMedEx[0].match("%") || vMedEx[1].match("CHANGE") || vMedEx[1].match("CHANGES")) && (vMedEx[2].match($("#hdnSelectedvParentNodeDisplayName").val()))) {
                            classValue = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val() + "-BL"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else if ((vMedEx[0].match("%") || vMedEx[1].match("CHANGE") || vMedEx[1].match("CHANGES")) && (vMedEx[2] != $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() || (!(vMedEx[2].match($("#hdnSelectedvParentNodeDisplayName").val()))))) {
                            var v1k1 = jsonData[v].vMedExDesc
                            var v1 = v1k1.replace(/ /g, "_").toUpperCase().split("_");
                            var k1 = v1[2].substr(1, 3);
                            classValue = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + k1 + "-BL"
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                                textValue = parseFloat(textValue).toFixed(2);
                            }
                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }
                        else {
                            type = "text";
                            classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            id += "_textbox_" + v;
                            var textValue = null;
                            if (vMedExResult != "" && vMedExResult != null) {
                                textValue = vMedExResult
                            }

                            //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                            var hasNumber = /\d/;
                            if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                                var indexOfArray = "";
                                if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                                    if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                        indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                                    }
                                }
                                else {
                                    indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                                }

                                if (arry != undefined) {
                                    if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                        if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                            classVal += " disableControl"
                                        }
                                        else {
                                            classVal += " enableControl"
                                        }
                                    }
                                }
                            }
                            //

                            data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                        }

                    }
                    else {
                        type = "text";
                        classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                        placeHolder = jsonData[v].vMedExDesc;
                        tabIndex = v;
                        id += "_textbox_" + v;
                        var textValue = null;
                        if (vMedExResult != "" && vMedExResult != null) {
                            textValue = vMedExResult
                        }

                        //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                        var hasNumber = /\d/;
                        if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                            var indexOfArray = "";
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                                if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                    indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                                }
                            }
                            else {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                            }

                            if (arry != undefined) {
                                if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                    if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                        classVal += " disableControl"
                                    }
                                    else {
                                        classVal += " enableControl"
                                    }
                                }
                            }
                        }
                        //
                        data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                    }

                }
                else {
                    type = "text";
                    classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    id += "_textbox_" + v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                    }

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal += " disableControl"
                                }
                                else {
                                    classVal += " enableControl"
                                }
                            }
                        }
                    }
                    //

                    data += TextBox(type, classVal, id, placeHolder, tabIndex, textValue);
                }
            }
            else if (jsonData[v].vMedExType.match("ComboBox")) {
                //debugger;
                if (jsonData[v].vMedExDesc.toUpperCase().match("LESION")) {
                    var type1 = "checkbox";
                    tabIndex = v;
                    var id1 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "checkbox" + v;
                    var id2 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "dropdown" + v;
                    var comboValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        comboValue = vMedExResult
                    }
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExDesc + " " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control" + strClassName + " form-control dynamic-size-dropdown-control " + jsonData[v].vMedExCode + " dropdown" + v + "";
                    option = jsonData[v].vMedExValues
                    forVal = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    lesionType = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1];

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity
                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal2 += " disableControl"
                                }
                                else {
                                    classVal2 += " enableControl"
                                }
                            }
                        }
                    }
                    //

                    data += DropDownWithCheckBox(type1, classVal1, classVal2, id1, id2, option, tabIndex, forVal, lesionType, comboValue);
                }
                else {
                    classVal = "dynamic-control" + strClassName + " form-control " + jsonData[v].vMedExCode;
                    tabIndex = v;
                    option = jsonData[v].vMedExValues
                    forVal = jsonData[v].vMedExDesc;
                    id += "_dropdown_" + v;
                    var comboValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        comboValue = vMedExResult
                    }

                    //To Disable the controls for TL and NTL that are not going to used for marking purpose for all subsequent Activity

                    var hasNumber = /\d/;
                    if (hasNumber.test(jsonData[v].vMedExDesc) == true) {
                        var indexOfArray = "";
                        if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[0])) == true) {
                            if (isNaN(parseInt(jsonData[v].vMedExDesc.slice(-2)[1])) == false) {
                                indexOfArray = jsonData[v].vMedExDesc.slice(-2)[1]
                            }
                        }
                        else {
                            indexOfArray = jsonData[v].vMedExDesc.slice(-2)
                        }

                        if (arry != undefined) {
                            if (arry.length > indexOfArray - 1 && indexOfArray != "") {
                                if (arry[indexOfArray - 1][0] != true && arry[indexOfArray - 1][1] != true) {
                                    classVal += " disableControl"
                                }
                                else {
                                    classVal += " enableControl"
                                }
                            }
                        }
                    }
                    //
                    
                    //if (jsonData[v].vMedExDesc.toUpperCase().match("BL-TL-ASSESSMENT")) {
                    //    continue;
                    //}                    

                    data += DropDown(classVal, id, option, tabIndex, forVal, comboValue);
                }
            }
            else if (jsonData[v].vMedExType.match("Radio")) {
                type = "radio";
                name = jsonData[v].vMedExDesc;
                classVal = "dynamic-control" + strClassName + " rGroup " + jsonData[v].vMedExCode;
                tabIndex = v;
                value = jsonData[v].vMedExCode;
                controlVal = jsonData[v].vMedExValues
                id += "_dropdown_" + v;
                data += Radio(type, classVal, id, name, tabIndex, value, controlVal);
            }
            //To Remove Div Space when get Label Heading
            //if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
            //    data += "</div>"
            //    data += "</div>"
            //}

            //comment by rinkal
            if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                //nikunj  
                //if ($("#hdnvActivityName").val().toUpperCase().match("BL") || $("#hdnvActivityName").val().toUpperCase().match("TP2") || $("#hdnvActivityName").val().toUpperCase().match("TP3")) {
                if (!$("#hdnvActivityName").val().toUpperCase().match("MARK")) {
                    //if ($("#hdnvSubActivityName").val().toUpperCase().match("NL")) {
                    //    if (jsonData[v].vMedExDesc.match("Lesion5") || jsonData[v].vMedExDesc.match("Size5") || jsonData[v].vMedExDesc.match("Description5") || jsonData[v].vMedExDesc.match("Response5")) {
                    //        data += "<div>"
                    //        data += "<hr>"
                    //        data += "</div>"
                    //    }
                    //}
                    //else if (jsonData[v].vMedExDesc.match("Lesion7") || jsonData[v].vMedExDesc.match("Organ7") || jsonData[v].vMedExDesc.match("Location7") || jsonData[v].vMedExDesc.match("Size7") || jsonData[v].vMedExDesc.match("Description7") || jsonData[v].vMedExDesc.match("Response7")) {
                    //    data += "<div>"
                    //    data += "<hr>"
                    //    data += "</div>"
                    //}
                }
                if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE")) {
                    if (jsonData[v].vMedExDesc.match("-NL-Assessment") || jsonData[v].vMedExDesc.match("-Overall Response") || jsonData[v].vMedExDesc.match("-Global Response") || jsonData[v].vMedExDesc.match("-Comments")) {
                        data += "<div>"
                        data += "<hr>"
                        data += "</div>"
                    }
                }

                //else if (jsonData[v].vMedExDesc.match("Is image quality adequate?")) {
                //    data += "<div>"
                //    data += "<br>"
                //    data += "</div>"
                //}
                //}

                data += "</div>"
                data += "</div>"
            }

            //if (jsonData[v].vMedExDesc.match("-TL-Assessment")) {
            //    data += "<div>"
            //    data += "<label id>"
            //    data += "</label>"
            //    data += "</div>"
            //    data += "</div>"
            //}

            //if (jsonData[v].vMedExDesc.match("-TL-Assessment")) {
            //    data += "<div>"
            //    data += "<label id>"
            //    data += "</label>"
            //    data += "</div>"
            //    //data += "</div>"
            //}

        }
        //data += "<table id='tblLesionDetail' class='table table-bordered table-striped dataTable'> </table>";
        //Change by rinkal
        data += "</div>";
        data += "</div>";
        data += "<div class='modal-footer'>";
        data += "<button type='button' class='btn btn-default pull-left' data-dismiss='modal' id='btnClose'>Close</button>";
        //data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
        var sizeData
        var clear
        debugger;
        if (!$("#hdnUserTypeName").val().toUpperCase().match("ADJUDICATOR") && $("#hdnWorkFlowStageId").val() == '' && (isVisit == 'N' || isVisit == '')) {
            sizeData = "";
            clear = "";
            //sizeData = "<button type='button' class='btn btn-default pull-left' id='btnAddTLSize' onclick='MILesionAddSize()'>Add Size</button>";
            clear = "<button type='button' class='btn btn-default pull-left' id='btnClearLesionData' onclick='MIClearLesionData()'>Clear</button>";
            data += sizeData;
            data += clear;
            if ($("#hdnsubjectRejectionDtl").val() != 'Y') {
                data += "<button type='button' class='btn btn-primary btn-hide-save" + strClassName + "' onclick='SaveMIFinalLession()' id='btnMIFinalSaveLesion'>Save changes</button>";
                data += "<button type='button' class='btn btn-primary btn-hide" + strClassName + "' onclick='btnMIFinalSubmitLesionClick()' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
            }
            else {
                $.map(arryActivity, function (elementOfArray, indexInArray) {
                    if (elementOfArray[0].toUpperCase() == $("#hdnvActivityName").val().toUpperCase()) {
                        if (elementOfArray[1].toUpperCase() == "FALSE") {
                            data += "<button type='button' class='btn btn-primary btn-hide-save" + strClassName + "' onclick='SaveMIFinalLession()' id='btnMIFinalSaveLesion'>Save changes</button>";
                            data += "<button type='button' class='btn btn-primary btn-hide" + strClassName + "' onclick='btnMIFinalSubmitLesionClick()' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
                        }
                    }
                });
            }
        }
        else {
            if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") && $("#hdnvSubActivityName").val().split("-")[0].toUpperCase().match("ADJUDICATOR") && (isVisit == 'N' || isVisit == '') && $("#hdnWorkFlowStageId").val() == '') {
                sizeData = "";
                clear = "";
                //sizeData = "<button type='button' class='btn btn-default pull-left' id='btnAddTLSize' onclick='MILesionAddSize()'>Add Size</button>";
                clear = "<button type='button' class='btn btn-default pull-left' id='btnClearLesionData' onclick='MIClearLesionData()'>Clear</button>";
                data += sizeData;
                data += clear;
                if ($("#hdnvSubActivityName").val().split('-')[0].toUpperCase().match("ADJUDICATOR") && bttitle.toUpperCase() == "") {
                    if ($("#hdnsubjectRejectionDtl").val() != 'Y') {
                        data += "<button type='button' class='btn btn-primary btn-hide-save" + strClassName + "' onclick='SaveMIFinalLession()' id='btnMIFinalSaveLesion2'>Save changes</button>";
                        data += "<button type='button' class='btn btn-primary btn-hide" + strClassName + "' onclick='btnMIFinalSubmitLesionClick()' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
                    }
                    else {
                        $.map(arryActivity, function (elementOfArray, indexInArray) {
                            if (elementOfArray[0].toUpperCase() == $("#hdnvActivityName").val().toUpperCase()) {
                                if (elementOfArray[1].toUpperCase() == "FALSE") {
                                    data += "<button type='button' class='btn btn-primary btn-hide-save" + strClassName + "' onclick='SaveMIFinalLession()' id='btnMIFinalSaveLesion'>Save changes</button>";
                                    data += "<button type='button' class='btn btn-primary btn-hide" + strClassName + "' onclick='btnMIFinalSubmitLesionClick()' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
                                }
                            }
                        });
                    }
                }
            }
        }

        data += "</div>";
        data += "</div>";
        data += "</div>";

        // Hiren 
        //$("#LesionModel").append(data);
        if (IsTimePoint == "true") {
            if (ArrayNo == 0) {
                $("#LesionModel").append(data);
            }
            else if (ArrayNo == 1) {
                $("#LesionModelNTL").append(data);
            }
            else if (ArrayNo == 2) {
                $("#LesionModelNL").append(data);
            }
            else if (ArrayNo == 3) {
                $("#LesionModelTL").append(data);
            }
        }
        else {
            $("#LesionModel").append(data);
        }
        //-----------------------------       

        $(".disableControl").attr('disabled', 'disabled');
        $(".extra").css('visibility', 'hidden');
        //$("#textboxId").css('visibility', 'hidden');
        $(".padding").css('padding', '0px');
        //$(".enableControl").

        bindevent();

        if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                $('.dynamic-tl-mark-control').each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.STATISTICS-CONTROL').each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
            }
            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                $('.dynamic-ntl-mark-control').each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
            }
        }

        if ($("#hdnvActivityName").val().toUpperCase().match("BL")) {
            if ($("#hdnvSubActivityName").val().toUpperCase().match("TL")) {
                $('.dynamic-diameter-sum-control').each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
            }
        }

        

        $('.dynamic-control' + strClassName).each(function (index, control) {
            if (control.type == "text" || control.type == "TextArea") {
                if (control.placeholder.toUpperCase().match("SIZE")) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                }
            }
        })

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //To Get Previous Visit Data
        //debugger;     //uncomment
        if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            if (!($("#hdnvActivityName").val().toUpperCase().match("BL"))) {
                if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                    MI_NLDetails();
                }
                else {
                    var OPMODE;
                    if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                        OPMODE = 3;
                    }
                    else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                        OPMODE = 4;
                    }
                    else {
                        OPMODE = 1;
                    }

                    var MILesionStatisticsDetails = {}
                    MILesionStatisticsDetails = {
                        //MODE: 1,
                        MODE: OPMODE,
                        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                        vWorkspaceId: $("#hdnvWorkspaceId").val(),
                        vSubjectId: $("#hdnvSubjectId").val(),
                        iMySubjectNo: $("#hdniMySubjectNo").val(),
                        ScreenNo: $("#hdnvMySubjectNo").val(),
                        Radiologist: $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0],
                        vActivity: $("#hdnSelectedvParentNodeDisplayName").val(),
                        vSubActivity: $("#hdnSelectedvChildNodeDisplayName").val(),
                        vParentActivityId: $("#hdnSelectedvActivityId").val(),
                        iParentNodeId: $("#hdnSelectediPeriod").val(),
                        vActivityId: $("#hdnSelectedvSubActivityId").val(),
                        iNodeId: $("#hdnSelectediSubNodeId").val(),
                        cSaveStatus: 'Y'
                    }

                    $.ajax({
                        url: ApiURL + "GetData/LesionStatisticsDetails",
                        type: "POST",
                        async: false,
                        data: MILesionStatisticsDetails,
                        success: function (jsonData) {
                            if (jsonData != null) {
                                if (jsonData.length > 0) {
                                    for (var v = 0; v < jsonData.length; v++) {
                                        $('.dynamic-control' + strClassName).each(function (index, control) {
                                            if (control.type == "text" || control.type == "TextArea") {
                                                if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {

                                                    if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("TL-ASSESSMENT")))) {
                                                    }
                                                    else if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("NTL-ASSESSMENT")))) {
                                                    }
                                                    else {
                                                        if (control.placeholder.toUpperCase() == jsonData[v].vMedExDesc.toUpperCase()) {
                                                            if (control.length != 0) {
                                                                //control.value = parseFloat(jsonData[v].vMedExResult)
                                                                control.value = jsonData[v].vMedExResult
                                                                this.disabled = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else {
                                                    if (control.placeholder.toUpperCase() == jsonData[v].vMedExDesc.toUpperCase()) {
                                                        if (control.length != 0) {
                                                            //control.value = parseFloat(jsonData[v].vMedExResult)
                                                            control.value = jsonData[v].vMedExResult
                                                            this.disabled = true;
                                                        }
                                                    }
                                                }

                                            }
                                            else if (control.type == "select-one") {
                                                if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {

                                                    if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("TL-ASSESSMENT")))) {
                                                    }
                                                    else if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("NTL-ASSESSMENT")))) {
                                                    }
                                                    else {
                                                        if (jsonData[v].vMedExDesc == control.name) {
                                                            //control.selectedOptions[0].text = jsonData[v].vMedExResult
                                                            //control.selectedIndex = 1;
                                                            var searchtext = jsonData[v].vMedExResult;
                                                            for (var i = 0; i < control.options.length; ++i) {
                                                                if (control.options[i].text === searchtext) control.options[i].selected = true;
                                                                this.disabled = true;
                                                            }
                                                        }
                                                    }
                                                }
                                                else {
                                                    if (jsonData[v].vMedExDesc == control.name) {
                                                        //control.selectedOptions[0].text = jsonData[v].vMedExResult
                                                        //control.selectedIndex = 1;
                                                        var searchtext = jsonData[v].vMedExResult;
                                                        for (var i = 0; i < control.options.length; ++i) {
                                                            if (control.options[i].text === searchtext) control.options[i].selected = true;
                                                            this.disabled = true;
                                                        }
                                                    }
                                                }

                                            }

                                            if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                                if (control.type == "text" || control.type == "TextArea") {
                                                    if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                                        this.disabled = true;
                                                    }
                                                }
                                                else if (control.type == "select-one") {
                                                    if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                                        if (!(control.name.toUpperCase().match("BOR")) || !(control.name.toUpperCase().match("BOR RESPONSE"))) {
                                                            this.disabled = true;
                                                        }
                                                    }
                                                }
                                            }

                                            else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                                if (control.type == "text" || control.type == "TextArea") {
                                                    if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                                        this.disabled = true;
                                                    }
                                                }
                                                else if (control.type == "select-one") {
                                                    if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                                        this.disabled = true;
                                                    }
                                                }
                                            }
                                        })
                                    }
                                }
                                else {
                                    //AlertBox("WARNING", " Dicom Viewer", "No Lesion Statistics Found for Previous Visit!")
                                    $('.dynamic-control' + strClassName).each(function (index, control) {
                                        if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                            if (control.type == "text" || control.type == "TextArea") {
                                                if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                                    this.disabled = true;
                                                }
                                            }
                                            else if (control.type == "select-one") {
                                                if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                                    if (!(control.name.toUpperCase().match("BOR")) && !(control.name.toUpperCase().match("BOR RESPONSE"))) {
                                                        this.disabled = true;
                                                    }
                                                }
                                            }
                                        }

                                        else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                            if (control.type == "text" || control.type == "TextArea") {
                                                if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                                    this.disabled = true;
                                                }
                                            }
                                            else if (control.type == "select-one") {
                                                if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                                    this.disabled = true;
                                                }
                                            }
                                        }
                                    })
                                }
                            }
                            else {
                                //AlertBox("WARNING", " Dicom Viewer", "No Lesion Statistics Found for Previous Visit!")
                                $('.dynamic-control' + strClassName).each(function (index, control) {
                                    if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                        if (control.type == "text" || control.type == "TextArea") {
                                            if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                                this.disabled = true;
                                            }
                                        }
                                        else if (control.type == "select-one") {
                                            if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                                if (!(control.name.toUpperCase().match("BOR")) && !(control.name.toUpperCase().match("BOR RESPONSE"))) {
                                                    this.disabled = true;
                                                }
                                            }
                                        }
                                    }

                                    else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                        if (control.type == "text" || control.type == "TextArea") {
                                            if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                                this.disabled = true;
                                            }
                                        }
                                        else if (control.type == "select-one") {
                                            if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                                this.disabled = true;
                                            }
                                        }
                                    }
                                })
                            }
                        },
                        error: function (e) {
                        }
                    });
                }
            }
            else {
                if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                    MI_NLDetails();
                }
            }
        }

        if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1") || $("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {

            if (!(isVisit == 'N' || isVisit == '')) {
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
            }
        }
        if ($("#hdnUserTypeName").val().toUpperCase().match("ADJUDICATOR")) {
            if (!$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
                $('.dynamic-control' + strClassName).each(function (index, control) {
                    if (control.length != 0) {
                        this.disabled = true;
                    }
                })
            }
        }
    }
}

function errorMILesionDetail() {
    AlertBox("ERROR", " Dicom Viewer", "Error While Retriving Lesion Details For MI!")
}


//For Biznet
function SaveLession() {
    var finalData = [];
    var validation = true;

    var parms = query.split('&');
    var vActivityId, iNodeId, vWorkspaceId, vSubjectId, iModifyBy, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId;
    iModifyBy = $("#hdnuserid").val();
    vWorkspaceId = $("#hdnWorkspaceId").val();
    vSubjectId = $("#hdnSubjectId").val();
    vActivityId = $("#hdnActivityID").val();
    iNodeId = $("#hdnNodeID").val();
    iMySubjectNo = $("#hdnMySubjectNo").val();
    ScreenNo = $("#hdnScreenNo").val();
    ParentWorkSpaceId = $("#hdnParentWorkSpaceId").val();
    PeriodId = $("#hdnPeriodId").val();

    if ($("#hdnsubjectRejectionDtl").val() != 'Y') {
        $('.dynamic-control').each(function (index, control) {
            if (control.type == "text") {
                if (control.value == "" || control.value == '' || control.value == null) {
                    AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                    validation = false;
                    return false;
                }
            }

            else if (control.type.toUpperCase() == "TEXTAREA") {
                if (control.value == "" || control.value == '' || control.value == null) {
                    AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                    validation = false;
                    return false;
                }
            }

            else if (control.type == "select-one") {
                //if (control.selectedIndex == 0) {
                if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == '') {
                    if (control.selectedOptions[0].text == " " || control.selectedOptions[0].text == ' ') {
                        AlertBox("WARNING", " Dicom Viewer", "Please Select " + control.name)
                        validation = false;
                        return false;
                    }
                }
            }
        })
    }

    if (validation == true) {

        $('.dynamic-control').get().forEach(function (control, index, array) {
            try {
                var LesionData = {
                }
                if (control.type == "text") {
                    LesionData = {
                        vMedExResult: control.value,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.placeholder,
                    }
                }
                else if (control.type == "select-one") {
                    LesionData = {
                        vMedExResult: control.selectedOptions[0].text,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.name,
                    }
                }
                else if (control.type == "radio") {
                    LesionData = {
                        vMedExResult: $('input[name="' + control.name + '"]:checked').val(),
                        vMedExCode: control.name.split('_')[1],
                        vMedExDesc: control.name.split('_')[0],
                    }
                }
                finalData.push(LesionData);

            }
            catch (e) {
                throw e;
            }
        });


        var LesionFianlData = {
            ParentWorkSpaceId: ParentWorkSpaceId,
            vWorkspaceId: vWorkspaceId,
            vSubjectId: vSubjectId,
            vActivityId: vActivityId,
            iNodeId: iNodeId,
            iMySubjectNo: iMySubjectNo,
            ScreenNo: ScreenNo,
            PeriodId: PeriodId,
            iModifyBy: iModifyBy,
            DATAMODE: 1,
            _LesionDataDTO: finalData
        }
        $.ajax({
            url: ApiURL + "SetData/saveLessionDetails",
            type: "POST",
            //data: JSON.stringify(LesionFianlData),
            data: LesionFianlData,
            async: true,
            //dataType: 'json',
            //contentType: 'application/json',
            crossDomain: true,
            cache: false,
            success: successSaveLessionDetails,
            error: errorSaveLessionDetails
        });

        function successSaveLessionDetails() {
            localStorage.setItem("IsReviewContinue", "true");
            AlertBox("SUCCESS", " Dicom Viewer", "Lesion Data Saved Successfully!")

            $('.dynamic-control').get().forEach(function (control, index, array) {
                try {
                    if (control.type == "text") {
                        control.value = ""
                    }
                    else if (control.type.toUpperCase() == "TEXTAREA") {
                        control.value = ""
                    }
                    else if (control.type == "select-one") {
                        //control.selectedOptions[0].text = ""         
                        control.selectedIndex = 0;
                    }
                }
                catch (e) {
                    throw e;
                }
            });
        }

        function errorSaveLessionDetails() {
            AlertBox("error", " Dicom Viewer", "Error")
        }
    }
}

//From BizNET
function SubmitLesion() {
    var modaldata = document.getElementById('LesionModel')
    while (modaldata.hasChildNodes()) {
        modaldata.removeChild(modaldata.lastChild);
    }

    var divdata = document.getElementById('divLesion')
    while (divdata.hasChildNodes()) {
        divdata.removeChild(divdata.lastChild);
    }

    data = {
        vParentWorkSpaceId: $("#hdnParentWorkSpaceId").val(),
        vWorkspaceId: $("#hdnWorkspaceId").val(),
        vSubjectId: $("#hdnSubjectId").val(),
        iMySubjectNo: $("#hdnMySubjectNo").val(),
        //ScreenNo: $("#hdnScreenNo").val(),
        vPeriodId: $("#hdnPeriodId").val(),
        vActivityId: $("#hdnActivityID").val(),
        iNodeId: $("#hdnNodeID").val()
    };
    $.ajax({
        url: WebURL + "MIDicomViewer/ClearSession",
        type: 'POST',
        data: '',
        //contentType: 'application/json; charset=utf-8',
        //dataType: 'json',
        async: false,
        success: function (msg) {
        },
        error: function (msg) {
            AlertBox("error", " Dicom Viewer", "Error")
        }
    });

    var filename = 'dicom'
    cornerstoneTools.saveAs(element, filename);

    if (BizNetDicomSave == true) {
        $.ajax({
            url: ApiURL + "SetData/Save_CRFHdrDtlSubDtl",
            type: "POST",
            data: data,
            async: false,
            success: function (data) {
                //toolsave();
                ///////////////window.close()
                //alert("Success")
            },
            error: function () {
            }
        });
    }

    BizNetDicomImage = BizNetDicomPath.split('_')
    WorkspaceId = BizNetDicomImage[0];
    SubjectId = BizNetDicomImage[1];
    NodeId = BizNetDicomImage[2];
    ModalityNo = BizNetDicomImage[3];

    $.ajax({
        url: WebURL + "MIDicomViewer/BizNetImage?WorkspaceId=" + WorkspaceId + "&SubjectId=" + SubjectId + "&NodeId=" + NodeId + "&ModalityNo=" + ModalityNo,
        type: "POST",
        async: false,
        success: function (data) {
        },
        error: function () {
        }
    });

    function BizNetDeleteImage() {
        BizNetDicomImage = BizNetDicomPath
        $.ajax({
            url: WebURL + "MIDicomViewer/BizNetImage?BizNetDicomImage=" + BizNetDicomImage,
            type: "POST",
            async: false,
            success: function (data) {
            },
            error: function () {
            }
        });
    }
}

//Not Used
//For MI
function SaveMILession() {

    var finalData = [];
    var validation = true;

    var parms = query.split('&');
    var vActivityId, iNodeId, vWorkspaceId, vSubjectId, iModifyBy, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId;
    iModifyBy = $("#hdnuserid").val();
    vWorkspaceId = $("#hdnvWorkspaceId").val();
    vSubjectId = $("#hdnvSubjectId").val();
    vActivityId = $("#hdnvActivityId").val();
    iNodeId = $("#hdniNodeId").val();
    iMySubjectNo = $("#hdniMySubjectNo").val();
    ScreenNo = $("#hdnvMySubjectNo").val();
    ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
    PeriodId = $("#hdniPeriod").val();

    $('.dynamic-control').each(function (index, control) {
        if (control.type == "text") {
            if (control.value == "" || control.value == '' || control.value == null) {
                AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                validation = false;
                return false;
            }
        }

        else if (control.type.toUpperCase() == "TEXTAREA") {
            if (control.value == "" || control.value == '' || control.value == null) {
                AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                validation = false;
                return false;
            }
        }

        else if (control.type == "select-one") {
            //if (control.selectedIndex == 0) {
            if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == '') {
                if (control.selectedOptions[0].text == " " || control.selectedOptions[0].text == ' ') {
                    AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                    validation = false;
                    return false;
                }
            }
        }
    })

    if (validation == true) {

        $('.dynamic-control').get().forEach(function (control, index, array) {
            try {
                var LesionData = {
                }
                if (control.type == "text") {
                    LesionData = {
                        vMedExResult: control.value,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.placeholder,

                    }
                }
                else if (control.type == "select-one") {
                    LesionData = {
                        vMedExResult: control.selectedOptions[0].text,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.name,
                    }
                }
                else if (control.type == "radio") {
                    LesionData = {
                        vMedExResult: $('input[name="' + control.name + '"]:checked').val(),
                        vMedExCode: control.name.split('_')[1],
                        vMedExDesc: control.name.split('_')[0],
                    }
                }
                finalData.push(LesionData);

            }
            catch (e) {
                throw e;
            }
        });


        var LesionFianlData = {
            ParentWorkSpaceId: ParentWorkSpaceId,
            vWorkspaceId: vWorkspaceId,
            vSubjectId: vSubjectId,
            vActivityId: vActivityId,
            iNodeId: iNodeId,
            iMySubjectNo: iMySubjectNo,
            ScreenNo: ScreenNo,
            PeriodId: PeriodId,
            iModifyBy: iModifyBy,
            DATAMODE: 1,
            _LesionDataDTO: finalData
        }

        $.ajax({
            url: ApiURL + "SetData/saveLessionDetails",
            type: "POST",
            //data: JSON.stringify(LesionFianlData),
            data: LesionFianlData,
            async: false,
            //dataType: 'json',
            //contentType: 'application/json',
            crossDomain: true,
            cache: false,
            success: successSaveMILessionDetails,
            error: errorSaveMILessionDetails
        });

        function successSaveMILessionDetails() {
            localStorage.setItem("IsReviewContinue", "true");
            AlertBox("SUCCESS", " Dicom Viewer", "Lesion Data Saved Successfully!")

            $('.dynamic-control').get().forEach(function (control, index, array) {
                try {

                    if (control.type == "text") {
                        control.value = ""
                    }
                    else if (control.type == "select-one") {
                        //control.selectedOptions[0].text = ""         
                        control.selectedIndex = 0;

                    }
                }
                catch (e) {
                    throw e;
                }
            });
        }

        function errorSaveMILessionDetails() {
            AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
        }
    }
}

//Not Used
function SubmitMILesion() {
    var modaldata = document.getElementById('LesionModel')
    while (modaldata.hasChildNodes()) {
        modaldata.removeChild(modaldata.lastChild);
    }

    var divdata = document.getElementById('divLesion')
    while (divdata.hasChildNodes()) {
        divdata.removeChild(divdata.lastChild);
    }

    data = {
        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
        vWorkspaceId: $("#hdnvWorkspaceId").val(),
        vSubjectId: $("#hdnvSubjectId").val(),
        iMySubjectNo: $("#hdniMySubjectNo").val(),
        //ScreenNo: $("#hdnvMySubjectNo").val(),
        vPeriodId: $("#hdniPeriod").val(),
        vActivityId: $("#hdnvActivityId").val(),
        iNodeId: $("#hdniNodeId").val()
    };

    $.ajax({
        url: WebURL + "MIDicomViewer/ClearSession",
        type: 'POST',
        data: '',
        //contentType: 'application/json; charset=utf-8',
        //dataType: 'json',
        async: false,
        success: function (msg) {
        },
        error: function (msg) {
            AlertBox("error", " Dicom Viewer", "Error")
        }
    });

    var filename = 'dicom'
    cornerstoneTools.saveAs(element, filename);

    if (MIDicomSave == true) {
        $.ajax({
            url: WebURL + "MIDicomViewer/SaveDicom",
            type: 'POST',
            data: '',
            //contentType: 'application/json; charset=utf-8',
            //dataType: 'json',
            async: false,
            success: function (msg) {
                if (msg == "Succes") {
                    $.ajax({
                        url: ApiURL + "SetData/Save_CRFHdrDtlSubDtl",
                        type: "POST",
                        data: data,
                        async: false,
                        success: function (data) {
                            AlertBox("success", " Dicom Viewer", "Image Saved Successfully!")
                            window.close()

                        },
                        error: function () {
                        }
                    });
                }
            },
            error: function () {
            }
        });
    }
    else {
        $.ajax({
            url: WebURL + "MIDeleteImage",
            type: "POST",
            async: false,
            success: function (data) {
            },
            error: function () {
            }
        });

    }

}
//------Not Used---------------------------------

//For Calculation of Statastics
function bindevent() {
    //debugger;     //uncomment
    $('.dynamic-size-checkbox-control').change(function () {
        //IF CheckBox Checked
        if (this.checked) {

            //This Condition is to Mark the Non Target Lesion Tag in Image using DropDown CheckBox Combination
            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL")) {
                if ($("#" + $(this).attr("refid")).get(0).selectedOptions[0].text == "" || $("#" + $(this).attr("refid")).get(0).selectedOptions[0].text == " ") {
                    AlertBox("WARNING", " Dicom Viewer", "Please Select Non Target Lesion First!")
                    this.checked = false;
                }
                else {
                    NTLMark = $("#" + $(this).attr("refid")).get(0).selectedOptions[0].text;
                }

            }
                //This Condition is to Get Line Size detail to TextBox CheckBox Combination in Target Lesion
            else {
                if (lineLength == "" || lineLength == '' || lineLength == null || lineLength == 'undefined' || lineLength == undefined) {
                    AlertBox("WARNING", " Dicom Viewer", "Please Make Measurement First!")
                    this.checked = false;
                    return false;
                }

                //Get Value By Its Parent
                //$(this).parent().parent().find("input[type='text']").val(lineLength)

                //Hiren Rami
                var lineTag;
                if (lineLength.indexOf("W:") >= 0) {
                    lineTag = lineLength.toUpperCase().replace(/ /g, "_").split("_")[3]
                }
                else {
                    lineTag = lineLength.toUpperCase().replace(/ /g, "_").split("_")[2]
                }
                //------------------------

                var length = lineLength;
                if (length != undefined) {
                    if (length.indexOf("mm") >= 0) {
                        length = length.replace("mm", "").replace("W:", "");
                    }
                    length = parseFloat(length);
                }

                if ($("#hdnvActivityName").val().toUpperCase().match("BL") || $("#hdnvSubActivityName").val().toUpperCase().match("BASELINE")) {
                    if ($("#hdnvSubActivityName").val().toUpperCase().match("TL") || $("#hdnvSubActivityName").val().toUpperCase().match("-TL")) {
                        if (length < 10) {
                            AlertBox("WARNING", " Dicom Viewer", "Length should be Minimum <b>10 mm</b> in <b>BASELINE</b> for <b>TARGET LESION</b>!")
                            this.checked = false;
                            return false;
                        }
                    }
                }

                if (this.className.replace(/ /g, "#").split("#")[1].length == 5) {
                    if (this.className.replace(/ /g, "#").split("#")[1].substring(this.className.replace(/ /g, "#").split("#")[1].length - 1, this.className.replace(/ /g, "#").split("#")[1].length) == (lineTag.substring(2))) {
                        $("#" + $(this).attr("refid")).val(length);
                    }
                    else {
                        AlertBox("WARNING", " Dicom Viewer", "Please Select Matching Size as per Your Lesion Marking!")
                        this.checked = false;
                        return false;
                    }
                }
                else if (this.className.replace(/ /g, "#").split("#")[1].length == 6) {
                    if (this.className.replace(/ /g, "#").split("#")[1].substring(this.className.replace(/ /g, "#").split("#")[1].length - 2, this.className.replace(/ /g, "#").split("#")[1].length) == (lineTag.substring(2))) {
                        $("#" + $(this).attr("refid")).val(length);
                    }
                    else {
                        AlertBox("WARNING", " Dicom Viewer", "Please Select Matching Size as per Your Lesion Marking!")
                        this.checked = false;
                        return false;
                    }
                }
                else {
                    if (this.className.replace(/ /g, "#").split("#")[1].match(lineTag.substring(2))) {
                        $("#" + $(this).attr("refid")).val(length);
                    }
                    else {
                        AlertBox("WARNING", " Dicom Viewer", "Please Select Matching Size as per Your Lesion Marking!")
                        this.checked = false;
                        return false;
                    }
                }

                //$("#" + $(this).attr("refid")).val(length);
                //lineLength = "";
            }

        }
            //IF CheckBox UnChecked
        else {
            //This Condition is to Mark the Non Target Lesion Tag in Image using DropDown CheckBox Combination
            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL")) {
                NTLMark = "";
            }
                //This Condition is to Get Line Size detail to TextBox CheckBox Combination in Target Lesion
            else {
                $("#" + $(this).attr("refid")).val("");
            }
            //lineLength = "";
        }

        //This Condition is to Mark the Non Target Lesion Tag in Image using DropDown CheckBox Combination
        if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL")) {

        }
            //This Condition is to Get Line Size detail to TextBox CheckBox Combination in Target Lesion
        else {
            var sum = 0;
            $('.dynamic-size-checkbox-control').each(function (index, control) {
                if (this.checked) {
                    var txtvalue = parseFloat($("#" + $(this).attr("refid")).val());
                    if (!isNaN(txtvalue)) {
                        sum += txtvalue;
                    }
                }
            })
            if (MIClearLesionDataFlag == false) {
                $('.dynamic-saved-size-textbox-control').each(function (index, control) {
                    var txtvalue = parseFloat(control.value);
                    if (!isNaN(txtvalue)) {
                        sum += txtvalue;
                    }
                })
            }

            $(".dynamic-diameter-sum-control").val(sum.toFixed(2));

            if ((!($("#hdnvActivityName").val().toUpperCase().match("BL")) || (!$("#hdnvActivityName").val().toUpperCase().match("BASELINE"))) || (!$("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
                $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-BL"
                var CurrentVisit2BL = document.getElementsByClassName("" + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-BL");
                var NADIR = document.getElementsByClassName('NADIR');
                if (CurrentVisit2BL.length != 0) {
                    var DIAMETERS_CurrentVisit = document.getElementsByClassName('DIAMETERS-' + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase())[0].value;
                    DIAMETERS_CurrentVisit = parseFloat(DIAMETERS_CurrentVisit)
                    if ((!isNaN(DIAMETERS_CurrentVisit))) {
                        if ((!(DIAMETERS_CurrentVisit == "0.00"))) {
                            var DIAMETERS_BL = document.getElementsByClassName('DIAMETERS-' + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + '-BL')[0].value;
                            if (DIAMETERS_BL == "" || isNaN(DIAMETERS_BL)) {
                                AlertBox("WARNING", " Dicom Viewer", "No Baseline (BL) Data Found!")
                                this.checked = false;
                                $("#" + $(this).attr("refid")).val("");
                                $(".dynamic-diameter-sum-control").val("");
                                return false;
                            }
                            DIAMETERS_BL = parseFloat(DIAMETERS_BL)
                            var RESULT = ((DIAMETERS_CurrentVisit - DIAMETERS_BL) / (DIAMETERS_BL)) * 100
                            CurrentVisit2BL[0].value = RESULT.toFixed(2);
                            if (!($("#hdnvActivityName").val().toUpperCase().match("TP2"))) {
                                var Visit = $("#hdnSelectedvParentNodeDisplayName").val();
                                var VisitCount = parseInt(Visit.substr(2, 2));
                                var PreviousVisitDiametersDetails = []
                                for (var v = VisitCount - 1 ; v >= 2 ; v--) {
                                    var blankValueChecker = document.getElementsByClassName('DIAMETERS-' + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + '-TP' + v)[0].value
                                    if ((!blankValueChecker == "") || (!blankValueChecker == null) || isNaN(blankValueChecker)) {
                                        PreviousVisitDiametersDetails.push(parseFloat(document.getElementsByClassName('DIAMETERS-' + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + '-TP' + v)[0].value));
                                    }
                                }
                                var minPreviousVisitDiametersDetails = Math.min.apply(Math, PreviousVisitDiametersDetails);
                                var minVal = Math.min(DIAMETERS_BL, minPreviousVisitDiametersDetails)
                                var NADIR_RESULT = ((DIAMETERS_CurrentVisit - minVal) / (minVal)) * 100
                                NADIR[0].value = NADIR_RESULT.toFixed(2);
                            }
                            else {
                                NADIR[0].value = RESULT.toFixed(2);
                            }
                        }
                        else {
                            CurrentVisit2BL[0].value = "";
                            NADIR[0].value = "";
                        }
                    }
                    else {
                        CurrentVisit2BL[0].value = "";
                        NADIR[0].value = "";
                    }
                }
            }
        }
        //}).change();
    });
}

//Changed for MI // To Save Entry in Temp Table
function SaveMIFinalLession() {   
    //$.confirm({
    //    title: 'Confirm!',
    //    icon: 'fa fa-warning',
    //    content: 'USER CONFIRMATION',
    //    onContentReady: function () {
    //        var self = this;
    //        this.setContentPrepend('<div>MI</div>');
    //        setTimeout(function () {
    //            self.setContentAppend('<div>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>Details?</div>');
    //        }, 1000);
    //    },
    //    columnClass: 'medium',
    //    //animation: 'zoom',
    //    closeIcon: true,
    //    closeIconClass: 'fa fa-close',
    //    //columnClass: 'small',
    //    boxWidth: '30%',
    //    autoClose: 'danger|15000',
    //    animation: 'news',
    //    closeAnimation: 'news',
    //    closeAnimation: 'scale',
    //    backgroundDismissAnimation: 'random',
    //    //backgroundDismissAnimation: 'glow',
    //    type: 'blue',
    //    theme: 'dark',
    //    draggable: true,
    //    buttons: {
    //        info: {
    //            btnClass: 'btn-blue',
    //            text: 'OK (O)',
    //            keys: ['O'],
    //            action: function () {
    debugger;
                    var strCurrentSubActivityName = "";
                    if ($("#hdnCurrentSubActivityName").val() != "") {
                        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
                    }

                    var validation = true;
                    var dublicateChecker = []

                    $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                        if (control.type == "text") {
                            //if (control.placeholder.toUpperCase().match("REMARK") || control.placeholder.toUpperCase().match("REMARKS")) {
                            if (control.placeholder.toUpperCase() == "REMARK" || control.placeholder.toUpperCase() == "REMARKS") { //Required to Match Exact Match
                                if (control.value == "" || control.value == '' || control.value == null) {
                                    AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                                    validation = false;
                                    return false;
                                }
                            }
                        }

                        else if (control.type.toUpperCase() == "TEXTAREA") {
                            //if (control.placeholder.toUpperCase().match("REMARK") || control.placeholder.toUpperCase().match("REMARKS")) {
                            if (control.placeholder.toUpperCase() == "REMARK" || control.placeholder.toUpperCase() == "REMARKS") { //Required to Match Exact Match
                                if (control.value == "" || control.value == '' || control.value == null) {
                                    AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                                    validation = false;
                                    return false;
                                }
                            }
                        }

                    })

                    $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                        if (control.type == "select-one") {

                            if ($("#hdnvSkipVisit").val() != "Y") {

                                if ((($("#hdnvSubActivityName").val().toUpperCase().match("TL"))) || (($("#hdnvSubActivityName").val().toUpperCase().match("-TL")))) {
                                    if (control.name.toUpperCase().match(($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-TL-ASSESSMENT"))) {
                                        if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == " ") {
                                            AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                                            validation = false;
                                            return false;
                                        }
                                    }
                                }

                                if ((($("#hdnvSubActivityName").val().toUpperCase().match("NTL"))) || (($("#hdnvSubActivityName").val().toUpperCase().match("-NTL")))) {
                                    if (control.name.toUpperCase().match(($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NTL-ASSESSMENT"))) {
                                        if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == " ") {
                                            AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                                            validation = false;
                                            return false;
                                        }
                                    }
                                }

                                if ((($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                                    if (control.name.toUpperCase().match(($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NL-ASSESSMENT"))) {
                                        if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == " ") {
                                            AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                                            validation = false;
                                            return false;
                                        }
                                    }
                                    else if (control.name.toUpperCase().match(($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE"))) {
                                        if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == " ") {
                                            AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                                            validation = false;
                                            return false;
                                        }
                                    }
                                }
                            }


                            if (control.selectedOptions[0].text != "" || control.selectedOptions[0].text != '') {
                                if (control.selectedOptions[0].text != " " || control.selectedOptions[0].text != ' ') {
                                    if (control.name.toUpperCase().match("ORGAN")) {
                                        dublicateChecker.push(control.selectedOptions[0].text)
                                    }
                                }
                            }
                        }
                    })

                    if (validation == true) {
                        for (var v = 0 ; v < dublicateChecker.length  ; v++) {
                            var itemcount = 0;
                            for (var j = 0 ; j < dublicateChecker.length  ; j++) {
                                if (dublicateChecker[v] == dublicateChecker[j]) {
                                    itemcount++;
                                }
                            }
                            if (itemcount > 2) {
                                AlertBox("WARNING", " Dicom Viewer", "Organ Details Found More Than Twice!")
                                return false;
                            }
                        }
                        //for (var v = 1 ; v < dublicateChecker.length; v++) {
                        //    //dublicateChecker.sort()
                        //    if (dublicateChecker[v - 1] == dublicateChecker[v]) {
                        //        for (var j = v ; j < dublicateChecker.length; j++ , v++) {
                        //            if (dublicateChecker[v - 1] == dublicateChecker[v]) {
                        //                alert("Same Organ Can Not Be selected More Than Two Times!");
                        //                return false;
                        //            }
                        //        }           
                        //    }
                        //}
                    }
                    if (validation == true) {
                        var chkBlankMarking = false;
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.className.toUpperCase().match("ENABLECONTROL")) {
                                if (control.value == "" || control.value == '' || control.value == null) {
                                    chkBlankMarking = true;
                                }
                            }
                        });
                        if (chkBlankMarking == false) {
                            $('.dynamic-size-checkbox-control').each(function (index, control) {
                                if (control.className.toUpperCase().match("ENABLECONTROL")) {
                                    if (control.disabled == false) {
                                        if (control.checked == false) {
                                            chkBlankMarking = true;
                                        }
                                    }
                                }
                            });
                        }
                        if (chkBlankMarking) {
                            $.confirm({
                                title: 'Confirm!',
                                icon: 'fa fa-warning',
                                content: 'USER CONFIRMATION',
                                onContentReady: function () {
                                    var self = this;
                                    this.setContentPrepend('<div>MI</div>');
                                    setTimeout(function () {
                                        self.setContentAppend('<div>Please Enter All Marking Value</div>');
                                    }, 1000);
                                },
                                columnClass: 'medium',
                                //animation: 'zoom',
                                closeIcon: true,
                                closeIconClass: 'fa fa-close',
                                //columnClass: 'small',
                                boxWidth: '30%',
                                autoClose: 'danger|15000',
                                animation: 'news',
                                closeAnimation: 'news',
                                closeAnimation: 'scale',
                                backgroundDismissAnimation: 'random',
                                //backgroundDismissAnimation: 'glow',
                                type: 'blue',
                                theme: 'dark',
                                draggable: true,
                                buttons: {
                                    info: {
                                        btnClass: 'btn-blue',
                                        text: 'OK (O)',
                                        keys: ['O'],
                                        action: function () {
                                            //SaveMIFinalLessionData();
                                            //validateStatisticsCriteria();
                                            if (savedJsonData.length != 0) {
                                                $("#txtRemarks").val("");
                                                $('#ModalMIRemarkPopUp').modal('show');
                                            }
                                            else {
                                                validateStatisticsCriteria();
                                            }
                                        }
                                    },
                                    danger: {
                                        btnClass: 'btn-red any-other-class',
                                        text: 'CANCEL (C)',
                                        keys: ['C'],
                                        action: function () {
                                            //AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                            //$.alert('Canceled!');
                                        }
                                    },
                                }
                            });
                        }
                        else {
                            //validateStatisticsCriteria();
                            if (savedJsonData.length != 0) {
                                $("#txtRemarks").val("");
                                $('#ModalMIRemarkPopUp').modal('show');
                            }
                            else {
                                validateStatisticsCriteria();
                            }
                        }

                        //validateStatisticsCriteria();
                    }

        //        }
        //    },
        //    danger: {
        //        btnClass: 'btn-red any-other-class',
        //        text: 'CANCEL (C)',
        //        keys: ['C'],
        //        action: function () {
        //            AlertBox("WARNING", " Dicom Viewer", "Please Save Data Again For <b>" + $("#hdnSelectedvParentNodeDisplayName").val() + " For " + $("#hdnSelectedvChildNodeDisplayName").val() + "</b> And Try Again Later!")
        //            //$.alert('Canceled!');
        //        }
        //    },
        //}
    //});
}

$("#btnMIRemark").on("click", function () {
    if ($("#txtRemarks").val() == "" || $("#txtRemarks").val() == null) {
        AlertBox("WARNING", " Dicom Viewer", "Please Enter Remarks!")
        return false;
    }
    else {
        remark = $("#txtRemarks").val();
        validateStatisticsCriteria();
        //removeDiv();
        //$(".spinner").hide();
    }
});

//Validation For Statistics Lesion Detail For NL
function validateStatisticsCriteria() {
    if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
        var TL = "";
        var NTL = "";
        var NL = "";
        var OVERALL = ""
        //Top up

        var strCurrentSubActivityName = "";
        if ($("#hdnCurrentSubActivityName").val() != "") {
            strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
        }

        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
            if (control.className.toUpperCase().match("ENABLECONTROL")) {
                if (control.value == "" || control.value == '' || control.value == null) {
                    AlertBox("WARNING", " Dicom Viewer", "Please Enter All Marking Detail")
                }


            }

            if (control.type == "text" || control.type == "TextArea") {
                if (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-TL-ASSESSMENT")) {
                    TL = control.value.toUpperCase()
                }
                else if (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NTL-ASSESSMENT")) {
                    NTL = control.value.toUpperCase()
                }
                else if (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NL-ASSESSMENT")) {
                    NL = control.value.toUpperCase()
                }
            }
            else if (control.type == "select-one") {
                if (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-TL-ASSESSMENT") == control.name.toUpperCase()) {
                    TL = control.selectedOptions[0].text.toUpperCase()
                }
                else if (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NTL-ASSESSMENT") == control.name.toUpperCase()) {
                    NTL = control.selectedOptions[0].text.toUpperCase()
                }
                else if (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-NL-ASSESSMENT") == control.name.toUpperCase()) {
                    NL = control.selectedOptions[0].text.toUpperCase()
                }
            }
        })

        if (!TL == "") {
            if (!NTL == "") {
                if (!NL == "") {

                    if (TL == "CR" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "CR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>CR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                            //$.alert('Canceled!');
                                                        }
                                                    },
                                                }
                                            });
                                            //var result = confirm('The Value For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + ' Should be CR. Do You Want To Continue ?');
                                            //if (result) {
                                            //    validation = true;
                                            //}
                                            //else {
                                            //    validation = false;
                                            //    AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                            //    return false;
                                            //}
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "CR") {

                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>CR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                            //$.alert('Canceled!');
                                                        }
                                                    },
                                                }
                                            });
                                            //var result = confirm('The Value For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + ' Should be CR. Do You Want To Continue ?');
                                            //if (result) {
                                            //    validation = true;
                                            //}
                                            //else {
                                            //    validation = false;
                                            //    AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                            //    return false;
                                            //}
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {

                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First .!")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later.!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First .!")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later.!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First .!")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b><font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later.!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                            //$.alert('Canceled!');
                                                        }
                                                    },
                                                }
                                            });
                                            //var result = confirm('The Value For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + ' Should be CR. Do You Want To Continue ?');
                                            //if (result) {
                                            //    validation = true;
                                            //}
                                            //else {
                                            //    validation = false;
                                            //    AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                            //    return false;
                                            //}
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {

                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                            //$.alert('Canceled!');
                                                        }
                                                    },
                                                }
                                            });
                                            //var result = confirm('The Value For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + ' Should be CR. Do You Want To Continue ?');
                                            //if (result) {
                                            //    validation = true;
                                            //}
                                            //else {
                                            //    validation = false;
                                            //    AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                            //    return false;
                                            //}
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {

                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "PR" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NE" || NTL == "NOT EVALUATED" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PR") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PR</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "SD" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be<font color="red"> <b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NE" || NTL == "NOT EVALUATED" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "SD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>SD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {]
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font>> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "NO")) {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "YES")) {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></<font color="red">> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "YES" || NL == "NO")) {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },

                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        //if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                        if (OVERALL != "NE") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>NE/NOT EVALUATED</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && (NL == "YES" || NL == "NO")) {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if ((($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE") == control.name.toUpperCase()) || (($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE") == control.name.toUpperCase())) {
                                    OVERALL = control.selectedOptions[0].text.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;
                                    }
                                    else {
                                        if (OVERALL != "PD") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <font color="red"><b>PD</b></font> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
                                                    }, 1000);
                                                },
                                                columnClass: 'medium',
                                                //animation: 'zoom',
                                                closeIcon: true,
                                                closeIconClass: 'fa fa-close',
                                                //columnClass: 'small',
                                                boxWidth: '30%',
                                                autoClose: 'danger|15000',
                                                animation: 'news',
                                                closeAnimation: 'news',
                                                closeAnimation: 'scale',
                                                backgroundDismissAnimation: 'random',
                                                //backgroundDismissAnimation: 'glow',
                                                type: 'blue',
                                                theme: 'dark',
                                                draggable: true,
                                                buttons: {
                                                    info: {
                                                        btnClass: 'btn-blue',
                                                        text: 'OK (O)',
                                                        keys: ['O'],
                                                        action: function () {
                                                            SaveMIFinalLessionData();
                                                        }
                                                    },
                                                    danger: {
                                                        btnClass: 'btn-red any-other-class',
                                                        text: 'CANCEL (C)',
                                                        keys: ['C'],
                                                        action: function () {
                                                            AlertBox("WARNING", " Dicom Viewer", "Please Change Value For <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>And Try Again Later!")
                                                        }
                                                    },
                                                }
                                            });
                                        }
                                        else {
                                            SaveMIFinalLessionData();
                                        }
                                    }
                                }
                            }
                        })
                    }
                        //
                    else {
                        SaveMIFinalLessionData();
                    }
                }
                else {
                    SaveMIFinalLessionData();
                }
            }
            else {
                SaveMIFinalLessionData();
            }
        }
        else {
            SaveMIFinalLessionData();
        }
    }       //down
    else {
        SaveMIFinalLessionData();
    }
}

//Save MI Final Lesion Detail Function
function SaveMIFinalLessionData() {

    var finalData = [];
    var parms = query.split('&');
    var vActivityId, iNodeId, vWorkspaceId, vSubjectId, iModifyBy, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId;
    iModifyBy = $("#hdnuserid").val();
    vWorkspaceId = $("#hdnvWorkspaceId").val();
    vSubjectId = $("#hdnvSubjectId").val();
    //vActivityId = $("#hdnvActivityId").val();    
    //iNodeId = $("#hdniNodeId").val();
    vActivityId = $("#hdnvSubActivityId").val();
    iNodeId = $("#hdniSubNodeId").val();
    iMySubjectNo = $("#hdniMySubjectNo").val();
    ScreenNo = $("#hdnvMySubjectNo").val();
    ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
    PeriodId = $("#hdniPeriod").val();
    vActivityName = $("#hdnSelectedvParentNodeDisplayName").val();
    vSubActivityName = $("#hdnSelectedvChildNodeDisplayName").val();
    vParentActivityId = $("#hdnvActivityId").val();
    iParentNodeId = $("#hdniNodeId").val();

    var strCurrentSubActivityName = "";
    if ($("#hdnCurrentSubActivityName").val() != "") {
        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
    }

    if (vWorkspaceId == "" || vWorkspaceId == '' || vWorkspaceId == null) {
        AlertBox("error", " Dicom Viewer", "Session Expired!")
        var url = $("#RedirectToLogin").val();
        location.href = url;
        return false;
    }

    //Logic With Audit Trail
    $('.dynamic-control' + strCurrentSubActivityName).get().forEach(function (control, index, array) {
        try {
            var LesionData = {
            }

            if (savedJsonData.length != 0) {
                for (var savedIndex = 0; savedIndex < savedJsonData.length; savedIndex++) {
                    if (savedJsonData[savedIndex].vMedExResult == null) {
                        savedJsonData[savedIndex].vMedExResult = "";
                    }
                    if (control.type == "text") {
                        if (control.id.split('_')[3] == savedJsonData[savedIndex].vMedExCode) {
                            LesionData = {
                                vMedExResult: control.value,
                                vMedExCode: control.id.split('_')[3],
                                vMedExDesc: control.placeholder,
                                vMedExType: "TextBox"
                            }
                            if (control.value != savedJsonData[savedIndex].vMedExResult) {
                                LesionData.vModificationRemark = remark.trim()
                                break;
                            }
                            else {    //if (control.value != "" || control.value != '' || control.value != null) {
                                LesionData.vModificationRemark = ""
                                break;
                            }
                        }

                        //finalData.push(LesionData);
                        //}
                    }

                    else if (control.type.toUpperCase() == "TEXTAREA") {
                        //if (control.value != "" || control.value != '' || control.value != null) {
                        if (control.id.split('_')[3] == savedJsonData[savedIndex].vMedExCode) {
                            LesionData = {
                                vMedExResult: control.value,
                                vMedExCode: control.id.split('_')[3],
                                vMedExDesc: control.placeholder,
                                vMedExType: "TextArea",
                            }

                            if (control.value != savedJsonData[savedIndex].vMedExResult) {
                                LesionData.vModificationRemark = remark.trim()
                                break;
                            }
                            else {    //if (control.value != "" || control.value != '' || control.value != null) {
                                LesionData.vModificationRemark = ""
                                break;
                            }
                        }

                    }

                    else if (control.type == "select-one") {
                        if (control.id.split('_')[3] == savedJsonData[savedIndex].vMedExCode) {
                            LesionData = {
                                vMedExResult: control.selectedOptions[0].text,
                                vMedExCode: control.id.split('_')[3],
                                vMedExDesc: control.name,
                                vMedExType: "ComboBox",
                            }

                            if (control.selectedOptions[0].innerText != savedJsonData[savedIndex].vMedExResult) {
                                LesionData.vModificationRemark = remark.trim()
                                break;
                            }
                            else {
                                LesionData.vModificationRemark = ""
                                break;
                            }
                        }

                    }
                    else if (control.type == "radio") {
                        if (control.name.split('_')[1] == savedJsonData[savedIndex].vMedExCode) {
                            LesionData = {
                                vMedExResult: $('input[name="' + control.name + '"]:checked').val(),
                                vMedExCode: control.name.split('_')[1],
                                vMedExDesc: control.name.split('_')[0],
                                vMedExType: "Radio",
                            }


                            if ($('input[name="' + control.name + '"]:checked').val() != savedJsonData[savedIndex].vMedExResult) {
                                LesionData.vModificationRemark = remark.trim()
                                break;
                            }
                            else {
                                LesionData.vModificationRemark = ""
                                break;
                            }
                        }

                        //finalData.push(LesionData);
                    }


                }
            }
            else {
                var LesionData = {
                }
                if (control.type == "text") {
                    //if (control.value != "" || control.value != '' || control.value != null) {
                    LesionData = {
                        vMedExResult: control.value,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.placeholder,
                        vMedExType: "TextBox"
                    }
                    //finalData.push(LesionData);
                    //}
                }

                else if (control.type.toUpperCase() == "TEXTAREA") {
                    //if (control.value != "" || control.value != '' || control.value != null) {
                    LesionData = {
                        vMedExResult: control.value,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.placeholder,
                        vMedExType: "TextArea",
                    }
                    //finalData.push(LesionData);
                    //}
                }

                else if (control.type == "select-one") {
                    //if (control.selectedOptions[0].text != "" || control.selectedOptions[0].text != '') {
                    //if (control.selectedOptions[0].text != " " || control.selectedOptions[0].text != ' ') {
                    LesionData = {
                        vMedExResult: control.selectedOptions[0].text,
                        vMedExCode: control.id.split('_')[3],
                        vMedExDesc: control.name,
                        vMedExType: "ComboBox",
                    }
                    //finalData.push(LesionData);
                    //}
                    //}
                }
                else if (control.type == "radio") {
                    LesionData = {
                        vMedExResult: $('input[name="' + control.name + '"]:checked').val(),
                        vMedExCode: control.name.split('_')[1],
                        vMedExDesc: control.name.split('_')[0],
                        vMedExType: "Radio",
                    }
                    //finalData.push(LesionData);
                }
                //finalData.push(LesionData);
            }

        }
        catch (e) {
            throw e;
        }
        finalData.push(LesionData);
    });

    if (finalData.length == 0 || finalData == null) {
        AlertBox("WARNING", " Dicom Viewer", "Please Select Mininum Value to Save!")
        return false;
    }

    var LesionFianlData = {
        ParentWorkSpaceId: ParentWorkSpaceId,
        vWorkspaceId: vWorkspaceId,
        vSubjectId: vSubjectId,
        vParentActivityId: vParentActivityId,
        iParentNodeId: iParentNodeId,
        vActivityId: vActivityId,
        iNodeId: iNodeId,
        iMySubjectNo: iMySubjectNo,
        ScreenNo: ScreenNo,
        PeriodId: PeriodId,
        vActivityName: vActivityName,
        vSubActivityName: vSubActivityName,
        iModifyBy: iModifyBy,
        DATAMODE: 1,
        MIFinalLesionDetailDataDTO: finalData
    }

    $.ajax({
        //url: ApiURL + "SetData/SaveMIFinalLession",
        url: WebURL + "MIDicomViewer/SaveMIFinalLession",
        type: "POST",
        data: JSON.stringify({ _MIFinalLesionDetailDTO: LesionFianlData }),
        //data: JSON.stringify(LesionFianlData),
        //data: LesionFianlData,
        //async: false,
        //dataType: 'json',
        contentType: 'application/json',
        crossDomain: true,
        cache: false,
        success: successSaveMILessionDetails,
        error: errorSaveMILessionDetails
    });

    function successSaveMILessionDetails(data) {
        $('#ModalMIRemarkPopUp').modal('hide');

        if (data == "1") {
            localStorage.setItem("IsReviewContinue", "true");
            //if (!$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
            //    SaveAndchangesFinalLesion();
            //}
            AlertBox("SUCCESS", " Dicom Viewer", "Lesion Data Saved Successfully!")
            $('.btn-hide-save' + strCurrentSubActivityName).hide();
        }
        else if (data == "SessionExpired") {
            AlertBox("WARNING", " Dicom Viewer", "Session Logged Out Please Exit and Login Again!")
        }
        else {
            AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
        }

        if (data != "1") {
            $('.dynamic-control' + strCurrentSubActivityName).get().forEach(function (control, index, array) {
                try {

                    if (control.type == "text") {
                        control.value = ""
                    }
                    else if (control.type.toUpperCase() == "TEXTAREA") {
                        control.value = ""
                    }
                    else if (control.type == "select-one") {
                        //control.selectedOptions[0].text = ""         
                        control.selectedIndex = 0;
                    }
                }
                catch (e) {
                    throw e;
                }
            });

            $('.dynamic-size-checkbox-control').get().forEach(function (control, index, array) {
                try {

                    if (control.type == "checkbox") {
                        control.checked = false
                    }
                }
                catch (e) {
                    throw e;
                }
            });
        }
    }

    function errorSaveMILessionDetails() {
        AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
    }
}

//To Save All Dicom Detail in Biznet, MI, And Dicom Physically
function SubmitMIFinalLesion() {
    debugger;
    var strCurrentSubActivityName = "";
    if ($("#hdnCurrentSubActivityName").val() != "") {
        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
    }

    //$.confirm({
    //    title: 'Confirm!',
    //    icon: 'fa fa-warning',
    //    content: 'USER CONFIRMATION',
    //    onContentReady: function () {
    //        var self = this;
    //        this.setContentPrepend('<div>MI</div>');
    //        setTimeout(function () {
    //            self.setContentAppend('<div>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Final All Dicom Details?</div>');
    //        }, 1000);
    //    },
    //    columnClass: 'medium',
    //    //animation: 'zoom',
    //    closeIcon: true,
    //    closeIconClass: 'fa fa-close',
    //    //columnClass: 'small',
    //    boxWidth: '30%',
    //    autoClose: 'danger|15000',
    //    animation: 'news',
    //    closeAnimation: 'news',
    //    closeAnimation: 'scale',
    //    backgroundDismissAnimation: 'random',
    //    //backgroundDismissAnimation: 'glow',
    //    type: 'blue',
    //    theme: 'dark',
    //    draggable: true,
    //    buttons: {
    //        info: {
    //            btnClass: 'btn-blue',
    //            text: 'OK (O)',
    //            keys: ['O'],
    //            action: function () {

                    cRadiologist = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase()
                    vLesionType = ""
                    vLesionSubType = ""

                    //For Reference
                    //vLesionType = "R1"
                    //vLesionSubType = "R1-TL"

                    data = {
                        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                        vWorkspaceId: $("#hdnvWorkspaceId").val(),
                        vSubjectId: $("#hdnvSubjectId").val(),
                        iMySubjectNo: $("#hdniMySubjectNo").val(),
                        vPeriodId: $("#hdniPeriod").val(),
                        vActivityId: $("#hdnvSubActivityId").val(),
                        iNodeId: $("#hdniSubNodeId").val(),
                        vLesionType: vLesionType,
                        vLesionSubType: vLesionSubType,
                        iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
                        iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
                        cRadiologist: cRadiologist,
                        iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val()
                    };

                    var lesiondata = {
                        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                        vWorkspaceId: $("#hdnvWorkspaceId").val(),
                        vSubjectId: $("#hdnvSubjectId").val(),
                        iMySubjectNo: $("#hdniMySubjectNo").val(),
                        ScreenNo: $("#hdnvMySubjectNo").val(),
                        vPeriodId: $("#hdniPeriod").val(),
                        vActivityId: $("#hdnvSubActivityId").val(),
                        iNodeId: $("#hdniSubNodeId").val(),
                        cSaveStatusFlagValidation: 'N'//Not Want To Set Validation For cSaveStatus so set Value To N
                    }

                    //*******************************For Skip Visit*******************************//

                    if ($("#hdnvSkipVisit").val() == "Y") {

                        //check for cSaveStatus that data is saved or not if yes than view only mode    

                        var MISkipLesionCRFDetail = {
                            vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                            vWorkspaceId: $("#hdnvWorkspaceId").val(),
                            vSubjectId: $("#hdnvSubjectId").val(),
                            iMySubjectNo: $("#hdniMySubjectNo").val(),
                            vPeriodId: $("#hdniPeriod").val(),
                            vActivityId: $("#hdnvSubActivityId").val(),
                            iNodeId: $("#hdniSubNodeId").val(),
                            vLesionType: vLesionType,
                            vLesionSubType: vLesionSubType,
                            iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
                            iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
                            cRadiologist: cRadiologist,
                            iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val(),
                            ScreenNo: $("#hdnvMySubjectNo").val(),
                            cSaveStatusFlagValidation: 'N',//Not Want To Set Validation For cSaveStatus so set Value To N
                            vActivityName: $("#hdnvActivityName").val()
                        }

                        $.ajax({
                            url: WebURL + "MIDicomViewer/SkipMIFinalLesionData",
                            type: "POST",
                            data: MISkipLesionCRFDetail,
                            async: true,
                            //timeout: 0,
                            //async: false,
                            success: function (data) {
                                if (data == "success") {
                                    //$.confirm({
                                    //    title: 'Confirm!',
                                    //    icon: 'fa fa-warning',
                                    //    content: 'SUCCESS',
                                    //    onContentReady: function () {
                                    //        var self = this;
                                    //        this.setContentPrepend('<div>MI</div>');
                                    //        setTimeout(function () {
                                    //            localStorage.setItem("IsReviewDone", "true");
                                    //            self.setContentAppend('<div>Dicom Detail Saved Successfully For <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>.</div>');
                                    //        }, 1000);
                                    //    },
                                    //    columnClass: 'medium',
                                    //    //animation: 'zoom',
                                    //    //closeIcon: true,
                                    //    //closeIconClass: 'fa fa-close',
                                    //    //columnClass: 'small',
                                    //    boxWidth: '30%',
                                    //    autoClose: 'info|15000',
                                    //    animation: 'news',
                                    //    closeAnimation: 'news',
                                    //    closeAnimation: 'scale',
                                    //    backgroundDismissAnimation: 'random',
                                    //    //backgroundDismissAnimation: 'glow',
                                    //    type: 'blue',
                                    //    theme: 'dark',
                                    //    draggable: true,
                                    //    buttons: {
                                    //        info: {
                                    //            btnClass: 'btn-blue',
                                    //            text: 'OK (O)',
                                    //            keys: ['O'],
                                    //            action: function () {
                                                    localStorage.setItem("IsReviewDone", "true");
                                                    AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!");
                                                    if (strCurrentSubActivityName == "-ALL") {
                                                        window.close();
                                                    }
                                                    else {
                                                        $('.btn-hide' + strCurrentSubActivityName).hide();
                                                        $('#ModalMIeSignature').modal('hide');
                                                    }
                                                    //Change by Hiren Rami
                                                    //window.close()
                                               // }
                                    //        },
                                    //    }
                                    //});
                                }
                                else if (data == "NO_LESION_DETAIL_FOUND") {
                                    AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                                }
                                else if (data == "SessionExpired") {
                                    AlertBox("WARNING", " Dicom Viewer", "Session Expited. Please Exit and Login Again!")
                                }
                                else {
                                    AlertBox("error", " Dicom Viewer", "Error While Saving Data!")
                                }
                            },
                            failure: function (response) {
                                AlertBox("error", " Dicom Viewer", "Error While Saving Data!" + response.d)
                            },
                            error: function (response) {
                                AlertBox("error", " Dicom Viewer", "Error While Saving Data!" + response.d)
                            }
                        });
                    }

                        //*******************************For Routine Entry*******************************//
                    else {
                        setTimeout(function () {

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //**************************************************Logic To Save Routine Entry****************************************************//

                            var DicomAnnotation = cornerstoneTools.saveDicomAnnotation();

                            var content = {};
                            //content.DicomAnnotation = "{\"DicomAnnotation\":" + JSON.stringify(DicomAnnotation) + "}";

                            MISubmitMIFinalLesionData = {
                                vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                                vWorkspaceId: $("#hdnvWorkspaceId").val(),
                                vSubjectId: $("#hdnvSubjectId").val(),
                                iMySubjectNo: $("#hdniMySubjectNo").val(),
                                vPeriodId: $("#hdniPeriod").val(),
                                vParentActivityId: $("#hdnvActivityId").val(),
                                iParentNodeId: $("#hdniNodeId").val(),
                                vActivityId: $("#hdnvSubActivityId").val(),
                                iNodeId: $("#hdniSubNodeId").val(),
                                vActivityName: $("#hdnSelectedvParentNodeDisplayName").val(),
                                vSubActivityName: $("#hdnSelectedvChildNodeDisplayName").val(),
                                cRadiologist: cRadiologist,
                                vLesionType: vLesionType,
                                vLesionSubType: vLesionSubType,
                                iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
                                iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
                                //iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val(),
                                iImageTranNo: 2,
                                ScreenNo: $("#hdnvMySubjectNo").val(),
                                //////vPeriodId: $("#hdniPeriod").val(),
                                /////vActivityId: $("#hdnvSubActivityId").val(),
                                /////iNodeId: $("#hdniSubNodeId").val(),
                                cSaveStatusFlagValidation: 'N',//Not Want To Set Validation For cSaveStatus so set Value To N
                                DicomAnnotation: DicomAnnotation,
                                DicomAnnotationDetail: JSON.stringify(DicomAnnotation)
                            }

                            $.ajax({
                                url: WebURL + "MIDicomViewer/SubmitMIFinalLesionData",
                                type: "POST",
                                data: MISubmitMIFinalLesionData,
                                async: true,
                                //data: { MISubmitMIFinalLesionData : JSON.stringify(MISubmitMIFinalLesionData) },
                                //timeout: 0,
                                //async: false,
                                //contentType: "application/json; charset=utf-8",
                                //dataType: "json",
                                success: function (data) {
                                    if (data == "success") {
                                        //alert("5");
                                        //AlertBox("SUCCESS", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                                        //window.close()

                                        //$.confirm({
                                        //    title: 'Confirm!',
                                        //    icon: 'fa fa-warning',
                                        //    content: 'SUCCESS',
                                        //    onContentReady: function () {
                                        //        var self = this;
                                        //        this.setContentPrepend('<div>MI</div>');
                                        //        setTimeout(function () {
                                        //            localStorage.setItem("IsReviewDone", "true");
                                        //            self.setContentAppend('<div>Dicom Detail Saved Successfully For <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>.</div>');
                                        //        }, 1000);
                                        //    },
                                        //    columnClass: 'medium',
                                        //    //animation: 'zoom',
                                        //    //closeIcon: true,
                                        //    //closeIconClass: 'fa fa-close',
                                        //    //columnClass: 'small',
                                        //    boxWidth: '30%',
                                        //    autoClose: 'info|15000',
                                        //    animation: 'news',
                                        //    closeAnimation: 'news',
                                        //    closeAnimation: 'scale',
                                        //    backgroundDismissAnimation: 'random',
                                        //    //backgroundDismissAnimation: 'glow',
                                        //    type: 'blue',
                                        //    theme: 'dark',
                                        //    draggable: true,
                                        //    buttons: {
                                        //        info: {
                                        //            btnClass: 'btn-blue',
                                        //            text: 'OK (O)',
                                        //            keys: ['O'],
                                        //            action: function () {
                                                        localStorage.setItem("IsReviewDone", "true");
                                                        AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!");
                                                        if (strCurrentSubActivityName == "-ALL") {
                                                            window.close();
                                                        }
                                                        else {
                                                            $('.btn-hide' + strCurrentSubActivityName).hide();
                                                            $('#ModalMIeSignature').modal('hide');
                                                        }
                                                        //Change by Hiren Rami
                                                        //window.close()
                                                    //}
                                        //        },
                                        //    }
                                        //});
                                    }
                                    else if (data == "NO_LESION_DETAIL_FOUND") {
                                        AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                                    }
                                    else if (data == "SessionExpired") {
                                        AlertBox("WARNING", " Dicom Viewer", "Session Expited. Please Exit and Login Again!")
                                    }
                                    else {
                                        AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!")
                                    }
                                },
                                error: function (e) {
                                    AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!" + e)
                                },
                                failure: function (response) {
                                    AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!" + response.d)
                                }
                            });

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        }, 0);
                    }
                    //$.alert('confirm!');
    //            }
    //        },
    //        danger: {
    //            btnClass: 'btn-red any-other-class',
    //            text: 'CANCEL (C)',
    //            keys: ['C'],
    //            action: function () {
    //                AlertBox("WARNING", " Dicom Viewer", "Please Save Data Again For <b>" + $("#hdnSelectedvParentNodeDisplayName").val() + " For " + $("#hdnSelectedvChildNodeDisplayName").val() + "</b> And Try Again Later!")
    //            }
    //        },
    //    }
    //});
}


function SaveAndchangesFinalLesion() {
    debugger;
    var strCurrentSubActivityName = "";
    if ($("#hdnCurrentSubActivityName").val() != "") {
        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
    }

    cRadiologist = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase()
    vLesionType = ""
    vLesionSubType = ""

    //For Reference
    //vLesionType = "R1"
    //vLesionSubType = "R1-TL"

    data = {
        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
        vWorkspaceId: $("#hdnvWorkspaceId").val(),
        vSubjectId: $("#hdnvSubjectId").val(),
        iMySubjectNo: $("#hdniMySubjectNo").val(),
        vPeriodId: $("#hdniPeriod").val(),
        vActivityId: $("#hdnvSubActivityId").val(),
        iNodeId: $("#hdniSubNodeId").val(),
        vLesionType: vLesionType,
        vLesionSubType: vLesionSubType,
        iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
        iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
        cRadiologist: cRadiologist,
        iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val()
    };

    var lesiondata = {
        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
        vWorkspaceId: $("#hdnvWorkspaceId").val(),
        vSubjectId: $("#hdnvSubjectId").val(),
        iMySubjectNo: $("#hdniMySubjectNo").val(),
        ScreenNo: $("#hdnvMySubjectNo").val(),
        vPeriodId: $("#hdniPeriod").val(),
        vActivityId: $("#hdnvSubActivityId").val(),
        iNodeId: $("#hdniSubNodeId").val(),
        cSaveStatusFlagValidation: 'Y'//Not Want To Set Validation For cSaveStatus so set Value To N
    }

    //*******************************For Skip Visit*******************************//

    //*******************************For Routine Entry*******************************//
                  
    setTimeout(function () {

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //**************************************************Logic To Save Routine Entry****************************************************//

        var DicomAnnotation = cornerstoneTools.saveDicomAnnotation();

        var content = {};
        //content.DicomAnnotation = "{\"DicomAnnotation\":" + JSON.stringify(DicomAnnotation) + "}";

        MISubmitMIFinalLesionData = {
            vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
            vWorkspaceId: $("#hdnvWorkspaceId").val(),
            vSubjectId: $("#hdnvSubjectId").val(),
            iMySubjectNo: $("#hdniMySubjectNo").val(),
            vPeriodId: $("#hdniPeriod").val(),
            vParentActivityId: $("#hdnvActivityId").val(),
            iParentNodeId: $("#hdniNodeId").val(),
            vActivityId: $("#hdnvSubActivityId").val(),
            iNodeId: $("#hdniSubNodeId").val(),
            vActivityName: $("#hdnSelectedvParentNodeDisplayName").val(),
            vSubActivityName: $("#hdnSelectedvChildNodeDisplayName").val(),
            cRadiologist: cRadiologist,
            vLesionType: vLesionType,
            vLesionSubType: vLesionSubType,
            iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
            iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
            iImageTranNo: 2,
            ScreenNo: $("#hdnvMySubjectNo").val(),
            cSaveStatusFlagValidation: 'Y',//Not Want To Set Validation For cSaveStatus so set Value To N
            DicomAnnotation: DicomAnnotation,
            DicomAnnotationDetail: JSON.stringify(DicomAnnotation)
        }

        $.ajax({
            url: WebURL + "MIDicomViewer/SubmitMIFinalLesionData",
            type: "POST",
            data: MISubmitMIFinalLesionData,
            async: true,
            success: function (data) {
                if (data == "success") {
                    //alert("5");
                    //AlertBox("SUCCESS", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                    //window.close()
                    location.reload();
                }
                else if (data == "NO_LESION_DETAIL_FOUND") {
                    AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                }
                else if (data == "SessionExpired") {
                    AlertBox("WARNING", " Dicom Viewer", "Session Expited. Please Exit and Login Again!")
                }
                else {
                    AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!")
                }
            },
            error: function (e) {
                AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!" + e)
            },
            failure: function (response) {
                AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Images Annotations and Data!" + response.d)
            }
        });

    }, 0);
}
//To View Data In DataTable that are saved for lesion
function getLesionDetails() {
    var parms = query.split('&');
    var vActivityId, iNodeId, vWorkspaceId, vSubjectId, iModifyBy, iMySubjectNo, ScreenNo, ParentWorkSpaceId, PeriodId;

    iModifyBy = $("#hdnuserid").val();
    ParentWorkSpaceId = $("#hdnParentWorkSpaceId").val();
    vWorkspaceId = $("#hdnWorkspaceId").val();
    vSubjectId = $("#hdnSubjectId").val();
    vActivityId = $("#hdnActivityID").val();
    iNodeId = $("#hdnNodeID").val();
    iMySubjectNo = $("#hdnMySubjectNo").val();
    ScreenNo = $("#hdnScreenNo").val();
    PeriodId = $("#hdnPeriodId").val();

    var ajaxdata = {
        vParentWorkSpaceId: ParentWorkSpaceId,
        vWorkspaceId: vWorkspaceId,
        vSubjectId: vSubjectId,
        iMySubjectNo: iMySubjectNo,
        ScreenNo: ScreenNo,
        vPeriodId: PeriodId,
        vActivityId: vActivityId,
        iNodeId: iNodeId
    }

    $.ajax({
        url: ApiURL + "GetData/LesionDetailsDATA",
        type: "POST",
        data: ajaxdata,
        async: false,
        success: function (jsonData) {
            if (jsonData.length > 0) {
                var activityDataSet = [];
                for (var v = 0 ; v < jsonData.length ; v++) {
                    var inDataSet = [];
                    inDataSet.push(jsonData[v].Name1, jsonData[v].Location1, jsonData[v].Description, jsonData[v].Organ1, jsonData[v]["Size (cm)"]);
                    activityDataSet.push(inDataSet);
                }

                otable = $("#tblLesionDetail").dataTable({
                    "bJQueryUI": true,
                    "sPaginationType": "full_numbers",
                    "bLengthChange": true,
                    "iDisplayLength": 10,
                    "bProcessing": true,
                    "bSort": true,
                    "autoWidth": false,
                    "aaData": activityDataSet,
                    "bInfo": true,
                    "bDestroy": true,
                    "sScrollX": "100%",
                    "aoColumns": [
                       { "sTitle": "Name" },
                       { "sTitle": "Location" },
                       { "sTitle": "Description" },
                       { "sTitle": "Organ" },
                       { "sTitle": "Size" }
                    ],
                    "columnDefs": [
                   {
                       "targets": [0, 1, 2, 3],
                       "searchable": false
                   },
                   { "bSortable": false, "targets": [0, 1, 2, 3] },
                    ],
                    "oLanguage": {
                        "sEmptyTable": "No Record Found",
                    },
                });
            }
        },
        failure: function (response) {
            AlertBox("error", " Dicom Viewer", "Error" + response.d)
        },
        error: function (response) {
            AlertBox("error", " Dicom Viewer", "Error" + response.d)
        }
    });
}

//To View Data of Temp Lesion from MI Temp Table from MI
function MILesionDetails() {
    var ArrayNo = localStorage.getItem("ArrayNo");
    var strCurrentSubActivityName = "";
    var strOnlyCurrentSubActivityName = "";
    if ($("#hdnCurrentSubActivityName").val() != "") {
        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
        strOnlyCurrentSubActivityName = $("#hdnCurrentSubActivityName").val().split("-")[1];
    }

    var iModifyBy = $("#hdnuserid").val();
    var ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
    var vWorkspaceId = $("#hdnvWorkspaceId").val();
    var vSubjectId = $("#hdnvSubjectId").val();
    var vActivityId = $("#hdnvSubActivityId").val();
    var iNodeId = $("#hdniSubNodeId").val();
    var iMySubjectNo = $("#hdniMySubjectNo").val();
    var ScreenNo = $("#hdnvMySubjectNo").val();
    var PeriodId = $("#hdniPeriod").val();

    var ajaxdata = {
        vParentWorkSpaceId: ParentWorkSpaceId,
        vWorkspaceId: vWorkspaceId,
        vSubjectId: vSubjectId,
        iMySubjectNo: iMySubjectNo,
        ScreenNo: ScreenNo,
        vPeriodId: PeriodId,
        vActivityId: vActivityId,
        iNodeId: iNodeId,
        cSaveStatusFlagValidation: 'N'//Not Want To Set Validation For cSaveStatus so set Value To N
    }

    setTimeout(function () {
        $.ajax({
            url: ApiURL + "GetData/LesionDetailsDATA",
            type: "POST",
            data: ajaxdata,
            async: false,
            success: function (jsonData) {
                if (jsonData == null) {
                    AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found!")
                }
                else {
                    if (jsonData.length > 0) {

                        for (var v = 0 ; v < jsonData.length ; v++) {
                            $('.dynamic-control' + strCurrentSubActivityName).get().forEach(function (control, index, array) {
                                try {

                                    if (control.type == "text") {
                                        if (jsonData[v].vMedExDesc == control.placeholder) {
                                            control.value = jsonData[v].vMedExResult
                                        }
                                    }
                                    else if (control.type.toUpperCase() == "TEXTAREA") {
                                        if (jsonData[v].vMedExDesc == control.placeholder) {
                                            control.value = jsonData[v].vMedExResult
                                        }
                                    }
                                    else if (control.type == "select-one") {
                                        if (jsonData[v].vMedExDesc == control.name) {
                                            //control.selectedOptions[0].text = jsonData[v].vMedExResult
                                            //control.selectedIndex = 1;
                                            var searchtext = jsonData[v].vMedExResult;
                                            for (var i = 0; i < control.options.length; ++i) {
                                                if (control.options[i].text === searchtext) control.options[i].selected = true;
                                            }
                                            //for (var i = 0; i < control.length; i++) {
                                            //    if (control[i].childNodes[0].nodeValue === jsonData[v].vMedExResult) {
                                            //            alert(i);
                                            //        }
                                            //    }
                                        }
                                    }
                                    else if (control.type == "radio") {
                                        if (jsonData[v].vMedExType.toUpperCase() == control.type.toUpperCase()) {
                                            control.value = jsonData[v].vMedExResult
                                        }
                                    }
                                }
                                catch (e) {
                                    throw e;
                                }
                            });
                        }

                        if (strOnlyCurrentSubActivityName == "TL") {
                            $('#LesionModel').on('shown.bs.modal', function () {
                                $('input:text:visible:first', this).focus();
                            })
                        }
                        else if (strOnlyCurrentSubActivityName == "NTL") {
                            $('#LesionModelNTL').on('shown.bs.modal', function () {
                                $('input:text:visible:first', this).focus();
                            })
                        }
                        else if (strOnlyCurrentSubActivityName == "NL") {
                            $('#LesionModelNL').on('shown.bs.modal', function () {
                                $('input:text:visible:first', this).focus();
                            })
                        }
                        else if (strOnlyCurrentSubActivityName == "TL" && ArrayNo == 3) {
                            LesionModelTL
                            $('#LesionModelTL').on('shown.bs.modal', function () {
                                $('input:text:visible:first', this).focus();
                            })
                        }
                        else {
                            $('#LesionModel').on('shown.bs.modal', function () {
                                $('input:text:visible:first', this).focus();
                            })
                        }
                    }
                }
            },
            failure: function (response) {
                AlertBox("error", " Dicom Viewer", "Error" + response.d)
            },
            error: function (response) {
                AlertBox("error", " Dicom Viewer", "Error" + response.d)
            }
        });
    }, 0);

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //**************************************************************To Get Previous Visit Data****************************************************//

    if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
        if (!($("#hdnvActivityName").val().toUpperCase().match("BL"))) {
            if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                MI_NLDetails();
            }
            else {
                var OPMODE;
                if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                    OPMODE = 3;
                }
                else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                    OPMODE = 4;
                }
                else {
                    OPMODE = 1;
                }

                var MILesionStatisticsDetails = {}
                MILesionStatisticsDetails = {
                    //MODE: 1,
                    MODE: OPMODE,
                    vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
                    vWorkspaceId: $("#hdnvWorkspaceId").val(),
                    vSubjectId: $("#hdnvSubjectId").val(),
                    iMySubjectNo: $("#hdniMySubjectNo").val(),
                    ScreenNo: $("#hdnvMySubjectNo").val(),
                    Radiologist: $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0],
                    vActivity: $("#hdnSelectedvParentNodeDisplayName").val(),
                    vSubActivity: $("#hdnSelectedvChildNodeDisplayName").val(),
                    vParentActivityId: $("#hdnSelectedvActivityId").val(),
                    iParentNodeId: $("#hdnSelectediPeriod").val(),
                    vActivityId: $("#hdnSelectedvSubActivityId").val(),
                    iNodeId: $("#hdnSelectediSubNodeId").val(),
                    cSaveStatus: 'Y'
                }

                $.ajax({
                    url: ApiURL + "GetData/LesionStatisticsDetails",
                    type: "POST",
                    async: false,
                    data: MILesionStatisticsDetails,
                    success: function (jsonData) {
                        if (jsonData != null) {
                            if (jsonData.length > 0) {
                                for (var v = 0; v < jsonData.length; v++) {
                                    $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                                        if (control.type == "text" || control.type == "TextArea") {
                                            if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {

                                                if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("TL-ASSESSMENT")))) {
                                                }
                                                else if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("NTL-ASSESSMENT")))) {
                                                }
                                                else {
                                                    if (control.placeholder.toUpperCase() == jsonData[v].vMedExDesc.toUpperCase()) {
                                                        if (control.length != 0) {
                                                            //control.value = parseFloat(jsonData[v].vMedExResult)
                                                            control.value = jsonData[v].vMedExResult
                                                            this.disabled = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (control.placeholder.toUpperCase() == jsonData[v].vMedExDesc.toUpperCase()) {
                                                    if (control.length != 0) {
                                                        //control.value = parseFloat(jsonData[v].vMedExResult)
                                                        control.value = jsonData[v].vMedExResult
                                                        this.disabled = true;
                                                    }
                                                }
                                            }

                                        }
                                        else if (control.type == "select-one") {
                                            if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {

                                                if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("TL-ASSESSMENT")))) {
                                                }
                                                else if ((jsonData[v].vSubActivityName.toUpperCase().match("NL")) && ((jsonData[v].vMedExDesc.toUpperCase().match("NTL-ASSESSMENT")))) {
                                                }
                                                else {
                                                    if (jsonData[v].vMedExDesc == control.name) {
                                                        //control.selectedOptions[0].text = jsonData[v].vMedExResult
                                                        //control.selectedIndex = 1;
                                                        var searchtext = jsonData[v].vMedExResult;
                                                        for (var i = 0; i < control.options.length; ++i) {
                                                            if (control.options[i].text === searchtext) control.options[i].selected = true;
                                                            this.disabled = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else {
                                                if (jsonData[v].vMedExDesc == control.name) {
                                                    //control.selectedOptions[0].text = jsonData[v].vMedExResult
                                                    //control.selectedIndex = 1;
                                                    var searchtext = jsonData[v].vMedExResult;
                                                    for (var i = 0; i < control.options.length; ++i) {
                                                        if (control.options[i].text === searchtext) control.options[i].selected = true;
                                                        this.disabled = true;
                                                    }
                                                }
                                            }

                                        }

                                        if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                            if (control.type == "text" || control.type == "TextArea") {
                                                if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                                    this.disabled = true;
                                                }
                                            }
                                            else if (control.type == "select-one") {
                                                if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                                    this.disabled = true;
                                                }
                                            }
                                        }

                                        else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                            if (control.type == "text" || control.type == "TextArea") {
                                                if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                                    this.disabled = true;
                                                }
                                            }
                                            else if (control.type == "select-one") {
                                                if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                                    this.disabled = true;
                                                }
                                            }
                                        }
                                    })
                                }
                            }
                            else {
                                //AlertBox("WARNING", " Dicom Viewer", "No Lesion Statistics Found for Previous Visit!")
                                $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                                    if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                        if (control.type == "text" || control.type == "TextArea") {
                                            if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                                this.disabled = true;
                                            }
                                        }
                                        else if (control.type == "select-one") {
                                            if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                                this.disabled = true;
                                            }
                                        }
                                    }

                                    else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                        if (control.type == "text" || control.type == "TextArea") {
                                            if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                                this.disabled = true;
                                            }
                                        }
                                        else if (control.type == "select-one") {
                                            if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                                this.disabled = true;
                                            }
                                        }
                                    }
                                })
                            }
                        }
                        else {
                            //AlertBox("WARNING", " Dicom Viewer", "No Lesion Statistics Found for Previous Visit!")
                            $('.dynamic-control' + strCurrentSubActivityName).each(function (index, control) {
                                if ($("#hdnvActivityName").val().toUpperCase().match("GLOBAL") || $("#hdnvActivityName").val().toUpperCase().match("RESPONSE")) {
                                    if (control.type == "text" || control.type == "TextArea") {
                                        if (((!(control.placeholder.toUpperCase().match("REMARK"))) || (!(control.placeholder.toUpperCase().match("REMARKS")))) && (!(control.placeholder.toUpperCase().match("GLOBAL")) || (!control.placeholder.toUpperCase().match("RESPONSE"))) && ((!control.placeholder.toUpperCase().match("COMMENTS")) || (!control.placeholder.toUpperCase().match("COMMENT")))) {
                                            this.disabled = true;
                                        }
                                    }
                                    else if (control.type == "select-one") {
                                        if ((!(control.name.toUpperCase().match("GLOBAL")) || (!control.name.toUpperCase().match("RESPONSE"))) && ((!control.name.toUpperCase().match("COMMENTS")) || (!control.name.toUpperCase().match("COMMENT")))) {
                                            this.disabled = true;
                                        }
                                    }
                                }

                                else if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") || $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                                    if (control.type == "text" || control.type == "TextArea") {
                                        if ((!(control.placeholder.toUpperCase().match("ADJUDICATOR"))) && ((!(control.placeholder.toUpperCase().match("REMARK"))) && (!(control.placeholder.toUpperCase().match("REMARKS"))))) {
                                            this.disabled = true;
                                        }
                                    }
                                    else if (control.type == "select-one") {
                                        if ((!(control.name.toUpperCase().match("ADJUDICATOR"))) && ((!(control.name.toUpperCase().match("REMARK"))) && (!(control.name.toUpperCase().match("REMARKS"))))) {
                                            this.disabled = true;
                                        }
                                    }
                                }
                            })
                        }
                    },
                    error: function (e) {
                    }
                });
            }
        }
        else {
            if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
                MI_NLDetails();
            }
        }
    }
}

//To View Data of CRF  From MI DB Lesion from MI
function MILesionSavedDetails() {
    var iModifyBy = $("#hdnuserid").val();
    var ParentWorkSpaceId = $("#hdnvParentWorkspaceId").val();
    var vWorkspaceId = $("#hdnvWorkspaceId").val();
    var vSubjectId = $("#hdnvSubjectId").val();
    var vActivityId = $("#hdnvSubActivityId").val();
    var iNodeId = $("#hdniSubNodeId").val();
    var iMySubjectNo = $("#hdniMySubjectNo").val();
    var ScreenNo = $("#hdnvMySubjectNo").val();
    var PeriodId = $("#hdniPeriod").val();

    var ajaxdata = {
        vParentWorkSpaceId: ParentWorkSpaceId,
        vWorkspaceId: vWorkspaceId,
        vSubjectId: vSubjectId,
        iMySubjectNo: iMySubjectNo,
        ScreenNo: ScreenNo,
        vPeriodId: PeriodId,
        vActivityId: vActivityId,
        iNodeId: iNodeId
    }

    $.ajax({
        url: ApiURL + "GetData/LesionSavedDetailsDATA",
        //url: ApiURL + "GetData/LesionDetailsDATA",
        type: "POST",
        data: ajaxdata,
        async: false,
        success: function (jsonData) {

            if (jsonData == null) {
                AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found!")
            }
            else {
                if (jsonData.length > 0) {

                    //This Logic is For To View CRF Table Temp Data in GridView that are also saved in BizNet
                    //Do Not Delete IT

                    var modalData = document.getElementById('MILesionModelData')
                    while (modalData.hasChildNodes()) {
                        modalData.removeChild(modalData.lastChild);
                    }

                    table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)
                    //var MiLesionDetailModel = $('<div class="modal-dialog">').append($('<div class="modal-content">')).append($('<div class="modal-header">'));

                    var MiLesionDetailModel = '';
                    //var table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)

                    //MiLesionDetailModel += '<div class="modal-dialog detail-dialog">';
                    MiLesionDetailModel += '<div class="modal-dialog" style="width:70%; !important">';
                    MiLesionDetailModel += '<div class="modal-content">';
                    MiLesionDetailModel += '<div class="modal-header">';
                    MiLesionDetailModel += '<button type="button" class="btn btn-info btn-sm pull-right box-tools" data-widget="remove" data-dismiss="modal" data-toggle="tooltip" title="" data-original-title="Remove">';
                    MiLesionDetailModel += '<i class="fa fa-times"></i>';
                    MiLesionDetailModel += '</button>';
                    MiLesionDetailModel += "<h4 class='modal-title'>" + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + "</h4>";
                    MiLesionDetailModel += '</div>';
                    MiLesionDetailModel += '<div class="modal-body" id="MILesionModelBodyData" >';

                    //This Logic Is For To Create Dynalic Tabel for Data Do Not Delete It
                    //MiLesionDetailModel += table;


                    MiLesionDetailModel += "<div class='row'>";
                    for (var v = 0; v < jsonData.length; v++) {
                        var id = null;
                        var value = null;
                        var classVal = null;
                        var type = null;
                        var placeHolder = null;
                        var tabIndex = null;
                        var option = null;
                        var checked = null;
                        var name = null;
                        var controlVal = null;
                        var forVal = null;


                        if ($("#hdnvActivityName").val().toUpperCase().match("BL") || $("#hdnvSubActivityName").val().toUpperCase().match("BASELINE")) {
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                MiLesionDetailModel += "<div class='col-lg-6 col-xs-12 form-group'>";
                            }
                            else {
                                MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                            }
                        }
                        else if ($("#hdnvActivityName").val().toUpperCase().match("MARK")) {
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                            }
                            else {
                                MiLesionDetailModel += "<div class='col-lg-4 col-xs-12 form-group'>";
                            }
                        }
                        else {
                            if ($("#hdnvSubActivityName").val().toUpperCase().match("NTL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NTL")) {
                                MiLesionDetailModel += "<div class='col-lg-6 col-xs-12 form-group'>";
                            }
                            else {
                                MiLesionDetailModel += "<div class='col-lg-3 col-xs-12 form-group'>";
                            }
                        }


                        //MiLesionDetailModel += "<div class='col-xs-4 form-group'>";
                        MiLesionDetailModel += "<div class=col-sm-12>";
                        //id = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + v;
                        id = jsonData[v].vMedExCode + "_" + v;
                        value = jsonData[v].vMedExDesc;
                        MiLesionDetailModel += Label(id, value);
                        MiLesionDetailModel += "</div>"
                        MiLesionDetailModel += "<div class=col-sm-12>";

                        if (jsonData[v].vMedExType.match("TextBox")) {
                            type = "text";
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            value = jsonData[v].vMedExResult
                            MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                        }
                        else if (jsonData[v].vMedExType.match("TextArea")) {
                            type = "text";
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            value = jsonData[v].vMedExResult
                            MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                        }
                        else if (jsonData[v].vMedExType.match("ComboBox")) {
                            type = "text";
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            value = jsonData[v].vMedExResult
                            MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                        }
                        else if (jsonData[v].vMedExType.match("Radio")) {
                            type = "text";
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            value = jsonData[v].vMedExResult
                            MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                        }
                        else {
                            type = "text";
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                            placeHolder = jsonData[v].vMedExDesc;
                            tabIndex = v;
                            value = jsonData[v].vMedExResult
                            MiLesionDetailModel += TextBox(type, classVal, id, placeHolder, tabIndex, value);
                        }
                        MiLesionDetailModel += "</div>"
                        MiLesionDetailModel += "</div>"
                    }


                    MiLesionDetailModel += '</div>';
                    MiLesionDetailModel += '</div>';
                    MiLesionDetailModel += '<div class="modal-footer">';
                    MiLesionDetailModel += '<button type="button" class="btn btn-default pull-left" data-dismiss="modal" id="btnMILesionModelDataClose">Close</button>';
                    MiLesionDetailModel += '</div>';
                    MiLesionDetailModel += '</div>';
                    MiLesionDetailModel += '</div>';

                    $("#MILesionModelData").append(MiLesionDetailModel);
                    //$("#MILesionModelBodyData").append(table);
                    //TableToDataTable('tblMIDetail')


                    $('.dynamic-control').each(function (index, control) {
                        if (control.length != 0) {
                            this.disabled = true;
                        }
                    })

                    //$(tblMIDetail).dataTable({
                    //    "bJQueryUI": true,
                    //    "sPaginationType": "full_numbers",
                    //    "bLengthChange": true,
                    //    "iDisplayLength": 10,
                    //    "bProcessing": true,
                    //    "bSort": true,
                    //    "autoWidth": false,
                    //    "bInfo": true,
                    //    "bDestroy": true,
                    //    "sScrollX": "100%",
                    //    "oLanguage": {
                    //        "sEmptyTable": "No Record Found",
                    //    },
                    //});                    
                }
            }
        },
        failure: function (response) {
            AlertBox("error", " Dicom Viewer", "Error" + response.d)
        },
        error: function (response) {
            AlertBox("error", " Dicom Viewer", "Error" + response.d)
        }
    });
}

//Not Used
//To Add Size in Size TextBox
function MILesionAddSize() {
    if (lineLength == "" || lineLength == '' || lineLength == null || lineLength == 'undefined') {
        AlertBox("WARNING", " Dicom Viewer", "Please Make Measurement First!")
    }
    else {
        $('.dynamic-control').each(function (index, control) {
            if (control.type == "text" || control.type == "TextArea") {
                if (control.placeholder.toUpperCase().match("SIZE")) {
                    if (control.length != 0) {
                        if (control.value == '' || control.value == "" || control.value == null) {
                            control.value = lineLength;
                            //lineLength = "";
                            control.disabled = true;
                            return false;
                        }
                    }
                }
            }
        })

        //var sizecontrol = document.getElementsByClassName('25041');
        //if (sizecontrol.length != 0) {
        //    for (var v = 0 ; v < sizecontrol.length ; v++) {
        //        if (sizecontrol[v].value == '' || sizecontrol[v].value == "" || sizecontrol[v].value == null) {
        //            sizecontrol[v].value = lineLength;
        //            sizecontrol[v].disabled = true;
        //            return true;
        //        }
        //    }
        //}

    }
}
//----------Not Used------------

//To Clear Lesion Data for TL and NTL
function MIClearLesionData() {
    var strCurrentSubActivityName = "";
    if ($("#hdnCurrentSubActivityName").val() != "") {
        strCurrentSubActivityName = "-" + $("#hdnCurrentSubActivityName").val().split("-")[1];
    }

    $('.dynamic-control' + strCurrentSubActivityName).get().forEach(function (control, index, array) {
        try {
            if (control.type == "text") {
                if (control.className.match("STATISTICS-CONTROL")) {
                    if (control.className.match("dynamic-diameter-sum-control")) {
                        control.value = ""
                    }
                    if (control.className.match("NADIR")) {
                        control.value = ""
                    }
                    if (control.className.match($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val() + "-BL")) {
                        control.value = ""
                    }
                }
                else {
                    control.value = ""
                }

            }
            else if (control.type.toUpperCase() == "TEXTAREA") {
                if (control.className == "STATISTICS-CONTROL") {
                    if (control.className.match("dynamic-diameter-sum-control")) {
                        control.value = ""
                    }
                    if (control.className.match("NADIR")) {
                        control.value = ""
                    }
                    if (control.className.match($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0] + "-" + $("#hdnSelectedvParentNodeDisplayName").val() + "-BL")) {
                        control.value = ""
                    }
                }
                else {
                    control.value = ""
                }
            }
            else if (control.type == "select-one") {
                //control.selectedOptions[0].text = ""         
                control.selectedIndex = 0;
            }
            else if (control.type == "checkbox") {
                //control.selectedOptions[0].text = ""         
                control.checked = false;
            }
            else {
                control.value = ""
            }
        }
        catch (e) {
            throw e;
        }
    });

    $('.dynamic-size-checkbox-control').each(function (index, control) {
        if (control.length != 0) {
            if (control.type == "checkbox") {
                control.checked = false;
            }
        }
    })

    MIClearLesionDataFlag = true;
}

//Session Expired
function SessionExpired() {
    AlertBox("WARNING", " Dicom Viewer", "Session Expired!")
}

//Details For NL
function MI_NLDetails() {
    //debugger;     //uncomment
    var strClassName = "";
    var IsTimePoint = $("#hdnIsTimePoint").val();
    if (IsTimePoint == "true") {
        strClassName = "-" + $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1]
    }
    else {
        strClassName = "-ALL"
    }

    var NLDetails = {}
    NLDetails = {
        //MODE: 1,
        MODE: 2,
        vParentWorkSpaceId: $("#hdnvParentWorkspaceId").val(),
        vWorkspaceId: $("#hdnvWorkspaceId").val(),
        vSubjectId: $("#hdnvSubjectId").val(),
        iMySubjectNo: $("#hdniMySubjectNo").val(),
        ScreenNo: $("#hdnvMySubjectNo").val(),
        Radiologist: $("#hdnSelectedvChildNodeDisplayName").val().split("-")[0],
        vActivity: $("#hdnSelectedvParentNodeDisplayName").val(),
        vSubActivity: $("#hdnSelectedvChildNodeDisplayName").val(),
        vParentActivityId: $("#hdnSelectedvActivityId").val(),
        iParentNodeId: $("#hdnSelectediPeriod").val(),
        vActivityId: $("#hdnSelectedvSubActivityId").val(),
        iNodeId: $("#hdnSelectediSubNodeId").val(),
        cSaveStatus: 'Y'
    }

    $.ajax({
        //url: ApiURL + "GetData/NLDetails",
        url: ApiURL + "GetData/LesionStatisticsDetails",
        type: "POST",
        async: false,
        data: NLDetails,
        success: function (NLJsonData) {
            if (NLJsonData != null) {
                if (NLJsonData.length > 0) {
                    $("#btnMIFinalSaveLesion").show();
                    $("#btnMIFinalSubmitLesion").show();
                    for (var v = 0; v < NLJsonData.length; v++) {
                        $('.dynamic-control' + strClassName).each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if (control.placeholder.toUpperCase() == NLJsonData[v].vMedExDesc.toUpperCase()) {
                                    if (control.length != 0) {
                                        control.value = (NLJsonData[v].vMedExResult)
                                        this.disabled = true;
                                    }
                                }
                            }
                            else if (control.type == "select-one") {
                                if (NLJsonData[v].vMedExDesc == control.name) {
                                    var searchtext = NLJsonData[v].vMedExResult;
                                    for (var i = 0; i < control.options.length; ++i) {
                                        if (control.options[i].text === searchtext) control.options[i].selected = true;
                                        this.disabled = true;
                                    }
                                }
                            }
                        })
                    }
                }
                else {
                    //Change by Hiren Rami
                    //$("#btnMIFinalSaveLesion").hide();
                    //$("#btnMIFinalSubmitLesion").hide();
                    //AlertBox("WARNING", " Dicom Viewer", "No New Lesion Details Found for Previous Visit! <br/> Please Fill The TL OR NTL Data For " + $("#hdnSelectedvParentNodeDisplayName").val())
                    //------------------------------------------
                }
            }
            else {
                //Change by Hiren Rami
                //$("#btnMIFinalSaveLesion").hide();
                //$("#btnMIFinalSubmitLesion").hide();                
                //AlertBox("WARNING", " Dicom Viewer", "No New Lesion Details Found for Previous Visit! <br/> Please Fill The TL OR NTL Data For " + $("#hdnSelectedvParentNodeDisplayName").val())
                //-------------------------------------
            }
        },
        error: function (e) {
        }
    });
}

function saveDicom() {
    var filename = 'dicom'
    cornerstoneTools.saveAs(element, filename);
}

function GetCurrentSubActivityName(e) {
    var strGetSubActivityDtl = e.getAttribute("data-attribute");
    $("#hdnvSubActivityId").val(strGetSubActivityDtl.split("#")[0]);
    $("#hdniSubNodeId").val(strGetSubActivityDtl.split("#")[1]);
    $("#hdnSelectedvSubActivityId").val(strGetSubActivityDtl.split("#")[0]);
    $("#hdnSelectediSubNodeId").val(strGetSubActivityDtl.split("#")[1]);

    $("#hdnvSubActivityName").val(e.innerText);
    $("#hdnSelectedvChildNodeDisplayName").val(e.innerText);
    $("#hdnCurrentSubActivityName").val(e.innerText);

    if (e.innerText.split("-")[1].toUpperCase() == "TL") {
        savedJsonData = savedJsonDataTL;
    }
    else if (e.innerText.split("-")[1].toUpperCase() == "NTL") {
        savedJsonData = savedJsonDataNTL;
    }
    else if (e.innerText.split("-")[1].toUpperCase() == "NL") {
        savedJsonData = savedJsonDataNL;
    }
}

function GetDetail(e) {
    var GetTitle = "";
    GetTitle = $("#hdnUserType").val() + "-" + e.id;
    var ActivityName = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1]
    var WorkSpaceId = $("#hdnvWorkspaceId").val();
    bttitle = GetTitle;
    var GetDetails = [];


    var ProjectLockDetailData = {
        vWorkspaceId: WorkSpaceId + '#' + ActivityName + '#' + GetTitle,
        SPName: 'Get_MEDEXWORKSPACEDTL'
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        url: WebURL + "MIHome/ProjectSubjectDetail",
        success: successImgTransmittalVisit,
        error: errorImgTransmittalVisit
    }

    $.ajax({
        data: ProjectFreezerDetailAjaxData.data,
        async: ProjectFreezerDetailAjaxData.async,
        type: ProjectFreezerDetailAjaxData.type,
        url: ProjectFreezerDetailAjaxData.url,
        complete: ProjectFreezerDetailAjaxData.success,
        error: ProjectFreezerDetailAjaxData.error
    });


    function successImgTransmittalVisit(jsonData) {
        if (jsonData.responseText != "") {
            jsonData = JSON.parse(jsonData.responseText);
            if (e.id == "TL") {
                $("#hdnIsTimePoint").val("true");
                localStorage.setItem("ArrayNo", 3);
            }
            else if (e.id == "NTL") {
                $("#hdnIsTimePoint").val("true");
                localStorage.setItem("ArrayNo", 1);
            }
            else if (e.id == "NL") {
                $("#hdnIsTimePoint").val("true");
                localStorage.setItem("ArrayNo", 2);
            }
            
            successMILesionDetail(jsonData)
            //$('#GetDetailTL').on('shown.bs.modal', function () {
            //    $('input:text:visible:first', this).focus();
            //})

            //if (jsonData.length > 0) {
            //    $("#GetDetailTL").append(jsonData);

            //}
        }

    }

    function errorImgTransmittalVisit() {

        AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");

    }


}

function GetCompareDetail() {
    var ActivityName = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1]
    var WorkSpaceId = $("#hdnvWorkspaceId").val();
    var subactivity1 = "R1-TL";
    var subactivity2 = "R2-TL";
    var subactivity3 = "R1-NTL";
    var subactivity4 = "R2-NTL";
    var subactivity5 = "R1-NL";
    var subactivity6 = "R2-NL";

    var GetDetails = [];


    var ProjectLockDetailData = {
        vWorkspaceId: WorkSpaceId + '#' + ActivityName + "#" + subactivity1 + "#" + subactivity2 + "#" + subactivity3 + "#" + subactivity4 + "#" + subactivity5 + "#" + subactivity6 + "#" + $("#hdnvSubjectId").val(),
        SPName: 'Get_MEDEXWORKSPACECOMPAREDTL'
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        url: WebURL + "MIHome/ProjectSubjectDetail",
        success: successImgTransmittalVisit,
        error: errorImgTransmittalVisit
    }

    $.ajax({
        data: ProjectFreezerDetailAjaxData.data,
        async: ProjectFreezerDetailAjaxData.async,
        type: ProjectFreezerDetailAjaxData.type,
        url: ProjectFreezerDetailAjaxData.url,
        complete: ProjectFreezerDetailAjaxData.success,
        error: ProjectFreezerDetailAjaxData.error
    });


    function successImgTransmittalVisit(jsonData) {
        $("#GetDetailTL").empty();
        if (jsonData.responseText != "") {
            jsonData = JSON.parse(jsonData.responseText);
            if (jsonData.length > 0) {
                var btnLesion = "";
                var data = "";
                data += '<div class="modal-dialog"  style="width: 70%; !important overflow: auto; margin: 30px auto;  max-height: 373px;">'
                data += "<div class='modal-content'>";
                data += "<div class='modal-header'>";
                data += "<h3>Comparative data</h4>"
                data += "<button type='button' class='btn btn-info btn-sm pull-right box-tools' data-widget='remove' data-dismiss='modal' data-toggle='tooltip' title='' data-original-title='Remove'><i class='fa fa-times'></i></button>";
                data += "</div>";
                data += " <div class='modal-body'>";
                data += "  <div class='row'>";
                data += "<div style='overflow-x:auto;'><table style='border-collapse: collapse; border-spacing: 0; width: 100%; border: 1px solid rgb(0, 0, 0);'>";
                
                for (var i = 0 ; i <= jsonData.length - 1; i++) {
                    if (jsonData[i].vMedExDesc == "Target Lesion (TL) Activity" || jsonData[i].vMedExDesc == "Non-Target Lesion (NTL) Activity" || jsonData[i].vMedExDesc == "New Lesion (NL) Activity" || jsonData[i].vMedExDesc == "Overall Timepoint Response") {
                        data += "<tr style='border: 1px solid #ccc;'><td colspan = 3 style='background-color: #4352FF; border: 1px solid #ddd; text-align: left; padding: 8px; color: white;'><center>" + jsonData[i].vMedExDesc + "</center></td></tr>"
                        data += "<tr style='border: 1px solid #ccc;'><th style='background-color: #B4B7DC; text-align: left; padding: 8px; color: black;'><center>Description</center></th><th style='background-color: #B4B7DC; text-align: left; padding: 8px; color: black;'><center>R1</center></th><th style='background-color: #B4B7DC; text-align: left; padding: 8px; color: black;'><center>R2</center></th>";
                    } else {
                        data += "<tr style='border: 1px solid #ccc;'><td style='border: 1px solid #ddd; text-align: left; padding: 8px;'>" + jsonData[i].vMedExDesc + "</td><td style='border: 1px solid #ddd; text-align: left; padding: 8px;'>" + jsonData[i].R1 + "</td><td style='border: 1px solid #ddd; text-align: left; padding: 8px;'>" + jsonData[i].R2 + "</td></tr>"
                    }
                    
                }
                data += "</table></div>";
                data += "<div class='modal-footer'>";
                data += "<button type='button' class='btn btn-default pull-left' data-dismiss='modal' id='btnClose'>Close</button>";
                data += "</div>";
                data += "</div>";
                data += "</div>";
                data += "</div>";
                data += "</div>";
                $("#GetDetailTL").append(data);
                $("#divLesion").append(btnLesion);
            }

            //successMILesionDetail(jsonData)
            //$('#GetDetailTL').on('shown.bs.modal', function () {
            //    $('input:text:visible:first', this).focus();
            //})

            //if (jsonData.length > 0) {
            //    $("#GetDetailTL").append(jsonData);

            //}
        }

    }

    function errorImgTransmittalVisit() {

        AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");

    }

}

function getUserProfile() {
    debugger;
    var vUserName = $('#hdnUserName').val();
    radiologist = $("#hdnUserType").val();
    radiologist2 = $("#hdnUserTypeR2").val();
    var data = {
        vUserName: vUserName
    }
    var ajaxData = {
        async: false,
        data: data,
        type: 'POST',
        url: ApiURL + "GetData/UserProfile"
    }
    setTimeout(function () {
        $.ajax({
            url: ajaxData.url,
            type: ajaxData.type,
            data: data,
            async: ajaxData.async,
            success: successUserProfile,
            error: errorUserProfile
        });
    }, 0);
    function successUserProfile(jsonData) {
        for (var i = 0; i <= jsonData.length - 1; i++) {
            if (radiologist == "R1") {
                if (jsonData[i].vUserTypeName == "Radiologist1") {
                    R_UserId = jsonData[i].iUserId;
                    R_UserType = jsonData[i].vUserTypeCode;
                    //VisitView(R_UserId, R_UserType);
                }
            }
            if (radiologist2 == "R2") {
                if (jsonData[i].vUserTypeName == "Radiologist2") {
                    R2_UserId = jsonData[i].iUserId;
                    R2_UserType = jsonData[i].vUserTypeCode;
                    //VisitView(R2_UserId, R2_UserType);
                }
            }
        }
        VisitView();
    }

    function errorUserProfile() {
        AlertBox('error', 'MI', 'Error While Retriving User Profile!')
    }
}

function btnMIFinalSubmitLesionClick() {
    var m_names = new Array("Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec");
    var d = new Date();
    var curr_date = d.getDate();
    var curr_month = d.getMonth();
    var curr_year = d.getFullYear();
    var time = [d.getHours(), d.getMinutes(), d.getSeconds()].join(':');
    $("#lblUserName").html($("#hdnUserNameWithProfile").val())
    //$("#lblDateDetail").html(new Date($.now()))
    $("#lblDateDetail").html(curr_date + "-" + m_names[curr_month] + "-" + curr_year + " " + time)
    $("#txtPassword").val("")
    $("#txtPassword").focus();
}

function VisitView() {
    debugger;
    var WorkSpaceID = $("#hdnvWorkspaceId").val();
    var SubjectID = $("#hdnvSubjectId").val();
    var index = 0;
    var visitDuplicateData = [];
    query = window.location.search.substring(1);
    getActivityDetail(WorkSpaceID, SubjectID, R_UserId, R_UserType); 
    if (query == "" || query == null) {
        var VisitData = localStorage.getItem("ActivityListForMeasurement");
        var VisitTabledata = "";
        VisitData = $.parseJSON(VisitData);
        //for (var i = 0 ; i < VisitData.length; i++) {
        //    if (VisitData[i].toString().split("#")[0] == $("#hdnvSubActivityName").val().split("-")[1]) {
        //        index = i;
        //    }
        //}
        //for (var j = 0; j < index; j++) {
        //    visitDuplicateData[j] = VisitData[j];
        //}

        if (VisitData.length > 0) {

            VisitTabledata += '<h4>Performed Actvity Detail</h4>';
            if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1")) {
                VisitTabledata += '<table style="width:100%" border="1"><tr><th>Activity</th><th colspan=2>R1 View</th></tr>';
                //VisitTabledata += '<tr><th></th><th>TL</th><th>NTL</th></tr>';
            } else if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {
                VisitTabledata += '<table style="width:100%" border="1"><tr><th>Activity</th><th colspan=2>R2 View</th></tr>';
                //VisitTabledata += '<tr><th></th><th>TL</th><th>NTL</th></tr>';
            }
            else {
                VisitTabledata += '<table style="width:100%" border="1"><tr><th>Activity</th><th colspan=2>R1 View</th><th colspan=2>R2 View</th></tr>';
                //VisitTabledata += '<tr><th></th><th>TL</th><th>NTL</th><th>TL</th><th>NTL</th></tr>';
            }
            
            jQuery.grep(VisitData, function (visit) {

                var ACTDATA = visit[0];
                var val = ACTDATA.split("#");
                var ActivityName = val[0];
                var iParentNodeId = val[1];
                if (ActivityName == "BL") {
                    VisitTabledata += '<tr><td style="color: green;">' + ActivityName + '</td>';
                    if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1")) {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R1" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                    else if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R2" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                    else {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R1" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R2" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                }
                else {
                    VisitTabledata += '<tr><td style="color: green;">' + ActivityName + '</td>';
                    if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1")) {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R1" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                    else if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R2" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                    else {
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R1" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                        VisitTabledata += '<td colspan=2><center><a  id="btn' + ActivityName + '" class="myIcon icon-success icon-ef-3 icon-ef-3a hover-color icon-center" data-tooltip="tooltip" name="R2" title="View" attrid="' + ActivityName + '" Onclick="onViewDicomImage(this)" Desc="' + ActivityName + '" style="cursor:pointer;"><i class="fa  fa-info-circle"></i></a></center></td>';
                    }
                }
                VisitTabledata += '</tr>';
                //data-target="#MeasurementModel"
            });
            VisitTabledata += '</table>';
            $("#divVisit").append(VisitTabledata);
        }
    }
}

function onViewDicomImage(e) {
    var url = $("#DicomViewer").val();
    subActivity = e.title;
    //var vVisit = $(e).attr("Desc").split("-")[1];
    var vVisit = $(e).attr("Desc");
    var vWorkSpaceId = $("#hdnvWorkspaceId").val();
    var vSubjectId = $("#hdnvSubjectId").val();
    var vMySubjectNo = $("#hdnvMySubjectNo").val();

    var iMySubjectNo = '';
    var iPeriod = '1';
    var vParentWorkspaceId = '';
    var ddlActivityVal = 0;
    var val = '';
    var vActivityId = '';
    var iNodeId = '';
    var ddlSubActivityVal = '';
    var vSubActivityId = '';
    var iSubNodeId = '';
    var vActivityName = '';
    var vSubActivityName = '';
    var iImgTransmittalHdrId = 0
    var iImgTransmittalDtlId = 0
    var iImageStatus = 0
    var vProjectNo = ''
    var ImgTransmittalDtl_iImageTranNo = 1;
    var ImageTransmittalImgDtl_iImageTranNo = 1;
    var isSubjectReject = 'N'
    var iModalityNo = ""
    var iAnatomyNo = "";
    var iImageCount = '1';
    var hdnIsViist = 'Y'
    var arrStorage = [];
    var ActivityData = [];
    var SubActivityData = [];
    var SubActivityNameData = [];

    ImgTransmittalVisit = [];

    GetImgTransmittalVisit(vWorkSpaceId, vSubjectId, vVisit)

    if (ImgTransmittalVisit.length > 0) {
        iImgTransmittalHdrId = ImgTransmittalVisit[0].iImgTransmittalHdrId
        vParentWorkspaceId = ImgTransmittalVisit[0].vParentWorkspaceId
        iNodeId = ImgTransmittalVisit[0].iNodeId
        iImgTransmittalDtlId = ImgTransmittalVisit[0].iImgTransmittalDtlId
        vActivityId = ImgTransmittalVisit[0].vActivityId
        vActivityName = ImgTransmittalVisit[0].vNodeDisplayName
        vProjectNo = ImgTransmittalVisit[0].vProjectNo
        iModalityNo = ImgTransmittalVisit[0].iModalityNo
        iImageStatus = ImgTransmittalVisit[0].iImageStatus
        ImgTransmittalDtl_iImageTranNo = ImgTransmittalVisit[0].ImgTransmittalDtl_iImageTranNo
        ImageTransmittalImgDtl_iImageTranNo = ImgTransmittalVisit[0].ImageTransmittalImgDtl_iImageTranNo
        iImageCount = ImgTransmittalVisit[0].iImageCount
        iMySubjectNo = ImgTransmittalVisit[0].iMySubjectNo
    }

    arryActivity = [];
    arryActivityList = [];

    if (e.name == "R1") {
        getActivityDetail(vWorkSpaceId, vSubjectId, R_UserId, R_UserType)
    }
    else if (e.name == "R2") {
        getActivityDetail(vWorkSpaceId, vSubjectId, R2_UserId, R2_UserType)
    }

    if (arryActivityList.length <= 0) {
        AlertBox("warning", "Dicom Study", "User does not have Activity Right.")
        return;
    }
    ActivityData = JSON.stringify(arryActivityList);

    var bActivity = false;

    for (var i = 0; i < arryActivityList.length; i++) {
        if (arryActivityList[i].toString().toUpperCase() == vActivityName.toUpperCase()) {
            bActivity = true
        }
    }

    if (bActivity == false) {
        AlertBox("warning", "Dicom Study", "User does not have Activity Right.")
        return;
    }

    arrySubActivityList = [];
    arrySubActivityNameList = [];
    strSingle = "";
    if (e.name == "R1") {
        getSubActivityDetail(vWorkSpaceId, iNodeId, vSubjectId, R_UserId, R_UserType)
    }
    else if (e.name == "R2") {
        getSubActivityDetail(vWorkSpaceId, iNodeId, vSubjectId, R2_UserId, R2_UserType)
    }
    if (arrySubActivityList.length <= 0) {
        AlertBox("warning", "Dicom Study", "User does not have Sub-activity Right.")
        return;
    }

    if (arrySubActivityList.length > 0) {
        if (strSingle != "") {
            vSubActivityId = strSingle.split('#')[0];
            iSubNodeId = strSingle.split('#')[1];
        }
        if (vVisit == "BL") {
            //if (subActivity == "TL") {
                vSubActivityName = arrySubActivityNameList[0]
            //} else if(subactivity == "ntl"){
            //    vsubactivityname = arrysubactivitynamelist[1]
            //}
        }
        else {
            vSubActivityName = arrySubActivityNameList[0]
        }
    }
    SubActivityData = JSON.stringify(arrySubActivityList);
    SubActivityNameData = JSON.stringify(arrySubActivityNameList);

    if (vActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || vActivityName.toUpperCase().match("MARK") || vActivityName.toUpperCase().match("BL")
        || vActivityName.toUpperCase().match("GLOBAL") || vActivityName.toUpperCase().match("RESPONSE") || vActivityName.toUpperCase().match("ADJUDICATOR")
        || vActivityName.toUpperCase().match("IOV ASSESSMENT")) {

        if (arrySubActivityList.length < 1) {
            AlertBox("warning", "Dicom Study", "User does not have Sub-activity Right.")
            return;
        }
    }
    else {
        if (arrySubActivityList.length < 3) {
            AlertBox("warning", "Dicom Study", "User does not have poper Sub-activity Right.")
            return;
        }
    }

    bValidation = false;

    Validation(vWorkSpaceId, vSubjectId, vMySubjectNo, iMySubjectNo, iPeriod, vParentWorkspaceId, vActivityId, iNodeId, vSubActivityId, iSubNodeId, vActivityName, vSubActivityName)

    if (bValidation == false) {
        return;
    }

    var strFormElement = '<form class="custom-form" method="post" action="' + url + '" target="_blank">';
    strFormElement += '<input type="hidden" name="vParentWorkspaceId" value="' + vParentWorkspaceId + '">';
    strFormElement += '<input type="hidden" name="vWorkSpaceId" value="' + vWorkSpaceId + '">';
    strFormElement += '<input type="hidden" name="vSubjectId" value="' + vSubjectId + '">';
    strFormElement += '<input type="hidden" name="vMySubjectNo" value="' + vMySubjectNo + '">';
    strFormElement += '<input type="hidden" name="iMySubjectNo" value="' + iMySubjectNo + '">';
    strFormElement += '<input type="hidden" name="iPeriod" value="' + iPeriod + '">';
    strFormElement += '<input type="hidden" name="vActivityId" value="' + vActivityId + '">';
    strFormElement += '<input type="hidden" name="iNodeId" value="' + iNodeId + '">';
    strFormElement += '<input type="hidden" name="vSubActivityId" value="' + vSubActivityId + '">';
    strFormElement += '<input type="hidden" name="iSubNodeId" value="' + iSubNodeId + '">';
    strFormElement += '<input type="hidden" name="vActivityName" value="' + vActivityName + '">';
    strFormElement += '<input type="hidden" name="vSubActivityName" value="' + vSubActivityName + '">';

    strFormElement += '<input type="hidden" name="vSkipVisit" value="N">';

    strFormElement += '<input type="hidden" name="iImgTransmittalHdrId" value="' + iImgTransmittalHdrId + '">';
    strFormElement += '<input type="hidden" name="iImgTransmittalDtlId" value="' + iImgTransmittalDtlId + '">';
    strFormElement += '<input type="hidden" name="iImageStatus" value="' + iImageStatus + '">';
    strFormElement += '<input type="hidden" name="vProjectNo" value="' + vProjectNo + '">';
    strFormElement += '<input type="hidden" name="iModalityNo" value="' + iModalityNo + '">';
    strFormElement += '<input type="hidden" name="iAnatomyNo" value="' + iAnatomyNo + '">';
    strFormElement += '<input type="hidden" name="iImageCount" value="' + iImageCount + '">';
    strFormElement += '<input type="hidden" name="ImgTransmittalDtl_iImageTranNo" value="' + ImgTransmittalDtl_iImageTranNo + '">';
    strFormElement += '<input type="hidden" name="ImageTransmittalImgDtl_iImageTranNo" value="' + ImageTransmittalImgDtl_iImageTranNo + '">';
    strFormElement += '<input type="hidden" name="subjectRejectionDtl" value="' + isSubjectReject + '">';
    strFormElement += '<input type="hidden" name="activityArray" value="' + arryActivity.join() + '">';
    strFormElement += '<input type="hidden" name="hdnIsViist" value="' + hdnIsViist + '">';

    strFormElement += '<input type="hidden" name="arrStorage" value="' + encodeURI(JSON.stringify(arrStorage)) + '">';
    strFormElement += '<input type="hidden" name="activityData" value="' + encodeURI(ActivityData) + '">';
    strFormElement += '<input type="hidden" name="subActivityData" value="' + encodeURI(SubActivityData) + '">';
    strFormElement += '<input type="hidden" name="subActivityNameData" value="' + encodeURI(SubActivityNameData) + '">';
    strFormElement += '</form>';


    var form = $(strFormElement);
    $(document.body).append(form[0]); //Resolve Chrome Version Change Issue 
    form.submit();

    $(".custom-form").remove();
}

function ShowToolTable() {
    debugger;
    var tr, currentdata, sizeofALLTL = 0.0, sizeofALLAL = 0.0, sizeofDISC = 0.0;
    var eventData = cornerstone.getEnabledElement($("#dicomImage")[0]);
    var enabledElement = cornerstone.getEnabledElement(element);
    var imagearray = enabledElement.image.imageId.split('/');
    var imageNameTemp = imagearray[imagearray.length - 1].split('?');
    var imageName = imageNameTemp[0].toUpperCase();

    var currentimageToolData = cornerstoneTools.getImagewiseToolData(imageName, "");

    if (currentimageToolData == undefined) {
        return;
    }
    ScaleData = ScaleData.filter(function (e) { return e.imageName != imageName });
    eCTDGridData = eCTDGridData.filter(function (e) { return e.vFileName != imageName || e.iDicomAnnotationNo != undefined; });
    eCTDGridData = [].concat(eCTDGridData, currentimageToolData);
    eCTDGridData.sort((a, b) => (a.vFileName > b.vFileName) ? 1 : ((b.vFileName > a.vFileName) ? -1 : 0));

    $("#tBodyToolDetails").empty();
    debugger;
    for (var toolind = 0; toolind < eCTDGridData.length; toolind++) {
        currentdata = JSON.parse(eCTDGridData[toolind].nvDicomAnnotation);
        if (currentdata.area == null) {
            for (var vd = 0 ; vd < DicomDataAnnotation.length ; vd++) {
                var Annotation = DicomDataAnnotation[vd];
                if (Annotation.vAnnotationType.toUpperCase() == "LENGTH") {
                    var dx = (currentdata.handles.end.x - currentdata.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (currentdata.handles.end.y - currentdata.handles.start.y) * (eventData.image.rowPixelSpacing || 1);
                    var length = Math.sqrt(dx * dx + dy * dy);
                    currentdata.area = Number(length).toFixed(2);
                }
                else if (Annotation.vAnnotationType.toUpperCase() == "RECTANGLEROI") {
                    var width = Math.abs(currentdata.handles.start.x - currentdata.handles.end.x);
                    var height = Math.abs(currentdata.handles.start.y - currentdata.handles.end.y);
                    var area = (width * (eventData.image.columnPixelSpacing || 1)) * (height * (eventData.image.rowPixelSpacing || 1));
                    currentdata.area = Number(area).toFixed(2);
                }
                else if (Annotation.vAnnotationType.toUpperCase() == "ORTHO") {
                    var dx = (currentdata.handles.end.x - data.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (currentdata.handles.end.y - data.handles.start.y) * (eventData.image.rowPixelSpacing || 1);
                    var length = Math.sqrt(dx * dx + dy * dy);
                    currentdata.area = Number(length).toFixed(2);
                }
                else if (Annotation.vAnnotationType.toUpperCase() == "PERPENDICULAR") {
                    var dx = (currentdata.handles.end.x - currentdata.handles.start.x) * (eventData.image.columnPixelSpacing || 1);
                    var dy = (currentdata.handles.end.y - currentdata.handles.start.y) * (eventData.image.rowPixelSpacing || 1);
                    var length = Math.sqrt(dx * dx + dy * dy);
                    currentdata.area = Number(length).toFixed(2);
                }
                else if (Annotation.vAnnotationType.toUpperCase() == "ELLIPTICALROI") {
                    var width = Math.abs(currentdata.handles.start.x - currentdata.handles.end.x);
                    var height = Math.abs(currentdata.handles.start.y - currentdata.handles.end.y);
                    var area = Math.PI * (width * eventData.image.columnPixelSpacing / 2) * (height * eventData.image.rowPixelSpacing / 2);
                    currentdata.area = Number(area).toFixed(2);
                }
            }
        }
        else {
            currentdata.area = Number(currentdata.area).toFixed(2);
        }

        tr = document.createElement("tr");
        if (eCTDGridData[toolind].cStatusIndi == "N") {
            tr.innerHTML = "<td style='overflow-wrap: break-word;'><a href='#' onclick='cornerstoneTools.scrollToIndex(element," + (currentdata.imageno - 1) + ");'>" + currentdata.text + "</a></td>" +
                           "<td>" + currentdata.area + " </td>" +
                           "<td></td>";
        }
        else {
            tr.innerHTML = "<td style='overflow-wrap: break-word;'><a href='#' onclick='cornerstoneTools.scrollToIndex(element," + (currentdata.imageno - 1) + ");'>" + currentdata.text + "</a></td>" +
                            "<td>" + currentdata.area + " </td>" +
                            "<td><button id='btntoolcleartools' type='button' class='btn btn-sm btn-default' data-container='body' data-toggle='tooltip' " +
                            " data-placement='bottom' title='Remove Tool' data-original-title='Remove Tool' onclick='removetool(\"" + currentdata.text.trim() + "\")'>" +
                            " <span class='fa fa-times'></span></button></td>";
        }
        $("#tBodyToolDetails").append(tr);
    }
}

function removetool(toolText) {
    debugger;
    var enabledElement = cornerstone.getEnabledElement(element);
    var imagearray = enabledElement.image.imageId.split('/');
    var imageNameTemp = imagearray[imagearray.length - 1].split('?');
    var imageName = imageNameTemp[0].toUpperCase();
    var currentimageToolData = cornerstoneTools.getImagewiseToolData(imageName, "");

    if (currentimageToolData == undefined) {
        return;
    }
    if (DicomTLToolData != undefined && DicomTLToolData.length > 0) {
        DicomTLToolData.splice(DicomTLToolData.map(function (Key) { return Key.imageName; }).indexOf(imageName), 1)
    }

    var d = currentimageToolData.filter(function (e) { return JSON.parse(e.nvDicomAnnotation).text == toolText; });
    if (d.length <= 0) {
        removeDBTool(toolText, imageName)
        return;
    }

    var toolStateManager = cornerstoneTools.getElementToolStateManager(element);
    var toolData = toolStateManager.get(element, d[0].vAnnotationType);
    // find this tool data
    var indexOfData = -1;
    for (var i = 0; i < toolData.data.length; i++) {
        if (toolData.data[i].text === toolText) {
            indexOfData = i;
        }
    }

    if (indexOfData !== -1) {
        toolData.data.splice(indexOfData, 1);

        var eventType = 'CornerstoneToolsMeasurementRemoved';
        var eventData = {
            toolType: d[0].vAnnotationType,
            element: element,
            measurementData: JSON.parse(d[0].nvDicomAnnotation)
        };
        $(element).trigger(eventType, eventData);
    }

    //cornerstoneTools.removeToolState(element, d[0].vAnnotationType, JSON.parse(d[0].nvDicomAnnotation));
    cornerstone.updateImage(element);
    ShowToolTable();
}
var arrySubActivityList = [];
var arrySubActivityNameList = [];
var strSingle = "";

function getSubActivityDetail(vWorkSpaceID, iParentNodeId, vSubjectId, UserId, UserType) {
    var iPeriod = 1;
    //var vUserTypeCode = $("#hdnUserTypeCode").val();
    //var iUserId = $("#hdnuserid").val();
    var MODE = '2';
    if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1") || $("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {
        var UserType = $("#hdnUserTypeCode").val();
        var UserId = $("#hdnuserid").val();
    }

    var subActivityDetailData = {
        vWorkSpaceID: vWorkSpaceID,
        iPeriod: iPeriod,
        iParentNodeId: iParentNodeId,
        vUserTypeCode: UserType,
        vSubjectId: vSubjectId,
        iUserId: UserId,
        MODE: MODE
    }

    var subActivityDetailAjaxData = {
        url: WebURL + "MIDicomStudy/ProjectActivityDetails",
        type: "POST",
        data: subActivityDetailData,
        async: false,
        success: successSubActivityDetail,
        error: errorSubActivityDetail
    }

    $.ajax({
        url: subActivityDetailAjaxData.url,
        type: subActivityDetailAjaxData.type,
        data: subActivityDetailAjaxData.data,
        async: subActivityDetailAjaxData.async,
        success: subActivityDetailAjaxData.success,
        error: subActivityDetailAjaxData.error
    });


    function successSubActivityDetail(jsonData) {
        if (jsonData == "error") {
            AlertBox("error", "Dicom Study", "Error While Getting Subject Detail!")
        }
        else if (jsonData.length > 0) {
            arrySubActivityList = [];
            jsonData = $.parseJSON(jsonData);
            for (var V = 0 ; V < jsonData.length; V++) {
                arrySubActivityList.push(jsonData[V].vActivityId + "#" + jsonData[V].iNodeId);
                arrySubActivityNameList.push(jsonData[V].vNodeDisplayName);
                if (jsonData[V].ParentActivityName == "BL") {
                    if (subActivity == "TL") {
                        if (V == 0) {
                            strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                        }
                    }
                    if (subActivity == "NTL") {
                        if (V == 1) {
                            strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                        }
                    }
                } else {
                    if (V == 0) {
                        strSingle = jsonData[V].vActivityId + "#" + jsonData[V].iNodeId;
                    }
                }
            }
            SubActivityData = JSON.stringify(arrySubActivityList);
            SubActivityNameData = JSON.stringify(arrySubActivityNameList);
            //localStorage.setItem("SubActivityList", JSON.stringify(arrySubActivityList));
            //localStorage.setItem("SubActivityNameList", JSON.stringify(arrySubActivityNameList));
            //if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
            //    $("#ddlSubActivity").val(Glo_SubActivity).change();
            //    localStorage.setItem("IsReviewDone", "false");
            //    localStorage.setItem("IsReviewContinue", "false");
            //    MIDicomStudy.getSubjectStudyDetail()
            //}

            //var strActivityName = $("#select2-ddlActivity-container")[0].innerHTML;
            //if (strActivityName.toUpperCase().match("ELIGIBILITY-REVIEW") || strActivityName.toUpperCase().match("MARK") || strActivityName.toUpperCase().match("BL") || (strActivityName.toUpperCase().match("GLOBAL") || strActivityName.toUpperCase().match("RESPONSE")) || strActivityName.toUpperCase().match("ADJUDICATOR") || strActivityName.toUpperCase().match("IOV ASSESSMENT")) {
            //    $(".SubActivity").show();
            //}
            //else {
            //    $("#ddlSubActivity").val(strSingle).change();
            //    $(".SubActivity").hide();
            //}

        }
        else {
            AlertBox("Information", "Dicom Study!", "No Data Available For Sub Activity!")
        }
    }
    function errorSubActivityDetail() {
        AlertBox("error", "Dicom Study!", "Error While Retriving  Sub Activity Details!");

    }

}

var arryActivity = [];
var arryActivityList = [];

function getActivityDetail(vWorkSpaceID, vSubjectId, UserId, UserType) {
    var iPeriod = 1;
    var iParentNodeId = 1;
    //var vUserTypeCode = $("#hdnUserTypeCode").val();

    //var iUserId = $("#hdnuserid").val();
    var MODE = '1';
    if ($("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST1") || $("#hdnUserTypeName").val().toUpperCase().match("RADIOLOGIST2")) {

        var UserType = $("#hdnUserTypeCode").val();
        var UserId = $("#hdnuserid").val();
    }


    var activityDetailData = {
        vWorkSpaceID: vWorkSpaceID,
        iPeriod: iPeriod,
        iParentNodeId: iParentNodeId,
        vUserTypeCode: UserType,
        vSubjectId: vSubjectId,
        iUserId: UserId,
        MODE: MODE
    }

    var activityDetailAjaxData = {
        url: WebURL + "MIDicomStudy/ProjectActivityDetails",
        type: "POST",
        data: activityDetailData,
        async: false,
        success: successActivityDetail,
        error: errorActivityDetail
    }


    $.ajax({
        url: activityDetailAjaxData.url,
        type: activityDetailAjaxData.type,
        data: activityDetailAjaxData.data,
        async: activityDetailAjaxData.async,
        success: activityDetailAjaxData.success,
        error: activityDetailAjaxData.error
    });


    function successActivityDetail(jsonData) {
        var arryActivityListMeasurement = [];
        if (jsonData == "error") {
            AlertBox("error", "Dicom Study", "Error While Getting Activity Detail!")
            getActivityDetail = false;
        }

        else if (jsonData.length > 0) {
            jsonData = $.parseJSON(jsonData);
            for (var V = 0 ; V < jsonData.length; V++) {
                arryActivity.push([jsonData[V].vNodeDisplayName, "False"]);
                arryActivityList.push([jsonData[V].vNodeDisplayName]);
                if (jsonData[V].cDataStatusColorCode == "GREEN") {
                    arryActivityListMeasurement.push([jsonData[V].vNodeDisplayName + "#" + jsonData[V].iNodeId]);
                    if (jsonData[V].vNodeDisplayName == $("#hdnvSubActivityName").val().split("-")[1]) {
                        break;
                    }
                }
            }
            ActivityData = JSON.stringify(arryActivityList);
            
            //localStorage.setItem("ActivityList", JSON.stringify(arryActivityList));
            localStorage.setItem("ActivityListForMeasurement", JSON.stringify(arryActivityListMeasurement));
            if (localStorage.getItem("IsReviewDone") == "true" || localStorage.getItem("IsReviewContinue") == "true") {
                //  $("#ddlActivity").val(Glo_Activity).change();
            }

        }
        else {
            AlertBox("Information", "Dicom Study!", "No Data Available For Activity!")
        }


    }
    function errorActivityDetail() {
        AlertBox("error", "Dicom Study!", "Error While Retriving Project Activity Details!");
    }

    function hasValueDeep(json, findValue) {
        var values = Object.values(json);
        var hasValue = values.includes(findValue);
        values.forEach(function (value) {
            if (typeof value === "object") {
                hasValue = hasValue || hasValueDeep(value, findValue);
            }
        })
        return hasValue;
    }
    //function RemoveCalibration() {
    //    alert("Hello");
    //}
}

var ImgTransmittalVisit = [];

function GetImgTransmittalVisit(vWorkspaceId, vSubjectID, vVisit) {

    ImgTransmittalVisit = [];


    var ProjectLockDetailData = {
        vWorkspaceId: vWorkspaceId + '#' + vSubjectID + '#' + vVisit,
        SPName: 'Pro_GetImgTransmittalVisit'
    }

    var ProjectFreezerDetailAjaxData = {
        async: false,
        data: ProjectLockDetailData,
        type: "POST",
        url: WebURL + "MIHome/ProjectSubjectDetail",
        success: successImgTransmittalVisit,
        error: errorImgTransmittalVisit
    }

    $.ajax({
        data: ProjectFreezerDetailAjaxData.data,
        async: ProjectFreezerDetailAjaxData.async,
        type: ProjectFreezerDetailAjaxData.type,
        url: ProjectFreezerDetailAjaxData.url,
        complete: ProjectFreezerDetailAjaxData.success,
        error: ProjectFreezerDetailAjaxData.error
    });


    function successImgTransmittalVisit(jsonData) {
        if (jsonData.responseText != "") {
            jsonData = JSON.parse(jsonData.responseText);

            if (jsonData.length > 0) {
                ImgTransmittalVisit = jsonData;

            }
        }

    }

    function errorImgTransmittalVisit() {

        AlertBox("error", "Dicom Study!", "Error While Retriving Project Subject Details!");

    }
}

var isSubjectReject = "N";
var bValidation = false;

function Validation(vWorkSpaceId, vSubjectId, vMySubjectNo, iMySubjectNo, iPeriod, vParentWorkspaceId, vActivityId, iNodeId, vSubActivityId, iSubNodeId, vActivityName, vSubActivityName) {

    if (vWorkSpaceId == "") {
        AlertBox("warning", "Image Review!", "Project not found.");
        DataSaveStatus = false;
        removeDiv();
        $(".spinner").hide();
    }
    var cRadiologist;
    cRadiologist = vSubActivityName.split('-')[0]


    bValidation = false;
    var DataSaveStatus = false;

    if ((!vActivityName.toUpperCase().match("ADJUDICATOR")) && (!vActivityName.toUpperCase().match("GLOBAL"))) {

        var MI_DataSaveStatus = {
            vParentWorkspaceId: vParentWorkspaceId,
            vWorkSPaceId: vWorkSpaceId,
            vActivityId: vActivityId,
            iNodeId: iNodeId,
            vSubActivityId: vSubActivityId,
            iSubNodeId: iSubNodeId,
            vsubjectid: vSubjectId,
            cRadiologist: cRadiologist,
        }

        $.ajax({
            url: WebURL + "MIDicomStudy/MI_DataSaveStatus",
            type: "POST",
            data: MI_DataSaveStatus,
            async: false,
            success: function (jsonDataSaveStatus) {
                var splitVal = jsonDataSaveStatus.split("@")[0];
                var data = splitVal.split("#")[0];
                if (data == "NOTALLOW" && jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "BLOCK") {
                    AlertBox("warning", "Image Review!", "Subject is Rejected And Dicom Study For " + vActivityName + " Is Not Assigned Yet.! ");
                }
                else if (data == "NOTALLOW") {
                    AlertBox("warning", "Image Review!", "Dicom Study For " + vActivityName + " Is Not Assigned Yet.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();

                }
                else if (data == "ERROR") {
                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();
                }
                else if (data == "ALLOW") {
                    if (splitVal.split("#")[1].toUpperCase() == "YES") {
                        DataSaveStatus = true;

                        for (var arryIndex = 0; arryIndex <= arryActivity.length - 1; arryIndex++) {

                            if (jsonDataSaveStatus.split("@")[1].split("#")[1].toUpperCase() == "BL") {
                                arryActivity[arryIndex][1] = "true";
                                if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                    arryActivity[arryIndex2][1] = "false";
                                }
                                else {
                                    arryActivity[arryIndex2][1] = "true";
                                }
                                break;
                            }

                            if (jsonDataSaveStatus.split("@")[1].split("#")[1].toUpperCase() == arryActivity[arryIndex][0].toUpperCase()) {
                                arryActivity[arryIndex][1] = "true";
                                for (var arryIndex2 = arryIndex + 1; arryIndex2 <= arryActivity.length - 1; arryIndex2++) {
                                    if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                        arryActivity[arryIndex2][1] = "false";
                                    }
                                    else {
                                        arryActivity[arryIndex2][1] = "true";
                                    }
                                }
                                break;
                            }
                        }

                        if (jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "BLOCK") {
                            isSubjectReject = "Y"
                        }
                        else if (jsonDataSaveStatus.split("@")[1].split("#")[0].toUpperCase() == "UNBLOCK") {
                            isSubjectReject = "N"
                        }
                    }
                    else {
                        DataSaveStatus = false;
                        AlertBox("error", "Image Review!!", "No Lesion Detail Found.Subject is not Eligible For Study.!");
                    }

                }
                else {
                    AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                    DataSaveStatus = false;
                    removeDiv();
                    $(".spinner").hide();

                }
            },
            error: function (e) {
                var error = e;
                AlertBox("error", "Image Review!", "Error While Retriving Image Assignment Status.!");
                DataSaveStatus = false;
                removeDiv();
                $(".spinner").hide();
            }
        });
    }
    else {
        DataSaveStatus = true;
        isSubjectReject = "N";
    }

    if (DataSaveStatus == true) {
        var CRFDataEntryStatus = {
            MODE: 1,
            vParentWorkSpaceId: vParentWorkspaceId,
            vWorkspaceId: vWorkSpaceId,
            vSubjectId: vSubjectId,
            iMySubjectNo: iMySubjectNo,
            ScreenNo: vMySubjectNo,
            Radiologist: cRadiologist,
            Activity: vActivityName

        }

        $.ajax({
            url: WebURL + "MIDicomStudy/CRFDataEntryStatus",
            type: "POST",
            data: CRFDataEntryStatus,
            async: false,
            success: function (jsonData) {
                if (jsonData.includes("#")) {
                    if (jsonData.split("#")[1].split("@")[0].toUpperCase() == "BLOCK") {
                        isSubjectReject = "Y";
                    }

                    for (var arryIndex = 0; arryIndex <= arryActivity.length - 1; arryIndex++) {
                        if (jsonData.split("#")[1].split("@")[1] != "" || jsonData.split("#")[1].split("@")[1] != null) {
                            if (jsonData.split("#")[1].split("@")[1].toUpperCase() == "BL") {
                                arryActivity[arryIndex][1] = "true";
                                if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                    arryActivity[arryIndex2][1] = "false";
                                }
                                else {
                                    arryActivity[arryIndex2][1] = "true";
                                }
                                break;
                            }

                            if (jsonData.split("#")[1].split("@")[1].toUpperCase() == arryActivity[arryIndex][0].toUpperCase()) {
                                arryActivity[arryIndex][1] = "true";
                                for (var arryIndex2 = arryIndex + 1; arryIndex2 <= arryActivity.length - 1; arryIndex2++) {
                                    if (arryActivity[arryIndex2][0].toUpperCase() == "GLOBAL RESPONSE" || arryActivity[arryIndex2][0].toUpperCase() == "ADJUDICATOR" || arryActivity[arryIndex2][0].toUpperCase() == "IOV ASSESSMENT") {
                                        arryActivity[arryIndex2][1] = "false";
                                    }
                                    else {
                                        arryActivity[arryIndex2][1] = "true";
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                if ((isSubjectReject == "Y") || vActivityName.toUpperCase().match("GLOBAL RESPONSE")) {
                    jsonData = "success";
                }
                var data = jsonData.split("#")[0];
                if (data == "error") {
                    $("#legend").hide();
                    AlertBox("error", "Image Review!", "Error While Retriving CRF Data Entry Control Details.!");
                    return false;
                }
                else if (data == "NO-DATA") {
                    $("#legend").hide();
                    AlertBox("error", "Image Review!", "No Data For CRF Data Entry Control Details.!");
                    return false;
                }
                else if (data == "success") {
                    DataSaveStatus = true
                    bValidation = true
                }
                else {
                    if (data == "") {
                        AlertBox("warning", "Image Review!", "No Detail Found.!")
                    }
                    else {
                        AlertBox("warning", "Image Review!", data)
                    }
                    removeDiv();
                    $(".spinner").hide();
                    return false;
                }
            },
            error: function (e) {
                alert(e);
            }
        });
    }

}