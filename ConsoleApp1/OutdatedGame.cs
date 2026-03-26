using System;
using System.Collections.Generic;
using System.Linq;


class Djebb
{
    public List<string> faces = new List<string>() {"|•_•|", "|0_0|", "|X_X|", "|◣_◢|","|*_*|","|•v•|","|T_T|"};
}


abstract class Table
{
    public int Rows { get; }
    public int Columns { get; }

    public Dictionary<(int r, int c), string> Cells = new Dictionary<(int, int), string>();
    
    public Table(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
        Initialize();
    }

    public void Initialize()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                Cells[(r, c)] = "[ ]";
            }
        }
    }

    public string DrawCellsData()
    {
        string data = "";

        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                data += $"Row: {r}, Column: {c}, Data inside: {Cells[(r, c)]}\n";
            }
        }
        return data;
    }
}

class Block
{
    public int posX = 0;
    public int posY = 0;
    public bool isCaptured = false;
}


class CommandManager
{
    public List<string> commands = new List<string>() { "FOCUS" };
}

class Program
{
    static void Main()
    {
        // Table testTable = new Table(4, 4);
        // Console.WriteLine(testTable.DrawCellsData());

        Map map = new Map(3,3);
        map.DrawMap();

    }
}

class Map : Table
{
    private List<Block> blocks = new List<Block>();
    private int pl_base = 0;
    string[] blockTypes = {"| B |","|YOU|",
     "| Ψ |", "| _ |", "| Δ |", "| ≈ |", "| ! |", "| ? |"}; // виды блоков

    // вероятности блоков
    double[] probabilities = { 
        0, // блоки базы
        0, // блоки игрока
        0.3,  // лес (30%)
        0.3,  // поле (15%)
        0.2,  // горы (15%)
        0.2, // вода (20%)
        0.05, // внимание/атака (5%)
        0.1   // неизвестно (15%)
    };

    public Map(int rows, int columns) : base(rows, columns)
    {
        GenerateWorld();
    }

    public string SelectBlockType(Random random)
    {
        double r = random.NextDouble();
        double cumulativeProbability = 0.0;
        
        for (int i = 0; i < probabilities.Length; i++)
        {
            cumulativeProbability += probabilities[i];
            if (r <= cumulativeProbability)
                return blockTypes[i];
        }
        return blockTypes[3];
    }

    public void GenerateWorld()
    {   
        Random random = new Random();
        for (int r = 0; r < Rows; r++) //rows
        {
            for (int c = 0; c < Columns; c++) //columns
            {
                Block block = new Block { posX = c, posY = r };
                string selectedType = SelectBlockType(random);

                blocks.Add(block);
                Cells[(r, c)] = selectedType;
                
            }
        }

        if (pl_base == 0)
        {
            int randomRow = random.Next(Rows);
            int randomCol = random.Next(Columns);

            Cells[(randomRow, randomCol)] = $"|{blockTypes[0][2]}{Cells[(randomRow, randomCol)][2]} |";
            pl_base++;
        }
        
    }


    public void DrawMap()
    {
        for (int r = 0; r < Rows; r++)
        {
            Console.Write(r + " ");
            for (int c = 0; c < Columns; c++)
            {
                Console.Write(Cells[(r, c)]);
            }
            Console.WriteLine();
        }
    }

    public List<Block> GetBlocks() { return blocks; }
}