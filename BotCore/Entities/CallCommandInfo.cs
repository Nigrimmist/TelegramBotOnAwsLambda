using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BotCore
{
    public class CallCommandInfo
    {
        /// <summary>
        /// Command name. That name will be displayed to user
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Command description. You can override General description for handler by filling that field.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Command alias word list. using for command alternative search
        /// </summary>
        public List<string> Aliases { get; set; }

    }
}
