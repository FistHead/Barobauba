using System.Collections.Generic;
using System.Linq;

class Table
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
        for (int r = 0; i < Rows; r++)
        {
            for (int c = 0; i < Columns; c++)
            {
                Cells[(r, c)] = "NONE";
            }
        }
    }

    public void DrawAllCells()
    {
        string data = "";

        for (int r = 0; i < Rows; r++)
        {
            for (int c = 0; i < Columns; c++)
            {
                data += $"Row: {r}, Column: {c}, Data inside: {Cells[(r,c)]} ";
            }
        }
    }

}

class Map
{

}

class Block
{
    
}

class Button
{
    
}

class Stepper
{
    
}

class Program 
{
    static void Main() 
    {
        Table testTable = new Table(3, 3);
        Console.WriteLine(testTable.DrawAllCells());
    }
}