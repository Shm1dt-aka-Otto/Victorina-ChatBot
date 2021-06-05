using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QuizLogic
{
    public class Quiz
    {
        public List<QuestionItem> Questions { get; set; }
        private Random random;
        private int count;
        public Quiz(string path = "data.txt")
        {
            var lines = File.ReadAllLines(path);
            Questions = lines
                .Select(line => line.Split('|'))
                .Select(line => new QuestionItem()
                {
                    Question = line[0],
                    Answer = line[1]
                })
                .ToList();
            random = new Random();
            count = Questions.Count;
        }

        public QuestionItem NextQuestion()
        {
            if (count < 1)
            {
                count = Questions.Count;
            }

            var index = random.Next(count - 1);
            var question = Questions[index];

            Questions.RemoveAt(index);
            Questions.Add(question);
            count--;

            return question;
        }
    }
}
