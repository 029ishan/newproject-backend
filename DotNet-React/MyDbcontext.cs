using DotNet_React.StudentData;
using Microsoft.EntityFrameworkCore;

namespace DotNet_React
{
    public class MyDbcontext :DbContext 
    {
        public MyDbcontext(DbContextOptions<MyDbcontext> options):base(options) { }
        public DbSet<DetailStudents> collegestudent {  get; set; }
        public DbSet<SignupDetail> Signupdetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SignupDetail>().ToTable("signup_data");
        }
    }

}
