﻿var itemEditing = [];
//var itemEditingPermission = [];
var gbE;
var fileDataPdf;
var fileOpen;

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;
$(function () {

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
            console.log(gbE.currentSelectedRowKeys[0].lmr_id);
            var files = e.value;
            fileDataPdf = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPdf.append('file', file);
                });
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].lmr_id);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

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
        editing: {
            mode: "popup",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            form: {
                items: itemEditing,
                colCount: 6,
            },
            popup: {
                title: "ใบอนุญาตลุ่มน้ำโขง",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window },
            },
            useIcons: true,
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            fileOpen = e.currentSelectedRowKeys[0].lmr_path;
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
        onRowUpdating: function (e) {
            fnUpdateLmr(e.newData, e.key.lmr_id);
        },
        onRowInserting: function (e) {
            e.data.lmr_id = fnInsertLmr(e.data);
            console.log(e.data.lmr_id);
        },
        onRowRemoving: function (e) {
            fnDeleteLmr(e.key.lmr_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                container.append($('<div class="gc"></div>'));
                var gc;
                gc = $(".gc").dxDataGrid({
                    showBorders: true,
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    //groupPanel: {
                    //    visible: true
                    //},
                    editing: {
                        mode: "popup",
                        allowDeleting: true,
                        allowAdding: true,
                        useIcons: true,
                        //form: {
                        //    items: itemEditingPermission,
                        //    colCount: 6,
                        //},
                        popup: {
                            title: "รถที่อยู่ในใบอนุญาตลุ่มน้ำโขง",
                            showTitle: true,
                            width: "70%",
                            position: { my: "center", at: "center", of: window },
                        },
                    },
                    onRowInserting: function (e) {
                        e.data.lmr_id = gbE.currentSelectedRowKeys[0].lmr_id;
                        e.data.lmrp_id = fnInsertLmrPermission(e.data);
                    },
                    onRowRemoving: function (e) {
                        console.log(e);
                        fnDeleteLmrPermission(e.key.lmrp_id);
                    },
                    onContentReady: function (e) {
                        var $btnView = $('<div id="btnView" class="mr-2">').dxButton({
                            icon: 'exportpdf', //or your custom icon
                            onClick: function () {
                                //On Click
                                if (fileOpen != null) {
                                    window.open(fileOpen, '_blank');
                                }
                            }
                        });
                        if (e.element.find('#btnView').length == 0)
                            e.element
                                .find('.dx-toolbar-after')
                                .prepend($btnView);

                        var $btnUpdate = $('<div id="btnUpdate" class="mr-2">').dxButton({
                            icon: 'upload',
                            onClick: function () {
                                cf.reset();
                                $("#mdNewFile").modal();
                            }
                        });
                        if (e.element.find('#btnUpdate').length == 0)
                            e.element
                                .find('.dx-toolbar-after')
                                .prepend($btnUpdate);
                    },

                }).dxDataGrid("instance");

                $.ajax({
                    type: "POST",
                    url: "../Home/GetColumnChooserLmr",
                    contentType: "application/json; charset=utf-8",
                    data: "{table_id: 25}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        console.log(data);
                        $.ajax({
                            type: "POST",
                            url: "../Home/GetNumberCar",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (dataLookup) {
                                data[0].setCellValue = function (rowData, value) {
                                    var dataNew = [];
                                    $.each(dataLookup, function () {
                                        if (this.license_id == value) {
                                            dataNew.push(this);
                                        }
                                    });
                                    rowData.license_id_head = value;
                                    rowData.license_car_head = dataNew[0].license_car;
                                }
                                data[0].lookup = {
                                    dataSource: dataLookup,
                                    displayExpr: "number_car",
                                    valueExpr: "license_id"
                                }
                                data[1].allowEditing = false

                                data[2].setCellValue = function (rowData, value) {
                                    var dataNew = [];

                                    $.each(dataLookup, function () {
                                        if (this.license_id == value) {
                                            dataNew.push(this);
                                        }
                                    });
                                    rowData.license_id_tail = value;
                                    rowData.license_car_tail = dataNew[0].license_car;
                                }

                                data[2].lookup = {
                                    dataSource: function (options) {
                                        console.log(options);
                                        return {
                                            store: dataLookup,
                                            filter: options.data ? ["!", ["license_id", "=", options.data.license_id_head]] : null
                                        };
                                    },
                                    displayExpr: "number_car",
                                    valueExpr: "license_id"
                                }
                                data[3].allowEditing = false
                            }
                        });
                        console.log(data);
                        gc.option('columns', data);                        
                    },
                });
                $.ajax({
                    type: "POST",
                    url: "../Home/GetLmrPermission",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var dataTemp = new DevExpress.data.DataSource({
                            store: new DevExpress.data.ArrayStore({
                                data: data
                            }),
                            filter: ["lmr_id", "=", gbE.currentSelectedRowKeys[0].lmr_id]
                        });
                        gc.option('dataSource', dataTemp);
                        //gc.option('dataSource', data);
                        console.log(data);
                    }
                });
            }
        },

    }).dxDataGrid('instance');

    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserLmr",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 23}",
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
                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id") {
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
        url: "../Home/GetLmr",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var d1 = parseJsonDate(data[i].lmr_expire);
                data[i].lmr_expire = d1
                var d2 = parseJsonDate(data[i].lmr_start);
                data[i].lmr_start = d2;
            }
            //console.log(data);
            dataGrid.option('dataSource', data);
        }
    });

    function fnInsertLmr(dataGrid) {
        console.log(dataGrid);
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertLmr",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลในอนุญาตลุ่มน้ำโขงเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล" + error, "error");
            }
        });
        return returnId;
    }


    function fnUpdateLmr(newData, keyItem) {
        console.log(keyItem);
        newData.lmr_id = keyItem;
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLmr",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลใบอนุญาติเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                }
            }
        });
    }

    function fnDeleteLmr(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLmr",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาตลุ่มน้ำโขงเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            }
        });
    }

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileLmr",
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

    function fnInsertLmrPermission(dataGrid) {
        var returnId = 0;
        console.log(dataGrid);
        $.ajax({
            type: "POST",
            url: "../Home/InsertLmrPermission",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มการรถในใบอนุญาตลุ่มน้ำโขงเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }

    function fnDeleteLmrPermission(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLmrPermission",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาตลุ่มน้ำโขงเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            }
        });
    }

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});

//$(function () {
//    var gc;

//    $("#gridContainer").dxDataGrid({
//        dataSource: employees,
//        keyExpr: "ID",
//        showBorders: true,
//        searchPanel: {
//            visible: true,
//            width: 240,
//            placeholder: "Search..."
//        },
//        selection: {
//            mode: "single"
//        },
//        editing: {
//            mode: "row",
//            allowUpdating: true,
//            allowDeleting: true,
//            allowAdding: true,
//            useIcons: true,
//        },
//        onSelectionChanged: function (e) {
//            e.component.collapseAll(-1);
//            e.component.expandRow(e.currentSelectedRowKeys[0]);
//        },
//        onContentReady: function (e) {
//            if (!e.component.getSelectedRowKeys().length)
//                e.component.selectRowsByIndexes(0);
//        },
//        onCellClick: function(e) {
//            console.log(e);
//        },
//        columns: [{
//            text: "สถานะ",
//            type: "buttons",
//            width: 110,
//            caption: "PDF",
//            buttons: [{
//                hint: "View",
//                icon: "download",
//                visible: true,
//                onClick: function (e) {
//                    console.log("Open");
//                }
//            }, {
//                hint: "Upload",
//                icon: "upload",
//                visible: true,
//                onClick: function (e) {
//                    console.log("Upload");
//                }
//            }]
//        }, {
//            dataField: "Prefix",
//            caption: "ใบอนุญาตเลขที่",
//        },
//        {
//            dataField: "FirstName",
//            caption: "รหัสเมือง",
//        },
//        {
//            dataField: "LastName",
//            caption: "วันที่อนุญาต",
//        }, {
//            dataField: "Position",
//            caption: "วันหมดอายุ",
//        }, {
//            dataField: "State",
//            caption: "เงินพิเศษ",
//        }, {
//            dataField: "BirthDate",
//            caption: "สถานะ",
//        }, {
//            type: "buttons",
//            width: 110,
//            buttons: ["edit", "delete"]
//        }, ],
//        masterDetail: {
//            enabled: false,
//            template: function (container, options) {
//                container.append($('<div class="gc"></div>'));
//                gc = $(".gc").dxDataGrid({
//                    dataSource: customers,
//                    searchPanel: {
//                        visible: true,
//                        width: 240,
//                        placeholder: "Search..."
//                    },
//                    editing: {
//                        mode: "row",
//                        allowUpdating: true,
//                        allowDeleting: true,
//                        allowAdding: true,
//                        useIcons: true,
//                    },
//                    onContentReady: function (e) {
//                        var $btnView = $('<div id="btnView" class="mr-3">').dxButton({
//                            icon: 'exportpdf', //or your custom icon
//                            onClick: function () {
//                                //On Click
//                            }
//                        });
//                        if (e.element.find('#btnView').length == 0)
//                            e.element
//                                .find('.dx-toolbar-after')
//                                .prepend($btnView);

//                        var $btnUpdate = $('<div id="btnUpdate" class="mr-3">').dxButton({
//                            icon: 'upload', //or your custom icon
//                            onClick: function () {
//                                //On Click
//                            }
//                        });
//                        if (e.element.find('#btnUpdate').length == 0)
//                            e.element
//                                .find('.dx-toolbar-after')
//                                .prepend($btnUpdate);

//                        //var $btnDelete = $('<div id="btnDelete" class="mr-3">').dxButton({
//                        //    icon: 'trash', //or your custom icon
//                        //    onClick: function () {
//                        //        //On Click
//                        //    }
//                        //});
//                        //if (e.element.find('#btnDelete').length == 0)
//                        //    e.element
//                        //        .find('.dx-toolbar-after')
//                        //        .prepend($btnDelete);
//                    },
//                    columns: [{
//                        dataField: "CompanyName",
//                        caption: "เบอร์รถหัว",
//                    },
//                    {
//                        dataField: "City",
//                        caption: "ทะเบียนหัว"
//                    },
//                    {
//                        dataField: "State",
//                        caption: "เบอร์รถท้าย"
//                    },
//                    {
//                        dataField: "Phone",
//                        caption: "ทะเบียนท้าย"
//                    }],
//                    showBorders: true
//                }).dxDataGrid("instance");
//            }
//        },
        
//    });


//});
