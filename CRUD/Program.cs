using CRUD;
using System.Collections;
using System.ComponentModel;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class VideoGameCatalogue
{
    private static ConsoleKey keyInput;

    private static List<CRUD.Type> typesList = Enum.GetValues(typeof(CRUD.Type)).Cast<CRUD.Type>().ToList();

    private static Dictionary<int, VideoGame> dictVideoGames = new Dictionary<int, VideoGame>();

    private static StringBuilder[] table;

    private static int currentPlacement;
    private static void Main(string[] args)
    {

        while (true)
        {
            table = CreateTableList(15);
            DisplayTable();
            Console.WriteLine("|+|\t:\tAjouter\n\n|▲||▼|\t:\tNavigation\n|DEL|\t:\tSupprimer\n|INSERT|:\tModifier\n|ENTER|\t:\tDétails\n\n|ESC|\t:\tQuitter");
            keyInput = Console.ReadKey(true).Key;
            switch (keyInput)
            {
                case ConsoleKey.Add:
                    Console.Clear();
                    currentPlacement = 1;
                    AddNewVideoGame(ref currentPlacement);
                    Console.Clear();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    break;
                case ConsoleKey.DownArrow:
                    Console.Clear();
                    break;
                case ConsoleKey.UpArrow:
                    Console.Clear();
                    break;
                case ConsoleKey.RightArrow:
                    Console.Clear();
                    break;
                case ConsoleKey.LeftArrow:
                    Console.Clear();
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Keyboard Error try again !");
                    break;
            }
        }
    }

    private static void AddNewVideoGame(ref int currentPlacement)
    {
        bool confirm = false;
        int count = 1;
        int[] selectionTypes = new int[typesList.Count];
        List<int> selectionedTypes = new List<int>();
        int typesValue = 0;
        CRUD.Type flags;
        string name = ReadString("Entrer un nom >> ");
        string studio = ReadString("Entrer le nom du studio >> ");
        int year = ReadInt("Entrer l'année de parution >> ");

        for (int increment = 1; increment < selectionTypes.Length; increment++)
        {
            selectionTypes[increment] = count;
            count *= 2;
        }

        while (!confirm)
        {
            flags = (CRUD.Type)typesValue;
            //Console.WriteLine(currentPlacement + " | " + typesValue);
            for (int increment = 1; increment < selectionTypes.Length; increment++)
            {
                if (increment == currentPlacement)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                Console.WriteLine(typesList[increment]);
            }
            Console.WriteLine();
            Console.Write("Type sélectionné : ");
            DisplaySelectionned(flags);

            keyInput = Console.ReadKey(true).Key;
            switch (keyInput)
            {
                case ConsoleKey.Spacebar:
                    confirm = true;
                    break;
                case ConsoleKey.Enter:
                    if (!(IsSelectioned(selectionTypes[currentPlacement], selectionedTypes)))
                    {
                        typesValue += selectionTypes[currentPlacement];
                        selectionedTypes.Add(selectionTypes[currentPlacement]);
                    }
                    else
                    {
                        typesValue -= selectionTypes[currentPlacement];
                        selectionedTypes.Remove(selectionTypes[currentPlacement]);
                    }
                    Console.Clear();
                    break;
                case ConsoleKey.DownArrow:
                    if (currentPlacement <  selectionTypes.Length)
                    {
                        currentPlacement++;
                    }
                    Console.Clear();
                    break;
                case ConsoleKey.UpArrow:
                    if (currentPlacement > 1)
                    {
                        currentPlacement--;
                    }
                    Console.Clear();
                    break;
                default:
                    break;
            }
        }
        VideoGame newGame = new VideoGame(name, studio, typesValue, year);
        dictVideoGames.Add(newGame.ID - 1, newGame);

    }

    private static void DisplaySelectionned(CRUD.Type flags)
    {
        for (int increment = 1; increment < typesList.Count; increment++)
        {
            if (flags.HasFlag(typesList[increment]))
            {
                Console.Write($"{typesList[increment]}, ");
            }
        }
    }

    private static bool IsSelectioned(int number, List<int> tab)
    {
        for (int increment = 0; increment < tab.Count; increment++)
        {
            if (tab[increment].Equals(number))
            {
                return true;
            }
        }
        return false;
    }

    private static void DisplayTable()
    {
        for (int increment = 0; increment < table.Length; increment++)
        {
            Console.WriteLine($"{table[increment]}");
        }
    }

    private static StringBuilder[] CreateTableList(int size)
    {
        StringBuilder[] table = new StringBuilder[size];
        for (int increment = 0; increment < dictVideoGames.Count; increment++)
        {
            table[increment] = CreateRow(dictVideoGames.GetValueOrDefault(increment));
        }
        return table;
    }
    private static StringBuilder CreateRow(VideoGame game)
    {
        int nameNumber = CountLetters(game.Name);
        int typesNumber = CountLetters(game.Types.ToString());
        int studioNumber = CountLetters(game.Studio);
        StringBuilder row = new StringBuilder();
        row.Append(game);
        row.Append(' ', (CountMaxNameLetters() - nameNumber) / 2);
        row.Append(game.Name);
        row.Append(' ', (CountMaxNameLetters() - nameNumber) / 2);
        row.Append(" | ");
        row.Append(' ', (CountMaxTypesLetters() - typesNumber) / 2);
        row.Append(game.Types);
        row.Append(' ', (CountMaxTypesLetters() - typesNumber) / 2);
        row.Append(" | ");
        row.Append(' ', (CountMaxStudioLetters() - studioNumber) / 2);
        row.Append(game.Studio);
        row.Append(' ', (CountMaxStudioLetters() - studioNumber) / 2);
        return row;
    }
    private static int CountLetters(string text)
    {
        return text.Length;
    }
    private static int CountMaxNameLetters()
    {
        string max;
        max = dictVideoGames[0].Name;
        for (int increment = 1; increment < dictVideoGames.Count; increment++)
        {
            if (max.Length < dictVideoGames[increment].Name.Length)
            {
                max = dictVideoGames[increment].Name;
            }
        }
        return CountLetters(max);
    }
    private static int CountMaxTypesLetters()
    {
        string max;
        max = dictVideoGames[0].Types.ToString();
        for (int increment = 1; increment < dictVideoGames.Count; increment++)
        {
            if (max.Length < dictVideoGames[increment].Types.ToString().Length)
            {
                max = dictVideoGames[increment].Types.ToString();
            }
        }
        return CountLetters(max);
    }
    private static int CountMaxStudioLetters()
    {
        string max;
        max = dictVideoGames[0].Studio;
        for (int increment = 1; increment < dictVideoGames.Count; increment++)
        {
            if (max.Length < dictVideoGames[increment].Studio.Length)
            {
                max = dictVideoGames[increment].Studio;
            }
        }
        return CountLetters(max);
    }

    private static int ReadInt(string msg = "Entrer un nombre >> ")
    {
        int number;
        do
        {
            Console.WriteLine(msg);
        } while(!(int.TryParse(Console.ReadLine(), out number)));
        return number;
    }
    private static string ReadString(string msg = "Entrer un texte >> ")
    {
        string text = "";
        Console.Write(msg);
        text = Console.ReadLine();
        return text;
    }
}