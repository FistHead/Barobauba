using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Numerics;



class Enemy
{
    public List<string> faces = new List<string>() {"|о_о|", "|0_0|", "|X_X|", "|◣_◢|","|*_*|","|•v•|","|T_T|"};
    public Level current_level;
    public int final_iq = 0;

    public string get_face(int face_idx)
    {
        return faces[face_idx];
    }

    public void Dialogue(List<string> dialogue, int delay)
    {
        int chat_idx = 1;
        Random rnd = new Random();
        
        Console.WriteLine($"{faces[0]} - {dialogue[0]}");
        while (chat_idx < dialogue.Count)
        {
            int random_face_idx = rnd.Next(faces.Count); 

            Console.Write("> ");
            string user_input = Console.ReadLine();

            if (user_input == current_level.target_word)
            {
                Console.WriteLine("УРА, НАКОНЕЦ-ТО");
                current_level?.finishLevel();
                break;
            }
            else
            {
                Console.WriteLine($"{faces[random_face_idx]} - {dialogue[chat_idx]}");
                chat_idx += 1;
            }
            
            Thread.Sleep(delay);
        }
    }
}   


abstract class Level
{
    public int level_index;
    public string target_word;
    public int attempts;
    public List<string> dialogue = new List<string>();
    public abstract void startLevel();
    public abstract void finishLevel();

    public Level(int level_idx)
    {
        level_index = level_idx;
    }
}

// 1. Реализация конкретного уровня
class LevelOne : Level
{
    private Enemy _enemy;

    public LevelOne(int idx, Enemy enemy) : base(idx)
    {
        _enemy = enemy;
        target_word = "яблоко";
        dialogue.Add("Ты попал в лучшее iq тестирование в мире");
        dialogue.Add("Надеюсь ты не настолько тупой, как те остальные кожаные...");
        dialogue.Add("Короче, я загадываю слово, а ты его должен угадать");
        dialogue.Add("За каждый правильный ответ я буду давать тебе 10 iq, а если не угадаешь, то будет нечто страшное");
    }

    public override void startLevel()
    {
        _enemy.Dialogue(dialogue, 500);
    }

    public override void finishLevel()
    {
        Console.WriteLine(_enemy.get_face(5) + "Афигеть ты умный +10 к IQ");
        _enemy.final_iq += 10;
    }
}


class Stepper
{
    private readonly Enemy _enemy;
    private List<Level> _levels;
    private int _currentLevelIndex = 0;

    public Stepper(Enemy enemy, List<Level> levels)
    { 
        _enemy = enemy;
        _levels = levels;
    }

    public void PlayNextLevel()
    {
        if (_currentLevelIndex < _levels.Count)
        {
            _enemy.current_level = _levels[_currentLevelIndex];
            _enemy.current_level.startLevel();
            _currentLevelIndex++;
        }
    }
}

class Program
{
    static void Main()
    {
        Enemy enemy = new Enemy();
        List<Level> levels = new List<Level> { new LevelOne(1, enemy) };
        Stepper stepper = new Stepper(enemy, levels);

        stepper.PlayNextLevel();
    }
}