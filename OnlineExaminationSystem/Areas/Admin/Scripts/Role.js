function Edit(Id) {
    $.ajax({
        url: "/User/GetRole/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#Description').val(result.Description);
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
                    url: "/User/DeleteRole/" + ID,
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
