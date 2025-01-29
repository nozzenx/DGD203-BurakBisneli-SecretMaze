using System.Net.Http.Headers;
using System.Numerics;

namespace DGD203_BurakBisneli_Midterm;


public static class Maze
{
    private static readonly Random Rnd = new Random(); // For generate random numbers.

    public record struct Vector(int X, int Y);

    private static int _width;
    private static int _height;

    private static Vector _mazeBoundary; // Max space of maze can be
    private static List<Vector> _maze = new List<Vector>();
    private static List<Vector> _walls = new List<Vector>();
    private static List<Vector> _neighborCells = new List<Vector>(); // Making a neighbour cells list for getting the cells we can go.
    private static Vector _playerPos;
    
    private static Vector _npcPos;
    private static Vector _item01Pos;
    public static bool _item01Collected = false;
    private static Vector _item02Pos;
    public static bool _item02Collected = false;
    private static Vector _item03Pos;
    public static bool _item03Collected = false;
    public static bool itemsGiven = false;

    private static readonly Vector[] Directions = new Vector[] // I used ChatGPT for suggestions, and it recommends that for more readable code. 
    {
        new Vector(1, 0),
        new Vector(-1, 0),
        new Vector(0, 1),
        new Vector(0, -1),
    };

    public static List<Vector> GenerateBaseRoad(int width, int height)
    {
        _width = width;
        _height = height;
        
        _mazeBoundary = new Vector(_width, _height);
        
        List<Vector> roadStarts = ChooseRandomStartingPoints(_mazeBoundary); // choosing random starting point inside 4 direction starting points.
        int randomStartingRoadIndex = Rnd.Next(roadStarts.Count);
        Vector randomStartingRoad = roadStarts[randomStartingRoadIndex];

        _maze.Add(randomStartingRoad); // Adding starting cell to maze.
        Vector currentRoad = randomStartingRoad; // Making our current pos to starting position
        
        
        while (_maze.Count < _width * _height) // We have width * height cells for total and if our mazes cells count > total cells we cant generate more road cells.
        {
            // Console.WriteLine(currentRoad.X + ", " + currentRoad.Y); 

            foreach (var direction in Directions) // For all possible directions we create a new road and add them to neighbour cells.
            {
                Vector newRoad = new Vector(currentRoad.X + direction.X, currentRoad.Y + direction.Y);
                if (newRoad.X >= 0 && newRoad.X < _mazeBoundary.X && newRoad.Y >= 0 && newRoad.Y < _mazeBoundary.Y &&
                    !_maze.Contains(newRoad) && !_walls.Contains(newRoad))
                {
                    _neighborCells.Add(newRoad);
                }
            }


            if (_neighborCells.Count == 0) // If no possible roads left we break the loop.
            {
                // Console.WriteLine("No neighbors left.");
                break;
            }

            int randomCell = Rnd.Next(_neighborCells.Count); // We choose random cell inside neighbour cells we add up.
            Vector nextCell = _neighborCells[randomCell]; // Making the random cell our next cell.

            _neighborCells.Remove(nextCell);

            foreach (Vector cell in _neighborCells)
            {
                _walls.Add(cell);
            }
            
            _maze.Add(nextCell); // Adding the next cell to our maze road.
            currentRoad = nextCell; // And make our current position to next cell for creating the next road if possible.
            
            _neighborCells.Clear(); // Clearing our neighbour cells because our current cell is different now and our neighbour cells going to change with that.

        }
        // Console.WriteLine($"Maze Last Point Values: X:{_maze.Last().X}, Y:{_maze.Last().Y}");
        // Console.WriteLine($"{width}x{height}");
        // Console.WriteLine(_maze.Count);
        return _maze;
    }

    public static void Print(List<Vector> maze)
    {
        Console.Clear();
        char[,] grid = new char[_width, _height];

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                grid[x, y] = '\u2588';
            }
        }

        foreach (var cell in maze)
        {
            if (cell != _playerPos || cell != _npcPos || cell != _item01Pos)
                grid[cell.X, cell.Y] = ' ';
            
        }
        foreach (var cell in maze)
        {
            if (cell == _playerPos )
                grid[cell.X, cell.Y] = 'X';
            if (cell == _item01Pos && !_item01Collected)
                grid[cell.X, cell.Y] = '?';
            if (cell == _item02Pos && !_item02Collected)
                grid[cell.X, cell.Y] = '?';
            if (cell == _item03Pos && !_item03Collected)
                grid[cell.X, cell.Y] = '?';
            if (cell == _npcPos)
                grid[cell.X, cell.Y] = 'S';
        }
        

        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                Console.Write(grid[x, y] + "  ");
            }
            Console.WriteLine();
        }
    }

    private static List<Vector> ChooseRandomStartingPoints(Vector boundary)
    {
        return
        [
            new Vector(Rnd.Next(boundary.X), 0),
            new Vector(Rnd.Next(boundary.X), boundary.Y-1),
            new Vector(0, Rnd.Next(boundary.Y)),
            new Vector(boundary.X-1, Rnd.Next(boundary.Y)),
        ];
    }
    

    public static List<Vector> GenerateConnectingRoads(List<Vector> maze)
    {
       
            Vector randomPointInMaze = maze[Rnd.Next(maze.Count)];
        Vector currentRoad = randomPointInMaze;
        
        foreach (var direction in Directions) // For all possible directions we create a new road and add them to neighbour cells.
        {
            Vector newRoad = new Vector(currentRoad.X + direction.X, currentRoad.Y + direction.Y);
            if (newRoad.X >= 0 && newRoad.X < _mazeBoundary.X && newRoad.Y >= 0 && newRoad.Y < _mazeBoundary.Y &&
                !_maze.Contains(newRoad) && _walls.Contains(newRoad))
            {
                _walls.Remove(newRoad);
                _neighborCells.Add(newRoad);
            }
            
        }
        currentRoad = _neighborCells[Rnd.Next(_neighborCells.Count)];
        maze.Add(currentRoad);
        _neighborCells.Clear();
        
        
        while (_maze.Count < _width * _height) // We have width * height cells for total and if our mazes cells count > total cells we cant generate more road cells.
        {
            // Console.WriteLine(currentRoad.X + ", " + currentRoad.Y); 

            foreach (var direction in Directions) // For all possible directions we create a new road and add them to neighbour cells.
            {
                Vector newRoad = new Vector(currentRoad.X + direction.X, currentRoad.Y + direction.Y);
                if (newRoad.X >= 0 && newRoad.X < _mazeBoundary.X && newRoad.Y >= 0 && newRoad.Y < _mazeBoundary.Y &&
                    !_maze.Contains(newRoad) && !_walls.Contains(newRoad))
                {
                    _neighborCells.Add(newRoad);
                }
            }


            if (_neighborCells.Count == 0) // If no possible roads left we break the loop.
            {
                // Console.WriteLine("No neighbors left.");
                break;
            }

            int randomCell = Rnd.Next(_neighborCells.Count); // We choose random cell inside neighbour cells we add up.
            Vector nextCell = _neighborCells[randomCell]; // Making the random cell our next cell.

            _neighborCells.Remove(nextCell);

            foreach (Vector cell in _neighborCells)
            {
                _walls.Add(cell);
            }
            
            _maze.Add(nextCell); // Adding the next cell to our maze road.
            currentRoad = nextCell; // And make our current position to next cell for creating the next road if possible.
            
            _neighborCells.Clear(); // Clearing our neighbour cells because our current cell is different now and our neighbour cells going to change with that.

        }
        return _maze;
    }

    public static List<Vector> Generate(int width, int height, int connectingRoads)
    {
        List<Vector> maze = GenerateBaseRoad(width, height);
        for (int i = 0; i < connectingRoads; i++)
            GenerateConnectingRoads(maze);

        UpdatePlayerPos(maze.First());
        GenerateOtherObjects();
        // Console.WriteLine($"player pos: {_playerPos}");
        return maze;
    }

    private static void UpdatePlayerPos(Vector playerPos)
    {
        _playerPos = playerPos;
        _maze.Add(_playerPos);
    }
    
    public static void UpdatePlayerPos(int direction, string? playerName)
    {
        switch (direction)
        {
            case 1:
                if(_maze.Contains(new Vector(_playerPos.X, _playerPos.Y - 1)))
                    _playerPos.Y -= 1;
                else
                {
                    Console.WriteLine($"You can't go that way because there's a wall.");
                }
                break;
            case 2:
                if(_maze.Contains(new Vector(_playerPos.X, _playerPos.Y + 1)))
                    _playerPos.Y += 1;
                else
                {
                    Console.WriteLine($"You can't go that way because there's a wall.");
                }
                break;
            case 3:
                if(_maze.Contains(new Vector(_playerPos.X - 1, _playerPos.Y)))
                    _playerPos.X -= 1;
                else
                {
                    Console.WriteLine($"You can't go that way because there's a wall.");
                }
                break;
            case 4 :
                if(_maze.Contains(new Vector(_playerPos.X + 1, _playerPos.Y)))
                    _playerPos.X += 1;
                else
                {
                    Console.WriteLine($"You can't go that way because there's a wall.");
                }
                break;
        }

        if (_playerPos == _item01Pos && !_item01Collected)
        {
            int answerInput = 0;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("There's a shining stone here. Should I take it?");
                Console.WriteLine("1. Take the shining stone.");
                Console.WriteLine("2. Leave the shining stone.");
                Console.WriteLine("---------------------------------");
                string input = Console.ReadLine()!;
                
                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Please enter a valid input.");
                else
                {
                    try
                    {
                        answerInput = Convert.ToInt32(input);
                        isValidInput = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
            }

            switch (answerInput)
            {
                case 1:
                    _item01Collected = true;
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You got shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    
                    break;
                case 2:
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You leaved the shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
        
        
        if (_playerPos == _item02Pos && !_item02Collected)
        {
            int answerInput = 0;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("There's a shining stone here. Should I take it?");
                Console.WriteLine("1. Take the shining stone.");
                Console.WriteLine("2. Leave the shining stone.");
                Console.WriteLine("---------------------------------");
                string input = Console.ReadLine()!;
                
                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Please enter a valid input.");
                else
                {
                    try
                    {
                        answerInput = Convert.ToInt32(input);
                        isValidInput = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
            }

            switch (answerInput)
            {
                case 1:
                    _item02Collected = true;
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You got shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    
                    break;
                case 2:
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You leaved the shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
        
        
        
        if (_playerPos == _item03Pos && !_item03Collected)
        {
            int answerInput = 0;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("There's a shining stone here. Should I take it?");
                Console.WriteLine("1. Take the shining stone.");
                Console.WriteLine("2. Leave the shining stone.");
                Console.WriteLine("---------------------------------");
                string input = Console.ReadLine()!;
                
                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Please enter a valid input.");
                else
                {
                    try
                    {
                        answerInput = Convert.ToInt32(input);
                        isValidInput = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
            }

            switch (answerInput)
            {
                case 1:
                    _item03Collected = true;
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You got shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    
                    break;
                case 2:
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine($"You leaved the shining stone.");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("Press enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
        
        

        if (_playerPos == _npcPos)
        {
            int answerInput = 0;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("---------------------------------");
                Console.WriteLine("There's a sorcerer here. Should I talk with him?");
                Console.WriteLine("1. Talk with sorcerer.");
                Console.WriteLine("2. Leave the sorcerer.");
                Console.WriteLine("---------------------------------");
                string input = Console.ReadLine()!;
                
                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Please enter a valid input.");
                else
                {
                    try
                    {
                        answerInput = Convert.ToInt32(input);
                        isValidInput = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
            }

            switch (answerInput)
            {
                case 1:
                    
                    int sorcererAnswerInput = 0;
                    bool sorcererTalkLoop = false;
                    while (!sorcererTalkLoop)
                    {
                        Console.WriteLine("---------------------------------");
                        Console.WriteLine($"{playerName}: Hello, can i ask something?");
                        Console.WriteLine($"Sorcerer: Hello young man, you can.");
                        Console.WriteLine($"1. Where are we? I was sleeping in my bed and suddenly I found myself here.");
                        Console.WriteLine($"2. What are you doing here?");
                        Console.WriteLine($"3. Are you a sorcerer?");
                        Console.WriteLine($"4. Leave.");
                        if(_item01Collected && _item02Collected && _item03Collected)
                            Console.WriteLine($"5. Give the stones.");
                        Console.WriteLine("---------------------------------");
                        string input = Console.ReadLine()!;
                
                        if (string.IsNullOrEmpty(input))
                            Console.WriteLine("Please enter a valid input.");
                        else
                        {
                            try
                            {
                                sorcererAnswerInput = Convert.ToInt32(input);
                                switch (sorcererAnswerInput)
                                {
                                    case 1:
                                        Console.WriteLine($"Sorcerer: We are in the secret labyrinth right now.");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"{playerName}: Okay, but why am i here?");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"Sorcerer: You gotta be under the influence of a dark magic. You're asleep right now and you can't wake up until you get out of here.");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"{playerName}: How can i leave here.");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"Sorcerer: I can teleport you to the exit but you need to help me first.");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"{playerName}: What kind of help?");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"Sorcerer: Find my 3 power stones in the labyrinth and bring it to me and i can help you get out of here.");
                                        Tools.WaitSeconds(2);
                                        Console.WriteLine($"{playerName}: Okay, i will find and get it to you.");
                                        
                                        Console.WriteLine("Press enter to continue...");
                                        Console.ReadLine();
                                        break;
                                    case 2:
                                        Console.WriteLine($"Sorcerer: I'm relaxing a bit. I will be leaving soon.");
                                        
                                        Console.WriteLine("Press enter to continue...");
                                        Console.ReadLine();
                                        break;
                                    case 3:
                                        Console.WriteLine($"Sorcerer: Dont you see my staff. Of course im a sorcerer. ");
                                        Console.WriteLine("Press enter to continue...");
                                        Console.ReadLine();
                                        break;
                                    case 4:
                                        Console.WriteLine($"I need to leave now.");
                                        sorcererTalkLoop = true;
                                        break;
                                    case 5:
                                        if (_item01Collected && _item02Collected && _item03Collected)
                                        {
                                            Console.WriteLine($"{playerName}: I found your stones.");
                                            Tools.WaitSeconds(2);
                                            Console.WriteLine($"Sorcerer: Thank you, young man I will teleport you to the exit now.");
                                            Tools.WaitSeconds(2);
                                            Console.WriteLine($"{playerName}: Good to see you mr sorcerer.");
                                            Console.WriteLine("Press enter to continue...");
                                            Console.ReadLine();
                                            itemsGiven = true;
                                            sorcererTalkLoop = true;
                                            break;   
                                        }
                                        break;
                                }
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Please enter a valid input.");
                            }
                        }
                    }
                    
                    break;
                case 2:
                    break;
            }
        }
    }

    private static void GenerateOtherObjects()
    {
        _npcPos = _maze[Rnd.Next(_maze.Count)];
        _item01Pos = _maze[Rnd.Next(_maze.Count)];
        _item02Pos = _maze[Rnd.Next(_maze.Count)];
        _item03Pos = _maze[Rnd.Next(_maze.Count)];
        _maze.Add(_npcPos);
        _maze.Add(_item01Pos);
    }
}