@Code
    ViewData("Title") = "Lookup"
End Code
<style>
    #gridContainer {
      width: 100%;
    }
</style>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
</div>
<script>
    $(".d10").next().toggle();
    $(".d10").click(function (e) {
        e.stopPropagation();
        $(".d10").next().toggle();
    });
</script>
<script src="~/scripts/Manage/lookup.js"></script>