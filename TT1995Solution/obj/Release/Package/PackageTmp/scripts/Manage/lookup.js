var dataColumn = [
    { dataField: "name_table", caption: "ตาราง", dataType: "string", width: "30%" },
    { dataField: "name_column", caption: "คอลัมน์", dataType: "string", width: "30%" },
    { dataField: "data_list", caption: "รายการ", dataType: "string", width: "30%" },
];
$(function () {

    

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

    GetColumn();
    function GetColumn() {
        $.ajax({
            type: "POST",
            url: "../Manage/GetTable",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataColumn[0].setCellValue = function (rowData, value) {
                    rowData.table_id = value;
                    rowData.column_id = null;
                }
                dataColumn[0].lookup = {
                    dataSource: data,
                    valueExpr: "table_id",
                    displayExpr: "name_table"
                }
                
            }
        });

        $.ajax({
            type: "POST",
            url: "../Manage/GetColumn",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataColumn[1].lookup = {
                    dataSource: function (options) {
                        return {
                            store: data,
                            filter: options.data ? ["table_id", "=", options.data.table_id] : null
                        };
                    },
                    valueExpr: "column_id",
                    displayExpr: "name_column"
                }

            }
        });
        console.log(dataColumn);
        dataGrid.option('columns', dataColumn);
    }

    fnGetLookup();

    function fnGetLookup() {
        $.ajax({
            type: "POST",
            url: "../Manage/GetLookup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataGrid.option('dataSource', data);
                console.log(data);
                
            }
        });
    }

    function fnInsertLookup(dataGrid) {
        console.log(JSON.stringify(dataGrid));
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
})
