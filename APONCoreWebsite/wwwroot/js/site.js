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

function autoLoginComplete() {
    alert("Done");
}

$(document).ready(function () {

    var userID = document.getElementById("authVCuserID").value;
    console.log("authVCuserID: " + userID);
    //if the user is logged out, look for tokens from the browser
    if (userID == -1) {
        var myStorage = window.localStorage;
        var userJWTInfo = myStorage.getItem("userJWT");

        let jwt = JSON.parse(userJWTInfo);
        const expDate = Date.parse(jwt.expiration);
        console.log(expDate);
        if (userJWTInfo.length > 0) {
            $.ajax({
                type: "POST",
                url: 'AutoLogin/?handler=URT',
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
        else {
        
        }


    }
    //if not, don't do anything

});

function logoutRefresh() {
    var lii = document.getElementById("loginImage");
    lii.src = "http://media.allportsopen.org/images/siteImages/login.png";
    lii.classList.remove("loggedin-image");
    lii.classList.add("loggedout-image");
    var lil = document.getElementById("loginLink");
    lil.href = "/Login";
}
