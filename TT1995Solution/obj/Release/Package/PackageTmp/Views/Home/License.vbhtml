@Code
    ViewData("Title") = "License"
End Code
<style>
    .custom-file {
        overflow-y: auto;
        height: 100%;
    }
</style>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
    <div id="context-menu"></div>
</div>

<!--New File-->
<div class="modal" id="mdNewFile">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <!-- Modal body -->
            <div class="modal-header">
                <label>เพิ่มไฟล์</label>
            </div>
            <div class="modal-body">
                <div class="row mb-2">
                    <div class="col-sm">
                        <div class="custom-file"></div>
                    </div>
                </div>
            </div>

            <!-- Modal footer -->
            <div class="modal-footer">
                <button type="button" id="btnSave" class="btn btn-success">Save</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
            </div>

        </div>
    </div>
</div>

<script src="~/scripts/Home/license.js"></script>

