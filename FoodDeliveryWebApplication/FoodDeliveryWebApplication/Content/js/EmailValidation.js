$(document).ready(function () {
    $("#verifyBtn").hide();
    $("#verifyEmail").hide();
    $("#txtEmail").change(function () {
        var validRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
        if ($("#txtEmail").val().match(validRegex)) {
            $("#verifyEmail").show();
            $("#verifyBtn").show();
        }
        else {
            $("#verifyEmail").hide();
            $("#verifyBtn").hide();
        }
    })
})


