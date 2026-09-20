using PortfolioBackend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace PortfolioBackend.Entities
{
    public class Projects
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string Link { get; set; }

        public string ImageUrl { get; set; }
    }

}
