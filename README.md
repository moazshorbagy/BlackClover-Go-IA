# BlackClover-Go-IA
An intelligent agent that plays the board game Go.


## Table of Content
* [Introduction](#Introduction)
* [Core](#Core)
* [GUI](#GUI)
* [Communication](#Communication)

## Introduction
The intelligent agent is called BlackClover and it's implemented in C#.
BlackClover plays on a tweaked version of the original Go game rules.
BlackClover plays on a 19x19 board and can, hopefully, win.

The project runs 3 threads; One for the agent(Core), one for the GUI and the third for Communication.

## Core
Core contains the logic needed to build the agent BlackClover. BlackClover uses **Monte Carlo Tree Search**  as 
the search algorithm. Zobrist hashing is used to hash the State that is put in the tree node. Core contains the 
problem definition of Go game. It also contains the required models as Board, State, GameInfo and Rules.

## GUI
The GUI runs on Unity game engine. It interfaces with the Core. The GUI supports 2 different modes.
The first is playing against a human. The other is playing against another AI on another PC.

## Communication
Communicating with the server that can allow us to play against another IA. This communication is done using 
the WebSocket protocol (RFC-6455). Communication will be done using strings containing JSON objects.
