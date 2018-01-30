namespace DZ9.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AreaDB : DbContext
    {
        public AreaDB()
            : base("name=AreaDB")
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<Timer> Timer { get; set; }
        public virtual DbSet<TimerArchive> TimerArchive { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>()
                .Property(e => e.IP)
                .IsUnicode(false);
        }
    }
}
