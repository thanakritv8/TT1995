@Code
    ViewData("Title") = "พขร"
End Code

<style>
    #gridContainer {
        width: 100%;
    }
</style>
<div> <h4>พขร</h4> </div>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
</div>

<div id="popup_history"></div>
<script>
    //Control Read Only and Read Write
    var boolStatus = false;
    var permission_status = '@Session("6")'; //1 = Read Only, 2 = Read and Write
    if (permission_status == 1) {
        boolStatus = false;
        //$("#context-menu").hide();
    } else {
        boolStatus = true;
        //$("#context-menu").show();
    }
    console.log(!boolStatus);
    //End Control

    $(".toggle-driver").next().toggle();
    $(".toggle-driver").click(function (e) {
        e.stopPropagation();
        $(".toggle-driver").next().toggle();
    });
</script>
<script src="~/scripts/Home/driver.js"></script>

