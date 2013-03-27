$(function () {

    $('#search-form').submit(function () {
        if ($('#searchCriteria').val().length != 0)
            return true;
        return false;
    });
});