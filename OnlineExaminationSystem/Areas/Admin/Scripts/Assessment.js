function Edit(Id) {
    $.ajax({
        url: "/Assessment/GetAssessment/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#Description').val(result.Description);
            $('#Duration').val(result.Duration);
            $('#VaccancyID').val(result.VaccancyID);
            $('#NumberOfQuestions').val(result.NumberOfQuestions);
            $('#StartDateTime').val(result.StartDateTime);
            $('#EndDateTime').val(result.EndDateTime);
        },
        error: function (errormessage) {
            var message = errormessage.responseText;
            var nIcons = "fa fa-times";
            var nType = "inverse"
            var nAnimIn = "animated bounceInLeft";
            var nAnimOut = "animated bounceOutLeft";
            notify(message, nIcons, nType, nAnimIn, nAnimOut);
        }
    });
}

function Prompt(ID) {
    swal({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/Assessment/DeleteAssessment/" + ID,
                    type: "Post",
                    contentType: "application/json;charset=UTF-8",
                    dataType: "json",
                    success: function () {
                        swal("Deleted!", "Data deleted.", "success");
                        window.location.reload(true);
                    },
                    error: function () {
                        swal("Cancel!", "Data not deleted.", "error");
                    }
                })
            } else {
                swal("Cancelled", "Your imaginary file is safe :)", "error");
            }
        });
}
