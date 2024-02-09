var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "lengthMenu": [[100, 200, 300, -1], ["100", "200", "300", "All"]],
        "pageLength": 100,
        "ajax": {

            url: '/admin/getall',
            dataSrc: 'data'
        },
        "columns": [
            /* { data: 'productId', "width": "15%" },*/
            { data: 'productName', "width": "15%" },
            { data: 'productDescription', "width": "15%" },
            { data: 'productPrice', "width": "15%" },
            /* { data: 'productImage', "width": "15%" },*/
            { data: 'isAvailable', "width": "15%" },
            /*{ data: 'isActive', "width": "15%" },*/

            { data: 'isTrending', "width": "15%" },
            { data: 'categoryName', "width": "15%" },
            {
                data: 'productCreatedAt', "width": "15%", render: function (data, type, row) {
                    // Format the date using Moment.js
                    return moment(data).format('YYYY-MM-DD');
                }
            },
            //{
            //    data: 'productImage',
            //    width: '15%',
            //    render: function (data, type, row) {

            //        if (type === 'display') {

            //            return `<img src="/image/${data}" class="btn btn-warning mx-2" />`
            //                ;
            //        }

            //        // For other types, return the raw data
            //        return data;
            //    }
            //},
            {
                data: 'productId',
                render: function (data, type, row) {
                    var productId = row.productId;
                    var isAvailable = row.isAvailable;

                    var updateBtn = `<a href="/Product/Update/${productId}" class="btn btn-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Edit
                        </a>`;

                    var deactivateBtn = `<a href="/Product/Deactive/${productId}" class="btn btn-danger">
                                <i class="bi bi-trash-fill"></i> Deactive
                             </a>`;

                    var activateBtn = `<a href="/Product/Active/${productId}" class="btn btn-success">
                                <i class="bi bi-check-circle-fill"></i> Active
                           </a>`;

                    return `<div class="btn-group" role="group">
                    ${isAvailable ? updateBtn + deactivateBtn : updateBtn + activateBtn}
                </div>`;
                },
                width: "45%"
            }


        ],
        "order": [[6, 'desc']],
        "lengthMenu": [100, 200, 300], 
        "pageLength": 100 
       
    });
    

    $('#IsAvailableCheckbox').on('change', function () {
        var isAvailableChecked = $(this).prop('checked');

        console.log('Checkbox state:', isAvailableChecked);

        
        var columnIndex = 3; 
        var searchTerm = isAvailableChecked ? 'true' : '';

        console.log('Applying search:', searchTerm);

        dataTable.column(columnIndex).search(searchTerm).draw();
    });
    //$('#IsTrendingCheckbox').on('change', function () {
    //    var isAvailableChecked = $(this).prop('checked');

    //    console.log('Checkbox state:', isTrendingChecked);

    //    // Use DataTable's search API to filter rows based on checkbox status
    //    var columnIndex = 4; // Change this to the correct index
    //    var searchTerm = isTrendingChecked ? 'true' : '';

    //    console.log('Applying search:', searchTerm);

    //    dataTable.column(columnIndex).search(searchTerm).draw();
    //});


    $('#FilterByCategory').on('change', function () {
        console.log('Change event triggered');
        var selectedCategory = $(this).val();
       /* alert(selectedCategory);*/
       
        dataTable.search('').columns().search('').draw();

        // Apply category filter if not "All"
        if (selectedCategory !== 'all') {
            console.log('Applying category filter:', selectedCategory);
            dataTable.column(5).search(selectedCategory).draw(); 
        }
        $.ajax({
            url: '/Home/Privacy',
            type: 'POST',
            data: { myData: selectedCategory },
            success: function (data) {
                sessionStorage.setItem('TheCategory', selectedCategory)
                console.log(data.success)
            },
            error: function () {
                alert("error");
            }
        });
    });

    

}
