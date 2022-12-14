(function ($) {
    $.fn.inputFilter = function (callback, errMsg) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop focusout", function (e) {
            if (callback(this.value)) {
                // Accepted value
                if (["keydown", "mousedown", "focusout"].indexOf(e.type) >= 0) {
                    $(this).removeClass("input-error");
                    this.setCustomValidity("");
                }
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                // Rejected value - restore the previous one
                $(this).addClass("input-error");
                this.setCustomValidity(errMsg);
                this.reportValidity();
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                // Rejected value - nothing to restore
                this.value = "";
            }
        });
    };
}(jQuery));

$(document).ready(function () {
    //Kiểm tra giá trị nhập vào của trường Điện thoại
    $("#phonenum").inputFilter(function (value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    }, "Số điện thoại phải là số");

    //Combobox địa chỉ
    var citis = document.getElementById("city");
    var districts = document.getElementById("district");
    var wards = document.getElementById("ward");
    var Parameter = {
        url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
        method: "GET",
        responseType: "application/json",
    };
    var promise = axios(Parameter);
    promise.then(function (result) {
        renderCity(result.data);
    });

    function renderCity(data) {
        for (const x of data) {
            citis.options[citis.options.length] = new Option(x.Name, x.Id);
        }
        citis.onchange = function () {
            district.length = 1;
            ward.length = 1;
            if (this.value != "") {
                const result = data.filter(n => n.Id === this.value);

                for (const k of result[0].Districts) {
                    districts.options[districts.options.length] = new Option(k.Name, k.Id);
                }
            }
        };
        districts.onchange = function () {
            ward.length = 1;
            const dataCity = data.filter((n) => n.Id === citis.value);
            if (this.value != "") {
                const dataWards = dataCity[0].Districts.filter(n => n.Id === this.value)[0].Wards;

                for (const w of dataWards) {
                    wards.options[wards.options.length] = new Option(w.Name, w.Id);
                }
            }
        };
    }

    //Nút đăng nhập
    $(".content__login-button").click(function () {
        //Chuyển tới trang đăng nhập
        //...
        //Quay lại trang sau khi đăng nhập
        window.location.href = 'DeliveryInfoLogin';
    });

    //Nút Tiếp tục
    $(".content__continue-button").click(function () {
        //Yêu cầu phải điền đủ thông tin
        $("input").prop('required', true);
        $("select").prop('required', true);



        var isFullFill = 1;
        if ($("#fullname").is(':invalid')) {
            $("#fullname-message").show();
            isFullFill = 0;
        }
        if ($("#phonenum").is(':invalid')) {
            $("#phonenum-message").show();
            isFullFill = 0;
        }
        if ($("#city").is(':invalid')) {
            $("#city-message").show();
            isFullFill = 0;
        }
        if ($("#district").is(':invalid')) {
            $("#district-message").show();
            isFullFill = 0;
        }
        if ($("#ward").is(':invalid')) {
            $("#ward-message").show();
            isFullFill = 0;
        }

        if (isFullFill == 1) {
            $fullname = $("#fullname").val();
            $phonenum = $("#phonenum").val();
            $city = $("#city option:selected").text();
            $district = $("#district option:selected").text();
            $ward = $("#ward option:selected").text();
            $specificAddress = $("#specificAddress").val();

            sessionStorage.setItem("FULLNAME", $fullname);
            sessionStorage.setItem("PHONENUM", $phonenum);
            sessionStorage.setItem("SPECIFIC_ADDRESS", $specificAddress);
            sessionStorage.setItem("WARD", $ward);
            sessionStorage.setItem("DISTRICT", $district);
            sessionStorage.setItem("CITY", $city);
        }
    });

    //Bỏ thông báo yêu cầu điền/chọn
    $("#fullname").keyup(function () {
        $("#fullname-message").hide();
    })
    $("#phonenum").keyup(function () {
        $("#phonenum-message").hide();
    })
    $("#city").change(function () {
        $("#city-message").hide();
    })
    $("#district").change(function () {
        $("#district-message").hide();
    })
    $("#ward").change(function () {
        $("#ward-message").hide();
    })
});
