$(function () {
    $('.js-basic-example').DataTable({
        "dom": "<'row'<'col-md-6 col-sm-6'i><'col-md-6 col-sm-6'f>><'table-responsive't><'row'<'col-md-6 col-sm-6'l><'col-md-6 col-sm-6'p>>"
    });

    //Exportable table
    $('.js-exportable').DataTable({
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ]
    });
});