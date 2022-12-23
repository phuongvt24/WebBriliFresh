$(document).ready(function() {
    // Số lượng loại sản phẩm tất cả
    $("#numPro").html($('.content__pro-info').length)

    //An thong bao
    if ($("#numPro").text()=="0"){
        $(".content__empty-message").show();
        //Vo hieu hoa
        $(".content__order-button").attr("disabled", true);
        $(".content__order-button").css("cursor", "auto");
        $(".content__order-button").css("background-color", "#DBDADA");
        $(".content__order-button").css("color", "#504a4a");
        $(".content__order-button").css("border", "1px solid #504a4a");
    } else {
        $(".content__empty-message").hide();
    }

    //Hiển thị "Đơn giá" và "Thành tiền" - giao diện ban đầu
    for (var i=0; i < $(".content__unit-price").length; i++) {
        var unitPrice = $(".content__unit-price")[i].innerHTML;
        var quantity = $(".content__quantity")[i].value;
        //Tính giá trị thành tiền
        var amount = unitPrice * quantity;
        //Chuyển "Đơn giá" từ dạng số sang dạng tiền tệ
        unitPrice = Number(unitPrice).toLocaleString('en');
        $(".content__unit-price")[i].innerHTML = unitPrice + " đ";
        //Chuyển "Thành tiền" từ dạng số sang dạng tiền tệ
        amount = Number(amount).toLocaleString('en');
        $(".content__amount")[i].innerHTML = amount + " đ";
    }

    //Nút giảm số lượng sản phẩm
    $('.content__minus').click(function () {
        var $input = $(this).parent().find('input');
        var count = parseInt($input.val()) - 1;
        count = count < 1 ? 1 : count;
        $input.val(count);
        $input.change();
        $.ajax({
            url: "/BuyAndPay/Update",
            data: {
                proId: $(this).data("id"),
                quantity: count,
                storeid: $(this).data("storeid")
            },
            success: function (data) {
                
                $("#quantity_cart").html(data.quantity);
            }
        });
        return false;
    });


    $(".content__quantity").change(function () {
        console.log($(this).val())
        $.ajax({
            url: "/buyandpay/update",
            data: {
                proid: $(this).siblings(".content__plus").data("id"),
                quantity: $(this).val(),
                storeid: $(this).siblings(".content__plus").data("storeid")
            },
            success: function (data) {
                console.log($("#quantity_cart"))
                $("#quantity_cart").html(data.quantity);
            }
        });
    })

    //Nút tăng số lượng sản phẩm
    $('.content__plus').click(function () {

        var $input = $(this).parent().find('input');
        $input.val(parseInt($input.val()) + 1);
        $input.change();
        $.ajax({
            url: "/BuyAndPay/Update",
            data: {
                proId: $(this).data("id"),
                quantity: $input.val(),
                storeid: $(this).data("storeid")
            },
            success: function (data) {
                console.log($("#quantity_cart"))
                $("#quantity_cart").html(data.quantity);
            }
        });
        return false;
    });

    //Nút chọn tất cả sản phẩm
    $(".content__select-all").click(function() {
        $('.content__select').prop("checked", $(this).prop("checked"));
        $(".content__message").hide();
    });
          
    //Nút chọn sản phẩm con
    $('.content__select').click(function() {
        //Lấy danh sách checkbox sản phẩm con
        var checkboxes = $('input.content__select');
        //Lấy danh sách checkbox sản phẩm con đã được check
        var checkedboxes = checkboxes.filter(':checked');
        //Nếu số lượng bằng nhau thì nút "Chọn tất cả sản phẩm" tự động check
        if(checkboxes.length == checkedboxes.length) {
            $(".content__select-all").prop("checked", true);
        } else {
            $(".content__select-all").prop("checked", false);
        }
        $(".content__message").hide();
    });

    //Nút xóa tất cả sản phẩm
    $('.content__remove-all').click(function(e) {
        //Thông báo xác nhận trước khi xóa
        var result = confirm("Xác nhận xóa sản phẩm ra khỏi giỏ hàng?");
        if (result) {
            //Xóa tất cả sản phẩm con
            $("div").remove(".content__data");
            //Cập nhật số lượng sản phẩm trong giỏ là 0
            $("#numPro").html("0");
            //Vô hiệu hóa nút này
            $(this).attr("disabled", true);
        }

        $(".content__empty-message").show();
        //Vo hieu hoa
        $(".content__order-button").attr("disabled", true);
        $(".content__order-button").css("cursor", "auto");
        $(".content__order-button").css("background-color", "#DBDADA");
        $(".content__order-button").css("color", "#504a4a");
        $(".content__order-button").css("border", "1px solid #504a4a");
    })

    //Nút xóa sản phẩm con
    $('.content__remove').click(function() {
        //Thông báo xác nhận trước khi xóa
        var result = confirm("Xác nhận xóa sản phẩm ra khỏi giỏ hàng?");
        
        if (result) {
            $.ajax({
                url: "/BuyAndPay/Delete",
                data: {
                    proId: $(this).data("id"),
                    storeid: $(this).data("storeid")
                },
                success: function (data) {
                    $("#quantity_cart").html(data.quantity);
                }
            });
            //Xóa sản phẩm đó
            $(this).parent().parent().remove();
            //Cập nhật số lượng sản phẩm trong giỏ
            var numPro = $('.content__pro-info').length;
            $("#numPro").html(numPro);
        }

        if ($("#numPro").text()=="0"){
            $(".content__empty-message").show();
            //Vo hieu hoa
            $(".content__order-button").attr("disabled", true);
            $(".content__order-button").css("cursor", "auto");
            $(".content__order-button").css("background-color", "#DBDADA");
            $(".content__order-button").css("color", "#504a4a");
            $(".content__order-button").css("border", "1px solid #504a4a");
        } else {
            $(".content__empty-message").hide();
        }
    })
   
    //Hiển thị "Thành tiền" trong trường hợp thay đổi số lượng sản phẩm
    $('.content__quantity').change(function () {
        
        for (var i=0; i < $(".content__unit-price").length; i++) {
            var unitPrice = $(".content__unit-price")[i].innerHTML;
            var quantity = $(".content__quantity")[i].value;
            //Chuyển "Đơn giá" từ dạng tiền tệ sang dạng số
            unitPrice = unitPrice.replace(/\D/g,'');
            unitPrice = parseFloat(unitPrice);
            //Tính giá trị thành tiền
            var amount = unitPrice * quantity;
            //Chuyển "Thành tiền" từ dạng số sang dạng tiền tệ
            amount = Number(amount).toLocaleString('en');
            $(".content__amount")[i].innerHTML = amount + " đ";
        }
    })

    //Nút Đặt hàng
    $('.content__order-button').click(function(e) {
        //Trước đó phải chọn ít nhất một loại sản phẩm
        if (!$('input.content__select').is(":checked")){
            e.preventDefault();
            $(".content__message").show();
        }
        else {
            //Lưu số lượng sản phẩm đã chọn
            var checkboxes = $('input.content__select');
            var checkedboxes = checkboxes.filter(':checked');
            sessionStorage.setItem("NUMPRO", checkedboxes.length);

            //Tạo mảng để lưu danh sách "Ảnh sản phẩm", "Tên sản phẩm", "Đơn giá", "Số lượng", "ThanhTien"
            var aArray = new Array();
            var bArray = new Array();
            var cArray = new Array();
            var dArray = new Array();
            var eArray = new Array();
            var fArray = new Array();
            var gArray = new Array();
            var hArray = new Array();
            
            for (var i=0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    var image = $('.content__pro-image')[i].src;
                    var name = $('.content__pro-name')[i].innerHTML;
                    var unitPrice = $('.content__unit-price')[i].innerHTML;
                    var quantity = $('.content__quantity')[i].value;
                    var amount = $('.content__amount')[i].innerHTML;
                    var proid = $('.pro_id_item')[i].innerHTML;
                    var storeid = $('.store_id_item')[i].innerHTML;
                    var priceformat = $('.price_no_format')[i].innerHTML;

                    //Thêm phần tử vào mảng theo từng dòng sản phẩm
                    aArray.push(image);
                    bArray.push(name);
                    cArray.push(unitPrice);
                    dArray.push(quantity);
                    eArray.push(amount);
                    fArray.push(proid);
                    gArray.push(storeid);
                    hArray.push(priceformat);
                }
                
            }
            //Lưu mảng
            sessionStorage.setItem("PROIMAGE", JSON.stringify(aArray));
            sessionStorage.setItem("PRONAME", JSON.stringify(bArray));
            sessionStorage.setItem("UNITPRICE", JSON.stringify(cArray));
            sessionStorage.setItem("QUANTITY", JSON.stringify(dArray));
            sessionStorage.setItem("AMOUNT", JSON.stringify(eArray));
            sessionStorage.setItem("PROID", JSON.stringify(fArray));
            sessionStorage.setItem("STOREID", JSON.stringify(gArray));
            sessionStorage.setItem("PRICE", JSON.stringify(hArray));
        }
    })
});
