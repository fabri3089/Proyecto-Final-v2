$(document).ready(function () {
    Catalog.init();
});


var Catalog = {
    allActivitiesSelector: "activityAll",

    activities: [],

    paymentTypesDetails: [],

    init: function () {
        Catalog.activities = $("div.list-group.activities a");
        Catalog.paymentTypesDetails = $("div.paymentTypeDetail");

        Catalog.paymentTypesDetails.slideDown(); //show All by default

        Catalog.activities.click(function () { //click handlers for <a> tags
            Catalog.setActive($(this));
            Catalog.showPaymentTypeDetail($(this));
        });
    },

    setActive: function (activity) {
        Catalog.activities.removeClass("active");
        activity.addClass("active");
    },

    showPaymentTypeDetail: function (activity) {
        var id = activity.prop("id");
        Catalog.paymentTypesDetails.hide(); //hide all details

        if (id == Catalog.allActivitiesSelector) { //show all paymentType's detail availables
            Catalog.paymentTypesDetails.slideDown(1000);
        }
        else {
            Catalog.paymentTypesDetails.each(function () {  //show only the paymentType's detail that match the activity
                var abono = $(this);
                if (abono.hasClass(id)) { $(this).slideDown(1000); }
            });
        }
    },
}