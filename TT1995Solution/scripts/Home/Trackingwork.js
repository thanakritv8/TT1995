var itemEditing = [];
var columnHide = [];
var gbTableId = '12';
var tableName = "trackingwork";
var idFile;
var dataGridAll;
var dataLookupFilter;
var gbE;
//คลิกขวาโชว์รายการ   
var contextMenuItemsRoot = [
    { text: 'New File' },
    { text: 'New Folder' },
];
var contextMenuItemsFolder = [
    { text: 'New File' },
    { text: 'New Folder' },
    { text: 'Rename' },
    { text: 'Delete' }
];
var contextMenuItemsFile = [
    { text: 'Rename' },
    { text: 'Delete' }
];
var OptionsMenu = contextMenuItemsFolder;

$(function () {
    function GetTrackingworkData() {
        //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetTrackingworkData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                //console.log(data);
                for (var i = 0; i < data.length; i++) {

                    var d = parseJsonDate(data[i].date_expire);
                    data[i].date_expire = d;

                    var d = parseJsonDate(data[i].start_schedule);
                    data[i].start_schedule = d;

                    var d = parseJsonDate(data[i].deadline);
                    data[i].deadline = d;
                }
                //dataGrid.option('dataSource', data);
            }
        }).responseJSON;
        //จบการโชว์ข้อมูลทะเบียน
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

    function fnGetHistory(table, idOfTable) {
        var dataValue = [];
        //โชว์ข้อมูลประวัติ
        return $.ajax({
            type: "POST",
            url: "../Home/getHistory",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{table: '" + table + "',idOfTable: '" + idOfTable + "'}",
            async: false,
            success: function (data) {

                for (var i = 0; i < data.length; i++) {
                    var d = parseJsonDate(data[i]._date);
                    data[i]._date = d;
                }
            }
        }).responseJSON;
        //จบการโชว์ข้อมูลประวัติ
    }

    //data grid
    var dataGrid = $("#gridContainer").dxDataGrid({
        allowColumnResizing: true,
        columnResizingMode: "widget",
        dataSource: GetTrackingworkData(),
        onContentReady: function (e) {
            
            //filter();
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        showBorders: true,
        columnChooser: {
            enabled: true,
            mode: "select"
        },
        paging: {
            enabled: true,
            pageSize: 10
        },
        onCellPrepared: function (e) {
            if (e.rowType == 'data' && e.column.dataField == 'tablename') {
                if (e.key.data_status == 'ยังไม่ได้ดำเนินการ' || e.key.data_status == 'ขาดต่อ') {
                    e.cellElement.css("color", "red");
                } else if (e.key.data_status == 'จัดเตรียมเอกสาร' || e.key.data_status == 'ยื่นเอกสาร' || e.key.data_status == 'ตรวจ GPS') {
                    e.cellElement.css("color", "orange");
                } else if (e.key.data_status == 'เสร็จสมบูรณ์') {
                    e.cellElement.css("color", "green");
                }
            }
            
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        "export": {
            enabled: true,
            fileName: "Trackingwork",
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        headerFilter: {
            visible: true
        }
        ,
        selection: {
            mode: "single"
        }
    }).dxDataGrid('instance');
    //จบการกำหนด dataGrid

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserPoom",
        contentType: "application/json; charset=utf-8",
        data: "{gbTableId: '" + gbTableId + "'}",
        dataType: "json",
        success: function (data) {
            var ndata = 0;
            data.forEach(function (item) {

                //โชว์ Dropdown หน้าเพิ่มและแก้ไข
                if (item.status_lookup != "0") {
                    var dataLookup;
                    $.ajax({
                        type: "POST",
                        url: "../Home/GetLookUp",
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: "{column_id: '" + item.column_id + "'}",
                        async: false,
                        success: function (data) {
                            dataLookup = data;
                        }
                    });
                    data[ndata].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "data_list",
                        valueExpr: "data_list"
                    }
                }

                //popup
                if (item.dataField == "history") {
                    data[ndata].cellTemplate = function (container, options) {
                        $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                                .text(options.value)
                                .on('dxclick', function (e) {
                                    popup_history._options.contentTemplate = function (content) {
                                        var maxHeight = $("#popup_history .dx-overlay-content").height() - 150;
                                        content.append("<div id='gridHistory' style='max-height: " + maxHeight + "px;' ></div>");
                                    }

                                    $("#popup_history").dxPopup("show");
                                    var gridHistory = $("#gridHistory").dxDataGrid({
                                        dataSource: fnGetHistory(gbTableId, options.row.data.tw_id),
                                        showBorders: true,
                                        height: 'auto',
                                        scrolling: {
                                            mode: "virtual"
                                        },
                                        searchPanel: {
                                            visible: true,
                                            width: "auto",
                                            placeholder: "Search..."
                                        }
                                    }).dxDataGrid('instance');

                                    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
                                    $.ajax({
                                        type: "POST",
                                        url: "../Home/GetColumnChooser",
                                        contentType: "application/json; charset=utf-8",
                                        data: "{gbTableId: '19'}",
                                        dataType: "json",
                                        async: false,
                                        success: function (data) {
                                            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
                                            gridHistory.option('columns', data);
                                        }
                                    });
                                    //จบการกำหนด Column

                                })
                                .appendTo(container);
                    }
                }

                ndata++;
                //จบการตั้งค่าโชว์ Dropdown

                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "tw_id" && item.dataField != "history") {
                    if (item.dataField == "number_car") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            editorOptions: {
                                disabled: false
                            },
                        });
                    } else if (item.dataField != "license_car") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                        });
                    }
                }
                //จบรายการหน้าโชว์หน้าเพิ่มและแก้ไข
            });

            $.ajax({
                type: "POST",
                url: "../Home/GetNumberCar",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (dataLookup) {
                    data[0].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "number_car",
                        valueExpr: "license_id"
                    }

                    data[1].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "license_car",
                        valueExpr: "license_id"
                    }
                }
            });
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);
            //dataGrid.option('filterValue', [["data_status", "=", "ยังไม่ได้ดำเนินการ"], "or", ["data_status", "=", "ขาดต่อ"], "or", ["data_status", "=", "จัดเตรียมเอกสาร"], "or", ["data_status", "=", "ยื่นเอกสาร"],  "or", ["data_status", "=", "ตรวจ GPS"]]);
        }
    });
    //จบการกำหนด Column

    var popup_history = $("#popup_history").dxPopup({
        visible: false,
        width: "60%",
        height: "70%",
        showTitle: true,
        title: "ประวัติ",
        contentTemplate: function (content) {
            return $("<div id='gridHistory'>test</div>");
        }
    }).dxPopup("instance");

    $(document).on("dxclick", ".dx-datagrid-column-chooser .dx-closebutton", function () {
        var dataColumnVisible = "",
            dataColumnHide = "";
        var columnCount = dataGrid.columnCount(),
            i;
        for (i = 0; i < columnCount; i++) {
            if (dataGrid.columnOption(i, "visible")) {
                dataColumnVisible = dataColumnVisible + "*" + dataGrid.columnOption(i).column_id;;
            } else {
                dataColumnHide = dataColumnHide + "*" + dataGrid.columnOption(i).column_id;
            }
        }

        //alert(dataColumnVisible);
        //alert(dataColumnHide);

        $.ajax({
            type: "POST",
            url: "../Home/SetColumnHide",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{dataColumnVisible:'" + dataColumnVisible + "',dataColumnHide:'" + dataColumnHide + "'}",
            success: function (data) {
                if (data = 1) {
                    //alert('Update Column Hide OK');
                } else {
                    alert('Update Column Hide error!!');
                }
            }
        });
    });

});
