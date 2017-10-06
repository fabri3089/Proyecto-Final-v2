//Init Global Configuration
var Config = { //Variable que contiene una llamada AJAX genérica y una propiedad para setear las urls a usar
    template: '',

    url: {
        preview: '/Emails/Preview'
    },

    call: function (url, type, object, successCallBack, errorCallBack) {
        $.ajax({
            type: type,
            url: url,
            data: JSON.stringify(object),
            contentType: 'application/json;',
            dataType: 'json',
            success: successCallBack,
            error: errorCallBack
        });
    },

    successCallBack: function (response)
    {
        if (response.Result = "OK")
        {
            Config.template = response.Data;
            Config.insertEmailPreview(Config.template);
            $("#previewRegion").show();
        }
        else
        {
            //TODO
        }
    },

    errorCallBack: function (xhr, textStatus, errorThrown)
    {
        //TODO
    },

    buildData: function ()
    {
        return { Title: $("#Title").val(), SubTitle: $("#SubTitle").val(), InnerTitle: $("#InnerTitle").val(), Description: $("#Description").val(), }
    },

    insertEmailPreview: function (html)
    {
        var doc = document.getElementById('iframe').contentWindow.document;
        doc.open();
        doc.write(html);
        doc.close();
    }
};
//End Global Configuration

$(document).ready(function () {
    $("#btnCreate").click(function () {
        Config.call(Config.url.preview, 'POST', Config.buildData(), Config.successCallBack, Config.errorCallBack);
    })

    $("input[type='reset']").click(function () {
        $("input[type='text']").val("");
    })
});

