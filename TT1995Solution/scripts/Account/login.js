﻿$("#btnLogin").click(function () {
    console.log("test");
    var strUsername = document.getElementById('txtUsername').value;
    var strPassword = document.getElementById('txtPassword').value;
    $.ajax({
        type: "POST",
        url: "../Account/CheckLogin",
        contentType: "application/json; charset=utf-8",
        data: "{Username:'" + strUsername + "', Password:'" + strPassword + "'}",
        dataType: "json",
        success: function (data) {
            document.getElementById('txtUsername').value = '';
            document.getElementById('txtPassword').value = '';
            if (data != '') {
                window.location.href = '../Home/Index';
            } else {
                document.getElementById('lbError').innerHTML = "Please check the information."
            }
        }, error: function (xhr, status, error) {
            
        }
    });
});