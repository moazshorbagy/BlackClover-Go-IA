# Communication

The library used is not yet decided.

The communication will done using the WebSocket protocol (RFC-6455). You will find many libraries for websocket support on different languages but make sure it supports RFC-6455 to ensure it can communicate with the server. The library I am using for websockets on Node.js is https://github.com/websockets/ws


- The communication will be done using strings containing JSON objects. You can also find many libraries that work with JSON on any language. Typescript has native support for JSON so I am not using any libraries.


- The server will use a Heartbeat system (ping-pong messages) to ensure that the clients are still connected to the server. Since Ping-pong messages will be sent very frequently (~1 message every second), the client has to be responsive all times even if the agent is still thinking. Therefore, you must make sure that the client code is not blocked by the agent code by running each of them on a separate thread and making sure that the client thread keeps polling for messages (or responding to ping events) all the time.


- The main flow of the communication will mainly be as follows:

1- Whenever the client requests a connection to the server, the server will either terminate the connection (if many clients are already connected) or send a message to request the name of the client.

2- After the client respond with their name and they should wait in a ready state till the game starts.

3- When the game starts, the server will send a game start message containing the game configuration (which could be ignored since they fixed during the competition) and the current state (containing the initial board, prisoners and remaining time for each player and a history of moves to apply to the given board in order to reach the current state). The message will also tell each player whether he will play as black or white.

4- The server will wait for the current player to send their move and upon receiving it, it will check if it valid or not. In both cases, the current player will receive a message to notify him if his move is valid or not. Also, if it is valid, the next player will receive a message containing the move. All the messages sent by the server during the game will contain the remaining time for each player.

5- The game could end for any of the following reasons: the game actually ended (by a resign or consecutive passes) or the game was ended prematurely by the user or due to a communication error. The client should expect an end message at any time. The end message will contain the reason for termination in addition to the winner's color and the scores. In all cases, the client shouldn't terminate the connection just because the game ended; if the reason is an error or a pause, it should go back to the ready state and wait for another game start message. This design choice is taken in order to quickly recover from any connection error. The new start game message will contain the move history so that the client side can regenerate the last state.


- The server will not be able to measure the ping time so it will consider it as part of the current player's time. However, all of the server processing time will not be deducted from any side (the timer will be paused before the server processes any message and resumed after the messages are sent).


The full details will be sent with the communication document which I expect to be ready alongside the server at the end of the midterm's week. Till then, the communication team should prepare their libraries of choice and the base code of the client. You could use the simple server example in the readme of https://github.com/websockets/ws to ensure that their libraries can work with ws and can send/receive json objects.