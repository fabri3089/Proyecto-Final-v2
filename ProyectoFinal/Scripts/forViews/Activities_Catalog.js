$(document).ready(function () {
    Catalog.init();
});


var Catalog = {
    activities: [],

    activitiesDetails: [],

    init: function () {
        Catalog.activities = $("div.container div.row div.list-group a");
        Catalog.activitiesDetails = $("div.thumbnail div.caption-full");

        Catalog.activities.first().addClass("active"); //Set .active class to first <a> tag in list-group
        Catalog.activitiesDetails.hide(); //hide activitiesDetails
        Catalog.activitiesDetails.first().slideDown(); //except firstOne, which is active by default

        Catalog.activities.click(function () { //click handlers for <a> tags
            Catalog.setActive($(this));
            Catalog.showDetail($(this));
        });
    },

    setActive: function (activity) {
        Catalog.activities.removeClass("active");
        activity.addClass("active");
    },

    showDetail: function (activity) {
        var id = activity.prop("id");
        Catalog.activitiesDetails.hide(); //hide all details
        Catalog.activitiesDetails.eq(id).slideDown(1000); //show the activity's detail
    },
}

