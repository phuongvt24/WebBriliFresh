const ratingBtns = document.querySelectorAll('.js-rating-btn')
const modal =  document.querySelector('.js-modal')
const modalClose = document.querySelector('.js-modal-close')
const modalContainer = document.querySelector('.js-modal-container')

var orderId = document.createElement('input')
var proId = document.createElement('input')

function showModal(OrderId, ProId) {

    modal.classList.add('open')
    orderId.value = OrderId
    orderId.name = "OrderId"
    orderId.type = "number"
    orderId.setAttribute("hidden", true)
    modalContainer.appendChild(orderId)

    proId.value = ProId
    proId.name = "ProId"
    proId.type = "number"
    proId.setAttribute("hidden", true)
    modalContainer.appendChild(proId)
}

function hideModal() {
    modalContainer.removeChild(proId)
    modalContainer.removeChild(orderId)
    modal.classList.remove('open')

}

modalClose.addEventListener('click', hideModal);


modalContainer.addEventListener('click', function (event) {
    event.stopPropagation()     // ngăn chặn việc nổi bọt
});
