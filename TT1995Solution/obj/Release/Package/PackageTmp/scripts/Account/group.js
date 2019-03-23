$(function () {
    function GetColumn() {
        var dataColumn = [{
            dataField: "group_name",
            caption: "ชื่อกลุ่ม",
            dataType: "string",
            width: '50%',
            
        }, {
            dataField: "remark",
            caption: "รายละเอียด",
            dataType: "string",
            width: '50%',
            
        }];
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
            var statusInsert = fnInsertGroup(e.data);
            if (statusInsert != '0') {
                e.data.group_id = statusInsert;
            } else {
                e.data = null;
            }
        },
        onRowRemoving: function (e) {
            e.cancel = !fnDeleteGroup(e.key.group_id);
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    GetColumn();

    function fnGetGroup() {
        $.ajax({
            type: "POST",
            url: "../Account/GetGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                //console.log(data);
                dataGrid.option('dataSource', data);
                //console.log(data);
            }
        });
    }

    fnGetGroup();


    function fnInsertGroup(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Account/InsertGroup",
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

    function fnDeleteGroup(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Account/DeleteGroup",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลเรียบร้อยแล้ว", "success");
                    boolDel = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    boolDel = false;
                }
            },
            error: function (request, status, error) {
                //console.log(request);
                boolDel = false
            }
        });
        return boolDel;
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});


