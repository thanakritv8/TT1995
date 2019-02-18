var itemEditing = [];
var gbE;

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;
$(function () {
    
    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPic);
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
            var files = e.value;
            fileDataPic = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPic.append('file', file);
                });
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('table_id', 3);
                fileDataPic.append('parentDirId', idFile);
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
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    //groupPanel: {
                    //    visible: true
                    //},
                    editing: {
                        mode: "row",
                        allowDeleting: true,
                        allowAdding: true,
                        useIcons: true,
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
                        e.data.tax_id = fnInsertTax(e.data);
                    },
                    onContentReady: function (e) {
                        var $btnView = $('<div id="btnView" class="mr-2">').dxButton({
                            icon: 'exportpdf', //or your custom icon
                            onClick: function () {
                                //On Click
                                window.open('../Path', '_blank');
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
                        data.forEach(function (item) {
                            //////////////////
                            if (item.dataField == "bit_name") {
                                data[ndata].groupIndex = 0;
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
                        console.log(data);
                        gc.option('columns', data);

                    },
                    error: function (error) {
                        console.log(error);
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
            });
            
            dataGrid.option('columns', data);

        },
        error: function (error) {
            console.log(error);
        }
    });

    

    //โชว์ข้อมูลภาษีทั้งหมดใน datagrid
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
            console.log(data);
            dataGrid.option('dataSource', data);
        }
    });
    //จบการโชว์ข้อมูลภาษี

    //Function Insert ข้อมูลภาษี
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

    //Function Update ข้อมูลภาษี
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

    //Function Delete ข้อมูลภาษี
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
                document.getElementById("lbNewFolder").value = '';
                if (data[0].Status != '0') {
                    DevExpress.ui.notify("Upload file success.", "error");
                } else {
                    DevExpress.ui.notify("Upload file fail", "error");
                }
            },
            error: function (error) {
                $("#mdNewFile").modal('hide');
                $("#mdNewFolder").modal('hide');
                document.getElementById("lbNewFolder").value = '';
                DevExpress.ui.notify("Upload file fail", "error");
            }
        });
    }

    function fnInsertBusinessInPermission(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertBusinessInPermission",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มการค้นหาเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});