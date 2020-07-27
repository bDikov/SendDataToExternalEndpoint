let intervalueId;

poll = (orderId) => {
    fetch(`/GetUpdateForOrder/${orderId}`)
        .then(response => {
            if (response.status === 200) {
                const statusDiv = document.getElementById("status");
                response.json().then(j => {
                    statusDiv.innerHTML = j.update;
                    if (j.finished)
                        clearInterval(intervalueId);
                    connection.Stop();
                    return;
                });
                connection.Stop();
                return;
            }
            connection.Stop();
            return;
        });
};

document.getElementById("btnSendAsp").addEventListener("click", e => {
    e.preventDefault();
    const email = document.getElementById("emailId").value;       
        $.ajax({
            async: true,
            data: $('#data').serialize(),
            type: "POST",
            url: '/Form/SendForm',
            success:
                function (response) {

                    console.log(88);
                    var text = response;                   
                    intervalueId = setInterval(poll, 1000, text);
                    connection.Stop();
                    return;
                },
            error: function (response) {
                alert("Invalid arguments");
                connection.Stop();
                return;
            }
        });
    connection.Stop();
    return;
});



