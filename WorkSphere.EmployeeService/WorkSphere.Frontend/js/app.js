const apiUrl = "https://localhost:7206/api/Employee";

async function getEmployees() {
    try {
        const response = await fetch(apiUrl);

        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const result = await response.json();

        console.log(result);
    }
    catch (error) {
        console.error("Error fetching employees:", error);
    }
}

getEmployees();