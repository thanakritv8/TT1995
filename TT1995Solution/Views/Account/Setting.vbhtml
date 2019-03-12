@Code
    ViewData("Title") = "Setting"
End Code

<div class="row">
    <div class="col">
        <div class="card mb-2">
            <div class="card-header">
                Profile Setting
            </div>
            <div class="card-body">
                <div class="row mb-2">
                    <div class="col-sm">
                        Name
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" style="background-color: #FCFBC8" id="name" placeholder="name" value="" required>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        Department
                    </div>
                    <div class="col-sm">
                        Sections
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" style="background-color: #FCFBC8" id="department" placeholder="department" value="" required>
                    </div>
                    <div class="col-sm">
                        <input type="text" class="form-control" style="background-color: #FCFBC8" id="sections" placeholder="sections" value="" required>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        Email
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" style="background-color: #FCFBC8" id="email" placeholder="email" value="" required>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        Username
                    </div>
                    <div class="col-sm">
                        Password
                    </div>

                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" style="background-color: #FCFBC8" id="username" placeholder="username" value="" required>
                    </div>
                    <div class="col-sm">
                        <input type="password" class="form-control" style="background-color: #FCFBC8" id="password" placeholder="password" value="" required>
                    </div>

                </div>
            </div>
            <div class="card-footer text-muted text-right">
                <button type="button" id="btnEdit" class="btn btn-success ml-auto">Save</button>
                @*<button type="button" class="btn btn-danger ml-auto">Clear</button>*@
            </div>
        </div>
    </div>
</div>
<!--Modal Alert Show-->
<div class="modal" id="AlertShow">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content text-center">

            <!-- Modal Header -->
            <div class="modal-header">
                <h4 class="modal-title">Information</h4>
                @*<button type="button" class="close" data-dismiss="modal">&times;</button>*@
            </div>

            <!-- Modal body -->
            <div class="modal-body">
                <label id="idAlert">Recorded file success</label>
            </div>

            <!-- Modal footer -->
            <div class="modal-footer">

                <button type="button" id="btnYes" class="btn btn-success">Yes</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal">No</button>
            </div>

        </div>
    </div>
</div>
<script>
    showaccount();
    function showaccount() {
        $.ajax({
            type: "POST",
            url: "../Account/GetAccountWithUserId",
            contentType: "application/json; charset=utf-8",
            data: "{UserId:'" + @Session("UserId") + "'}",
            dataType: "json",
            success: function (data) {
                var tblSomething = '';
                $.each(data, function (idx, obj) {
                    $.each(obj, function (key, value) {
                        if (key == "Name") {
                            document.getElementById("name").value = value;
                        } else if (key == "Username") {
                            document.getElementById("username").value = value;
                        } else if (key == "Password") {
                            document.getElementById("password").value = value;
                        } else if (key == "Department") {
                            document.getElementById("department").value = value;
                        } else if (key == "Sections") {
                            document.getElementById("sections").value = value;
                        } else if (key == "Email") {
                            document.getElementById("email").value = value;
                        }
                    });
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    }

    $("#btnYes").click(function () {
        var formData = new FormData();
        var name = document.getElementById('name').value;
        var sections = document.getElementById('sections').value;
        var username = document.getElementById('username').value;
        var password = document.getElementById('password').value;
        var department = document.getElementById('department').value;
        var email = document.getElementById('email').value;
        if (username != '' && password != '' && password.length >= 6) {
            formData.append("name", name);
            formData.append("sections", sections);
            formData.append("username", username);
            formData.append("password", password);
            formData.append("department", department);
            formData.append("email", email);
            formData.append("userid", @Session("UserId"));

            $.ajax({
                type: "POST",
                url: '../Account/UpDateAccount',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    myReload();
                },
                error: function (error) {
                    alert(error);
                }
            });
        } else {
            if (password.length < 6) {
                document.getElementById('idAlert').innerHTML = "Password must more than 6 character.";
                $("#AlertShow").modal();
            }
        }
    });
    function myReload() {
        $('#AlertShow').modal('hide');
        window.location.href = '../Account/Logout';
    }


    $("#btnEdit").click(function () {
        document.getElementById('idAlert').innerHTML = "You want edit profile?";
        $("#AlertShow").modal();
    });
</script>

