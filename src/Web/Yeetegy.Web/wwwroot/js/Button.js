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
    var postsId = event.target.getAttribute("id").split("_");
    var typeVote = postsId[0]; /*types Of Votes - Post, Comment, Replay. Example - if you wanna upvote a comment the typeVote will be comment*/
    var vote = postsId[1] === "Like" ? true : false; /*vote type dislike or like*/
    var id = postsId[2]; /*the id of the post/comment/replay */
    /*gets a secret token generated in a  invisible from so aspnet knows the request is valid*/
    var token = $("#voteform input[name=__RequestVerificationToken]").val(); 

    var voteUrl = "/api/Votes/" + typeVote; /*request is made to this address*/

    var keyid = typeVote.toLowerCase() + "Id";
    var json = { isUpVote: vote }
    json[keyid] = id;

    var request = new XMLHttpRequest();
    request.open('POST', voteUrl, /* async = */ false);
    request.setRequestHeader("X-CSRF-TOKEN", token);
    request.setRequestHeader("Content-Type", "application/json;charset=UTF-8");
    request.send(JSON.stringify(json));

    var code = request.status;
    var myResponse = JSON.parse(request.response);

    if (code != 200) {
        if (code === 401) {
            window.location = "/Identity/Account/Login";
        } else {
            window.location = "/Home/HttpError?statusCode=" + code;
        }
    }
    /// UnLike,UnDislike,
    /// LikeToDislike,DislikeToLike
    switch (myResponse.status) {
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
    window.open("/Posts/PostDetails?id=" + id);
}