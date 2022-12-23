$(document).ready(function () {
    //Số lượng loại sản phẩm tất cả
    $numPro = sessionStorage.getItem("NUMPRO");
    $("#numPro").html($numPro);

    //Chèn dòng sản phẩm tùy theo số lượng loại sản phẩm đã chọn trước đó
    var $row =  `<div class="row">
                            <div class="col-sm-1">
                                <span class="content__pro-no"></span>
                            </div>
                            <div class="col-md-4">
                                <div class="content__pro-info">
                                    <img class="content__pro-image" ></img>
                                    <span class="content__pro-name"></span>
                                </div>
                            </div>
                            <div class="col-md-2"><span class="content__unit-price"></span> </div>
                            <div class="col-md-2"><span class="content__quantity"></span></div>
                            <div class="col-md-2"><span class="content__amount"></span></div>
                        </div>`;
    for (var i=0; i < $numPro;i++) {
        //Thêm một dòng sản phẩm
        $("#products").append ($row);
        //Thứ tự của dòng sản phẩm
        $('.content__pro-no')[i].innerHTML=(i+1);
    }

    //Lấy danh sách "Ảnh sản phẩm", "Tên sản phẩm", "Đơn giá", "Số lượng", "Thành tiền"
    var a = JSON.parse(sessionStorage.getItem("PROIMAGE"));
    var b = JSON.parse(sessionStorage.getItem("PRONAME"));
    var c = JSON.parse(sessionStorage.getItem("UNITPRICE"));
    var d = JSON.parse(sessionStorage.getItem("QUANTITY"));
    var e = JSON.parse(sessionStorage.getItem("AMOUNT"));
    //Chèn dữ liệu vào từng dòng sản phẩm
    for (var i=0; i < $(".content__pro-name").length; i++) {
        $(".content__pro-image")[i].src=a[i];
        $(".content__pro-name")[i].innerHTML=b[i];
        $(".content__unit-price")[i].innerHTML=c[i];
        $(".content__quantity")[i].innerHTML=d[i];
        $(".content__amount")[i].innerHTML=e[i];
    }

    //Hiển thị "Tạm tính" và "Tổng tiền"
    var $subtotal = 0;
    var $discount = 0;
    var $total = 0;

    for (var i=0; i < $(".content__amount").length; i++) {
        var $amount = $(".content__amount")[i].innerHTML;
        //Chuyển "Thành tiền" từ dạng tiền tệ sang dạng số
        $amount = $amount.replace(/\D/g,'');
        $amount = parseFloat($amount);
        $subtotal += $amount;
        $total = $subtotal;
    }
    //Chuyển "Tạm tính" từ dạng số sang dạng tiền tệ
    $(".content__subtotal-value").html(Number($subtotal).toLocaleString('en')+ " ₫");
    //"Tổng tiền" bằng "Tạm tính" trong trường hợp không nhập mã giảm giá
    $(".content__total-value").html($(".content__subtotal-value").text());

    //Trường hợp có nhập mã giảm giá
    $(".content__coupon-button").click(function() {
        //Giả sử mã là 12345
       
        if ($(".content__coupon-text").val() == "12345") {
            $(".content__coupon-message").hide();
            //Bỏ ẩn
            $(".content__discount").show();
            $(".content__discount-value").show();
            //Giả sử phần trăm giảm giá của mã là 10%
            $(".content__discount-value").html(0.1);
            var $discountPercent = $(".content__discount-value").text();
            //Tính số tiền được giảm
            $discount = $subtotal * $discountPercent;
            //Tính "Tổng tiền"
            $total = $subtotal - $discount;
            //Chuyển "Giảm giá" từ dạng số sang dạng tiền tệ
            $(".content__discount-value").html ("- " + Number($discount).toLocaleString('en')+ " ₫");
            //Chuyển "Tổng tiền" từ dạng số sang dạng tiền tệ
            $(".content__total-value").html(Number($total).toLocaleString('en')+ " ₫");
        }
        else {
            $(".content__coupon-message").show();
        }
    });

    

    // Giao diện khi có nhập mã khuyến mãi
    $(".content__coupon-text").keyup (function(e){
        //Phím Enter
        $(window).keydown (function(e){
            if (e.keyCode == 13) {
                e.preventDefault();
                $(".content__coupon-button").click();
            }
        });

        if ($(".content__coupon-text").val() == "") {
            $(".content__coupon-button").css("background-color", "#DBDADA");
            $(".content__coupon-button").css("color", "#504a4a");
            $(".content__coupon-button").attr("disabled", true);
            $(".content__coupon-button").css("cursor", "auto");
            $(".content__coupon-message").hide();
        }
        else {
            $(".content__coupon-button").css("background-color", "#71D30F");
            $(".content__coupon-button").css("color", "#FFFFFF");
            $(".content__coupon-button").attr("disabled", false);
            $(".content__coupon-button").css("cursor", "pointer");
        }
    });

    //Nút Tiếp tục
    $('.content__continue-button').click(function() {
        //Lưu "Tạm tính", "Giảm giá", "Tổng tiền"
        sessionStorage.setItem("SUBTOTAL", $subtotal);
        sessionStorage.setItem("DISCOUNT", $discount);
        sessionStorage.setItem("TOTAL", $total);
    })
})