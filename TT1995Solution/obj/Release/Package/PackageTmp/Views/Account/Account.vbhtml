@Code
    ViewData("Title") = "Account"
End Code
<style>
    .toast-top-center {
        top: 50%;
        margin: 0 auto;
        margin-left: -150px;
    }

    #dataTable {
        width: 100%;
    }

    .modal-full {
        min-width: 100%;
    }
</style>
<!-- Breadcrumbs-->
<ol class="breadcrumb">
    <li class="breadcrumb-item">
        <a href="#">Main</a>
    </li>
    <li class="breadcrumb-item active">Account</li>
</ol>

<!-- DataTables -->
<div class="card mb-3">
    <div class="card-header">
        <i class="fas fa-table"></i>
        Config Account
        <button type="button" class="btn btn-success btn-sm float-sm-right" data-toggle="modal" data-tooltip="tooltip" data-placement="right" title="Add" data-target="#accountAdd"><i class="fas fa-plus-circle"></i></button>
    </div>

    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-bordered" id="dataTable" cellspacing="0">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Username</th>
                        <th>Department</th>
                        <th>Sections</th>
                        <th>Email</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody id="dataRow"></tbody>
            </table>

            <!-- The Modal docAdd-->
            <!--Modal add-->
            <div class="modal ml-2 mr-2" id="accountAdd">
                <div class="modal-dialog modal-dialog-centered modal-full">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Account Add</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="row">
                                <div class="col">
                                    <div class="card mb-2">
                                        <div class="card-header">
                                            Profile
                                        </div>
                                        <div class="card-body">
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    Name
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="namea" placeholder="name" value="" required>
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
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="departmenta" placeholder="department" value="" required>
                                                </div>
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="sectionsa" placeholder="sections" value="" required>
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    Email
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="emaila" placeholder="email" value="" required>
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
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="usernamea" placeholder="username" value="" required>
                                                </div>
                                                <div class="col-sm">
                                                    <input type="password" class="form-control" style="background-color: #FCFBC8" id="passworda" placeholder="password" value="" required>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="card mb-2" style="height:388px">
                                        <div class="card-header">
                                            Permission
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive" style="height:310px">
                                                <table class="table table-bordered" id="dataTable" cellspacing="0">
                                                    <thead>
                                                        <tr>
                                                            <th>Application</th>
                                                            <th>Group</th>
                                                            <th>Action</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="dataRowPeradd"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" id="btnSave" class="btn btn-success" data-dismiss="modal">Save</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal add-->
            <!--Modal Edit-->
            <div class="modal ml-2 mr-2" id="accountEdit">
                <div class="modal-dialog modal-dialog-centered modal-full">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Account Edit <label hidden id="seq"></label></h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="row">
                                <div class="col">
                                    <div class="card mb-2">
                                        <div class="card-header">
                                            Profile
                                        </div>
                                        <div class="card-body">
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    Name
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="namee" placeholder="name" value="" required>
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
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="departmente" placeholder="department" value="" required>
                                                </div>
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="sectionse" placeholder="sections" value="" required>
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    Email
                                                </div>
                                            </div>
                                            <div class="row mb-2">
                                                <div class="col-sm">
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="emaile" placeholder="email" value="" required>
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
                                                    <input type="text" class="form-control" style="background-color: #FCFBC8" id="usernamee" placeholder="username" value="" required>
                                                </div>
                                                <div class="col-sm">
                                                    <input type="password" class="form-control" style="background-color: #FCFBC8" id="passworde" placeholder="password" value="" required>
                                                </div>

                                            </div>


                                        </div>
                                    </div>
                                </div>
                                <div class="col">
                                    <div class="card mb-2" style="height:388px">
                                        <div class="card-header">
                                            Permission
                                        </div>
                                        <div class="card-body">
                                            <div class="table-responsive" style="height:310px">
                                                <table class="table table-bordered" id="dataTable" cellspacing="0">
                                                    <thead>
                                                        <tr>
                                                            <th>Application</th>
                                                            <th>Group</th>
                                                            <th>Action</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody id="dataRowPer"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" id="btnEdit" class="btn btn-success" data-dismiss="modal">OK</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal Edit-->
            <!--Modal Del-->
            <div class="modal" id="accountDel">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Account Delete <label hidden id="seqDel"></label></h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            <div class="card mb-2">
                                <div class="card-body">
                                    <div class="row mb-2">
                                        <div class="col-sm">
                                            <input type="text" class="form-control" id="usernamed" placeholder="group name" value="" readonly>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" id="btnDel" class="btn btn-success" data-dismiss="modal">Delete</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal Del-->
            <!--Modal Permission Del-->
            <div class="modal" id="PerDel">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Confirm</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            Permission Delete ID <label hidden id="PerId"></label>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">
                            <button type="button" id="btnDelPer" class="btn btn-success" data-dismiss="modal">Delete</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal Permission Del-->
            <!--Modal Permission Add-->
            <div class="modal" id="PerAdd">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Confirm</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            Permission Add ID <label hidden id="UserId"></label>
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">

                            <button type="button" id="btnAddPer" class="btn btn-success" data-dismiss="modal">Add</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal Permission Add-->
            <!--Modal Account Permission Add-->
            <div class="modal" id="AccPerAdd">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">

                        <!-- Modal Header -->
                        <div class="modal-header">
                            <h4 class="modal-title">Confirm</h4>
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                        </div>

                        <!-- Modal body -->
                        <div class="modal-body">
                            Permission Add
                        </div>

                        <!-- Modal footer -->
                        <div class="modal-footer">

                            <button type="button" id="btnAccAddPer" class="btn btn-success" data-dismiss="modal">Add</button>
                            <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
                        </div>

                    </div>
                </div>
            </div>
            <!--End Modal Account Permission Add-->

        </div>
    </div>
    <div class="card-footer small text-muted">
        <div class="row">
            <div class="col">
                Updated yesterday at 11:59 PM
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

                    <button type="button" id="btnRe" class="btn btn-success">OK</button>
                    @*<button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>*@
                </div>

            </div>
        </div>
    </div>
</div>
<script>

    getAccount();

    //window.location.href = '../Home/DocQms';
    function myReload() {
        $('#AlertShow').modal('hide');
        //getDoc();
        window.location.href = '../Account/Account';
    }
    $("#btnRe").click(function () {
        myReload();
    });

    $(document).ready(function () {
        $('[data-tooltip="tooltip"]').tooltip();
        //$('#dataTable').DataTable({
        //    "scrollX": true
        //});
        //$("#btnSave").click(function () {
        //    var formData = new FormData();
        //    var firstname = document.getElementById('firstnamea').value;
        //    var lastname = document.getElementById('lastnamea').value;
        //    var username = document.getElementById('usernamea').value;
        //    var password = document.getElementById('passworda').value;
        //    var department = document.getElementById('departmenta').value;
        //    var email = document.getElementById('emaila').value;
        //    var groupid = document.getElementById('nameGroupa').value;
        //    if (groupid != 0 && username != '' && password != '') {
        //        formData.append("firstname", firstname);
        //        formData.append("lastname", lastname);
        //        formData.append("username", username);
        //        formData.append("password", password);
        //        formData.append("department", department);
        //        formData.append("email", email);
        //        formData.append("groupid", groupid);
        //        $.ajax({
        //            type: "POST",
        //            url: '../Account/UploadAccount',
        //            data: formData,
        //            dataType: 'json',
        //            contentType: false,
        //            processData: false,
        //            success: function (response) {
        //                document.getElementById('firstnamea').value = '';
        //                document.getElementById('lastnamea').value = '';
        //                document.getElementById('usernamea').value = '';
        //                document.getElementById('passworda').value = '';
        //                document.getElementById('departmenta').value = '';
        //                document.getElementById('emaila').value = '';
        //                document.getElementById('nameGroupa').value = '0';
        //                getAccount();
        //                showToast(2);
        //            },
        //            error: function (error) {
        //                alert(error);
        //            }
        //        });
        //    } else {
        //        showToast(0);
        //    }
        //});

        //$("#btnEdit").click(function () {
        //    var formData = new FormData();
        //    var firstname = document.getElementById('firstnamee').value;
        //    var lastname = document.getElementById('lastnamee').value;
        //    var username = document.getElementById('usernamee').value;
        //    var password = document.getElementById('passworde').value;
        //    var department = document.getElementById('departmente').value;
        //    var email = document.getElementById('emaile').value;
        //    var groupid = document.getElementById('nameGroupe').value;
        //    var isactive = document.getElementById('isactive').value;
        //    var seq = document.getElementById('seq').innerHTML;
        //    formData.append("firstname", firstname);
        //    formData.append("lastname", lastname);
        //    formData.append("username", username);
        //    formData.append("password", password);
        //    formData.append("department", department);
        //    formData.append("email", email);
        //    formData.append("isactive", isactive);
        //    formData.append("groupid", groupid);
        //    formData.append("userid", seq);
        //    $.ajax({
        //        type: "POST",
        //        url: '../Account/UpDateAccount',
        //        data: formData,
        //        dataType: 'json',
        //        contentType: false,
        //        processData: false,
        //        success: function (response) {
        //            document.getElementById('firstnamee').value = '';
        //            document.getElementById('lastnamee').value = '';
        //            document.getElementById('usernamee').value = '';
        //            document.getElementById('passworde').value = '';
        //            document.getElementById('departmente').value = '';
        //            document.getElementById('emaile').value = '';
        //            document.getElementById('seq').innerHTML = '';
        //            getAccount();
        //            showToast(2);
        //        },
        //        error: function (error) {
        //            alert(error);
        //        }
        //    });
        //});

        //$("#btnDel").click(function () {
        //    var formData = new FormData();
        //    var seq = document.getElementById('seqDel').innerHTML;
        //    formData.append("UserId", seq);
        //    $.ajax({
        //        type: "POST",
        //        url: '../Account/DelAccount',
        //        data: formData,
        //        dataType: 'json',
        //        contentType: false,
        //        processData: false,
        //        success: function (response) {
        //            document.getElementById('seqDel').innerHTML = '';
        //            getAccount();
        //            showToast(2);
        //        },
        //        error: function (error) {
        //            alert(error);
        //        }
        //    });
        //});
    });

    //Action Modal

    $("#btnDel").click(function () {
        var formData = new FormData();
        var seq = document.getElementById('seqDel').innerHTML;
        formData.append("UserId", seq);

        $.ajax({
            type: "POST",
            url: '../Account/DelAccount',
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (response) {
                document.getElementById('seqDel').innerHTML = '';
                //getAccount();
                //showToast(2);
                document.getElementById('idAlert').innerHTML = "Delete account success.";
                $("#AlertShow").modal();
            },
            error: function (error) {
                alert(error);
            }
        });
        //window.location.href = '../Account/Account';
    });

    $("#btnEdit").click(function () {
        var formData = new FormData();
        var name = document.getElementById('namee').value;
        var sections = document.getElementById('sectionse').value;
        var username = document.getElementById('usernamee').value;
        var password = document.getElementById('passworde').value;
        var department = document.getElementById('departmente').value;
        var email = document.getElementById('emaile').value;
        var UserId = document.getElementById("seq").innerHTML;
        if (username != '' && password != '' && password.length >= 6) {
            formData.append("name", name);
            formData.append("sections", sections);
            formData.append("username", username);
            formData.append("password", password);
            formData.append("department", department);
            formData.append("email", email);
            formData.append("userid", UserId);

            $.ajax({
                type: "POST",
                url: '../Account/UpDateAccount',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    document.getElementById('namee').value = '';
                    document.getElementById('sectionse').value = '';
                    document.getElementById('usernamee').value = '';
                    document.getElementById('passworde').value = '';
                    document.getElementById('departmente').value = '';
                    document.getElementById('emaile').value = '';
                    document.getElementById('seq').innerHTML = '';
                    //getAccount();
                    //showToast(2);
                    document.getElementById('idAlert').innerHTML = "Update account success.";
                    $("#AlertShow").modal();
                },
                error: function (error) {
                    alert(error);
                }
            });
            //window.location.href = '../Account/Account';
        } else {
            if (password.length < 6) {
                document.getElementById('idAlert').innerHTML = "Password must more than 6 character.";
                $("#AlertShow").modal();
            }
            //showToast(0);
        }

    });

    $("#btnSave").click(function () {
        var formData = new FormData();
        var name = document.getElementById('namea').value;
        var sections = document.getElementById('sectionsa').value;
        var username = document.getElementById('usernamea').value;
        var password = document.getElementById('passworda').value;
        var department = document.getElementById('departmenta').value;
        var email = document.getElementById('emaila').value;

        var AppPer = '';
        var AccPer = '';
        var oTable = document.getElementById('dataRowPeradd');
        var rowLength = oTable.rows.length - 1;
        for (i = 0; i < rowLength; i++) {
            var oCells = oTable.rows.item(i).cells;
            var cellLength = oCells.length - 1;
            for (var j = 0; j < cellLength; j++) {
                var cellVal = oCells.item(j).innerHTML;
                if (j == 0) {
                    AppPer += cellVal + ',';
                }else if(j == 1){
                    AccPer += cellVal + ',';
                }
            }
        }

        if (username != '' && password != '' && password.length >= 6) {
            formData.append("name", name);
            formData.append("sections", sections);
            formData.append("username", username);
            formData.append("password", password);
            formData.append("department", department);
            formData.append("email", email);
            formData.append("appper", AppPer);
            formData.append("accper", AccPer);
            $.ajax({
                type: "POST",
                url: '../Account/InsertAccount',
                data: formData,
                dataType: 'json',
                contentType: false,
                processData: false,
                success: function (response) {
                    //Insert Permission

                    document.getElementById('namea').value = '';
                    document.getElementById('sectionsa').value = '';
                    document.getElementById('usernamea').value = '';
                    document.getElementById('passworda').value = '';
                    document.getElementById('departmenta').value = '';
                    document.getElementById('emaila').value = '';
                    //getAccount();
                    //showToast(2);
                    document.getElementById('idAlert').innerHTML = "Recorded account success.";
                    $("#AlertShow").modal();
                },
                error: function (error) {
                    alert(error);
                }
            });
            //window.location.href = '../Account/Account';
        } else {
            if (password.length < 6) {
                document.getElementById('idAlert').innerHTML = "Password must more than 6 character.";
                $("#AlertShow").modal();
            }
            //showToast(0);
        }
    });
    //End Action Modal

    $("#btnDelPer").click(function () {
        var PerId = document.getElementById("PerId").innerHTML;
        var UserId = document.getElementById("seq").innerHTML;
        console.log(PerId);
        console.log(UserId);
        if (PerId != '') {
            $.ajax({
                type: "POST",
                url: "../Account/DeletePermission",
                contentType: "application/json; charset=utf-8",
                data: "{PerId: '" + PerId + "'}",
                dataType: "json",
                success: function (data) {
                    //showToast(2);

                    getApplication(UserId);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('Hey, something went wrong because: ' + errorThrown);
                }
            });
        }
    });

    $("#btnAddPer").click(function () {
        var AppId = document.getElementById("cbApp").value;
        var AccessId = document.getElementById("cbPer").value;
        var UserId = document.getElementById("UserId").innerHTML;
        if (AppId != 0 && AccessId != 0 && UserId != '') {
            $.ajax({
                type: "POST",
                url: "../Account/InsertPermission",
                contentType: "application/json; charset=utf-8",
                data: "{AppId: '" + AppId + "', AccessId: '" + AccessId + "', UserId:'" + UserId + "'}",
                dataType: "json",
                success: function (data) {
                    //showToast(2);
                    getApplication(UserId);
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert('Hey, something went wrong because: ' + errorThrown);
                }
            });
        } else {
            showToast(0);
        }
    });

    $('#PerDel').on('show.bs.modal', function (e) {
        var bookId = $(e.relatedTarget).data('book-id');
        document.getElementById("PerId").innerHTML = bookId;
    });

    $('#PerAdd').on('show.bs.modal', function (e) {
        var bookId = $(e.relatedTarget).data('book-id');
        document.getElementById("UserId").innerHTML = bookId;
    });

    function getApp(UserId) {
        $.ajax({
            type: "POST",
            url: "../Account/GetApp",
            contentType: "application/json; charset=utf-8",
            data: "{UserId:'" + UserId + "'}",
            dataType: "json",
            success: function (data) {
                var cbSomething = "<option value='0'></option>";
                $.each(data, function (idx, obj) {
                    var AppId = 0;
                    $.each(obj, function (key, value) {
                        if (key == "Name") {
                            cbSomething += "<option value='" + AppId + "'>" + value + "</option>";
                        } else if (key == "AppId") {
                            AppId = value;
                        }
                    });
                });

                $('#cbApp').html(cbSomething);
                $('#cbPer').html("<option value='0'></option><option value='2'>User</option><option value='3'>User Admin</option>");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    };

    function getApplication(UserId) {
        $.ajax({
            type: "POST",
            url: "../Account/GetApplication",
            contentType: "application/json; charset=utf-8",
            data: "{UserId:'" + UserId + "'}",
            dataType: "json",
            success: function (data) {
                console.log(data);
                var tblSomething = "";
                var AppId = "";
                $.each(data, function (idx, obj) {
                    tblSomething += "<tr>";

                    $.each(obj, function (key, value) {
                        if (key == "id") {
                            AppId = value;
                        } else {
                            tblSomething += "<td>" + value + "</td>";
                        }
                    });
                    tblSomething += '<td class="text-center"><div class="btn-group"><button type="button" class="btn btn-danger btn-sm" data-tooltip="tooltip" data-placement="right" title="Delete" data-toggle="modal" data-target="#PerDel" data-book-id=' + AppId + '><i class="fas fa-trash-alt"></i></button></div></td>';
                    tblSomething += "</tr>";
                });
                tblSomething += "<tr>";
                tblSomething += "<td><select class='form-control' id='cbApp'></select></td>";
                tblSomething += "<td><select class='form-control' id='cbPer'></select></td>";
                tblSomething += "<td class='text-center'><button type='button' class='btn btn-success btn-sm' data-tooltip='tooltip' data-placement='right' title='Add' data-toggle='modal' data-target='#PerAdd' data-book-id='" + UserId + "'><i class='fas fa-plus-circle'></i></button></td>";
                tblSomething += "</tr>";
                $('#dataRowPer').html(tblSomething);
                getApp(UserId);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    };

    $('#accountEdit').on('show.bs.modal', function (e) {
        var bookId = $(e.relatedTarget).data('book-id');
        document.getElementById("seq").innerHTML = bookId;
        getApplication(bookId);
        $.ajax({
            type: "POST",
            url: "../Account/GetAccountWithUserId",
            contentType: "application/json; charset=utf-8",
            data: "{UserId:'" + bookId + "'}",
            dataType: "json",
            success: function (data) {
                var tblSomething = '';
                $.each(data, function (idx, obj) {
                    $.each(obj, function (key, value) {
                        if (key == "Name") {
                            document.getElementById("namee").value = value;
                        } else if (key == "Username") {
                            document.getElementById("usernamee").value = value;
                        } else if (key == "Password") {
                            document.getElementById("passworde").value = value;
                        } else if (key == "Department") {
                            document.getElementById("departmente").value = value;
                        } else if (key == "Sections") {
                            document.getElementById("sectionse").value = value;
                        } else if (key == "Email") {
                            document.getElementById("emaile").value = value;
                        }
                    });
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    });

    $('#accountAdd').on('show.bs.modal', function (e) {

        var tblSomething = "<tr>";
        tblSomething += "<td><select class='form-control' id='cbAppa'></select></td>";
        tblSomething += "<td><select class='form-control' id='cbPera'></select></td>";
        tblSomething += "<td class='text-center'><button type='button' class='btn btn-success btn-sm' data-tooltip='tooltip' data-placement='right' title='Add' data-toggle='modal' data-target='#AccPerAdd' data-book-id='" + UserId + "'><i class='fas fa-plus-circle'></i></button></td>";
        tblSomething += "</tr>";
        $('#dataRowPeradd').html(tblSomething);
        $.ajax({
            type: "POST",
            url: "../Account/GetApp",
            contentType: "application/json; charset=utf-8",
            data: "{UserId:'0'}",
            dataType: "json",
            success: function (data) {
                var cbSomething = "<option value='0'></option>";
                $.each(data, function (idx, obj) {
                    var AppId = 0;
                    $.each(obj, function (key, value) {
                        if (key == "Name") {
                            cbSomething += "<option value='" + AppId + "'>" + value + "</option>";
                        } else if (key == "AppId") {
                            AppId = value;
                        }
                    });
                });

                $('#cbAppa').html(cbSomething);
                $('#cbPera').html("<option value='0'></option><option value='2'>User</option><option value='3'>User Admin</option>");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    });

    $("#btnAccAddPer").click(function () {
        var AppId = document.getElementById("cbAppa").value;
        var AppName = $('#cbAppa').find(":selected").text();
        var AccessId = document.getElementById("cbPera").value;
        var AccessName = $('#cbPera').find(":selected").text();
        var cbOptionApp = document.getElementById("cbAppa");
        var cbOptionPer = document.getElementById("cbPera");
        var AppRm = document.getElementById("cbAppa");
        AppRm.remove(AppRm.selectedIndex);
        cbOptionApp[0].selected = true;
        cbOptionPer[0].selected = true;

        var table = document.getElementById("dataRowPeradd");
        var row = table.insertRow(0);
        var cell1 = row.insertCell(0);
        var cell2 = row.insertCell(1);
        var cell3 = row.insertCell(2);
        cell3.setAttribute('style', 'text-align:center;');
        cell1.innerHTML = AppName;
        cell2.innerHTML = AccessName;
        cell3.innerHTML = "<button class='btn btn-danger' onclick='funcDelete(this.parentNode.parentNode.rowIndex)'><i class='fas fa-trash-alt' aria-hidden='true'></i></button>";

    });

    function funcDelete(x) {
        document.getElementById("dataRowPeradd").deleteRow(x - 1);
    }

    //$('#accountAdd').on('show.bs.modal', function (e) {
    //    $.ajax({
    //        type: "POST",
    //        url: "../Account/GetGroup",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        success: function (data) {
    //            var tblSomething = "<option value='0'>Select Group</option>";
    //            $.each(data, function (idx, obj) {
    //                var groupid = 0;
    //                $.each(obj, function (key, value) {
    //                    if (key == "nameGroup") {
    //                        tblSomething += "<option value='" + groupid + "'>" + value + "</option>";
    //                    } else if (key == "GroupId") {
    //                        groupid = value;
    //                    }
    //                });
    //            });
    //            $('#nameGroupa').html(tblSomething);
    //        },
    //        error: function (jqXHR, textStatus, errorThrown) {
    //            alert('Hey, something went wrong because: ' + errorThrown);
    //        }
    //    });
    //});

    $('#accountDel').on('show.bs.modal', function (e) {
        var bookId = $(e.relatedTarget).data('book-id');
        document.getElementById("seqDel").innerHTML = bookId;
        $.ajax({
            type: "POST",
            url: "../Account/GetAccountWithUserId",
            contentType: "application/json; charset=utf-8",
            data: "{ UserId:'" + bookId + "'}",
            dataType: "json",
            success: function (data) {
                $.each(data, function (idx, obj) {
                    $.each(obj, function (key, value) {
                        if (key == "Username") {
                            document.getElementById("usernamed").value = value;
                        }
                    });
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    });

    function getAccount() {
        $.ajax({
            type: "POST",
            url: "../Account/GetAccount",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                var tblSomething = '';
                $.each(data, function (idx, obj) {
                    tblSomething += '<tr>';
                    var UserId = 0;
                    $.each(obj, function (key, value) {
                        if (key == "UserId") {
                            UserId = value;
                        }else{
                            tblSomething += "<td>" + value + "</td>";
                        }
                    });

                    tblSomething += '<td class=\'text-center\'><div class="btn-group"><button type="button" class="btn btn-warning btn-sm" data-toggle="modal" data-tooltip="tooltip" data-placement="right" title="Edit" data-target="#accountEdit" data-book-id=' + UserId + '><i class="fas fa-edit"></i></button><button type="button" class="btn btn-danger btn-sm" data-tooltip="tooltip" data-placement="right" title="Delete" data-toggle="modal" data-target="#accountDel" data-book-id=' + UserId + '><i class="fas fa-trash-alt"></i></button></div></td>'
                    tblSomething += '</tr>';

                });
                $('#dataRow').html(tblSomething);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    }

    //Toast

    function Toast(type, css, msg) {
        this.type = type;
        this.css = css;
        this.msg = msg;
    }

    var toasts = [
        new Toast('error', 'toast-top-center', 'Please check the filling.'),
        new Toast('warning', 'toast-top-center', 'warning'),
        new Toast('success', 'toast-top-center', 'success')
    ];

    //toastr.options.extendedTimeOut = 0; //1000;
    var i = 0;

    function showToast(indexMsg) {
        toastr.options.timeOut = 3000;
        toastr.options.fadeOut = 250;
        toastr.options.fadeIn = 250;
        var t = toasts[indexMsg];
        toastr.options.positionClass = t.css;
        toastr[t.type](t.msg);
    }

</script>

