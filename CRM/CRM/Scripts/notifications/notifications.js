
var NotificationsApiService = function(){
    var getNotifications = function (handleGetNotifications) {
        $.getJSON("/api/UserNotificationApi/GetNotifications", function (notifications) { handleGetNotifications(notifications) })
    }

    var deleteNotifications = function (handleNotificationsDelete, failed) {
        $.ajax({
            url: "/api/UserNotificationApi/DeleteAll",
            method: "DELETE"
        })
         .done(handleNotificationsDelete)
         .fail(failed);
    }

    return {
        getNotifications: getNotifications,
        deleteNotifications: deleteNotifications
    }


}();




var NotificationsController = function (notificationApiService) {
    var handleGetNotifications = function (notifications) {
        if (notifications.length == 0) {
            return;
        }

        $(".js-notification-count").text(notifications.length).removeClass("hide").addClass("animated bounce");

        //NOTIFICATIONS POPOVER
        $(".notifications").popover({
            html: true,
            title: "Notifications",
            content: function () {
                var compiled = _.template($("#notifications-template").html());

                return compiled({ notifications: notifications });

            },
            placement: "bottom"

        });
    }

    var handleNotificationsDelete = function () {
        //alert("Deleted!");
        $(".js-notification-count").addClass("hide");
        $(".popover-content ul li").fadeOut(function () {
            $(".popover").hide();
        });
    }

    var failed = function () {
        alert("Something Went Wrong!!!");
    }

    var hidePopover = function () {
        $('body').on('click', function (e) {
            $('[data-original-title]').each(function () {
                //the 'is' for buttons that trigger popups
                //the 'has' for icons within a button that triggers a popup
                if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                    $(this).popover('hide');

                }
            });
        });
    }

    var deleteNotifications = function () {

        $(document.body).on("click", "#mark-as-read", function () { notificationApiService.deleteNotifications(handleNotificationsDelete, failed) });
    }

    var init = function () {
        notificationApiService.getNotifications(handleGetNotifications);

        deleteNotifications();

        hidePopover();

    }


    return {
        init: init
    }

}(NotificationsApiService);






