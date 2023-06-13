using CRUD;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class VideoGameCatalogue
{
    private static ConsoleKey keyInput;

    private static StringBuilder[] table;

    private static List<CRUD.Type> typesList = Enum.GetValues(typeof(CRUD.Type)).Cast<CRUD.Type>().ToList();
    private static Dictionary<int, VideoGame> dictVideoGames = new Dictionary<int, VideoGame>();

    private static int[] selection;

    private static int currentPlacement;
    private static void Main(string[] args)
    {
        CRUDLogic();
    }

    private static void CRUDLogic()
    {
        currentPlacement = 0;
        while (true)
        {
            table = CreateTableList(dictVideoGames.Count);
            DisplayTable();
            Console.WriteLine("|+|\t:\tAjouter\n\n|▲||▼|\t:\tNavigation\n|DEL|\t:\tSupprimer\n|ENTER|\t:\tModifier\n\n|ESC|\t:\tQuitter");
            keyInput = Console.ReadKey(true).Key;
            switch (keyInput)
            {
                case ConsoleKey.Add:
                    Console.Clear();
                    currentPlacement = 1;
                    AddNewVideoGame();
                    currentPlacement = 0;
                    Console.Clear();
                    break;
                case ConsoleKey.DownArrow:
                    if (currentPlacement < dictVideoGames.Count - 1)
                    {
                        currentPlacement++;
                    }
                    Console.Clear();
                    break;
                case ConsoleKey.UpArrow:
                    if (currentPlacement > 0)
                    {
                        currentPlacement--;
                    }
                    Console.Clear();
                    break;
                case ConsoleKey.Enter:
                    Console.Clear();
                    //UpdateGame();
                    dictVideoGames.Remove(currentPlacement);
                    AddNewVideoGame(currentPlacement);
                    currentPlacement = 0;
                    Console.Clear();
                    break;
                case ConsoleKey.Delete:
                    dictVideoGames.Remove(currentPlacement);
                    currentPlacement = 0;
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

    private static void AddNewVideoGame(int id = 0)
    {
        int count = 1;
        selection = new int[typesList.Count];
        string name = ReadString("Entrer un nom >> ");
        Console.Clear();
        string studio = ReadString("Entrer le nom du studio >> ");
        Console.Clear();
        int year = ReadInt("Entrer l'année de parution >> ");

        for (int increment = 1; increment < selection.Length; increment++)
        {
            selection[increment] = count;
            count *= 2;
        }
        int typesValue = SelectionTypesLogic(selection);
        VideoGame newGame = new VideoGame(name, studio, typesValue, year);
        if (id == 0)
        {
            dictVideoGames.Add(newGame.ID - 1, newGame);
        }
        else
        {
            dictVideoGames.Add(id, newGame);
        }

    }

    private static int SelectionTypesLogic(int[] selectionTypes)
    {
        bool confirm = false;
        CRUD.Type flags;
        int typesValue = 0;
        List<int> selectionedTypes = new List<int>();
        while (!confirm)
        {
            Console.Clear();
            Console.WriteLine("|ENTER|\t:\tAjouter/Supprimer\n|▲||▼|\t:\tNavigation\n|SPACE|\t:\tConfirmer\n\n");
            flags = (CRUD.Type)typesValue;
            //Console.WriteLine(currentPlacement + " | " + typesValue);
            for (int increment = 1; increment < selectionTypes.Length; increment++)
            {
                if (increment == currentPlacement)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                Console.WriteLine(typesList[increment]);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
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
                    if (currentPlacement < selectionTypes.Length)
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
        return typesValue;
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
            if (increment == currentPlacement)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;
            }
            Console.WriteLine($"{table[increment]}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.Write(msg);
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