namespace MafiatorApp.Services
{
   public interface IAlert
    {
        void ShortAlert(string message, MessageType type);
    }
   public enum MessageType
   {
       None,
       Success,
       Warning,
       Error,
       Info
   }
}
