using System;
using System.Collections.Generic;
using System.Text;

namespace BotCore.Entities
{
    public class DescriptionInfo
    {
        /// <summary>
        /// General description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// One string = one sample of using including full command and arguments. Example : "Search y kittens". Please, do not fill it if your command not support arguments
        /// </summary>
        public List<string> SamplesOfUsing { get; set; }

        public string CommandScheme { get; set; }

        public DescriptionInfo()
        {
            SamplesOfUsing = new List<string>();
        }
    }
}
