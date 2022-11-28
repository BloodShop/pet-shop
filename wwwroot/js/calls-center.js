﻿/*import { signalR } from '../lib/signalr/signalr.js'*/

$(() => {
    LoadCallsData();

    let calls = [];
    let $logBody = $("#logBody");
    //let $theWarning = $("#theWarning");

    var connection = new signalR.HubConnectionBuilder().withUrl("/callcenter").build();
    connection.start()
        .then(() => {
            connection.invoke("JoinCallCenters");
        })
        .catch(err => console.error(err.toString()));

    connection.on("NewCallReceivedAsync", function() {
        LoadCallsData();
    });
    connection.on("CallDeletedAsync", function () {
        LoadCallsData();
    });
    connection.on("CallEditedAsync", function () {
        LoadCallsData();
    });

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
                console.log(error)
            }
        });
    }
});
