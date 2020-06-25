namespace Map.Client
{
    using Map.Client.Models;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GpsContext : DbContext
    {
        public DbSet<IMEI> IMEIs { get; set; }

        //public DbSet<TeltonikaTcpPacket> TeltonikaTcpPackets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IMEI>().Property(i => i.Value).IsRequired();

                //modelBuilder.ApplyConfiguration(new TeltonikaTcpPacketConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;uid=dbuser;pwd=1234;database=GPS");
        }
    }

    public class TeltonikaTcpPacketConfiguration : IEntityTypeConfiguration<TeltonikaTcpPacket>
    {
        public void Configure(EntityTypeBuilder<TeltonikaTcpPacket> builder)
        {
            
            builder.ToTable("TcpPacket");
            builder.Property<int>("ID")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasAnnotation("Key", 0);

         
            //var navigation = builder.Metadata.FindNavigation(nameof(Person.Pictures));

            //navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
