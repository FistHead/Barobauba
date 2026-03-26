using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Media;
using System.Numerics;



class Enemy
{
    public List<string> faces = new List<string>() {"|•_•|", "|0_0|", "|X_X|", "|◣_◢|","|*_*|","|•v•|","|T_T|"};
    public Level current_level;
    public int final_iq = 0;

    public string get_face(int face_idx)
    {
        return faces[face_idx];
    }

    public string answer_dialogue(List<string> dialogue, int index)
    {
        return dialogue[index];
    }

    public void Chatter(string user_message, bool guess)
    {
        if(user_message == ' ')
        {
            answer_dialogue(current_level.dialogue, current_level.dialogue_index);
            current_level.dialogue_index += 1;
        }
    }

    public string Guess()
    {
        input = Console.ReadLine();
        return input;
    }

    public void Talk(bool guess, SoundPlayer sound, float delay)
    {
        sound.Play();
        Thread.Sleep(delay);
        if (guess)
        {
            input = Console.ReadLine();
            return input;
        }
        input = Console.ReadLine();
        return input;
    }
}   



abstract class Level
{
    public int level_index;
    public string target_word;
    public int attempts;
    public List<string> dialogue;
    public int dialogue_index = 0;
    public abstract void startLevel();
    public abstract void finishLevel();

    public Level(int level_idx)
    {
        level_index = level_idx;
    }
}

// class FirstLevel: Level
// {
//     private readonly string _target_word;
//     private string _maxAttempts;
//     private int current_attempt;
//     public List<string> dialogue = new List<string> {"Итак, я загадал предмет, который очень похож на грушу, но ее нельзя скушать","эхх, это что? ТАК СЛОЖНО УГАДАТЬ 1 СЛОВО?!!?!?!?!"};


//     public FirstLevel(string target, int maxAtt, List<string> dg) : base(0)
//     {
//         _target_word = target.ToLowerInvariant();
//         _maxAttempts = maxAtt;
//         dialogue = dg;
        
//     }
//     public int AttemptsLeft => _maxAttempts - current_attempt;

//     public override void StartLevel()
//     {
//         Console.WriteLine(dialogue[0]);
//     }

//     public override void finishLevel()
//     {
//         if (Completed)
//             Console.WriteLine("Поздравляю! ТЫ, проходишь дальше...");
//         else
//             Console.WriteLine(dialogue[-1]);
//     }
// }




class Stepper
{
    private readonly Enemy _enemy;
    private List<Level> _levels;

    public Stepper(Enemy enemy, Level levels)
    { 
        _enemy = enemy;
        _levels = levels;
    }


}
// class Program
// {
//     static void Main()
//     {
//         var enemy = new Enemy();
//         var stepper = new Stepper(enemy);
//         stepper.DrawStep();
//     }
// }