namespace AzureDevopsTracker.Integrations
{
    public class MessageConfig
    {
        public MessageConfig(EMessengers messenger, string url)
        {
            Messenger = messenger;
            URL = url;
        }

        public static EMessengers? Messenger { get; private set; }
        public static string URL { get; private set; }
    }

    public enum EMessengers
    {
        DISCORD,
        MICROSOFT_TEAMS
    }
}