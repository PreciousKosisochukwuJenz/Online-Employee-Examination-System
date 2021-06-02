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
                    url: "/Applicants/DeleteApplicant/" + ID,
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
