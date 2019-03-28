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
var gbTableId = '4';
var _dataSource;
var dataGridAll;
var dataLookupFilter;

var statusUpdateOrList = 0;
var statusUpdateRegistrar = 0;

var HE_or_list
var HE_registrar;

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
    $("a:contains('บันทึกเจ้าหน้าที่')").addClass("active");
    function getDataOR() {
        //โชว์ข้อมูลทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetOR",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d1 = parseJsonDate(data[i].record_date);
                    data[i].record_date = d1
                }
                //console.log(data);
                //dataGrid.option('dataSource', data);
            }
        }).responseJSON;
        //จบการโชว์ข้อมูล
    }

    function fnGetHistory(table, idOfTable) {
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

    dataGridAll = getDataOR();

    //กำหนดการแสดงผลของ datagrid
    var dataGrid = $("#gridContainer").dxDataGrid({
        allowColumnResizing: true,
        columnResizingMode: "widget",
        dataSource: dataGridAll,
        onContentReady: function (e) {
            filter();
        },
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
                title: "รายการบันทึกเจ้าหน้าที่",
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
                            if (typeof gbE != "undefined") {
                                if (statusUpdateOrList == 2) {
                                    updateOrList(gbE.data.or_id, HE_or_list.option("value"));
                                    gbE.data.or_list = HE_or_list.option("value");
                                }
                                if (statusUpdateRegistrar == 2) {
                                    updateRegistrar(gbE.data.or_id, HE_registrar.option("value"));
                                    gbE.data.registrar = HE_registrar.option("value");
                                }
                            }
                            dataGrid.saveEditData();
                        }
                    }
                }],
                onHidden: function (e) {
                    setDefaultNumberCar();
                }
            },
            useIcons: true,
        },
        "export": {
            enabled: true,
            fileName: "officer_records",
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        //headerFilter: {
        //    visible: true
        //},
        onEditingStart: function (e) {
            dataGrid.option('columns[0].allowEditing', false);
            statusUpdateOrList = 0;
            statusUpdateRegistrar = 0;
            gbE = e;
        },
        onInitNewRow: function (e) {

            var arr = {
                dataSource: dataLookupFilter,
                displayExpr: "number_car",
                valueExpr: "license_id"
            }

            dataGrid.option('columns[0].lookup', arr);

            dataGrid.option('columns[0].allowEditing', true);
        },
        onRowUpdating: function (e) {
            e.cancel = !fnUpdateOR(e.newData, e.key.or_id);
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
                    e.data.history = "ประวัติ";
                    e.data.or_list_view = "View";
                    e.data.registrar_view = "View";
                }
            });

            var statusInsert = fnInsertOR(e.data, HE_or_list.option("value"), HE_registrar.option("value"));
            if (statusInsert != '0') {
                e.data.or_list = HE_or_list.option("value");
                e.data.registrar = HE_registrar.option("value");
                e.data.or_id = statusInsert;
                //ตัด number_car ออก
                filter();
                setDefaultNumberCar();

            } else {
                e.cancel = true;
            }
            //ตัด number_car ออก
        },
        onRowRemoving: function (e) {
            if (fnDeleteOR(e.key.or_id) == true) {
                //กรองอาเรย์
                dataGridAll.forEach(function (filterdata) {
                    dataGridAll = dataGridAll.filter(function (arr) {
                        return arr.license_id != e.key.license_id;
                    });
                });

                //push array
                dataLookupFilter.push({ number_car: e.key.number_car, license_id: e.key.license_id });

                setDefaultNumberCar();
            } else {
                e.cancel = true;
            }
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
                    parentIdExpr: "parentDirId",
                    keyExpr: "file_id",
                    displayExpr: "name_file",
                    height: "150px",
                    //คลิกโชว์รูปภาพแบบ Gallery
                    onItemClick: function (e) {
                        gallery = [];
                        itemData = fnGetFiles(options.key.license_id);
                        var item = e.itemData;
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
                                //$("#mdShowPic").modal();
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
                            var type_file = item.type_file
                            idFK = item.fk_id;
                            idFile = item.file_id;
                            if (name == "Root") {
                                OptionsMenu = contextMenuItemsRoot;
                            } else if (type_file == "folder") {
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

    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
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
                fileDataPic.append('table_id', 4);
                fileDataPic.append('parentDirId', idFile);
                fileDataPic.append('newFolder', folderName);
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
            fileDataPic.append('table_id', 4);
            fileDataPic.append('file_id', idFile);
            fileDataPic.append('rename', folderName);
            fnRename(fileDataPic);
        } else {
            DevExpress.ui.notify("กรุณากรอกชื่อโฟล์เดอร์", "error");
        }
    });

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
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        data: "{gbTableId: '" + gbTableId + "'}",
        dataType: "json",
        async: false,
        success: function (data) {
            var ndata = 0;
            data.forEach(function (item) {

                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "history" && item.dataField != "or_list_view" && item.dataField != "registrar_view") {
                    if (item.dataField == "number_car") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            editorOptions: {
                                disabled: false
                            },
                        });
                    } else if (item.dataField == "or_list") {
                        itemEditing.push({
                            colSpan: 3,
                            dataField: "รายการบันทึก",
                            template: function (e, itemElement) {
                                var content = "";
                                if (typeof e.component._options.validationGroup.key.or_list != "undefined") {
                                    content = e.component._options.validationGroup.key.or_list
                                }
                                itemElement.append($('<div class="HE_or_list">' + content + '</div>'));
                                HE_or_list = $(".HE_or_list").dxHtmlEditor({
                                    height: 300,
                                    toolbar: {
                                        items: [
                                            "undo", "redo", "separator",
                                            {
                                                formatName: "size",
                                                formatValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"]
                                            },
                                            {
                                                formatName: "font",
                                                formatValues: ["Arial", "Courier New", "Georgia", "Impact", "Lucida Console", "Tahoma", "Times New Roman", "Verdana"]
                                            },
                                            "separator",
                                            "bold", "italic", "strike", "underline", "separator",
                                            "alignLeft", "alignCenter", "alignRight", "alignJustify", "separator",
                                            "color", "background"
                                        ]
                                    },
                                    onValueChanged: function (e) {
                                        if (statusUpdateOrList == 0) {
                                            statusUpdateOrList = 1;
                                        } else if (statusUpdateOrList = 1) {
                                            statusUpdateOrList = 2;
                                        }
                                    }
                                }).dxHtmlEditor("instance");
                            }
                        });
                    } else if (item.dataField == "registrar") {
                        itemEditing.push({
                            colSpan: 3,
                            dataField: "พนักงานบันทึก",
                            template: function (e, itemElement) {
                                var content = "";
                                if (typeof e.component._options.validationGroup.key.registrar != "undefined") {
                                    content = e.component._options.validationGroup.key.registrar
                                }
                                itemElement.append($('<div class="HE_registrar">' + content + '</div>'));
                                HE_registrar = $(".HE_registrar").dxHtmlEditor({
                                    height: 300,
                                    toolbar: {
                                        items: [
                                            "undo", "redo", "separator",
                                            {
                                                formatName: "size",
                                                formatValues: ["8pt", "10pt", "12pt", "14pt", "18pt", "24pt", "36pt"]
                                            },
                                            {
                                                formatName: "font",
                                                formatValues: ["Arial", "Courier New", "Georgia", "Impact", "Lucida Console", "Tahoma", "Times New Roman", "Verdana"]
                                            },
                                            "separator",
                                            "bold", "italic", "strike", "underline", "separator",
                                            "alignLeft", "alignCenter", "alignRight", "alignJustify", "separator",
                                            "color", "background"
                                        ]
                                    },
                                    onValueChanged: function (e) {
                                        if (statusUpdateRegistrar == 0) {
                                            statusUpdateRegistrar = 1;
                                        } else if (statusUpdateRegistrar = 1) {
                                            statusUpdateRegistrar = 2;
                                        }
                                    }
                                }).dxHtmlEditor("instance");
                            }
                        });
                    } else if (item.dataField != "license_car") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                        });
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.or_id),
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

                //popup
                if (item.dataField == "or_list_view") {
                    data[ndata].cellTemplate = function (container, options) {
                        $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                                .text(options.value)
                                .on('dxclick', function (e) {
                                    popup_or_list._options.contentTemplate = function (content) {
                                        var maxHeight = $("#popup_or_list .dx-overlay-content").height() - 150;
                                        content.append("<div id='html_or_list' style='max-height: " + maxHeight + "px;' ></div>");
                                    }

                                    $("#popup_or_list").dxPopup("show");

                                    $("#html_or_list").empty();
                                    $('#html_or_list').append(options.data.or_list);

                                })
                                .appendTo(container);
                    }
                }

                //popup
                if (item.dataField == "registrar_view") {
                    data[ndata].cellTemplate = function (container, options) {
                        $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                                .text(options.value)
                                .on('dxclick', function (e) {
                                    popup_registrar._options.contentTemplate = function (content) {
                                        var maxHeight = $("#popup_registrar .dx-overlay-content").height() - 150;
                                        content.append("<div id='html_registrar' style='max-height: " + maxHeight + "px;' ></div>");
                                    }

                                    $("#popup_registrar").dxPopup("show");

                                    $("#html_registrar").empty();
                                    $('#html_registrar').append(options.data.registrar);

                                })
                                .appendTo(container);
                    }
                }

                ndata++;
                //จบรายการหน้าโชว์หน้าเพิ่มและแก้ไข
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
            var filter = [{ column_id: '49' }, { column_id: '50' }];
            //กรองอาเรย์
            filter.forEach(function (filterdata) {
                data = data.filter(function (arr) {
                    return arr.column_id != filterdata.column_id;
                });
            });
            _dataSource = data[0].lookup.dataSource;
            //ตัวแปร data โชว์ Column และตั้งค่า Column ไหนที่เอามาโชว์บ้าง
            dataGrid.option('columns', data);

        },
        error: function (error) {
            console.log(error);
        }
    });
    //จบการกำหนด Column



    //กำหนดการ Upload files
    var cf = $(".custom-file").dxFileUploader({
        maxFileSize: 10000000,
        multiple: true,
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
                    } else {
                        fileDataPic.append('file', file);
                    }
                });
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('table_id', 4);
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


    //Get Files from controller Home/GetFiles
    function fnGetFiles(license_id) {
        var itemData;
        $.ajax({
            type: "POST",
            url: "../Home/GetFiles",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{table_id: 4}",
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
    function fnChangeTreeview(tax_id, itemData) {
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
            filter: ["fk_id", "=", tax_id]
        });
        treeview.option("dataSource", dts);
    }

    //Function Insert ข้อมูล
    function fnInsertOR(dataGrid, orList, registrar) {
        dataGrid.IdTable = gbTableId;
        dataGrid.or_list = orList;
        dataGrid.registrar = registrar;
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertOR",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลบันทึกเจ้าหน้าที่เรียบร้อยแล้ว", "success");
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

    //Function Update ข้อมูล
    function fnUpdateOR(newData, keyItem) {
        var boolUpdate = false;
        newData.or_id = keyItem;
        newData.IdTable = gbTableId;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateOR",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลบันทึกเจ้าหน้าที่เรียบร้อยแล้ว", "success");
                    boolUpdate = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                    boolUpdate = false;
                }
            }
        });
        return boolUpdate;
    }

    //Function Delete ข้อมูล
    function fnDeleteOR(keyItem) {
        var boolDel = false;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteOR",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลรายการบันทึกเจ้าหน้าที่เรียบร้อยแล้ว", "success");
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
            url: "../Home/InsertFileOR",
            data: fileUpload,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (data) {
                fileDataPic = new FormData();
                document.getElementById("btnSave").disabled = false;
                $("#mdNewFile").modal('hide');
                $("#mdNewFolder").modal('hide');
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
            url: "../Home/fnRenameOR",
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
    }

    //Function Delete file in treeview
    function fnDeleteFiles(file_id) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteFileOR",
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

    function filter() {

        //เซ็ตอาเรย์เริ่มต้น
        var dataLookupAll = dataGrid._options.columns[0].lookup.dataSource;
        //เซ็ตอาเรย์ที่จะกรอง
        var filter = dataGridAll;
        //กรองอาเรย์
        filter.forEach(function (filterdata) {
            dataLookupAll = dataLookupAll.filter(function (arr) {
                return arr.license_id != filterdata.license_id;
            });
        });
        dataLookupFilter = dataLookupAll;

        console.log(dataLookupFilter);
    }

    function setDefaultNumberCar() {
        var arr = {
            dataSource: _dataSource,
            displayExpr: "number_car",
            valueExpr: "license_id"
        }
        dataGrid.option('columns[0].lookup', arr);
    }

    //Set Column Hide
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

    var popup_or_list = $("#popup_or_list").dxPopup({
        visible: false,
        width: "60%",
        height: "70%",
        showTitle: true,
        title: "รายการบันทึก",
        contentTemplate: function (content) {
            return $("<div id='html_or_list'>test</div>");
        }
    }).dxPopup("instance");

    var popup_registrar = $("#popup_registrar").dxPopup({
        visible: false,
        width: "60%",
        height: "70%",
        showTitle: true,
        title: "พนักงานบันทึก",
        contentTemplate: function (content) {
            return $("<div id='html_registrar'>test</div>");
        }
    }).dxPopup("instance");

    function updateOrList(or_id, data) {
        $.ajax({
            type: "POST",
            url: "../Home/UpdateOrList",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{or_id:'" + or_id + "',data:'" + data + "',IdTable: '" + gbTableId + "'}",
            success: function (data) {
                if (data = 1) {
                    //alert('Update Column Hide OK');
                } else {
                    alert('Update Column Hide error!!');
                }
            }
        });

    }

    function updateRegistrar(or_id, data) {
        $.ajax({
            type: "POST",
            url: "../Home/UpdateRegistrar",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{or_id:'" + or_id + "',data:'" + data + "',IdTable: '" + gbTableId + "'}",
            success: function (data) {
                if (data = 1) {
                    //alert('Update Column Hide OK');
                } else {
                    alert('Update Column Hide error!!');
                }
            }
        });

    }
});