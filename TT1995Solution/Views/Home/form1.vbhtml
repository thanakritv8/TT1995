@Code
    ViewData("Title") = "form1"
End Code

<!doctype html>
<html>
<head>
    <meta charset="utf-8">
    <title>Untitled Document</title>
    <link href="https://fonts.googleapis.com/css?family=Sarabun&display=swap" rel="stylesheet">
    <style>
        body {
            font-family: 'Sarabun', sans-serif;
            font-size: 16px;
        }
        input {
            text-align: center;
        }
    </style>
</head>
<body>
    <div align="center">
        <table border="0" cellpadding="0" cellspacing="0" width="700">
            <tr>
                <td rowspan="2">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                คำขอที่ <input type="number" id="txt1" style="width: 120px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                รับวันที่ <input type="date" id="txt2" style="width: 119px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                ผู้รับ <input type="text" id="txt3" style="width: 120px; margin-left: 19px">
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <img src="header.png" width="100">ใช้เฉพาะกฎหมายว่าด้วยการขนส่งทางบกเท่านั้น
                </td>
            </tr>
            <tr>
                <td>
                    <p style="font-size: 20px; font-weight: 600; margin-bottom: 0; margin-left: 45px;">กรมการขนส่งทางบก</p>
                    <p style="font-size: 18px; font-weight: 600; margin-top: 10px; margin-bottom: 0; margin-left: 72px;">แบบคำขออื่น ๆ</p>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="border-top: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                เลขทะเบียน <input type="text" id="txt4" style="width: 110px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                จังหวัด <input type="text" id="txt5" style="width: 110px; margin-left: 33px">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right" style="padding-top: 10px;">
                    วันที่ <input type="number" id="txt6" min="1" max="31" style="width: 50px;"> เดือน <input type="text" id="txt7" style="width: 120px;"> พ.ศ. <input type="number" id="txt8" style="width: 50px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 10px;">
                    <strong>เรียน นายทะเบียน</strong>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 25px; padding-left: 50px">
                    ข้าพเจ้า <input type="text" id="txt9" style="width: 419px;"> อายุ <input type="number" id="txt10" style="width: 119px;"> ปี
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    อยู่ที่ <input type="text" id="txt11" style="width: 50px;"> หมู่ที่ <input type="text" id="txt12" style="width: 50px;"> ซอย <input type="text" id="txt13" style="width: 114px;"> ถนน <input type="text" id="txt14" style="width: 119px;"> ตำบล/แขวง <input type="text" id="txt15" style="width: 114px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    อำเภอ/เขต <input type="text" id="txt16" style="width: 155px;"> จังหวัด <input type="text" id="txt17" style="width: 169px;"> โทรศัพท์ <input type="text" id="txt18" style="width: 170px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    ได้รับมอบอำนาจจากผู้ประกอบการขนส่งชื่อ <input type="text" id="txt19" value="บจก.ทรีทรานส์(1995)" style="width: 411px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    สำนักงานตั้งอยู่ที่ <input type="text" id="txt20" value="101/2" style="width: 58px;"> หมู่ที่ <input type="text" id="txt21" style="width: 50px;"> ซอย <input type="text" id="txt22" style="width: 175px;"> ถนน <input type="text" id="txt23" value="ทล.ระยองสาย 3191" style="width: 175px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    ตำบล/แขวง <input type="text" id="txt24" value="มาบข่า" style="width: 157px;">  อำเภอ/เขต <input type="text" id="txt25" value="นิคมพัฒนา" style="width: 157px;"> จังหวัด <input type="text" id="txt26" value="ระยอง" style="width: 158px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 5px;">
                    โทรศัพท์ <input type="text" id="txt27" style="width: 190px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 25px;">
                    มีความประสงค์<br>
                    <textarea rows="3" id="txt28" style="width: 692px"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left" style="padding-top: 25px;">
                    พร้อมนี้ได้แนบหลักฐานเพื่อประกอบการพิจารณา
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 5px;">
                    1. <input type="text" id="txt29" style="width: 326px;">
                    2. <input type="text" id="txt30" style="width: 326px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 5px;">
                    3. <input type="text" id="txt31" style="width: 326px;">
                    4. <input type="text" id="txt32" style="width: 326px;">
                </td>
            </tr>
            <tr>
                <td colspan="2" style="padding-top: 5px;">
                    5. <input type="text" id="txt33" style="width: 326px;">
                    6. <input type="text" id="txt34" style="width: 326px;">
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2" style="padding-top: 25px; padding-right: 25px">
                    ลงชื่อ <input type="text" id="txt35"> ผู้ยื่นคำขอ
                </td>
            </tr>
        </table>
        <hr width="700" style="margin-top: 30px;">
        <button type="submit" style="margin: 20px auto">บันทึก</button>
    </div>
</body>
</html>

