var $TABLE = $('#table');
var $BTN = $('#export-btn');
var $EXPORT = $('#export');

$('.table-add').click(function () {
    var $clone = $TABLE.find('tr.hide').clone(true).removeClass('hide table-line');
    $TABLE.find('table').append($clone);
});

$('.table-remove').click(function () {
    $(this).parents('tr').detach();
});

$('.table-up').click(function () {
    var $row = $(this).parents('tr');
    if ($row.index() === 1) return; // Don't go above the header
    $row.prev().before($row.get(0));
});

$('.table-down').click(function () {
    var $row = $(this).parents('tr');
    $row.next().after($row.get(0));
});

// A few jQuery helpers for exporting only
jQuery.fn.pop = [].pop;
jQuery.fn.shift = [].shift;

$BTN.click(function () {
    var $rows = $TABLE.find('tr:not(:hidden)');
    var headers = [];
    var data = [];

    // Get the headers (add special header logic here)
    $($rows.shift()).find('th:not(:empty)').each(function () {
        headers.push($(this).attr('data-json'));
    });

    // Turn all existing rows into a loopable array
    $rows.each(function () {
        var $td = $(this).find('td');
        var h = {};

        // Use the headers from earlier to name our hash keys
        headers.forEach(function (header, i) {
            h[header] = $td.eq(i).text();
        });

        data.push(h);
    });

    Config.call(Config.url, 'POST', data, Config.successCallBack, Config.errorCallBack);
});


var Config = {

    url: '/Files/Save',

    call: function (url, type, data, successCallBack, errorCallBack) {
        $.ajax({
            type: type,
            url: url,
            data: Config.buildData(data),
            contentType: 'application/json;',
            dataType: 'json',
            success: successCallBack,
            error: errorCallBack
        });
    },

    successCallBack: function (response) {
        if (response.Result = "OK") {
            $EXPORT.text('¡Sus cambios se han guardado exitosamente!')
                   .removeClass('text-danger').addClass('text-success')
                   .fadeIn(400).fadeOut(9000);
        }
        else {
            $EXPORT.text('Ha surgido un error en el guardado. Intenta más tarde.')
                .removeClass('text-success').addClass('text-danger')
                .fadeIn(400).fadeOut(9000);
        }
    },

    errorCallBack: function (xhr, textStatus, errorThrown) {
        $EXPORT.text('Ha surgido un error en el guardado. Intenta más tarde.')
               .removeClass('text-success').addClass('text-danger')
               .fadeIn(400).fadeOut(9000);
    },

    buildData: function (data) {
        $.each(data, function (i) {
            data[i].RoutineID = $("#RoutineID").val();
        });
        return JSON.stringify(data);
    }
};