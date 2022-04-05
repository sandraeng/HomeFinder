// To add save/like funtionality just include this script at bottom of page.
// Binds ajax post event to all buttons called btnSaveLike. Button must be part of a form.
//
// After page is loaded, bind to buttons on document.
$(document).ready(function () {
    bindSaveLike();
});
function bindSaveLike() {
    $("[id*=btnSaveLike]").on("click", function (e) {
        e.preventDefault();
        saveLike(e);
    });
}

function saveLike(clickEvent) {
    form = $(clickEvent.target).parents('form').eq(0);

    relevantData = {
        Id: $(clickEvent.target).siblings("#Id").eq(0).val(),
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val(),
    };

    $.ajax({
        type: "POST",
        url: form.attr('action'),
        data: relevantData,
        dataType: "json",
        success: function (response) {
            alert("Saved to your liked objects!");
        }
    });
}