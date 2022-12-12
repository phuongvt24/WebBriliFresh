const ratingBtns = document.querySelectorAll('.js-rating-btn')
const modal =  document.querySelector('.js-modal')
const modalClose = document.querySelector('.js-modal-close')
const modalContainer = document.querySelector('.js-modal-container')

function showModal() {
    modal.classList.add('open')
}

function hideModal() {
    modal.classList.remove('open')
}

modalClose.addEventListener('click', hideModal);

// Lặp qua từng button và nghe sự kiện click
for (const ratingBtn of ratingBtns) {
    ratingBtn.addEventListener('click', showModal)
}

modalContainer.addEventListener('click', function (event) {
    event.stopPropagation()     // ngăn chặn việc nổi bọt
});
