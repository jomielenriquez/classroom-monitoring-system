error = {
    displayError: function (title, message, btn) {
        $("#errorTitle").html(title);
        $("#errorMessage").html(message);
        $('#errorOverlay').css('display', 'flex');
        // Overwrite existing click handler
        const scanBtn = document.getElementById('modalBtn');
        scanBtn.onclick = null; // clear previous click events
        scanBtn.onclick = btn;  // assign new one
    }
}
device = {
    enrollFingerPrint: async function (vars) {
        if (vars.userId == null || vars.userId == undefined || vars.userId == "") {
            error.displayError(
                "An error occured",
                "Please select user",
                function () {
                    $('#errorOverlay').css('display', 'none');
                }
            );
            return;
        }

        $('#loadingOverlay2').css('display', 'flex');

        try {
            const response = await fetch("http://localhost:5000/enroll", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ userId: vars.userId })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.isSuccessful) {
                result.userId = vars.userId;
                console.log(result);
                fingerPrint.saveFingerPrint(result);
            }
            else {
                error.displayError(
                    "An error occured",
                    "Invalid fingerprint",
                    function () {
                        $('#errorOverlay').css('display', 'none');
                    }
                );
            }

        } catch (error) {
            error.displayError(
                "An error occured",
                error,
                function () {
                    $('#errorOverlay').css('display', 'none');
                }
            );
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

            if (result.isSuccessful) {
                monitoring.login(result);
            }
            else {
                error.displayError(
                    "An error occured",
                    "Invalid fingerprint",
                    function () {
                        $('#errorOverlay').css('display', 'none');
                    }
                );
            }
        } catch (error) {
            error.displayError(
                "An error occured",
                error,
                function () {
                    $('#errorOverlay').css('display', 'none');
                }
            );
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
                error.displayError(
                    "Error",
                    result.message,
                    function () {
                        $('#errorOverlay').css('display', 'none');
                    }
                );
            } else {
                console.log(result);
            }

        } catch (error) {
            error.displayError(
                "An error occured",
                error,
                function () {
                    $('#errorOverlay').css('display', 'none');
                }
            );
        } finally {
            $('#loadingOverlay').css('display', 'none');
        }
    }
}

fingerPrint = {
    saveFingerPrint: async function (vars) {
        $('#loadingOverlay').css('display', 'flex');
        try {
            loadingOverlay2.style.display = 'flex';
            console.log({ positionNumber: vars.position, userId: vars.userId });
            const response = await fetch("/Fingerprint/RegisterNew", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ positionNumber: vars.position, userId: vars.userId })
            });

            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }

            const result = await response.json();

            if (result.isSuccessful) {
                error.displayError(
                    "Succses",
                    "Fingerprint successfully saved.",
                    function () {
                        window.location.href = "/Device/Dashboard";
                    }
                );
            } else {
                error.displayError(
                    "An error occured",
                    result.message,
                    function () {
                        $('#errorOverlay').css('display', 'none');
                    }
                );
            } 

        } catch (error) {
            error.displayError(
                "An error occured",
                error,
                function () {
                    $('#errorOverlay').css('display', 'none');
                }
            );
        } finally {
            $('#loadingOverlay').css('display', 'none');
        }
    }
}

