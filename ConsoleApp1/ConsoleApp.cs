using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Numerics;
using System.IO;

class Manager
{
    public int iq = 0;
}

class Enemy
{
    private List<string> faces = new List<string>() {"|о_о|", "|0_0|", "|X_X|", "|◣_◢|","|*_*|","|оvо|","|T_T|"};
    public Level current_level;
    public int anger = 0;

    public string get_face(int face_idx)
    {
        return faces[face_idx];
    }

    public void PlayDialogueAndGuess()
    {
        for (int i = 0; i < current_level.dialogue.Count; i++)
        {
            Console.WriteLine($"{get_face(current_level.dialogue[i].face)} - {current_level.dialogue[i].text}");
            
            if (i < current_level.dialogue.Count - 1)
            {
                while (Console.ReadKey(true).Key != ConsoleKey.Spacebar) { }
            }
        }

        Random rnd = new Random();

        while (anger < current_level.attempts)
        {
            Console.Write("> ");
            string user_input = Console.ReadLine();

            if (user_input.Trim().ToLower() == current_level.target_word.ToLower())
            {
                Console.WriteLine("УРА, НАКОНЕЦ-ТО");
                current_level.finishLevel();
                return;
            }
            else
            {
                anger++;
                if (anger >= current_level.attempts)
                {
                    Console.WriteLine($"{get_face(3)} - ТЫ МЕНЯ ДОСТАЛ! ИГРА ОКОНЧЕНА, Я ТЕБЯ УНИЧТОЖАЮ!");
                    Environment.Exit(0);
                }

                if (current_level.wrong_answers.Count > 0)
                {
                    var wrongReplic = current_level.wrong_answers[rnd.Next(current_level.wrong_answers.Count)];
                    Console.WriteLine($"{get_face(wrongReplic.face)} - {wrongReplic.text}");
                }
                else
                {
                    Console.WriteLine($"{get_face(3)} - НЕВЕРНО!");
                }
            }
        }
    }
}

abstract class Level
{
    public int level_index;
    public string target_word;
    public int attempts = 3;
    public string filepath = "";
    public List<(string text, int face)> dialogue = new List<(string text, int face)>();
    public List<(string text, int face)> wrong_answers = new List<(string text, int face)>();

    public abstract void startLevel();
    public abstract void finishLevel();

    public Level(int level_idx, string target_w, int _attempts, string _filepath)
    {
        level_index = level_idx;
        target_word = target_w;
        attempts = _attempts;
        filepath = _filepath;
        dialogue = LoadDialogue(filepath);
    }

    public List<(string text, int face)> LoadDialogue(string path)
    {
        List<(string text, int face)> loaded_dialogue = new List<(string text, int face)>();

        if (File.Exists(path))
        {
            List<string> lines = File.ReadAllLines(path).ToList();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                int lastSpaceIdx = line.LastIndexOf(' ');
                if (lastSpaceIdx != -1 && int.TryParse(line.Substring(lastSpaceIdx + 1), out int faceIdx))
                {
                    string text = line.Substring(0, lastSpaceIdx);
                    loaded_dialogue.Add((text, faceIdx));
                }
                else
                {
                    loaded_dialogue.Add((line, 0));
                }
            }
        }
        else
        {
            loaded_dialogue.Add(("ERROR, ФАЙЛ С ДИАЛОГОМ НЕ БЫЛ НАЙДЕН", 0));
        }

        return loaded_dialogue;
    }
}

class LevelOne : Level
{
    private Enemy _enemy;
    private Manager _manager;

    public LevelOne(int idx, string target_w, int _attempts, string _filepath, Enemy enemy, Manager manager) 
        : base(idx, target_w, _attempts, _filepath)
    {
        _enemy = enemy;
        _manager = manager;
        wrong_answers = new List<(string text, int face)> 
        { 
            ("Так, ладно, подумай еще раз", 3), 
            ("Ахх, ну ты даешь", 6), 
            ("ДА ТЫ ИЗДЕВАЕШЬСЯ!", 3) 
        };
    }

    public override void startLevel()
    {
        _enemy.current_level = this;
        _enemy.PlayDialogueAndGuess();
    }

    public override void finishLevel()
    {
        Console.WriteLine(_enemy.get_face(5) + " Афигеть ты умный +10 к IQ");
        _manager.iq += 10;
    }
}

class Game
{
    private readonly Enemy _enemy;
    private List<Level> _levels;
    private int _currentLevelIndex = 0;

    public Game(Enemy enemy, List<Level> levels)
    { 
        _enemy = enemy;
        _levels = levels;
    }

    public void PlayNextLevel()
    {
        if (_currentLevelIndex < _levels.Count)
        {
            _levels[_currentLevelIndex].startLevel();
            _currentLevelIndex++;
        }
    }
}

class Program
{
    static void Main()
    {
        Manager manager = new Manager();
        Enemy enemy = new Enemy();
        
        List<Level> levels = new List<Level> 
        { 
            new LevelOne(1, "лампочка", 3, "Dialogues/dialogue1.txt", enemy, manager) 
        };
        
        Game Game = new Game(enemy, levels);
        Game.PlayNextLevel();
    }
}