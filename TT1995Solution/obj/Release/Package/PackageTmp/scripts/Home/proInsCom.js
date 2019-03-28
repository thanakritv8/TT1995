var itemEditing = [[], []];
var itemEditing2 = [];
var columnHide = [];
var gbTableId = '5';
var tableName = "product_insurance_company";
var idFile;
var gbE;
var statusUpdateProtection = 0;
var statusUpdateNotProtection = 0;
var t;
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
var html_editor;
var html_editor_not_protection;
$(function () {
    $("a:contains('ประกันภัยสินค้า')").addClass("active");
    function getDataPic() {
        var dataValue = [];
        //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetPICData",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    //var d = parseJsonDate(data[i].start_date);
                    //data[i].start_date = d;

                    //var d = parseJsonDate(data[i].end_date);
                    //data[i].end_date = d;
                }
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

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

    dataGridAll = getDataPic();
    //console.log(dataGridAll);

    //data grid
    var dataGrid = $("#gridContainer").dxDataGrid({
        allowColumnResizing: true,
        columnResizingMode: "widget",
        dataSource: getDataPic(),
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
                colCount: 2,
                items: [{
                    itemType: "group",
                    caption: "ข้อมูล",
                    items: itemEditing[0]
                }, {
                    itemType: "group",
                    caption: "ข้อตกลงคุ้มครอง",
                    items: itemEditing[1]
                }, {
                    colSpan: 6,
                    dataField: "สินค้าที่คุ้มครอง",
                    template: function (e, itemElement) {
                        var content = "";
                        if (typeof e.component._options.validationGroup.key.not_protection != "undefined") {
                            content = e.component._options.validationGroup.key.not_protection
                        }
                        itemElement.append($('<div class="html-editor">' + content + '</div>'));
                        html_editor = $(".html-editor").dxHtmlEditor({
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
                                if (statusUpdateProtection == 0) {
                                    statusUpdateProtection = 1;
                                }
                            }
                        }).dxHtmlEditor("instance");
                    },
                    label: {
                        location: 'top',
                        alignment: 'left'
                    }
                }, {
                    colSpan: 6,
                    dataField: "สินค้าที่ไม่คุ้มครอง",
                    template: function (e, itemElement) {
                        var content = "";
                        if (typeof e.component._options.validationGroup.key.not_protection != "undefined") {
                            content = e.component._options.validationGroup.key.not_protection
                        }
                        itemElement.append($('<div class="html-editor-not-protection">' + content + '</div>'));
                        html_editor_not_protection = $(".html-editor-not-protection").dxHtmlEditor({
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
                                if (statusUpdateNotProtection == 0) {
                                    statusUpdateNotProtection = 1;
                                }
                            }
                        }).dxHtmlEditor("instance");
                    },
                    label: {
                        location: 'top',
                        alignment: 'left'
                    }
                }
                //JSON.stringify(itemEditing2[0])
                ]
                //items: [JSON.stringify(itemEditing2[0])]
            },
            popup: {
                title: "รายการบริษัทประกันสินค้า",
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
                            //alert(statusUpdateProtection);
                            //console.log(gbE);
                            ////console.log(gbE);
                            ////console.log(dataGrid);
                            if (typeof gbE != "undefined") {
                                if (statusUpdateProtection == 1) {
                                    updateProtection(gbE.data.pic_id, html_editor.option("value"));
                                    gbE.data.protection = html_editor.option("value");
                                }

                                if (statusUpdateNotProtection == 1) {
                                    updateNotProtection(gbE.data.pic_id, html_editor_not_protection.option("value"));
                                    gbE.data.not_protection = html_editor_not_protection.option("value");
                                }
                            }
                            dataGrid.saveEditData();
                        }
                    }
                }, {
                    toolbar: 'bottom',
                    location: 'after',
                    widget: "dxButton",
                    options: {
                        text: "Cancel",
                        onClick: function (args) {
                            console.log(args);
                            dataGrid.cancelEditData();
                        }
                    }
                }]
            },
            useIcons: true,
        },
        "export": {
            enabled: true,
            fileName: "product_insurance_company",
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        headerFilter: {
            visible: true
        },
        onEditingStart: function (e) {
            //alert(e.key.protection);
            //alert(e.key.not_protection);
            //if (e.key.protection == null) {
            //    statusUpdateProtection = 1;
            //} else {
            //    statusUpdateProtection = 0;
            //}

            //if (e.key.not_protection == null) {
            //    statusUpdateNotProtection = 1;
            //} else {
            //    statusUpdateNotProtection = 0;
            //}
            statusUpdateProtection = 0;
            statusUpdateNotProtection = 0;
            gbE = e;
        }
        //,
        //onEditorPrepared: function (e) {

        //    if (typeof html_editor != "undefined" && typeof e.row != "undefined") {
        //        alert(html_editor);
        //        alert(e.row.key.protection);

        //        //console.log(e.row.key.protection);
        //        html_editor.option("value", e.row.key.protection);
        //        console.log(html_editor);
        //    }

        //}
        ,
        onInitNewRow: function (e) {
        },
        onRowUpdating: function (e) {
            if (!fnUpdatePIC(e.newData, e.key.pic_id)) {
                e.cancel = true;
            }
        },
        onRowInserting: function (e) {
            var idInsert = fnInsertPIC(e.data, html_editor.option("value"), html_editor_not_protection.option("value"));
            if (idInsert != 0) {
                e.data.pic_id = idInsert;
                e.data.history = "ประวัติ";
                e.data.protection_view = "View";
                e.data.not_protection_view = "View";
                e.data.protection = html_editor.option("value");
                e.data.not_protection = html_editor_not_protection.option("value");
            } else {
                e.cancel = true;
            }
        },
        onRowRemoving: function (e) {
            e.cancel = fnDeletePIC(e.key.pic_id);

            //กรองอาเรย์
            dataGridAll.forEach(function (filterdata) {
                dataGridAll = dataGridAll.filter(function (arr) {
                    return arr.license_id != e.key.license_id;
                });
            });

        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                //สร้าง id treeview
                container.append($('<div id="treeview"></div>'));
                var itemData = fnGetFiles(options.key.pic_id, gbTableId);
                //console.log(itemData);
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
                        itemData = fnGetFiles(options.key.pic_id, gbTableId);
                        var item = e.itemData;
                        //console.log(e);
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
                fnChangeTreeview(options.key.pic_id, itemData);
            }
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            isFirstClick = false;
        },
        onRowClick: function (e) {
            if (gbE.currentSelectedRowKeys[0].pic_id == e.key.pic_id && isFirstClick && rowIndex == e.rowIndex && gbE.currentDeselectedRowKeys.length == 0) {
                dataGrid.clearSelection();
            } else if (gbE.currentSelectedRowKeys[0].pic_id == e.key.pic_id && !isFirstClick) {
                isFirstClick = true;
                rowIndex = e.rowIndex;
            }
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    //จบการกำหนด dataGrid

    //Get files where id and IdTable
    function fnGetFiles(PICId, IdTable) {
        //alert(PICId + IdTable);
        var itemData;
        $.ajax({
            type: "POST",
            url: "../Home/GetFilesTew",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{Id: " + PICId + ",IdTable: " + IdTable + "}",
            async: false,
            success: function (data) {
                data.push({
                    "file_id": "root",
                    "fk_id": PICId,
                    "name_file": "Root",
                    "type_file": "folder",
                    "icon": "../Img/folder.png"
                });
                itemData = data;
            }
        });
        return itemData;
    }

    //function เปลี่ยนเปลี่ยนข้อมูลเมื่อมีการ เพิ่ม ลบ ไฟล์
    function fnChangeTreeview(pic_id, itemData) {
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
            filter: ["fk_id", "=", pic_id]
        });
        treeview.option("dataSource", dts);
    }

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        data: "{gbTableId: '" + gbTableId + "'}",
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.pic_id),
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
                if (item.dataField == "protection_view") {
                    data[ndata].cellTemplate = function (container, options) {
                        $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                                .text(options.value)
                                .on('dxclick', function (e) {
                                    console.log(options);
                                    console.log(e);
                                    console.log(popup_protection);
                                    popup_protection._options.contentTemplate = function (content) {
                                        var maxHeight = $("#popup_protection .dx-overlay-content").height() - 150;
                                        content.append("<div id='html_protection' style='max-height: " + maxHeight + "px;' ></div>");
                                    }

                                    $("#popup_protection").dxPopup("show");

                                    $("#html_protection").empty();
                                    $('#html_protection').append(options.data.protection);

                                })
                                .appendTo(container);
                    }
                }
                //popup
                if (item.dataField == "not_protection_view") {
                    data[ndata].cellTemplate = function (container, options) {
                        $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                                .text(options.value)
                                .on('dxclick', function (e) {
                                    console.log(options);
                                    console.log(e);
                                    console.log(popup_not_protection);
                                    popup_not_protection._options.contentTemplate = function (content) {
                                        var maxHeight = $("#popup_not_protection .dx-overlay-content").height() - 150;
                                        content.append("<div id='html_not_protection' style='max-height: " + maxHeight + "px;' ></div>");
                                    }

                                    $("#popup_not_protection").dxPopup("show");

                                    $("#html_not_protection").empty();
                                    $('#html_not_protection').append(options.data.not_protection);

                                })
                                .appendTo(container);
                    }
                }
                ndata++;
                //จบการตั้งค่าโชว์ Dropdown

                //popup
                data[0].cellTemplate = function (container, options) {
                    $('<a style="color:green" />').addClass('dx-link')
                            .text(options.value)
                            .on('dxclick', function () {
                                popup_data.option("contentTemplate", null);
                                popup_data._options.contentTemplate = function (content) {
                                    content.append("<div><table border=1 width='100%'><tr class='black white-text' ><td width='35%' align='left'>จำนวนเงินจำกัดความรับผิดสำหรับค่าสินไหมทดแทน</td></tr><tr ><td valign='top'>1. จำนวนเงินจำกัดความรับผิดชอบรวม " + options.data.t1 + " บาท<br>2. จำนวนเงินจำกัดความรับผิดต่อการเรียกร้องหรือต่ออุบัติเหตุแต่ละครั้ง " + options.data.t2 + " บาท<br>3. จำนวนเงินจำกัดความรับผิดต่อหนึ่งยานพาหนะ " + options.data.t3 + " บาท<br>4. จำนวนเงินจำกัดความรับผิดเพื่อการส่งมอบชักช้าต่อการเรียกร้องหรือต่ออุบัติเหตุแต่ละครั้งและต่อหนึ่งยานพาหนะ " + options.data.t4 + " บาท</td></tr></table></div>")
                                }
                                $("#popup_data").dxPopup("show");
                            })
                            .appendTo(container);
                }

                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "pic_id" && item.dataField != "history" && item.dataField != "protection_view" && item.dataField != "not_protection_view") {
                    if (item.group_field == "1") {
                        itemEditing[0].push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            label: {
                                location: item.location,
                                alignment: item.alignment
                            }
                        });
                    } else if (item.group_field == "2") {
                        itemEditing[1].push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            label: {
                                location: item.location,
                                alignment: item.alignment
                            }
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

    //Function Update ข้อมูล gps_company
    function fnUpdatePIC(newData, keyItem) {
        //console.log(keyItem);
        newData.key = keyItem;
        newData.IdTable = gbTableId;
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/UpdatePIC",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลเรียบร้อยแล้ว", "success");
                    returnStatus = true;
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                    returnStatus = false;
                }
            }
        });
        return returnStatus;
    }

    //Function Insert ข้อมูล gps_company
    function fnInsertPIC(dataGrid, dataHtmlEditorProtection, dataHtmlEditorNotProtection) {
        //console.log(dataGrid);
        dataGrid.IdTable = gbTableId;
        dataGrid.DataHtmlEditorProtection = dataHtmlEditorProtection;
        dataGrid.DataHtmlEditorNotProtection = dataHtmlEditorNotProtection;
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertPIC",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                returnId = data[0].Status;
                if (data[0].Status != "0") {
                    DevExpress.ui.notify("เพิ่มข้อมูลเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถเพิ่มข้อมูลได้", "error");
                }
            }
        });
        return returnId;
    }

    //Function Delete ข้อมูล gps_company
    function fnDeletePIC(keyItem) {
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/DeletePIC",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลเรียบร้อยแล้ว", "success");
                    returnStatus = false;
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                    returnStatus = true;
                }
            }
        });
        return returnStatus;
    }

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

    // Onclick New Folder
    $("#btnNewFolder").dxButton({
        onClick: function () {
            document.getElementById("btnNewFolder").disabled = true;
            var folderName = document.getElementById("lbNewFolder").value;
            if (folderName != "") {
                fileDataPic = new FormData();
                fileDataPic.append('fk_id', idFK);
                fileDataPic.append('parentDirId', idFile);
                fileDataPic.append('newFolder', folderName);
                fileDataPic.append('tableId', gbTableId);
                fileDataPic.append('tableName', tableName);
                fnInsertFiles(fileDataPic);
            } else {
                DevExpress.ui.notify("กรุณากรอกชื่อโฟล์เดอร์", "error");
                document.getElementById("btnNewFolder").disabled = false;
            }
        }
    });

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
                $("#mdNewFolder").modal('hide');
                document.getElementById("lbNewFolder").value = '';
                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status, gbTableId);
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
                fileDataPic.append('parentDirId', idFile);
                fileDataPic.append('newFolder', "");
                fileDataPic.append('tableId', gbTableId);
                fileDataPic.append('tableName', tableName);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataPic);
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

    //Function Rename file in treeview
    function fnRename(fileUpload) {

        $.ajax({
            type: "POST",
            url: "../Home/fnRenamePIC",
            data: fileUpload,
            dataType: "json",
            contentType: false,
            processData: false,
            success: function (data) {

                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status, gbTableId);
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

    //Event click of id = btnRename
    $("#btnRename").click(function () {
        document.getElementById("btnRename").disabled = true;
        var folderName = document.getElementById("lbRename").value;
        if (folderName != "") {
            fileDataPic = new FormData();
            fileDataPic.append('fk_id', idFK);
            fileDataPic.append('file_id', idFile);
            fileDataPic.append('rename', folderName);
            fnRename(fileDataPic);
        } else {
            DevExpress.ui.notify("กรุณากรอกชื่อโฟล์เดอร์", "error");
        }
    });

    //Function Delete file in treeview
    function fnDeleteFiles(file_id) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteFile",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + file_id + "',FolderName:'" + tableName + "'}",
            dataType: 'json',
            success: function (data) {
                if (data[0].Status != '0') {
                    var itemData = fnGetFiles(data[0].Status, gbTableId);
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

    var popup_data = $("#popup_data").dxPopup({
        visible: false,
        width: "auto",
        height: "auto",
        showTitle: true,
        title: "รายละเอียด",
        contentTemplate: function (content) {
            return $("")

        }
    }).dxPopup("instance");

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

    var popup_protection = $("#popup_protection").dxPopup({
        visible: false,
        width: "60%",
        height: "70%",
        showTitle: true,
        title: "ดูรายการบันทึก",
        contentTemplate: function (content) {
            return $("<div id='html_protection'>test</div>");
        }
    }).dxPopup("instance");

    var popup_not_protection = $("#popup_not_protection").dxPopup({
        visible: false,
        width: "60%",
        height: "70%",
        showTitle: true,
        title: "ดูพนักงานบันทึก",
        contentTemplate: function (content) {
            return $("<div id='html_not_protection'>test</div>");
        }
    }).dxPopup("instance");

    function updateProtection(pic_id, data) {

        //alert(aic_id + data);

        $.ajax({
            type: "POST",
            url: "../Home/UpdateProtectionPic",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{pic_id:'" + pic_id + "',data:'" + data + "',IdTable: '" + gbTableId + "'}",
            success: function (data) {
                if (data = 1) {
                    //alert('Update Column Hide OK');
                } else {
                    alert('Update Column Hide error!!');
                }
            }
        });

    }

    function updateNotProtection(pic_id, data) {

        //alert(aic_id + data);

        $.ajax({
            type: "POST",
            url: "../Home/UpdateNotProtectionPic",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{pic_id:'" + pic_id + "',data:'" + data + "',IdTable: '" + gbTableId + "'}",
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