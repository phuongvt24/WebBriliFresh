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
    $("#phonenumEdit").inputFilter(function (value) {
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

    //Combobox địa chỉ - Edit
    var citisEdit = document.getElementById("cityEdit");
    var districtsEdit = document.getElementById("districtEdit");
    var wardsEdit = document.getElementById("wardEdit");
    var ParameterEdit = {
        url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
        method: "GET",
        responseType: "application/json",
    };
    var promiseEdit = axios(ParameterEdit);
    promiseEdit.then(function (result) {
        renderCityEdit(result.data);
    });

    function renderCityEdit(data) {
        for (const x of data) {
            citisEdit.options[citisEdit.options.length] = new Option(x.Name, x.Id);
        }
        citisEdit.onchange = function () {
            district.length = 1;
            ward.length = 1;
            if (this.value != "") {
                const result = data.filter(n => n.Id === this.value);

                for (const k of result[0].Districts) {
                    districtsEdit.options[districtsEdit.options.length] = new Option(k.Name, k.Id);
                }
            }

            $('#cityEdit option[value=""]').text("Chọn Tỉnh/Thành phố");
            $('#districtEdit option[value=""]').text("Chọn Quận/Huyện");
            $('#wardEdit option[value=""]').text("Chọn Phường/Xã");

            $("#cityEdit").prop('required', true);
            $("#districtEdit").prop('required', true);
            $("#wardEdit").prop('required', true);
        };
        districtsEdit.onchange = function () {
            ward.length = 1;
            const dataCity = data.filter((n) => n.Id === citisEdit.value);
            if (this.value != "") {
                const dataWards = dataCity[0].Districts.filter(n => n.Id === this.value)[0].Wards;

                for (const w of dataWards) {
                    wardsEdit.options[wardsEdit.options.length] = new Option(w.Name, w.Id);
                }
            }
        };
    }

    //Địa chỉ ở ô đầu tiên là mặc định
    $(".content__default")[0].innerHTML = "Mặc định";
    $(".content__address-change")[0].style.backgroundColor = "#58B63A";
    $(".content__address-change")[0].style.backgroundColor = "##a1e08c";
    $(".content__address-change")[0].style.border = "1px solid #118129";
    $(".content__address-list")[0].style.backgroundColor = "#D9EBFB";
    $(".content__address-list")[0].style.border = "1px dashed #1A86E9";

    // Ẩn dấu phẩy nếu không có địa chỉ cụ thể
    $(".content__specificAddress").each(function () {
        if ($(this).text() == "") {
            $(this).siblings(".content__specificAddress-comma").html("");
        }
    })

    //Thay đổi giao diện khi chọn các địa chỉ có sẵn
    $(".content__address-list").click(function () {
        $(".content__address-change-button").each(function () {
            if ($(this).is(':checked')) {
                $(this).parent().css("background-color", "#58B63A");
                $(this).parent().css("border", "1px solid #118129");

                $(this).parent().parent().parent().css("background-color", "#D9EBFB");
                $(this).parent().parent().parent().css("border", "1px dashed #1A86E9");
            }
            else {
                $(this).parent().css("background-color", "#6C5D5D");
                $(this).parent().css("border", "1px solid #292D32");

                $(this).parent().parent().parent().css("background-color", "#FEF8F8");
                $(this).parent().parent().parent().css("border", "1px solid #8F8C8C");
            }
        })
    });

    //Ẩn table Sửa địa chỉ khi chọn nút Giao đến địa chỉ này, nút Mặc định, nút Xóa
    $(".content__address-change-button, .content__address-default-button").click(function () {
        $("#addresstableEdit").hide();
        $("#space").show();

        //Khôi phục nút Tiếp tục
        $(".content__continue-button").prop("disabled", false);
        $(".content__continue-button").css("cursor", "pointer");
        $(".content__continue-button").css("background-color", "#EAFFE4");
        $(".content__continue-button").css("border", "1px solid #118129");
        $(".content__continue-button").css("color", "#118129");
        $(".content__continue-button").hover(function () {
            $(this).css("background-color", "#118129");
            $(this).css("border", "1px solid #118129");
            $(this).css("color", "#FFFFFF");
        }, function () {
            $(this).css("background-color", "#EAFFE4");
            $(this).css("border", "1px solid #118129");
            $(this).css("color", "#118129");
        })
    })

    //Chọn nút Sửa
    $(".content__address-edit-button").click(function () {
        $("#addresstableEdit").show();
        $("#space").hide();
        $(this).siblings(".content__address-change").children(".content__address-change-button").prop("checked", true);
        //Ẩn thông báo nếu có trước đó
        $("#fullname-message-edit").hide();
        $("#phonenum-message-edit").hide();
        $("#city-message-edit").hide();
        $("#district-message-edit").hide();
        $("#ward-message-edit").hide();

        $("#cityEdit").prop('required', false);
        $("#districtEdit").prop('required', false);
        $("#wardEdit").prop('required', false);
        //Drop down chọn option empty
        $("#cityEdit").val("");
        $("#districtEdit").val("");
        $("#wardEdit").val("");
        //Vô hiệu hóa nút Tiếp tục
        $(".content__continue-button").attr("disabled", "disabled");
        $(".content__continue-button").css("cursor", "auto");
        $(".content__continue-button").css("background-color", "#EAFFE4");
        $(".content__continue-button").css("border", "1px solid #8F8C8C");
        $(".content__continue-button").css("color", "#8F8C8C");

        //Tự fill
        $("#fullnameEdit").val($(this).parent().siblings(".content__fullname").text());
        $("#phonenumEdit").val($(this).parent().siblings(".content__phonenum").text());
        $('#cityEdit option[value=""]').text($(this).parent().siblings(".content__address").children(".content__city").text());
        $('#districtEdit option[value=""]').text($(this).parent().siblings(".content__address").children(".content__district").text());
        $('#wardEdit option[value=""]').text($(this).parent().siblings(".content__address").children(".content__ward").text());
        $("#specificAddressEdit").val($(this).parent().siblings(".content__address").children(".content__specificAddress").text());

        //Yêu cầu phải điền thông tin
        $("#fullnameEdit").prop('required', true);
        $("#phonenumEdit").prop('required', true);
    })

    //Chọn nút Lưu sau khi sửa
    $("#saveButton").click(function () {
        isFullFill = 1;
        if ($("#fullnameEdit").is(':invalid')) {
            $("#fullname-message-edit").show();
            isFullFill = 0;
        }

        if ($("#phonenumEdit").is(':invalid')) {
            $("#phonenum-message-edit").show();
            isFullFill = 0;
        }

        if ($("#cityEdit").is(':invalid')) {
            $("#city-message-edit").show();
            isFullFill = 0;
        }

        if ($("#districtEdit").is(':invalid')) {
            $("#district-message-edit").show();
            isFullFill = 0;
        }

        if ($("#wardEdit").is(':invalid')) {
            $("#ward-message-edit").show();
            isFullFill = 0;
        }

        if (isFullFill == 1) {
            $(".content__address-change-button").each(function () {
                if ($(this).is(':checked')) {
                    $(this).parent().parent().siblings(".content__fullname").html($("#fullnameEdit").val());
                    $(this).parent().parent().siblings(".content__phonenum").html($("#phonenumEdit").val());
                    $(this).parent().parent().siblings(".content__address").children(".content__city").html($("#cityEdit option:selected").text());
                    $(this).parent().parent().siblings(".content__address").children(".content__district").html($("#districtEdit option:selected").text());
                    $(this).parent().parent().siblings(".content__address").children(".content__ward").html($("#wardEdit option:selected").text());
                    if ($("#specificAddressEdit").val() == "") {
                        $(this).parent().parent().siblings(".content__address").children(".content__specificAddress-comma").html("");
                        $(this).parent().parent().siblings(".content__address").children(".content__specificAddress").html("");
                    }
                    else {
                        $(this).parent().parent().siblings(".content__address").children(".content__specificAddress-comma").html(", ");
                        $(this).parent().parent().siblings(".content__address").children(".content__specificAddress").html($("#specificAddressEdit").val());
                    }
                }
            })
            $("#addresstableEdit").hide();
            //Khôi phục nút Tiếp tục
            $(".content__continue-button").prop("disabled", false);
            $(".content__continue-button").css("cursor", "pointer");
            $(".content__continue-button").css("background-color", "#EAFFE4");
            $(".content__continue-button").css("border", "1px solid #118129");
            $(".content__continue-button").css("color", "#118129");
            $(".content__continue-button").hover(function () {
                $(this).css("background-color", "#118129");
                $(this).css("border", "1px solid #118129");
                $(this).css("color", "#FFFFFF");
            }, function () {
                $(this).css("background-color", "#EAFFE4");
                $(this).css("border", "1px solid #118129");
                $(this).css("color", "#118129");
            })
        }
    })

    //Chọn nút Mặc định
    $(".content__address-default-button").click(function () {
        //Gán các giá trị của địa chỉ mặc định hiện tại vào các biến
        var fullname = $(".content__fullname")[0].innerHTML;
        var phonenum = $(".content__phonenum")[0].innerHTML;
        var specificAddress = $(".content__specificAddress")[0].innerHTML;
        var ward = $(".content__ward")[0].innerHTML;
        var district = $(".content__district")[0].innerHTML;
        var city = $(".content__city")[0].innerHTML;

        if (specificAddress == "") {
            $(this).parent().siblings(".content__address").children(".content__specificAddress-comma").html("");
            $(".content__specificAddress-comma")[0].innerHTML = ", ";
        }
        else {
            $(this).parent().siblings(".content__address").children(".content__specificAddress-comma").html(", ");
            $(".content__specificAddress-comma")[0].innerHTML = "";
        }

        if ($(this).parent().siblings(".content__address").children(".content__specificAddress").text() == "") {
            $(".content__specificAddress-comma")[0].innerHTML = "";
        }
        else {
            $(".content__specificAddress-comma")[0].innerHTML = ", ";
        }

        //Hoán đổi các giá trị giữa hai địa chỉ mặc định cũ và mới
        $(".content__fullname")[0].innerHTML = ($(this).parent().siblings(".content__fullname").text());
        $(".content__phonenum")[0].innerHTML = ($(this).parent().siblings(".content__phonenum").text());
        $(".content__specificAddress")[0].innerHTML = ($(this).parent().siblings(".content__address").children(".content__specificAddress").text());
        $(".content__ward")[0].innerHTML = ($(this).parent().siblings(".content__address").children(".content__ward").text());
        $(".content__district")[0].innerHTML = ($(this).parent().siblings(".content__address").children(".content__district").text());
        $(".content__city")[0].innerHTML = ($(this).parent().siblings(".content__address").children(".content__city").text());

        $(this).parent().siblings(".content__fullname").html(fullname);
        $(this).parent().siblings(".content__phonenum").html(phonenum);
        $(this).parent().siblings(".content__address").children(".content__specificAddress").html(specificAddress);
        $(this).parent().siblings(".content__address").children(".content__ward").html(ward);
        $(this).parent().siblings(".content__address").children(".content__district").html(district);
        $(this).parent().siblings(".content__address").children(".content__city").html(city);

        //Chọn địa chỉ mặc định
        $(".content__address-change-button")[0].checked = true;
        $(".content__address-change-button").each(function () {
            if ($(this).is(':checked')) {
                $(this).parent().css("background-color", "#58B63A");
                $(this).parent().css("border", "1px solid #118129");

                $(this).parent().parent().parent().css("background-color", "#D9EBFB");
                $(this).parent().parent().parent().css("border", "1px dashed #1A86E9");
            }
            else {
                $(this).parent().css("background-color", "#6C5D5D");
                $(this).parent().css("border", "1px solid #292D32");

                $(this).parent().parent().parent().css("background-color", "#FEF8F8");
                $(this).parent().parent().parent().css("border", "1px solid #8F8C8C");
            }
        })
    })

    //Chọn nút Xóa
    $(".content__address-remove-button").click(function () {
        //Thông báo xác nhận trước khi xóa
        var result = confirm("Xác nhận xóa địa chỉ này?");
        if (result) {
            $(this).parent().parent().remove();
            //Chọn địa chỉ mặc định
            $(".content__address-change-button")[0].checked = true;
            $(".content__default")[0].innerHTML = "Mặc định";
            $(".content__address-change-button").each(function () {
                if ($(this).is(':checked')) {
                    $(this).parent().css("background-color", "#58B63A");
                    $(this).parent().css("border", "1px solid #118129");

                    $(this).parent().parent().parent().css("background-color", "#D9EBFB");
                    $(this).parent().parent().parent().css("border", "1px dashed #1A86E9");
                }
                else {
                    $(this).parent().css("background-color", "#6C5D5D");
                    $(this).parent().css("border", "1px solid #292D32");

                    $(this).parent().parent().parent().css("background-color", "#FEF8F8");
                    $(this).parent().parent().parent().css("border", "1px solid #8F8C8C");
                }
            })
        }
    })


    //Chọn thêm địa chỉ khác
    $("#addAddress").click(function () {
        $("#addresstable").show();
        $("#addresstableEdit").hide();
        $("#fullnameEdit").prop("required", false);
        $("#phonenumEdit").prop("required", false);
        $("#cityEdit").prop('required', false);
        $("#districtEdit").prop('required', false);
        $("#wardEdit").prop('required', false);
        $("#space").hide();
        //Khôi phục nút Tiếp tục
        $(".content__continue-button").prop("disabled", false);
        $(".content__continue-button").css("cursor", "pointer");
        $(".content__continue-button").css("background-color", "#EAFFE4");
        $(".content__continue-button").css("border", "1px solid #118129");
        $(".content__continue-button").css("color", "#118129");
        $(".content__continue-button").hover(function () {
            $(this).css("background-color", "#118129");
            $(this).css("border", "1px solid #118129");
            $(this).css("color", "#FFFFFF");
        }, function () {
            $(this).css("background-color", "#EAFFE4");
            $(this).css("border", "1px solid #118129");
            $(this).css("color", "#118129");
        })


        $("#availableAddress").css("color", "#1A86E9");
        $("#availableAddress").css("text-decoration", "underline");
        $("#availableAddress").css("cursor", "pointer");
        $("#availableAddress").attr("disabled", false);
        $("#address1").css("display", "none");
        $("#address2").css("display", "none");
        $(".content__address-change-button").prop("checked", false);
        //Vô hiệu hóa nút
        $(this).css("color", "#000000");
        $(this).css("text-decoration", "none");
        $(this).attr("disabled", "disabled");
        $(this).css("cursor", "auto");
        //Yêu cầu phải điền thông tin
        $("#fullname").prop('required', true);
        $("#phonenum").prop('required', true);
        $("#city").prop('required', true);
        $("#district").prop('required', true);
        $("#ward").prop('required', true);
    })

    //Chọn địa chỉ có sẵn
    $("#availableAddress").click(function () {
        $("#address1").show();
        $("#address2").show();
        $("#space").show();
        $("#addAddress").css("color", "#1A86E9");
        $("#addAddress").css("text-decoration", "underline");
        $("#addAddress").css("cursor", "pointer");
        $("#addAddress").attr("disabled", false);
        $("#addresstable").css("display", "none");
        $("#addressButton1").prop("checked", true);
        $(".content__address-change-button").each(function () {
            if ($(this).is(':checked')) {
                $(this).parent().css("background-color", "#58B63A");
                $(this).parent().css("border", "1px solid #118129");

                $(this).parent().parent().parent().css("background-color", "#D9EBFB");
                $(this).parent().parent().parent().css("border", "1px dashed #1A86E9");
            }
            else {
                $(this).parent().css("background-color", "#6C5D5D");
                $(this).parent().css("border", "1px solid #292D32");

                $(this).parent().parent().parent().css("background-color", "#FEF8F8");
                $(this).parent().parent().parent().css("border", "1px solid #8F8C8C");
            }
        })
        //Vô hiệu hóa nút
        $(this).css("color", "#000000");
        $(this).css("text-decoration", "none");
        $(this).css("cursor", "auto");
        $(this).attr("disabled", true);
        //Không yêu cầu phải điền thông tin
        $("input[type=text]").prop('required', false);
        $("select").prop('required', false);
    })

    //Nút Tiếp tục
    $(".content__continue-button").click(function () {
        $boolean = 0;
        $(".content__address-change-button").each(function () {
            if ($(this).is(':checked')) {
                $fullname = $(this).parent().parent().siblings(".content__fullname").text();
                $phonenum = $(this).parent().parent().siblings(".content__phonenum").text();
                $specificAddress = $(this).parent().parent().siblings(".content__address").children(".content__specificAddress").text();
                $ward = $(this).parent().parent().siblings(".content__address").children(".content__ward").text();
                $district = $(this).parent().parent().siblings(".content__address").children(".content__district").text();
                $city = $(this).parent().parent().siblings(".content__address").children(".content__city").text();

                sessionStorage.setItem("FULLNAME", $fullname);
                sessionStorage.setItem("PHONENUM", $phonenum);
                sessionStorage.setItem("SPECIFIC_ADDRESS", $specificAddress);
                sessionStorage.setItem("WARD", $ward);
                sessionStorage.setItem("DISTRICT", $district);
                sessionStorage.setItem("CITY", $city);

                $boolean = 1;
            }
        })
        //Kiểm tra nếu chọn thêm địa chỉ mới
        if ($boolean == 0) {
            var isFullfill = 1;
            if ($("#fullname").is(':invalid')) {
                $("#fullname-message").show();
                isFullfill = 0;
            }
            if ($("#phonenum").is(':invalid')) {
                $("#phonenum-message").show();
                isFullfill = 0;
            }
            if ($("#city").is(':invalid')) {
                $("#city-message").show();
                isFullfill = 0;
            }
            if ($("#district").is(':invalid')) {
                $("#district-message").show();
                isFullfill = 0;
            }
            if ($("#ward").is(':invalid')) {
                $("#ward-message").show();
                isFullfill = 0;
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
        }


    })

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

    //Bỏ thông báo yêu cầu điền/chọn
    $("#fullnameEdit").keyup(function () {
        $("#fullname-message-edit").hide();
    })
    $("#phonenumEdit").keyup(function () {
        $("#phonenum-message-edit").hide();
    })
    $("#cityEdit").change(function () {
        $("#city-message-edit").hide();
    })
    $("#districtEdit").change(function () {
        $("#district-message-edit").hide();
    })
    $("#wardEdit").change(function () {
        $("#ward-message-edit").hide();
    })
});