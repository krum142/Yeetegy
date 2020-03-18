// When the user scrolls down 20px from the top of the document, show the button
var mybutton = document.getElementById("myBtn");
var isClicked = false;
var notLoggedIn = false;
window.onscroll = function () { scrollFunction() };

function scrollFunction() {
    if (document.body.scrollTop > 200 || document.documentElement.scrollTop > 200) {
        mybutton.style.display = "block";
    } else {
        mybutton.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}

function voteButton() {
    var x = event.target.getAttribute("id").split("_");
    var vote = x[0] === "Like" ? true : false;
    var id = x[1];
    var token = $("#voteform input[name=__RequestVerificationToken]").val();

    var voteUrl = "/api/Votes";
    var json = { postId: id, isUpVote: vote }
    request = new XMLHttpRequest();
    request.open('POST', voteUrl, /* async = */ false);
    request.setRequestHeader("X-CSRF-TOKEN", token);
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send(JSON.stringify(json));

    var code = request.status;
    var dooo = JSON.parse(request.response);

    console.log(dooo.status);

    if (code != 200) {
        if (code === 401) {
            window.location = "/Identity/Account/Login";
        } else {
            window.location = "/Home/HttpError?statusCode=" + code;
        }
    }
    /// UnLike,UnDislike,
    /// LikeToDislike,DislikeToLike.
    switch (dooo.status) {
        case "Like": case "Dislike":
            event.target.innerHTML = parseInt(event.target.innerHTML) + 1;
        break;
        case "UnLike": case"UnDislike":
            event.target.innerHTML = parseInt(event.target.innerHTML) - 1;
            break;
        case "LikeToDislike":
            event.target.innerHTML = parseInt(event.target.innerHTML) + 1;
            event.target.previousSibling.previousSibling.innerHTML =
                parseInt(event.target.previousSibling.previousSibling.innerHTML) - 1;
        break;
        case "DislikeToLike":
            event.target.innerHTML = parseInt(event.target.innerHTML) + 1;
            event.target.nextSibling.nextSibling.innerHTML =
                parseInt(event.target.nextSibling.nextSibling.innerHTML) - 1;
        break;
    }
}

function redirect() {
    var id = event.target.getAttribute("id");
    window.location = "/Posts/PostDetails?id=" + id;
}