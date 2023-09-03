let connection = new signalR.HubConnectionBuilder()
    .withUrl("/directHub")
    .build();

const userDataElement = document.getElementById("userData");
const directId = document.getElementById("userData").getAttribute("data-directId");

connection.on("ReceiveMessage", (username, content, timestamp, profilePictureBytes) =>
    addMessageToChat(username, content, timestamp, profilePictureBytes));

document.addEventListener("DOMContentLoaded", function () {
    const messageForm = document.getElementById("messageForm");
    const contentTextarea = messageForm.querySelector("#content");

    const username = userDataElement.getAttribute("data-username");
    const profilePictureBytes = userDataElement.getAttribute("data-profilepicture");

    const currentUTCDate = new Date(Date.UTC(
        new Date().getUTCFullYear(),
        new Date().getUTCMonth(),
        new Date().getUTCDate(),
        new Date().getUTCHours(),
        new Date().getUTCMinutes(),
        new Date().getUTCSeconds()
    ));

    const customSubmitButton = document.getElementById("sendMessageButton");
    customSubmitButton.addEventListener("click", function (e) {
        const messageContent = contentTextarea.value.trim();
        if (messageContent) {
            connection.invoke("SendMessage", username, messageContent, formatDateToCustomString(currentUTCDate), profilePictureBytes, directId).catch(function (err) {
                console.error(err);
            });

            messageForm.submit();
        }
    });
});


connection.start().then(() => {
    connection.invoke("JoinDirect", directId)
});