device = {
    enrollFingerPrint: async function (userId) {
        // Show the loader
        $('#loadingOverlay2').css('display', 'flex');

        try {
            // 👇 Show the loader before starting
            loadingOverlay2.style.display = 'flex';

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

            // Hide the loader
            loadingOverlay.style.display = 'none';

            //alert("Fingerprint Enrollment: " + JSON.stringify(result));
        } catch (error) {
            console.error("Error:", error);
            //alert("Error: " + error.message);
        } finally {
            // 👇 Always hide the loader (even if error)
            $('#loadingOverlay2').css('display', 'none');
        }
    },
    verifyFingerPrint: async function () {
        // Show the loader
        $('#loadingOverlay2').css('display', 'flex');

        try {
            // 👇 Show the loader before starting
            loadingOverlay2.style.display = 'flex';

            const response = await fetch("http://localhost:5000/verify", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            // Hide the loader
            loadingOverlay.style.display = 'none';

            //alert("Fingerprint Enrollment: " + JSON.stringify(result));
            //alert(result.position);
            console.log(result)
            if (result.isSuccessful) {
                monitoring.login(result);
            }
        } catch (error) {
            console.error("Error:", error);
            //alert("Error: " + error.message);
        } finally {
            // 👇 Always hide the loader (even if error)
            $('#loadingOverlay2').css('display', 'none');
        }
    }
}

monitoring = {
    login: async function (vars) {
        $('#loadingOverlay').css('display', 'flex');
        console.log(vars);
        try {
            // 👇 Show the loader before starting
            loadingOverlay2.style.display = 'flex';

            const response = await fetch("/Login/LoginUsingDevice", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ positionNumber: vars.position })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            // Hide the loader
            //loadingOverlay.style.display = 'none';

            // Handle the response
            if (result.redirectUrl) {
                // Redirect if login successful
                window.location.href = result.redirectUrl;
            } else if (!result.isSuccessful) {
                //alert("Login failed: " + (result.message || "Unknown error"));
            } else {
                // Optional: handle successful login in JS
                console.log(result);
            }

        } catch (error) {
            console.error("Error:", error);
            //alert("Error: " + error.message);
        } finally {
            // 👇 Always hide the loader (even if error)
            $('#loadingOverlay').css('display', 'none');
        }
    }
}

