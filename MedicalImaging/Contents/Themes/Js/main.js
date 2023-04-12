(function (b) {
    "use strict";
    b(document).ready(function () {
        var a = b("#loginForm");
        a.length && a.validate({
            rules: {
                txtUserName: "required", loginPassword: "required"
            }
            , errorPlacement: function (a, b) {
                return !0
            }
        }
        );
        a = b("#signupForm");
        a.length && a.validate({
            rules: {
                singupName: "required", singupUsername: "required", singupEmail: {
                    required: !0, email: !0
                }
                , singupPassword: "required", singupPasswordAgain: {
                    equalTo: "#singupPassword"
                }
            }
            , errorPlacement: function (a, b) {
                return !0
            }
        }
        );
        a = b("#forgotForm");
        a.length && a.validate({
            rules: {
                forgotEmail: {
                    required: !0, email: !0
                }
            }
            , errorPlacement: function (a, b) {
                return !0
            }
        }
        );
        a = b("#subscribeForm");
        a.length && a.validate({
            rules: {
                subscribeEmail: {
                    required: !0, email: !0
                }
            }
            , errorPlacement: function (a, b) {
                return !0
            }
        }
        );
        a = b("#contactForm");
        a.length && a.validate({
            rules: {
                contactName: "required", contactEmail: {
                    required: !0, email: !0
                }
                , contactSubject: "required", contactMessage: "required"
            }
            , errorPlacement: function (a, b) {
                return !0
            }
        }
        );
        b("[data-img-src]").each(function () {
            var a = b(this).data("img-src");
            b(this).css("background-image", "url(" + a + ")")
        }
        )



        b(document).on('click', '.close,.btnbacks', function () {
            b('.txtUserName').removeClass('error');
            b('.loginPassword').removeClass('error');
            b('.Profile').removeClass('error');
            b('.forgotEmail').removeClass('error');

        });


        b('#loginFormModal').bind('DOMSubtreeModified', function (e) {
            var cls = $(this).hasClass("in")
            //console.log(cls);
            if (cls == false) {

            }

        });





        // b('.forgotEmail').keyup(function () {
        // if (this.value != this.value.replace(/[^0-9\.]/g, '')) {
        // this.value = this.value.replace(/[^0-9\.]/g, '');
        // }
        // });



        b(".forgotEmail").keypress(function (event) {
            // Backspace, tab, enter, end, home, left, right
            // We don't support the del key in Opera because del == . == 46.
            var controlKeys = [8, 9, 13, 35, 36, 37, 39];
            // IE doesn't support indexOf
            var isControlKey = controlKeys.join(",").match(new RegExp(event.which));
            // Some browsers just don't raise events for control keys. Easy.
            // e.g. Safari backspace.
            if (!event.which || // Control keys in most browsers. e.g. Firefox tab is 0
                (49 <= event.which && event.which <= 57) || // Always 1 through 9
                (48 == event.which && $(this).attr("value")) || // No 0 first digit
                isControlKey) { // Opera assigns values for control keys.
                return;
            } else {
                event.preventDefault();
            }
        });




    }
    )
}

)(jQuery);