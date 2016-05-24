var initSort = function () {
    var $ = jQuery;
    var sortableList = $(".sortable-list");
    sortableList.sortable();
    sortableList.disableSelection();
    sortableList.on("sortstart", function (event, ui) {
        ui.item.find('td').addClass('noborder').not(':first').hide();
    });
    sortableList.on("sortstop", function (event, ui) {
        ui.item.find('td').show().removeClass('noborder');
        var buffer = [];
        sortableList.children('tr').each(function () {
            var item = $(this);
            buffer.push(item.attr('data-id'));
        });
        Server.SaveSort(buffer);
    });
};