using Microsoft.EntityFrameworkCore;

namespace StudentSystem.Models
{
    public class SISDBContext: DbContext
    {
        public SISDBContext(DbContextOptions<SISDBContext> options) : base(options){}

        //Table Students
        public DbSet<Student> Students { get; set; }

    }
}
