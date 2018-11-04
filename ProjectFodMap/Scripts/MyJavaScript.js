
$(document).ready(function () {
    // Stops the form from being submitted and just rendering raw text when user presses enter.
    $(window).keydown(function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            return search();
        }
    });
    $('#SubmitButton').click(function () {
        return search();
    });
});

function search() {
    var serchStr = $('#SearchBar').val();
    $.ajax({
        url: '/Meal/Meals',
        data: { 'searchStr': serchStr },
        type: "GET",
        cache: false,
        success: function (partialViewResult) {
            $("#tMeals").html(partialViewResult);  // jQuery magic right here
        },
        error: function () {
        }
    });
    $('#SearchBar').val('');
}

function checkMealName() {
    if ($('#MealName').val === "") {
        $("#submit").prop('disabled', false);
        // event.preventDefault(); // Need something to stop form submission
    }
}