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

cornerstoneTools.toolStyle.setToolWidth(3);
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
    if ($("#hdniImageStatus").val() == 1 || $("#hdniImageStatus").val() == "1") {
        cornerstone.loadAndCacheImage(imageIds[newImageIdIndex]).then(function (image) {
            document.getElementById("sliceInterval").style.display = "block";
            $("#sliceInterval").text("Slice Location : " + image.data.intString('x00201041'))
        });
    }
}

function selectImage(event) {
    debugger
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
    //$('#rotation').text("Rotation: " + Math.round(viewport.rotation) + "°");
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
        debugger;
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
                debugger;

                document.getElementById("sliceThickness").style.display = "block";
                $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))

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

            query = window.location.search.substring(1);

            if (!query == "" || !query == null) {
                document.getElementById("sliceThickness").style.display = "block";
                $("#sliceThickness").text("Slice Thickness : " + image.data.string('x00180050'))

                document.getElementById("sliceInterval").style.display = "none";
                //$("#sliceInterval").text("Slice Interval : " + image.data.uint16('x00180088'))
                $("#sliceInterval").text("Slice Interval : " + image.data.string('x00180088'))

                document.getElementById("acquisitionDate").style.display = "block";
                $("#acquisitionDate").text("Acquisition Date : " + image.data.string('x00080022'))
 
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

$(document).ready(function () {


    $('#ModalMIeSignature').on('shown.bs.modal', function () {
        $('#txtPassword').focus();
    })


    debugger;
    //success();

    createDiv();
    $(".spinner").show();

    query = window.location.search.substring(1);

    if (query == '' || query == "" || query == null) {
        if (!$("#hdnvActivityName").val().toUpperCase().match("MARK") && !$("#hdnvActivityName").val().toUpperCase().match("BL") && !$("#hdnvActivityName").val().toUpperCase().match("BASE") && !$("#hdnvActivityName").val().toUpperCase().match("BASELINE") && !$("#hdnvActivityName").val().toUpperCase().match("BASE LINE") && !$("#hdnvActivityName").val().toUpperCase().match("TP") && !$("#hdnvActivityName").val().toUpperCase().match("GLOBAL") && !$("#hdnvActivityName").val().toUpperCase().match("GLOBAL RESPONSE") && !$("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")) {
            AlertBox("ERROR", " Dicom Viewer", "NOT ALLOWED ! NO USER RIGHTS !");
            return false;
        }

        if (!$("#hdnvSubActivityName").val().toUpperCase().match("TL") && !$("#hdnvSubActivityName").val().toUpperCase().match("NTL") && !$("#hdnvSubActivityName").val().toUpperCase().match("NL") && !$("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR") && !$("#hdnvSubActivityName").val().toUpperCase().match("GLOBAL") && !$("#hdnvSubActivityName").val().toUpperCase().match("GLOBAL RESPONSE")) {
            AlertBox("ERROR", " Dicom Viewer", "NOT ALLOWED ! NO USER RIGHTS !");
            return false;
        }
    }

    setTimeout(function () {
        createDiv();
        $(".spinner").show();
    }, 0);


    if ($("#hdnvSkipVisit").val() == "Y") {
        $("#divViewer").empty();
        $("#divRow").empty();
    }

    $('#LesionModel').on('show', function () {
        this.focus();
    })


    var divHeader = document.getElementById('divHeader')
    while (divHeader.hasChildNodes()) {
        divHeader.removeChild(divHeader.lastChild);
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
    debugger;
    if (query == '' || query == "" || query == null) {
        $.ajax({
            url: ApiURL + "GetData/ProjectActivityDetails",
            type: "POST",
            data: activityDetailData,
            async: false,
            success: function (jsonData) {
                if (jsonData.length > 0) {
                    for (var v = 0 ; v < jsonData.length ; v++) {
                        if (($("#hdnvActivityId").val() == jsonData[v].vActivityId) && ($("#hdniNodeId").val() == jsonData[v].iNodeId)) {
                            $("#hdnSelectedvParentNodeDisplayName").val(jsonData[v].vNodeDisplayName.toUpperCase())
                            $("#hdnSelectedvActivityId").val(jsonData[v].vActivityId)
                            $("#hdnSelectediPeriod").val(jsonData[v].iNodeId)
                        }
                        if (($("#hdnvSubActivityId").val() == jsonData[v].vActivityId) && ($("#hdniSubNodeId").val() == jsonData[v].iNodeId)) {
                            $("#hdnSelectedvChildNodeDisplayName").val(jsonData[v].vNodeDisplayName.toUpperCase())
                            $("#hdnSelectedvSubActivityId").val(jsonData[v].vActivityId)
                            $("#hdnSelectediSubNodeId").val(jsonData[v].iNodeId)
                        }
                    }
                }
                else {
                    AlertBox("Error", " Dicom Viewer", "No Activity Node  Detail Found. Please Try Again Later!")
                    return false;
                }
            },
            error: function (e) {
                AlertBox("Error", " Dicom Viewer", "Error While Retriving Activity Node Detail. Please Try Again Later!")
                return false;
            }
        });
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
        cRadiologist = $("#hdnvSubActivityName").val().split('-')[0];
        if (cRadiologist == null || cRadiologist == "") {
            AlertBox("warning", "Dicom Study!", "Please Select Proper Sub Activity To Review Dicom!");
            return false
        }
    }

    debugger;
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
            ImageTransmittalImgDtl_iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val()

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

    $("#btnSaveLesion").on("click", function () {
        debugger;
        SaveLession();
    });

    $("#btnSubmitLesion").on("click", function () {
        debugger;
        SubmitLesion();
    });

    $("#btnMISaveLesion").on("click", function () {
        debugger;
        SaveMILession();
    });

    $("#btnMISubmitLesion").on("click", function () {
        debugger;
        SubmitMILesion();
    });

    $("#btnMIFinalSaveLesion").on("click", function () {
        debugger;
        SaveMIFinalLession();
    });

    $("#btnMIFinalSubmitLesion").on("click", function () {
        debugger;
        var eSignData = "";

        eSignData += '<div class="modal fade" id="ModalMIeSignature" role="dialog">';
        eSignData += '<div class="modal-dialog">';
        eSignData += '<div class="modal-content">';
        eSignData += '<div class="modal-header">';
        eSignData += '<button type="button" class="btn btn-info btn-sm pull-right box-tools" data-widget="remove" data-dismiss="modal" data-toggle="tooltip" title="" data-original-title="Remove">';
        eSignData += '<i class="fa fa-times"></i>';
        eSignData += '</button>';
        eSignData += '<h4 class="modal-title">e-Signature Authentication</h4>';
        eSignData += '</div>';
        eSignData += '<div class="modal-body">';
        eSignData += '<div class="row">';
        eSignData += '<div class="col-lg-6 col-xs-12 form-group">';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<label for="" id="lblUser">User :</label>';
        eSignData += '<label for="" id="lblUserName"></label>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '<div class="col-lg-6 col-xs-12 form-group">';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<label for="" id="lblDate">DateTime :</label>';
        eSignData += '</div>';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<label for="" id="lblDateDetail"></label>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '<div class="col-lg-12 col-xs-12 form-group">';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<label for="txtPassword">Password*</label>';
        eSignData += '</div>';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<input type="password" class="form-control" id="txtPassword" placeholder="e-Signature" tabindex="1">';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '<div class="col-lg-12 col-xs-12 form-group">';
        eSignData += '<div class="col-sm-12">';
        eSignData += '<label id="lbleSignature" for="">I Here by Confirm Signing of this Record Electronically.</label>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '<div class="modal-footer">';
        eSignData += '<button type="button" class="btn btn-default pull-left" data-dismiss="modal" id="btnCloseMIeSignature">Close</button>';
        eSignData += '<button type="button" class="btn btn-primary" id="btnMIeSignatureVerification">Submit</button>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '</div>';
        eSignData += '</div>';

        //var MIeSign = document.getElementById('MIeSign')
        //while (MIeSign.hasChildNodes()) {
        //    MIeSign.removeChild(MIeSign.lastChild)
        //}

        //$("#MIeSign").append(eSignData);

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

    });

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
                            debugger;
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

$(document).keypress(function (e) {
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

function successSubjectImageStudyDetail(jsonData) {
    debugger;
    if (jsonData.length > 0) {

        var element = $('#dicomImage').get(0);
        var imgStatus = $("#hdniImageStatus").val()

        $("#hdncR1TLReviewStatus").val(jsonData[0].cR1TLReviewStatus)
        $("#hdncR1NTLReviewStatus").val(jsonData[0].cR1NTLReviewStatus)
        $("#hdncR2TLReviewStatus").val(jsonData[0].cR2TLReviewStatus)
        $("#hdncR2NTLReviewStatus").val(jsonData[0].cR2NTLReviewStatus)
        $("#hdncReviewStatusValue").val(jsonData[0].cReviewStatus);


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

        //*********************************For Reference use do not delete it.**************************//

        //imageIds.push("http://90.0.0.68/DICOM/DICOM/0000009471/AH15-01150/22/1/3/Updated/0000009471.png")                        
        //imageIds.push("dicomweb://90.0.0.68/DICOM/DICOM/0000009471/AH15-01150/22/1/3/Uploaded/CT0001.dcm")
        //imageIds.push("http://90.0.0.68/Dicom_Viewer2/1.png");

        //*********************************For Reference use do not delete it.**************************//


        if (imgStatus == 1) {
            for (var i = 0; i < jsonData.length; i++) {
                imageIds.push(DicomURL_1 + jsonData[i].vServerPath + "?" + Math.random());
                //cornerstone.loadAndCacheImage(imageIds[i] + "?" + Math.random());
                //imageIds.push(DicomURL_1 + jsonData[i].vServerPath);
                cornerstone.loadAndCacheImage(imageIds[i]);
            }
        }
        if (imgStatus == 2) {
            for (var i = 0; i < jsonData.length; i++) {
                imageIds.push(DicomURL_2 + jsonData[i].vServerPath + "?" + Math.random());
                //cornerstone.loadAndCacheImage(imageIds[i] + "?" + Math.random());
                //imageIds.push(DicomURL_2 + jsonData[i].vServerPath);
                cornerstone.loadAndCacheImage(imageIds[i]);
            }
        }

        //updateTheImage(0);
        loadAndViewImage(imageIds[0]);

        var button = "";

        var buttondata = document.getElementById('dicomButtonGroup')
        while (buttondata.hasChildNodes()) {
            buttondata.removeChild(buttondata.lastChild)
        }

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
        button += '<button id="toolrotation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Rotation" data-original-title="Rotation" onclick="toolrotation()"><span class="fa fa-repeat"></span></button>';
        button += '<button id="toolarrowannotation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Arrow Annotation" data-original-title="Arrow Annotation" onclick="toolarrowannotation()"><span class="fa fa-tags"></span></button>';
        button += '<button id="toolmagnify" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Magnify" data-original-title="Magnify" onclick="toolmagnify()"><span class="fa fa-search"></span></button>';
        button += '<button id="toolresettools" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Reset Tools" data-original-title="Reset Tools" onclick="toolresettools()"><span class="fa fa-refresh"></span></button>';
        button += '<button id="toolcleartools" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Clear Tools" data-original-title="Clear Tools" onclick="toolcleartools()"><span class="fa fa-times"></span></button>';
        button += '<button id="toolresetview" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Reset View" data-original-title="Reset View" onclick="toolresetview()"><span class="fa fa-picture-o"></span></button>';
        //button += '@*<button id="toolinterpolation" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Interpolation" data-original-title="Interpolation"><span class="fa fa-picture-o"></span></button>*@';
        button += '<button id="toolfreeformroi" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="FreeForm ROI" data-original-title="FreeForm ROI" onclick="toolfreeformroi()"><span class="fa fa-paint-brush"></span></button>';
        button += '<button id="toolhighlight" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Highlight" data-original-title="Highlight" onclick="toolhighlight()"><span class="fa fa-bookmark"></span></button>';
        button += '<button id="toolfullscreen" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Full Screen" onclick="toolfullscreen()"><span class="fa fa-arrows-alt"></span></button>';

        ReviewStatus = jsonData[0].cReviewStatus;

        if (ReviewStatus == 'N') {
            //button += '<button id="toolsave" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Save" data-original-title="Highlight" onclick="toolsave()"><span class="fa fa-file"></span></button>';
        }
        else {
            //button += '<button id="toolsave" type="button" class="btn btn-sm btn-default" data-container="body" data-toggle="tooltip" data-placement="bottom" title="Save" data-original-title="Highlight" onclick="toolsave()"><span class="fa fa-file"></span></button>';
        }

        $("#dicomButtonGroup").append(button);

        if (query == '' || query == "" || query == null) {
            debugger;
            if ($("#hdnEditPreviousVisit").val() == "true") {

                if ($("#hdniImageStatus").val() == "1") {

                    var SubjectSubSequentImageStudyDetailData = {
                        iImgTransmittalHdrId: $("#hdniImgTransmittalHdrId").val(),
                        iImgTransmittalDtlId: $("#hdniImgTransmittalDtlId").val(),
                        iImageStatus: '2',
                        cRadiologist: $("#hdnvSubActivityName").val().split('-')[0],
                        MODE: "1",
                        ImageTransmittalImgDtl_iImageTranNo: $("#hdnImageTransmittalImgDtl_iImageTranNo").val()
                    }

                    $.ajax({
                        url: ApiURL + "GetData/SubjectSubSequentImageStudyDetail",
                        type: "POST",
                        async: false,
                        data: SubjectSubSequentImageStudyDetailData,
                        success: function (SubSequentImageJson) {
                            if (SubSequentImageJson != null) {
                                if (SubSequentImageJson.length > 0) {
                                    AlertBox("WARNING", " Dicom Viewer", "Please Select Sub Sequent Image To Mark <b> " + $("#hdnSelectedvParentNodeDisplayName").val() + " For " + $("#hdnSelectedvChildNodeDisplayName").val() + "</b>")
                                    return
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
                        },
                        error: function (e) {
                            AlertBox("ERROR", " Dicom Viewer", "Error While Retriving Sub Sequent Image Study Details!")
                        }
                    });

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
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
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
                                btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
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
                            btnLesion += '<button style="text-align: right" type="button" id="btnLesionFinalSavedDetail" onclick="MILesionSavedDetails()" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#MILesionModelData" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val() + " DETAIL" + '</button>';
                            $("#divLesion").append(btnLesion);
                        }
                    }

                }

                    ///// For R2
                else if ($("#hdnvSubActivityName").val().toUpperCase().match("R2")) {


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

        //IF NO IMAGE FOUND IN THIS SUCCESS METHOD THEN SKIP VISIT
    else {
        if (($("#hdnvActivityName").val().toUpperCase().match("GLOBAL")) || ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR"))) {
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
}

function errorSubjectImageStudyDetail() {
    AlertBox("error", " Dicom Viewer", "Error While Retriving Dicom Image For Subject!")
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
        data += "<button type='button' class='btn btn-primary' id='btnSaveLesion'>Save changes</button>";
        data += "<button type='button' class='btn btn-primary' id='btnSubmitLesion'>Submit</button>";
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
    debugger;
    if (jsonData.length > 0) {
        var savedJsonData = []
        var finalData = []
        var LesionData = []
        var MARKData = []
        var modaldata = document.getElementById('LesionModel')
        while (modaldata.hasChildNodes()) {
            modaldata.removeChild(modaldata.lastChild);
        }

        var divdata = document.getElementById('divLesion')
        while (divdata.hasChildNodes()) {
            divdata.removeChild(divdata.lastChild);
        }

        if ((!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) && (!($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {

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
                        if (jsonData.length > 0) {
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

        var btnLesion = "";
        var data = "";
        var markdata = ""

        if ((!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) && (!($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) && (!($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                if (MARKData[0] != null) {
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
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////
        debugger;
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
            type: "POST",
            data: ajaxdata,
            async: false,
            success: function (jsonSavedData) {

                if (jsonSavedData == null) {
                    //AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found!")
                }
                else {
                    if (jsonSavedData.length > 0) {
                        savedJsonData = jsonSavedData
                    }
                }
            },
            error: function (e) {
            }
        });
        ///////////////////////////////////////////////////////////////////////////////////////////////
        if ((($("#hdnvActivityName").val().toUpperCase().match("GLOBAL"))) || (($("#hdnvActivityName").val().toUpperCase().match("RESPONCE")))) {
            data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
        }
        else if ((($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR")))) {
            data += '<div class="modal-dialog"  style="width: 90%; margin: 30px auto; height: 91% !important;" tabindex="-1">'
        }
        else if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            if ((!($("#hdnvSubActivityName").val().toUpperCase().match("NL"))) || (!($("#hdnvSubActivityName").val().toUpperCase().match("-NL")))) {
                data += '<div class="modal-dialog"  style="width: 62%; float:right;  margin: 30px auto; height: 91% !important;" tabindex="-1">'
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

        data += "<div class='modal-content set-margin' style='height:100% !important;'>";
        data += "<div class='modal-header'>";
        data += "<button type='button' class='btn btn-info btn-sm pull-right box-tools' data-widget='remove' data-dismiss='modal' data-toggle='tooltip' title='' data-original-title='Remove'><i class='fa fa-times'></i></button>";
        data += " <h4 class='modal-title'>" + $("#hdnvSubActivityName").val() + " DETAIL" + "</h4>";
        btnLesion += '<button style="text-align: right" type="button" id="' + "btn" + $("#hdnvSubActivityName").val().split(" ").join("") + '" class="btn btn-primary margin btn-default btn-sm" data-toggle="modal" data-target="#LesionModel,#MarkModel" title="' + $("#hdnvSubActivityName").val().toUpperCase() + " DETAIL" + '" data-tooltip="tooltip">' + $("#hdnvSubActivityName").val().toUpperCase() + '</button>';

        data += "</div>";
        if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            data += " <div class='modal-body' style='height:79% !important; overflow: auto !important; '>";
        }
        else {
            data += " <div class='modal-body' style='height:77% !important; overflow: auto !important; '>";
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

            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            }

            //FOR MARK ACTIVITY AND TARGET LESION
            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {
            }

            if (!($("#hdnvActivityName").val().toUpperCase().match("MARK"))) {

                if ($("#hdnvActivityName").val().toUpperCase().match("BL")) {
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

            if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                data += "<div class=col-sm-12>";
                value = jsonData[v].vMedExDesc;
                data += Label(id, value);
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
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control form-control dynamic-size-textbox-control " + jsonData[v].vMedExCode + " textbox_" + v + "";
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                        textValue = parseFloat(textValue).toFixed(2);
                        classVal2 += " dynamic-saved-size-textbox-control"
                    }
                    data += TextBoxWithCheckBox(type1, type2, classVal1, classVal2, id1, id2, placeHolder, tabIndex, textValue);
                }
                else if (vMedEx.length >= 3) {
                    if (vMedEx.length > 3) {
                        if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val().toUpperCase())) {
                            classValue = "DIAMETERS-" + vMedEx[3].toUpperCase() + " " + "dynamic-diameter-sum-control"
                            type = "text";
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                    classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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

            else if (jsonData[v].vMedExType.match("TextArea")) {
                if (jsonData[v].vMedExDesc.toUpperCase().match("SIZE")) {
                    var type1 = "checkbox";
                    var type2 = "text";
                    tabIndex = v;
                    var id1 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "checkbox" + v;
                    var id2 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "textbox" + v;
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control form-control dynamic-size-textbox-control " + jsonData[v].vMedExCode + " textbox_" + v + "";
                    placeHolder = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    var textValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        textValue = vMedExResult
                        textValue = parseFloat(textValue).toFixed(2);
                    }
                    data += TextBoxWithCheckBox(type1, type2, classVal1, classVal2, id1, id2, placeHolder, tabIndex, textValue);
                }

                else if (vMedEx.length >= 3) {
                    if (vMedEx.length > 3) {
                        if ((vMedEx[2].match("DIAMETER") || vMedEx[2].match("DIAMETERS")) && vMedEx[3].match($("#hdnSelectedvParentNodeDisplayName").val().toUpperCase())) {
                            classValue = "DIAMETERS-" + vMedEx[3].toUpperCase() + " " + "dynamic-diameter-sum-control"
                            type = "text";
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control STATISTICS-CONTROL " + classValue + " " + jsonData[v].vMedExCode;
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
                            classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                        classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
                    classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
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
            else if (jsonData[v].vMedExType.match("ComboBox")) {
                if (jsonData[v].vMedExDesc.toUpperCase().match("LESION")) {
                    var type1 = "checkbox";
                    tabIndex = v;
                    var id1 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "checkbox" + v;
                    var id2 = jsonData[v].nMedExWorkSpaceDtlNo + "_" + jsonData[v].vMedExGroupCode + "_" + jsonData[v].vMedExSubGroupCode + "_" + jsonData[v].vMedExCode + "_" + jsonData[v].RepetitionNo + "_" + "dropdown" + v;
                    var comboValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        comboValue = vMedExResult
                    }
                    classVal1 = "dynamic-size-checkbox-control " + jsonData[v].vMedExCode + " checkbox_" + v + "";
                    classVal2 = "dynamic-control form-control dynamic-size-dropdown-control " + jsonData[v].vMedExCode + " dropdown" + v + "";
                    option = jsonData[v].vMedExValues
                    forVal = jsonData[v].vMedExDesc;
                    tabIndex = v;
                    lesionType = $("#hdnSelectedvChildNodeDisplayName").val().split("-")[1];
                    data += DropDownWithCheckBox(type1, classVal1, classVal2, id1, id2, option, tabIndex, forVal, lesionType, comboValue);
                }
                else {
                    classVal = "dynamic-control form-control " + jsonData[v].vMedExCode;
                    tabIndex = v;
                    option = jsonData[v].vMedExValues
                    forVal = jsonData[v].vMedExDesc;
                    id += "_dropdown_" + v;
                    var comboValue = null;
                    if (vMedExResult != "" && vMedExResult != null) {
                        comboValue = vMedExResult
                    }
                    data += DropDown(classVal, id, option, tabIndex, forVal, comboValue);
                }

            }
            else if (jsonData[v].vMedExType.match("Radio")) {
                type = "radio";
                name = jsonData[v].vMedExDesc;
                classVal = "dynamic-control rGroup " + jsonData[v].vMedExCode;
                tabIndex = v;
                value = jsonData[v].vMedExCode;
                controlVal = jsonData[v].vMedExValues
                id += "_dropdown_" + v;
                data += Radio(type, classVal, id, name, tabIndex, value, controlVal);
            }
            //To Remove Div Space when get Label Heading
            if (!jsonData[v].vMedExType.toUpperCase().match("LABEL")) {
                data += "</div>"
                data += "</div>"
            }

        }
        //data += "<table id='tblLesionDetail' class='table table-bordered table-striped dataTable'> </table>";
        data += "</div>";
        data += "</div>";
        data += "<div class='modal-footer'>";
        data += "<button type='button' class='btn btn-default pull-left' data-dismiss='modal' id='btnClose'>Close</button>";
        data += "<button type='button' class='btn btn-default pull-left' id='btnLesionDetail' onclick='MILesionDetails()' data-toggle='modal' data-target='#MILesionModelData'>Lesion Detail</button>";
        var sizeData
        var clear

        if (!$("#hdnUserTypeName").val().toUpperCase().match("ADJUDICATOR")) {
            sizeData = "";
            clear = "";
            //sizeData = "<button type='button' class='btn btn-default pull-left' id='btnAddTLSize' onclick='MILesionAddSize()'>Add Size</button>";
            clear = "<button type='button' class='btn btn-default pull-left' id='btnClearLesionData' onclick='MIClearLesionData()'>Clear</button>";
            data += sizeData;
            data += clear;
            data += "<button type='button' class='btn btn-primary' id='btnMIFinalSaveLesion'>Save changes</button>";
            data += "<button type='button' class='btn btn-primary' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
            //data += "<button type='button' class='btn btn-primary' id='btnMIFinalSaveDicom' onclick='saveDicom()'>Save Dicom</button>";
        }
        else {
            if ($("#hdnvActivityName").val().toUpperCase().match("ADJUDICATOR") && $("#hdnvSubActivityName").val().toUpperCase().match("ADJUDICATOR")) {
                sizeData = "";
                clear = "";
                //sizeData = "<button type='button' class='btn btn-default pull-left' id='btnAddTLSize' onclick='MILesionAddSize()'>Add Size</button>";
                clear = "<button type='button' class='btn btn-default pull-left' id='btnClearLesionData' onclick='MIClearLesionData()'>Clear</button>";
                data += sizeData;
                data += clear;
                data += "<button type='button' class='btn btn-primary' id='btnMIFinalSaveLesion'>Save changes</button>";
                data += "<button type='button' class='btn btn-primary' id='btnMIFinalSubmitLesion' data-toggle='modal' data-target='#ModalMIeSignature'>Submit</button>";
            }
        }

        data += "</div>";
        data += "</div>";
        data += "</div>";
        $("#LesionModel").append(data);
        $("#divLesion").append(btnLesion);

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

        $('.dynamic-control').each(function (index, control) {
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
        debugger;
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
                        //async: false,
                        data: MILesionStatisticsDetails,
                        success: function (jsonData) {
                            if (jsonData != null) {
                                if (jsonData.length > 0) {
                                    for (var v = 0; v < jsonData.length; v++) {
                                        $('.dynamic-control').each(function (index, control) {
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
                                    $('.dynamic-control').each(function (index, control) {
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
                                $('.dynamic-control').each(function (index, control) {
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
            success: successSaveLessionDetails,
            error: errorSaveLessionDetails
        });

        function successSaveLessionDetails() {
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


//For Calculation of Statastics
function bindevent() {
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
                if (lineLength == "" || lineLength == '' || lineLength == null || lineLength == 'undefined') {
                    AlertBox("WARNING", " Dicom Viewer", "Please Make Measurement First!")
                    this.checked = false;
                }

                //Get Value By Its Parent
                //$(this).parent().parent().find("input[type='text']").val(lineLength)
                var length = lineLength;
                if (length != undefined) {
                    if (length.indexOf("mm") >= 0) {
                        length = length.replace("mm", "");
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

                $("#" + $(this).attr("refid")).val(length);
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
    debugger;
    $.confirm({
        title: 'Confirm!',
        icon: 'fa fa-warning',
        content: 'USER CONFIRMATION',
        onContentReady: function () {
            var self = this;
            this.setContentPrepend('<div>MI</div>');
            setTimeout(function () {
                self.setContentAppend('<div>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>Details?</div>');
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

                    var validation = true;

                    var dublicateChecker = []

                    //********************No Validation Required on Each dynamic Control. Do not Delete it. It is for Reference use.**************************//

                    //$('.dynamic-control').each(function (index, control) {

                    //    if (control.type == "text") {
                    //        if (control.value == "" || control.value == '' || control.value == null) {
                    //            AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                    //            validation = false;
                    //            return false;
                    //        }
                    //    }

                    //    else if (control.type.toUpperCase() == "TEXTAREA") {
                    //        if (control.value == "" || control.value == '' || control.value == null) {
                    //            AlertBox("WARNING", " Dicom Viewer", "Please Enter " + control.placeholder)
                    //            validation = false;
                    //            return false;
                    //        }
                    //    }

                    //    else if (control.type == "select-one") {
                    //        //if (control.selectedIndex == 0) {
                    //        if (control.selectedOptions[0].text == "" || control.selectedOptions[0].text == '' || control.selectedOptions[0].text == " " || control.selectedOptions[0].text == ' ') {
                    //            AlertBox("WARNING", " Dicom Viewer", "Please select " + control.name)
                    //            validation = false;
                    //            return false;
                    //        }
                    //        else {
                    //            if (control.name.toUpperCase().match("ORGAN")) {

                    //                dublicateChecker.push(control.selectedOptions[0].text)
                    //            }
                    //        }
                    //    }
                    //})
                    //********************No Validation Required on Each dynamic Control. Do not Delete it. It is for Reference use.**************************//


                    $('.dynamic-control').each(function (index, control) {

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

                    $('.dynamic-control').each(function (index, control) {
                        if (control.type == "select-one") {
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
                                AlertBox("WARNING", " Dicom Viewer", "Organ Details Found More Than Twise!")
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
                        validateStatisticsCriteria();
                    }

                }
            },
            danger: {
                btnClass: 'btn-red any-other-class',
                text: 'CANCEL (C)',
                keys: ['C'],
                action: function () {
                    AlertBox("WARNING", " Dicom Viewer", "Please Save Data Again For <b>" + $("#hdnSelectedvParentNodeDisplayName").val() + " For " + $("#hdnSelectedvChildNodeDisplayName").val() + "</b> And Try Again Later!")
                    //$.alert('Canceled!');
                }
            },
        }
    });
}

//Validation For Statistics Lesion Detail For NL
function validateStatisticsCriteria() {
    if ($("#hdnvSubActivityName").val().toUpperCase().match("NL") || $("#hdnvSubActivityName").val().toUpperCase().match("-NL")) {
        var TL = "";
        var NTL = "";
        var NL = "";
        var OVERALL = ""

        $('.dynamic-control').each(function (index, control) {
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
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>CR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>CR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "CR" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "PR" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PR" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NE" || NTL == "NOT EVALUATED" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PR</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "SD" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "SD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NE" || NTL == "NOT EVALUATED" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>SD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "NO")) {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "YES")) {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "PD" && (NL == "YES" || NL == "NO")) {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if ((TL == "NE" || TL == "NOT EVALUATED") && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
                            if (control.type == "text" || control.type == "TextArea") {
                                if ((control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "- OVERALL RESPONSE")) || (control.placeholder.toUpperCase() == ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALLRESPONSE"))) {
                                    OVERALL = control.value.toUpperCase()
                                    if (OVERALL == "" || OVERALL == null) {
                                        //AlertBox("WARNING", " Dicom Viewer", "Please Select <b>" + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + " </b>First !")
                                        //validation = false;
                                        //return false;                                                
                                    }
                                    else {
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                        if (OVERALL != "NE" || OVERALL != "NOT EVALUATED") {
                                            $.confirm({
                                                title: 'Confirm!',
                                                icon: 'fa fa-warning',
                                                content: 'USER CONFIRMATION',
                                                onContentReady: function () {
                                                    var self = this;
                                                    this.setContentPrepend('<div>MI</div>');
                                                    setTimeout(function () {
                                                        self.setContentAppend('<div>The Value Should be <b>NE/NOT EVALUATED</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                        //
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && (NL == "YES" || NL == "NO")) {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NE" || NTL == "NOT EVALUATED") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && (NTL == "NON CR/NON PD" || NTL == "NON CR / NON PD" || NTL == "NN") && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "CR" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "CR" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "PD" && NL == "NO") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                    }
                                }
                            }
                        })
                    }
                    else if (TL == "PD" && NTL == "PD" && NL == "YES") {
                        $('.dynamic-control').each(function (index, control) {
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
                                                        self.setContentAppend('<div>The Value Should be <b>PD</b> For ' + ($("#hdnSelectedvChildNodeDisplayName").val().split("-")[0].toUpperCase() + "-" + $("#hdnSelectedvParentNodeDisplayName").val().toUpperCase() + "-OVERALL RESPONSE") + '</br>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Details?</div>');
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
    }
    else {
        SaveMIFinalLessionData();
    }
}

//Save MI Final Lesion Detail Function
function SaveMIFinalLessionData() {

    //******This is For Fatching Result For All Dynamic Control Because All Field is Complasary in Validation So We Get Result in All Control*******//

    //$('.dynamic-control').get().forEach(function (control, index, array) {
    //    try {
    //        var LesionData = {
    //        }
    //        if (control.type == "text") {
    //            LesionData = {
    //                vMedExResult: control.value,
    //                vMedExCode: control.id.split('_')[3],
    //                vMedExDesc: control.placeholder,
    //                vMedExType: "TextBox"
    //            }
    //        }

    //        else if (control.type.toUpperCase() == "TEXTAREA") {
    //            LesionData = {
    //                vMedExResult: control.value,
    //                vMedExCode: control.id.split('_')[3],
    //                vMedExDesc: control.placeholder,
    //                vMedExType: "TextArea",
    //            }
    //        }

    //        else if (control.type == "select-one") {
    //            LesionData = {
    //                vMedExResult: control.selectedOptions[0].text,
    //                vMedExCode: control.id.split('_')[3],
    //                vMedExDesc: control.name,
    //                vMedExType: "ComboBox",
    //            }
    //        }
    //        else if (control.type == "radio") {
    //            LesionData = {
    //                vMedExResult: $('input[name="' + control.name + '"]:checked').val(),
    //                vMedExCode: control.name.split('_')[1],
    //                vMedExDesc: control.name.split('_')[0],
    //                vMedExType: "Radio",
    //            }
    //        }
    //        finalData.push(LesionData);

    //    }
    //    catch (e) {
    //        throw e;
    //    }
    //});
    //******This is For Fatching Result For All Dynamic Control Because All Field is Complasary in Validation So We Get Result in All Control*******//

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

    if (vWorkspaceId == "" || vWorkspaceId == '' || vWorkspaceId == null) {
        AlertBox("error", " Dicom Viewer", "Session Expired!")
        var url = $("#RedirectToLogin").val();
        location.href = url;
        return false;
    }
    $('.dynamic-control').get().forEach(function (control, index, array) {
        try {
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
            finalData.push(LesionData);

        }
        catch (e) {
            throw e;
        }
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
        url: ApiURL + "SetData/SaveMIFinalLession",
        type: "POST",
        //data: JSON.stringify(LesionFianlData),
        data: LesionFianlData,
        //async: false,
        //dataType: 'json',
        //contentType: 'application/json',
        crossDomain: true,
        cache: false,
        success: successSaveMILessionDetails,
        error: errorSaveMILessionDetails
    });

    function successSaveMILessionDetails(data) {
        if (data.length > 0) {
            if (data[0].Status == "1") {
                AlertBox("SUCCESS", " Dicom Viewer", "Lesion Data Saved Successfully!")
            }
            else {
                AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
            }
        }
        else {
            AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
        }

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

    function errorSaveMILessionDetails() {
        AlertBox("error", " Dicom Viewer", "Error While Saving Lesion Detail!")
    }
}

//To Save All Dicom Detail in Biznet, MI, And Dicom Physically
function SubmitMIFinalLesion() {
    debugger;

    $.confirm({
        title: 'Confirm!',
        icon: 'fa fa-warning',
        content: 'USER CONFIRMATION',
        onContentReady: function () {
            var self = this;
            this.setContentPrepend('<div>MI</div>');
            setTimeout(function () {
                self.setContentAppend('<div>Do You Want To Save <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' </b>Final All Dicom Details?</div>');
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

                        $.ajax({
                            url: ApiURL + "GetData/LesionDetailsDATA",
                            type: "POST",
                            data: lesiondata,
                            //timeout: 0,
                            //async: false,
                            success: function (jsonData) {
                                if (jsonData == null || jsonData == "" || jsonData == '') { // This is for Non Repeatation of data Also Work For Repetation                                            
                                    AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                                }
                                else {
                                    $.ajax({
                                        url: WebURL + "MIDicomViewer/ClearSession",
                                        type: 'POST',
                                        data: '',
                                        //timeout: 0,
                                        //async: false,
                                        success: function (msg) {
                                            if (msg == "success") {
                                                $.ajax({
                                                    url: WebURL + "MIDicomViewer/SkipMIFinalLesion",
                                                    type: "POST",
                                                    //  timeout: 0,
                                                    data: data,
                                                    //async: false,
                                                    success: function (data) {
                                                        if (data == "SessionExpired" || data == "RecordsNotFound" || data == "ImageNotSavedProperlyToPhysicalPath" || data == "error") {
                                                            AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Details!")
                                                        }
                                                        else if (data == "success") {

                                                            $.confirm({
                                                                title: 'Confirm!',
                                                                icon: 'fa fa-warning',
                                                                content: 'SUCCESS',
                                                                onContentReady: function () {
                                                                    var self = this;
                                                                    this.setContentPrepend('<div>MI</div>');
                                                                    setTimeout(function () {
                                                                        self.setContentAppend('<div>Dicom Detail Saved Successfully For <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>.</div>');
                                                                    }, 1000);
                                                                },
                                                                columnClass: 'medium',
                                                                //animation: 'zoom',
                                                                //closeIcon: true,
                                                                //closeIconClass: 'fa fa-close',
                                                                //columnClass: 'small',
                                                                boxWidth: '30%',
                                                                autoClose: 'info|15000',
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
                                                                            AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                                                                            window.close()
                                                                        }
                                                                    },
                                                                }
                                                            });

                                                            //AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                                                            //alert("Dicom Detail Saved Successfully!")
                                                            //window.close()
                                                        }
                                                        else {
                                                            AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Details!")
                                                        }
                                                    },
                                                    error: function () {
                                                        AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Details!")
                                                    }
                                                });
                                            }
                                            else {
                                                AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Details!")
                                            }
                                        },
                                        error: function (msg) {
                                            AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Detail!")
                                        }
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

                        //*******************************For Routine Entry*******************************//
                    else {
                        setTimeout(function () {

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                            //**************************************************Logic To Save Routine Entry****************************************************//

                            //var bool_LesionDetailsDATA = false;
                            //var bool_ClearSession = false;
                            //var bool_SubmitMIFinalLesion = false;

                            //$.ajax({
                            //    url: ApiURL + "GetData/LesionDetailsDATA",
                            //    type: "POST",
                            //    data: lesiondata,
                            //    async: false,
                            //    success: function (jsonData) {
                            //        if (jsonData == null || jsonData == "" || jsonData == '') { // This is for Non Repeatation of data Also Work For Repetation                              
                            //            AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                            //        }
                            //        else {
                            //            bool_LesionDetailsDATA = true;
                            //        }
                            //    },
                            //    failure: function (response) {
                            //        AlertBox("error", " Dicom Viewer", "Error While Retriving Lesion Detail Data. Error : " + response.d)
                            //    },
                            //    error: function (response) {
                            //        AlertBox("error", " Dicom Viewer", "Error While Retriving Lesion Detail Data. Error : " + response.d)
                            //    }
                            //});

                            //if (bool_LesionDetailsDATA == true) {
                            //    $.ajax({
                            //        url: WebURL + "MIDicomViewer/ClearSession",
                            //        type: 'POST',
                            //        data: '',
                            //        async: false,
                            //        success: function (msg) {
                            //            if (msg == "success") {
                            //                bool_ClearSession = true;
                            //            }
                            //            else {
                            //                AlertBox("error", " Dicom Viewer", "Error While Clearing Session Data!")
                            //            }
                            //        },
                            //        error: function (msg) {
                            //            AlertBox("error", " Dicom Viewer", "Error While Clearing Session Data!")
                            //        }
                            //    });
                            //}

                            //if (bool_ClearSession == true) {
                            //    var filename = 'dicom';
                            //    cornerstoneTools.saveAs(element, filename);
                            //}

                            //if (MIDicomSave == true) {
                            //    $.ajax({
                            //        url: WebURL + "MIDicomViewer/SubmitMIFinalLesion",
                            //        type: "POST",
                            //        data: data,
                            //        //async: false,
                            //        success: function (SubmitMIFinalLesionData) {
                            //            if (SubmitMIFinalLesionData == "success") {
                            //                bool_SubmitMIFinalLesion = true;
                            //                $.confirm({
                            //                    title: 'Confirm!',
                            //                    icon: 'fa fa-warning',
                            //                    content: 'SUCCESS',
                            //                    onContentReady: function () {
                            //                        var self = this;
                            //                        this.setContentPrepend('<div>MI</div>');
                            //                        setTimeout(function () {
                            //                            self.setContentAppend('<div>Dicom Detail Saved Successfully For <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>.</div>');
                            //                        }, 1000);
                            //                    },
                            //                    columnClass: 'medium',
                            //                    //animation: 'zoom',
                            //                    //closeIcon: true,
                            //                    //closeIconClass: 'fa fa-close',
                            //                    //columnClass: 'small',
                            //                    boxWidth: '30%',
                            //                    autoClose: 'info|15000',
                            //                    animation: 'news',
                            //                    closeAnimation: 'news',
                            //                    closeAnimation: 'scale',
                            //                    backgroundDismissAnimation: 'random',
                            //                    //backgroundDismissAnimation: 'glow',
                            //                    type: 'blue',
                            //                    theme: 'dark',
                            //                    draggable: true,
                            //                    buttons: {
                            //                        info: {
                            //                            btnClass: 'btn-blue',
                            //                            text: 'OK (O)',
                            //                            keys: ['O'],
                            //                            action: function () {
                            //                                AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                            //                                window.close()

                            //                            }
                            //                        },
                            //                    }
                            //                });
                            //            }
                            //            else {

                            //                $.ajax({
                            //                    url: WebURL + "MIDicomViewer/MIManageImage",
                            //                    type: "GET",
                            //                    data: "",
                            //                    async: false,
                            //                    success: function (data) {
                            //                        if (data == "success") {
                            //                            //alert("2");
                            //                            if (SubmitMIFinalLesionData == "SessionExpired" || SubmitMIFinalLesionData == "RecordsNotFound" || SubmitMIFinalLesionData == "ImageNotSavedProperlyToPhysicalPath" || SubmitMIFinalLesionData == "error") {
                            //                                AlertBox("error", " Dicom Viewer", "Dicom Detail RollBack Successfully!" + SubmitMIFinalLesionData)
                            //                            }
                            //                            else {
                            //                                AlertBox("error", " Dicom Viewer", "Dicom Detail RollBack Successfully!")
                            //                            }

                            //                        }
                            //                        else {
                            //                            AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //                        }
                            //                    },
                            //                    error: function (e) {
                            //                        AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //                    }
                            //                });
                            //            }
                            //        },
                            //        error: function () {
                            //            $.ajax({
                            //                url: WebURL + "MIDicomViewer/MIManageImage",
                            //                type: "GET",
                            //                data: "",
                            //                async: false,
                            //                success: function (data) {
                            //                    if (data == "success") {
                            //                        //alert("4");
                            //                        AlertBox("error", " Dicom Viewer", "Dicom Detail RollBack Successfully!")
                            //                    }
                            //                    else {
                            //                        AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //                    }
                            //                },
                            //                error: function (e) {
                            //                    AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //                }
                            //            });
                            //        }
                            //    });
                            //}
                            //else {
                            //    $.ajax({
                            //        url: WebURL + "MIDicomViewer/MIManageImage",
                            //        type: "GET",
                            //        data: "",
                            //        async: false,
                            //        success: function (data) {
                            //            if (data == "success") {                                            
                            //                if (MISessionExpired == true) {
                            //                    AlertBox("error", " Dicom Viewer", "Dicom Detail RollBack Successfully! Session Expired")
                            //                    var url = $("#RedirectToLogin").val();
                            //                    location.href = url;
                            //                }
                            //                else {
                            //                    AlertBox("error", " Dicom Viewer", "Dicom Detail RollBack Successfully!")
                            //                }
                            //            }
                            //            else {
                            //                AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //            }
                            //        },
                            //        error: function (e) {
                            //            AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                            //        }
                            //    });
                            //}

                            //**************************************************Logic To Save Routine Entry****************************************************//
                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                            debugger;
                            //saveDicom();


                            $.ajax({
                                url: ApiURL + "GetData/LesionDetailsDATA",
                                type: "POST",
                                data: lesiondata,
                                //timeout: 0,
                                //async: false,
                                success: function (jsonData) {
                                    if (jsonData == null || jsonData == "" || jsonData == '') { // This is for Non Repeatation of data Also Work For Repetation                              
                                        AlertBox("WARNING", " Dicom Viewer", "No Lesion Detail Found! Please Save Lesion Data Before Submiting It!")
                                    }
                                    else {
                                        $.ajax({
                                            url: WebURL + "MIDicomViewer/ClearSession",
                                            type: 'POST',
                                            data: '',
                                            async: false,
                                            //timeout: 0,
                                            success: function (msg) {
                                                if (msg == "success") {
                                                    var filename = 'dicom'
                                                    cornerstoneTools.saveAs(element, filename);
                                                    if (MISessionExpired == true) {
                                                        $.ajax({
                                                            url: WebURL + "MIDicomViewer/MIManageImage",
                                                            type: "GET",
                                                            data: "",
                                                            async: false,
                                                            //timeout: 0,
                                                            success: function (data) {
                                                                if (data == "success") {
                                                                    //alert("1");
                                                                    AlertBox("error", " Dicom Viewer", "Dicom Details RollBacked! Session is Expired! <br/> Please Login Again And Submit Data!")
                                                                    var url = $("#RedirectToLogin").val();
                                                                    location.href = url;
                                                                }
                                                                else {
                                                                    AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                                }
                                                            },
                                                            error: function (e) {
                                                                AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                            }
                                                        });
                                                    }
                                                    else if (MIDicomSave == true) {
                                                        $.ajax({
                                                            url: WebURL + "MIDicomViewer/SubmitMIFinalLesion",
                                                            type: "POST",
                                                            data: data,
                                                            //timeout: 0,
                                                            async: false,
                                                            success: function (data) {
                                                                if (data == "SessionExpired" || data == "RecordsNotFound" || data == "ImageNotSavedProperlyToPhysicalPath" || data == "error") {
                                                                    var dataresult = data;
                                                                    $.ajax({
                                                                        url: WebURL + "MIDicomViewer/MIManageImage",
                                                                        type: "GET",
                                                                        data: "",
                                                                        //timeout: 0,
                                                                        async: false,
                                                                        success: function (data) {
                                                                            if (data == "success") {
                                                                                //alert("2");
                                                                                AlertBox("error", " Dicom Viewer", "Dicom Detail RollBacked! " + dataresult + "<br/> Please Submit Data Again!")
                                                                            }
                                                                            else {
                                                                                AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image! " + dataresult)
                                                                            }
                                                                        },
                                                                        error: function (e) {
                                                                            AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image! " + dataresult)
                                                                        }
                                                                    });
                                                                }
                                                                else if (data == "success") {
                                                                    $.confirm({
                                                                        title: 'Confirm!',
                                                                        icon: 'fa fa-warning',
                                                                        content: 'SUCCESS',
                                                                        onContentReady: function () {
                                                                            var self = this;
                                                                            this.setContentPrepend('<div>MI</div>');
                                                                            setTimeout(function () {
                                                                                self.setContentAppend('<div>Dicom Detail Saved Successfully For <b>' + $("#hdnvActivityName").val().toUpperCase() + ' ' + $("#hdnvSubActivityName").val().toUpperCase() + ' <b>.</div>');
                                                                            }, 1000);
                                                                        },
                                                                        columnClass: 'medium',
                                                                        //animation: 'zoom',
                                                                        //closeIcon: true,
                                                                        //closeIconClass: 'fa fa-close',
                                                                        //columnClass: 'small',
                                                                        boxWidth: '30%',
                                                                        autoClose: 'info|15000',
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
                                                                                    AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                                                                                    window.close()

                                                                                }
                                                                            },
                                                                        }
                                                                    });
                                                                    //AlertBox("success", " Dicom Viewer", "Dicom Detail Saved Successfully!")
                                                                    //alert("Dicom Detail Saved Successfully!")
                                                                    //window.close()
                                                                }
                                                                else {
                                                                    $.ajax({
                                                                        url: WebURL + "MIDicomViewer/MIManageImage",
                                                                        type: "GET",
                                                                        data: "",
                                                                        //timeout: 0,
                                                                        async: false,
                                                                        success: function (data) {
                                                                            if (data == "success") {
                                                                                //alert("3");
                                                                                AlertBox("error", " Dicom Viewer", "Dicom Detail RollBacked! <br/> Please Submit Data Again!")
                                                                            }
                                                                            else {
                                                                                AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                                            }
                                                                        },
                                                                        error: function (e) {
                                                                            AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                                        }
                                                                    });
                                                                }
                                                            },
                                                            error: function () {
                                                                $.ajax({
                                                                    url: WebURL + "MIDicomViewer/MIManageImage",
                                                                    type: "GET",
                                                                    data: "",
                                                                    //timeout: 0,
                                                                    async: false,
                                                                    success: function (data) {
                                                                        if (data == "success") {
                                                                            //alert("4");
                                                                            AlertBox("error", " Dicom Viewer", "Dicom Detail RollBacked! <br/> Please Submit Data Again!")
                                                                        }
                                                                        else {
                                                                            AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                                        }
                                                                    },
                                                                    error: function (e) {
                                                                        AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image!")
                                                                    }
                                                                });
                                                            }
                                                        });
                                                    }
                                                    else {
                                                        $.ajax({
                                                            url: WebURL + "MIDicomViewer/MIManageImage",
                                                            type: "GET",
                                                            data: "",
                                                            //timeout: 0,
                                                            async: false,
                                                            success: function (data) {
                                                                if (data == "success") {
                                                                    //alert("5");
                                                                    AlertBox("error", " Dicom Viewer", "Dicom Detail RollBacked! Error While Saving Dicom Images! <br/> Please Submit Data Again!")
                                                                }
                                                                else {
                                                                    AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image! Error While Saving Dicom Images!")
                                                                }
                                                            },
                                                            error: function (e) {
                                                                AlertBox("error", " Dicom Viewer", "Error While RollBack Dicom Image! Error While Saving Dicom Images!")
                                                            }
                                                        });
                                                    }
                                                }
                                            },
                                            error: function (msg) {
                                                AlertBox("error", " Dicom Viewer", "Error While Saving Dicom Detail!")
                                            }
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

                            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        }, 0);
                    }
                    //$.alert('confirm!');
                }
            },
            danger: {
                btnClass: 'btn-red any-other-class',
                text: 'CANCEL (C)',
                keys: ['C'],
                action: function () {
                    AlertBox("WARNING", " Dicom Viewer", "Please Save Data Again For <b>" + $("#hdnSelectedvParentNodeDisplayName").val() + " For " + $("#hdnSelectedvChildNodeDisplayName").val() + "</b> And Try Again Later!")
                }
            },
        }
    });

}

//To View Data In DataTable that are saved for lesion
function getLesionDetails() {
    debugger;
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
    debugger;
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
                            $('.dynamic-control').get().forEach(function (control, index, array) {
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

                        $('#LesionModel').on('shown.bs.modal', function () {
                            $('input:text:visible:first', this).focus();
                        })

                        //*******************************This Logic is For To View CRF Table Temp Data in GridView*******************************//
                        //******************************************************Do Not Delete IT*************************************************//


                        //var modalData = document.getElementById('MILesionModelData')
                        //while (modalData.hasChildNodes()) {
                        //    modalData.removeChild(modalData.lastChild);
                        //}

                        //table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)
                        ////var MiLesionDetailModel = $('<div class="modal-dialog">').append($('<div class="modal-content">')).append($('<div class="modal-header">'));

                        //var MiLesionDetailModel = '';
                        ////var table = Table('tblMIDetail', 'tblMIDetail', 'table table-bordered table-striped dataTable', jsonData)

                        //MiLesionDetailModel += '<div class="modal-dialog detail-dialog">';
                        //MiLesionDetailModel += '<div class="modal-content" >';
                        //MiLesionDetailModel += '<div class="modal-header">';
                        //MiLesionDetailModel += '<button type="button" class="btn btn-info btn-sm pull-right box-tools" data-widget="remove" data-dismiss="modal" data-toggle="tooltip" title="" data-original-title="Remove">';
                        //MiLesionDetailModel += '<i class="fa fa-times"></i>';
                        //MiLesionDetailModel += '</button>';
                        //MiLesionDetailModel += "<h4 class='modal-title'>" + $("#hdnvSubActivityName").val() + " DETAIL" + "</h4>";
                        //MiLesionDetailModel += '</div>';
                        //MiLesionDetailModel += '<div class="modal-body" id="MILesionModelBodyData" >';
                        //MiLesionDetailModel += table;
                        //MiLesionDetailModel += '</div>';
                        //MiLesionDetailModel += '<div class="modal-footer">';
                        //MiLesionDetailModel += '<button type="button" class="btn btn-default pull-left" data-dismiss="modal" id="btnMILesionModelDataClose">Close</button>';
                        //MiLesionDetailModel += '</div>';
                        //MiLesionDetailModel += '</div>';
                        //MiLesionDetailModel += '</div>';

                        //$("#MILesionModelData").append(MiLesionDetailModel);
                        ////$("#MILesionModelBodyData").append(table);
                        ////TableToDataTable('tblMIDetail')

                        //$(tblMIDetail).dataTable({
                        //    "bJQueryUI": true,
                        //    //"sPaginationType": "full_numbers",
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


                        //*******************************This Logic is For To View CRF Table Temp Data in GridView*******************************//
                        //******************************************************Do Not Delete IT*************************************************//

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
                                    $('.dynamic-control').each(function (index, control) {
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
                                $('.dynamic-control').each(function (index, control) {
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
                            $('.dynamic-control').each(function (index, control) {
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
    debugger;
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

//To Clear Lesion Data for TL and NTL
function MIClearLesionData() {
    debugger;
    //var sizecontrol = document.getElementsByClassName('dynamic-control');
    //if (sizecontrol.length != 0) {
    //    for (var v = 0 ; v < sizecontrol.length ; v++) {
    //        if (sizecontrol[v].value != '' || sizecontrol[v].value != "" || sizecontrol[v].value != null) {
    //            sizecontrol[v].value = "";
    //            return true;
    //        }
    //    }
    //}

    //$('.dynamic-control').each(function (index, control) {
    //    if (control.length != 0) {
    //        if (control.type == "checkbox") {
    //            control.checked = false;                
    //        }
    //    }
    //})    
    $('.dynamic-control').get().forEach(function (control, index, array) {
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
    debugger;
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
        //async: false,
        data: NLDetails,
        success: function (NLJsonData) {
            if (NLJsonData != null) {
                if (NLJsonData.length > 0) {
                    $("#btnMIFinalSaveLesion").show();
                    $("#btnMIFinalSubmitLesion").show();
                    for (var v = 0; v < NLJsonData.length; v++) {
                        $('.dynamic-control').each(function (index, control) {
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
                    $("#btnMIFinalSaveLesion").hide();
                    $("#btnMIFinalSubmitLesion").hide();
                    AlertBox("WARNING", " Dicom Viewer", "No New Lesion Details Found for Previous Visit! <br/> Please Fill The TL OR NTL Data For " + $("#hdnSelectedvParentNodeDisplayName").val())
                }
            }
            else {
                $("#btnMIFinalSaveLesion").hide();
                $("#btnMIFinalSubmitLesion").hide();
                AlertBox("WARNING", " Dicom Viewer", "No New Lesion Details Found for Previous Visit! <br/> Please Fill The TL OR NTL Data For " + $("#hdnSelectedvParentNodeDisplayName").val())
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

