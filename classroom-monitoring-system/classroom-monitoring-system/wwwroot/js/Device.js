device = {
    enrollFingerPrint: function (userId) {
        // Show the loader
        $('#loadingOverlay2').css('display', 'flex');

        $.ajax({
            url: "http://localhost:5000/enroll",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ userId: userId }),
            success: function (result) {
                console.log(result)
                //alert("Fingerprint Enrollment: " + JSON.stringify(result));
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                //alert("Error: " + error);
            },
            complete: function () {
                // Always hide the loader
                $('#loadingOverlay2').css('display', 'none');
            }
        });
    },
    verifyFingerPrint: function () {
        // Show the loader
        $('#loadingOverlay2').css('display', 'flex');

        $.ajax({
            url: "http://localhost:5000/verify",
            method: "POST",
            contentType: "application/json",
            success: function (result) {
                alert(result.position);
                console.log(result)
                if (result.isSuccessful) {
                    monitoring.login(result);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                //alert("Error: " + error);
            },
            complete: function () {
                // Always hide the loader
                $('#loadingOverlay2').css('display', 'none');
            }
        });
    }
}

monitoring = {
    login: function (vars) {
        $('#loadingOverlay').css('display', 'flex');
        console.log(vars);
        //$.ajax({
        //    url: "Login/LoginUsingDevice",
        //    method: "POST",
        //    data: { possitionNumber: vars.position },
        //    success: function (response) {
        //        console.log("logging in");
        //        console.log(response);
        //        if (response.isSuccessful) {
        //            // Redirect manually
        //            window.location.href = response.redirectUrl;
        //        } else {
        //            alert(response.message);
        //        }
        //    },
        //    error: function (xhr, status, error) {
        //        console.error("Error:", error);
        //        //alert("Error: " + error);
        //    },
        //    complete: function () {
        //        // Always hide the loader
        //        $('#loadingOverlay').css('display', 'none');
        //    }
        //});


        $.ajax({
            url: '/Login/LoginUsingDevice',
            type: 'POST',
            data: { positionNumber: vars.position },
            success: function (response) {
                if (response.redirectUrl) {
                    // Redirect if login successful
                    window.location.href = response.redirectUrl;
                } else {
                    alert("Login failed: " + (response.message || "Unknown error"));
                }
            },
            error: function (xhr, status, error) {
                console.error(error);
                alert("AJAX error: " + error);
            },
            complete: function () {
                // Always hide the loader
                $('#loadingOverlay').css('display', 'none');
            }
        });
    }
}

//try {
//    // 👇 Show the loader before starting
//    loadingOverlay2.style.display = 'flex';

//    const response = await fetch("http://localhost:5000/enroll", {
//        method: "POST",
//        headers: {
//            "Content-Type": "application/json"
//        },
//        body: JSON.stringify({ userId: userId })
//    });

//    if (!response.ok) {
//        throw new Error(`HTTP error! status: ${response.status}`);
//    }

//    const result = await response.json();

//    // Hide the loader
//    loadingOverlay.style.display = 'none';

//    alert("Fingerprint Enrollment: " + JSON.stringify(result));
//} catch (error) {
//    console.error("Error:", error);
//    alert("Error: " + error.message);
//} finally {
//    // 👇 Always hide the loader (even if error)
//    loadingOverlay2.style.display = 'none';
//}