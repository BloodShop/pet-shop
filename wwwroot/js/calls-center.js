/*import { signalR } from '../lib/signalr/signalr.js'*/

$(() => {
    LoadCallsData();

    let $theWarning = $("#theWarning");
    $theWarning.hide();

    var connection = new signalR.HubConnectionBuilder().withUrl("/callcenter").build();
    connection.start()
        .then(() => connection.invoke("JoinCallCenters"))
        .catch(err => console.error(err.toString()));

    connection.on("NewCallReceivedAsync", () => LoadCallsData());
    connection.on("CallDeletedAsync", () => LoadCallsData());
    connection.on("CallEditedAsync", () => LoadCallsData());

    function LoadCallsData() {
        var tr = '';
        $.ajax({
            url: '/Calls/GetCalls',
            method: 'GET',
            success: (result) => {
                $.each(result, (k, v) => {
                    tr += `<tr>
                        <td>${v.Name}</td>
                        <td>${v.Email}</td>
                        <td>${moment(v.CallTime).format("llll")}</td>
                        <td>
                            <a href="../Calls/Details?id=${v.Id}" class="btn btn-sm btn-success deatils-button" data-id="${v.Id}">Details</a>
                            <a href="../Calls/Edit?id=${v.Id}" class="btn btn-sm btn-danger edit-button" data-id="${v.Id}">Edit</a>
                            <a href="../Calls/Delete?id=${v.Id}" class="btn btn-sm btn-warning delete-button" data-id="${v.Id}">Delete</a>
                        </td>
                    </tr>`;
                })
                $("#logBody").html(tr);
            },
            error: (error) => {
                $theWarning.text("Failed to get calls...");
                $theWarning.show();
                console.log(error)
            }
        });
    }
});