namespace PortfolioBackend.Entities
{
    public class Certificates
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Link {  get; set; }
    }
}
