// Calls the Pin action and reloads the page to update the pinned status
$(document).ready(function () {
    $(".pinButton").click(function (e) {
        e.preventDefault();

        var itemId = $(this).data("id");

        $.ajax({
            type: "POST",
            url: "/Chatrooms/Pin/" + itemId,
            success: function (result) {
            
                console.log("Pin successful!");
                location.reload();
            },
            error: function (error) {
           
                console.log("Pin failed:", error);
            }
        });
    });
});