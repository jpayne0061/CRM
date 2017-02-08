var DeleteService = function () {
    var deleteTaskService = function (success, fail, id) {
        $.ajax({
            url: "/api/TaskApi/Delete/" + id,
            method: "DELETE"
        })
         .done(success)
         .fail(fail);
    }

    var removeTeamMemberService = function (success, fail, custId, memberId) {
        $.ajax({
            type: "POST",
            url: "/api/CustomerApi/DeleteTeamMember",
            data: { CustomerId: custId, TeamMemberId: memberId }
        })
         .done(success)
         .fail(fail);
    }

    var deleteMessageService = function (success, fail, id) {
        $.ajax({
            url: "/api/MessagesApi/Delete/" + id,
            method: "DELETE"
        })
         .done(success)
         .fail(fail);
    }

    var deleteCustomerService = function (success, fail, id) {
        $.ajax({
            url: "/api/CustomerApi/Delete/" + id,
            method: "DELETE"
        })
         .done(success)
         .fail(fail);
    }

    var markTaskComplete = function (success, fail, id) {
        $.ajax({
            url: "/api/TaskApi/MarkComplete/" + id,
            method: "PUT"
        })
         .done(success)
         .fail(fail);
    }

    return {
        deleteMessageService: deleteMessageService,
        deleteTaskService: deleteTaskService,
        removeTeamMemberService: removeTeamMemberService,
        deleteCustomerService: deleteCustomerService,
        markTaskComplete : markTaskComplete
    }
}();



var DeleteController = function (deleteService) {

    var deleteSuccess = function (num) {
        num.parent().parent("tr").fadeOut(function () {
            $(this).remove();
        })
    }

    var markCompleteSuccess = function (num) {
        num.parent().parent("tr").addClass("deactivated").children(".completed").html("<div class='green'>COMPLETED</div>");
    }

    var fail = function () {
        alert("Something Went Wrong!!!");
    }

    var removeTeamMemberSuccess = function (num) {
        num.parent().fadeOut(function () {
            $(this).remove();
        })
    }

    var onClickMarkTaskComplete = function () {
        $(document.body).on("click", ".js-mark-task-complete", function (e) {
            var num = $(e.target);
            bootbox.confirm("Are you sure you want to mark this task complete?", function (result) {
                if (result) {
                    deleteService.markTaskComplete(function () { markCompleteSuccess(num) }, fail, num.attr("js-data-id"));
                }

            });

        });
    }

    var onClickDeleteTask = function () {
        $(document.body).on("click", ".js-delete-task", function (e) {
            var num = $(e.target);
            bootbox.confirm("Are you sure you want to delete this task?", function (result) {
                if (result) {
                    deleteService.deleteTaskService(function () { deleteSuccess(num) }, fail, num.attr("js-data-id"));
                }

            });

        });
    }


    var onClickRemoveTeamMember = function () {
        $(document.body).on("click", ".js-glyphicon-remove-member", function (e) {
            var num = $(e.target);
            var custId = $(e.target).attr("js-cust-id");
            bootbox.confirm("Are you sure you want to remove " + $(e.target).parent().text() + " from the team?", function (result) {
                if (result) {
                    deleteService.removeTeamMemberService(function () { removeTeamMemberSuccess(num) }, fail, custId, num.attr("js-data-id"));
                }

            });


        });
    }



    var onClickDeleteMessage = function () {
        $(document.body).on("click", ".js-delete-message", function (e) {
            var num = $(e.target);
            bootbox.confirm("Are you sure you want to delete this message?", function (result) {
                if (result) {
                    deleteService.deleteMessageService(function () { deleteSuccess(num) }, fail, num.attr("js-data-id"));
                }

            });

        });
    }

    var onClickDeleteCustomer = function () {
        $(document.body).on("click", ".delete-customer", function (e) {
            var num = $(e.target);
            bootbox.confirm("Are you sure you want to delete this customer?", function (result) {
                if (result) {
                    deleteService.deleteCustomerService(function () { deleteSuccess(num) }, fail, num.attr("js-data-id"));
                }

            });
        });
    }






    var init = function () {
        onClickDeleteMessage();
        onClickDeleteTask();
        onClickRemoveTeamMember();
        onClickDeleteCustomer();
        onClickMarkTaskComplete();
    }

    return {
        init : init

    }

}(DeleteService);


