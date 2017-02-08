var ApiService = function () {
    var checkForCurrentSession = function (callback) {
        $.ajax({
            url: "/api/ChatMessagesApi/GetChatSession",
            cache: false,
            success: function (listSession) { callback(listSession) }
            //all this stuff below needs to go in a separate function
        })
    }


    var getChatMessages = function (handleData, chatPartner) {
        $.ajax({
            url: "/api/ChatMessagesApi/GetMessages/" + chatPartner,
            cache: false
        })
         .done(function (messageArray) {
             handleData(messageArray)
         }).fail(function () {
             fail();
         });
    }



    var createChatSession = function (message, success, fail, chatPartner) {
        $.ajax({
            type: "POST",
            url: "/api/ChatMessagesApi/StartChatSession",
            data: { ReceiverId: chatPartner }

        })
         .done(function () {
             success(message);
         }).fail(function () {
             fail();
         });
    }

    var checkPartnerForSession = function (message, handleData, chatPartner) {
        $.ajax({
            url: "/api/ChatMessagesApi/GetPartnerSession/" + chatPartner,
            cache: false
        })
        .done(function (listSession) {
            handleData(listSession, message);
        }).fail(function () {
            fail();
        });
    }

    var endChatSession = function (done) {
        $.ajax({
            url: "/api/ChatMessagesApi/EndChatSession",
            method: "PATCH"
        }).done(done)
    }

    var sendMessage = function (message, handleData, chatPartner) {
        $.ajax({
            type: "POST",
            url: "/api/ChatMessagesApi/PostMessage",
            data: { ReceiverId: chatPartner, Body: message }

        })
        .done(handleData)
        .fail(function () {
          alert("Something Went Wrong!!!");
        });
    }

    return {
        sendMessage: sendMessage,
        endChatSession: endChatSession,
        checkPartnerForSession: checkPartnerForSession,
        createChatSession: createChatSession,
        getChatMessages: getChatMessages,
        checkForCurrentSession : checkForCurrentSession
    }

}();


var ChatController = function (apiService) {

    var chatPartner;
    var chatPartnerName;

    var chatSessionInterval;
    var chatMessagesInterval;
    var currentSessionIsActive = false;


    //DOM EVENT
    var onClickSend = function () {
        $(document.body).on("click", "#js-send-chat-message", function () {
            sendEvent();
        });
    }

    //DOM EVENT
    var onEnter = function () {
        $("#js-chat-input").keypress(function (event) {
            if (event.which == 13) {
                event.preventDefault();
                sendEvent();
            }
        });
    }

    //DOM EVENT
    var onClickBeginChat = function () {
        $(document.body).on("click", "#js-begin-chat", beginChat);
    }


    //DOM EVENT
    var exitChat = function () {
        $(document.body).on("click", "#js-exit-chat", function () {

            if (!currentSessionIsActive) {
                $("#js-messages-div").empty();
                $("#chat-div").toggle();
                chatSessionInterval = setInterval(function () { apiService.checkForCurrentSession(handleChatSession) }, 2500);
                //chatSessionInterval = setInterval(checkChatSession, 2500);
                return;
            }


            bootbox.confirm("Are you sure you want to end the chat?", function (result) {
                if (result) {
                    //sendTerminalMessage();
                    apiService.sendMessage(chatPartner, function () { apiService.endChatSession(reset) }, chatPartner)
                }
            })

        });
    }


    //UI & STATE
    var reset = function () {
        $("#js-messages-div").empty();
        $("#chat-div").toggle();
        clearInterval(chatMessagesInterval);
        chatSessionInterval = setInterval(function () { apiService.checkForCurrentSession(handleChatSession) }, 2500);
        currentSessionIsActive = false;
    }

    //EVENT
    var sendEvent = function () {
        var message = $("#js-chat-input").val();
        $("#js-chat-input").val("");

        if (currentSessionIsActive == false) {
            apiService.checkPartnerForSession(message, handlePartnerSessionQuery, chatPartner);
        }
        else {
            apiService.sendMessage(message, function () { appendMessageInUi("You", message) }, chatPartner);
        }
    }


    // UI AND STATE
    var beginChat = function () {
        clearInterval(chatSessionInterval);
        chatPartner = $("input[name='chat-partner']:checked").val();
        chatPartnerName = $("input[name='chat-partner']:checked").attr("chat-name");
        $("#chat-div").toggle();//ONE
        $("#js-chat-input").focus();
        $("#chat-header-name").text(chatPartnerName);
    }

    var createSessionSuccess = function (message) {
        currentSessionIsActive = true;
        chatMessagesInterval = setInterval(function () { apiService.getChatMessages(handleMessageReceived, chatPartner) }, 1000);
        //chatMessagesInterval = setInterval(getChatMessages, 1000);
        apiService.sendMessage(message, function () { appendMessageInUi ("You", message)}, chatPartner);
    }

    var handlePartnerSessionQuery = function (listSession, message) {
        console.log("here here!!!!!", message);
        if (listSession.length == 0) {
            apiService.createChatSession(message, createSessionSuccess, function () { appendMessageInUi("", "Chat failed. Other user may now be in chat.") }, chatPartner);
            return;
        }
        else {
            appendMessageInUi("", "Chat failed. Other user may now be in chat.")
            chatSessionInterval = setInterval(checkChatSession, 2500);
            chatPartner = "";
            chatPartnerName = "";
        }
    }

    var appendMessageInUi = function (author, message) {
        $("#js-messages-holder").append("<div>" + "<strong>" + author + ": " +"</strong>" + message + "</div>");
        $("#js-messages-holder").animate({
            scrollTop: $("#js-messages-holder")[0].scrollHeight
        }, 200);
    }

    var endedChatMessage = function () {
        $("#js-messages-holder").append("<div id='end-chat-message'>" + "<i>" + chatPartnerName + " has ended this chat session" + "</i>" + "</div>");
        $("#js-messages-holder").animate({
            scrollTop: $("#js-messages-holder")[0].scrollHeight
        }, 200);
    }

    var handleMessageReceived = function (messageArray) {
        if (messageArray.length == 0) {
            return;
        }

        for (var i in messageArray) {
            var message = messageArray[i]["Body"];
            //check for ending chat session(sending ID as body)
            if (message == messageArray[i]["ReceiverId"]) {
                endedChatMessage();
                currentSessionIsActive = false;
                clearInterval(chatMessagesInterval);
                return;
            }

            appendMessageInUi(chatPartnerName, message);
        }
    }



    var handleChatSession = function (listSession) {
        if (listSession.length == 0) {
            return;
        }
        $("#chat-div").toggle();//TWO
        $("#js-chat-input").focus();
        //stop checking sessions
        clearInterval(chatSessionInterval);
        //set partner ID to what is received
        chatPartner = listSession[0]["SenderId"];
        chatPartnerName = listSession[0]["RequesterName"];
        //start checking for messages
        chatMessagesInterval = setInterval(function () { apiService.getChatMessages(handleMessageReceived, chatPartner) }, 1000);
        //chatMessagesInterval = setInterval(getChatMessages, 1000);
        currentSessionIsActive = true;
        $("#chat-header-name").text(chatPartnerName);
    }

    //if you receive listSession with length > 0
    var receiveChatSession = function (listSession) {
        if (listSession.length == 0) {
            chatSessionInterval = setInterval(function () { apiService.checkForCurrentSession(handleChatSession) }, 2500);
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
            $("#js-chat-input").focus();
            $("#chat-header-name").text(chatPartnerName);
            chatMessagesInterval = setInterval(function () { apiService.getChatMessages(handleMessageReceived, chatPartner) }, 1000);
        }
    }


    var init = function () {

        apiService.checkForCurrentSession(receiveChatSession);

        // ONCLICK EXIT CHAT
        exitChat();

        //ON CLICK BEGIN CHAT
        onClickBeginChat();

        //this handles submission of message if user hits 'ENTER'
        //ON ENTER SEND MESSAGE
        onEnter();

        //this handles submission of message if user clicks 'SEND' button
        //ON CLICK SEND MESSAGE
        onClickSend();
    }

    return {
        init: init
    }

}(ApiService);