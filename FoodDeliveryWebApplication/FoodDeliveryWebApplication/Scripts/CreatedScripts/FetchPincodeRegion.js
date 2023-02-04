
    $(document).ready(function () {
        
    $("#txtPincode").change(function () {

     var inputPincode = $("#txtPincode").val();
    var url = "/Restaurant/GetLocation"
    $.ajax({
        url: url,
    data: {'pincode': inputPincode },
    type: "GET",
    success: function (data) {
                    if ($("#txtPincode").valid() == true) {
        $("#txtLocation").show();
                    }
    else {
        $("#txtLocation").hide();
                    }

    $("#ddlArea").empty();
    $.each(data, function (index, row) {
        $("#ddlArea").append("<option value='" + row.Name + "'>" + row.Name + "</option>")
    })

                },
    error: function (data) {
        alert("Failed to retrieve location details");

                }
            })
        })
    })

