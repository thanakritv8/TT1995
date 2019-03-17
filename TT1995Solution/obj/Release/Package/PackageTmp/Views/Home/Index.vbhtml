﻿@Code
    ViewData("Title") = "หน้าแรก"
End Code
<style>
    #gridContainer {
        width: 100%;
    }
    #accordion h1 {
        font-size: 18px;
    }

    #accordion h1,
    #accordion p {
        margin: 0;
    }

    .dx-theme-material #accordion .dx-accordion-item-opened h1 {
        margin-top: 7px;
    }
</style>
<div>
    <div class="mt-3 mb-3" id="gridContainer"></div>
    <script type="text/html" id="title">
        <% if(tablename == 'รูปรถ'){ %>
            <% if(data_status == 'สมบูรณ์'){ %>
            <h1 style="color:#15c83f"><%= tablename%></h1>
            <% }else{ %>
            <h1 style="color:#f73b3b"><%= tablename%></h1>
            <% } %>
        <% }else{ %>
        <% if(data_status == 'เสร็จสมบูรณ์'){ %>
        <h1 style="color:#15c83f"><%= tablename%></h1>
        <% }else if(data_status == 'ยังไม่ได้ดำเนินการ' || data_status == 'ขาดต่อ' || data_status == ''){ %>
        <h1 style="color:#f73b3b"><%= tablename%></h1>
        <% }else if(data_status == 'จัดเตรียมเอกสาร' || data_status == 'ยื่นเอกสาร' || data_status == 'ตรวจ GPS'){ %>
        <h1 style="color:#facd20"><%= tablename%></h1>
        <% }} %>

        @*<h1 style="color:#f73b3b"><%= tablename%></h1>*@
    </script>

    <script type="text/html" id="contentdata">
        <div class="accodion-item">
            <div>
                <% if(tablename == 'รูปรถ'){ %>
                
                    <% if(p1 != ''){ %>
                    <p><b>รูปด้านหน้า : </b>
                    <a href="<%= p1%>" target="_blank"><%= n1%></a></p>
                    <% } %>
                    <% if(p2 != ''){ %>
                    <p><b>รูปด้านหลัง : </b>
                    <a href="<%= p2%>" target="_blank"><%= n2%></a></p>
                    <% } %>
                    <% if(p3 != ''){ %>
                    <p><b>รูปด้านข้างซ้าย : </b>
                    <a href="<%= p3%>" target="_blank"><%= n3%></a></p>
                    <% } %>
                    <% if(p4 != ''){ %>
                    <p><b>รูปด้านข้างขวา : </b>
                    <a href="<%= p4%>" target="_blank"><%= n4%></a></p>
                    <% } %>
                
                <% }else{ %>
                <p>
                    <% if(data_number != ''){ %>
                    <b>เลขที่ : </b>
                    <span><%= data_number%></span>
                    <% }else{ %><p>&nbsp;</p><% } %>
                </p>
                <p>
                    <% if(date_start != ''){ %>
                    <b>วันที่อนุมัติ : </b>
                    <span><%= date_start%></span>
                    <% }else{ %><p>&nbsp;</p><% } %>
                </p>
                <p>
                    <% if(date_expire != ''){ %>
                    <b>วันที่หมดอายุ : </b>
                    <span><%= date_expire%></span>
                    <% }else{ %><p>&nbsp;</p><% } %>
                </p>
                <p>
                    <% if(data_status != ''){ %>
                    <b>สถานะ : </b>
                    <span><%= data_status%></span>
                    <% }else{ %><p>&nbsp;</p><% } %>
                </p>
                <% } %>
            </div>
        </div>
    </script>
</div>
<script src="~/scripts/Home/index.js"></script>

