﻿@Code
    ViewData("Title") = "ประกัน หลัก"
End Code
<style>
    .custom-file {
        overflow-y: auto;
        height: 100%;
    }
</style>
<div> <h4>ประกันหลัก</h4> </div>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
    <div id="context-menu"></div>
</div>
<!--New Folder-->
<div class="modal" id="mdNewFolder">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <!-- Modal body -->
            <div class="modal-header">
                <label>เพิ่มโฟล์เดอร์</label>
            </div>
            <div class="modal-body">
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" id="lbNewFolder" placeholder="folder name" value="">
                    </div>
                </div>
            </div>
            <!-- Modal footer -->
            <div class="modal-footer">
                <button type="button" id="btnNewFolder" class="btn btn-success btn-sm">New Folder</button>
                <button type="button" id="clearModal" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
<!--Rename-->
<div class="modal" id="mdRename">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <!-- Modal body -->
            <div class="modal-header">
                <label hidden id="idRename"></label>
            </div>
            <div class="modal-body">
                <div class="row mb-2">
                    <div class="col-sm">
                        <div class="col-sm">Name</div>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-sm">
                        <input type="text" class="form-control" id="lbRename" placeholder="document name" value="">
                    </div>
                </div>
            </div>

            <!-- Modal footer -->
            <div class="modal-footer">
                <button type="button" id="btnRename" class="btn btn-success">Rename</button>
                <button type="button" id="btnClose" class="btn btn-danger" data-dismiss="modal">Close</button>
            </div>

        </div>
    </div>
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
                <button type="button" id="btnSave" class="btn btn-success btn-sm">Save</button>
                <button type="button" class="btn btn-danger btn-sm" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
<div class="widget-container">
    <div id="popup"></div>
</div>
<div id="popup_history"></div>
<script src="~/scripts/Home/mainInsurance.js"></script>




