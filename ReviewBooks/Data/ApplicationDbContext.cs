using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewBooks.Models;

namespace ReviewBooks.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<ReviewBooks.Models.Book> Book { get; set; } = default!;
        public DbSet<ReviewBooks.Models.BookReview> BookReview { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookReview>()
                .HasOne(br => br.Book)
                .WithMany()
                .HasForeignKey(br => br.BookId);

            modelBuilder.Entity<BookReview>()
                .HasOne(br => br.User)
                .WithMany()
                .HasForeignKey(br => br.UserId);
        }
    }
}
