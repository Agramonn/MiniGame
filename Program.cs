﻿/*
Creating your own game is an exciting way to practice your programming skills. Games rely heavily on processing user input to make dynamic decisions. 
very game must also have a set of defined rules that determine actions and events in the game.

Suppose you want to create your own game. You might not be ready to develop a fully featured game, so you decide to start as small as possible. 
You want to move a character across the screen and make it consume an object. The object consume can affect the state of the player. To keep the game going, you want to regenerate the object in a new location once it has been consumed. You decide that you'll need to use methods to keep your game code organized.

You'll develop the following features of a mini-game application:

-A feature to determine if the player consumed the food
-A feature that updates player status depending on the food consumed
-A feature that pauses movement speed depending on the food consumed
-A feature to regenerate food in a new location
-An option to terminate the game if an unsupported character is pressed
-A feature to terminate the game if the Terminal window was resized
*/

Random random = new Random();
Console.CursorVisible = false;
int height = Console.WindowHeight - 1;
int width = Console.WindowWidth - 5;
bool shouldExit = false;

// Console position of the player
int playerX = 0;
int playerY = 0;

// Console position of the food
int foodX = 0;
int foodY = 0;

// Available player and food strings
string[] states = {"('-')", "(^-^)", "(X_X)"};
string[] foods = {"@@@@@", "$$$$$", "#####"};

// Current player string displayed in the Console
string player = states[0];

// Index of the current food
int food = 0;

// Index of the current state
int state = 0;

// Speed of the player
int speed = 0;

// (Optional) False = detect nondirectional key input to exit the program
bool nonDirectionalKey = false;

InitializeGame();
while (!shouldExit) 
{
    if (TerminalResized())
    {
        changeSizeConsole();
        break;
    }
    Move(nonDirectionalKey);
    ConsumeFood();
    MovementAfected();
}
// Exit the game and clear de console if the console was resized
void changeSizeConsole()
{
    if (TerminalResized())
    {
        Console.Clear();
        Console.WriteLine("Console was resized. Program exiting.");
        shouldExit = true;
    }
}
// Returns true if the Terminal was resized 
bool TerminalResized() 
{
    return height != Console.WindowHeight - 1 || width != Console.WindowWidth - 5;
}

// Make player consume food
void ConsumeFood()
{
    if (foodX == playerX && foodY == playerY)
    {
        ChangePlayer(food);
        ShowFood();
    }
}

// Afect the movement for the player depending on which food consumed
void MovementAfected()
{
    switch (state)
    {
        case 1:
            // Increase the right and left movement by 3.
            speed = 1;
            break;
        case 2:
            FreezePlayer();
            speed = 0;
            break;
        default:
            speed = 0;
            break;
    }
}

// Displays random food at a random location
void ShowFood() 
{
    // Update food to a random index
    food = random.Next(0, foods.Length);

    // Update food position to a random location
    foodX = random.Next(0, width - player.Length);
    foodY = random.Next(0, height - 1);

    // Display the food at the location
    Console.SetCursorPosition(foodX, foodY);
    Console.Write(foods[food]);
}

// Changes the player to match the food consumed
void ChangePlayer(int consumedFood) 
{
    state = consumedFood;
    player = states[consumedFood];
    Console.SetCursorPosition(playerX, playerY);
    Console.Write(player);
}

// Temporarily stops the player from moving
void FreezePlayer() 
{
    System.Threading.Thread.Sleep(1000);
    player = states[0];
    state = 0;
}

// Reads directional input from the Console and moves the player
void Move(bool nonDirectionalInput = true) 
{
    int lastX = playerX;
    int lastY = playerY;
    
    switch (Console.ReadKey(true).Key) 
    {
        case ConsoleKey.UpArrow:
            playerY--; 
            break;
		case ConsoleKey.DownArrow: 
            playerY++; 
            break;
		case ConsoleKey.LeftArrow:  
            playerX--;
            playerX += (speed * -1); 
            break;
		case ConsoleKey.RightArrow: 
            playerX++; 
            playerX += speed;
            break;
		case ConsoleKey.Escape:  
            shouldExit = true; 
            break;
        default:
            if(!nonDirectionalInput)
            {
                shouldExit = true;
            }
            break;
    }

    // Clear the characters at the previous position
    Console.SetCursorPosition(lastX, lastY);
    for (int i = 0; i < player.Length; i++) 
    {
        Console.Write(" ");
    }

    // Keep player position within the bounds of the Terminal window
    playerX = (playerX < 0) ? 0 : (playerX >= width ? width : playerX);
    playerY = (playerY < 0) ? 0 : (playerY >= height ? height : playerY);

    // Draw the player at the new location
    Console.SetCursorPosition(playerX, playerY);
    if (shouldExit) 
    {
        Console.Clear();
        Console.WriteLine("You exit from the program...");
    }
    else{Console.Write(player);}
}

// Clears the console, displays the food and player
void InitializeGame() 
{
    Console.Clear();
    ShowFood();
    Console.SetCursorPosition(0, 0);
    Console.Write(player);
}