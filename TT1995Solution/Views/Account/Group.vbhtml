@Code
    ViewData("Title") = "กลุ่ม"
End Code
<style>
    #gridContainer {
        width: 100%;
    }
</style>
<div> <h4>กลุ่ม</h4> </div>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
</div>
<div id="popup_history"></div>
<script>
    $(".d10").next().toggle();
    $(".d10").click(function (e) {
        e.stopPropagation();
        $(".d10").next().toggle();
    });
</script>
<script src="~/scripts/Account/group.js"></script>

