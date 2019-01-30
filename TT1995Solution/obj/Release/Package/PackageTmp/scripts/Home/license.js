﻿var itemEditing = [];
var idRowClick = '';
var cRowClick = 0;
var fileDataPdf;
var fileDataPic;
var idItem = '';
var name = '';
var idFK = '';
var gbE;

var gallery = [];
var gallerySelect = 0;

var contextMenuItemsFolder = [
        { text: 'New File' }
];
var contextMenuItemsFile = [
    { text: 'Delete' }
];
var OptionsMenu = contextMenuItemsFolder;
$(function () {

    var galleryWidget = $("#gallery").dxGallery({
        dataSource: gallery,
        loop: true,
        //showNavButtons: true,
        showIndicator: true,
        selectedIndex: gallerySelect
    }).dxGallery("instance");    

    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPic);            
        }
    });
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            var ndata = 0;
            data.forEach(function (item) {
                //Start Lookup 
                if (item.dataField == "color_car" || item.dataField == "brand_car" || item.dataField == "province") {
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
                        valueExpr: "lookup_id"
                    }
                }
                ndata++;
                //End Lookup
                
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
            });
            //itemEditing.push({
            //    template: function (data, itemElement) {
            //        itemElement.append($("<div>").attr("id", "dxPic").dxFileUploader({
            //            multiple: true,
            //            allowedFileExtensions: [".jpg", ".jpeg", ".png"],
            //            accept: "image/*",
            //            uploadMode: "useForm",
            //            onValueChanged: function (e) {
            //                var files = e.value;
            //                fileDataPic = new FormData();
            //                if (files.length > 0) {
            //                    $.each(files, function (i, file) {
            //                        fileDataPic.append('file', file);
            //                    });
            //                    fileDataPic.append('type', 'pic');
            //                }
            //            },
            //        }));
            //    },
            //    name: "dxPic",
            //    label: {
            //        text: "รูปรถ"
            //    },
            //    colSpan:3,
            //});
            //itemEditing.push({
            //    template: function (data, itemElement) {
            //        itemElement.append($("<div>").attr("id", "dxPdf").dxFileUploader({
            //            multiple: true,
            //            allowedFileExtensions: [".pdf"],
            //            accept: ".pdf",
            //            uploadMode: "useForm",
            //            onValueChanged: function (e) {
            //                var files = e.value;
            //                fileDataPdf = new FormData();
            //                if (files.length > 0) {
            //                    $.each(files, function (i, file) {
            //                        fileDataPdf.append('file', file);                               
            //                    });
            //                    fileDataPdf.append('type', 'pdf');
            //                }
            //            },
            //        }));
            //    },
            //    name: "dxPdf",
            //    label: {
            //        text: "ไฟล์ Pdf"
            //    },
            //    colSpan: 3,
            //});
            dataGrid.option('columns', data);
        }
    });

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

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

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
    }).dxFileUploader('instance')

    getContextMenu();
    function getContextMenu() {
        $("#context-menu").dxContextMenu({
            dataSource: OptionsMenu,
            width: 200,
            target: ".treeview",
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
                                console.log(idFile);
                                fnDeleteFiles(idFile);
                            }
                        });
                    }
                }
            }
        });
    }

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
                var currentEmployeeData = options.data;
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
                            "fk_id": options.key.license_id,
                            "name_file": "PDF",
                            "icon": "../Img/folder.png"
                        });
                        data.push({
                            "file_id": "pic",
                            "fk_id": options.key.license_id,
                            "name_file": "PICTURE",
                            "icon": "../Img/folder.png"
                        });
                        itemData = data;                       
                    }
                });
                $("<div>")
                    .addClass("treeview")
                    .dxTreeView({
                        dataStructure: "plain",
                        parentIdExpr: "type_file",
                        keyExpr: "file_id",
                        displayExpr: "name_file",
                        dataSource: new DevExpress.data.DataSource({
                            store: new DevExpress.data.ArrayStore({
                                key: "file_id",
                                data: itemData
                            }),
                            filter: ["fk_id", "=", options.key.license_id]
                        }),
                        height: "150px",
                        onItemClick: function (e) {
                            gallery = [];
                            var item = e.itemData;
                            if (item.path_file) {
                                //console.log(item);
                                itemData.forEach(function (itemFiles) {
                                    
                                    //console.log(itemFiles);
                                    if (itemFiles.path_file && itemFiles.type_file == "pic" && itemFiles.fk_id == item.fk_id) {
                                        gallery.push(itemFiles.path_file);
                                    }
                                });
                                var nGallery = 0;
                                console.log(gallery);
                                gallery.forEach(function (itemFiles) {
                                    console.log(itemFiles + " = " + item.path_file);
                                    if (itemFiles == item.path_file) {
                                        gallerySelect = nGallery;
                                    }
                                    nGallery++;
                                });
                                if (item.type_file == "pic") {
                                    console.log(gallerySelect);
                                    galleryWidget.option("dataSource", gallery);
                                    galleryWidget.option("selectedIndex", gallerySelect);
                                    
                                    $("#mdShowPic").modal();
                                } else {
                                    window.open(item.path_file, '_blank');
                                }
                            }
                        },
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
                    }).appendTo(container);
            }
        },
        onRowClick: function (e) {
            var component = e.component,
                prevClickTime = component.lastClickTime;
            component.lastClickTime = new Date();
            if (prevClickTime && (component.lastClickTime - prevClickTime < 300)) {
                if (idRowClick == e.key.license_id && cRowClick <= 1) {
                    e.component.collapseAll(-1);
                } else {
                    cRowClick = 0;
                    gbE = e;
                    e.component.collapseAll(-1);
                    e.component.expandRow(e.key);
                    idRowClick = e.key.license_id;
                }
                cRowClick++;
            }
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    //galleryWidget.option("dataSource", gallery);
   
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

    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFile",
            data: fileUpload,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                gbE.component.collapseAll(-1);
                gbE.component.expandRow(gbE.key);
                fileDataPic = new FormData();
                document.getElementById("btnSave").disabled = false;
                $("#mdNewFile").modal('hide');
                if (data[0].Status == "1") {
                    //DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
                } else {
                    DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
                }
            },
            error: function (error) {
                DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
            }
        });
    }

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

});