@Code
    ViewData("Title") = "Application"
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
</style>
<!-- Breadcrumbs-->
<ol class="breadcrumb">
    <li class="breadcrumb-item">
        <a href="#">Main</a>
    </li>
    <li class="breadcrumb-item active">Application</li>
</ol>

<div class="card mb-3">
    <div class="card-header">
        <i class="fas fa-layer-group"></i>
        Select Username
    </div>

    <div class="card-body">
        <select onchange="UserChange()" class="form-control" id="cbUsername"></select>
    </div>
</div>


<!-- DataTables -->
<div class="card mb-3">
    <div class="card-header">
        <i class="far fa-check-square"></i>
        Checked Application
    </div>

    <div class="card-body">

        <div class="table-responsive">

            <table class="table table-bordered" id="dataTable" cellspacing="0">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Application</th>
                        <th>Group</th>
                        <th>Action</th>
                    </tr>
                </thead>
                <tbody id="dataRow"></tbody>
            </table>
        </div>
    </div>
    <div class="card-footer small text-muted">
        <div class="row">
            <div class="col">
                Updated yesterday at 11:59 PM
            </div>
        </div>
    </div>
    <!--Modal Del-->
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
                    Permission Delete ID <label id="PerId"></label>
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
</div>
<script>
    $(document).ready(function () {
        $('[data-tooltip="tooltip"]').tooltip();
        getUser();
    });
    //getApplication(1);
    $('#PerDel').on('show.bs.modal', function (e) {
        var bookId = $(e.relatedTarget).data('book-id');
        document.getElementById("PerId").innerHTML = bookId;
    });

    function getUser() {
        $.ajax({
            type: "POST",
            url: "../Account/GetUsername",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var cbSomething = "<option value='0'>Select Username</option>";
                $.each(data, function (idx, obj) {
                    var UserId = 0;
                    $.each(obj, function (key, value) {
                        if (key == "Username") {
                            cbSomething += "<option value='" + UserId + "'>" + value + "</option>";
                        } else if (key == "UserId") {
                            UserId = value;
                        }
                    });
                });

                $('#cbUsername').html(cbSomething);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    };

    function UserChange() {
        var UserId = document.getElementById("cbUsername").value;
        getApplication(UserId);
    };

    function getApplication(UserId) {
        console.log(UserId);
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
                            tblSomething += "<td>" + value + "</td>";
                        } else {
                            tblSomething += "<td>" + value + "</td>";
                        }
                    });
                    tblSomething += '<td class="text-center"><div class="btn-group"><button type="button" class="btn btn-danger btn-sm" data-tooltip="tooltip" data-placement="right" title="Delete" data-toggle="modal" data-target="#PerDel" data-book-id=' + AppId + '><i class="fas fa-trash-alt"></i></button></div></td>';
                    tblSomething += "</tr>";
                });

                console.log(tblSomething);
                $('#dataRow').html(tblSomething);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    };



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

