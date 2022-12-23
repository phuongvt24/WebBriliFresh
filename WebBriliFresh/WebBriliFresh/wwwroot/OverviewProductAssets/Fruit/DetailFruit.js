$(document).ready(function () {
    $('.quantity-right-plus').click(function (e) {

        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());

        // If is not undefined

        $('#quantity').val(quantity + 1);


        // Increment

    });

    $('.quantity-left-minus').click(function (e) {
        // Stop acting like a button
        e.preventDefault();
        // Get the field name
        var quantity = parseInt($('#quantity').val());

        // If is not undefined

        // Increment
        if (quantity > 0) {
            $('#quantity').val(quantity - 1);
        }
    });
    $(".description .more-desc").click(function () {
        $(".product").css("height", "auto");
        $(this).css("display", "none");
        $(".description-body").css("display", "inline");

        // $(".description p:last-child").append("<button class="+"less-desc>"+" Rút gọn</button>");
        $(".description .less-desc").css("display", "inline");
        $('html, body').animate({
            scrollTop: parseInt($(".description").offset().top)
        }, 100);
    })
    $(".description .less-desc").click(function () {
        $(".description-body").css("display", "block");
        $(".product").css("height", "515px");
        $(this).css("display", "none");
        $(".description .more-desc").css("display", "inline");
        $('html, body').animate({
            scrollTop: parseInt($(".product-info").offset().top)
        }, 100);

    })
});
