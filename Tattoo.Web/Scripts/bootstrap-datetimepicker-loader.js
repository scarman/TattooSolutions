$('.date').datetimepicker({
        icons: {
            time: "fa fa-clock-o",
            date: "fa fa-calendar",
            up: "fa fa-arrow-up",
            down: "fa fa-arrow-down"
        }
    })
    .each(function() {
        var $input = $(this);
        var $parent = $input.parent();
        $parent.find('.datetimepicker-reset').on('click', function() {
            $input.val('');
            $input.datetimepicker('update');
        });
        $parent.find('.datetimepicker-show').on('click', function() {
            $input.datetimepicker('show');
        });
    });
