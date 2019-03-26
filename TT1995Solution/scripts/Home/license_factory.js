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
var gbTableId = '30';
var fileOpen;

$(function () {
    $("a:contains('เข้าโรงงาน')").addClass("active");
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
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].license_factory_id);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    function getLicenseFactory() {
        var dataValue = [];
        //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetLicenseFactory",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                console.log(data);
                for (var i = 0; i < data.length; i++) {
                    var d1 = parseJsonDate(data[i].expire_date);
                    data[i].expire_date = d1
                    var d2 = parseJsonDate(data[i].start_date);
                    data[i].start_date = d2;
                }
                //dataGrid.option('dataSource', data);
            }
        }).responseJSON;
        //จบการโชว์ข้อมูลทะเบียน
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

    dataGridAll = getLicenseFactory();

    var dataGrid = $("#gridContainer").dxDataGrid({
        allowColumnResizing: true,
        columnResizingMode: "widget",
        dataSource: getLicenseFactory(),
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
        }, onContentReady: function (e) {
            //filter();
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
                title: "ใบอนุญาตโรงงาน",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window },
                //onHidden: function (e) {
                //    setDefaultNumberCar();
                //}
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
        onEditingStart: function (e) {
            dataGrid.option('columns[0].allowEditing', false);
        },
        onInitNewRow: function (e) {

            //var arr = {
            //    dataSource: dataLookupFilter,
            //    displayExpr: "number_car",
            //    valueExpr: "license_id"
            //}

            //dataGrid.option('columns[0].lookup', arr);
            //console.log(dataGrid);
            dataGrid.option('columns[0].allowEditing', true);
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
        onRowUpdating: function (e) {
            e.cancel = !fnUpdateLicenseFactory(e.newData, e.key.license_factory_id);
        },
        onRowInserting: function (e) {
            //$.ajax({
            //    type: "POST",
            //    url: "../Home/GetLicenseCar",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: "{license_id: " + e.data.license_id + "}",
            //    async: false,
            //    success: function (data) {
            //        e.data.license_car = data[0].license_car;
            //        e.data.history = "ประวัติ";
            //    }
            //});
            e.data.history = "ประวัติ";
             
            var statusInsert = fnInsertLicenseFactory(e.data);
            if (statusInsert != '0') {
                e.data.license_factory_id = statusInsert;
            } else {
                e.cancel = true;
            }

            //ตัด number_car ออก
            //dataGridAll.push({ license_id: e.data.license_id, number_car: e.data.number_car });
            //filter();
            //setDefaultNumberCar();
        },
        onRowRemoving: function (e) {
            e.cancel = !fnDeleteLicenseFactory(e.key.license_factory_id);

            ////กรองอาเรย์
            //dataGridAll.forEach(function (filterdata) {
            //    dataGridAll = dataGridAll.filter(function (arr) {
            //        return arr.license_id != e.key.license_id;
            //    });
            //});

            ////push array
            //dataLookupFilter.push({ number_car: e.key.number_car, license_id: e.key.license_id });

            //setDefaultNumberCar();
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
        }
    }).dxDataGrid('instance');

    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserLicenseFactory",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 30}",
        dataType: "json",
        async: false,
        success: function (data) {
            console.log(data);
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
                    //if (item.dataField != "license_car" && item.dataField != "number_car") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                        });
                    //}
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.license_factory_id),
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
            $.ajax({
                type: "POST",
                url: "../Home/GetDriverName",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (dataLookup) {
                    data[0].setCellValue = function (rowData, value) {
                        
                        var dataNew = [];
                        $.each(dataLookup, function () {
                            if (this.driver_id == value) {
                                dataNew.push(this);
                            }
                        });
                        console.log(dataNew);
                        rowData.driver_name = value;
                        rowData.number_car = dataNew[0].number_car;
                        rowData.license_car = dataNew[0].license_car;
                        data[1].allowEditing = false;
                        data[2].allowEditing = false;
                    }
                    data[0].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "driver_name",
                        valueExpr: "driver_id"
                    }
                }
            });
            
            data[1].allowEditing = false;
            data[2].allowEditing = false;
            //_dataSource = data[0].lookup.dataSource;
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);
        },
        error: function (error) {
            console.log(error);
        }
    });

    function fnInsertLicenseFactory(dataGrid) {
        console.log(dataGrid);
        dataGrid.IdTable = gbTableId;
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertLicenseFactory",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง" && data[0].Status > 0) {
                    DevExpress.ui.notify("เพิ่มข้อมูลใบอนุญาตโรงงานเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify("กรุณากรอกข้อมูลให้ถูกต้อง", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล" + error, "error");
            }
        });
        return returnId;
    }

    function fnUpdateLicenseFactory(newData, keyItem) {
        var boolUpdate = false;
        newData.license_factory_id = keyItem;
        newData.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLicenseFactory",
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
        return boolUpdate;
    }

    function fnDeleteLicenseFactory(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLicenseFactory",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลใบอนุญาตโรงงาน เรียบร้อยแล้ว", "success");
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
            url: "../Home/InsertFileLicenseFactory",
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
    //function filter() {
    //    //console.log(dataGridAll);
    //    //เซ็ตอาเรย์เริ่มต้น
    //    var dataLookupAll = dataGrid._options.columns[0].lookup.dataSource;
    //    //เซ็ตอาเรย์ที่จะกรอง
    //    var filter = dataGridAll;
    //    //กรองอาเรย์
    //    filter.forEach(function (filterdata) {
    //        dataLookupAll = dataLookupAll.filter(function (arr) {
    //            return arr.license_id != filterdata.license_id;
    //        });
    //    });
    //    dataLookupFilter = dataLookupAll;
    //}
    //function setDefaultNumberCar() {
    //    var arr = {
    //        dataSource: _dataSource,
    //        displayExpr: "driver_name",
    //        valueExpr: "driver_id"
    //    }
    //    dataGrid.option('columns[0].lookup', arr);
    //}
    //$(document).on("dxclick", ".dx-datagrid-column-chooser .dx-closebutton", function () {
    //    var dataColumnVisible = "",
    //        dataColumnHide = "";
    //    var columnCount = dataGrid.columnCount(),
    //        i;
    //    for (i = 0; i < columnCount; i++) {
    //        if (dataGrid.columnOption(i, "visible")) {
    //            dataColumnVisible = dataColumnVisible + "*" + dataGrid.columnOption(i).column_id;;
    //        } else {
    //            dataColumnHide = dataColumnHide + "*" + dataGrid.columnOption(i).column_id;
    //        }
    //    }
    //    //alert(dataColumnVisible);
    //    //alert(dataColumnHide);
    //    $.ajax({
    //        type: "POST",
    //        url: "../Home/SetColumnHide",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        data: "{dataColumnVisible:'" + dataColumnVisible + "',dataColumnHide:'" + dataColumnHide + "'}",
    //        success: function (data) {
    //            if (data = 1) {
    //                //alert('Update Column Hide OK');
    //            } else {
    //                alert('Update Column Hide error!!');
    //            }
    //        }
    //    });
    //});

});