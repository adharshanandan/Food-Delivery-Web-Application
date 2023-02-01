function BtnSubmitClick() {
   
    var Enquiry = {};
    Enquiry["Name"] = $("#txtName").val();
    Enquiry["EmailId"] = $("#txtEmail").val();
    Enquiry["Message"] = $("#txtMsg").val();

    $.ajax({
        url: "Home/UploadData",  
        method: "POST",
        data: { "obj": Enquiry },
        success: function (data) {
            alert(data);
            aler("Success");
        },
        error: function (err) {
            alert("Failed to submit inquiry !");

        }
    });
}