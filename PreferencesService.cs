using DüsseldorferSchülerinventar.Models;

namespace DüsseldorferSchülerinventar.Services
{
    public class PreferencesService
    {
        private const string AuthKey = "auth_token";
        private const string CurrentUserIdKey = "current_user_id";
        private const string CurrentProfileIdKey = "current_profile_id";

        public string AuthToken
        {
            get => Preferences.Get(AuthKey, string.Empty);
            set => Preferences.Set(AuthKey, value);
        }

        public int CurrentUserId
        {
            get => Preferences.Get(CurrentUserIdKey, 0);
            set => Preferences.Set(CurrentUserIdKey, value);
        }

        public int CurrentProfileId
        {
            get => Preferences.Get(CurrentProfileIdKey, 0);
            set => Preferences.Set(CurrentProfileIdKey, value);
        }

        public void ClearSession()
        {
            Preferences.Remove(AuthKey);
            Preferences.Remove(CurrentUserIdKey);
            Preferences.Remove(CurrentProfileIdKey);
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(AuthToken);
    }
}