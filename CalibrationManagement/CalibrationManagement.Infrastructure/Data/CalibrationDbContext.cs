using Microsoft.EntityFrameworkCore;
using CalibrationManagement.Core.Entities;

namespace CalibrationManagement.Infrastructure.Data
{
    public class CalibrationDbContext : DbContext
    {
        public CalibrationDbContext(DbContextOptions<CalibrationDbContext> options) : base(options)
        {
        }

        public DbSet<CalInfo> CalInfo { get; set; }
        public DbSet<CalData> CalData { get; set; }
        public DbSet<CalStandards> CalStandards { get; set; }
        public DbSet<CalSetup> CalSetup { get; set; }
        public DbSet<CalTechs> CalTechs { get; set; }
        public DbSet<Tolerances> Tolerances { get; set; }

        public DbSet<Company> Company { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<OrdrStat> OrdrStats { get; set; }
        public DbSet<OrDetail> OrderDetails { get; set; }
        public DbSet<ModelNo> ModelNumbers { get; set; }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<PayDtl> PaymentDetails { get; set; }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureCalibrationRelationships(modelBuilder);
            ConfigureBusinessRelationships(modelBuilder);
            ConfigureFinancialRelationships(modelBuilder);

            ConfigureIndexes(modelBuilder);

            ConfigureUniqueConstraints(modelBuilder);
        }

        private void ConfigureCalibrationRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalInfo>()
                .HasOne(c => c.Company)
                .WithMany(co => co.CalInfos)
                .HasForeignKey(c => c.CoId)
                .HasPrincipalKey(co => co.CoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CalInfo>()
                .HasMany(c => c.CalData)
                .WithOne(cd => cd.CalInfo)
                .HasForeignKey(cd => cd.CalId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CalInfo>()
                .HasMany(c => c.CalStandards)
                .WithOne(cs => cs.CalInfo)
                .HasForeignKey(cs => cs.CalId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureBusinessRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>()
                .HasMany(c => c.Contacts)
                .WithOne(co => co.Company)
                .HasForeignKey(co => co.CoId)
                .HasPrincipalKey(c => c.CoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.OrdrStats)
                .WithOne(o => o.Company)
                .HasForeignKey(o => o.CoId)
                .HasPrincipalKey(c => c.CoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.ModelNumbers)
                .WithOne(m => m.Company)
                .HasForeignKey(m => m.CoId)
                .HasPrincipalKey(c => c.CoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Company>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Company)
                .HasForeignKey(i => i.CoId)
                .HasPrincipalKey(c => c.CoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<OrdrStat>()
                .HasMany(o => o.OrderDetails)
                .WithOne(od => od.Order)
                .HasForeignKey(od => od.OrderNo)
                .HasPrincipalKey(o => o.OrderNo)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureFinancialRelationships(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.PaymentDetails)
                .WithOne(pd => pd.Invoice)
                .HasForeignKey(pd => pd.InvoiceNo)
                .HasPrincipalKey(i => i.InvoiceNo)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.CalNo)
                .IsUnique();

            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.CoId);

            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.SerialNo);

            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.CalDate);

            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.DueDate);

            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.CalType);

            modelBuilder.Entity<CalData>()
                .HasIndex(cd => cd.CalId);

            modelBuilder.Entity<CalStandards>()
                .HasIndex(cs => cs.CalId);

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.CoId)
                .IsUnique();

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.CoName);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => c.CoId);

            modelBuilder.Entity<OrdrStat>()
                .HasIndex(o => o.OrderNo)
                .IsUnique();

            modelBuilder.Entity<OrdrStat>()
                .HasIndex(o => o.CoId);

            modelBuilder.Entity<ModelNo>()
                .HasIndex(m => m.SerialNo)
                .IsUnique();

            modelBuilder.Entity<ModelNo>()
                .HasIndex(m => m.CoId);

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNo)
                .IsUnique();

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.CoId);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        private void ConfigureUniqueConstraints(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalInfo>()
                .HasIndex(c => c.CalNo)
                .IsUnique()
                .HasDatabaseName("IX_cal_info_cal_no_unique");

            modelBuilder.Entity<Company>()
                .HasIndex(c => c.CoId)
                .IsUnique()
                .HasDatabaseName("IX_company_co_id_unique");

            modelBuilder.Entity<OrdrStat>()
                .HasIndex(o => o.OrderNo)
                .IsUnique()
                .HasDatabaseName("IX_ordrstat_order_no_unique");

            modelBuilder.Entity<ModelNo>()
                .HasIndex(m => m.SerialNo)
                .IsUnique()
                .HasDatabaseName("IX_model_no_serial_no_unique");

            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNo)
                .IsUnique()
                .HasDatabaseName("IX_invoice_invoice_no_unique");

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Username)
                .IsUnique()
                .HasDatabaseName("IX_users_username_unique");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=CalibrationManagement;Username=postgres;Password=postgres");
            }
        }
    }
}
