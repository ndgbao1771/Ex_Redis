const API_URL = 'https://localhost:7126/api/Users';
let userModal;

document.addEventListener('DOMContentLoaded', function () {
    userModal = new bootstrap.Modal(document.getElementById('userModal'));
    loadUsers();
});

function loadUsers() {
    $.ajax({
        url: API_URL,
        method: 'GET',
        success: function (users) {
            const tbody = $('#userTableBody');
            tbody.empty();
            users.forEach(user => {
                tbody.append(`
                    <tr>
                        <td>${user.id}</td>
                        <td>${user.name}</td>
                        <td>${user.email}</td>
                        <td>${user.phone}</td>
                        <td>${user.address}</td>
                        <td>${new Date(user.registerDate).toLocaleDateString()}</td>
                        <td>
                            <button class="btn btn-sm btn-primary btn-action" onclick="editUser(${user.id})">Edit</button>
                            <button class="btn btn-sm btn-danger btn-action" onclick="deleteUser(${user.id})">Delete</button>
                        </td>
                    </tr>
                `);
            });
        },
        error: function (xhr, status, error) {
            console.error('Error:', error);
            console.log('Status:', status);
            console.log('Response:', xhr.responseText);
        }
    });
}

function showCreateModal() {
    $('#userForm')[0].reset();
    $('#userId').val('');
    $('#modalTitle').text('Create User');
    userModal.show();
}

function editUser(id) {
    $.ajax({
        url: `${API_URL}/${id}`,
        method: 'GET',
        success: function (user) {
            $('#userId').val(user.id);
            $('#name').val(user.name);
            $('#email').val(user.email);
            $('#phone').val(user.phone);
            $('#address').val(user.address);
            $('#modalTitle').text('Edit User');
            userModal.show();
        }
    });
}

function saveUser() {
    const userId = $('#userId').val();
    const userData = {
        id: userId ? parseInt(userId) : 0,
        name: $('#name').val(),
        email: $('#email').val(),
        phone: $('#phone').val(),
        address: $('#address').val()
    };

    $.ajax({
        url: userId ? `${API_URL}/${userId}` : API_URL,
        method: userId ? 'PUT' : 'POST',
        contentType: 'application/json',
        data: JSON.stringify(userData),
        success: function () {
            userModal.hide();
            loadUsers();
        }
    });
}

function deleteUser(id) {
    if (confirm('Are you sure you want to delete this user?')) {
        $.ajax({
            url: `${API_URL}/${id}`,
            method: 'DELETE',
            success: function () {
                loadUsers();
            }
        });
    }
}