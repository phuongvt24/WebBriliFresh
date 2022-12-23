const rangeInput = document.querySelectorAll(".range-input input"),
    priceInput = document.querySelectorAll(".price-input input"),
    range = document.querySelector(".slider .progress");
let priceGap = 10000;

priceInput.forEach((input) => {
    input.addEventListener("input", (e) => {
        let minPrice = parseInt(priceInput[0].value),
            maxPrice = parseInt(priceInput[1].value);

        if (maxPrice - minPrice >= priceGap && maxPrice <= rangeInput[1].max) {
            if (e.target.className === "input-min") {
                rangeInput[0].value = minPrice;
                range.style.left = (minPrice / rangeInput[0].max) * 100 + "%";
            } else {
                rangeInput[1].value = maxPrice;
                range.style.right = 100 - (maxPrice / rangeInput[1].max) * 100 + "%";
            }
        }
    });
});

rangeInput.forEach((input) => {
    input.addEventListener("input", (e) => {
        let minVal = parseInt(rangeInput[0].value),
            maxVal = parseInt(rangeInput[1].value);

        if (maxVal - minVal < priceGap) {
            if (e.target.className === "range-min") {
                //rangeInput[0].value = (maxVal - priceGap).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                rangeInput[0].value = (maxVal - priceGap);

            } else {
                //rangeInput[1].value = (minVal + priceGap).toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
                rangeInput[1].value = (minVal + priceGap);

            }
        } else {
            //priceInput[0].value = minVal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            //priceInput[1].value = maxVal.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");

            priceInput[0].value = minVal;
            priceInput[1].value = maxVal;

            range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
            range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";



        }
    });
});

let minVal = parseInt(rangeInput[0].value),
    maxVal = parseInt(rangeInput[1].value);

range.style.left = (minVal / rangeInput[0].max) * 100 + "%";
range.style.right = 100 - (maxVal / rangeInput[1].max) * 100 + "%";

$(document).ready(function () {
    toastr.options = {
        "debug": false,
        "positionClass": "toast-top-center",
        "onclick": null,
        "showDuration": "1000",
        "hideDuration": "1000",
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    };
    
    $(".product-item-add-btn").click(function () {
        /* $("#success").click();*/
        console.log("Phương")
        toastr.success("Sản phẩm vào giỏ hàng!!!");
        $.ajax({
            url: "/BuyAndPay/AddToCart",
            data: {
                proId: $(this).data("id"),
                storeid: $(this).data("storeid"),
                saleprice: $(this).data("saleprice"),
                type: "ajax"  
            },
            success: function (data) {
                $("#quantity_cart").html(data.quantity);
            }
        });
    });
});