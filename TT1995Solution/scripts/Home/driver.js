$(function () {
    var dataGrid = $("#gridContainer").dxDataGrid({
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        showBorders: true,
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
            mode: "row",
            allowDeleting: boolStatus,
            allowAdding: boolStatus,
            allowUpdating: boolStatus,
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
        //headerFilter: {
        //    visible: true
        //},
        onRowInserting: function (e) {
            //console.log(e);
            //$.ajax({
            //    type: "POST",
            //    url: "../Home/GetLicenseCar",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: "{license_id: " + e.data.license_id_head + "}",
            //    async: false,
            //    success: function (data) {
            //        e.data.license_car_head = data[0].license_car;
            //    }
            //});
            //$.ajax({
            //    type: "POST",
            //    url: "../Home/GetLicenseCar",
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    data: "{license_id: " + e.data.license_id_tail + "}",
            //    async: false,
            //    success: function (data) {
            //        e.data.license_car_tail = data[0].license_car;
            //    }
            //});

            e.data.driver_id = fnInsertDriver(e.data);
            console.log(e.data.driver_id);
            //fnInsertDriver(e.data);
        },
        onRowUpdating: function (e) {
            console.log(e.key.driver_id);
            fnUpdateDriver(e.newData, e.key.driver_id);
        },
        onRowRemoving: function (e) {
            fnDeleteDriver(e.key.driver_id);
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');

    //กำหนดในส่วนของ Column ทั้งหน้าเพิ่มข้อมูลและหน้าแก้ไขข้อมูล
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooserDrive",
        contentType: "application/json; charset=utf-8",
        data: "{table_id: 6}",
        dataType: "json",
        async: false,
        success: function (data) {
            $.ajax({
                type: "POST",
                url: "../Home/GetNumberCar",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: false,
                success: function (dataLookup) {
                    data[0].setCellValue = function (rowData, value) {
                        var dataNew = [];
                        $.each(dataLookup, function () {
                            if (this.license_id == value) {
                                dataNew.push(this);
                            }
                        });
                        rowData.license_id_head = value;
                        rowData.license_car_head = dataNew[0].license_car;
                    }
                    data[0].lookup = {
                        dataSource: dataLookup,
                        displayExpr: "number_car",
                        valueExpr: "license_id"
                    }
                    data[1].allowEditing = false

                    data[2].setCellValue = function (rowData, value) {
                        var dataNew = [];

                        $.each(dataLookup, function () {
                            if (this.license_id == value) {
                                dataNew.push(this);
                            }
                        });
                        rowData.license_id_tail = value;
                        rowData.license_car_tail = dataNew[0].license_car;
                    }

                    data[2].lookup = {
                        dataSource: function (options) {
                            console.log(options);
                            return {
                                store: dataLookup,
                                filter: options.data ? ["!", ["license_id", "=", options.data.license_id_head]] : null
                            };
                        },
                        displayExpr: "number_car",
                        valueExpr: "license_id"
                    }
                    data[3].allowEditing = false

                }
            });

            //console.log(data);
            dataGrid.option('columns', data);
        },
        error: function (error) {
            console.log(error);
        }
    });
    //จบการกำหนด Column

    //function GetColumn() {
    //    $.ajax({
    //        type: "POST",
    //        url: "../Manage/GetTable",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {
    //            dataColumn[0].setCellValue = function (rowData, value) {
    //                rowData.table_id = value;
    //                rowData.column_id = null;
    //            }
    //            dataColumn[0].lookup = {
    //                dataSource: data,
    //                valueExpr: "table_id",
    //                displayExpr: "display"
    //            }
    //            dataColumn[1].lookup = {
    //                dataSource: function (options) {
    //                    return {
    //                        store: data,
    //                        filter: options.data ? ["table_id", "=", options.data.table_id] : null
    //                    };
    //                },
    //                valueExpr: "column_id",
    //                displayExpr: "display"
    //            }
    //        }
    //    });

    //    $.ajax({
    //        type: "POST",
    //        url: "../Manage/GetColumn",
    //        contentType: "application/json; charset=utf-8",
    //        dataType: "json",
    //        async: false,
    //        success: function (data) {

    //        }
    //    });
    //    console.log(dataColumn);
    //    dataGrid.option('columns', dataColumn);
    //}

    function fnGetDriver() {
        $.ajax({
            type: "POST",
            url: "../Home/GetDriver",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d1 = parseJsonDate(data[i].start_work_date);
                    data[i].start_work_date = d1
                }
                dataGrid.option('dataSource', data);
                console.log(data);
            }
        });
    }

    fnGetDriver();
    //GetColumn();


    function fnInsertDriver(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Home/InsertDriver",
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

    //Function Update ข้อมูล
    function fnUpdateDriver(newData, keyItem) {
        newData.driver_id = keyItem;
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Home/UpdateDriver",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูล พขร เรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                }
            }
        });
    }

    function fnDeleteDriver(keyItem) {
        $.ajax({
            type: "POST",
            url: "../Home/DeleteDriver",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลรายการค้นหาเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            }
        });
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
})