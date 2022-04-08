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
    var form = $(clickEvent.target);
    var relevantData = {
        //Id: $(clickEvent.target).siblings("#Id").eq(0).val(),
        Id: form.children('[name="Id"]').first().val(),
        __RequestVerificationToken: $('input[name="__RequestVerificationToken"]', form).val(),
    };

    $.ajax({
        type: "POST",
        url: form.attr('action'),
        data: relevantData,
        dataType: "json",
        success: function (res, status, xhr) {
            if (res.success == true) {
                alert("Saved to your liked objects!");
            }
            if (res.success == false) {
                alert("You have already saved this to favorite objects!");
            }
        },
        error: function (res, status) {
            var redirectOn = [401, 404];
            if (redirectOn.includes(res.status)) {
                var redirectURL = res.getResponseHeader('location');
                window.location.replace(redirectURL);
            }
        },
    });

}