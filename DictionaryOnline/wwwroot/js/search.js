$(document).ready(function () {
    $("#seachForm").on('submit', function (e) {
        e.preventDefault();
        var form = $(this);

        $.ajax({
            url: form.attr('action'),
            method: 'GET',
            data: form.serialize(),
            success: function (response) {
                $('.search-results').html(response);
            },
            error: function () {
                $('.search-results').html('<div class="alert alert-danger">Có lỗi xảy ra khi tìm kiếm. Vui lòng thử lại.</div>');
            }
        });
    });
});