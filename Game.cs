namespace DGD203_BurakBisneli_Midterm;

public class Game 
{
    private bool _isRunning = true;
    private bool _mapGenerated = false;
    private List<Maze.Vector> _map;

    private string? _playerName;

    public void GameTest()
    {
        
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
        Console.WriteLine("X: This is you in the map you can change your  in game.");
        Console.WriteLine("I: This is a item in the map.");
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
        
        
        while (!Maze.itemGiven)
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
            
        }
        Console.WriteLine("---------------------------------");
        Console.WriteLine("Game is over. Thank you for playing!");
        Console.WriteLine("---------------------------------");
        Tools.WaitSeconds(4);
        
    }
}