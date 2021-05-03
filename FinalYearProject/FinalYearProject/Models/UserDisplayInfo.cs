namespace FinalYearProject.Models
{
    public record UserDisplayInfo
    {
        public UserDisplayInfo() { }

        public UserDisplayInfo(User user)
        {
            if (user is not null)
            {
                Username = user.Username;
                ProfileColourHex = user.ProfileColourHex;
            }
        }

        public string Username { get; set; }

        public string ProfileColourHex { get; set; }
    }
}
