# Battleships

## Task

The program should create a 10x10 grid, and place several ships on the grid at random with the following sizes:

1x Battleship (5 squares)

2x Destroyers (4 squares)

The player enters or selects coordinates of the form “A5”, where “A” is the column and “5” is the row, to specify a square to target. Shots result in hits, misses or sinks. The game ends when all ships are sunk.

## Gameplay

There are two game modes implemented "Easy" and "Hard"

In easy mode (type "easy") you can see all battleships on the battlefield - nice chance to test mechanics

In hard mode (type "hard") - battleships are hidden

To shoot - enter coodinates e.g. A1 or J10 or E5 and etc...

To give up - type "quit"

To exit the game (close application) - type "exit"

## Application architecture

### Packages used: 

**Microsoft.Extensions.Hosting** - to use at least dependency injection engine

**Spectre.Console** - cool package to easily draw console outputs 

### Business logic devided in couple blocks:

**GameManager** - contains game business logic

**GameDesigner** - generate map end iniit game data

**GameViewModel** - contains logic to draw view of the game

### Unit tests:

Unit tests are not created because of couple reasons:

1. Not requested in the task
2. Take additional time and money to create
3. Much easier and productive is to manually test applications of such size 
4. To review unit tests style you may check other reposiotries (e.g. https://github.com/hamster-coder-pro/sport-radar/tree/master/SportRadar/SportRadar.Services.Tests)