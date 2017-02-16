var JoinRequestsService = function () {
    var deny = function (success, fail, id) {
        $.ajax({
            url: "/api/JoinRequestApi/" + id,
            method: "DELETE"
        })
         .done(success)
         .fail(fail);
    }

    var accept = function (success, fail, id) {
        $.post("/api/JoinRequestApi/Accept", { "": id })
         .done(success)
         .fail(fail);
    }

    return {
        deny: deny,
        accept : accept
    }

}();

var JoinRequestsController = function (joinRequestsService) {

    var success = function (num) {
        num.parent().parent("tr").fadeOut(function () {
            $(this).remove();
        });
       
        $("tr[js-data-remove='" + num.attr("js-data-remove") + "']").fadeOut(function () {
            $(this).remove();
        });
    }

    var fail = function () {
        alert("something went wrong!");
    }

    var onClickDeny = function () {
        $(document.body).on("click", ".deny-request", function (e) {
            var num = $(e.target);
            bootbox.confirm("Are you sure you want to deny this request?", function (result) {
                if (result) {
                    joinRequestsService.deny(function () { success(num) }, fail, num.attr("js-data-id"))
                }

            });


        });
    }


    var onClickAccept = function () {
        $(document.body).on("click", ".accept-request", function (e) {
            var num = $(e.target);
            console.log(num.attr("js-data-remove"));
            bootbox.confirm("Are you sure you want to accept this request?", function (result) {
                if (result) {
                    joinRequestsService.accept(function () { success(num) }, fail, num.attr("js-data-id"))
                }

            });


        });
    }

    var init = function () {
        onClickDeny();
        onClickAccept();
    }

    return {
        init: init
    }

}(JoinRequestsService);

