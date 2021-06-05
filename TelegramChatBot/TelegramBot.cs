using QuizLogic;
using System;
using Telegram.Bot;


namespace TelegramChatBot
{

    class TelegramBot
    {
        private static GameObject Game;
        private static TelegramBotClient bot;
        static void Main(string[] args)
        {
            Game = new GameObject();
            Console.WriteLine("Hello World!");
            var token = "Whoops...";
            bot = new TelegramBotClient(token);
            bot.OnMessage += Bot_OnMessage;
            Game.SendMessage = (l, s) => bot.SendTextMessageAsync(l, s);
            bot.StartReceiving();
            Console.ReadLine();
            Game.Finish();
        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var chatId = e.Message.Chat.Id;
            var message = e.Message.Text;
            var fromId = e.Message.From.Id;
            Game.OnMessage(message, chatId, fromId);
        }
    }
}
