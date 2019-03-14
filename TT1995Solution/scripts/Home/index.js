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
    DevExpress.ui.setTemplateEngine("underscore");
    var dataGrid = $("#gridContainer").dxDataGrid({
        showBorders: true,
        selection: {
            mode: "single"
        },
        searchPanel: {
            visible: true,
            width: 240,
            placeholder: "Search..."
        },
        headerFilter: {
            visible: true
        },
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
        onSelectionChanged: function (e) {
            e.component.collapseAll(-1);
            e.component.expandRow(e.currentSelectedRowKeys[0]);
        },
        onContentReady: function (e) {
            if (!e.component.getSelectedRowKeys().length)
                e.component.selectRowsByIndexes(0);
        },
        columns: [{
            dataField: "number_car",
            caption: "เบอร์รถ",
            fixed: true,
        },{
            dataField: "license_car",
            caption: "ทะเบียนรถ",
            fixed: true,
        }, {
            dataField: "province",
            caption: "จังหวัด",
        }, {
            dataField: "brand_car",
            caption: "ยี่ห้อรถ"
        }, {
            dataField: "number_body",
            caption: "เลขตัวรถ"
        }, {
            dataField: "number_engine",
            caption: "เลขเครื่องยนต์"
        }, {
            dataField: "license_date",
            caption: "วันจดทะเบียน",
            dataType: "datetime",
            format: "MM/dd/yyyy",
        }, {
            dataField: "ownership",
            caption: "ผู้ถือกรรมสิทธิ์"
        }, {
            dataField: "license_status",
            caption: "สถานะ"
        }],
        masterDetail: {
            enabled: false,
            template: function (container, options) {
                
                var currentIndex = options.data;
                container.append($(
                    '<div id="accordion"><div id="accordion-container"></div>'
                ));
                console.log(currentIndex.license_id);
                $.ajax({
                    type: "POST",
                    url: "../Home/GetSubIndex",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: "{license_id: " + currentIndex.license_id + "}",
                    async: false,
                    success: function (data) {
                        console.log(data);
                        for (var i = 0; i < data.length; i++) {
                            var d1 = moment(new Date(parseJsonDate(data[i].date_start))).format('DD-MM-YYYY');
                            data[i].date_start = d1;
                            var d2 = moment(new Date(parseJsonDate(data[i].date_expire))).format('DD-MM-YYYY');
                            data[i].date_expire = d2;
                        }
                        var accordion = $("#accordion-container").dxAccordion({
                            dataSource: data,
                            animationDuration: 300,
                            collapsible: false,
                            multiple: false,
                            itemTitleTemplate: $("#title"),
                            itemTemplate: $("#contentdata"),
                            onSelectionChanged: function (e) {
                                //console.log(e.addedItems[0].CompanyName);
                                //tagBox.option("value", e.addedItems[0].CompanyName);
                            }
                        }).dxAccordion("instance");
                    }
                });

                
            }
        }
    }).dxDataGrid('instance');

    $.ajax({
        type: "POST",
        url: "../Home/GetIndex",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var d1 = parseJsonDate(data[i].license_date);
                data[i].license_date = d1;
            }
            console.log(data);
            dataGrid.option('dataSource', data);
        }
    });

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }
});