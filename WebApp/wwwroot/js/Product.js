﻿var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
            { "data": "title", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "5%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "8%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div role="group">
                                <a href="/Admin/Product/Upsert?id=${data}"
                                    class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>&nbsp Edit</a>
                                <a onClick=Delete('/Admin/Product/Delete/${data}')
                                    class="btn btn-danger mx-2"><i class="bi bi-trash3"></i>&nbsp Delete</a>
                            </div>
                        `
                },
                "width": "19%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}