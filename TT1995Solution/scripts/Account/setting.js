$(function () {
    var formData = {
        "Old_Password": "",
        "Password": ""
    };
    var formWidget = $("#form").dxForm({
        formData: formData,
        readOnly: false,
        showColonAfterLabel: true,
        showValidationSummary: true,
        validationGroup: "customerData",
        items: [{
            itemType: "group",
            caption: "Change Password",
            items: [
              {
                  dataField: "Old_Password",
                  editorOptions: {
                      mode: "password"
                  },
                  validationRules: [{
                      type: "required",
                      message: "Old Password is required"
                  }]
              }, {
                  dataField: "Password",
                  editorOptions: {
                      mode: "password"
                  },
                  validationRules: [{
                      type: "required",
                      message: "Password is required"
                  }]
              }, {
                  label: {
                      text: "Confirm Password"
                  },
                  editorType: "dxTextBox",
                  editorOptions: {
                      mode: "password"
                  },
                  validationRules: [{
                      type: "required",
                      message: "Confirm Password is required"
                  }, {
                      type: "compare",
                      message: "'Password' and 'Confirm Password' do not match",
                      comparisonTarget: function () {
                          return formWidget.option("formData").Password;
                      }
                  }]
              }]
        }, {
            itemType: "button",
            horizontalAlignment: "left",
            buttonOptions: {
                text: "Submit",
                type: "success",
                useSubmitBehavior: true
            }
        }]
    }).dxForm("instance");

    $("#form-container").on("submit", function (e) {
        changePassword(formWidget.option("formData"));
        formWidget.resetValues();
        e.preventDefault();
    });

    function changePassword(data) {
        $.ajax({
            url: "../Account/ChangePassword",
            method: "POST",
            data: data,
            dataType: "json",
            async: false
        }).done(function (data) {
            console.log(data);
            if (data[0].Status == 1) {
                DevExpress.ui.notify("แก้ไขรหัสผ่านเรียบร้อยแล้ว", "success");
                window.location.href = "../Account/Logout";
            } else {
                DevExpress.ui.notify("ไม่สามารถแก้ไขรหัสผ่านได้ได้", "error");
            }
        });

    }

});