namespace PortfolioBackend.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }   
        public string Bio { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfileImageUrl { get; set; }
        public string GitHub { get; set; }
        public string LinkedIn { get; set; }
    }
}
