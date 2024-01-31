var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            url: '/admin/getall',
            dataSrc: 'data'
        },
        "columns": [
            /* { data: 'productId', "width": "15%" },*/
            { data: 'productName', "width": "15%" },
            { data: 'productDescription', "width": "15%" },
            { data: 'productPrice', "width": "15%" },
           /* { data: 'productImage', "width": "15%" },*/ // Add this line for productImage
            { data: 'isAvailable', "width": "15%" } , // Add this line for isAvailable
            /*{ data: 'isActive', "width": "15%" },*/      // Add this line for isActive
            
            /*{ data: 'isTrending', "width": "15%" }  */
            { data: 'categoryName', "width": "15%" },
            {
                data: 'productCreatedAt', "width": "15%", render: function (data, type, row) {
                    // Format the date using Moment.js
                    return moment(data).format('YYYY-MM-DD HH:mm:ss');
                }
            },
            {
                data: 'productId',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                      <a href="/Product/Update/${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square "></i> Edit</a>   
          

                      <a onClick=Delete('/admin/DeleteProduct/${data}') class="btn btn-danger mx-2"> <i class="bi bi-trash-fill"></i> Delete</a>
                     </div>`
                },
                "width": "25%"
            }
        ],
        "order": [[3, 'desc']] // Sort by the 'productCreatedAt' column in descending order
    });
}

