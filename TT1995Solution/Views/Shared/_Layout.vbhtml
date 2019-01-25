<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - My ASP.NET Application</title>
    <link rel="stylesheet" type="text/css" href="~/Content/sb-admin.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.0/jquery.min.js"></script>
    <script>window.jQuery || document.write(decodeURIComponent('%3Cscript src="js/jquery.min.js"%3E%3C/script%3E'))</script>
    <link rel="stylesheet" type="text/css" href="https://cdn3.devexpress.com/jslib/18.2.4/css/dx.common.css" />
    <link rel="dx-theme" data-theme="generic.light" href="https://cdn3.devexpress.com/jslib/18.2.4/css/dx.light.css" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <script src="https://cdn3.devexpress.com/jslib/18.2.4/js/dx.all.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.2/jszip.min.js"></script>
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/css/bootstrap.min.css" integrity="sha384-GJzZqFGwb1QTTN6wy59ffF1BuGJpLSa9DkKMp0DgiMDm4iYMj70gZWKYbI706tWS" crossorigin="anonymous">
</head>
<body>
    <nav class="navbar navbar-expand navbar-dark bg-light static-top">
        <a class="navbar-brand mr-1 text-muted" href="~/Home/Index">Document Management System</a>
        <button class="btn btn-link btn-sm text-white order-1 order-sm-0" id="sidebarToggle" href="#">
            <i class="fas fa-bars" style="color:rgb(16,91,172)"></i>
        </button> 
    </nav>
    <div id="wrapper">
        @If Session("StatusLogin") = "1" Then
        @<ul Class="sidebar navbar-nav" style="background-color:rgb(0,79,162)">
            <li Class="nav-item dropdown">
                <a Class="nav-link dropdown-toggle" href="#" id="pagesDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <i Class="fas fa-fw fa-folder"></i>
                    <span Class="text-light"> Document</span>
                </a>
                <div Class="dropdown-menu" aria-labelledby="pagesDropdown">
                    <a Then Class="dropdown-item" href="../Home/License">QMS</a>
                    <a Then Class="dropdown-item" href="#">ISO14001-2015</a>
                    <a Class="dropdown-item" href="#">IATF16949-2016</a>
                </div>
            </li>
            <li Class="nav-item">
                <a Class="nav-link" href="../Account/Logout">
                    <i Class="fas fa-sign-out-alt"></i>
                    <span Class="text-light"> Logout</span>
                </a>
            </li>
        </ul>
        End If
        <!-- Sidebar -->

        <div Class="container body-content">
            @RenderBody()

            @If Session("StatusLogin") = "1" Then
            @<hr />
            @<footer>
                <p>&copy; @DateTime.Now.Year - My ASP.NET Application</p>
            </footer>
            End If
        </div>
    </div>
    <script src="~/Scripts/sb-admin.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.6/umd/popper.min.js" integrity="sha384-wHAiFfRlMFy6i5SRaxvfOCifBUQy1xHdJ/yoi7FRNXMRBu5WHdZYu1hA6ZOblgut" crossorigin="anonymous"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.2.1/js/bootstrap.min.js" integrity="sha384-B0UglyR+jN6CkvvICOB2joaf5I4l3gm9GU6Hc1og6Ls7i6U/mkkaduKaBhlAXv9k" crossorigin="anonymous"></script>
</body>
</html>
