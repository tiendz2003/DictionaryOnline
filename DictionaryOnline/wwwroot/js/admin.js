$(document).ready(function () {
    // Toggle sidebar
    $(document).on('click', '.toggle-sidebar', function () {
        $('.sidebar').toggleClass('show');
        $('.main-content').toggleClass('push');
    });

    $(document).ready(function () {
        $("#searchForm").on("submit", function (e) {
            e.preventDefault();

            var formData = {
                fromLang: $("select[name='fromLang']").val(),
                toLang: $("select[name='toLang']").val(),
                term: $("#searchTerm").val()
            };

            $.ajax({
                url: window.searchUrl,
                type: 'GET',
                data: formData,
                success: function (result) {
                    $("#searchResults").html(result);
                },
                error: function () {
                    alert("Có lỗi xảy ra khi tra cứu.");
                }
            });
        });
    });

    // Add support for search with Enter key
    $('#searchTerm').on('keydown', function (e) {
        if (e.which === 13) { // Enter key code is 13
            $('#searchButton').click();
        }
    });

    // Language switcher
    $(document).on('click', '.language-switcher', function () {
        let sourceSelect = $(this).prev();
        let targetSelect = $(this).next();

        let sourceVal = sourceSelect.val();
        let targetVal = targetSelect.val();

        sourceSelect.val(targetVal);
        targetSelect.val(sourceVal);
    });
});