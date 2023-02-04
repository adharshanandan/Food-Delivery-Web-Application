
$(document).ready(function () {
    var i = 1;
    $("input[type='hidden']").each(function () {

        if ($(this).attr('id') != 0) {
            $("#p1_" + i).attr('class', 'iconConfirm');
            $("#span1_" + i).html("<b>Order Placed</b>");

        }



        if ($(this).val() == "Confirmed") {

            $("#p2_" + i).attr('class', 'iconConfirm');
            $("#span2_" + i).html("<b>Order Confirmed</b>");


        }
        if ($(this).val() == "Confirmed" && $(this).attr('id') != 0) {
            $("#v1_" + i).css({ 'border-color': 'green' });

        }
        if ($(this).attr("name") == "Picked") {

            $("#p3_" + i).attr('class', 'iconConfirm');
            $("#span3_" + i).html("<b>Order Picked</b>");

        }

        if ($(this).val() == "Confirmed" && $(this).attr('id') != 0 && $(this).attr("name") == "Picked") {
            $("#v2_" + i).css({ 'border-color': 'green' });

        }

        i++;
    });


    $(".btnCancelOrder[type='button']").each(function () {
        if ($(this).val() == "Picked") {
            $(this).hide();

        }
        else {
            $(this).show();
        }
    });

});

$(".btnCancelOrder").click(function () {
    var orderId = $(this).attr('id');
    var payMode = $(this).attr('name');
    if (payMode == "Card") {
        window.location.href = "/Payment/SelectBankToRefund?orderId="+orderId;
        //$.ajax({
        //    method: "POST",
        //    url: "/Payment/SelectBankToRefund/",
        //    data: { "orderId": orderId }
          
        //})
    }
    else {
        $.ajax({
            method: "POST",
            url: "/Customer/CancelOrders/",
            data: { "orderId": orderId },
            success: function (result) {
                alert(result);
                location.reload();
            },
            error: function (err) {
                alert(err);
            }
        })
    }
  
})