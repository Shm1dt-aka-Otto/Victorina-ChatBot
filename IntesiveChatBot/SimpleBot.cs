using System;
using System.IO;
using System.Linq;

namespace IntesiveChatBot
{
    class SimpleBot
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data.txt");
            var questions = lines
                .Select(line => line.Split('|'))
                .Select(line => (line[0], line[1]))
                .ToList();
            var random = new Random();
            var count = questions.Count;
            var score = 0;
            while (true)
            {
                var index = random.Next(count - 1);
                var question = questions[index];

                var opened = 0;
                while (opened < question.Item2.Length)
                {
                    Console.WriteLine($"{question.Item1}: {question.Item2.Length} букв");
                    var answer = question.Item2
                        .Substring(0, opened)
                        .PadRight(question.Item2.Length, '_');
                    Console.WriteLine(answer);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var tryAnswer = Console.ReadLine().ToLower().Replace('ё', 'е');
                    if (tryAnswer == question.Item2)
                    {
                        score++;
                        Console.ForegroundColor = ConsoleColor.Green;   
                        Console.WriteLine("Правильно!");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"У вас {score} очков");
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неправильно :(");
                        opened++; 
                    }
                    Console.ResetColor();
                }

                if (opened == question.Item2.Length)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Никто не отгадал! Это было - {question.Item2}");
                    Console.ResetColor();
                }
            }
        }
    }
}
