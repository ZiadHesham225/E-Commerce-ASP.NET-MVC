$(document).ready(function () {
    loadProducts();

    $('#searchForm').submit(function (e) {
        e.preventDefault();
        searchProducts();
    });

    $('#sortSelect').change(function () {
        sortProducts();
    });

    $('#filterBtn').click(function () {
        $('.filter-box').toggle();
    });

    $('#applyFilterBtn').click(function () {
        filterProducts();
    });
});

function loadProducts(page = 1) {
    $.ajax({
        url: `/Home/GetProducts?page=${page}`,
        success: function (result) {
            displayProducts(result);
        }
    });
}

function searchProducts(page = 1) {
    var query = $('#searchInput').val();
    $.ajax({
        url: `/Home/Search?query=${query}&page=${page}`,
        success: function (result) {
            displayProducts(result);
        }
    });
}

function sortProducts(page = 1) {
    var sortBy = $('#sortSelect').val();
    $.ajax({
        url: `/Home/Sort?sortBy=${sortBy}&page=${page}`,
        success: function (result) {
            displayProducts(result);
        }
    });
}

function filterProducts(page = 1) {
    var category = $('input[name="categoryRadio"]:checked').val();
    var minPrice = $('#minPrice').val();
    var maxPrice = $('#maxPrice').val();
    var inStock = $('#inStockCheckbox').is(':checked');

    $.ajax({
        url: `/Home/Filter?category=${category}&minPrice=${minPrice}&maxPrice=${maxPrice}&inStock=${inStock}&page=${page}`,
        success: function (result) {
            displayProducts(result);
        }
    });
}

function displayProducts(result) {
    var productGrid = $('.product-grid');
    productGrid.html('');

    result.products.forEach(product => {
        productGrid.append(`
            <div class="product-card">
                <h3>${product.name}</h3>
                <p>Price: $${product.price}</p>
            </div>
        `);
    });

    setupPagination(result.totalPages, result.currentPage);
}

function setupPagination(totalPages, currentPage) {
    var pagination = $('.pagination');
    pagination.html('');

    for (let i = 1; i <= totalPages; i++) {
        pagination.append(`
            <button class="page-btn ${i === currentPage ? 'active' : ''}" onclick="loadProducts(${i})">${i}</button>
        `);
    }
}
