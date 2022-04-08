// To add save/like funtionality just include this script at bottom of page.
// Binds ajax post event to all buttons called btnSaveLike. Button must be part of a form.
//
// After page is loaded, bind to buttons on document.
$(document).ready(function () {
    bindSaveLike();
});
function bindSaveLike() {
    $('[name="btnSaveLike"]').parents('form').on("submit", function (e) {
        e.preventDefault();
        saveLike(e);
        return false;
    });
}

function saveLike(clickEvent) {
    //form = $(clickEvent.target).parents('form').eq(0);
    form = $(clickEvent.target);
    console.log(form);
    relevantData = {
        //Id: $(clickEvent.target).siblings("#Id").eq(0).val(),
        Id: form.children('[name="Id"]').first().val(),
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val(),
    };

    $.ajax({
        type: "POST",
        url: form.attr('action'),
        data: relevantData,
        dataType: "json",
        success: function (response) {
            if (response.success == true) {
                alert("Saved to your liked objects!");
            }
            if (response.success == false) {
                alert("You have already saved this to favorite objects!");
            }
        }
    });
}