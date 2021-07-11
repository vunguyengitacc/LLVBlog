﻿$(document).ready(function () {
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
    let y = document.forms["fForgotPass"]["NewPassword"].value;
    let y1 = document.forms["fForgotPass"]["ConfirmPassword"].value;
    if (y == "") {
        alert("Không được bỏ trống mật khẩu");
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