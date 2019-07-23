@Code
    ViewData("Title") = "Report2"
End Code

<div>
    <h4>หนังสือมอบอำนาจ</h4>
</div>
<div class="row">
    <div class="col-12">
        <div id="newRowButton" style="float: right"></div>
    </div>
</div>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
    <div id="context-menu"></div>
</div>
<div class="widget-container">
    <div id="popup"></div>
</div>
<style>
    #view_form2 input {
        border: none;
        border-bottom: 1px solid #808080;
    }
</style>
<div class="modal fade" id="create_form2" tabindex="-1" role="dialog" aria-labelledby="exampleModalScrollableTitle" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalScrollableTitle">สร้างเอกสาร</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="700" style="position: relative;">
                        <tr>
                            <td align="center" style="padding-top: 40px; padding-bottom: 50px;">
                                <p style="font-size: 18px; font-weight: 600; margin-top: 0; margin-bottom: 0;">หนังสือมอบอำนาจ</p>
                                <table border="0" cellpadding="0" cellspacing="0" width="76" height="113" style="position: absolute; top: 0; right: 0;">
                                    <tr>
                                        <td style="border: 1px solid; vertical-align: middle; text-align: center;">
                                            ปิด<br>อากร<br>แสตมป์
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                เขียนที่ <input type="text" id="txt1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                วันที่ <input type="number" min="1" max="31" id="txt2"> เดือน <input type="text" id="txt3"> ปี <input type="number" id="txt4">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                โดยหนังสือฉบับนี้ ข้าพเจ้า <input type="text" id="txt5" style="width: 372px"> อายุ <input type="number" id="txt6" style="width: 50px"> ปี
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                เชื้อชาติ <input type="text" id="txt7" style="width: 60px"> สัญชาติ <input type="text" id="txt8" style="width: 60px"> อยู่บ้านเลขที่ <input type="text" id="txt9" style="width: 84px"> หมู่ที่ <input type="text" id="txt10" style="width: 60px"> ซอย <input type="text" id="txt11" style="width: 140px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt12" style="width: 162px"> ตำบล/แขวง <input type="text" id="txt13" style="width: 162px"> อำเภอ/เขต <input type="text" id="txt14" style="width: 164px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt15" style="width: 160px"> ซึ่งเป็นผู้มีอำนาจลงนามผูกพัน <input type="text" id="txt16" style="width: 284px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สำนักงานตั้งอยู่ที่ <input type="text" id="txt17" style="width: 93px"> ถนน <input type="text" id="txt18" style="width: 180px"> ตำบล/แขวง <input type="text" id="txt19" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                อำเภอ/เขต <input type="text" id="txt20" style="width: 136px"> จังหวัด <input type="text" id="txt21" style="width: 180px"> โทรศัพท์ <input type="text" id="txt22" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                ขอมอบอำนาจให้ <input type="text" id="txt23" style="width: 247px"> อายุ <input type="text" id="txt24" style="width: 50px"> ปี เชื้อชาติ <input type="text" id="txt25" style="width: 120px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สัญชาติ <input type="text" id="txt26" style="width: 106px"> อยู่บ้านเลขที่ <input type="text" id="txt27" style="width: 130px"> หมู่ที่ <input type="text" id="txt28" style="width: 100px"> ซอย <input type="text" id="txt29" style="width: 130px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt30" style="width: 165px"> ตำบล/แขวง <input type="text" id="txt31" style="width: 163px"> อำเภอ/เขต <input type="text" id="txt32" style="width: 160px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt33" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                เป็นผู้มีอำนาจทำการแทนข้าพเจ้า
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                1. <input type="text" id="txt34" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                2. <input type="text" id="txt35" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                3. <input type="text" id="txt36" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                4. <input type="text" id="txt37" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                เพื่อเป็นหลักฐาน ข้าพเจ้าได้ลงลายมือชื่อ หรือพิมพ์ลายนิ้วมือไว้เป็นสำคัญต่อหน้าพนายแล้ว
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 25px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt38" style="width: 180px;"> ผู้มอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt39" style="width: 180px;"> ผู้รับมอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px;">
                                ข้าพเจ้าขอรับรองว่าเป็นลายมือชื่อ หรือลายพิมพ์นิ้วมืออันแท้จริงของผู้มอบอำนาจจริง
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt40" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt41" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 20px;">
                                โปรดดูคำเตือนด้านหลัง
                                <table border="0" cellpadding="0" cellspacing="0" style="position: absolute; bottom: -15px; left: 0">
                                    <tr>
                                        <td style="border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            บัตรประตัวมอบอำนาจ
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            เลขที่ <input type="number" id="txt42" style="width: 120px; margin-left: 40px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันออกบัตร <input type="date" id="txt43" style="width: 119px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันหมดอายุ <input type="date" id="txt44" style="width: 119px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <div class="modal-footer" style="justify-content: center;">
                <button type="button" class="btn btn-success">Save</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="view_form2" tabindex="-1" role="dialog" aria-labelledby="exampleModalScrollableTitle" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalScrollableTitle">หนังสือมอบอำนาจ</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="700" style="position: relative;">
                        <tr>
                            <td align="center" style="padding-top: 40px; padding-bottom: 50px;">
                                <p style="font-size: 18px; font-weight: 600; margin-top: 0; margin-bottom: 0;">หนังสือมอบอำนาจ</p>
                                <table border="0" cellpadding="0" cellspacing="0" width="76" height="113" style="position: absolute; top: 0; right: 0;">
                                    <tr>
                                        <td style="border: 1px solid; vertical-align: middle; text-align: center;">
                                            ปิด<br>อากร<br>แสตมป์
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                เขียนที่ <input type="text" id="txt1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                วันที่ <input type="number" min="1" max="31" id="txt2"> เดือน <input type="text" id="txt3"> ปี <input type="number" id="txt4">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                โดยหนังสือฉบับนี้ ข้าพเจ้า <input type="text" id="txt5" style="width: 372px"> อายุ <input type="number" id="txt6" style="width: 50px"> ปี
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                เชื้อชาติ <input type="text" id="txt7" style="width: 60px"> สัญชาติ <input type="text" id="txt8" style="width: 60px"> อยู่บ้านเลขที่ <input type="text" id="txt9" style="width: 84px"> หมู่ที่ <input type="text" id="txt10" style="width: 60px"> ซอย <input type="text" id="txt11" style="width: 140px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt12" style="width: 162px"> ตำบล/แขวง <input type="text" id="txt13" style="width: 162px"> อำเภอ/เขต <input type="text" id="txt14" style="width: 164px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt15" style="width: 160px"> ซึ่งเป็นผู้มีอำนาจลงนามผูกพัน <input type="text" id="txt16" style="width: 284px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สำนักงานตั้งอยู่ที่ <input type="text" id="txt17" style="width: 93px"> ถนน <input type="text" id="txt18" style="width: 180px"> ตำบล/แขวง <input type="text" id="txt19" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                อำเภอ/เขต <input type="text" id="txt20" style="width: 136px"> จังหวัด <input type="text" id="txt21" style="width: 180px"> โทรศัพท์ <input type="text" id="txt22" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                ขอมอบอำนาจให้ <input type="text" id="txt23" style="width: 247px"> อายุ <input type="text" id="txt24" style="width: 50px"> ปี เชื้อชาติ <input type="text" id="txt25" style="width: 120px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สัญชาติ <input type="text" id="txt26" style="width: 106px"> อยู่บ้านเลขที่ <input type="text" id="txt27" style="width: 130px"> หมู่ที่ <input type="text" id="txt28" style="width: 100px"> ซอย <input type="text" id="txt29" style="width: 130px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt30" style="width: 165px"> ตำบล/แขวง <input type="text" id="txt31" style="width: 163px"> อำเภอ/เขต <input type="text" id="txt32" style="width: 160px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt33" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                เป็นผู้มีอำนาจทำการแทนข้าพเจ้า
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                1. <input type="text" id="txt34" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                2. <input type="text" id="txt35" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                3. <input type="text" id="txt36" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                4. <input type="text" id="txt37" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                เพื่อเป็นหลักฐาน ข้าพเจ้าได้ลงลายมือชื่อ หรือพิมพ์ลายนิ้วมือไว้เป็นสำคัญต่อหน้าพนายแล้ว
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 25px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt38" style="width: 180px;"> ผู้มอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt39" style="width: 180px;"> ผู้รับมอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px;">
                                ข้าพเจ้าขอรับรองว่าเป็นลายมือชื่อ หรือลายพิมพ์นิ้วมืออันแท้จริงของผู้มอบอำนาจจริง
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt40" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt41" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 20px;">
                                โปรดดูคำเตือนด้านหลัง
                                <table border="0" cellpadding="0" cellspacing="0" style="position: absolute; bottom: -15px; left: 0">
                                    <tr>
                                        <td style="border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            บัตรประตัวมอบอำนาจ
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            เลขที่ <input type="number" id="txt42" style="width: 120px; margin-left: 40px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันออกบัตร <input type="date" id="txt43" style="width: 119px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันหมดอายุ <input type="date" id="txt44" style="width: 119px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <div class="modal-footer" style="justify-content: center;">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="edit_form2" tabindex="-1" role="dialog" aria-labelledby="exampleModalScrollableTitle" aria-hidden="true">
    <div class="modal-dialog modal-lg modal-dialog-scrollable" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="exampleModalScrollableTitle">แก้ไขหนังสือมอบอำนาจ</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div align="center">
                    <table border="0" cellpadding="0" cellspacing="0" width="700" style="position: relative;">
                        <tr>
                            <td align="center" style="padding-top: 40px; padding-bottom: 50px;">
                                <p style="font-size: 18px; font-weight: 600; margin-top: 0; margin-bottom: 0;">หนังสือมอบอำนาจ</p>
                                <table border="0" cellpadding="0" cellspacing="0" width="76" height="113" style="position: absolute; top: 0; right: 0;">
                                    <tr>
                                        <td style="border: 1px solid; vertical-align: middle; text-align: center;">
                                            ปิด<br>อากร<br>แสตมป์
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                เขียนที่ <input type="text" id="txt1">
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 5px;">
                                วันที่ <input type="number" min="1" max="31" id="txt2"> เดือน <input type="text" id="txt3"> ปี <input type="number" id="txt4">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                โดยหนังสือฉบับนี้ ข้าพเจ้า <input type="text" id="txt5" style="width: 372px"> อายุ <input type="number" id="txt6" style="width: 50px"> ปี
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                เชื้อชาติ <input type="text" id="txt7" style="width: 60px"> สัญชาติ <input type="text" id="txt8" style="width: 60px"> อยู่บ้านเลขที่ <input type="text" id="txt9" style="width: 84px"> หมู่ที่ <input type="text" id="txt10" style="width: 60px"> ซอย <input type="text" id="txt11" style="width: 140px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt12" style="width: 162px"> ตำบล/แขวง <input type="text" id="txt13" style="width: 162px"> อำเภอ/เขต <input type="text" id="txt14" style="width: 164px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt15" style="width: 160px"> ซึ่งเป็นผู้มีอำนาจลงนามผูกพัน <input type="text" id="txt16" style="width: 284px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สำนักงานตั้งอยู่ที่ <input type="text" id="txt17" style="width: 93px"> ถนน <input type="text" id="txt18" style="width: 180px"> ตำบล/แขวง <input type="text" id="txt19" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                อำเภอ/เขต <input type="text" id="txt20" style="width: 136px"> จังหวัด <input type="text" id="txt21" style="width: 180px"> โทรศัพท์ <input type="text" id="txt22" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                ขอมอบอำนาจให้ <input type="text" id="txt23" style="width: 247px"> อายุ <input type="text" id="txt24" style="width: 50px"> ปี เชื้อชาติ <input type="text" id="txt25" style="width: 120px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                สัญชาติ <input type="text" id="txt26" style="width: 106px"> อยู่บ้านเลขที่ <input type="text" id="txt27" style="width: 130px"> หมู่ที่ <input type="text" id="txt28" style="width: 100px"> ซอย <input type="text" id="txt29" style="width: 130px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                ถนน <input type="text" id="txt30" style="width: 165px"> ตำบล/แขวง <input type="text" id="txt31" style="width: 163px"> อำเภอ/เขต <input type="text" id="txt32" style="width: 160px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px;">
                                จังหวัด <input type="text" id="txt33" style="width: 180px">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 15px; padding-left: 50px;">
                                เป็นผู้มีอำนาจทำการแทนข้าพเจ้า
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                1. <input type="text" id="txt34" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                2. <input type="text" id="txt35" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                3. <input type="text" id="txt36" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                4. <input type="text" id="txt37" style="width: 627px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 5px; padding-left: 50px;">
                                เพื่อเป็นหลักฐาน ข้าพเจ้าได้ลงลายมือชื่อ หรือพิมพ์ลายนิ้วมือไว้เป็นสำคัญต่อหน้าพนายแล้ว
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 25px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt38" style="width: 180px;"> ผู้มอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt39" style="width: 180px;"> ผู้รับมอบอำนาจ
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px;">
                                ข้าพเจ้าขอรับรองว่าเป็นลายมือชื่อ หรือลายพิมพ์นิ้วมืออันแท้จริงของผู้มอบอำนาจจริง
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt40" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-top: 20px; padding-left: 320px">
                                (ลงชื่อ) <input type="text" id="txt41" style="width: 180px;"> พยาน
                            </td>
                        </tr>
                        <tr>
                            <td align="right" style="padding-top: 20px;">
                                โปรดดูคำเตือนด้านหลัง
                                <table border="0" cellpadding="0" cellspacing="0" style="position: absolute; bottom: -15px; left: 0">
                                    <tr>
                                        <td style="border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            บัตรประตัวมอบอำนาจ
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            เลขที่ <input type="number" id="txt42" style="width: 120px; margin-left: 40px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันออกบัตร <input type="date" id="txt43" style="width: 119px;">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                            วันหมดอายุ <input type="date" id="txt44" style="width: 119px;">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>
            <div class="modal-footer" style="justify-content: center;">
                <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>
<script src="~/scripts/Home/report2.js"></script>

