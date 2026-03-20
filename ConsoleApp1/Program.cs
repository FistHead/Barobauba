using System;
using System.Collections.Generic;
using System.Linq;


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

class Map : Table
{
    private List<Block> blocks = new List<Block>();

    public Map(int rows, int columns) : base(rows, columns)
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        List<string> block_types = new List<string>() {"[F]","[M]","[B]","[!]","[?]"};
        Random random = new Random();

        for (int i = 0; i < Rows * Columns / 4; i++)
        {
            Block block = new Block();
            block.posX = random.Next(Columns);
            block.posY = random.Next(Rows);

            blocks.Add(block);
            
            Cells[(block.posY, block.posX)] = "[F]";
        }
    }


    public void DrawMap()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                Console.Write(Cells[(r, c)] + " ");
            }
            Console.WriteLine();
        }
    }

    public List<Block> GetBlocks() { return blocks; }
}

class Unit
{
    public int hp;
    public int stamina;
    public int speed;
}

class Building
{

}

class Button
{
    public string command;
    public string name;
}

class Stepper
{

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
