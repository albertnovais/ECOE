//Loader

$(document).ready(function () {
    $('.loader').fadeOut("slow");
})

//autocomplete

$(document).ready(function () {
    $('#NomeU').autocomplete({
        source: '@Url.Action("Search", "Pessoa")'
    })
})


//Abrir Modal

$('#myModal').on('shown.bs.modal', function () {
    $('#myButton').trigger('focus')
})



//            API para Edição de Texto
//TinyMCE https://www.tiny.cloud
//

tinymce.init({
    selector: '#mytextarea',
    plugins: "image, print",
    language: 'pt_BR',
    language_url: "http://localhost:50168/Scripts/pt_BR.js"
});

//Datatables


$(document).ready(function () {
    $('#datatables').DataTable({
        "dom": '<"top"fB>rt<"bottom"il>p<"clear">',
        colReorder: true,
        responsive: true,
        buttons: [
            {
                extend: 'copyHtml5',
                text: 'Copiar',
                autoPrint: false,
            },
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                autoPrint: false,
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
            },
        ],
        "pagingType": "first_last_numbers",
        "language": {
            "sEmptyTable": "Nenhum registro encontrado",
            "sProcessing": "A processar...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "Não foram encontrados resultados",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando de 0 até 0 de 0 registros",
            "sInfoFiltered": "(filtrado de _MAX_ registros no total)",
            "sInfoPostFix": "",
            "sSearch": "Procurar:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Primeiro",
                "sPrevious": "Anterior",
                "sNext": "Seguinte",
                "sLast": "Último"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            },
        }
    }
    )
});


//Paginação

$(document).ready(function () {
    var item = $('a[href*="' + window.location.pathname + '"]');
    item.parent().addClass("active");
    item.parent().parent().parent().addClass("active menu-open");
});
