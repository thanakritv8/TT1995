var dataAll;
$(function () {
    // รับข้อมูลของ Report 1
    function getData() {
        return $.ajax({
            type: "GET",
            url: "http://43.254.133.49:8015/TTApi/Tabien/Report/GetReport1All",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    dataAll = getData();

    // สร้างปุ่มสร้างเอกสาร
    var newButton = $("#newRowButton")
   .dxButton({
       text: "+ สร้างเอกสาร",
       onClick: function () {
           create_doc();
       }
   })
   .dxButton("instance");

    function create_doc() {
        // หาความยาว เพื่อจะได้ทราบค่าตัวสุดท้าย
        var last_json = dataAll.length;
        // เอา Json มาเก็บในตัวแปร เพื่อเอาไปใส่ Array
        var last_data = dataAll[last_json - 1];
        var data_arr = [];
        for (var n in last_data) {
            data_arr.push([n, last_data[n]]);
        }
        console.log(data_arr);
        // เซ็ตค่า val ของแต่ละ input 
        for (i = 1; i <= 35; i++) {
            $('#create_form1 #txt' + i).val(data_arr[i][1]);
        }
        $('#create_form1').modal('show');
    }

    // สร้าง DataGrid
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

    // บันทึกการสร้างเอกสาร
    $('#btnSaveForm1').click(function () {
        $.ajax({
            type: "POST",
            url: "http://43.254.133.49:8015/TTApi/Tabien/Report/InsertReport1",
            //contentType: "application/json; charset=utf-8",
            data: {
                id: "",
                txt1: $('#create_form1 #txt1').val(),
                txt2: $('#create_form1 #txt2').val(),
                txt3: $('#create_form1 #txt3').val(),
                txt4: $('#create_form1 #txt4').val(),
                txt5: $('#create_form1 #txt5').val(),
                txt6: $('#create_form1 #txt6').val(),
                txt7: $('#create_form1 #txt7').val(),
                txt8: $('#create_form1 #txt8').val(),
                txt9: $('#create_form1 #txt9').val(),
                txt10: $('#create_form1 #txt10').val(),
                txt11: $('#create_form1 #txt11').val(),
                txt12: $('#create_form1 #txt12').val(),
                txt13: $('#create_form1 #txt13').val(),
                txt14: $('#create_form1 #txt14').val(),
                txt15: $('#create_form1 #txt15').val(),
                txt16: $('#create_form1 #txt16').val(),
                txt17: $('#create_form1 #txt17').val(),
                txt18: $('#create_form1 #txt18').val(),
                txt19: $('#create_form1 #txt19').val(),
                txt20: $('#create_form1 #txt20').val(),
                txt21: $('#create_form1 #txt21').val(),
                txt22: $('#create_form1 #txt22').val(),
                txt23: $('#create_form1 #txt23').val(),
                txt24: $('#create_form1 #txt24').val(),
                txt25: $('#create_form1 #txt25').val(),
                txt26: $('#create_form1 #txt26').val(),
                txt27: $('#create_form1 #txt27').val(),
                txt28: $('#create_form1 #txt28').val(),
                txt29: $('#create_form1 #txt29').val(),
                txt30: $('#create_form1 #txt30').val(),
                txt31: $('#create_form1 #txt31').val(),
                txt32: $('#create_form1 #txt32').val(),
                txt33: $('#create_form1 #txt33').val(),
                txt34: $('#create_form1 #txt34').val(),
                txt35: $('#create_form1 #txt35').val(),
            },
            dataType: "json",
            async: false,
            success: function (data) {
                console.log(data);
                getreport1();
            },
            error: function (error) {
                console.log(error);
            DevExpress.ui.notify("กรุณาตรวจสอบข้อมูล", "error");
        }
        });
    })
    
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
            fixed: true,
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
