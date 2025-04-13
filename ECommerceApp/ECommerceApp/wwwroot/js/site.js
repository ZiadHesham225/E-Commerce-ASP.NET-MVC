// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function refreshCartCount() {
    if (document.querySelector("#cart-count")) {
        $.ajax({
            url: '/Cart/GetCartCount',
            type: 'GET',
            success: function (data) {
                const cartCountElement = document.querySelector("#cart-count");
                if (data.count > 0) {
                    cartCountElement.innerText = data.count;
                    cartCountElement.classList.remove('d-none');
                } else {
                    cartCountElement.classList.add('d-none');
                }
            }
        });
    }
}
