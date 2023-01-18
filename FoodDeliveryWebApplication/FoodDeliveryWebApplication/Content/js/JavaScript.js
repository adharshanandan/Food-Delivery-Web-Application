function BtnSubmitClick() {
   
    var Enquiry = {};
    Enquiry["Name"] = $("#txtName").val();
    Enquiry["EmailId"] = $("#txtEmail").val();
    Enquiry["Message"] = $("#txtMsg").val();

    $.ajax({
        url: "Home/UploadData",
        contentType: "application/json;charset=ut-f-8",
        method: "POST",
        data: JSON.stringify(Enquiry),
        success: function (data) {
            alert("success");
            //var fd = new FormData();
            //var xhr = new XMLHttpRequest();
            ///*xhr.open(url);*/
            //xhr.send(fd);

        },
        error: function (data) {
            alert("Failed to submit inquiry !");

        }
    });
}