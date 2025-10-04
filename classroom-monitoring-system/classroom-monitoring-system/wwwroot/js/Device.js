device = {
    enrollFingerPrint: async function (userId) {
        try {
            const response = await fetch("http://localhost:5000/enroll", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ userId: userId })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();
            console.log("Enroll result:", result);

            alert("Fingerprint Enrollment: " + JSON.stringify(result));
        } catch (error) {
            console.error("Error:", error);
            alert("Error: " + error.message);
        }
    }
}