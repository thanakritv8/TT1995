$(function () {

    var newButton = $("#newRowButton")
   .dxButton({
       text: "+ สร้างเอกสาร",
       onClick: function () {
           create_doc();
       }
   })
   .dxButton("instance");

    function create_doc() {
        $('#create_form2').modal('show');
    }

    function getreport2() {
        $.ajax({
            type: "GET",
            url: "http://43.254.133.49:8015/TTApi/Tabien/Report/GetReport2All",
            dataType: 'json',
            async: false,
            success: function (data) {
                console.log(data);
                var grid_report2 = $("#gridContainer").dxDataGrid({
                    dataSource: data,
                    keyExpr: "id",
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
                    showBorders: true
                }).dxDataGrid("instance");
            },
            error: function (error) {
                console.log(error);
            }
        });
    }
    getreport2();
});

var show_column = [
        {
            dataField: "txt1",
            caption: "เขียนที่",
        },
        {
            dataField: "txt2",
            caption: "วันที่",
        },
        {
            dataField: "txt3",
            caption: "เดิอน",
        },
        {
            dataField: "txt4",
            caption: "ปี",
        },
        {
            dataField: "txt5",
            caption: "ชื่อ",
        },
        {
            dataField: "txt6",
            caption: "อายุ",
        },
        {
            dataField: "txt7",
            caption: "เชื้อชาติ",
        },
        {
            dataField: "txt8",
            caption: "สัญชาติ",
        },
        {
            dataField: "txt9",
            caption: "บ้านเลขที่",
        },
        {
            dataField: "txt10",
            caption: "หมู่ที่",
        },
        {
            dataField: "txt11",
            caption: "ซอย",
        },
        {
            dataField: "txt12",
            caption: "ถนน",
        },
        {
            dataField: "txt13",
            caption: "ตำบล/แขวง",
        },
        {
            dataField: "txt14",
            caption: "อำเภอ/เขต",
        },
        {
            dataField: "txt15",
            caption: "จังหวัด",
        },
        {
            dataField: "txt16",
            caption: "ผู้มีอำนาจลงนามผูกพัน",
        },
        {
            dataField: "txt17",
            caption: "สำนักงานตั้งอยู่ที่",
        },
        {
            dataField: "txt18",
            caption: "ถนน",
        },
        {
            dataField: "txt19",
            caption: "ตำบล/แขวง",
        },
        {
            dataField: "txt20",
            caption: "อำเภอ/เขต",
        },
        {
            dataField: "txt21",
            caption: "จังหวัด",
        },
        {
            dataField: "txt22",
            caption: "โทรศัพท์",
        },
        {
            dataField: "txt23",
            caption: "ขอมอบอำนาจให้",
        },
        {
            dataField: "txt24",
            caption: "อายุ",
        },
        {
            dataField: "txt25",
            caption: "เชื้อชาติ",
        },
        {
            dataField: "txt26",
            caption: "สัญชาติ",
        },
        {
            dataField: "txt27",
            caption: "บ้านเลขที่",
        },
        {
            dataField: "txt28",
            caption: "หมู่ที่",
        },
        {
            dataField: "txt29",
            caption: "ซอย",
        },
        {
            dataField: "txt30",
            caption: "ถนน",
        },
        {
            dataField: "txt31",
            caption: "ตำบล/แขวง",
        },
        {
            dataField: "txt32",
            caption: "อำเภอ/เขต",
        },
        {
            dataField: "txt33",
            caption: "จังหวัด",
        },
        {
            dataField: "txt34",
            caption: "ผู้มีอำนาจแทนข้าพเจ้า 1",
        },
        {
            dataField: "txt35",
            caption: "ผู้มีอำนาจแทนข้าพเจ้า 2",
        },
        {
            dataField: "txt36",
            caption: "ผู้มีอำนาจแทนข้าพเจ้า 3",
        },
        {
            dataField: "txt37",
            caption: "ผู้มีอำนาจแทนข้าพเจ้า 4",
        },
        {
            dataField: "txt38",
            caption: "ชื่อผู้มอบอำนาจ",
        },
        {
            dataField: "txt39",
            caption: "ชื่อผู้รับมอบอำนาจ",
        },
        {
            dataField: "txt40",
            caption: "พยาน 1",
        },
        {
            dataField: "txt41",
            caption: "พยาน 2",
        },
        {
            dataField: "txt42",
            caption: "เลขที่",
        },
        {
            dataField: "txt43",
            caption: "วันออกบัตร",
        },
        {
            dataField: "txt44",
            caption: "วันหมดอายุ",
        },
        ,
        {
            dataField: "id",
            caption: "",
            allowEditing: false,
            cellTemplate: function (container, options) {
                $('<a style="color:green;font-weight:bold;" />').addClass('dx-link')
                    .text('View')
                    .on('dxclick', function (e) {
                        show_popup_view(e, 'หนังสือมอบอำนาจ', options, options.value);
                    }).appendTo(container);

                $('<a style="color:green;font-weight:bold;margin-left:5px;" />').addClass('dx-link')
                    .text('Edit')
                    .on('dxclick', function (e) {
                        show_popup_edit(e, 'แก้ไขหนังสือมอบอำนาจ', options, options.value);
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
    $("#view_form2 input[id^='txt']").prop("readonly", true);
    for (i = 1; i <= 44; i++) {
        $('#view_form2 #txt' + i).val(data_arr[i][1]);
    }
    $('#view_form2').modal('show');
}

function show_popup_edit(e, title, options, id) {
    var data_arr = [];
    for (var n in options.row.data) {
        data_arr.push([n, options.row.data[n]]);
    }
    for (i = 1; i <= 44; i++) {
        $('#edit_form2 #txt' + i).val(data_arr[i][1]);
    }
    $('#edit_form2').modal('show');
}