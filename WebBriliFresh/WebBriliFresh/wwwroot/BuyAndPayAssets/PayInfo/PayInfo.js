function goBack() {
    window.history.back();
}

$(document).ready(function () {
    //Số lượng loại sản phẩm tất cả
    $numPro = sessionStorage.getItem("NUMPRO");
    $("#numPro").html($numPro + " sản phẩm.");

    //Chèn dòng sản phẩm tùy theo số lượng loại sản phẩm đã chọn trước đó
    var $row =  `<div class="content__pro-info">
                    <img class="content__pro-image"></img>
                    <span class="content__pro-detail">
                        <span class="content__pro-name"></span>
                        <br>
                        <span class="content__pro-quantity"></span>
                    </span>
                    <span class="content__pro-unit-price"></span>
                </div>`;
    for (var i=0; i < $numPro; i++) {
        //Thêm một dòng sản phẩm
        $("#products").append ($row);
    }

    //Lấy danh sách "Ảnh sản phẩm", "Tên sản phẩm", "Đơn giá", "Số lượng"
    var h = JSON.parse(sessionStorage.getItem("PRICE"));
    var a = JSON.parse(sessionStorage.getItem("PROIMAGE"));
    var b = JSON.parse(sessionStorage.getItem("PRONAME"));
    var c = JSON.parse(sessionStorage.getItem("UNITPRICE"));
    var d = JSON.parse(sessionStorage.getItem("QUANTITY"));
    var e = JSON.parse(sessionStorage.getItem("PROID"));
    var f = JSON.parse(sessionStorage.getItem("STOREID"));
    

    //Chèn dữ liệu vào từng dòng sản phẩm
    for (var i=0; i < $(".content__pro-name").length; i++) {
        $(".content__pro-image")[i].src=a[i];
        $(".content__pro-name")[i].innerHTML=b[i];
        $(".content__pro-unit-price")[i].innerHTML=c[i];
        $(".content__pro-quantity")[i].innerHTML="SL: x" + d[i];
    }


    //Lấy và hiển thị "Họ tên", "Điện thoại", "Địa chỉ"
    $fullname = sessionStorage.getItem("FULLNAME");
    $phonenum = sessionStorage.getItem("PHONENUM");
    $specificAddress = sessionStorage.getItem("SPECIFIC_ADDRESS");
    $ward = sessionStorage.getItem("WARD");
    $district = sessionStorage.getItem("DISTRICT");
    $city = sessionStorage.getItem("CITY");

    $addid = sessionStorage.getItem("ADDID");
    $disid = sessionStorage.getItem("DISID");

    $("#fullname").html($fullname);
    $("#phonenum").html($phonenum);
    $("#specificAddress").html($specificAddress);
    $("#ward").html($ward);
    $("#district").html($district);
    $("#city").html($city);

    


    $("#First_Name").val($fullname)
    $("#Phone_cus").val($phonenum)
    $("#SpecificAddress_add").val($specificAddress)
    $("#Ward_add").val($ward)
    $("#District_add").val($district)
    $("#City_add").val($city)


    $("#Address_Id").val($addid)
    $("#Dis_Id").val($disid)


    var checkoutitem = []
    for (var i = 0; i < e.length; i++) {
        checkoutitem.push({ productId: e[i], quantity: d[i], storeId: f[i], saleprice: h[i] });
    };
    $("#listorder").val(JSON.stringify(checkoutitem));
    





    //Ẩn dấu phẩy nếu địa chỉ cụ thể trống
    if ($specificAddress =="")
    {
      $("#specificAddress-comma").hide();
    }


    //Lấy và hiển thị "Tạm tính", "Giảm giá", "Tổng tiền"
    $subtotal = sessionStorage.getItem("SUBTOTAL");
    $discount = sessionStorage.getItem("DISCOUNT");
    $total = sessionStorage.getItem("TOTAL");

    $("#Sub_Total").val($subtotal);
    $("#Order_Total").val($total - (-14000));
    //Tính lại tổng tiền sau khi có phí vận chuyển
    var $transortFee = $("#transort-fee").text();
    $total = parseFloat($total) + parseFloat($transortFee);

    $("#transort-fee").html(Number($transortFee).toLocaleString('en') + " ₫");
    $("#subtotal").html(Number($subtotal).toLocaleString('en') + " ₫");
    $("#discount").html("- " + Number($discount).toLocaleString('en') + " ₫");
    $("#total").html(Number($total).toLocaleString('en') + " ₫");

    //Chọn hình thức giao hàng
    $(".content__delivery-method").click(function () {
        $(".content__delivery-method-button").each(function () {         
            if ($(this).is(':checked')) {
                $(this).parent().css("border", "1px solid #1ED330");
                $(this).parent().css("background-color", "#C2F49B");
                
                $("#deliveryName").html($(this).siblings().text().toUpperCase());

                
                $("#type_trans").val($(this).val())
                switch ($(this).val()) {
                    case "1":
                        $transortFee = 14000;
                        break;
                    case "2":
                        $transortFee = 30000;
                        break;
                }
                $("#transort-fee").html(Number($transortFee).toLocaleString('en') + " ₫");
                $("#deliveryFee").html(Number($transortFee).toLocaleString('en') + " ₫");
                //Tính lại tổng tiền
                $total = sessionStorage.getItem("TOTAL");
                $total = parseFloat($total) + parseFloat($transortFee);
                $("#total").html(Number($total).toLocaleString('en') + " ₫");
                $("#Order_Total").val($total);

            }
            else {
                $(this).parent().css("border", "1px solid #826C6C");
                $(this).parent().css("background-color", "#DBDADA");
            }
        })
    });

    //Chọn hình thức thanh toán
    $(".content__payment-method").click(function () {
        if ($("#payment-method-button2").is(':checked')) {
            $(".content__E-wallets").show();
            $("#E-wallet-button1").prop('required', true);
        }
        else {
            $(".content__E-wallet-button").prop("checked", false);
            $(".content__E-wallet").css("background-color", "#E3DDDD");
            $(".content__E-wallet").css("border", "1px solid #826C6C");
            $(".content__E-wallets").hide();
            $("#E-wallet-button1").prop('required', false);
            $("#E-wallet-message").hide();
        }

        if ($("#payment-method-button1").is(':checked')) {
            $("#Pay_by").val($("#payment-method-button1").val())
        }
        if ($("#payment-method-button3").is(':checked')) {
            $("#Pay_by").val($("#payment-method-button3").val())
        }
        if ($("#payment-method-button4").is(':checked')) {
            $("#Pay_by").val($("#payment-method-button4").val())
        }
        

        
    });

    //Chọn loại ví điện tử
    $(".content__E-wallet").click(function () {
        $(".content__E-wallet-button").each(function() {
            if ($(this).is(':checked')) {
                $(this).parent().css("background-color", "#B4EB89");
                $(this).parent().css("border", "1px solid #1ED330");
                $("#E-wallet-message").hide();
            }
            else {
                $(this).parent().css("background-color", "#E3DDDD");
                $(this).parent().css("border", "1px solid #826C6C");
            }
        })
        if ($("#E-wallet-button1").is(':checked')) {
            $("#Pay_by").val($("#E-wallet-button1").val())
        }
        if ($("#E-wallet-button2").is(':checked')) {
            $("#Pay_by").val($("#E-wallet-button2").val())
        }
        if ($("#E-wallet-button3").is(':checked')) {
            $("#Pay_by").val($("#E-wallet-button3").val())
        }
        if ($("#E-wallet-button4").is(':checked')) {
            $("#Pay_by").val($("#E-wallet-button4").val())
        }
        if ($("#E-wallet-button5").is(':checked')) {
            $("#Pay_by").val($("#E-wallet-button5").val())
        }
    });

    $("#products").hide();
    //Nút Xem thông tin
    $("#detail-button").click (function () {
        var txt = $("#products").is(":visible") ? 'Xem thông tin' : 'Rút gọn';
        $("#detail-button").text(txt);
        $("#products").toggle();
    });

    

    /**/
    //Nút đặt hàng
    $("#order-button-fake").click(function (event) {
        //Thông báo nếu chưa chọn ví điện tử trong trường hợp chọn thanh toán bằng ví điện tử
        if ($("#E-wallet-button1").is(':invalid')) {
            $("#E-wallet-message").show();
        }
        //event.preventDefault();

        //var checkoutitem = []
        //for (var i = 0; i < e.length; i++)
        //{
        //    checkoutitem.push ({ productId: e[i], quantity: d[i], storeId: f[i], saleprice: h[i] });
        //}
        //$.ajax({
        //    dataType: 'text',
        //    type: 'POST',
        //    url: '/BuyAndPay/checkout',
        //    data: { checkoutitem: JSON.stringify(checkoutitem) },
        //    success: function (data) {
        //        console.log(data)
        //    }
        //});
        //$("#order-button").click();
    });


});
