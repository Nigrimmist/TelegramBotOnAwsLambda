using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BotCore.Entities;

namespace BotCore.Modules
{
    public class Or : MessageHandlerBase
    {
        public override DescriptionInfo ModuleDescription
        {
            get
            {
                return new DescriptionInfo()
                {
                    Description = @"Выбирает между чем-то. Использует ""Или"" в качестве разделителя",
                    CommandScheme = "<аргумент1> или <аргумент2> или <аргумент_n>",
                    SamplesOfUsing = new List<string>()
                    {
                        "да или нет",
                        "лукашенко или лукашенко",
                        "1 или 2 или 3 или 4"
                    }
                };
            }
        }


        private Random _r = new Random();
        private const int ChanceOfSpecialAnswer = 30;

        private List<string> _customMessages = new List<string>()
        {
            "Думаю {0}",
            "Определенно {0}",
            "{0}. К гадалке не ходи",
            "Не нужно быть семи пядей во лбу, чтобы понять, что {0} тут единственно верный вариант",
            "Скорее всего {0}",
            "Я выбираю {0}",
            "Пусть будет {0}",
            "Эники бэники... {0}!"
        };

        public override string HandleMessage(string message)
        {
            var variants = Regex.Split(message, " или ", RegexOptions.IgnoreCase).Distinct().ToList();
            string answer = "А что, есть выбор?";
            if (variants.Any())
            {
                answer = variants[_r.Next(0, variants.Count())].Trim().Trim('?', '!');

                string[] separators = { ",", ".", "!",  " ",  "?", ":" };
                answer = answer.Split(separators, StringSplitOptions.RemoveEmptyEntries).Last();
                if (_r.Next(1, 101) < ChanceOfSpecialAnswer)
                    answer = string.Format(_customMessages[_r.Next(0, _customMessages.Count)], answer);
            }
            

            return answer;
        }

        public override bool CanHandle(string message)
        {
            return new Regex(" или ",RegexOptions.IgnoreCase).IsMatch(message);
        }
    }
}
