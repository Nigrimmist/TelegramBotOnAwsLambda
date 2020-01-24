using System;
using System.Collections.Generic;
using System.Text;
using BotCore.Entities;
using BotCore.Modules;

namespace BotCore.Managers
{
    public class BotModuleManager
    {
        private readonly List<MessageHandlerBase> _handlers;

        public BotModuleManager()
        {
            _handlers = new List<MessageHandlerBase>()
            {
                new Or()
            };

        }

        public string HandleMessage(string message)
        {
            Random r = new Random();
            List<string> notUnderstandAnswers = new List<string>()
            {
                "ась?", "нипонимати", "а если по-русски?", "хозяин, ну чего он от меня хочет? (", "СЛОЖНА", "а полегче вопросики есть?", "атань"
            };
            foreach (var handler in _handlers)
            {
                if (handler.CanHandle(message))
                {
                    return handler.HandleMessage(message);
                }
            }

            return notUnderstandAnswers[r.Next(0, notUnderstandAnswers.Count)];
        }
    }
}
