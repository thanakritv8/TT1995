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
                    if (item.dataField == "license_date") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            label: {
                                text: "วันจดทะเบียน"
                            },
                            editorType: "dxDateBox",                            
                        });
                    } else if (item.dataField == "number_car") {
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
            dataGrid.option('dataSource', data);
        }
    });

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

    }).dxDataGrid('instance');

    function fnInsertLicense(dataGrid) {
        $.ajax({
            type: "POST",
            url: "../Home/InsertLicense",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            success: function (data) {
                //dataGrid.option('dataSource', data);
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
                //dataGrid.option('dataSource', data);
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
                //dataGrid.option('dataSource', data);
            }
        });
    }
    
});