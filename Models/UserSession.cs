namespace StudentEmailApp.Models
{
    /// <summary>
    /// Holds the global state for the currently authenticated user in the desktop session.
    /// </summary>
    public static class UserSession
    {
        public static string CurrentUsername { get; set; } = "Guest";
        public static int CurrentUserId { get; set; } = 0;

        public static void Clear()
        {
            CurrentUsername = "Guest";
            CurrentUserId = 0;
        }
    }
}
