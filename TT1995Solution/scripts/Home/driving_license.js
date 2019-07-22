var itemEditing = [];
var columnHide = [];
var gbTableId = '31';
var tableName = "driving_license";
var idFile;
var data_lookup_number_car;
var _dataSource;
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
    $("a:contains('บอข')").first().addClass("active");

    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPdf);
        }
    });

    //กำหนดการ Upload files
    var cf = $(".custom-file").dxFileUploader({
        maxFileSize: 4000000,
        multiple: true,
        allowedFileExtensions: [".pdf"],
        accept: ".pdf",
        uploadMode: "useForm",
        onValueChanged: function (e) {
            console.log(gbE.currentSelectedRowKeys[0].license_factory_id);
            var files = e.value;
            fileDataPdf = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPdf.append('file', file);
                });
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].dl_id);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    function getDataDl() {
        var dataValue = [];
        //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetDLData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d = parseJsonDate(data[i].start_date);
                    data[i].start_date = d;

                    var d = parseJsonDate(data[i].expire_date);
                    data[i].expire_date = d;
                }
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

        dataSource: getDataDl(),
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        allowColumnResizing: true,
        columnResizingMode: "widget",
        showBorders: true,
        columnChooser: {
            enabled: true,
            mode: "select"
        }, onContentReady: function (e) {
            var columnChooserView = e.component.getView("columnChooserView");
            if (!columnChooserView._popupContainer) {

                columnChooserView._initializePopupContainer();
                columnChooserView.render();

                columnChooserView._popupContainer.on("hiding", function (e) {
                    //alert('hiding');
                    console.log(dataGrid);
                });
            }
        },
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
            mode: "popup",
            allowUpdating: boolStatus,
            allowDeleting: boolStatus,
            allowAdding: boolStatus,
            form: {
                items: itemEditing,
                colCount: 6,
            },
            popup: {
                title: "รายการ บอข",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window },
                toolbarItems: [{
                    toolbar: 'bottom',
                    location: 'after',
                    widget: "dxButton",
                    options: {
                        text: "Save",
                        onClick: function (e) {
                            //alert(statusUpdateProtection);
                            //console.log(gbE);
                            ////console.log(gbE);
                            ////console.log(dataGrid);
                            if (typeof gbE != "undefined") {
                                if (statusUpdateProtection == 2) {
                                    updateProtection(gbE.data.dl_id, html_editor.option("value"));
                                    gbE.data.protection = html_editor.option("value");
                                }
                            }
                            dataGrid.saveEditData();
                        }
                    }
                }, {
                    toolbar: 'bottom',
                    location: 'after',
                    widget: "dxButton",
                    options: {
                        text: "Cancel",
                        onClick: function (args) {
                            console.log(args);
                            dataGrid.cancelEditData();
                        }
                    }
                }]

            },
            useIcons: true,
        },
        "export": {
            enabled: true,
            fileName: "driving_license",
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        headerFilter: {
            visible: true
        }, onEditingStart: function (e) {
            statusUpdateProtection = 0;
            gbE = e;
        }, onEditorPrepared: function (e) {
            if (typeof html_editor != "undefined" && typeof e.row != "undefined") {
                //console.log(e.row.key.protection);
                html_editor.option("value", e.row.key.protection);
            }

        },
        onInitNewRow: function (e) {
            //$(".test").append("<p>Test</p>");
        },
        onRowUpdating: function (e) {
            if (!fnUpdateDL(e.newData, e.key.dl_id)) {
                e.cancel = true;
            }
        },
        onRowInserting: function (e) {
            var idInsert = fnInsertDL(e.data);
            if (idInsert != 0) {
                e.data.dl_id = idInsert;
                e.data.history = "ประวัติ";
            } else {
                e.cancel = true;
            }
        },
        onRowRemoving: function (e) {
            e.cancel = fnDeleteDL(e.key.dl_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                var currentV8 = options.data;
                console.log(currentV8);
                container.append($(
                    '<div class="row"><div class="col-2 ml-3 mr-2" id="btnView"></div><div class="col-2" id="btnUpload"></div></div>'
                ));

                $("#btnView").dxButton({
                    "icon": "exportpdf",
                    "text": "View",
                    onClick: function () {
                        if (fileOpen != null) {
                            window.open(fileOpen, '_blank');
                        }
                    }
                });

                $("#btnUpload").dxButton({
                    "icon": "upload",
                    "text": "Upload",
                    onClick: function () {
                        cf.reset();
                        $("#mdNewFile").modal();
                    }
                });
            }
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            isFirstClick = false;
        },
        onRowClick: function (e) {
            fileOpen = e.currentSelectedRowKeys[0].path;
            if (gbE.currentSelectedRowKeys[0].license_id == e.key.license_id && isFirstClick && rowIndex == e.rowIndex && gbE.currentDeselectedRowKeys.length == 0) {
                dataGrid.clearSelection();
            } else if (gbE.currentSelectedRowKeys[0].license_id == e.key.license_id && !isFirstClick) {
                isFirstClick = true;
                rowIndex = e.rowIndex;
            }
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    //จบการกำหนด dataGrid

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        data: "{gbTableId: '" + gbTableId + "'}",
        dataType: "json",
        async: false,
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
                                    dataSource: fnGetHistory(gbTableId, options.row.data.dl_id),
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
                if (item.dataField != "history") {
                    itemEditing.push({
                        colSpan: item.colSpan,
                        dataField: item.dataField,
                        width: "100%",


                    });
                }
                //จบรายการหน้าโชว์หน้าเพิ่มและแก้ไข
            });
            $.ajax({
                type: "POST",
                url: "../Home/GetDriverNameJoinTable",
                contentType: "application/json; charset=utf-8",
                data: "{TableName: '" + tableName + "'}",
                dataType: "json",
                async: false,
                success: function (dataLookup) {
                    data_lookup_number_car = dataLookup;
                    data[0].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "driver_name",
                        valueExpr: "driver_id"
                    }

                }
            });
            console.log(data);
            _dataSource = data[0].lookup.dataSource;
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);

        }
    });
    //จบการกำหนด Column

    //Function Insert ข้อมูล driving_license
    function fnInsertDL(dataGrid) {
        dataGrid.IdTable = gbTableId;
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertDL",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                returnId = data[0].Status;
                //console.log(data);
                if (data[0].Status != "0") {
                    DevExpress.ui.notify("เพิ่มข้อมูลเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถเพิ่มข้อมูลได้", "error");
                }
            }
        });
        return returnId;
    }

    //Function Delete ข้อมูล gps_company
    function fnDeleteDL(keyItem) {
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteDL",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลเรียบร้อยแล้ว", "success");
                    returnStatus = false;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    returnStatus = true;
                }
            }
        });
        return returnStatus;
    }

    //Function Update ข้อมูล gps_company
    function fnUpdateDL(newData, keyItem) {
        //console.log(keyItem);
        newData.key = keyItem;
        newData.IdTable = gbTableId;
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateDL",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลเรียบร้อยแล้ว", "success");
                    returnStatus = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                    returnStatus = false;
                }
            }
        });
        return returnStatus;
    }

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

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileDL",
            data: fileUpload,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                fileDataPic = new FormData();
                document.getElementById("btnSave").disabled = false;
                $("#mdNewFile").modal('hide');
                console.log(data);
                if (data[0].Status != '0') {
                    fileOpen = data[0].Status;
                    DevExpress.ui.notify("Upload file success.", "success");
                } else {
                    DevExpress.ui.notify("Upload file fail", "error");
                }
            },
            error: function (error) {
                $("#mdNewFile").modal('hide');
                DevExpress.ui.notify("Upload file fail", "error");
            }
        });
    }

    $(document).on("dxclick", ".dx-datagrid-addrow-button", function () {
        var data = $.ajax({
            type: "POST",
            url: "../Home/GetDriverFull",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (dataLookup) {
                data_lookup_number_car = dataLookup;
                

            }
        }).responseJSON;
        
        var arr = {
            dataSource: data,
            displayExpr: "driver_name",
            valueExpr: "driver_id"
        }
        dataGrid.option('columns[0].lookup', arr);
    });

});