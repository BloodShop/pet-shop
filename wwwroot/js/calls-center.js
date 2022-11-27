// calls-center.js
"use strict";

const client = new signalR.HubConnectionBuilder()
    .withUrl("/callcenter")
    .build();

client.on("NewCallReceived", newCall => {
    addCall(newCall);
});

let $theWarning = $("#theWarning");
let $logBody = $("#logBody");
let calls = [];

$theWarning.hide();
$logBody.on("click", ".delete-button", function () {
    deleteCall(this);
});

function addCalls() {
    $logBody.empty();
    $.each(calls, (i, c) => addCall(c));
}

function addCall(call) {
    let template = `<tr>
            <td>${call.name}</td>
            <td>${call.email}</td>
            <td>${moment(call.callTime).format("llll")}</td>
            <td><button class="btn btn-sm btn-warning delete-button" data-id="${call.id}">Clear</button></td>
        </tr>`;
    $logBody.append($(template));
}

function deleteCall(button) {
    let id = $(button).attr("data-id");
    $.ajax({
        url: `/api/calls/${id}`,
        method: "delete"
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
            client.start()
                .then(() => {
                    client.invoke("JoinCallCenters");
                })
                .catch(err => console.error(err.toString()));
        })
        .catch(() => {
            $theWarning.text("Failed to get calls...");
            $theWarning.show();
        });
}

getCalls();