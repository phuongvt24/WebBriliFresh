$(document).ready(function () {
    var slider = function () {
        $(".slider").slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 3000,
            arrows: false,
        });
    };
    var slider_2 = function () {
        $(".slider-discounts").slick({
            slidesToShow: 3,
            slidesToScroll: 1,
            autoplay: true,
            autoplaySpeed: 3000,
            arrows: false,
        });
    };
    slider();
    slider_2();
});
