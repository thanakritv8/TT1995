var itemEditing = [];
var idRowClick = '';
var cRowClick = 0;
var fileDataPdf;
var fileDataPic;
var idItem = '';
var idFile;
var name = '';
var idFK = '';
var gbE;
var dataAll;
var _dataSource;
var dataGridFull;
var dataLookupFilter;
var gbTableId = '28';
var fileOpen;

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
            console.log(gbE.currentSelectedRowKeys[0].lv8_id);
            var files = e.value;
            fileDataPdf = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPdf.append('file', file);
                });
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].lv8_id);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    var dataGrid = $("#gridContainer").dxDataGrid({
        showBorders: true,
        selection: {
            mode: "single"
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        headerFilter: {
            visible: true
        },
        columnChooser: {
            enabled: true,
            mode: "select"
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
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            form: {
                items: itemEditing,
                colCount: 6,
            },
            popup: {
                title: "ใบอนุญาต(วอ.8)",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window },
            },
            useIcons: true
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            fileOpen = e.currentSelectedRowKeys[0].path;
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
            console.log(e.newData);
            fnUpdateLv8(e.newData, e.key.lv8_id);
        },
        onRowInserting: function (e) {
            $.ajax({
                type: "POST",
                url: "../Home/GetLicenseCar",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: "{license_id: " + e.data.license_id + "}",
                async: false,
                success: function (data) {
                    e.data.license_car = data[0].license_car;
                }
            });
            e.data.lv8_id = fnInsertLv8(e.data);
        },
        onRowRemoving: function (e) {
            fnDeleteLv8(e.key.lv8_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                var currentV8 = options.data;
                console.log(currentV8);
                container.append($(
                    '<div class="row"><div class="col-2 ml-3 mr-2" id="btnView"></div><div class="col-2" id="btnUpload"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">1. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta1"></div><div class="col-2" id="btn1"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">2. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta2"></div><div class="col-2" id="btn2"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">3. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta3"></div><div class="col-2" id="btn3"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">4. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta4"></div><div class="col-2" id="btn4"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">5. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta5"></div><div class="col-2" id="btn5"></div></div>' +
                    '<div class="row"><div class="col mb-1 mt-2">6. ชื่อวัตถุอันตราย</div></div><div class="row"><div class="col-9 ml-3 mr-1" id="ta6"></div><div class="col-2" id="btn6"></div></div>'
                ));

                var nameHazmat1 = currentV8.name_hazmat1
                var nameHazmat2 = currentV8.name_hazmat2
                var nameHazmat3 = currentV8.name_hazmat3
                var nameHazmat4 = currentV8.name_hazmat4
                var nameHazmat5 = currentV8.name_hazmat5
                var nameHazmat6 = currentV8.name_hazmat6


                var ta1 = $("#ta1").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat1,
                }).dxTextArea('instance');
                var ta2 = $("#ta2").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat2
                }).dxTextArea('instance');
                var ta3 = $("#ta3").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat3
                }).dxTextArea('instance');
                var ta4 = $("#ta4").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat4
                }).dxTextArea('instance');
                var ta5 = $("#ta5").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat5
                }).dxTextArea('instance');
                var ta6 = $("#ta6").dxTextArea({
                    placeholder: 'ช่องกรอกข้อมูล',
                    value: nameHazmat6
                }).dxTextArea('instance');
                $("#btn1").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({ name_Hazmat1: ta1._options.text }, currentV8.lv8_id);
                            }
                        });
                    }
                });
                $("#btn2").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({ name_Hazmat2: ta2._options.text }, currentV8.lv8_id);
                            }
                        });
                    }
                });
                $("#btn3").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({name_Hazmat3: ta3._options.text}, currentV8.lv8_id);
                            }
                        });
                    }
                });
                $("#btn4").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({ name_Hazmat4: ta4._options.text }, currentV8.lv8_id);
                            }
                        });
                    }
                });
                $("#btn5").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({ name_Hazmat5: ta5._options.text }, currentV8.lv8_id);
                            }
                        });
                    }
                });
                $("#btn6").dxButton({
                    "icon": "save",
                    "text": "Save",
                    onClick: function () {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm changes");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnUpdateLv8({ name_Hazmat6: ta6._options.text }, currentV8.lv8_id);
                            }
                        });
                    }
                });

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
        }
    }).dxDataGrid('instance');

    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserLv8",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 28}",
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
                ndata++;
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
                }
            });
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);
        },
        error: function (error) {
            console.log(error);
        }
    });

    $.ajax({
        type: "POST",
        url: "../Home/GetLv8",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
            for (var i = 0; i < data.length; i++) {
                var d1 = parseJsonDate(data[i].lv8_expire);
                data[i].lv8_expire = d1
                var d2 = parseJsonDate(data[i].lv8_start);
                data[i].lv8_start = d2;
            }
            dataGrid.option('dataSource', data);
        }
    });

    function fnInsertLv8(dataGrid) {
        console.log(dataGrid);
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertLv8",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลใบอนุญาต(วอ.8)เรียบร้อยแล้ว", "success");
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


    function fnUpdateLv8(newData, keyItem) {
        
        newData.lv8_id = keyItem;
        console.log(newData);
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLv8",
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

    function fnDeleteLv8(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLv8",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาต(วอ.8) เรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            }
        });
    }

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileLv8",
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
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});