var itemEditing = [];
//var itemEditingPermission = [];
var gbE;
var fileDataPdf;
var fileOpen;

var gbTableId = '22';

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;
$(function () {
    $("a:contains('กัมพูชา')").addClass("active");
    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPdf);
        }
    });

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

    //กำหนดการ Upload files
    var cf = $(".custom-file").dxFileUploader({
        maxFileSize: 4000000,
        multiple: true,
        allowedFileExtensions: [".pdf"],
        accept: ".pdf",
        uploadMode: "useForm",
        onValueChanged: function (e) {
            console.log(gbE.currentSelectedRowKeys[0].lc_id);
            var files = e.value;
            fileDataPdf = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPdf.append('file', file);
                });
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].lc_id);
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
        allowColumnResizing: true,
        columnResizingMode: "widget",
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
                title: "ใบอนุญาตกัมพูชา",
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
            fileOpen = e.currentSelectedRowKeys[0].lc_path;
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
            e.cancel = !fnUpdateLc(e.newData, e.key.lc_id);
        },
        onRowInserting: function (e) {
            var statusInsert = fnInsertLc(e.data);
            if (statusInsert != '0') {
                e.data.lc_id = statusInsert;
                e.data.history = 'View';
            } else {
                e.cancel = true;
            }
        },
        onRowRemoving: function (e) {
            e.cancel = !fnDeleteLc(e.key.lc_id);
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
                        allowDeleting: boolStatus,
                        allowAdding: boolStatus,
                        useIcons: true,
                        //form: {
                        //    items: itemEditingPermission,
                        //    colCount: 6,
                        //},
                        popup: {
                            title: "รถที่อยู่ในใบอนุญาตกัมพูชา",
                            showTitle: true,
                            width: "70%",
                            position: { my: "center", at: "center", of: window },
                        },
                    },
                    onRowInserting: function (e) {
                        e.data.lc_id = gbE.currentSelectedRowKeys[0].lc_id;

                        var statusInsert = fnInsertLcPermission(e.data);
                        if (statusInsert != '0') {
                            e.data.lcp_id = statusInsert;
                        } else {
                            e.cancel = true;
                        }
                    },
                    onRowRemoving: function (e) {
                        e.cancel = !fnDeleteLcPermission(e.key.lcp_id);
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
                    url: "../Home/GetColumnChooserLc",
                    contentType: "application/json; charset=utf-8",
                    data: "{table_id: 24}",
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
                                    console.log(dataNew);
                                    rowData.license_id_tail = value;
                                    rowData.license_car_tail = dataNew[0].license_car;
                                    rowData.style_car = dataNew[0].style_car;
                                    rowData.shaft = dataNew[0].shaft;

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
                    url: "../Home/GetLcPermission",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        var dataTemp = new DevExpress.data.DataSource({
                            store: new DevExpress.data.ArrayStore({
                                data: data
                            }),
                            filter: ["lc_id", "=", gbE.currentSelectedRowKeys[0].lc_id]
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
        url: "../Home/GetColumnChooserLc",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 22}",
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.lc_id),
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
            });

            dataGrid.option('columns', data);

        },
        error: function (error) {
            console.log(error);
        }
    });

    $.ajax({
        type: "POST",
        url: "../Home/GetLc",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var d1 = parseJsonDate(data[i].lc_expire);
                data[i].lc_expire = d1
                var d2 = parseJsonDate(data[i].lc_start);
                data[i].lc_start = d2;
            }
            //console.log(data);
            dataGrid.option('dataSource', data);
        }
    });

    function getDataLc() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetLc",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d1 = parseJsonDate(data[i].lc_expire);
                    data[i].lc_expire = d1
                    var d2 = parseJsonDate(data[i].lc_start);
                    data[i].lc_start = d2;
                }
            }
        }).responseJSON;
    }

    function fnInsertLc(dataGrid) {
        var returnId = 0;
        dataGrid.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/InsertLc",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง" && data[0].Status > 0) {
                    DevExpress.ui.notify("เพิ่มข้อมูลในอนุญาตกัมพูชาเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล", "error");
            }
        });
        return returnId;
    }


    function fnUpdateLc(newData, keyItem) {
        var boolUpdate = false;
        console.log(keyItem);
        newData.lc_id = keyItem;
        newData.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLc",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลใบอนุญาติเรียบร้อยแล้ว", "success");
                    boolUpdate = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                    boolUpdate = false;
                }
            }
        });
        dataGrid.option('dataSource', getDataLc());
        dataGrid.refresh();
        return boolUpdate;
    }

    function fnDeleteLc(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLc",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาตกัมพูชาเรียบร้อยแล้ว", "success");
                    boolDel = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    boolDel = false;
                }
            }
        });
        return boolDel;
    }

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileLc",
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

    function fnInsertLcPermission(dataGrid) {
        var returnId = 0;
        console.log(dataGrid);
        $.ajax({
            type: "POST",
            url: "../Home/InsertLcPermission",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง" && data[0].Status > 0) {
                    DevExpress.ui.notify("เพิ่มการรถในใบอนุญาตกัมพูชาเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify("กรุณากรอกข้อมูลให้ถูกต้อง", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("กรุณากรอกข้อมูลให้ถูกต้อง", "error");
            }

        });
        return returnId;
    }

    function fnDeleteLcPermission(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLcPermission",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาตกัมพูชาเรียบร้อยแล้ว", "success");
                    boolDel = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    boolDel = false;
                }
            }
        });
        return boolDel;
    }

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
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