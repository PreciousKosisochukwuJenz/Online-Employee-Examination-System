let SessionExist;
function initFavs() {
    // Handle Favorites
    var items = document.getElementsByClassName('product_fav');
    for (var x = 0; x < items.length; x++) {
        var item = items[x];
        item.addEventListener('click', function (fn) {
            CheckIfSessionExists();
            if (SessionExist == true) {
                fn.target.classList.toggle('active');
                var id = fn.target.id;
            }
           
        });
    }
}
function initFavBooks() {
    // Handle Favorites
    var items = document.getElementsByClassName('book_fav');
    for (var x = 0; x < items.length; x++) {
        var item = items[x];
        item.addEventListener('click', function (fn) {
            CheckIfSessionExists();
            if (SessionExist == true) {
                fn.target.classList.toggle('active');
                var id = fn.target.id;
            }

        });
    }
}function initBanner2Slider() {
    if ($('.banner_2_slider').length) {
        var banner2Slider = $('.banner_2_slider');
        banner2Slider.owlCarousel(
            {
                items: 1,
                loop: true,
                nav: false,
                dots: true,
                dotsContainer: '.banner_2_dots',
                smartSpeed: 1200
            });
    }
}

function CheckIfSessionExists() {
    $.ajax({
        url:"/Account/CheckSessionExists",
        type: "Get",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            SessionExist = result;
            if (result == false) {
                url = "/Account/Login";
                SwalAlert(url, true);
            }
            else {
            }
        }
    })
}

function SwalAlert(optionalUrl, isToRedirect) {
    Swal.fire({
        text: "You can\'t access this feature because you ain\'t login yet ,Do you want to login now?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, go to login!'
    }).then((result) => {
        if (result.value) {
            if (isToRedirect == true) {
                window.location.href= optionalUrl;
            }
            else {

            }
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'error'
            )
        }
    })
}

function initQuantity() {
    // Handle product quantity input
    if ($('.product_quantity').length) {
        var input = $('#quantity_input');
        var incButton = $('#quantity_inc_button');
        var decButton = $('#quantity_dec_button');

        var originalVal;
        var endVal;

        incButton.on('click', function () {
            originalVal = input.val();
            endVal = parseFloat(originalVal) + 1;
            input.val(endVal);
        });

        decButton.on('click', function () {
            originalVal = input.val();
            if (originalVal > 0) {
                endVal = parseFloat(originalVal) - 1;
                input.val(endVal);
            }
        });
    }
}