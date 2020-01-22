XMLHttpRequest = require("xmlhttprequest").XMLHttpRequest;
WebSocket = require("websocket").w3cwebsocket;
const signalR = require('@aspnet/signalr');
const axios = require('axios');
const apiBaseUrl = process.env.BASE_URL || 'https://netconfbcn2019demo2publishtosignalr.azurewebsites.net';

const data = {
    newMessage: '',
    messages: []
};

getConnectionInfo().then(start);

function start(info) {
    info.accessToken = info.accessToken || info.accessKey;
    info.url = info.url || info.endpoint;

    const options = {
        accessTokenFactory: () => info.accessToken
    };

    const connection = new signalR.HubConnectionBuilder()
        .withUrl(info.url, options)
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on('votingresults', newvote);
    connection.onclose(() => console.log('+++ server closed +++'));
    console.log('connecting to signalr')
    connection.start().then(() => console.log('connected!')).catch(console.error);
}

function getAxiosConfig() {
    const config = {
        headers: {}
    };
    return config;
}

function getConnectionInfo() {
    return axios.post(`${apiBaseUrl}/api/negotiatevotingresults`, null, getAxiosConfig())
        .then(resp => resp.data);
}

function newvote(newvote) {
    console.log('Message received!');
    console.log(newvote);
}