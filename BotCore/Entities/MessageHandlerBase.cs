namespace BotCore.Entities
{
    public abstract class MessageHandlerBase 
    {
        
        /// <summary>
        /// Message handler
        /// </summary>
        /// <param name="message">Incoming command</param>
        public abstract string HandleMessage(string message);

        
        public abstract bool CanHandle(string message);
       

        /// <summary>
        /// Your command description info
        /// </summary>
        public virtual DescriptionInfo ModuleDescription
        {
            get { return null; }
        }
    }
}
