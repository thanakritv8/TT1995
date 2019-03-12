@Code
    ViewData("Title") = "Group"
End Code
<style>
    #dataTable {
        width: 100%;
    }
</style>
<!-- Breadcrumbs-->
<ol class="breadcrumb">
    <li class="breadcrumb-item">
        <a href="#">Main</a>
    </li>
    <li class="breadcrumb-item active">Group</li>
</ol>

<!-- DataTables -->
<div class="card mb-3">
    <div class="card-header">
        <i class="fas fa-table"></i>
        Show Group
    </div>

    <div class="card-body">
        <div class="table-responsive">
            <table class="table table-bordered" id="dataTable" cellspacing="0">
                <thead>
                    <tr>
                        <th>Group Id</th>
                        <th>Group Name</th>
                        <th>Note</th>
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
</div>
<script>

    getGroup();

    function getGroup() {
        $.ajax({
            type: "POST",
            url: "../Account/GetGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                var tblSomething = '';
                $.each(data, function (idx, obj) {
                    tblSomething += '<tr>';
                    $.each(obj, function (key, value) {
                        if (key == "AccessId") {
                            tblSomething += "<td>" + value + "</td>";
                        } else if (key == "Name") {
                            tblSomething += "<td>" + value + "</td>";
                        } else if (key == "Note") {
                            tblSomething += "<td>" + value + "</td>";
                        }
                    });
                    tblSomething += '</tr>';

                });
                $('#dataRow').html(tblSomething);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert('Hey, something went wrong because: ' + errorThrown);
            }
        });
    }

</script>

