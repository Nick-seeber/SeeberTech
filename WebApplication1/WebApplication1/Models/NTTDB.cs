namespace WebApplication1.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class NTTDB : DbContext
    {
        public NTTDB()
            : base("name=NTTDB")
        {
        }

        public virtual DbSet<media_info> media_info { get; set; }
        public virtual DbSet<TestTable> TestTables { get; set; }
        public virtual DbSet<current_movies> current_movies { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestTable>()
                .Property(e => e.TestName)
                .IsFixedLength();
        }
    }
}
