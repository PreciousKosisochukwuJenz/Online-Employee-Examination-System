'use strict';
$(document).ready(function () {
    let form = $("#basic-forms");

    form.steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slideLeft",
        onStepChanging: function (event, currentIndex, newIndex) {
            var progressID = currentIndex + 1;
            // Allways allow previous action even if the current form is not valid!
            if (currentIndex > newIndex) {
                return true;
            }
            // Needed in some cases if the user went back (clean up)
            if (currentIndex < newIndex) {
                // To remove error styles
                form.find(".body:eq(" + newIndex + ") label.error").remove();
                form.find(".body:eq(" + newIndex + ") .error").removeClass("error");
            }
            form.validate().settings.ignore = ":disabled,:hidden";
            var state =  form.valid();
            SaveProgress(progressID);
            return state;
        },
        onStepChanged: function (event, currentIndex, priorIndex) {

        },
        onFinished: function (event, currentIndex) {
            swal({
                title: 'Confirmation',
                text: "Are you sure, you want to submit?",
                type: 'warning',
                showCancelButton: true,
                confirmButtonClass: "btn-danger",
                confirmButtonText: "Yes",
                cancelButtonText: "No",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        Submit();
                    } else {
                        swal("Cancelled", "", "error");
                    }
                });

        }
    })


    form.validate({
        errorClass: 'help-block animation-slideDown',
        errorElement: 'p',
        showErrors: function (errorMap, errorList) {
            $(".p-error").html($.map(errorList, function (el) {
                return "Anwser required";
            }).join(", "));
        },
        wrapper: "p",
        submitHandler: function (form) {
        },
    });
});

