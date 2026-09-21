const apiUrl = "http://localhost:7151/employee/api/Employee";
function showToast(message) {
    const toast = document.getElementById("toast");

    toast.textContent = message;
    toast.style.display = "block";

    setTimeout(() => {
        toast.style.display = "none";
    }, 3000);
}

let editingEmployeeId = null;

const submitButton = document.getElementById("submitButton");
const cancelButton = document.getElementById("cancelButton");


// =========================
// GET ALL EMPLOYEES
// =========================
async function getEmployees() {
    try {

        const token = localStorage.getItem("token");

        const response = await fetch(apiUrl, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        });

        const result = await response.json();

        console.log(result);

        if (!response.ok) {
            alert(result.message || "Unable to load employees.");
            return;
        }

        const tableBody = document.getElementById("employeeTableBody");

        tableBody.innerHTML = `
            <tr>
                <td colspan="7" style="text-align: center;">
                    Loading employees...
                </td>
            </tr>
        `;

        const employees = result.data;

        tableBody.innerHTML = "";

        if (employees.length === 0) {
            tableBody.innerHTML = `
                <tr>
                    <td colspan="7" style="text-align: center;">
                        No employees found.
                    </td>
                </tr>
            `;

            return;
        }

        employees.forEach(employee => {

            const row = `
                <tr>
                    <td>${employee.id}</td>
                    <td>${employee.name}</td>
                    <td>${employee.department}</td>
                    <td>${employee.age}</td>
                    <td>${employee.email}</td>
                    <td>${employee.phone}</td>
                    <td>
                        <button onclick="editEmployee(${employee.id})">
                            Edit
                        </button>

                        <button onclick="deleteEmployee(${employee.id})">
                            Delete
                        </button>
                    </td>
                </tr>
            `;

            tableBody.innerHTML += row;
        });
    }
    catch (error) {

        console.error("Error fetching employees:", error);

        alert("Unable to connect to Employee Service.");
    }
}


// =========================
// CANCEL EDIT
// =========================
function cancelEdit() {

    editingEmployeeId = null;

    document.getElementById("employeeForm").reset();

    submitButton.textContent = "Add Employee";

    cancelButton.style.display = "none";
}

cancelButton.addEventListener("click", cancelEdit);


// =========================
// ADD / UPDATE EMPLOYEE
// =========================
document.getElementById("employeeForm").addEventListener(
    "submit",
    async function (event) {

        event.preventDefault();

        const employee = {
            name: document.getElementById("name").value.trim(),
            department: document.getElementById("department").value.trim(),
            age: Number(document.getElementById("age").value),
            email: document.getElementById("email").value.trim(),
            phone: document.getElementById("phone").value.trim()
        };


        // =========================
        // FRONTEND VALIDATION
        // =========================

        if (employee.name.length < 3 || employee.name.length > 50) {
            alert("Name must be between 3 and 50 characters.");
            return;
        }

        if (employee.department.length === 0 || employee.department.length > 50) {
            alert("Department is required and must not exceed 50 characters.");
            return;
        }

        if (employee.age < 18 || employee.age > 60) {
            alert("Age must be between 18 and 60.");
            return;
        }

        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (!emailPattern.test(employee.email)) {
            alert("Please enter a valid email address.");
            return;
        }

        const phonePattern = /^[0-9]{10}$/;

        if (!phonePattern.test(employee.phone)) {
            alert("Phone number must contain exactly 10 digits.");
            return;
        }


        try {

            let response;

            const token = localStorage.getItem("token");


            // =========================
            // UPDATE EMPLOYEE
            // =========================
            if (editingEmployeeId !== null) {

                response = await fetch(
                    `${apiUrl}/${editingEmployeeId}`,
                    {
                        method: "PUT",
                        headers: {
                            "Content-Type": "application/json",
                            Authorization: `Bearer ${token}`
                        },
                        body: JSON.stringify(employee)
                    }
                );

            }


            // =========================
            // ADD EMPLOYEE
            // =========================
            else {

                response = await fetch(apiUrl, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`
                    },
                    body: JSON.stringify(employee)
                });

            }


            const result = await response.json();

            console.log(result);


            // =========================
            // API ERROR
            // =========================
            if (!response.ok) {

                alert(
                    result.message ||
                    "Operation failed. Please try again."
                );

                return;
            }


            // =========================
            // SUCCESS
            // =========================

            if (editingEmployeeId !== null) {
                showToast("Employee updated successfully!");
            }
            else {
                showToast("Employee added successfully!");
            }


            // Reset to ADD mode
            editingEmployeeId = null;

            submitButton.textContent = "Add Employee";

            cancelButton.style.display = "none";

            document.getElementById("employeeForm").reset();

            await getEmployees();

        }
        catch (error) {

            console.error("Error saving employee:", error);

            alert(
                "Unable to connect to Employee Service."
            );
        }
    }
);


// =========================
// EDIT EMPLOYEE
// =========================
async function editEmployee(id) {

    try {

        const token = localStorage.getItem("token");

        const response = await fetch(
            `${apiUrl}/${id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );

        const result = await response.json();

        console.log(result);


        if (!response.ok) {

            alert(
                result.message ||
                "Unable to load employee."
            );

            return;
        }


        const employee = result.data;

        editingEmployeeId = employee.id;


        // Fill form
        document.getElementById("name").value = employee.name;

        document.getElementById("department").value =
            employee.department;

        document.getElementById("age").value =
            employee.age;

        document.getElementById("email").value =
            employee.email;

        document.getElementById("phone").value =
            employee.phone;


        // Change form to EDIT mode
        submitButton.textContent = "Update Employee";

        cancelButton.style.display = "inline-block";

    }
    catch (error) {

        console.error("Error loading employee:", error);

        alert(
            "Unable to connect to Employee Service."
        );
    }
}


// =========================
// DELETE EMPLOYEE
// =========================
async function deleteEmployee(id) {

    const confirmDelete = confirm(
        "Are you sure you want to delete this employee?"
    );

    if (!confirmDelete) {
        return;
    }


    try {

        const token = localStorage.getItem("token");

        const response = await fetch(
            `${apiUrl}/${id}`,
            {
                method: "DELETE",
                headers: {
                    Authorization: `Bearer ${token}`
                }
            }
        );


        const result = await response.json();

        console.log(result);


        if (!response.ok) {

            alert(
                result.message ||
                "Failed to delete employee."
            );

            return;
        }


        showToast("Employee deleted successfully!");


        await getEmployees();

    }
    catch (error) {

        console.error("Error deleting employee:", error);

        alert(
            "Unable to connect to Employee Service."
        );
    }
}


// =========================
// LOAD EMPLOYEES
// =========================
getEmployees();


// =========================
// LOGIN
// =========================
async function login() {

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch(
        "http://localhost:7091/api/Auth/login",

        {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                username: username,
                password: password
            })
        }
    );

    const result = await response.json();

    if (response.ok) {

        localStorage.setItem("token", result.token);

        alert("Login successful!");

        // Load employees after login
        await getEmployees();

    }
    else {

        alert(result.message || "Login failed");
    }
}