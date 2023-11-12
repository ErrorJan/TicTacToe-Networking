# TicTacToe-Networking
A project for school where we have two (or more) clients playing Tic Tac Toe with each other.
Achieved by the clients connecting to the server and the server being the main source of truth. Client-Server-Architecture

Made by: Jan König, Liam Haid, Karlo Bilić<br>
License: Nothing (for now? Maybe [MIT license](https://opensource.org/license/mit/)?)

## Sub-projects
### TicTacToe-Client
Here the client code resides. Everything from GUI to sending the information what the player clicked to the server.

### TicTacToe-Server
Here the server code resides. Everything from a TUI to the actual code listening for new clients/players that want to connect to the server and start a game.
The server code is multi threaded.
The TUI has it's own thread, and if it crashes it can be restored.
~~The Main thread is tasked with handling cross thread events like quitting or when something crashes for it to restore it. It also handles new connections and creates a new thread, when a new session needs to be created.~~
The Session(s) thread is where the players will play against each other.

### TicTacToe-Shared
Here resides the library that is shared between all the projects (mainly the client and the server)
Here the data of the netcode is stored.

### TicTacToe-Server-Test
This is a unit test project for the server to test various functionality. Mainly the netcode portion.
You could say that this project enforces the netcode protocol.
