
function setupButtons(studyViewer) {
    // Get the button elements
    var buttons = $(studyViewer).find('button');

    // Tool button event handlers that set the new active tool

    // WW/WL
    $(buttons[0]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.wwwc.activate(element, 1);
            cornerstoneTools.wwwcTouchDrag.activate(element);
        });
    });

    // Invert
    $(buttons[1]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            var viewport = cornerstone.getViewport(element);
            // Toggle invert
            if (viewport.invert === true) {
                viewport.invert = false;
            } else {
                viewport.invert = true;
            }
            cornerstone.setViewport(element, viewport);
        });
    });

    // Zoom
    $(buttons[2]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.zoom.activate(element, 5); // 5 is right mouse button and left mouse button
            cornerstoneTools.zoomTouchDrag.activate(element);
        });
    });

    // Pan
    $(buttons[3]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.pan.activate(element, 3); // 3 is middle mouse button and left mouse button
            cornerstoneTools.panTouchDrag.activate(element);
        });
    });

    // Stack scroll
    $(buttons[4]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.stackScroll.activate(element, 1);
            cornerstoneTools.stackScrollTouchDrag.activate(element);
        });
    });

    // Length measurement
    $(buttons[5]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.length.activate(element, 1);
        });
    });

    // Angle measurement
    $(buttons[6]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            //cornerstoneTools.angle.activate(element, 1);
            cornerstoneTools.simpleAngle.activate(element, 1);
        });
    });

    // Pixel probe
    $(buttons[7]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.probe.activate(element, 1);
        });
    });

    // Elliptical ROI
    $(buttons[8]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function(element) {
            cornerstoneTools.ellipticalRoi.activate(element, 1);
        });
    });

    // Rectangle ROI
    $(buttons[9]).on('click touchstart', function() {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.rectangleRoi.activate(element, 1);
        });
    });

    // Play clip
    $(buttons[10]).on('click touchstart', function() {
        forEachViewport(function(element) {
          var stackState = cornerstoneTools.getToolState(element, 'stack');
          var frameRate = stackState.data[0].frameRate;
          // Play at a default 10 FPS if the framerate is not specified
          if (frameRate === undefined) {
            frameRate = 10;
          }
          cornerstoneTools.playClip(element, frameRate);
        });
    });

    // Stop clip
    $(buttons[11]).on('click touchstart', function() {
        forEachViewport(function(element) {
            cornerstoneTools.stopClip(element);
        });
    });

    // Rotation
    $(buttons[13]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
                cornerstoneTools.rotate.activate(element, 1);
                cornerstoneTools.rotateTouchDrag.activate(element); 
        });
    });

    // Arrow Annotation
    $(buttons[14]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.arrowAnnotate.activate(element, 1);
            cornerstoneTools.arrowAnnotateTouch.activate(element);
        });
    });

    //Reset Tools
    $(buttons[15]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.wwwc.activate(element, 1);            
        });
    });

    //Clear Tools
    $(buttons[16]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.wwwc.activate(element, 1);
            var toolStateManager = cornerstoneTools.globalImageIdSpecificToolStateManager;          
            toolStateManager.clear(element)
            cornerstone.updateImage(element);
        });
    });

    //Reset View
    $(buttons[17]).on('click touchstart', function () {
        //disableAllTools();
        forEachViewport(function (element) {
         
            //var element = $('#viewport')
            var canvas = $('#viewport canvas')
            var enabledElement = cornerstone.getEnabledElement(element);
            var viewport = cornerstone.getDefaultViewport(canvas, enabledElement.image)
            cornerstone.setViewport(element, viewport);
            //cornerstone.updateImage(element);
            fitToWindow(element)
            resizeMain()
            resizeStudyViewer();
        });
    });


    //Interpolation
    $(buttons[18]).on('click touchstart', function () {
        //disableAllTools();
        forEachViewport(function (element) {
            var viewport = cornerstone.getViewport(element);
            if (viewport.pixelReplication === true) {
                viewport.pixelReplication = false;
            } 
            else {
                viewport.pixelReplication = true;
                }
            cornerstone.updateImage(element);
            //cornerstone.setViewport(element, viewport);
            });       
    });

    //Angle Measurement
    $(buttons[19]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.angle.activate(element, 1);
            //cornerstoneTools.simpleAngle.activate(element, 1);
        });
    });

    //FreeForm ROI
    $(buttons[20]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.freehand.activate(element, 1);
        });
    });

    //HighLight
    $(buttons[21]).on('click touchstart', function () {
        disableAllTools();
        forEachViewport(function (element) {
            cornerstoneTools.highlight.activate(element, 1);
        });
    });

    //FullScreen
    $(buttons[22]).on('click touchstart', function (e) {
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
    });

    $(document).on("webkitfullscreenchange mozfullscreenchange fullscreenchange", function () {
        if (!document.fullscreenElement && !document.mozFullScreenElement &&
            !document.webkitFullscreenElement && !document.msFullscreenElement) {
            $(container).width(256);
            $(container).height(256);
            $(element).width(256);
            $(element).height(256);
        } else {
            $(container).width($(window).width());
            $(container).height($(window).height());
            $(element).width($(container).width());
            $(element).height($(container).height());
        }
        cornerstone.resize(element, true);
    })

    $(window).on("resize orientationchange", function () {
        if (document.fullscreenElement || document.mozFullScreenElement ||
            document.webkitFullscreenElement || document.msFullscreenElement) {
            $(container).width($(window).width());
            $(container).height($(window).height());
            $(element).width($(container).width());
            $(element).height($(container).height());
            cornerstone.resize(element, true);
        }
    })

    // Tooltips
    $(buttons[0]).tooltip();
    $(buttons[1]).tooltip();
    $(buttons[2]).tooltip();
    $(buttons[3]).tooltip();
    $(buttons[4]).tooltip();
    $(buttons[5]).tooltip();
    $(buttons[6]).tooltip();
    $(buttons[7]).tooltip();
    $(buttons[8]).tooltip();
    $(buttons[9]).tooltip();
    $(buttons[10]).tooltip();
    $(buttons[11]).tooltip();
    $(buttons[12]).tooltip();
    $(buttons[13]).tooltip();
    $(buttons[14]).tooltip();
    $(buttons[15]).tooltip();
    $(buttons[16]).tooltip();
    $(buttons[17]).tooltip();
    $(buttons[18]).tooltip();
    $(buttons[19]).tooltip();
    $(buttons[20]).tooltip();
    $(buttons[21]).tooltip();

};