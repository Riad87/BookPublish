
jQuery(document).ready(function () {

    /*
        Fullscreen background
    */
    //$.backstretch("assets/img/backgrounds/1.jpg");

    /*
        Form validation
    */
    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on('focus', function () {
        $(this).removeClass('input-error');
    });

    $('.login-form').on('submit', function (e) {

        $(this).find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                e.preventDefault();
                $(this).addClass('input-error');
            }
            else {
                $(this).removeClass('input-error');
            }
        });

    });

});

window.onload = function () {
    if (!NiftyCheck())
        return;
    Rounded("#login", "all", "#4A44A4", "#1C5D99", "smooth");
    Rounded("div#nifty", "all", "#4A44A4", "#1C5D99", "smooth");
    Rounded("div#niftyfooter", "all", "#4A44A4", "#1C5D99", "smooth");
}

$(document).ready(function () {
    $('.deletebutton').click(function () {
        $(this).closest('form')[0].submit();
    });
});

$('.orderdetail').on('click', function (e) {
    $.get($(this).attr('href'), function (response) {
        $('#orderDetail').html(response);
    });
    e.preventDefault();
});

$('#pagesizelist').on('change', function (event) {
    window.location.href = updateQueryStringParameter(window.location.href, 'pagesize', $('#pagesizelist').val());
});

$('#SearchString').keypress(function (event) {
    if (event.which == 13) {
        event.preventDefault();
        window.location.href = updateQueryStringParameter(window.location.href, 'SearchString', $('#SearchString').val());
    }
});

function updateQueryStringParameter(uri, key, value) {
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = uri.indexOf('?') !== -1 ? "&" : "?";
    if (uri.match(re)) {
        return uri.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        return uri + separator + key + "=" + value;
    }
}

$('.editbutton').on('click', function (e) {
    $.get($(this).attr('href'), function (response) {
        $('#modaldata').html(response);
    });
    $('#editor').modal('show');
    e.preventDefault();
});

$('.passwordbutton').on('click', function (e) {
    $.get($(this).attr('href'), function (response) {
        $('#modalpwddata').html(response);
    });
    $('#pwdeditor').modal('show');
    e.preventDefault();
});

$(document).on('click', '#btn-edit-submit', function (e) {
    var self = $(this);
    var uri = $(this).attr('href') + "/";
    e.preventDefault();
    $.ajax({
        url: uri,
        method: "POST",
        data: self.closest('form').serialize(),
        success: function (data) {
            if (data.success == true) {
                $('#editor').modal('hide');
                location.reload(false)
            } else {
                var errors = data['errors'];

                displayValidationErrors(errors);
            }
        }
    });
});

$('.createbutton').on('click', function (e) {
    $.get($(this).attr('href'), function (response) {
        $('#modalCreatedata').html(response);
    });
    $('#createnewUser').modal('show');
    e.preventDefault();
});

$(document).on('click', '#btn-create-submit', function (e) {
    var self = $(this);
    var uri = $(this).attr('href') + "/";
    e.preventDefault();
    $.ajax({
        url: uri,
        method: "POST",
        data: self.closest('form').serialize(),
        success: function (data) {
            if (data.success == true) {
                $('#createnewUser').modal('hide');
                location.reload(true)
            } else {
                var errors = data['errors'];

                displayValidationErrorsCreate(errors);
            }
        }
    });
});

$(document).on('click', '#btn-createrole-submit', function (e) {
    var self = $(this);
    var uri = $(this).attr('href') + "/";
    e.preventDefault();
    $.ajax({
        url: uri,
        method: "POST",
        data: self.closest('form').serialize(),
        success: function (data) {
            if (data.success == true) {
                $('#createnewRole').modal('hide');
                location.reload(false)
            } else {
                var errors = data['errors'];

                displayValidationErrorsCreateRole(errors);
            }
        }
    });
});

$(document).on('click', '#btn-changepwd-submit', function (e) {
    var self = $(this);
    var uri = $(this).attr('href') + "/";
    e.preventDefault();
    $.ajax({
        url: uri,
        method: "POST",
        data: self.closest('form').serialize(),
        success: function (data) {
            if (data.success == true) {
                $('#pwdeditor').modal('hide');
                location.reload(false)
            } else {
                var errors = data['errors'];

                displayValidationErrorsCreate(errors);
            }
        }
    });
});
function displayValidationErrors(errors) {
      
    $.each(errors, function (idx, errorMessage) {
        var li = $('<li>').text(errorMessage).appendTo('#errormessage');
    });
}

function displayValidationErrorsCreate(errors) {

    $.each(errors, function (idx, errorMessage) {
        var li = $('<li>').text(errorMessage).appendTo('#errormessageCreate');
    });
}

function displayValidationErrorsCreateRole(errors) {

    $.each(errors, function (idx, errorMessage) {
        var li = $('<li>').text(errorMessage).appendTo('#errormessageCreateRole');
    });
}
