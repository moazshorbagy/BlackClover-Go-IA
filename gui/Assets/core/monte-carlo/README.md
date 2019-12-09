# Monte Carlo Tree Search

BlackClover uses **Monte Carlo Tree Search**  as the search algorithm. 
The policy network for now will be random, and in the future we can train or get a trained network and use it.
Monte Carlo parameters will be decided so as to make the whole game time be under than 15 minutes.


Zobrist hashing is used to hash the State that is put in the tree node.
