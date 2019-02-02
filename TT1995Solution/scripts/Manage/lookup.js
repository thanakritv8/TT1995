var dataColumn = [];
$.ajax({
    type: "POST",
    url: "../Home/GetColumnChooser",
    contentType: "application/json; charset=utf-8",
    data: "{table_id: 1}",
    dataType: "json",
    success: function (data) {
        dataColumn = data;
    }
});

var dataGrid = $("#gridContainer").dxDataGrid({
    columns: dataColumn,
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
    "export": {
        enabled: true,
        fileName: "License",
    },
    filterRow: {
        visible: true,
        applyFilter: "auto"
    },
    headerFilter: {
        visible: true
    },
    onRowInserting: function (e) {
        fnInsertLookup(e.data);
    },
    onRowRemoving: function (e) {
        fnDeleteLookup(e.key.license_id);
    },
    selection: {
        mode: "single"
    },
}).dxDataGrid('instance');

fnGetLookup();

function fnGetLookup() {
    $.ajax({
        type: "POST",
        url: "../Home/GetLookup",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            dataGrid.option('dataSource', data);
        }
    });
}

function fnInsertLookup(dataGrid) {
    $.ajax({
        type: "POST",
        url: "../Manage/InsertLookup",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(dataGrid),
        dataType: "json",
        async: false,
        success: function (data) {
            if (data[0].Status == "1") {
                DevExpress.ui.notify("เพิ่มการค้นหาเรียบร้อยแล้ว", "success");
            } else {
                DevExpress.ui.notify(data[0].Status, "error");
            }
        }
    });
}

function fnDeleteLookup(keyItem) {
    $.ajax({
        type: "POST",
        url: "../Manage/DeleteLookup",
        contentType: "application/json; charset=utf-8",
        data: "{keyId: '" + keyItem + "'}",
        dataType: "json",
        success: function (data) {
            if (data[0].Status == 1) {
                DevExpress.ui.notify("ลบข้อมูลรายการค้นหาเรียบร้อยแล้ว", "success");
            } else {
                DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
            }
        }
    });
}