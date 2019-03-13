<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - Three Trans (1995)</title>


    <link href="~/Content/css/sb-admin.css" rel="stylesheet" />
    <link href="~/Content/vendor/bootstrap/css/bootstrap.css" rel="stylesheet">
    <link href="~/Content/vendor/fontawesome-free/css/all.min.css" rel="stylesheet" type="text/css">

    @*<link rel="stylesheet" type="text/css" href="~/Content/sb-admin.min.css" />*@
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script>window.jQuery || document.write(decodeURIComponent('%3Cscript src="js/jquery.min.js"%3E%3C/script%3E'))</script>
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/18.2.4/css/dx.common.css" />
    <link rel="dx-theme" data-theme="generic.light" href="https://cdn3.devexpress.com/jslib/18.2.4/css/dx.light.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/18.2.4/js/dx.all.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/underscore.js/1.9.1/underscore-min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.13.0/moment.min.js"></script>
    <style>
        .bg-custom {
            background-color: rgb(0,128,0);
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-expand navbar-dark bg-custom static-top">

        <a Class="navbar-brand mr-1" href="~/Home/Index">Three Trans (1995)</a>
        @*<Button Class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
            <i Class="fas fa-bars" style="color:rgb(16,91,172)"></i>   
        </Button>*@
        @If Session("StatusLogin") = "1" Then
            @<ul Class="navbar-nav ml-auto">
                <li Class="nav-item">
                    <a Class="nav-link" href="#">
                        @Session("FirstName").ToString()
                        <span Class="sr-only">(Of current)</span>
                    </a>
                </li>
                <li Class="nav-item dropdown no-arrow">
                    <a Class="nav-link dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <i Class="fas fa-user-circle fa-fw"></i>
                    </a>
                    <div Class="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                        <a Class="dropdown-item" href="#">Settings</a>
                        <div Class="dropdown-divider"></div>
                        <a Class="dropdown-item" href="../Account/Logout" data-toggle="modal" data-target="#logoutModal">Logout</a>
                    </div>
                </li>
            </ul>
        End If



    </nav>
    <div id="wrapper">
        @If Session("StatusLogin") = "1" Then
        @<ul Class="sidebar navbar-nav" style="background-color:rgb(34,139,34)">
            @*rgb(34,139,34)*@
            <li class="nav-item">
                <a class="nav-link" href="../Home/Index">
                    <span Class="text-light">Home</span>
                </a>
            </li>
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light"> ทะเบียน</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/License">เล่มทะเบียน</a>
                    <a Then Class="dropdown-item" href="../Home/Tax">ภาษี</a>
                    <a Class="dropdown-item" href="../Home/OfficerRecords">บันทึกเจ้าหน้าที่</a>
                    <a Class="dropdown-item" href="../Home/Driver">พขร</a>
                </div>
            </li>
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light"> ประกอบการ</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/BusinessIn">ภายในประเทศ</a>
                    <a Then Class="dropdown-item" href="../Home/BusinessOut">ภายนอกประเทศ</a>
                </div>
            </li>
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light"> ใบอนุญาต</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/LicenseCambodia">กัมพูชา</a> @*../Home/LicenseCambodia*@
                    <a Then Class="dropdown-item" href="../Home/LicenseMekongRiver">ลุ่มน้ำโขง</a>
                    <a Then Class="dropdown-item" href="#">เข้าโรงงาน</a>
                    <a Then Class="dropdown-item" href="../Home/LicenseV8">วัตถุอันตราย(วอ.8)</a>
                </div>
            </li>
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light">ประกัน</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/ActInsurance">พรบ</a>
                    <a Then Class="dropdown-item" href="../Home/MainInsurance">ภัยรถยนต์</a>
                    <a Then Class="dropdown-item" href="../Home/DomProIns">สินค้าภายในประเทศ</a>
                    <a Then Class="dropdown-item" href="../Home/EnvironmentInsurance">สิ่งแวดล้อม</a>
                </div>
             </li>
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light">บริษัทประกัน</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/ActInsCom">พรบ</a>
                    <a Then Class="dropdown-item" href="../Home/MainInsCom">ภัยรถยนต์</a>
                    <a Then Class="dropdown-item" href="../Home/ProInsCom">สินค้า</a>
                </div>
            </li>
            <li class="nav-item">
                <a class="nav-link" href="../Home/GpsCompany">
                    <span Class="text-light">บริษัท GPS</span>
                </a>
            </li>
            <li Class="nav-item">
                <a Class="nav-link" href="../Home/Expressway">
                    <span Class="text-light"> ทางด่วน</span>
                </a>
            </li>
            <li Class="nav-item">
                <a Class="nav-link" href="../Home/Trackingwork">
                    <span Class="text-light"> ติดตามงาน</span>
                </a>
            </li>
            <li Class="nav-item">
                <a Class="nav-link" href="../Home/Installment">
                    <span Class="text-light"> การผ่อนชำระ</span>
                </a>
            </li>
            <li Class="nav-item">
                <a Class="nav-link" href="../Home/Gps_car">
                    <span Class="text-light"> GPS ติดรถ</span>
                </a>
            </li>
            <li Class="nav-item dropdown">
                @if Session("GroupId") = "1" Then
                @<a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light"> Administrator</span>
                </a>
                @<div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="#">Group</a>
                    <a Then Class="dropdown-item" href="#">Account</a>
                    <a Then Class="dropdown-item" href="#">Premission</a>
                     <a Then Class="dropdown-item" href="#">Setting</a>
                    <a Then Class="dropdown-item" href="../Manage/Lookup">Manage Lookup</a>
                </div>
                End If
            </li>
        </ul>
        End If
        <!-- Sidebar -->

        <div id="content-wrapper" class="mt-0">
            <div class="container-fluid">
                @RenderBody()
            </div>
            <!-- Logout Modal-->
            <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="exampleModalLabel">Ready to Leave?</h5>
                            <button class="close" type="button" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">×</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            Select "Logout" below if you are ready to end your current session.
                        </div>
                        <div class="modal-footer">
                            <button class="btn btn-secondary btn-sm" type="button" data-dismiss="modal">Cancel</button>
                            <a class="btn btn-danger btn-sm" href="../Account/Logout">Logout</a>
                        </div>
                    </div>
                </div>
            </div>
            @If Session("StatusLogin") = "1" Then
            @<footer Class="sticky-footer">
                <div Class="container my-auto">
                    <div Class="copyright text-center my-auto">
                        <span> Three Trans (1995) Co., Ltd </span><br>
                        <span>
                            101/2 ถนนทางหลวงระยองสาย 3191 ต.มาบข่า อ.นิคมพัฒนา จ.ระยอง 21180
                        </span><br>

                    </div>
                </div>
            </footer>
            End If
        </div>
    </div>
    <script src="~/Scripts/sb-admin.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
</body>
</html>
