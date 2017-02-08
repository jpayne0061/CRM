function initChat() {
    var chatPartner;
    var chatPartnerName;

    var chatSessionInterval;
    var chatMessagesInterval;
    var currentSessionIsActive = false;

    checkForCurrentSession();

    function updateScroll() {

        $("#js-messages-holder").animate({
            scrollTop: $("#js-messages-holder")[0].scrollHeight
        }, 200);

    }

    function getChatMessages() {

        //var chatPartner = $("#js-chat-partner").val();
        $.ajax({
            url: "/api/ChatMessagesApi/GetMessages/" + chatPartner,
            cache: false,
            success: function (html) {
                console.log(html);
                if (html.length == 0) {
                    return;
                }


                for (var i in html) {
                    //check for ending chat session(sending ID as body)
                    if (html[i]["Body"] == html[i]["ReceiverId"]) {
                        $("#js-messages-holder").append("<div id='end-chat-message'>" + "<i>" + chatPartnerName + " has ended this chat session" + "</i>" + "</div>");
                        currentSessionIsActive = false;
                        clearInterval(chatMessagesInterval);
                        return;
                    }


                    $("#js-messages-holder").append("<div>" + "<strong>" + chatPartnerName + ": " + "</strong>" + html[i]["Body"] + "</div>");
                    updateScroll();
                }



            }
        });
    }


    //this should run once at page load
    //if there are no currently active session,
    //then upon success, checkChatSession should be called
    function checkForCurrentSession() {
        $.ajax({
            url: "/api/ChatMessagesApi/GetChatSession",
            cache: false,
            success: function (listSession) {
                if (listSession.length == 0) {
                    console.log("no chat sessions!");
                    chatSessionInterval = setInterval(checkChatSession, 2500);
                    return;
                }
                else {

                    if (listSession[0]["SenderId"] == listSession[0]["RequestingUser"]) {
                        chatPartner = listSession[0]["ReceiverId"];
                        chatPartnerName = listSession[0]["RecipientName"];
                    }
                    else {
                        chatPartner = listSession[0]["SenderId"];
                        chatPartnerName = listSession[0]["RequesterName"];
                    }
                    currentSessionIsActive = true;
                    $("#chat-div").toggle();
                    $("#chat-header-name").text(chatPartnerName);
                    chatMessagesInterval = setInterval(getChatMessages, 1000);
                }

            }
        })
    }


    //checks whether your chat partner is in a chat session
    function checkPartnerForSession(message) {
        $.ajax({
            url: "/api/ChatMessagesApi/GetPartnerSession/" + chatPartner,
            cache: false,
            success: function (listSession) {
                if (listSession.length == 0) {
                    //go ahead, then
                    $.ajax({
                        type: "POST",
                        url: "/api/ChatMessagesApi/StartChatSession",
                        data: { ReceiverId: chatPartner }

                    })
                     .done(function () {
                         currentSessionIsActive = true;
                         chatMessagesInterval = setInterval(getChatMessages, 1000);
                         sendMessage(message);
                     }).fail(function () {
                         $("#js-messages-holder").append("<div>" + "Chat failed. Other user may now be in chat." + "</div>");
                         alert("Something Went Wrong!!!");
                     });


                    return;
                }
                else {
                    $("#js-messages-holder").append("<div>" + "Chat failed. " + chatPartnerName + " is now in a chat." + "</div>");
                    chatSessionInterval = setInterval(checkChatSession, 2500);
                    chatPartner = "";
                    chatPartnerName = "";
                }

            }
        })
    }



    //this only checks for receiving chats
    function checkChatSession() {
        $.ajax({
            url: "/api/ChatMessagesApi/GetChatSession",
            cache: false,
            success: function (listSession) {
                if (listSession.length == 0) {
                    console.log("checked chat session, none to be found");
                    return;
                }
                $("#chat-div").toggle();
                //stop checking sessions
                clearInterval(chatSessionInterval);
                console.log(listSession);
                //set partner ID to what is received
                chatPartner = listSession[0]["SenderId"];
                chatPartnerName = listSession[0]["RequesterName"];
                //start checking for messages
                chatMessagesInterval = setInterval(getChatMessages, 1000);
                currentSessionIsActive = true;
                $("#chat-header-name").text(chatPartnerName);

            }
        })
    }



    $(document.body).on("click", "#js-exit-chat", function () {

        if (!currentSessionIsActive) {
            
            $("#chat-div").toggle();
            $("#js-messages-div").empty();
            chatSessionInterval = setInterval(checkChatSession, 2500);
            return;
        }


        bootbox.confirm("Are you sure you want to end the chat?", function (result) {
            if (result) {

                $.ajax({
                    type: "POST",
                    url: "/api/ChatMessagesApi/PostMessage",
                    data: { ReceiverId: chatPartner, Body: chatPartner }

                })
                 .done(function () {
                     //$("#js-messages-holder").append("<div>" + "You have ended the chat session " + "</div>");
                     $.ajax({
                         url: "/api/ChatMessagesApi/EndChatSession",
                         method: "PATCH"
                     }).done(function () {
                         $("#js-messages-div").empty();
                         $("#chat-div").toggle();
                         clearInterval(chatMessagesInterval);
                         chatSessionInterval = setInterval(checkChatSession, 2500);
                         currentSessionIsActive = false;
                     })
                     //alert("sent!");
                 }).fail(function () {
                     alert("Something Went Wrong!!!");
                 });

            }
        })

    });


    //this selects user to chat with, clears chatSession interval
    //(stops checking for incoming chat requests) and shows chat box
    function beginChat() {
        clearInterval(chatSessionInterval);
        //var radioValue = $("input[name='gender']:checked").val();
        chatPartner = $("input[name='chat-partner']:checked").val();
        chatPartnerName = $("input[name='chat-partner']:checked").attr("chat-name");
        $("#chat-div").toggle();
        $("#chat-header-name").text(chatPartnerName);
    }

    //send first message?




    function sendMessage(message) {
        $.ajax({
            type: "POST",
            url: "/api/ChatMessagesApi/PostMessage",
            data: { ReceiverId: chatPartner, Body: message }

        })
        .done(function () {
            $("#js-messages-holder").append("<div>" + "<strong>" + "You: " + "</strong>" + message + "</div>");
            updateScroll();
            //alert("sent!");
        }).fail(function () {
            alert("Something Went Wrong!!!");
        });
    }


    function sendEvent() {
        var message = $("#js-chat-input").val();
        $("#js-chat-input").val("");

        if (currentSessionIsActive == false) {
            checkPartnerForSession(message);
        }
        else {

            console.log("message: ", message, "partner: ", chatPartner);
            sendMessage(message);
        }
    }



    $(document.body).on("click", "#js-begin-chat", beginChat);


    //this handles submission of message if user hits 'ENTER'
    $("#js-chat-input").keypress(function (event) {
        if (event.which == 13) {
            event.preventDefault();
            sendEvent();
        }
    });

    //this handles submission of message if user clicks 'SEND' button
    $(document.body).on("click", "#js-send-chat-message", function () {
        sendEvent();
    });
}