$(function () {
    function GetColumn() {
        
        var jsonGroup;
        $.ajax({
            type: "POST",
            url: "../Account/GetGroup",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                jsonGroup = data;
            }
        });

        var dataColumn = [{
            dataField: "username",
            caption: "Username",
            dataType: "string",
            width: '30%',
        }, {
            dataField: "password",
            caption: "Password",
            dataType: "string",
            width: '30%',
            
        }, {
            dataField: "firstname",
            caption: "Firstname",
            dataType: "string",
            width: '30%',

        }, {
            dataField: "lastname",
            caption: "Lastname",
            dataType: "string",
            width: '30%',

        }, {
            dataField: "tel",
            caption: "Tel",
            dataType: "string",
            width: '30%',

        }, {
            dataField: "address",
            caption: "Address",
            dataType: "string",
            width: '30%',

        }, {
            dataField: "group_id",
            caption: "Name Group",
            dataType: "string",
            width: '30%',
            lookup: {
                dataSource: jsonGroup,
                displayExpr: "group_name",
                valueExpr: "group_id"
            }
        }];
        //console.log(dataColumn);
        dataGrid.option('columns', dataColumn);
    }

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
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            useIcons: true,
        },
        filterRow: {
            visible: true,
            applyFilter: "auto"
        },
        onEditorPreparing(e) {
            //console.log(e);
            //if(e.parentType == "dataRow" && e.dataField == "password")
            //    e.editorOptions.mode = 'password';
        },
        onInitialized: function(e){
            //console.log(e);
        },
        onRowInserting: function (e) {
            e.data.user_id = fnInsertAccount(e.data);
            e.data.password = '●●●●●●●●●●●●●●';
        },
        onRowUpdating: function (e) {
            //console.log(e);
            fnUpdateAccount(e.newData, e.key.user_id);
            e.newData.password = '●●●●●●●●●●●●●●';
            //console.log(e);
        },
        onRowRemoving: function (e) {
            fnDeleteAccount(e.key.user_id);
        },
        selection: {
            mode: "single"
        },
    }).dxDataGrid('instance');
    GetColumn();

    function fnGetAccount() {
        $.ajax({
            type: "POST",
            url: "../Account/GetAccount",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                dataGrid.option('dataSource', data);
                //console.log(data);
            }
        });
    }

    fnGetAccount();


    function fnInsertAccount(dataGrid) {
        var returnId = 0;
        $.ajax({
            type: "POST",
            url: "../Account/InsertAccount",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(dataGrid),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status != "กรุณากรอกข้อมูลให้ถูกต้อง") {
                    DevExpress.ui.notify("เพิ่มข้อมูลเรียบร้อยแล้ว", "success");
                    returnId = data[0].Status;
                } else {
                    DevExpress.ui.notify(data[0].Status, "error");
                }
            }
        });
        return returnId;
    }

    function fnDeleteAccount(keyItem) {
        console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Account/DeleteAccount",
            contentType: "application/json; charset=utf-8",
            data: "{keyId: '" + keyItem + "'}",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("ลบข้อมูลเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถลบข้อมูลได้", "error");
                }
            },
            error: function (request, status, error) {
                console.log(request);
            }
        });
    }

    function fnUpdateAccount(newData, keyItem) {
        newData.user_id = keyItem;
        //console.log(keyItem);
        $.ajax({
            type: "POST",
            url: "../Account/UpdateAccount",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(newData),
            dataType: "json",
            async: false,
            success: function (data) {
                if (data[0].Status == 1) {
                    DevExpress.ui.notify("แก้ไขข้อมูลเรียบร้อยแล้ว", "success");
                } else {
                    DevExpress.ui.notify("ไม่สามารถแก้ไขข้อมูลได้กรุณาตรวจสอบข้อมูล", "error");
                }
            }
        });
    }

    //Function Convert ตัวแปรประเภท Type date ของ javascripts
    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});


