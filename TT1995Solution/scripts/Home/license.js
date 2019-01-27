var itemEditing = [];

$(function () {
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data.forEach(function (item) {
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
            itemEditing.push({
                template: function (data, itemElement) {
                    itemElement.append($("<div>").attr("id", "dxPic").dxFileUploader({
                        multiple: true,
                        allowedFileExtensions: [".jpg", ".jpeg", ".png"]
                    }));
                },
                name: "dxPic",
                label: {
                    text: "รูปรถ"
                },
                colSpan:3,
            });
            itemEditing.push({
                template: function (data, itemElement) {
                    itemElement.append($("<div>").attr("id", "dxPdf").dxFileUploader({
                        multiple: true,
                        allowedFileExtensions: [".pdf"]
                    }));
                },
                name: "dxPdf",
                label: {
                    text: "ไฟล์ Pdf"
                },
                colSpan: 3,
            });
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
            console.log(e);
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
                $("<div>")
                    .dxTreeView({
                        items: products,
                        dataStructure: "plain",
                        parentIdExpr: "categoryId",
                        keyExpr: "ID",
                        displayExpr: "name",
                    }).appendTo(container);
                
            }
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
        },
       
        selection: {
            mode: "single"
        },

    }).dxDataGrid('instance');

    function fnInsertLicense(dataGrid) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("เพิ่มข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "success");
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
                    DevExpress.ui.notify("ลบข้อมูลรายการจดทะเบียนเรียบร้อยแล้ว", "error");
                }
            }
        });
    }
    
});

var products = [{
    ID: "1",
    name: "Stores",
    expanded: true
}, {
    ID: "1_1",
    categoryId: "1",
    name: "Super Mart of the West",
    expanded: true
}, {
    ID: "1_1_1",
    categoryId: "1_1",
    name: "Video Players"
}, {
    ID: "1_1_1_1",
    categoryId: "1_1_1",
    name: "HD Video Player",
    icon: "images/products/1.png",
    price: 220
}, {
    ID: "1_1_1_2",
    categoryId: "1_1_1",
    name: "SuperHD Video Player",
    icon: "images/products/2.png",
    price: 270
}, {
    ID: "1_1_2",
    categoryId: "1_1",
    name: "Televisions",
    expanded: true
}, {
    ID: "1_1_2_1",
    categoryId: "1_1_2",
    name: "SuperLCD 42",
    icon: "images/products/7.png",
    price: 1200
}, {
    ID: "1_1_2_2",
    categoryId: "1_1_2",
    name: "SuperLED 42",
    icon: "images/products/5.png",
    price: 1450
}, {
    ID: "1_1_2_3",
    categoryId: "1_1_2",
    name: "SuperLED 50",
    icon: "images/products/4.png",
    price: 1600
}, {
    ID: "1_1_2_4",
    categoryId: "1_1_2",
    name: "SuperLCD 55",
    icon: "images/products/6.png",
    price: 1750
}, {
    ID: "1_1_2_5",
    categoryId: "1_1_2",
    name: "SuperLCD 70",
    icon: "images/products/9.png",
    price: 4000
}, {
    ID: "1_1_3",
    categoryId: "1_1",
    name: "Monitors"
}, {
    ID: "1_1_3_1",
    categoryId: "1_1_3",
    name: "19\"",
}, {
    ID: "1_1_3_1_1",
    categoryId: "1_1_3_1",
    name: "DesktopLCD 19",
    icon: "images/products/10.png",
    price: 160
}, {
    ID: "1_1_4",
    categoryId: "1_1",
    name: "Projectors"
}, {
    ID: "1_1_4_1",
    categoryId: "1_1_4",
    name: "Projector Plus",
    icon: "images/products/14.png",
    price: 550
}, {
    ID: "1_1_4_2",
    categoryId: "1_1_4",
    name: "Projector PlusHD",
    icon: "images/products/15.png",
    price: 750
}
];