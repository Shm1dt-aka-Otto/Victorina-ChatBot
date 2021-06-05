using System;
using QuizLogic;
using VkBotFramework;
using VkBotFramework.Models;
using VkNet.Model.RequestParams;

namespace VKChatBot
{
    class VKBot
    {
        private static GameObject Game;
        private static VkBot bot;
        private static string AccessToken = "Whoops";
        private static string GroupUrl = "Whoops";


        static void Main(string[] args)
        {
            Game = new GameObject();
            bot = new VkBot(AccessToken, GroupUrl);
            bot.OnMessageReceived += BotOnMessageReceived;
            Game.SendMessage = (l, s) => bot.Api.Messages.Send(new MessagesSendParams()
            {
                Message = s,
                PeerId = l,
                RandomId = Environment.TickCount
            });
            bot.Start();
            Console.WriteLine("Бот запущен");
            Console.ReadLine();
            Game.Finish();
        }

        private static void BotOnMessageReceived(object? sender, MessageReceivedEventArgs e)
        {
            var chatId = e.Message.PeerId.Value;
            var message = e.Message.Text;
            var fromId = e.Message.FromId.Value;
            Game.OnMessage(message, chatId, fromId);
        }
    }
}
