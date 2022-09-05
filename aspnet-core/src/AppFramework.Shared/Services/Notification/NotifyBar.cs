using Hardcodet.Wpf.TaskbarNotification;

namespace AppFramework.Shared
{
    public static class NotifyBar
    {
        public static void Info(string title, string message)
        {
            new TaskbarIcon().ShowBalloonTip(title, message, BalloonIcon.Info);
        }

        public static void Error(string title, string message)
        {
            new TaskbarIcon().ShowBalloonTip(title, message, BalloonIcon.Error);
        }

        public static void Warning(string title, string message)
        {
            new TaskbarIcon().ShowBalloonTip(title, message, BalloonIcon.Warning);
        }
    }
}
