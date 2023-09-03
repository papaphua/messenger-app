function addMessageToChat(username, content, timestamp, profilePictureBytes) {
    const messagesContainer = document.getElementById("messagesContainer");
    const messageElement = document.createElement("div");
    messageElement.className = "card mt-3";
    messageElement.innerHTML = `
        <div class="card-body">
            <div class="row">
                <div class="col">
                    <h5 class="card-title">Message</h5>
                    <p class="card-text">
                        <strong>Sender:</strong> ${username}
                    </p>
                    <p class="card-text">
                        <strong>Content:</strong> ${content}
                    </p>
                    <p class="card-text">
                        <strong>Timestamp:</strong> ${timestamp}
                    </p>
                </div>
                <div class="col">
                    ${profilePictureBytes
        ? `<img class="profile-picture" src="data:image/jpeg;base64,${profilePictureBytes}" alt="Profile Picture"/>`
        : '<div class="empty-circle"></div>'}
                </div>
            </div>
        </div>
    `;

    messagesContainer.prepend(messageElement);
}

function formatDateToCustomString(date) {
    const year = date.getUTCFullYear();
    const month = String(date.getUTCMonth() + 1).padStart(2, '0'); // Month is 0-indexed, so add 1 and pad with leading zero.
    const day = String(date.getUTCDate()).padStart(2, '0');
    const hours = String(date.getUTCHours()).padStart(2, '0');
    const minutes = String(date.getUTCMinutes()).padStart(2, '0');
    const seconds = String(date.getUTCSeconds()).padStart(2, '0');

    return `${day}/${month}/${year} ${hours}:${minutes}:${seconds}`;
}