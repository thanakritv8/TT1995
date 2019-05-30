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
var gbTableId = '1';

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;

//ตัวแปรเก็บรูปภาพ
var gallery = [];
var gallerySelect = 0;

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
    $("a:contains('เล่มทะเบียน')").addClass("active");
    $("#setPosition").click(function () {
        console.log($("#setPosition"))
        if ($("#setPosition")[0].checked == true) {
            $("#positionSelect").removeAttr("disabled");
        } else {
            $("#positionSelect").attr("disabled", true);
        }
    });
    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            if ($("#setPosition")[0].checked == true) {
                fileDataPic.append('position', $("#positionSelect").val());
            }
            fnInsertFiles(fileDataPic);
            
        }
    });
    
    //จบการกำหนดปุ่ม
    $("#btnNewFolder").dxButton({
        onClick: function () {
            document.getElementById("btnNewFolder").disabled = true;
            var folderName = document.getElementById("lbNewFolder").value;
            if (folderName != "") {
                fileDataPic = new FormData();
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('parentDirId', idFile);
                fileDataPic.append('newFolder', folderName);
                fileDataPic.append('table_id', 1);
                fnInsertFiles(fileDataPic);
            } else {
                DevExpress.ui.notify("กรุณากรอกชื่อโฟล์เดอร์", "error");
            }
        }
    });

    $("#btnRename").click(function () {
        document.getElementById("btnRename").disabled = true;
        var folderName = document.getElementById("lbRename").value;
        if (folderName != "") {
            fileDataPic = new FormData();
            fileDataPic.append('fk_id', idFK);
            fileDataPic.append('file_id', idFile);
            fileDataPic.append('rename', folderName);
            fileDataPic.append('table_id', 1);
            fnRename(fileDataPic);
            //showDate();
        } else {
            DevExpress.ui.notify("กรุณากรอกชื่อโฟล์เดอร์", "error");
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

    //กำหนดการแสดงรูปภาพที่มาจากการคลิกรูปภาพใน treeview
    var galleryWidget = $("<div>").dxGallery({
    }).dxGallery("instance");
    //จบการกำหนดการแสดงรูปภาพ

    var galleryWidget;
    $("#popup").dxPopup({
        visible: false,
        width: 800,
        height: 600,
        contentTemplate: function (content) {
            galleryWidget = $("<div>").appendTo(content).dxGallery({
                dataSource: gallery,
                height: 500,
                loop: true,
                slideshowDelay: 2000,
                showNavButtons: true,
                showIndicator: true,
                selectedIndex: gallerySelect
            }).
            dxGallery("instance");
        }
    });
    //จบการกำหนดการแสดงรูปภาพ

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        //url: "../Home/GetColumnChooserLicense",
        url: "../Home/GetColumnChooser", //Edit By Tew 250219
        contentType: "application/json; charset=utf-8",
        data: "{gbTableId:"+ gbTableId +"}",
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.license_id),
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
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "license_id" && item.dataField != "history") {
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
                var d1 = parseJsonDate(data[i].license_date);
                data[i].license_date = d1;
                var d2 = parseJsonDate(data[i].license_expiration);
                data[i].license_expiration = d2;
            }
            dataGrid.option('dataSource', data);
        }
    });
    //จบการโชว์ข้อมูลทะเบียน

    //กำหนดการ Upload files
    var cf = $(".custom-file").dxFileUploader({
        maxFileSize: 10000000,
        multiple: false,
        allowedFileExtensions: [".pdf", ".jpg", ".jpeg", ".png"],
        accept: "image/*,.pdf",
        uploadMode: "useForm",
        onValueChanged: function (e) {
            var files = e.value;
            
            fileDataPic = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                   
                    if (file.type != "application/pdf") {
                        //Resize Pic
                        var img = document.createElement("img");
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            img.src = e.target.result;
                            img.onload = function () {
                                var canvas = document.createElement("canvas");
                                var ctx = canvas.getContext("2d");
                                ctx.drawImage(img, 0, 0);
                                var MAX_WIDTH = 800;
                                var MAX_HEIGHT = 600;
                                var width = img.width;
                                var height = img.height;

                                if (width > height) {
                                    if (width > MAX_WIDTH) {
                                        height *= MAX_WIDTH / width;
                                        width = MAX_WIDTH;
                                    }
                                } else {
                                    if (height > MAX_HEIGHT) {
                                        width *= MAX_HEIGHT / height;
                                        height = MAX_HEIGHT;
                                    }
                                }
                                canvas.width = width;
                                canvas.height = height;
                                var ctx = canvas.getContext("2d");
                                ctx.drawImage(img, 0, 0, width, height);
                                dataurl = canvas.toDataURL("image/jpeg");
                                fetch(dataurl)
                                    .then(res => res.blob())
                                    .then(blob => {
                                        fileDataPic.append('file', blob, file.name);
                                    });
                            }
                        }
                        reader.readAsDataURL(file);
                        $("#picCar").show();
                    } else {
                        fileDataPic.append('file', file);
                    }
                });
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('table_id', 1);
                fileDataPic.append('parentDirId', idFile);
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
                        cf.reset();
                        $("#mdNewFile").modal();
                    } else if (e.itemData.text == "New Folder") {
                        //console.log(e.file_id);
                        //document.getElementById('idNewFolder').innerHTML = idItem;
                        $("#mdNewFolder").modal();
                    } else if (e.itemData.text == "Rename") {
                        $("#mdRename").modal();
                    }
                    else if (e.itemData.text == "Delete") {
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
        allowColumnResizing: true,
        columnResizingMode: "widget",
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
            allowUpdating: boolStatus,
            allowDeleting: boolStatus,
            allowAdding: boolStatus,
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
            if (!fnUpdateLicense(e.newData, e.key.license_id)) {
                e.newData = e.oldData;
            }
            
        },
        onRowInserting: function (e) {
            var statusInsert = fnInsertLicense(e.data);
            if (statusInsert != '0') {
                e.data.license_id = statusInsert;
                e.data.history = "ประวัติ";
            } else {
                e.data = null;
            }
        },
        onRowRemoving: function (e) {
            e.cancel = !fnDeleteLicense(e.key.license_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                $("#picCar").hide();
                $("#positionSelect").attr("disabled", true);
                //สร้าง id treeview
                container.append($('<div id="treeview"></div>'));
                var itemData = fnGetFiles(options.key.license_id);
                //เก็บข้อมูล treeview ไว้ในตัวแปรชื่อ treeview
                treeview = $("#treeview").dxTreeView({
                    dataStructure: "plain",
                    parentIdExpr: "parentDirId",
                    keyExpr: "file_id",
                    displayExpr: "name_file",
                    height: "150px",
                    //คลิกโชว์รูปภาพแบบ Gallery
                    onItemClick: function (e) {
                        console.log(e);
                        gallery = [];
                        itemData = fnGetFiles(options.key.license_id);
                        var item = e.node.itemData;
                        console.log(e);
                        if (item.path_file) {
                            itemData.forEach(function (itemFiles) {
                                if (itemFiles.path_file && itemFiles.type_file == "pic" && itemFiles.parentDirId == item.parentDirId && itemFiles.fk_id == item.fk_id) {
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
                                console.log(itemData);
                                galleryWidget.option("dataSource", gallery);
                                galleryWidget.option("selectedIndex", gallerySelect);
                                $("#popup").dxPopup("show");
                            } else {
                                window.open(item.path_file, '_blank');
                            }
                        }
                    },
                    //โชว์รายการคลิกขวา
                    onItemContextMenu: function (e) {
                        var item = e.node.itemData;
                        if (item.file_id) {
                            name = item.name_file
                            var type_file = item.type_file
                            idFK = item.fk_id;
                            idFile = item.file_id;
                            if (name == "Root") {
                                OptionsMenu = contextMenuItemsRoot;
                            }else if (type_file == "folder") {
                                OptionsMenu = contextMenuItemsFolder;
                            } else {
                                OptionsMenu = contextMenuItemsFile;
                            }
                            getContextMenu();
                        }
                    },
                    onItemExpanded: function (e) {
                        var item = e.itemData;
                        showDate();
                        //console.log(item);
                        //ExpandedAndCollapsed(item.id, 1);                            
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
   
    //Get Files from controller Home/GetFiles
    function fnGetFiles(license_id) {
        var itemData;
        $.ajax({
            type: "POST",
            url: "../Home/GetFiles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{table_id: 1}",
            async: false,
            success: function (data) {
                data.push({
                    "file_id": "root",
                    "fk_id": license_id,
                    "name_file": "Root",
                    "type_file": "folder",
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
        console.log(itemData);
        var nItem = 0;
        itemData.forEach(function (item) {
            if (item.file_id == idFile) {
                itemData[nItem].expanded = true;
            }
            nItem++;
        })
        var dts = new DevExpress.data.DataSource({
            store: new DevExpress.data.ArrayStore({
                key: "file_id",
                data: itemData
            }),
            filter: ["fk_id", "=", license_id]
        });
        treeview.option("dataSource", dts);
        showDate();
    }

    //20190322 Edit Show position
    
    function showDate() {

        var dataNode = $(".dx-treeview-node-is-leaf");
        console.log(dataNode);
        for (var i = 0; i < dataNode.length; i++) {
            var str = dataNode[i].innerHTML;
            if (str.indexOf("badge") == -1) {
                var positionStart = str.indexOf("<span>");
                var positionEndStart = str.indexOf("</span>") + 7;
                var subStr = str.substring(positionStart, positionEndStart);
                console.log(treeview._options.items);
                var data_filter = treeview._options.items.filter(function (x) { return x.file_id == dataNode[i].dataset.itemId; })
                console.log(dataNode[i].dataset.itemId);
                
                //var data_filter = treeview._options.items.filter(element => element.file_id == dataNode[i].dataset.itemId)
                console.log(data_filter);
                if (data_filter[0].position != null && data_filter[0].position != '') {
                    var dp;
                    if (data_filter[0].position == 1) {
                        dp = 'ด้านหน้า';
                    } else if (data_filter[0].position == 2) {
                        dp = 'ด้านหลัง';
                    } else if (data_filter[0].position == 3) {
                        dp = 'ด้านซ้าย';
                    } else if (data_filter[0].position == 4) {
                        dp = 'ด้านขวา';
                    } else if (data_filter[0].position == 5) {
                        dp = 'ด้านหน้าข้างขวา';
                    } else if (data_filter[0].position == 6) {
                        dp = 'ด้านหน้าข้างซ้าย';
                    } else if (data_filter[0].position == 7) {
                        dp = 'ด้านหลังข้างขวา';
                    } else if (data_filter[0].position == 8) {
                        dp = 'ด้านหลังข้างซ้าย';
                    }
                    $(".dx-treeview-node-is-leaf")[i].innerHTML = str.replace(subStr, subStr + '<span class="badge badge-success ml-1 mr-2 mt-1" data-toggle="tooltip" title="ตำแหน่ง" style="float:right;">' + dp + '</span>');
                }
            }
        }
    }


    //Function Insert ข้อมูลทะเบียน
    function fnInsertLicense(dataGrid) {
        var returnId = 0;
        dataGrid.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/InsertLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }
    

    //Function Update ข้อมูลทะเบียน
    function fnUpdateLicense(newData, keyItem) {
        var boolUpdate = false;
        newData.key = keyItem;
        newData.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                    boolUpdate = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                    boolUpdate = false;
                }
            }
        });
        return boolUpdate;
    }

    //Function Delete ข้อมูลทะเบียน
    function fnDeleteLicense(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteLicense",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
                    boolDel = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    boolDel = false;
                }
            }
        });
        return boolDel;
    }

    //Function Insert file in treeview
    function fnInsertFiles(fileUpload) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertFileLicense",
            data: fileUpload,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                fileDataPic = new FormData();
                document.getElementById("btnSave").disabled = false;
                $("#mdNewFile").modal('hide');
                $("#mdNewFolder").modal('hide');
                $("#picCar").hide();
                $("#positionSelect").attr("disabled", true);
                document.getElementById("lbNewFolder").value = '';
                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status);
                    fnChangeTreeview(data[0].Status, itemData);
                } else {
                    DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
                }
            },
            error: function (error) {
                $("#mdNewFile").modal('hide');
                $("#mdNewFolder").modal('hide');
                document.getElementById("lbNewFolder").value = '';
                DevExpress.ui.notify("ไม่สามารถเพิ่มไฟล์ได้", "error");
            }
        });
    }

    //Function Rename file in treeview
    function fnRename(fileUpload) {
        
            $.ajax({
                type: "POST",
                url: "../Home/fnRenameLicense",
                data: fileUpload,
                dataType: "json",
                contentType: false,
                processData: false,
                success: function (data) {

                    if (data[0].Status != '0') {
                        var itemData = fnGetFiles(data[0].Status);
                        fnChangeTreeview(data[0].Status, itemData);
                        
                    } else {
                        DevExpress.ui.notify("ไม่สามารถแก้ไขได้", "error");
                    }
                    document.getElementById('lbRename').value = '';
                    $('#mdRename').modal('hide');
                    document.getElementById("btnRename").disabled = false;
                },
                error: function (error) {
                    DevExpress.ui.notify(error, "error");
                }
            });
        //} else {
        //    $('#mdNewFolder').modal('hide');
        //    alert("กรุณากรอกข้อมูลให้ครบ");
        //}
    }

    //Function Delete file in treeview
    function fnDeleteFiles(file_id) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteFileLicense",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + file_id + "'}",
            dataType: 'json',
            success: function (data) {
                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status);
                    fnChangeTreeview(data[0].Status, itemData);
                    DevExpress.ui.notify("ลบไฟล์เรียบร้อยแล้ว", "success");
                } else {
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
        //console.log(jsonDateString);
        if (jsonDateString != null) {
            return new Date(parseInt(jsonDateString.replace('/Date(', '')));
        }
    }

    //Function Open Pdf
    //function pre_pdf(path_file) {
    //    $('#pre_pdf').attr('href', path_file);
    //    $('#pre_pdf').EZView();
    //    $('#pre_pdf').click();
    //}

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