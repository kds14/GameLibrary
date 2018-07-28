# GameLibrary
(WIP) Various reusable classes for game development

## Algorithm/GridSearch.cs
Function FindPath is A* pathfinding (shown in yellow) and BuildRadius is Breadth-First Search used for targeting (shown in blue at the end).

![](https://i.imgur.com/QlzHbxo.gif)

## DataStructures/PriorityQueue.cs
A priority queue used for the A* pathfinding in GridSearch. Any priority queue implementation could be substituted.

## Models/Point.cs and Models/Grid.cs
Model classes that are used in GridSearch. Point could easily be substituted.

## Network/Network.cs
A basic peer-to-peer (P2P) network for turn based games. It uses the Lidgren networking library.
