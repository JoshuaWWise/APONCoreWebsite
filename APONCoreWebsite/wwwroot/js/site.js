// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

class JWT {
    constructor(username, userid, authlevel, token, expiration, theme, imageurl) {
        this.username = username;
        this.userid = userid;
        this.authlevel = authlevel;
        this.token = token;
        this.expiration = expiration;
        this.imageurl = imageurl;
        this.theme = theme;
    }
}

class SmallEpisode {
    constructor(epID, serID, title, date, webDesc, imgURL) {
        this.episodeID = epID;
        this.seriesID = serID;
        this.title = title;
        this.showDate = date;
        this.webDescription = webDesc;
        this.epImageURL = imgURL;
    }

}


function autoLoginComplete() {
   
}

$(document).ready(function () {


    var userID = document.getElementById("authVCuserID").value;

    //if the user is logged out, look for tokens from the browser
    if (userID == -1) {
        var myStorage = window.localStorage;
        var userJWTInfo = myStorage.getItem("userJWT");

        let jwt = JSON.parse(userJWTInfo);



        $.ajax({
            type: "POST",
            url: '/AutoLogin/?handler=URT',
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            contentType: "application/json; charset=utf-8",
            data: userJWTInfo
        });

        var lii = document.getElementById("loginImage");
        lii.src = jwt.imageurl;
        lii.classList.remove("loggedout-image");
        lii.classList.add("loggedin-image");
        var lil = document.getElementById("loginLink");
        lil.href = "/User?Id=" + jwt.userid;






    }
    //if not, don't do anything

});


function logoutFromSession() {
   //This function is used when a page returns an indication that the token has expired. 
    window.localStorage.removeItem("userJWT");
    $.ajax({
        type: "POST",
        url: '../../Login?handler=logout',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        contentType: "application/json; charset=utf-8",
        data: null
    });
    logoutRefresh();
    window.location.href = "/Login";
}

function logoutRefresh() {
    var lii = document.getElementById("loginImage");
    lii.src = "http://media.allportsopen.org/images/siteImages/login.png";
    lii.classList.remove("loggedin-image");
    lii.classList.add("loggedout-image");
    var lil = document.getElementById("loginLink");
    lil.href = "/Login";
}

function SwitchForumPostToEdit(fpID) {


    document.getElementById("editMode" + fpID).value = true;
    document.getElementById("forumPostText" + fpID).style.visibility = "hidden";
    document.getElementById("forumPostEditor" + fpID).style.visibility = "visible";
    document.getElementById("btnSwitchToForumPostEdit" + fpID).style.visibility = "hidden";
    document.getElementById("btnSaveForumPost" + fpID).style.visibility = "visible";

    var textAreaName = "ForumPostTextEditorTextArea" + fpID;

    var myTinyMCE = new tinymce.Editor(textAreaName, {
        selector: textAreaName,
        width: 700, height: 200,
        menubar: false,
        toolbar: 'undo redo | bold italic underline strikethrough |  image media  link   | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print  | ltr rtl',
        toolbar_sticky: true,
        plugins: 'image link media'
    }, tinymce.EditorManager);
    myTinyMCE.render();



}

function RemoveTinyMCE(fpID) {
    var textAreaName = "#ForumPostTextEditorTextArea" + fpID;
    tinymce.remove(textAreaName);


}

function updateImg(senderObjectID, targetObjectID) {
    console.log("target: " + targetObjectID + ": " + document.getElementById(targetObjectID).src);
    console.log(document.getElementById(senderObjectID).value);
    document.getElementById(targetObjectID).src = document.getElementById(senderObjectID).value;
}

function submitImage(evt, formdata, targetFolder, InputUpdateField, ImageUpdateField, filename) {
    var handler = "NewsImage";
    var imageURL = "https://media.allportsopen.org/images/";
    let url = "";
    console.log(evt);
    console.log(targetFolder);
    console.log(filename);
    let fd = new FormData;
    fd = formdata;


    switch (targetFolder) {
        case "news":
            imageURL += "News/" + filename;
            url = '../../ImageHandler?handler=' + handler;
            fd.append("imageType", "news");
            break;
        case "episode":
            imageURL += "EpImages/" + filename;
            url = '../../ImageHandler?handler=' + handler;
            fd.append("imageType", "episode");
            break;
    }

    evt.preventDefault();
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url: url,
        data: formdata,
        contentType: false,
        processData: false,

        success: function (resp) {

            if (resp == "") {
              
                document.getElementById(InputUpdateField).innerHTML = imageURL;
                document.getElementById(InputUpdateField).value = imageURL;
                document.getElementById(ImageUpdateField).src = imageURL;

            }
            else {
                alert("There was a problem uploading the image.");
            }
        }
    });

   
}

function SubmitEpisodeForm() {
    let FD = new FormData();
    FD = document.getElementById("mainForm");

    FD.append('seriesID', document.getElementById("epSeriesID").value);

    FD.append("File", document.getElementById("episodeUploadInput").files[0])
    let url = window.location.protocol + "//" + window.location.host + "/Handlers/SaveHandler?handler=NewEpisode";
    let sendFD = $(FD).serialize();
    $.ajax({
        type: "POST",
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN", $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        url: url,
        data: sendFD,
        contentType: false,
        processData: false,

        success: function (resp) {

            alert(resp);
        }
    });

}


