/// <reference path="jquery-1.8.2.js" />
/// <reference path="jqueryui/jquery-ui-1.10.3.custom.js" />

/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />
/// <reference path="knockout-2.2.0.debug.js" />
/// <reference path="modernizr-2.6.2.js" />

var monster = {
    set: function (name, value, days, path) {
        var date = new Date(),
            expires = '',
            type = typeof (value),
            valueToUse = '';
        path = path || "/";
        if (days) {
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toGMTString();
        }
        if (type === "object" && type !== "undefined") {
            if (!("JSON" in window)) throw "Bummer, your browser doesn't support JSON parsing.";
            valueToUse = JSON.stringify({ v: value });
        }
        else
            valueToUse = escape(value);

        document.cookie = name + "=" + valueToUse + expires + "; path=" + path;
    },
    get: function (name) {
        var nameEQ = name + "=",
            ca = document.cookie.split(';'),
            value = '',
            firstChar = '',
            parsed = {};
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) === 0) {
                value = c.substring(nameEQ.length, c.length);
                firstChar = value.substring(0, 1);
                if (firstChar == "{") {
                    parsed = JSON.parse(value);
                    if ("v" in parsed) return parsed.v;
                }
                if (value == "undefined") return undefined;
                return unescape(value);
            }
        }
        return null;
    }
};

function SetCookiesInfo() {
    
    //alert(monster.get('cookieInfo'));

    if (!(monster.get('cookieInfo') === 'true'))
    {
        $('#cookies-info').show('blind', 1100);
        $('#cookies-info > a').click(function () {
            monster.set('cookieInfo', 'true', 256);
            $('#cookies-info').hide('blind', 1100);
        });
    }
}

var RollingTime = 150;

function PopUp(panelId) {
    $('#' + panelId).each(function () {
        //var currStyle = $(this).attr('style');
        //$(this).attr('style', 'top:40px;right:20px;' + currStyle);
        if ($(this).css("display") == "none")
            $(this).slideDown(RollingTime);//.toggle(500);
        else
            $(this).slideUp(RollingTime);
    });
}

$(document).ready(function () {
    Decorate();
    //alert('ready()');

    SetCookiesInfo();

    //$(".kanc-date-picker").datepicker("option", "dateFormat", "yy-mm-dd");
    //$(".kanc-date-picker").datepicker();

    //$(".kanc-dodaj-pozycje-faktury").bind("click", function () {
    $(".kanc-dialog").dialog({
        autoOpen: false,
        height: 300,
        width: 990,
        modal: true
    });
    //});
    $(".kanc-hide-button").click(function () {
        //alert($(this).attr("data-shown"));
        //if ($(this).attr("data-shown") == 'undefined')
        //{
        //    $(this).attr("data-shown", 1);
        //}

        if ($(this).attr("data-shown") == 0) {
            $('#' + $(this).attr("data-target-id")).slideDown();
            $(this).attr("data-shown", 1);
        }
        else {
            $('#' + $(this).attr("data-target-id")).slideUp();
            $(this).attr("data-shown", 0);
        }
    });

    $(".kanc-call-dialog-button").click(function () {
        //alert($(this).attr("data-target-id"));
        var targetSelector = '#' + $(this).attr("data-target-id");
        $(targetSelector).dialog({
            //autoOpen: false,
            height: 300,
            width: 990,
            modal: true
        });
        // podpiecie zamkniecia dialogu pod submit ponizej data-target-id
        $(targetSelector + ' input[type="submit"]').click(function () {
            $(targetSelector).dialog("close");
        });
    });

    $("div.kanc-date-tracker-button").click(function () {
        var NumerOfDays = parseInt($(this).parent().children("input.kanc-date-tracker").val());
        //alert(NumerOfDays);
        //var ControlDate = $(this).parent().parent().children("input.kanc-date-picker").datepicker("getDate");
        var ControlDate = $(this).parent().parent().children("input.kanc-date-picker").datepicker('getDate');
        //alert(ControlDate);
        //var TodayDate = new Date();
        //TodayDate.setDate(ControlDate.getDate() + NumerOfDays);
        //alert(TodayDate);
        //$(this).parent().parent().children("input.kanc-date-picker").datepicker("setDate", ControlDate + NumerOfDays);
        //$(this).parent().children("input.kanc-date-tracker").val(0);

        //var ControlDate = $('input[name="arrivalDate"]').val();
        //var NumerOfDays = $('input[name="numOfNights"]').val();
        //var date = new Date(ControlDate);
        //var d = date.getDate();
        //var m = date.getMonth();
        //var y = date.getFullYear();
        //var edate = new Date(y, m, d + NumerOfDays);
        ////$('input[name="departureDate"]').val(edate);

        //var ControlDate = $('.pickupDate').datepicker('getDate');
        //var nextDayDate = new Date();
        var nextDayDate = new Date(ControlDate.getFullYear(), ControlDate.getMonth(), ControlDate.getDate() + NumerOfDays);
        //nextDayDate.setDate(ControlDate.getDate() + NumerOfDays);
        //alert(nextDayDate);
        //$('input').val(nextDayDate);

        //$(this).parent().parent().children("input.kanc-date-picker").val(nextDayDate);
        $(this).parent().parent().children("input.kanc-date-picker").datepicker("setDate", nextDayDate);
        $(this).parent().children("input.kanc-date-tracker").val(0);
    });

    //$(document).bind('click', function (e) {
    //    var clicked = $(e.target);
    //    //alert(clicked.html());
    //    //alert(clicked.hasClass('popup-panel-button'));
    //    if (!(clicked.hasClass('popup-panel-button')))
    //        $('.popup-panel').slideUp(RollingTime);
    //});
});

$(document).ajaxSuccess(function () {
    //alert('ajaxSuccess()');
    Decorate();

    //var dialogWidth = $(".kanc-ajax-dialog-submit").attr("data-dialog-width");
    //var dialogHeight = $(".kanc-ajax-dialog-submit").attr("data-dialog-height");

    var submitDlg = $(".kanc-ajax-dialog-submit:not([data-dialog-set])").dialog({
        autoOpen: true,
        width: $(".kanc-ajax-dialog-submit").attr("data-dialog-width"),
        height: $(".kanc-ajax-dialog-submit").attr("data-dialog-height"),
        modal: true,
        draggable: false,
        close: function (event, ui) {
            $(this).dialog('destroy').remove();
        },
        buttons: {
            "Zapisz": function () {
                $(this).parents(".ui-dialog").children(".kanc-ajax-dialog-submit").children("form").submit();
                $(this).dialog("close");
            },
            "Anuluj": function () {
                $(this).dialog("close");
            }
        }
    });

    submitDlg.attr('data-dialog-set', 'true');

    var deleteDlg = $(".kanc-ajax-dialog-delete-submit:not([data-timestamp])").dialog({
        autoOpen: true,
        width: $(".kanc-ajax-dialog-delete-submit").attr("data-dialog-width"),
        height: $(".kanc-ajax-dialog-delete-submit").attr("data-dialog-height"),
        modal: true,
        draggable: false,
        close: function (event, ui) {
            $(this).dialog('destroy').remove();
        },
        buttons: {
            "Usuń": function () {
                $(this).parents(".ui-dialog").children(".kanc-ajax-dialog-delete-submit").children("form").submit();
                $(this).dialog("close");
            },
            "Anuluj": function () {
                $(this).dialog("close");
            }
        }
    });

    deleteDlg.attr('data-dialog-set', 'true');

    $ae.applyAwesomeStyles();

    //$(".kanc-ajax-dialog").dialog("open");
});

var DialogZIndex = 1;

function Decorate()
{
    //alert('Decorate()');

    $(".button").button();
    $(".kanc-button").button();
    $('input[type="submit"]').button();
    $(".kanc-hide-button").button();
    $(".kanc-spinner").spinner();
    $(".buttonset").buttonset();

    //$("input").menu();
    $("input:not(.kanc-spinner)").addClass("text-box single-line ui-menu ui-widget ui-widget-content ui-corner-all");

    $.datepicker.setDefaults($.datepicker.regional["pl"]);

    $(".kanc-date-picker").datepicker({
        changeMonth: true,
        dateFormat: "yy-mm-dd"
    });

    $('input[type="checkbox"]').button();

    //$(".ui-dialog").on("dialogopen", function (event, ui) {
    //    //alert('otwarlem dialog');
    //    //$(this).attr("style:z-index", 100 + DialogZIndex);
    //    $(this).attr("style", $(this).attr("style") + "z-index:1001");
    //    //DialogZIndex++;
    //});
}