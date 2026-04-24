using System;
using System.Collections.Generic;
using System.Threading;
using System.Media;

//--------------------------------------------------------
abstract class Action
{
    public string Name;
    public string Description;
    public abstract void Execute();

    public Action(string name, string desc)
    {
        Name = name;
        Description = desc;
    }
}
//--------------------------------------------------------
abstract class GameObject
{
    public string Name;
    public string Description;
    public Action Action;

    public GameObject(string name, string desc, Action action)
    {
        Name = name;
        Description = desc;
        Action = action;
    }
}
//--------------------------------------------------------
class States
{
    public int LevelIndex = 0;
    public int Iq = 0;
    public bool HasScrewdriver = false;
    public bool IsFree = false;
    public Room CurrentRoom = null!;
}
//--------------------------------------------------------
class SimpleObject : GameObject
{
    public SimpleObject(string name, string desc, Action action) : base(name, desc, action) { }
}

class Look : Action
{
    private States _state;

    public Look(States state) : base("Осмотреться", "Изучить окружение")
    {
        _state = state;
    }

    public override void Execute()
    {
        Room room = _state.CurrentRoom;
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"--- {room.Name} ---");
        Console.WriteLine(room.Description);

        if (room.Objects.Count > 0)
        {
            Console.WriteLine("\nВы видите:");
            for (int i = 0; i < room.Objects.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {room.Objects[i].Name}: {room.Objects[i].Description}");
            }
        }
        else
        {
            Console.WriteLine("\nЗдесь нет ничего примечательного.");
        }
        Console.ResetColor();
        Console.WriteLine();
    }
}

class VentilationAction : Action
{
    private Action nextAction;
    public override void Execute()
    {
        Console.WriteLine("Вентиляция открывается со скрипом...");
        nextAction.Execute();
    }

    public VentilationAction(Action action) : base("Вентиляция", "Открыть решетку")
    {
        nextAction = action;
    }
}

class Dialogue : Action
{
    public List<string> replics;
    public float DelayPerChar = 0.03f;

    public Dialogue(string npcName, List<string> lines) : base("Говорить", $"Диалог с {npcName}")
    {
        replics = lines;
    }

    public override void Execute()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var line in replics)
        {
            foreach (char c in line)
            {
                Console.Write(c);
                Thread.Sleep((int)(DelayPerChar * 100));
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }
}

class MoveAction : Action
{
    private Room _target;
    private States _state;
    public MoveAction(Room target, States state) : base("Идти", $"Перейти в {target.Name}")
    {
        _target = target;
        _state = state;
    }
    public override void Execute()
    {
        Console.WriteLine($"Вы переходите в {_target.Name}....");
        _state.CurrentRoom = _target;
        Thread.Sleep(1000);
    }
}

class Room
{
    public string Name;
    public string Description;
    public List<GameObject> Objects = new List<GameObject>();

    public Room(string name, string desc)
    {
        Name = name;
        Description = desc;
    }
}

class EscapeAction : Action
{
    private States _state;
    public EscapeAction(States state) : base("Сбежать", "Вылезти через вентиляцию") { _state = state; }

    public override void Execute()
    {
        if (_state.HasScrewdriver)
        {
            Console.WriteLine("Вы открутили решетку отверткой и выбрались на свободу!");
            _state.IsFree = true;
        }
        else
        {
            Console.WriteLine("Решетка намертво прикручена. Нужно чем-то её поддеть...");
        }
    }
}

class BookAction : Action 
{
    private States _s;
    public BookAction(States s) : base("Обыскать", "Искать что-то полезное") { _s = s; }
    public override void Execute() 
    {
        if (!_s.HasScrewdriver)
        {
            Console.WriteLine("\n[!] Вы нашли старую, ржавую отвертку!");
            _s.HasScrewdriver = true;
        }
        else Console.WriteLine("\nЗдесь больше нет ничего полезного.");
    }
}

#region Игра
class Program
{
    static void Main()
    {
        States game = new States();

        Room cell = new Room("Тюремная камера", "Сырая и темная комната. На стене висит плакат.");
        Room corridor = new Room("Главный коридор", "Длинный пролет. Слышны шаги охранников.");

        var borisDialogue = new Dialogue("Борис", new List<string> { 
            "Псс, парень! Хочешь выйти отсюда?", 
            "В коридоре есть вентиляция.",
            "Поищи отвертку в ящиках." 
        });

        cell.Objects.Add(new SimpleObject("Сокамерник Борис", "Старый зэк", borisDialogue));
        cell.Objects.Add(new SimpleObject("Дверь", "Путь в коридор", new MoveAction(corridor, game)));

        corridor.Objects.Add(new SimpleObject("Ящик", "Старый железный ящик", new BookAction(game)));
        corridor.Objects.Add(new SimpleObject("Вентиляция", "Решетка на стене", new EscapeAction(game)));
        corridor.Objects.Add(new SimpleObject("Назад", "Вернуться в камеру", new MoveAction(cell, game)));

        game.CurrentRoom = cell;
        
        while (!game.IsFree)
        {
            new Look(game).Execute();
            
            Console.WriteLine("\nНапиши номер объекта (или 0/enter для выхода):");
            Console.Write("> ");
            string? input = Console.ReadLine();
            
            if (int.TryParse(input, out int index) && index > 0 && index <= game.CurrentRoom.Objects.Count)
            {
                // Вызываем действие объекта
                game.CurrentRoom.Objects[index - 1].Action.Execute();
                Console.WriteLine("\nНажми любую клавишу...");
                Console.ReadKey();
            }
            else if (index == 0) break;
        }

        if (game.IsFree)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("ПОЗДРАВЛЯЮ! Вы НА СВОБОДЕ!");
            Console.ResetColor();

            SoundPlayer sp = new SoundPlayer(@"sounds/kids-saying-yay-sound-effect_3.wav");
            sp.Play();
        }
    }
}
#endregion