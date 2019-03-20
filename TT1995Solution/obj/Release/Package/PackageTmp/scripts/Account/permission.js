$(function () {
    function GetColumn(){
        var jsonStatus = [{
            permission_status_name: "Read Only",
            permission_status: 1
        }, {
            permission_status_name: "Read and Write",
            permission_status: 2
        }];

        var jsonGroup;
        $.ajax({
            type: "POST",
            url: "../Account/GetGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                jsonGroup = data;
            }
        });

        var jsonApplication;
        $.ajax({
            type: "POST",
            url: "../Account/GetApplication",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                jsonApplication = data;
            }
        });
        var dataColumn = [{
            dataField: "group_id",
            caption: "Name Group",
            dataType: "string",
            width: '30%',
            lookup: {
                dataSource: jsonGroup,
                displayExpr: "group_name",
                valueExpr: "group_id"
            }
        }, {
            dataField: "application_id",
            caption: "Name Application",
            dataType: "string",
            width: '30%',
            lookup: {
                dataSource: jsonApplication,
                displayExpr: "application_name",
                valueExpr: "application_id"
            }
        }, {
            dataField: "permission_status",
            caption: "Permission Status",
            dataType: "string",
            width: '30%',
            lookup: {
                dataSource: jsonStatus,
                displayExpr: "permission_status_name",
                valueExpr: "permission_status"
            }
        }];
        console.log(dataColumn);
        dataGrid.option('columns', dataColumn);
    }
    
    var dataGrid = $("#gridContainer").dxDataGrid({
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        showBorders: true,
        paging: {
            enabled: true,
            pageSize: 10
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        editing: {
            mode: "row",
            allowDeleting: true,
            allowAdding: true,
            useIcons: true,
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        onRowInserting: function (e) {
            e.data.permission_id = fnInsertPermission(e.data);
        },
        onRowRemoving: function (e) {
            fnDeletePermission(e.key.permission_id);
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    GetColumn();

    function fnGetPermission() {
        $.ajax({
            type: "POST",
            url: "../Account/GetPermission",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataGrid.option('dataSource', data);
                //console.log(data);
            }
        });
    }

    fnGetPermission();


    function fnInsertPermission(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Account/InsertPermission",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }

    function fnDeletePermission(keyItem) {
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Account/DeletePermission",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            },
            error: function (request, status, error) {
                console.log(request);
            }
        });
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});


