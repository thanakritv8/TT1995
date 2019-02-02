var itemEditing = [];
var idRowClick = '';
var cRowClick = 0;
var fileDataPdf;
var fileDataPic;
var idItem = '';
var name = '';
var idFK = '';
var gbE;

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;

//ตัวแปรเก็บรูปภาพ
var gallery = [];
var gallerySelect = 0;

//คลิกขวาโชว์รายการ
var contextMenuItemsFolder = [
        { text: 'New File' }
];
var contextMenuItemsFile = [
    { text: 'Delete' }
];
var OptionsMenu = contextMenuItemsFolder;

$(function () {
    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPic);
        }
    });
    //จบการกำหนดปุ่ม

    //กำหนดการแสดงรูปภาพที่มาจากการคลิกรูปภาพใน treeview
    var galleryWidget = $("#gallery").dxGallery({
        dataSource: gallery,
        loop: true,
        showIndicator: true,
        selectedIndex: gallerySelect
    }).dxGallery("instance");
    //จบการกำหนดการแสดงรูปภาพ

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
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
                ndata++;
                //จบการตั้งค่าโชว์ Dropdown
                
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
                    }else {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                        });
                    }
                }
                //จบรายการหน้าโชว์หน้าเพิ่มและแก้ไข
            });
            
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);
        }
    });
    //จบการกำหนด Column

    //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
    $.ajax({
        type: "POST",
        url: "../Home/GetLicense",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var d = parseJsonDate(data[i].license_date);
                data[i].license_date = d;
            }
            dataGrid.option('dataSource', data);
        }
    });
    //จบการโชว์ข้อมูลทะเบียน

    //กำหนดการ Upload files
    var cf = $(".custom-file").dxFileUploader({
        maxFileSize: 4000000,
        multiple: true,
        allowedFileExtensions: [".jpg", ".jpeg", ".png"],
        accept: "image/*",
        uploadMode: "useForm",
        onValueChanged: function (e) {
            var files = e.value;
            fileDataPic = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    fileDataPic.append('file', file);
                });
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('type', idFile);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    //กำหนดรายการคลิกขวาใน treeview และเงื่อนไขกรณีที่มีการคลิกเลือกรายการ
    getContextMenu();
    function getContextMenu() {
        $("#context-menu").dxContextMenu({
            dataSource: OptionsMenu,
            width: 200,
            target: "#treeview",
            onItemClick: function (e) {
                if (!e.itemData.items) {
                    if (e.itemData.text == "New File") {
                        if (idFile == "pdf") {
                            cf.option("allowedFileExtensions", [".pdf"]);
                            cf.option("accept", ".pdf");
                        } else {
                            cf.option("allowedFileExtensions", [".jpg", ".jpeg", ".png"]);
                            cf.option("accept", "image/*");
                        }
                        $("#mdNewFile").modal();
                    } else if (e.itemData.text == "Delete") {
                        var result = DevExpress.ui.dialog.confirm("Are you sure?", "Confirm delete");
                        result.done(function (dialogResult) {
                            if (dialogResult) {
                                fnDeleteFiles(idFile);
                            }
                        });
                    }
                }
            }
        });
    }
    //จบการกำหนดรายการคลิกขวา

    //ตัวแปร treeview ใช้เพื่อเอาไป update ข้อมูลใน treeview
    var treeview;

    //กำหนดการแสดงผลของ datagrid
    var dataGrid = $("#gridContainer").dxDataGrid({
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
                title: "รายการจดทะเบียน",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window },
            },
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
        onRowUpdating: function (e) {
            fnUpdateLicense(e.newData, e.key.license_id);
        },
        onRowInserting: function (e) {
            fnInsertLicense(e.data);
        },
        onRowRemoving: function (e) {
            fnDeleteLicense(e.key.license_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                //สร้าง id treeview
                container.append($('<div id="treeview"></div>'));
                var itemData = fnGetFiles(options.key.license_id);
                //เก็บข้อมูล treeview ไว้ในตัวแปรชื่อ treeview
                treeview = $("#treeview").dxTreeView({
                    dataStructure: "plain",
                    parentIdExpr: "type_file",
                    keyExpr: "file_id",
                    displayExpr: "name_file",
                    height: "150px",
                    //คลิกโชว์รูปภาพแบบ Gallery
                    onItemClick: function (e) {
                        gallery = [];
                        var item = e.itemData;
                        if (item.path_file) {
                            itemData.forEach(function (itemFiles) {
                                if (itemFiles.path_file && itemFiles.type_file == "pic" && itemFiles.fk_id == item.fk_id) {
                                    gallery.push(itemFiles.path_file);
                                }
                            });
                            var nGallery = 0;
                            gallery.forEach(function (itemFiles) {
                                if (itemFiles == item.path_file) {
                                    gallerySelect = nGallery;
                                }
                                nGallery++;
                            });
                            if (item.type_file == "pic") {
                                galleryWidget.option("dataSource", gallery);
                                galleryWidget.option("selectedIndex", gallerySelect);

                                $("#mdShowPic").modal();
                            } else {
                                window.open(item.path_file, '_blank');
                            }
                        }
                    },
                    //โชว์รายการคลิกขวา
                    onItemContextMenu: function (e) {
                        var item = e.itemData;
                        if (item.file_id) {
                            name = item.name_file
                            idFK = item.fk_id;
                            idFile = item.file_id;
                            if (name == "PDF" || name == "PICTURE") {
                                OptionsMenu = contextMenuItemsFolder;
                            } else {
                                OptionsMenu = contextMenuItemsFile;
                            }
                            getContextMenu();
                        }
                    },
                }).dxTreeView("instance");
                //จบการสร้าง treeview
                fnChangeTreeview(options.key.license_id, itemData);
            }
        },

        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            isFirstClick = false;
        },
        onRowClick: function (e) {
            if (gbE.currentSelectedRowKeys[0].license_id == e.key.license_id && isFirstClick && rowIndex == e.rowIndex) {
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
   
    //Get Files from controller Home/GetFiles
    function fnGetFiles(license_id) {
        var itemData;
        $.ajax({
            type: "POST",
            url: "../Home/GetFiles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                data.push({
                    "file_id": "pdf",
                    "fk_id": license_id,
                    "name_file": "PDF",
                    "icon": "../Img/folder.png"
                });
                data.push({
                    "file_id": "pic",
                    "fk_id": license_id,
                    "name_file": "PICTURE",
                    "icon": "../Img/folder.png"
                });
                itemData = data;
            }
        });
        return itemData;
    }
    //End get Files

    //function เปลี่ยนเปลี่ยนข้อมูลเมื่อมีการ เพิ่ม ลบ ไฟล์
    function fnChangeTreeview(license_id, itemData) {
        var dts = new DevExpress.data.DataSource({
            store: new DevExpress.data.ArrayStore({
                key: "file_id",
                data: itemData
            }),
            filter: ["fk_id", "=", license_id]
        });
        treeview.option("dataSource", dts);
    }

    //Function Insert ข้อมูลทะเบียน
    function fnInsertLicense(dataGrid) {
        
        $.ajax({
            type: "POST",
            url: "../Home/InsertLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == "1") {
                    DevExpress.ui.notify("เพิ่มข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
    }

    //Function Update ข้อมูลทะเบียน
    function fnUpdateLicense(newData, keyItem) {
        newData.key = keyItem;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                }
            }
        });
    }

    //Function Delete ข้อมูลทะเบียน
    function fnDeleteLicense(keyItem) {
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLicense",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถข้อมูลได้", "error");
                }
            }
        });
    }

    //Function Insert file in treeview
    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFile",
            data: fileUpload,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                fileDataPic = new FormData();
                document.getElementById("btnSave").disabled = false;
                $("#mdNewFile").modal('hide');
               
                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status);
                    fnChangeTreeview(data[0].Status, itemData);
                } else {
                    DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
            }
        });
    }

    //Function Delete file in treeview
    function fnDeleteFiles(file_id) {
        console.log(file_id);
        $.ajax({
            type: "POST",
            url: "../Home/DeleteFile",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + file_id + "'}",
            dataType: 'json',
            success: function (data) {
                if (data[0].Status == 1) {
                    gbE.component.collapseAll(-1);
                    gbE.component.expandRow(gbE.key);
                    DevExpress.ui.notify("ลบไฟล์เรียบร้อยแล้ว", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("ไม่สามารถลบไฟล์ได้", "error");
            }
        });
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

});