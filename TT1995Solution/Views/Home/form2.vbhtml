@Code
    ViewData("Title") = "form2"
End Code

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta http-equiv="X-UA-Compatible" content="ie=edge">
    <title>Document</title>
    <link href="https://fonts.googleapis.com/css?family=Sarabun&display=swap" rel="stylesheet">
    <style>
        body {
            font-family: 'Sarabun', sans-serif;
            font-size: 16px;
        }
    </style>
</head>

<body>
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
                    เขียนที่ <input type="text" id="write_location">
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-top: 5px;">
                    วันที่ <input type="number" min="1" max="31" id="day"> เดือน <input type="text" id="month"> ปี <input type="number" id="year">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px; padding-left: 50px;">
                    โดยหนังสือฉบับนี้ ข้าพเจ้า <input type="text" id="name" style="width: 372px"> อายุ <input type="number" id="old" style="width: 50px"> ปี
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    เชื้อชาติ <input type="text" id="origin" style="width: 60px"> สัญชาติ <input type="text" id="nationality" style="width: 60px"> อยู่บ้านเลขที่ <input type="text" id="house_no" style="width: 84px"> หมู่ที่ <input type="text" id="moo" style="width: 60px"> ซอย <input type="text" id="soi" style="width: 140px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    ถนน <input type="text" id="road" style="width: 162px"> ตำบล/แขวง <input type="text" id="sub_district" style="width: 162px"> อำเภอ/เขต <input type="text" id="district" style="width: 164px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    จังหวัด <input type="text" id="province" style="width: 160px"> ซึ่งเป็นผู้มีอำนาจลงนามผูกพัน <input type="text" id="authorized_person" style="width: 284px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    สำนักงานตั้งอยู่ที่ <input type="text" id="company_no" style="width: 93px"> ถนน <input type="text" id="company_road" style="width: 180px"> ตำบล/แขวง <input type="text" id="company_sub_district" style="width: 180px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    อำเภอ/เขต <input type="text" id="company_district" style="width: 136px"> จังหวัด <input type="text" id="company_province" style="width: 180px"> โทรศัพท์ <input type="text" id="company_tel" style="width: 180px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px; padding-left: 50px;">
                    ขอมอบอำนาจให้ <input type="text" id="authorize_name" style="width: 247px"> อายุ <input type="text" id="authorize_old" style="width: 50px"> ปี เชื้อชาติ <input type="text" id="authorize_origin" style="width: 120px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    สัญชาติ <input type="text" id="authorize_nationality" style="width: 106px"> อยู่บ้านเลขที่ <input type="text" id="authorize_house_no" style="width: 130px"> หมู่ที่ <input type="text" id="authorize_moo" style="width: 100px"> ซอย <input type="text" id="authorize_soi" style="width: 130px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    ถนน <input type="text" id="authorize_road" style="width: 165px"> ตำบล/แขวง <input type="text" id="authorize_sub_district" style="width: 163px"> อำเภอ/เขต <input type="text" id="authorize_district" style="width: 160px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px;">
                    จังหวัด <input type="text" id="authorize_province" style="width: 180px">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 15px; padding-left: 50px;">
                    เป็นผู้มีอำนาจทำการแทนข้าพเจ้า
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; padding-left: 50px;">
                    1. <input type="text" id="description_1" style="width: 627px;">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; padding-left: 50px;">
                    2. <input type="text" id="description_2" style="width: 627px;">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; padding-left: 50px;">
                    3. <input type="text" id="description_3" style="width: 627px;">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; padding-left: 50px;">
                    4. <input type="text" id="description_4" style="width: 627px;">
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px; padding-left: 50px;">
                    เพื่อเป็นหลักฐาน ข้าพเจ้าได้ลงลายมือชื่อ หรือพิมพ์ลายนิ้วมือไว้เป็นสำคัญต่อหน้าพนายแล้ว
                </td>
            </tr>
            <tr>
                <td style="padding-top: 25px; padding-left: 320px">
                    (ลงชื่อ) <input type="text" id="sign_1" style="width: 180px;"> ผู้มอบอำนาจ
                </td>
            </tr>
            <tr>
                <td style="padding-top: 20px; padding-left: 320px">
                    (ลงชื่อ) <input type="text" id="sign_2" style="width: 180px;"> ผู้รับมอบอำนาจ
                </td>
            </tr>
            <tr>
                <td style="padding-top: 20px;">
                    ข้าพเจ้าขอรับรองว่าเป็นลายมือชื่อ หรือลายพิมพ์นิ้วมืออันแท้จริงของผู้มอบอำนาจจริง
                </td>
            </tr>
            <tr>
                <td style="padding-top: 20px; padding-left: 320px">
                    (ลงชื่อ) <input type="text" id="sign_3" style="width: 180px;"> พยาน
                </td>
            </tr>
            <tr>
                <td style="padding-top: 20px; padding-left: 320px">
                    (ลงชื่อ) <input type="text" id="sign_4" style="width: 180px;"> พยาน
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
                                เลขที่ <input type="number" id="card_no" style="width: 120px; margin-left: 40px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                วันออกบัตร <input type="date" id="date_of_issue" style="width: 119px;">
                            </td>
                        </tr>
                        <tr>
                            <td style="border-bottom: 1px solid; border-right: 1px solid; border-left: 1px solid; padding: 5px;">
                                วันหมดอายุ <input type="date" id="expired_date" style="width: 119px;">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        <hr width="700" style="margin-top: 30px;">
        <button type="submit" style="margin: 20px auto">บันทึก</button>
    </div>
</body>

</html>

