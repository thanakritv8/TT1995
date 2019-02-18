$(function () {
    var gc;

    $("#gridContainer").dxDataGrid({
        dataSource: employees,
        keyExpr: "ID",
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
            mode: "row",
            allowUpdating: true,
            allowDeleting: true,
            allowAdding: true,
            useIcons: true,
        },
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
        },
        onContentReady: function (e) {
            if (!e.component.getSelectedRowKeys().length)
                e.component.selectRowsByIndexes(0);
        },
        onCellClick: function(e) {
            console.log(e);
        },
        columns: [{
            text: "สถานะ",
            type: "buttons",
            width: 110,
            caption: "PDF",
            buttons: [{
                hint: "View",
                icon: "download",
                visible: true,
                onClick: function (e) {
                    console.log("Open");
                }
            }, {
                hint: "Upload",
                icon: "upload",
                visible: true,
                onClick: function (e) {
                    console.log("Upload");
                }
            }]
        }, {
            dataField: "Prefix",
            caption: "ใบอนุญาตเลขที่",
        },
        {
            dataField: "FirstName",
            caption: "รหัสเมือง",
        },
        {
            dataField: "LastName",
            caption: "วันที่อนุญาต",
        }, {
            dataField: "Position",
            caption: "วันหมดอายุ",
        }, {
            dataField: "State",
            caption: "เงินพิเศษ",
        }, {
            dataField: "BirthDate",
            caption: "สถานะ",
        }, {
            type: "buttons",
            width: 110,
            buttons: ["edit", "delete"]
        }, ],
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                container.append($('<div class="gc"></div>'));
                gc = $(".gc").dxDataGrid({
                    dataSource: customers,
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    editing: {
                        mode: "row",
                        allowUpdating: true,
                        allowDeleting: true,
                        allowAdding: true,
                        useIcons: true,
                    },
                    onContentReady: function (e) {
                        var $btnView = $('<div id="btnView" class="mr-3">').dxButton({
                            icon: 'exportpdf', //or your custom icon
                            onClick: function () {
                                //On Click
                            }
                        });
                        if (e.element.find('#btnView').length == 0)
                            e.element
                                .find('.dx-toolbar-after')
                                .prepend($btnView);

                        var $btnUpdate = $('<div id="btnUpdate" class="mr-3">').dxButton({
                            icon: 'upload', //or your custom icon
                            onClick: function () {
                                //On Click
                            }
                        });
                        if (e.element.find('#btnUpdate').length == 0)
                            e.element
                                .find('.dx-toolbar-after')
                                .prepend($btnUpdate);

                        //var $btnDelete = $('<div id="btnDelete" class="mr-3">').dxButton({
                        //    icon: 'trash', //or your custom icon
                        //    onClick: function () {
                        //        //On Click
                        //    }
                        //});
                        //if (e.element.find('#btnDelete').length == 0)
                        //    e.element
                        //        .find('.dx-toolbar-after')
                        //        .prepend($btnDelete);
                    },
                    columns: [{
                        dataField: "CompanyName",
                        caption: "เบอร์รถหัว",
                    },
                    {
                        dataField: "City",
                        caption: "ทะเบียนหัว"
                    },
                    {
                        dataField: "State",
                        caption: "เบอร์รถท้าย"
                    },
                    {
                        dataField: "Phone",
                        caption: "ทะเบียนท้าย"
                    }],
                    showBorders: true
                }).dxDataGrid("instance");
            }
        },
        
    });


});

var customers = [{
    "ID": 1,
    "CompanyName": "15",
    "City": "70-7001",
    "State": "16",
    "Phone": "70-7002",
}, {
    "ID": 2,
    "CompanyName": "15",
    "City": "70-7001",
    "State": "16",
    "Phone": "70-7002",
}, {
    "ID": 3,
    "CompanyName": "15",
    "City": "70-7001",
    "State": "16",
    "Phone": "70-7002",
}, {
    "ID": 4,
    "CompanyName": "15",
    "City": "70-7001",
    "State": "16",
    "Phone": "70-7002",
}];

var employees = [{
    "ID": 1,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}, {
    "ID": 2,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}, {
    "ID": 3,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}, {
    "ID": 4,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}, {
    "ID": 5,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}, {
    "ID": 6,
    "Prefix": "2945",
    "FirstName": "T",
    "LastName": "2019-01-02",
    "Position": "2019-04-02",
    "BirthDate": "จัดเตรียม",
    "State": "5000",
}];