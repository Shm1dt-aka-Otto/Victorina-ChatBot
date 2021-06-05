using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;


namespace QuizLogic
{
    public class GameObject
    {
        private static Quiz quiz;
        private static Dictionary<long, QuestionState> States;
        private static Dictionary<long, int> UserScores;
        private static string StateFilename = "state.json";
        private static string ScoreFilename = "score.json";

        public GameObject()
        {
            quiz = new Quiz("data.txt");
            if (File.Exists(StateFilename))
            {
                var json = File.ReadAllText(StateFilename);
                States = JsonConvert.DeserializeObject<Dictionary<long, QuestionState>>(json);
            }
            else
            {
                States = new Dictionary<long, QuestionState>();
            }
            if (File.Exists(ScoreFilename))
            {
                var json = File.ReadAllText(ScoreFilename);
                UserScores = JsonConvert.DeserializeObject<Dictionary<long, int>>(json);
            }
            else
            {
                UserScores = new Dictionary<long, int>();
            }
        }

        public void Finish()
        {
            var stateJson = JsonConvert.SerializeObject(States);
            File.WriteAllText(StateFilename, stateJson);
            var scoreJson = JsonConvert.SerializeObject(UserScores);
            File.WriteAllText(ScoreFilename, scoreJson);
        }

        public void NewRounds(long chatId)
        {
            if (!States.TryGetValue(chatId, out var state))
            {
                state = new QuestionState();
                States[chatId] = state;
            }
            state.CurrentItem = quiz.NextQuestion();
            state.Opened = 0;
            SendMessage(chatId, state.DispayQuestion);
        }

        public Action<long, string> SendMessage;

        public void OnMessage(string message, long chatId, long fromId)
        {
            if (message == null)
            {
                return;
            }
            if (message == "/start")
            {
                NewRounds(chatId);
            }
            else
            {
                if (!States.TryGetValue(chatId, out var state))
                {
                    state = new QuestionState();
                    States[chatId] = state;
                }

                if (state.CurrentItem == null)
                {
                    state.CurrentItem = quiz.NextQuestion();
                }

                var question = state.CurrentItem;
                var tryAnswer = message?.ToLower().Replace('ё', 'е');
                if (tryAnswer == question.Answer)
                {
                    if (UserScores.ContainsKey(fromId))
                    {
                        UserScores[fromId]++;
                    }
                    else
                    {
                        UserScores[fromId] = 1;
                    }
                    SendMessage(chatId, $"Правильно!\nУ вас {UserScores[fromId]} очков");
                    NewRounds(chatId);
                }
                else
                {
                    state.Opened++;
                    if (state.IsEnd)
                    {
                        SendMessage(chatId, $"Никто не отгадал! Это было - {question.Answer}");
                        NewRounds(chatId);
                    }
                    else
                    {
                        SendMessage(chatId, state.DispayQuestion);
                    }
                }
            }
        }
    }
}
