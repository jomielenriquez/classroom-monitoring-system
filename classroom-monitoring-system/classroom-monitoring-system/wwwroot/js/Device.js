device = {
    enrollFingerPrint: async function (userId) {
        $('#loadingOverlay2').css('display', 'flex');

        try {
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

            loadingOverlay.style.display = 'none';
        } catch (error) {
            console.error("Error:", error);
        } finally {
            $('#loadingOverlay2').css('display', 'none');
        }
    },
    verifyFingerPrint: async function () {
        $('#loadingOverlay2').css('display', 'flex');

        try {
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

            console.log(result)
            if (result.isSuccessful) {
                monitoring.login(result);
            }
        } catch (error) {
            console.error("Error:", error);
        } finally {
            $('#loadingOverlay2').css('display', 'none');
        }
    }
}

monitoring = {
    login: async function (vars) {
        $('#loadingOverlay').css('display', 'flex');
        console.log(vars);
        try {
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

            if (result.redirectUrl) {
                window.location.href = result.redirectUrl;
            } else if (!result.isSuccessful) {
            } else {
                console.log(result);
            }

        } catch (error) {
            console.error("Error:", error);
        } finally {
            $('#loadingOverlay').css('display', 'none');
        }
    }
}

