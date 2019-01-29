// Write your JavaScript code.
$(document).ready(function () {
    $('a.show-more').on('click', function () {
        $($(this).data('target')).dropdown('toggle');
        return false;
    });
});