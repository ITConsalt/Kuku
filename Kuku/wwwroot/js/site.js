// Write your JavaScript code.
$(document).ready(function () {
    $('a.show-more').on('click', function () {
        $($(this).data('target')).dropdown('toggle');
        return false;
    });
    $('.filter-product .dropdown-menu').on('show.bs.dropdown', function () { $(this).off('click') });
    $('.filter-product .dropdown-menu a.dropdown-item').on('click', function () {
        window.location = $(this).attr('href');
    });

    $.widget("custom.catcomplete", $.ui.autocomplete, {
        _create: function () {
            this._super();
            this.widget().menu("option", "items", "> :not(.autocomplete-category)");
        },
        _renderMenu: function (ul, items) {
            var that = this,
                currentCategory = "";
            $.each(items, function (index, item) {
                
                if (item.itemType !== currentCategory) {
                    ul.append("<li class='dropdown-header autocomplete-category'>" + item.itemType + "</li>");
                    currentCategory = item.itemType;
                }

                ul.append("<li class='dropdown-item'><a href='" + item.itemLink + "'>" + item.itemName + "</a></li>");
                //ul.append("<a class='dropdown-item' style='padding-left: 35px' href='" + item.itemLink + "'>" + item.itemName + "</a>");
                
            });
        }
    });
    $("[data-autocomplete-source]").each(function () {
        var target = $(this);
        target.catcomplete({
            delay: 500, //default = 300
            minLength: 2,
            //autoFocus: true,
            source: target.attr("data-autocomplete-source")
        });
    });
});