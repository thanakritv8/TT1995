var itemEditing = [];
//var itemEditingPermission = [];
var gbE;
var fileDataPdf;
var fileOpen;

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;
var gbTableId = '13';
var gbTableId_p = '14';

var _dataSource;
var dataGridAll;
var dataLookupFilter;

var gc;

$(function () {

    var dataGrid = $("#gridContainer").dxDataGrid({
        showBorders: true,
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        selection: {
            mode: "single"
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            fileOpen = e.currentSelectedRowKeys[0].business_path;
            console.log(gbE);
            isFirstClick = false;
        },
        onRowClick: function (e) {
            if (gbE.currentSelectedRowKeys[0].license_id == e.key.license_id && isFirstClick && rowIndex == e.rowIndex && gbE.currentDeselectedRowKeys.length == 0) {
                dataGrid.clearSelection();
            } else if (gbE.currentSelectedRowKeys[0].license_id == e.key.license_id && !isFirstClick) {
                isFirstClick = true;
                rowIndex = e.rowIndex;
            }
        },
    }).dxDataGrid('instance');

    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserBusinessIn",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 13}",
        dataType: "json",
        async: false,
        success: function (data) {
            var ndata = 0;
            data.forEach(function (item) {
                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "history") {
                    itemEditing.push({
                        colSpan: item.colSpan,
                        dataField: item.dataField,
                        width: "100%",
                        editorOptions: {
                            disabled: false
                        },
                    });
                }
                ndata++;

            });
            dataGrid.option('columns', data);
        },
        error: function (error) {
            console.log(error);
        }
    });

    $.ajax({
        type: "POST",
        url: "../Home/GetIndex",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            dataGrid.option('dataSource', data);
        }
    });

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});