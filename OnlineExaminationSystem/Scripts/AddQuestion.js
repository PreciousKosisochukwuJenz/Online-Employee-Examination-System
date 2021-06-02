$(function () {

    $('input:radio[name="QuestionType"]').change(
        function () {
            if (this.checked && this.value == 'Multi_Choice') {
                $("#OptionDIV").slideDown();
                $("#CorrectAnswerDIV").slideDown();
            } else if (this.checked && this.value == 'Fill_in_the_blank') {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideDown();
            } else {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideUp();
            }
        });

    var choice = $("input:radio[name='QuestionType']");
    $.each(choice,
        function (i, item) {
            if (item.checked && item.id == 'Multi_Choice') {
                $("#OptionDIV").slideDown();
                $("#CorrectAnswerDIV").slideDown();
            }
            else if (item.checked && item.id == 'Fill_in_the_blank') {
                $("#OptionDIV").slideUp();
                $("#CorrectAnswerDIV").slideDown();
            }
        });

});
function Prompt(ID) {
    swal({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Yes, delete it!",
        cancelButtonText: "No, cancel!",
        closeOnConfirm: false,
        closeOnCancel: false
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/QuestionBank/DeleteQuestion/" + ID,
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
                swal("Cancelled", "Operation cancelled", "error");
            }
        });
}
