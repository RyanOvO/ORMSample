using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ORMSample
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("server=SDWM-20141109VA;database=Blogging;user id=sa;pwd=@zhiyong0304.;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // **************   数据表字段映射   *****************
            modelBuilder.Entity<Blog>().ToTable("Blog");
            modelBuilder.Entity<Blog>().Property(b => b.Url).HasColumnName("url");
            modelBuilder.Entity<Blog>().Property(b => b.BlogId).HasColumnName("id");

            modelBuilder.Entity<Post>().ToTable("Post");
            modelBuilder.Entity<Post>().Property(p => p.Content).HasColumnName("content");
            modelBuilder.Entity<Post>().Property(p => p.Title).HasColumnName("title");
            modelBuilder.Entity<Post>().Property(p => p.PostId).HasColumnName("id");
            modelBuilder.Entity<Post>().Property(p => p.BlogId).HasColumnName("BlogID");


            // ******************  shadow 映射   *****************
            modelBuilder.Entity<Blog>().Property<DateTime>("CreatedTime").HasColumnName("CreatedTime");
            modelBuilder.Entity<Post>().Property<DateTime>("CreatedTime").HasColumnName("CreatedTime");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var modifiedEntries = ChangeTracker
                                .Entries().Where(x => x.State == EntityState.Added);

            foreach (var item in modifiedEntries)
            {
                item.Property("CreatedTime").CurrentValue = DateTime.Now;
            }
            return await base.SaveChangesAsync();
        }
    }

}
