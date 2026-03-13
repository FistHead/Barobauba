using System.Collections.Generic;
using System.Linq;
using System;

abstract class Table
{
    public int Rows {get;}
    public int Columns {get;}

    public Dictionary<(int r, int c), string> Cells  = new Dictionary<(int, int), string>();
    
    public Table(int rows,int columns)
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
                Cells[(r, c)] = "NONE";
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
                data += $"Row: {r}, Column: {c}, Data inside: {Cells[(r,c)]}\n";
            }
        }
        return data;
    }

}

class Block
{
    private int posX = 0;
    private int posY = 0;
}
class Map: Table
{  
    private List<Block> blocks = new List<Block>();
    public Map(int rows, int columns) : base(rows, columns)
    {
        
    }
}
class Unit
{
    private int hp;
    private int stamina;

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
    public List<string> commands = new List<string> {"FOCUS"};
}

class Program 
{
    static void Main() 
    {
        // Table testTable = new Table(4, 4);
        // Console.WriteLine(testTable.DrawCellsData());
        Map map = new Map(4,4);
    }
}