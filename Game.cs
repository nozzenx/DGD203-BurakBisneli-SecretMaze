namespace DGD203_BurakBisneli_Midterm;

public class Game 
{
    private bool _isRunning = true;
    private bool _mapGenerated = false;
    private List<Maze.Vector> _map;
    private bool gameEnded = false;

    private string? _playerName;

    public void GameTest()
    {
        
        while (!gameEnded)
        {
            int answerInput = 0;
            bool isValidInput = false;
            while (!isValidInput)
            {
                Console.WriteLine("----------SECRET LABYRINTH----------");
                Console.WriteLine("");
                Console.WriteLine("---------------------------------");
                Console.WriteLine("1. New Game");
                Console.WriteLine("2. Credits");
                Console.WriteLine("3. Exit");
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
                    _mapGenerated = false;
                    Console.Clear();
                    MainGameLoop();
                    break;
                case 2:
                    Credits();
                    break;
                case 3:
                    gameEnded = true;
                    break;
            }
            
        }
        
        
        
        
       
    }

    private void MainGameLoop()
    {
        Maze.itemsGiven = false;
        Maze._item01Collected = false;
        Maze._item02Collected = false;
        Maze._item03Collected = false;
        
        
        while (!_mapGenerated) // I use ChatGPT for this my code giving error sometimes and I want to avoid that and try again. But I think it's still broken sometimes.
        {
            try
            {
                _map = Maze.Generate(20, 20, 5);
                
                _mapGenerated = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Hello, welcome to the secret labyrinth game!");
        Console.WriteLine("This game have random generated labyrinths but it's not working sometimes.");
        Console.WriteLine("I will make you to see map now!");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Press enter to continue...");
        Console.ReadLine();
        Tools.LoadingScreen(15);
        Maze.Print(_map);
        Console.WriteLine("---------------------------------");
        Console.WriteLine("!Map needs to CONTAIN!");
        Console.WriteLine("\u2588: This is the wall of the labyrinth.");
        Console.WriteLine("X: This is you in the map you can change your position in game.");
        Console.WriteLine("?: This is a secret thing in the map (It needs to be 3 of them.).");
        Console.WriteLine("S: This is an npc in the map.");
        Console.WriteLine("!!IF YOU DONT SEE SOME OF THEM OR MAP LOOKS WEIRD (Or if you dont like the shape of the roads) PLEASE RESTART THE GAME NOW!!");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("If you see them please press enter your NAME to start the game. Have fun.");
        _playerName = Console.ReadLine()!.ToUpper();
        Console.Clear();
        Tools.LoadingScreen(15);
        Console.WriteLine("Oh, where i am.");
        Tools.WaitSeconds(2);
        Console.WriteLine("My head hurts.");
        Tools.WaitSeconds(2);
        Console.WriteLine("There's big walls.");
        Tools.WaitSeconds(2);
        Console.WriteLine("I need to find a way out of here.");
        Tools.WaitSeconds(2);
        
        
        while (!Maze.itemsGiven)
        {
            int directionInput = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Clear();
                Maze.Print(_map);
                Console.WriteLine("Please enter the positions number you want to move:");
                Console.WriteLine("1. Up ");
                Console.WriteLine("2. Down ");
                Console.WriteLine("3. Left ");
                Console.WriteLine("4. Right ");
                string input = Console.ReadLine()!;
                
                if (string.IsNullOrEmpty(input))
                    Console.WriteLine("Please enter a valid input.");
                else
                {
                    try
                    {
                        directionInput = Convert.ToInt32(input);
                        isValidInput = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
            }
            Maze.UpdatePlayerPos(directionInput, _playerName);
            Console.Clear();
            
        }
        Tools.WaitSeconds(2);
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Game is over. Thank you for playing!");
        Console.WriteLine("---------------------------------");
        Tools.WaitSeconds(3);
        Console.Clear();
        Credits();
    }
    
    private void Credits()
    {
        Console.WriteLine("CREDITS");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Game Design");
        Console.WriteLine("Burak Bisneli");
        Console.WriteLine(" ");
        Console.WriteLine("Programming");
        Console.WriteLine("Burak Bisneli");
        Console.WriteLine("ChatGPT");
        Console.WriteLine("ClaudeAI");
        Console.WriteLine(" ");
        Console.WriteLine("Story");
        Console.WriteLine("Burak Bisneli");
        Tools.WaitSeconds(5);
    }
}