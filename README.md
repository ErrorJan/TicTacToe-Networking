# TicTacToe-Networking
A project for school where we have two (or more) clients playing Tic Tac Toe with each other.
Achieved by the clients connecting to the server and the server being the main source of truth. Client-Server-Architecture

Made by: Jan König, Liam Haid, Karlo Bilić<br>
License: [MIT license](https://opensource.org/license/mit/)

## Sub-projects
### TicTacToe UI
Here the client code resides. Everything from GUI to sending the information what the player clicked to the server.

### TicTacToe-Server
Is the Server that connects both clients together. It sends out events of what changed and the clients just tell it what they want to join.

### TicTacToe-Shared
Here resides the library that is shared between all the projects.
Here the data of the netcode is stored.

### TicTacToe-Server-Test
This is a unit test project for the server to test various functionality. Mainly the netcode portion.
You could say that this project enforces the netcode protocol.
