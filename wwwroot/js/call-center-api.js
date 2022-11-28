$(() => {
    let calls = [];
    let $logBody = $("#logBody");
    let $theWarning = $("#theWarning");
    $theWarning.hide();


    var connection = new signalR.HubConnectionBuilder().withUrl("/callcenter").build();
    connection.start()
        .then(() => {
            connection.invoke("JoinCallCenters");
        })
        .catch(err => console.error(err.toString()));

    connection.on("NewCallReceivedAsync", newCall => addCall(newCall));
    connection.on("CallEditedAsync", editCall => editCall(newCall));
    

    $logBody.on("click", ".delete-button", () => deleteCall(this));
    $logBody.on("click", ".edit-button", () => editCall(this));


    function addCalls() {
        $logBody.empty();
        $.each(calls, (i, c) => addCall(c));
    }

    function addCall(call) {
        let template = `<tr>
            <td>${call.name}</td>
            <td>${call.email}</td>
            <td>${moment(call.callTime).format("llll")}</td>
            <td>
                <a href="../Calls/Details?id=${v.Id}" class="btn btn-sm btn-success deatils-button" data-id="${v.Id}">Details</a>
                <a class="btn btn-sm btn-danger edit-button" data-id="${v.Id}">Edit</a>
                <a class="btn btn-sm btn-warning delete-button" data-id="${call.id}">Delete</a>
            </td>
        </tr>`;
        $logBody.append($(template));
    }

    function editCall(button) {
        let id = $(button).attr("data-id");
        $.ajax({
            //dataType: "json",
            //data: jsonCall,
            //enctype: 'multipart/form-data',
            url: `/api/calls/${id}`,
            type: 'POST',
        })
            .then(res => {
                getCalls();
            })
    }

    function deleteCall(button) {
        let id = $(button).attr("data-id");
        $.ajax({
            url: `/api/calls/${id}`,
            method: "DELETE"
        })
            .then(res => {
                $(button).closest("tr").remove();
            });
    }

    function getCalls() {
        $.getJSON("/api/calls")
            .then(res => {
                calls = res;
                addCalls();
            })
            .catch(() => {
                $theWarning.text("Failed to get calls...");
                $theWarning.show();
            });
    }

    getCalls();
});