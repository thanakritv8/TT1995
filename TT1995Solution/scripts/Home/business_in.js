var itemEditing = [];
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
            console.log(gbE.currentSelectedRowKeys[0].business_id);
            var files = e.value;
            fileDataPdf = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPdf.append('file', file);
                });
                fileDataPdf.append('fk_id', gbE.currentSelectedRowKeys[0].business_id);
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
                title: "x",
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
        onRowUpdating: function (e) {
            fnUpdateBusinessIn(e.newData, e.key.business_id);
        },
        onRowInserting: function (e) {
            e.data.business_id = fnInsertBusinessIn(e.data);
        },
        onRowRemoving: function (e) {
            fnDeleteBusinessIn(e.key.business_id);
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
                            title: "รถที่อยู่ในประกอบการภายในประเทศ",
                            showTitle: true,
                            width: "70%",
                            position: { my: "center", at: "center", of: window },
                        },
                    },
                    onRowInserting: function (e) {                        
                        e.data.business_id = gbE.currentSelectedRowKeys[0].business_id;
                        e.data.bip_id = fnInsertBusinessInPermission(e.data);
                    },
                    onRowRemoving: function (e) {
                        console.log(e);
                        fnDeleteBusinessInPermission(e.key.bip_id);
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
                    url: "../Home/GetColumnChooserBusinessIn",
                    contentType: "application/json; charset=utf-8",
                    data: "{table_id: 14}",
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        var ndata = 0;
                        var numType = 0;
                        data.forEach(function (item) {

                            if (item.dataField == "bit_name") {
                                data[ndata].groupIndex = 0;
                            }
                            if (ndata != 0 && item.dataField != "bit_name") {
                                data[ndata].allowEditing = false;
                            } else if (item.dataField == "bit_name") {
                                numType = ndata;
                            }
                            ndata++;
                            //itemEditingPermission.push({
                            //    colSpan: item.colSpan,
                            //    dataField: item.dataField,
                            //    width: "100%",
                            //    editorOptions: {
                            //        disabled: false
                            //    },
                            //});
                        });
                        //console.log(itemEditingPermission);
                        $.ajax({
                            type: "POST",
                            url: "../Home/GetNumberCarBusiness",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (dataLookup) {
                                data[0].setCellValue = function (rowData, value) {
                                    console.log(rowData);
                                    console.log(value);
                                    var dataNew = [];
                                    $.each(dataLookup, function () {
                                        if (this.number_car == value) {
                                            dataNew.push(this);
                                        }
                                    });
                                    rowData.number_car = dataNew[0].number_car;
                                    rowData.license_car = dataNew[0].license_car;
                                    rowData.brand_car = dataNew[0].brand_car;
                                    rowData.number_body = dataNew[0].number_body;
                                    rowData.number_engine = dataNew[0].number_engine;
                                    console.log(dataNew[0].tax_expire);
                                    var d1 = parseJsonDate(dataNew[0].tax_expire);
                                    rowData.tax_expire = d1;
                                    rowData.style_car = dataNew[0].style_car;
                                }
                                
                                data[0].lookup = {
                                    dataSource: dataLookup,
                                    displayExpr: "number_car",
                                    valueExpr: "number_car"
                                };
                                $.ajax({
                                    type: "POST",
                                    url: "../Home/GetBusinessInType",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    async: false,
                                    success: function (dataType) {
                                        data[numType].lookup = {
                                            dataSource: dataType,
                                            displayExpr: "bit_name",
                                            valueExpr: "bit_id"
                                        }
                                    }
                                });                                
                            }
                        });
                        console.log(data);
                        gc.option('columns', data);
                    },
                    
                });
                $.ajax({
                    type: "POST",
                    url: "../Home/GetBusinessInPermission",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        for (var i = 0; i < data.length; i++) {
                            
                            var d1 = parseJsonDate(data[i].tax_expire);
                            data[i].tax_expire = d1
                        }
                        //console.log(data);
                        var dataTemp = new DevExpress.data.DataSource({
                            store: new DevExpress.data.ArrayStore({
                                data: data
                            }),
                            filter: ["business_id", "=", gbE.currentSelectedRowKeys[0].business_id]
                        });
                        gc.option('dataSource', dataTemp);

                        console.log(gc);
                    }
                });
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
        url: "../Home/GetBusinessIn",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var d1 = parseJsonDate(data[i].business_expire);
                data[i].business_expire = d1
                var d2 = parseJsonDate(data[i].business_start);
                data[i].business_start = d2;
            }
            //console.log(data);
            dataGrid.option('dataSource', data);
        }
    });

    function fnInsertBusinessIn(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertBusinessIn",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลประกอบการภายในประเทศเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล", "error");
            }
        });
        return returnId;
    }


    function fnUpdateBusinessIn(newData, keyItem) {
        newData.business_id = keyItem;
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Home/UpdateBusinessIn",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลประกอบการภายในประเทศเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                }
            }
        });
    }

    function fnDeleteBusinessIn(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteBusinessIn",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลประกอบการภายในประเทศเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            }
        });
    }

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileBusinessIn",
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

    function fnInsertBusinessInPermission(dataGrid) {
        var returnId = 0;
        console.log(dataGrid);
        $.ajax({
            type: "POST",
            url: "../Home/InsertBusinessInPermission",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มการรถในประกอบเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }

    function fnDeleteBusinessInPermission(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteBusinessInPermission",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลประกอบการภายในประเทศเรียบร้อยแล้ว", "success");
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