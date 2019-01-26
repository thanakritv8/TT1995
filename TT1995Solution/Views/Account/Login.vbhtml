@Code
    ViewData("Title") = "Login"
End Code

<div class="card card-login mx-auto mt-5">
    <div class="card-header">Login</div>
    <div class="card-body">
        <input type="text" id="txtUsername" class="form-control mb-4" placeholder="Username" autofocus="autofocus">
        <input type="password" id="txtPassword" class="form-control mb-4" placeholder="Password">
        <button class="btn btn-success btn-block" id="btnLogin">Login</button>
        <div class="text-danger text-center mt-3" id="lbError"></div>
    </div>
</div>
<script src="~/scripts/Account/login.js"></script>

