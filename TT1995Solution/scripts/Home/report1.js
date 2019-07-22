$(function () {
    function getreport1() {
        $.ajax({
            type: "GET",
            url: "http://43.254.133.49:8015/TTApi/Tabien/Report/GetReport1All",
            dataType: 'json',
            async: false,
            success: function (data) {
                console.log(data);
                var grid_report1 = $("#gridContainer").dxDataGrid({
                    dataSource: data,
                    keyExpr: "txt1",
                    searchPanel: {
                        visible: true,
                        width: 240,
                        placeholder: "Search..."
                    },
                    paging: {
                        pageSize: 10
                    },
                    columns: show_column,
                    columnChooser: {
                        enabled: true
                    },
                    columnAutoWidth: true,
                    allowColumnResizing: true,
                    filterRow: {
                        visible: true,
                        applyFilter: "auto"
                    },
                    headerFilter: {
                        visible: true
                    },
                    //selection: {
                    //    mode: "single"
                    //},
                    //onSelectionChanged: function (selectedItems) {
                    //    //$("ul li:nth-child(2)").removeClass("disabled")
                    //    $('ul[role|="menu"] li:nth-child(2)').removeClass("disabled");
                    //    $("a:contains('Next')").attr('href', '#next');
                    //    var data = selectedItems.selectedRowsData[0];
                    //    equipment_safety_select = data.eq_safety_id;



                    //    //console.log(data);
                    //},
                    //editing: {
                    //    allowUpdating: true, // Enables editing
                    //    allowAdding: true, // Enables insertion
                    //    allowDeleting: true // Enables removing
                    //},
                    showBorders: true
                }).dxDataGrid("instance");
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    getreport1();
    
});
var show_column = [
        {
            dataField: "txt1",
            caption: "คำขอที่",
        },
        {
            dataField: "txt2",
            caption: "รับวันที่",
        },
        {
            dataField: "txt3",
            caption: "ผู้รับ",
        },
        {
            dataField: "txt4",
            caption: "เลขทะเบียน",
        },
        {
            dataField: "txt5",
            caption: "จังหวัด",
        },
        {
            dataField: "txt6",
            caption: "วันที่",
        },
        ,
        {
            dataField: "txt7",
            caption: "เดือน",
        },
        {
            dataField: "txt8",
            caption: "พ.ศ.",
        },
        {
            dataField: "txt9",
            caption: "ชื่อ",
        },
        {
            dataField: "txt10",
            caption: "อายุ",
        },
        {
            dataField: "txt11",
            caption: "บ้านเลขที่",
        },
        {
            dataField: "txt12",
            caption: "หมู่ที่",
            visible: false
        },
        {
            dataField: "txt13",
            caption: "ซอย",
            visible: false
        },
        {
            dataField: "txt14",
            caption: "ถนน",
            visible: false
        },
        {
            dataField: "txt15",
            caption: "ตำบล/แขวง",
            visible: false
        },
        {
            dataField: "txt16",
            caption: "อำเภอ/เขต",
            visible: false
        },
        {
            dataField: "txt17",
            caption: "จังหวัด",
            visible: false
        },
        {
            dataField: "txt18",
            caption: "โทรศัพท์",
            visible: false
        },
        {
            dataField: "txt19",
            caption: "รับมอบอำนาจจาก",
        },
        {
            dataField: "txt20",
            caption: "ที่ตั้งสำนักงาน",
        },
        {
            dataField: "txt21",
            caption: "หมู่ที่",
            visible: false
        },
        {
            dataField: "txt22",
            caption: "ซอย",
            visible: false
        },
        {
            dataField: "txt23",
            caption: "ถนน",
            visible: false
        },
        {
            dataField: "txt24",
            caption: "ตำบล/แขวง",
            visible: false
        },
        {
            dataField: "txt25",
            caption: "อำเภอ/เขต",
            visible: false
        },
        {
            dataField: "txt26",
            caption: "จังหวัด",
            visible: false
        },
        {
            dataField: "txt27",
            caption: "โทรศัพท์",
            visible: false
        },
        {
            dataField: "txt28",
            caption: "มีความประสงค์",
        },
        {
            dataField: "txt29",
            caption: "หลักฐาน 1",
            visible: false
        },
        {
            dataField: "txt30",
            caption: "หลักฐาน 2",
            visible: false
        },
        {
            dataField: "txt31",
            caption: "หลักฐาน 3",
            visible: false
        },
        {
            dataField: "txt32",
            caption: "หลักฐาน 4",
            visible: false
        },
        {
            dataField: "txt33",
            caption: "หลักฐาน 5",
            visible: false
        },
        {
            dataField: "txt34",
            caption: "หลักฐาน 6",
            visible: false
        },
        {
            dataField: "txt35",
            caption: "ผู้ยื่นคำขอ",
        },
        {
            dataField: "id",
            caption: "",
            allowEditing: false,
            cellTemplate: function (container, options) {
                $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                    .text('View')
                    .on('dxclick', function (e) {
                        show_popup_view(e, 'แบบคำขออื่นๆ', options, options.value);
                    }).appendTo(container);

                $('<a style="color:green;font-weight:bold;margin-left:5px;" />').addClass('dx-link')
                    .text('Edit')
                    .on('dxclick', function (e) {
                        show_popup_edit(e, 'แก้ไขแบบคำขออื่นๆ', options, options.value);
                    }).appendTo(container);

                $('<a style="color:green;font-weight:bold;margin-left:5px;" />').addClass('dx-link')
                    .text('Delete')
                    .on('dxclick', function (e) {
                        // delete_form_id();
                    }).appendTo(container);

            }
        }
];

function show_popup_view(e, title, options, id) {
    console.log(options.row.data);
    console.log(id);
    var data_arr = [];
    for (var n in options.row.data) {
        data_arr.push([n, options.row.data[n]]);
    }
    $("#view_form1 input[id^='txt']").prop("readonly", true);
    for (i = 1; i <= 35; i++) {
        $('#view_form1 #txt' + i).val(data_arr[i][1]);
    }
    $('#view_form1').modal('show');
}

function show_popup_edit(e, title, options, id) {
    var data_arr = [];
    for (var n in options.row.data) {
        data_arr.push([n, options.row.data[n]]);
    }
    for (i = 1; i <= 35; i++) {
        $('#edit_form1 #txt' + i).val(data_arr[i][1]);
    }
    $('#edit_form1').modal('show');
}
