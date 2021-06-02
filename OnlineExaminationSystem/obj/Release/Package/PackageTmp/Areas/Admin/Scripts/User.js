function Edit(Id) {
    $.ajax({
        url: "/User/GetUser/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#Firstname').val(result.Firstname);
            $('#Lastname').val(result.Lastname);
            $('#Username').val(result.Username);
            $('#Email').val(result.Email);
            $('#PhoneNumber').val(result.PhoneNumber);
            $('#Address').val(result.Address);
            $('#RoleID').val(result.RoleID);
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
        //confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel plx!",
        confirmButtonClass: "btn-success",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/User/DeleteUser/" + ID,
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
