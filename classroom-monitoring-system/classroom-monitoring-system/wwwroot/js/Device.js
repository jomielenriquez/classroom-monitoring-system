device = {
    enrollFingerPrint: async function (userId) {
        // Show the loader
        $('#loadingOverlay2').css('display', 'flex');

        $.ajax({
            url: "http://localhost:5000/enroll",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ userId: userId }),
            success: function (result) {
                alert("Fingerprint Enrollment: " + JSON.stringify(result));
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
                alert("Error: " + error);
            },
            complete: function () {
                // Always hide the loader
                $('#loadingOverlay2').css('display', 'none');
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