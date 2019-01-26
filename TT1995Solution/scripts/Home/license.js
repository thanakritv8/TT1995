var itemEditing = [];

$(function () {
    $.ajax({
        type: "POST",
        url: "../Home/GetColumnChooser",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            data.forEach(function (item) {
                
                //console.log(item);
                if (item.dataField != "create_date" && item.dataField != "create_by_user_id" && item.dataField != "update_date" && item.dataField != "update_by_user_id") {
                    if (item.dataField == "license_date") {
                        itemEditing.push({
                            colSpan: item.colSpan,
                            label: {
                                text: "วันจดทะเบียน"
                            },
                            editorType: "dxDateBox",
                            
                        });
                    } else {
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
        onCellClick: function (e) {
            //console.log(e);
        },
        onEditingStart: function (e) {
            console.log(e);
        },
        onRowUpdating: function (e) {
            //var strPath = document.getElementById('dxPic');
            console.log(e);
            //console.log(strPath);
        },
        onRowUpdated: function (e) {
            //console.log(e);
        },

    }).dxDataGrid('instance');


});

var employee = {
    "ID": 1,
    "FirstName": "John",
    "LastName": "Heart",
    "CompanyName": "Super Mart of the West",
    "Position": "CEO",
    "OfficeNo": "901",
    "BirthDate": new Date(1964, 2, 16),
    "HireDate": new Date(1995, 0, 15),
    "Address": "351 S Hill St.",
    "City": "Los Angeles",
    "State": "CA",
    "Zipcode": "90013",
    "Phone": "+1(213) 555-9392",
    "Email": "jheart@dx-email.com",
    "Skype": "jheart_DX_skype"
};

var customers = [{
    "ID": 1,
    "CompanyName": "Super Mart of the West",
    "Address": "702 SW 8th Street",
    "City": "Bentonville",
    "State": "Arkansas",
    "Zipcode": 72716,
    "Phone": "(800) 555-2797",
    "Fax": "(800) 555-2171",
    "Website": "http://www.nowebsitesupermart.com"
}, {
    "ID": 2,
    "CompanyName": "Electronics Depot",
    "Address": "2455 Paces Ferry Road NW",
    "City": "Atlanta",
    "State": "Georgia",
    "Zipcode": 30339,
    "Phone": "(800) 595-3232",
    "Fax": "(800) 595-3231",
    "Website": "http://www.nowebsitedepot.com"
}, {
    "ID": 3,
    "CompanyName": "K&S Music",
    "Address": "1000 Nicllet Mall",
    "City": "Minneapolis",
    "State": "Minnesota",
    "Zipcode": 55403,
    "Phone": "(612) 304-6073",
    "Fax": "(612) 304-6074",
    "Website": "http://www.nowebsitemusic.com"
}, {
    "ID": 4,
    "CompanyName": "Tom's Club",
    "Address": "999 Lake Drive",
    "City": "Issaquah",
    "State": "Washington",
    "Zipcode": 98027,
    "Phone": "(800) 955-2292",
    "Fax": "(800) 955-2293",
    "Website": "http://www.nowebsitetomsclub.com"
}, {
    "ID": 5,
    "CompanyName": "E-Mart",
    "Address": "3333 Beverly Rd",
    "City": "Hoffman Estates",
    "State": "Illinois",
    "Zipcode": 60179,
    "Phone": "(847) 286-2500",
    "Fax": "(847) 286-2501",
    "Website": "http://www.nowebsiteemart.com"
}, {
    "ID": 6,
    "CompanyName": "Walters",
    "Address": "200 Wilmot Rd",
    "City": "Deerfield",
    "State": "Illinois",
    "Zipcode": 60015,
    "Phone": "(847) 940-2500",
    "Fax": "(847) 940-2501",
    "Website": "http://www.nowebsitewalters.com"
}, {
    "ID": 7,
    "CompanyName": "StereoShack",
    "Address": "400 Commerce S",
    "City": "Fort Worth",
    "State": "Texas",
    "Zipcode": 76102,
    "Phone": "(817) 820-0741",
    "Fax": "(817) 820-0742",
    "Website": "http://www.nowebsiteshack.com"
}, {
    "ID": 8,
    "CompanyName": "Circuit Town",
    "Address": "2200 Kensington Court",
    "City": "Oak Brook",
    "State": "Illinois",
    "Zipcode": 60523,
    "Phone": "(800) 955-2929",
    "Fax": "(800) 955-9392",
    "Website": "http://www.nowebsitecircuittown.com"
}, {
    "ID": 9,
    "CompanyName": "Premier Buy",
    "Address": "7601 Penn Avenue South",
    "City": "Richfield",
    "State": "Minnesota",
    "Zipcode": 55423,
    "Phone": "(612) 291-1000",
    "Fax": "(612) 291-2001",
    "Website": "http://www.nowebsitepremierbuy.com"
}, {
    "ID": 10,
    "CompanyName": "ElectrixMax",
    "Address": "263 Shuman Blvd",
    "City": "Naperville",
    "State": "Illinois",
    "Zipcode": 60563,
    "Phone": "(630) 438-7800",
    "Fax": "(630) 438-7801",
    "Website": "http://www.nowebsiteelectrixmax.com"
}, {
    "ID": 11,
    "CompanyName": "Video Emporium",
    "Address": "1201 Elm Street",
    "City": "Dallas",
    "State": "Texas",
    "Zipcode": 75270,
    "Phone": "(214) 854-3000",
    "Fax": "(214) 854-3001",
    "Website": "http://www.nowebsitevideoemporium.com"
}, {
    "ID": 12,
    "CompanyName": "Screen Shop",
    "Address": "1000 Lowes Blvd",
    "City": "Mooresville",
    "State": "North Carolina",
    "Zipcode": 28117,
    "Phone": "(800) 445-6937",
    "Fax": "(800) 445-6938",
    "Website": "http://www.nowebsitescreenshop.com"
}];