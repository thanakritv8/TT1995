var itemEditing = [[], [], [], []];
var columnHide = [];
var gbTableId = '7';
var tableName = "main_insurance_company";
var idFile;
var gbE;

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

var feed = {
    cellTemplate: function (container, options) {
        $('<a/>').addClass('dx-link')
            .text('Command')
            .on('dxclick', function () {
                //Do something with options.data;
            })
            .appendTo(container);
    }
};

$(function () {
    $("a:contains('ภัยรถยนต์')").last().addClass("active");
    function getDataMic() {
        var dataValue = [];
        //โชว์ข้อมูลทะเบียนทั้งหมดใน datagrid
        return $.ajax({
            type: "POST",
            url: "../Home/GetMICData",
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

    //data grid
    var dataGrid = $("#gridContainer").dxDataGrid({
        allowColumnResizing: true,
        columnResizingMode: "widget",
        dataSource: getDataMic(),
        searchPanel: {
            visible: true,
            width: 100,
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
                },{
                    itemType: "group",
                    caption: "ความผิดชอบต่อบุคคลภายนอก",
                    items: itemEditing[1]
                }, {
                    itemType: "group",
                    caption: "รถยนต์สูญหาย เสียหาย ไฟไหม้",
                    items: itemEditing[2]
                }, {
                    itemType: "group",
                    caption: "ความคุ้มครองตามเอกสารแบบท้าย",
                    items: itemEditing[3]
                }]
            },
            popup: {
                title: "รายการบริษัทประกันภัยรถยนต์",
                showTitle: true,
                width: "70%",
                position: { my: "center", at: "center", of: window }
            },
            useIcons: true,
        },
        "export": {
            enabled: true,
            fileName: "main_insurance_company",
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        headerFilter: {
            visible: true
        },
        onEditingStart: function (e) {
            
        },
        onInitNewRow: function (e) {
        },
        onRowUpdating: function (e) {
            if (!fnUpdateMIC(e.newData, e.key.mic_id)) {
                e.cancel = true;
            }
        },
        onRowInserting: function (e) {
            var idInsert = fnInsertMIC(e.data);
            if (idInsert != 0) {
                $.ajax({
                    type: "POST",
                    url: "../Home/GetLicenseCarTew",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{number_car: '" + e.data.number_car + "'}",
                    async: false,
                    success: function (data) {
                        e.data.license_car = data[0].license_car;
                        e.data.license_id = data[0].license_id;
                        e.data.history = "ประวัติ";
                    }
                });
                e.data.mic_id = idInsert;
            } else {
                e.cancel = true;
            }
            
        },
        onRowRemoving: function (e) {
           e.cancel = fnDeleteMIC(e.key.mic_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                //สร้าง id treeview
                container.append($('<div id="treeview"></div>'));
                var itemData = fnGetFiles(options.key.mic_id, gbTableId);
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
                        itemData = fnGetFiles(options.key.mic_id, gbTableId);
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
                fnChangeTreeview(options.key.mic_id, itemData);
            }
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
            gbE = e;
            isFirstClick = false;
        },
        onRowClick: function (e) {
            if (gbE.currentSelectedRowKeys[0].mic_id == e.key.mic_id && isFirstClick && rowIndex == e.rowIndex && gbE.currentDeselectedRowKeys.length == 0) {
                dataGrid.clearSelection();
            } else if (gbE.currentSelectedRowKeys[0].mic_id == e.key.mic_id && !isFirstClick) {
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
    function fnGetFiles(MICId, IdTable) {
        //alert(PICId + IdTable);
        var itemData;
        $.ajax({
            type: "POST",
            url: "../Home/GetFilesTew",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: "{Id: " + MICId + ",IdTable: " + IdTable + "}",
            async: false,
            success: function (data) {
                data.push({
                    "file_id": "root",
                    "fk_id": MICId,
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
    function fnChangeTreeview(mic_id, itemData) {
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
            filter: ["fk_id", "=", mic_id]
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
                                        dataSource: fnGetHistory(gbTableId, options.row.data.mic_id),
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

                //popup
                data[0].cellTemplate = function (container, options) {
                    $('<a style="color:green" />').addClass('dx-link')
                            .text(options.value)
                            .on('dxclick', function () {
                                popup_data.option("contentTemplate", null);
                                popup_data._options.contentTemplate = function (content) {
                                    content.append("<div><table border=1 width='100%'><tr class='black white-text' ><td width='35%' align='center'>ความรับผิดชอบต่อบุคคลภายนอก</td><td width='25%' align='center'>รถยนต์เสียหาย สูญหาย ไฟไหม้</td><td width='40%' align='center'>ความคุ้มครองตามเอกสารแนบท้าย</td></tr><tr ><td valign='top'><table border=0 width='100%'><tr><td>1) ความเสียหายต่อชีวิต ร่างกาย หรืออนามัยเฉพาะส่วนเกินวงเงินสูงสุดตาม พรบ<table border=0 style='width:100%'><tr><td style='width:80%'  align='center' >" + options.data.t1_1_1 + "</td><td style='width:20%'  align='right' >บาท/คน</td></tr><tr><td style='width:80%'  align='center'>" + options.data.t1_1_2 + "</td><td style='width:20%'  align='right' >บาท/ครั้ง</td></tr></table></td></tr><tr><td>2) ความเสียหายต่อทรัพย์สิน <table border=0 style='width:100%'><tr><td width='80%'  align='center'>" + options.data.t1_2_1 + "</td><td width='20%'  align='right' >บาท/ครั้ง</td></tr></table></td></tr><tr><td>&nbsp;&nbsp;2.1) ความเสียหายสวนแรก <table border=0 style='width:100%'><tr><td width='80%'  align='center'>" + options.data.t1_2_2 + "</td><td width='20%'  align='right' >บาท/ครั้ง</td></tr></table></td></tr></table></td><td valign='top'><table border=0 width='100%'><tr><td>1) ความเสียหายต่อรถยนต์<table border=0 width='100%'><tr ><td width='80%'  align='center'>" + options.data.t2_1_1 + "</td><td width='20%'  align='right'>บาท/ครั้ง</td></tr><tr><td colspan='2'>1.1 ความเสียหายส่วนแรก<table border=0 width='100%'><tr><td width='80%' align='center'>" + options.data.t2_1_2 + "</td><td width='20%' align='right'>บาท/ครั้ง</td></tr></table></td></tr></table></td></tr><tr><td>2)รถยนต์สูญหายไฟไหม้/ไฟไหม้<table border=0 width='100%'><tr ><td width='80%'  align='center'>" + options.data.t2_2_1 + "</td><td width='20%'  align='right'>บาท/ครั้ง</td></tr></table></td></tr><tr><td></td></tr></table></td><td><table border=0 width='100%'><tr><td>1) อุบัติเหตุส่วนบุคคล<table border=0 width='100%'><tr><td >1.1 เสียชีวิต สูญเสียอวัยวะ ทุพพลภาพถาวร<table border=0 width='100%'><tr></td><td width='30%' align='left'>&nbsp;&nbsp; ก) ผู้ขับขี่ 1 คน</td><td width='40%' align='center'>" + options.data.t3_1_1_b + "</td><td width='30%' align='right'>บาท</td></tr><tr><td width='30%' align='left'>&nbsp;&nbsp; ข) ผู้โดยสาร 2 คน</td><td width='40%' align='center'>" + options.data.t3_1_1_a + "</td><td width='20%' align='right'>บาท/คน</td></tr></table></td></tr><tr><td >1.2 ทุพพลภาพชั่วคราว<table border=0 width='100%'><tr><td width='30%' align='left'>&nbsp;&nbsp; ก) ผู้ขับขี่ 1 คน</td><td width='40%' align='center'>" + options.data.t3_1_2_a + "</td><td width='30%' align='right'>บาท/สัปดาห์</td></tr><tr><td width='30%' align='left'>&nbsp;&nbsp; ข) ผู้โดยสาร - คน</td><td width='40%' align='center'>" + options.data.t3_1_2_b + "</td><td width='30%' align='right'>บาท/คน/สัปดาห์</td></tr></table></td></tr></table></td></tr><tr><td>2) ค่ารักษาพยาบาล<table border=0 width='100%'><tr><td ><table border=0 width='100%'><tr><td width='30%' align='left'></td><td width='40%' align='center'>" + options.data.t3_2_1 + "</td><td width='30%' align='right'>บาท/คน</td></tr></table></td></tr></table></td></tr><tr><td>3) การประกันตัวผู้ขับขี่<table border=0 width='100%'><tr><td ><table border=0 width='100%'><tr><td width='30%' align='left'></td><td width='40%' align='center'>" + options.data.t3_3_1 + "</td><td width='30%' align='right'>บาท/ครั้ง</td></tr></table></td></tr></table></td></tr></table></td></tr></table></div>")
                                }
                                $("#popup_data").dxPopup("show");
                            })
                            .appendTo(container);
                }

                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "mic_id" && item.dataField != "license_id" && item.dataField != "history") {
                    if (item.group_field == "1") {
                        if (item.dataField == "number_car") {
                            itemEditing[0].push({
                                colSpan: item.colSpan,
                                dataField: item.dataField,
                                width: "100%",
                                editorOptions: {
                                    disabled: false
                                },
                                label: {
                                    location: item.location,
                                    alignment: item.alignment
                                },
                            });
                        } else if (item.dataField != "license_car") {
                            itemEditing[0].push({
                                colSpan: item.colSpan,
                                dataField: item.dataField,
                                width: "100%",
                                editorOptions: {
                                    placeholder: item.placeholder
                                },
                                label: {
                                    location: item.location,
                                    alignment: item.alignment
                                }
                            });
                        }
                    } else if (item.group_field == "2") {
                        itemEditing[1].push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            editorOptions: {
                                placeholder: item.placeholder
                            },
                            label: {
                                location: item.location,
                                alignment: item.alignment
                            }
                        });
                    } else if (item.group_field == "3") {
                        itemEditing[2].push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            editorOptions: {
                                placeholder: item.placeholder
                            },label: { 
                                location: item.location,
                                alignment: item.alignment
                            },
                            itemEditing
                        });
                    } else if (item.group_field == "4") {
                        itemEditing[3].push({
                            colSpan: item.colSpan,
                            dataField: item.dataField,
                            width: "100%",
                            editorOptions: {
                                placeholder: item.placeholder
                            },
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
    function fnUpdateMIC(newData, keyItem) {
        //console.log(keyItem);
        newData.key = keyItem;
        newData.IdTable = gbTableId;
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/UpdateMIC",
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
    function fnInsertMIC(dataGrid) {
        dataGrid.IdTable = gbTableId;
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertMIC",
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
    function fnDeleteMIC(keyItem) {
        var returnStatus;
        $.ajax({
            type: "POST",
            url: "../Home/DeleteMIC",
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
                fileDataMic = new FormData();
                fileDataMic.append('fk_id', idFK);
                fileDataMic.append('parentDirId', idFile);
                fileDataMic.append('newFolder', folderName);
                fileDataMic.append('tableId', gbTableId);
                fileDataMic.append('tableName', tableName);
                fnInsertFiles(fileDataMic);
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
                fileDataMic = new FormData();
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
            fileDataMic = new FormData();
            if (files.length > 0) {
                $.each(files, function (i, file) {
                    //fileDataMic.append('file', file);
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
                                        fileDataMic.append('file', blob, file.name);
                                    });
                            }
                        }
                        reader.readAsDataURL(file);
                    } else {
                        fileDataMic.append('file', file);
                    }
                });
                fileDataMic.append('fk_id', idFK);
                fileDataMic.append('parentDirId', idFile);
                fileDataMic.append('newFolder', "");
                fileDataMic.append('tableId', gbTableId);
                fileDataMic.append('tableName', tableName);
            }
        },
    }).dxFileUploader('instance');
    //จบการกำหนด Upload files

    //กำหนดปุ่มเพิ่มรูปภาพเข้าไปในระบบ
    $("#btnSave").dxButton({
        onClick: function () {
            document.getElementById("btnSave").disabled = true;
            fnInsertFiles(fileDataMic);
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
            url: "../Home/fnRenameMIC",
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
            fileDataMic = new FormData();
            fileDataMic.append('fk_id', idFK);
            fileDataMic.append('file_id', idFile);
            fileDataMic.append('rename', folderName);
            fnRename(fileDataMic);
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

});