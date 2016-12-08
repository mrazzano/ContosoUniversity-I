var ContosoController = function () {

    var confirmDelete = function (e) {
        var form = this;
        e.preventDefault();

        bootbox.confirm({
            title: "Confirm Delete",
            message: "Are you sure you want to delete this record?",

            buttons: {
                cancel: {
                    label: "No",
                    className: "btn-default"
                },
                confirm: {
                    label: "Yes",
                    className: "btn-danger"
                }

            },
            callback: function (confirmed) {
                if (confirmed) {
                    form.submit();
                }
            }
        });

        return false;
    }

    var init = function () {
        $("#deleteForm").submit(confirmDelete);
    };

    return {
        init: init()
    }
}();