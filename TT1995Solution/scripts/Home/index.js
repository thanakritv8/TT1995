var itemEditing = [];
//var itemEditingPermission = [];
var gbE;
var fileDataPdf;
var fileOpen;

//ตัวแปรควบคุมการคลิก treeview
var isFirstClick = false;
var rowIndex = 0;
var gbTableId = '13';
var gbTableId_p = '14';

var _dataSource;
var dataGridAll;
var dataLookupFilter;

var gc;

$(function () {

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
                title: "ประกอบการภายในประเทศ",
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
            e.data.history = "ประวัติ";
        },
        onRowRemoving: function (e) {
            fnDeleteBusinessIn(e.key.business_id);
        },
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                container.append($('<div class="gc"></div>'));
                dataGridAll = fnGetBusinessInPermission(options.key.business_id);

                gc = $(".gc").dxDataGrid({
                    dataSource: fnGetBusinessInPermission(options.key.business_id),
                    showBorders: true,
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    //groupPanel: {
                    //    visible: true
                    //},
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
        
                        });
                        //console.log(itemEditingPermission);
                        $.ajax({
                            type: "POST",
                            url: "../Home/GetNumberCarBusiness",
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: false,
                            success: function (dataLookup) {
                                
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
                
                //รายการหน้าโชว์หน้าเพิ่มและแก้ไข
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id" && item.dataField != "history") {
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
        url: "../Home/GetIndex",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            dataGrid.option('dataSource', data);
        }
    });

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

    //function fnGetBusinessInPermission(businessId) {
    //    return $.ajax({
    //        type: "POST",
    //        url: "../Home/GetBusinessInPermission",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        data: "{BusinessId: '" + businessId + "'}",
    //        async: false,
    //        success: function (data) {
    //            for (var i = 0; i < data.length; i++) {

    //                var d1 = parseJsonDate(data[i].tax_expire);
    //                data[i].tax_expire = d1
    //            }
    //            return data
    //        }
    //    }).responseJSON;
    //}

});