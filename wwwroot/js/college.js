$(function () {

    $(document).on('click', '#profile', function () {
        getUserProfile();
    });

    function getUserProfile() {
        $.ajax({
            type: "GET",
            url: "/UserProfile/Edit",
            data: '',
            contentType: "application/json; charset=utf-8",
            dataType: "html",
            success: function (response) {
                $("#exampleModal").find(".modal-body").html(response);
                $("#exampleModal").modal('show');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    }
});