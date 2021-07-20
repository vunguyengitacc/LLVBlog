$(document).ready(function () {
    $('.login-info-box').fadeOut();
    $('.login-show').addClass('show-log-panel');
    $('.forgotPassword-show').fadeOut();
});


$('.login-reg-panel input[type="radio"]').on('change', function () {
    if ($('#log-login-show').is(':checked')) {
        $('.register-info-box').fadeOut();
        $('.login-info-box').fadeIn();
        $('.forgotPassword-show').fadeOut();
        $('.forgot-show').fadeOut();
        $('.white-panel').addClass('right-log');
        $('.register-show').addClass('show-log-panel');
        $('.login-show').removeClass('show-log-panel');
    }
    else if ($('#log-reg-show').is(':checked')) {
        $('.register-info-box').fadeIn();
        $('.login-info-box').fadeOut();
        $('.forgotPassword-show').fadeOut();
        $('.white-panel').removeClass('right-log');
        $('.forgot-show').fadeIn();
        $('.login-show').addClass('show-log-panel');
        $('.register-show').removeClass('show-log-panel');
    }
    else if ($('#log-forgot-show').is(':checked')) {
        $('.register-info-box').fadeOut();
        $('.login-info-box').fadeIn();
        $('.login-show').fadeIn();
        $('.forgotPassword-show').fadeIn();
        $('.forgot-show').fadeOut();
        $('.white-panel').addClass('right-log');
        $('.login-show').removeClass('show-log-panel');
        $('.register-show').removeClass('show-log-panel');
    }
});

function validateForm() {
    let x = document.forms["fLogin"]["Username"].value;
    let y = document.forms["fLogin"]["Password"].value;
    let form = document.getElementById("Validate"); 
    if (x == "") {
        form.innerHTML = "Không được bỏ trống tên tài khoản"
        return false;
    }
    else if (y == "") {
        form.innerHTML = "Không được bỏ trống mật khẩu";
        return false;
    }
}

function validateRForm() {
    let a = document.forms["fRegister"]["FirstName"].value;
    let b = document.forms["fRegister"]["LastName"].value;
    let c = document.forms["fRegister"]["Gmail"].value;
    let x = document.forms["fRegister"]["Username"].value;
    let y = document.forms["fRegister"]["Password"].value;
    let y1 = document.forms["fRegister"]["Repeat_Pass"].value;
    if (x == "" || y == "" || a == "" || b == "") {
        alert("Hãy điền đầy đủ thông tin đăng ký");
        return false;
    }
    else if (a.length > 50) {
        alert("Họ vượt quá số ký tự cho phép");
        return false;
    }
    else if (a.length < 3) {
        alert("Họ phải có ít nhất 3 ký tự");
        return false;
    }
    else if (b.length < 3) {
        alert("Tên phải có ít nhất 3 ký tự");
        return false;
    }
    else if (b.length > 50) {
        alert("Tên vượt quá số ký tự cho phép");
        return false;
    }
    else if (y.length < 8) {
        alert("Mật khẩu có ít nhất 8 ký tự");
        return false;
    }
    else if (y != y1) {
        alert("Mật khẩu không khớp");
        return false;
    }

}