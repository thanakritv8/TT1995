
$(function () {
    
    $("#year").val((new Date).getFullYear());
    
    $('#year').datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years",
        autoclose: true,
    });
    var data_select_fleet = [];
    var data_select_status = [];

    function parseJsonDate(jsonDateString) {
        return new Date(parseInt(jsonDateString.replace('/Date(', '')));
    }

    function GetDataDetailTabien() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetDataDetailTabien",
            data: "{ year:" + "'" + $('#year').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d = parseJsonDate(data[i].start);
                    data[i].start = d;

                    var d = parseJsonDate(data[i].expire);
                    data[i].expire = d;
                }
            }
        }).responseJSON;
    }

    function GetDataDetailOther() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetDataDetailOther",
            data: "{ year:" + "'" + $('#year').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var d = parseJsonDate(data[i].start);
                    data[i].start = d;

                    var d = parseJsonDate(data[i].expire);
                    data[i].expire = d;
                }
            }
        }).responseJSON;
    }

    var DataDetailTabien = GetDataDetailTabien();
    var DataDetailOther = GetDataDetailOther();
    console.log(DataDetailOther);
    //#region First Step
    var base_chart_width = $('#chart-donut-fleet-category').width();
    
    var CarCategoty;

    
    
    function GetFleetCategoryTabien() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetFleetCategoryTabien",
            
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_donut_fleet_category = $("#chart-donut-fleet-category").dxPieChart({
        size: {
            height: '100%',
            width: base_chart_width
        },
        type: "doughnut",
        palette: 'Harmony Light',
        pointSelectionMode: "multiple",
        dataSource: GetFleetCategoryTabien(),
        title: "Fleet Category Tabien",
        series: [{
            argumentField: "fleet",
            valueField: "qty",
            label: {
                visible: true,
                connector: {
                    visible: true
                },
                format: "fixedPoint",
                customizeText: function (point) {
                    point.point.select();
                    return point.argumentText + ": " + point.valueText;
                }
            }
        }],
        onInitialized: function (e) {
            console.log(chart_donut_fleet_category);
        },
        legend: {
            visible: false,
        }, onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            var data_chart_select = chart_donut_fleet_category.getAllSeries()[0]._points;
            data_select_fleet = [];
            jQuery.each(data_chart_select, function (i, val) {
                if (val._currentStyle == 'selection') {
                    data_select_fleet.push(val.argument);
                }

            });
            chart_bar_fleet_category.option('dataSource', FilterFleetToCar(data_select_fleet));
        },

        tooltip: {
            enabled: true
        }
    }).dxPieChart("instance");

    var data_chart_select = chart_donut_fleet_category.getAllSeries()[0]._points;
    data_select_fleet = [];

    jQuery.each(data_chart_select, function (i, val) {
        if (val._currentStyle == 'selection') {
            data_select_fleet.push(val.argument);
        }

    });

    function FilterFleetToCar(filter) {
        //console.log("'" + filter + "'");
        return $.ajax({
            type: "POST",
            url: "../Home/FilterFleetToCar",
            dataType: "json",
            data: "{ filter:'" + filter + "'}",
           
            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    function GetCarCategoryTabien() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetCarCategoryTabien",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    CarCategoty = GetCarCategoryTabien();

    var chart_bar_fleet_category = $("#chart-bar-fleet-category").dxChart({
        rotated: true,
        title: "Car Category",
        dataSource: CarCategoty,
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: {
            color: "#f3c40b",
            argumentField: "internal_call",
            valueField: "qty",
            type: "bar",
            label: {
                visible: true,
                font: {
                    size: '10',
                }
            }
        },
        legend: {
            visible: false
        },
        onPointClick: function (info) {
            console.log(info);
            show_popup(info.target.data.internal_call,'car-category');
        },
    }).dxChart("instance");

    var popup_history = $("#popup_history").dxPopup({
        visible: false,
        width: "60%",
        height: "60%",
        showTitle: true,
        title: "test"
    }).dxPopup("instance");

    function show_popup(title, ChartType, arg1, arg2) {
        popup_history.option('title', title);
        popup_history._options.contentTemplate = function (content) {
            var maxHeight = Math.ceil($("#popup_history .dx-overlay-content").height() - 150);
            content.append("<div id='gridHistory' style='max-height: " + maxHeight + "px;' ></div>");
        }
        $("#popup_history").dxPopup("show");
        if (ChartType == "car-category") {
            show_datagrid_car_category(title);
        } else if (ChartType == "tabien") {
            show_datagrid_tabien(title, FilterStatusToTabien(arg1,arg2));
        } else if (ChartType == "tabienAll") {
            show_datagrid_tabien(title, DataDetailTabien);
        } else if (ChartType == "other") {
            show_datagrid_tabien(title, FilterStatusToOther(arg1, arg2));
        }
    }

    function FilterStatusToTabien(table_name,month) {
        var data = [];
        $.each(data_select_status, function (key, value) {
            var data_filter = DataDetailTabien.filter(function (arr) {
                return arr.status == value && arr.month_expired == month && arr.table_name == table_name;
            });
            data = $.merge(data, data_filter);
        });
        return data;
    }

    function FilterStatusToOther(table_name, month) {
        var data = [];
        $.each(data_select_status, function (key, value) {
            var data_filter = DataDetailOther.filter(function (arr) {
                return arr.status == value && arr.month_expired == month && arr.table_name == table_name;
            });
            data = $.merge(data, data_filter);
        });
        console.log(data);
        return data;
    }

    function show_datagrid_car_category(title,dataSource) {
        var gridHistory = $("#gridHistory").dxDataGrid({
            dataSource: getDetailCarCategory(data_select_fleet, title),
            columns: [
                {
                    dataField: "number_car",
                    caption: "เบอร์รถ",
                },
                {
                    dataField: "license_car",
                    caption: "ทะเบียน",
                },
                {
                    dataField: "internal_call",
                    caption: "ลักษณะรถเรียกภายใน",
                }
            ]
            ,
            showBorders: true,
            height: 'auto',
            scrolling: {
                mode: "virtual"
            },
            export: {
                enabled: true,
                fileName: "data",
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Search..."
            },
        }).dxDataGrid('instance');
    }

    function getDetailCarCategory(filterFleet, filterCar) {
        
        //var dataJson = {
        //    filterFleet: JSON.stringify(filterFleet),
        //    filterCar: filterCar
        //}
        filterCar = filterCar.replace("'", "\\'\\'");
        //console.log(filterCar);
        return $.ajax({
            type: "POST",
            url: "../Home/getDetailCarCategory",
            dataType: "json",
            data: "{ filterFleet:'" + filterFleet + "',filterCar:'" + filterCar +"'}",

            contentType: "application/json; charset=utf-8",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }



    function show_datagrid_tabien(title,dataSource) {
        var gridHistory = $("#gridHistory").dxDataGrid({
            dataSource: dataSource,
            columns: [
                {
                    dataField: "number_car",
                    caption: "เบอร์รถ",
                },
                {
                    dataField: "license_car",
                    caption: "ทะเบียน",
                },
                {
                    dataField: "status",
                    caption: "สถานะ",
                },
                {
                    dataField: "month_expired",
                    caption: "เดือน"
                },
                {
                    dataField: "expire",
                    captin:"วันที่"
                },
                {
                    dataField: "table_name",
                    caption: "ตาราง",
                }
            ]
            ,
            showBorders: true,
            height: 'auto',
            scrolling: {
                mode: "virtual"
            },
            export: {
                enabled: true,
                fileName: "data",
            },
            searchPanel: {
                visible: true,
                width: 240,
                placeholder: "Search..."
            },
        }).dxDataGrid('instance');
    }

    $("#button-car-category").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Car Category','car-category');
            
        }
    });

    //#endregion First Step
    
    //#region Second Step 

    function GetStatusTabien() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetStatusTabien",
            data: "{ year:" + "'" + $('#year').val() + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    function GetStackedBarTabien(filter) {
        return $.ajax({
            type: "POST",
            data: "{ filter:'" + filter + "',year:"+ $('#year').val() +"}",
            url: "../Home/GetStackedBarTabien",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_donut_tabien_status = $("#chart-donut-tabien-status").dxPieChart({
        size: {
            height: '100%',
            width: base_chart_width
        },
        type: "doughnut",
        palette: 'Harmony Light',
        pointSelectionMode: "multiple",
        dataSource: GetStatusTabien(),
        title: "Status",
        series: [{
            argumentField: "status",
            valueField: "qty",
            label: {
                visible: true,
                connector: {
                    visible: true
                },
                format: "fixedPoint",
                customizeText: function (point) {
                    point.point.select();
                    return point.argumentText + ": " + point.valueText;
                }
            }
        }],
        legend: {
            visible: true,
        }, onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            var data_chart_select_status = chart_donut_tabien_status.getAllSeries()[0]._points;
            data_select_status = [];
            jQuery.each(data_chart_select_status, function (i, val) {
                if (val._currentStyle == 'selection') {
                    data_select_status.push(val.argument);
                }

            });
            chart_stacked_bar_tabien.option('dataSource', GetStackedBarTabien(data_select_status));
            chart_stacked_bar_other.option('dataSource', GetStackedBarOther(data_select_status));
            chart_stacked_bar_enter_factory.option('dataSource', GetStackedBarEnterFactory(data_select_status));
        },

        tooltip: {
            enabled: true
        }
    }).dxPieChart("instance");

    var data_chart_select_status = chart_donut_tabien_status.getAllSeries()[0]._points;

    jQuery.each(data_chart_select_status, function (i, val) {
        if (val._currentStyle == 'selection') {
            data_select_status.push(val.argument);
        }

    });

    var chart_stacked_bar_tabien = $("#chart_stacked_bar_tabien").dxChart({
        dataSource: GetStackedBarTabien(data_select_status),
        commonSeriesSettings: {
            argumentField: "month_expired",
            type: "stackedBar",
        },
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: [
            { valueField: "ai_qty", name: "ประกัน พรบ", stack: "1", color: 'green', },
            { valueField: "mi_qty", name: "ประกันภัยรถยนต์", stack: "1", color: 'red' },
            {
                valueField: "tax_qty", name: "ภาษี", stack: "1", color: 'blue', label: {
                    visible: true,
                    font: {
                        size: '10',
                    },
                    customizeText: function (valueFromNameField) {
                        console.log(valueFromNameField);
                        //var data_filter = book_tabien.filter(function (arr) {
                        //    return arr.month_expired == valueFromNameField.argument;
                        //});
                        return valueFromNameField.point.data.price;
                    },
                    position: 'outside'
                }
            },
        ],
        onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }

            show_popup(e.target._options.name + '(' + e.target.aggument + ')', 'tabien', e.target._options.name, e.target.argument);
            //console.log(e.target._options.name +' , '+ e.target.argument);
            //console.log(e);
            
            
        },
        legend: {
            horizontalAlignment: "right",
            position: "inside",
            border: { visible: true },
            columnCount: 2,
            customizeItems: function (items) {
                var sortedItems = [];

                items.forEach(function (item) {
                    if (item.series.name.split(" ")[0] === "Male:") {
                        sortedItems.splice(0, 0, item);
                    } else {
                        sortedItems.splice(3, 0, item);
                    }
                });
                return sortedItems;
            }
        },
        valueAxis: {
            title: {
                text: "License"
            }
        },
        title: "Main License",
        argumentAxis: { // or valueAxis, or commonAxisSettings
            label: {
                displayMode: "stagger",
                staggeringSpacing: 10
            }
        },
        tooltip: {
            enabled: true
        }
    }).dxChart("instance");

    function GetStackedBarOther(filter) {
        return $.ajax({
            type: "POST",
            data: "{ filter:'" + filter + "',year:" + $('#year').val() + "}",
            url: "../Home/GetStackedBarOther",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }
    console.log(GetStackedBarOther(data_select_status));
    var chart_stacked_bar_other = $("#chart_stacked_bar_other").dxChart({
        dataSource: GetStackedBarOther(data_select_status),
        commonSeriesSettings: {
            argumentField: "month_expired",
            type: "stackedBar",
        },
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: [
            { valueField: "dpi_qty", name: "ประกันภัยสินค้าภายในประเทศ", stack: "1",  },
            { valueField: "ei_qty", name: "ประกันภัยสิ่งแวดล้อม", stack: "1",  },
            { valueField: "lv8_qty", name: "ใบอนุญาต วอ.8", stack: "1",  },
            { valueField: "lc_qty", name: "ใบอนุญาตกัมพูชา", stack: "1",  },
            { valueField: "lmr_qty", name: "ใบอนุญาตลุ่มน้ำโขง", stack: "1",  },
            { valueField: "bi_qty", name: "ประกอบการในประเทศ", stack: "1",  },
            {
                valueField: "bo_qty", name: "ประกอบการนอกประเทศ", stack: "1",  label: {
                    visible: true,
                    font: {
                        size: '10',
                    },
                    customizeText: function (valueFromNameField) {
                        console.log(valueFromNameField);
                        //var data_filter = book_tabien.filter(function (arr) {
                        //    return arr.month_expired == valueFromNameField.argument;
                        //});
                        return valueFromNameField.point.data.price;
                    },
                    position: 'outside'
                }
            },
        ],
        onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            //var data_chart_select = chart_donut_fleet_category.getAllSeries()[0]._points;
            show_popup(e.target._options.name + '(' + e.target.aggument + ')', 'other', e.target._options.name, e.target.argument);
            //console.log(e.target._options.name +' , '+ e.target.argument);
        },
        legend: {
            horizontalAlignment: "right",
            position: "inside",
            border: { visible: true },
            columnCount: 2,
            customizeItems: function (items) {
                var sortedItems = [];

                items.forEach(function (item) {
                    if (item.series.name.split(" ")[0] === "Male:") {
                        sortedItems.splice(0, 0, item);
                    } else {
                        sortedItems.splice(3, 0, item);
                    }
                });
                return sortedItems;
            }
        },
        valueAxis: {
            title: {
                text: "License"
            }
        },
        title: "Other License",
        argumentAxis: { // or valueAxis, or commonAxisSettings
            label: {
                displayMode: "stagger",
                staggeringSpacing: 10
            }
        },
        tooltip: {
            enabled: true
        }
    }).dxChart("instance");

    function GetStackedBarEnterFactory(filter) {
        return $.ajax({
            type: "POST",
            data: "{ filter:'" + filter + "',year:" + $('#year').val() + "}",
            url: "../Home/GetStackedBarEnterFactory",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_stacked_bar_enter_factory = $("#chart_stacked_bar_enter_factory").dxChart({
        dataSource: GetStackedBarEnterFactory(data_select_status),
        commonSeriesSettings: {
            argumentField: "month_expired",
            type: "stackedBar",
        },
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: [
            { valueField: "lf_qty", name: "ใบอนุญาตโรงงาน", stack: "1", },
            {
                valueField: "lcf_qty", name: "ใบอนุญาตรถเข้าโรงงาน", stack: "1", label: {
                    visible: true,
                    font: {
                        size: '10',
                    },
                    customizeText: function (valueFromNameField) {
                        console.log(valueFromNameField);
                        //var data_filter = book_tabien.filter(function (arr) {
                        //    return arr.month_expired == valueFromNameField.argument;
                        //});
                        return valueFromNameField.point.data.price;
                    },
                    position: 'outside'
                }
            },
        ],
        onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            console.log(e);

        },
        legend: {
            horizontalAlignment: "right",
            position: "inside",
            border: { visible: true },
            columnCount: 2,
            customizeItems: function (items) {
                var sortedItems = [];

                items.forEach(function (item) {
                    if (item.series.name.split(" ")[0] === "Male:") {
                        sortedItems.splice(0, 0, item);
                    } else {
                        sortedItems.splice(3, 0, item);
                    }
                });
                return sortedItems;
            }
        },
        valueAxis: {
            title: {
                text: "License"
            }
        },
        title: "License Factory",
        argumentAxis: { // or valueAxis, or commonAxisSettings
            label: {
                displayMode: "stagger",
                staggeringSpacing: 10
            }
        },
        tooltip: {
            enabled: true
        }
    }).dxChart("instance");

    $("#button-book-tabien").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Main License','tabienAll');
        }
    });

    $("#button-book-other").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Car Category', 'book-other');
        }
    });

    $("#button-book-enter-factory").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Car Category', 'book-enter-factory');
        }
    });

    //#endregion Second Step

    //#region third Step
    function GetFleetCategoryDriver() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetFleetCategoryDriver",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_donut_driver_fleet = $("#chart-donut-driver-fleet").dxPieChart({
        size: {
            height: '100%',
            width: base_chart_width
        },
        type: "doughnut",
        palette: 'Harmony Light',
        pointSelectionMode: "multiple",
        dataSource: GetFleetCategoryDriver(),
        title: "Fleet Category Driver",
        series: [{
            argumentField: "fleet",
            valueField: "qty",
            label: {
                visible: true,
                connector: {
                    visible: true
                },
                format: "fixedPoint",
                customizeText: function (point) {
                    point.point.select();
                    return point.argumentText + ": " + point.valueText;
                }
            }
        }],
        onInitialized: function (e) {
            console.log(chart_donut_fleet_category);
        },
        legend: {
            visible: false,
        }, onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            var data_chart_select = chart_donut_fleet_category.getAllSeries()[0]._points;
            data_select_fleet = [];
            jQuery.each(data_chart_select, function (i, val) {
                if (val._currentStyle == 'selection') {
                    data_select_fleet.push(val.argument);
                }

            });
            chart_bar_fleet_category.option('dataSource', FilterFleetToCar(data_select_fleet));
        },

        tooltip: {
            enabled: true
        }
    }).dxPieChart("instance");

    function GetDlCategoryDriver() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetDlCategoryDriver",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_bar_dl_category_driver = $("#chart-bar-dl-category-driver").dxChart({
        rotated: true,
        title: "Driving License Category",
        dataSource: GetDlCategoryDriver(),
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: {
            color: "#f3c40b",
            argumentField: "table_name",
            valueField: "qty",
            type: "bar",
            label: {
                visible: true,
                font: {
                    size: '10',
                }
            }
        },
        legend: {
            visible: false
        },
        onPointClick: function (info) {
            console.log(info);
            show_popup(info.target.data.internal_call);
        },
    }).dxChart("instance");

    function GetStatusCategoryDriver() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetStatusCategoryDriver",
            data: "{year:" + $('#year').val() + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_donut_status_category_driver = $("#chart-donut-status-category-driver").dxPieChart({
        size: {
            height: '100%',
            width: base_chart_width
        },
        type: "doughnut",
        palette: 'Harmony Light',
        pointSelectionMode: "multiple",
        dataSource: GetStatusCategoryDriver(),
        title: "Status Category Driver",
        series: [{
            argumentField: "status",
            valueField: "qty",
            label: {
                visible: true,
                connector: {
                    visible: true
                },
                format: "fixedPoint",
                customizeText: function (point) {
                    point.point.select();
                    return point.argumentText + ": " + point.valueText;
                }
            }
        }],
        onInitialized: function (e) {
            console.log(chart_donut_fleet_category);
        },
        legend: {
            visible: false,
        }, onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            var data_chart_select = chart_donut_fleet_category.getAllSeries()[0]._points;
            data_select_fleet = [];
            jQuery.each(data_chart_select, function (i, val) {
                if (val._currentStyle == 'selection') {
                    data_select_fleet.push(val.argument);
                }

            });
            chart_bar_fleet_category.option('dataSource', FilterFleetToCar(data_select_fleet));
        },

        tooltip: {
            enabled: true
        }
    }).dxPieChart("instance");

    function GetStackedBarDlDriver() {
        return $.ajax({
            type: "POST",
            url: "../Home/GetStackedBarDlDriver",
            data: "{year:" + $('#year').val() + "}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (data) {
            }
        }).responseJSON;
    }

    var chart_stacked_dl_driver = $("#chart-stacked-dl-driver").dxChart({
        dataSource: GetStackedBarDlDriver(),
        commonSeriesSettings: {
            argumentField: "month_expired",
            type: "stackedBar",
        },
        size: {
            height: '100%',
            width: base_chart_width
        },
        series: [
            { valueField: "dl_qty", name: "ใบอนุญาตขับขี่", stack: "1", },
            { valueField: "dldot_qty", name: "ใบอนุญาตขับขี่ขนส่งวัตถุอันตราย", stack: "1", },
            { valueField: "dlngt_qty", name: "ใบอนุญาตขับขี่ขนส่งก๊าสธรรมชาติ", stack: "1", },
            { valueField: "dlot_qty", name: "ใบอนุญาตขับขี่ขนส่งน้ำมัน", stack: "1", },
            { valueField: "p_qty", name: "พาสสปอร์ต", stack: "1", },
            {
                valueField: "lf_qty", name: "ใบอนุญาตโรงงาน", stack: "1", label: {
                    visible: true,
                    font: {
                        size: '10',
                    },
                    customizeText: function (valueFromNameField) {
                        console.log(valueFromNameField);
                        //var data_filter = book_tabien.filter(function (arr) {
                        //    return arr.month_expired == valueFromNameField.argument;
                        //});
                        return valueFromNameField.point.data.price;
                    },
                    position: 'outside'
                }
            },
        ],
        onPointClick: function (e) {
            var point = e.target;
            if (point.isSelected()) {
                point.clearSelection();
            } else {
                point.select();
            }
            console.log(e);

        },
        legend: {
            horizontalAlignment: "right",
            position: "inside",
            border: { visible: true },
            columnCount: 2,
            customizeItems: function (items) {
                var sortedItems = [];

                items.forEach(function (item) {
                    if (item.series.name.split(" ")[0] === "Male:") {
                        sortedItems.splice(0, 0, item);
                    } else {
                        sortedItems.splice(3, 0, item);
                    }
                });
                return sortedItems;
            }
        },
        valueAxis: {
            title: {
                text: "License"
            }
        },
        title: "Main License",
        argumentAxis: { // or valueAxis, or commonAxisSettings
            label: {
                displayMode: "stagger",
                staggeringSpacing: 10
            }
        },
        tooltip: {
            enabled: true
        }
    }).dxChart("instance");

    $("#button-dl-category-driver").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Car Category');
        }
    });

    $("#button-dl-driver").dxButton({
        text: "Data All",
        // icon: "exportxlsx",
        visible: true,
        onClick: function () {
            show_popup('Car Category');
        }
    });

    //#endregion third Step

    $("#year").change(function () {
        //#region second Step

        chart_donut_tabien_status.option('dataSource', GetStatusTabien());
        chart_donut_tabien_status.refresh();
        
        chart_stacked_bar_tabien.option('dataSource', GetStackedBarTabien(data_select_status));
        chart_stacked_bar_tabien.refresh();

        DataDetailTabien = GetDataDetailTabien();

        chart_stacked_bar_other.option('dataSource', GetStackedBarOther(data_select_status));
        chart_stacked_bar_other.refresh();

        DataDetailOther = GetDataDetailOther();

        chart_stacked_bar_enter_factory.option('dataSource', GetStackedBarEnterFactory(data_select_status));
        chart_stacked_bar_enter_factory.refresh();

        

        //#endregion second Step

        //#region third Step
        chart_donut_status_category_driver.option('dataSource', GetStatusCategoryDriver());
        chart_donut_status_category_driver.refresh();

        chart_stacked_dl_driver.option('dataSource', GetStackedBarDlDriver());
        chart_stacked_dl_driver.refresh();
        //#endregion third Step
    });
   
});
