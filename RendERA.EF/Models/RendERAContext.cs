using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RendERA.DB.Models
{
    public partial class RendERAContext : DbContext
    {
        public RendERAContext()
        {
        }

        public RendERAContext(DbContextOptions<RendERAContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ConfigurationSettings> ConfigurationSettings { get; set; }
        public virtual DbSet<DefaultRenderSoftwares> DefaultRenderSoftwares { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<ErrorLogs> ErrorLogs { get; set; }
        public virtual DbSet<JobLog> JobLog { get; set; }
        public virtual DbSet<JobSettings> JobSettings { get; set; }
        public virtual DbSet<Jobs> Jobs { get; set; }
        public virtual DbSet<MasterCategory> MasterCategory { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MessageReply> MessageReply { get; set; }
        public virtual DbSet<Parameter> Parameter { get; set; }
        public virtual DbSet<ParameterDocumnetMapping> ParameterDocumnetMapping { get; set; }
        public virtual DbSet<ParameterSoftwareRenderMapping> ParameterSoftwareRenderMapping { get; set; }
        public virtual DbSet<ParameterUserCategoryMapping> ParameterUserCategoryMapping { get; set; }
        public virtual DbSet<Parameters> Parameters { get; set; }
        public virtual DbSet<PaymentTransaction> PaymentTransaction { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ServerList> ServerList { get; set; }
        public virtual DbSet<Team> Team { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=SQL12-16-LATEST\\SQL2016;Database=RendERA;User ID=yteamdev;Password=YTeam@2017;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:DefaultSchema", "yteamdev");

            modelBuilder.Entity<ConfigurationSettings>(entity =>
            {
                entity.ToTable("ConfigurationSettings", "dbo");

                entity.Property(e => e.Beginner).HasColumnName("beginner");
            });

            modelBuilder.Entity<DefaultRenderSoftwares>(entity =>
            {
                entity.ToTable("DefaultRenderSoftwares", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.ToTable("Document", "dbo");

                entity.Property(e => e.FileName).IsRequired();

                entity.Property(e => e.FileUploadUrl).IsRequired();

                entity.Property(e => e.IsSubmitted).HasDefaultValueSql("((0))");

                entity.Property(e => e.TrackingId)
                    .IsRequired()
                    .HasDefaultValueSql("('QWERJD12')");

                entity.Property(e => e.TransactionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.AssignedProject)
                    .WithMany(p => p.Document)
                    .HasForeignKey(d => d.AssignedProjectId)
                    .HasConstraintName("FK_ProjectId");
            });

            modelBuilder.Entity<ErrorLogs>(entity =>
            {
                entity.ToTable("ErrorLogs", "dbo");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<JobLog>(entity =>
            {
                entity.ToTable("JobLog", "dbo");

                entity.Property(e => e.IsRead).HasDefaultValueSql("((0))");

                entity.Property(e => e.LogDetail).IsRequired();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.JobLog)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("JobLog_FK_Document");
            });

            modelBuilder.Entity<JobSettings>(entity =>
            {
                entity.ToTable("JobSettings", "dbo");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Jobs>(entity =>
            {
                entity.HasKey(e => e.JobId);

                entity.ToTable("Jobs", "dbo");

                entity.Property(e => e.JobId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FileDownloadUrl).HasMaxLength(30);

                entity.Property(e => e.FileUploadUrl)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remark).HasMaxLength(500);
            });

            modelBuilder.Entity<MasterCategory>(entity =>
            {
                entity.ToTable("MasterCategory", "dbo");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("Message", "dbo");

                entity.Property(e => e.MessageToPost).IsRequired();

                entity.Property(e => e.Subject).IsRequired();
            });

            modelBuilder.Entity<MessageReply>(entity =>
            {
                entity.ToTable("MessageReply", "dbo");

                entity.Property(e => e.ReplyMessage).IsRequired();
            });

            modelBuilder.Entity<Parameter>(entity =>
            {
                entity.ToTable("Parameter", "dbo");

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            });

            modelBuilder.Entity<ParameterDocumnetMapping>(entity =>
            {
                entity.HasKey(e => new { e.DocumentId, e.ParameterId })
                    .HasName("PK__Paramete__753E292813C0BD44");

                entity.ToTable("ParameterDocumnetMapping", "dbo");

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.ParameterDocumnetMapping)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_Document");

                entity.HasOne(d => d.Parameter)
                    .WithMany(p => p.ParameterDocumnetMapping)
                    .HasForeignKey(d => d.ParameterId)
                    .HasConstraintName("FK_ParameterSoftwareRenderMapping_ParameterId");
            });

            modelBuilder.Entity<ParameterSoftwareRenderMapping>(entity =>
            {
                entity.HasKey(e => new { e.ParameterId, e.SoftwareRenderId })
                    .HasName("PK__Paramete__217CD5C83C7F4200");

                entity.ToTable("ParameterSoftwareRenderMapping", "dbo");
            });

            modelBuilder.Entity<ParameterUserCategoryMapping>(entity =>
            {
                entity.HasKey(e => new { e.ParameterId, e.UserCategoryId })
                    .HasName("PK__Paramete__AA4E7E4FDDE958A2");

                entity.ToTable("ParameterUserCategoryMapping", "dbo");
            });

            modelBuilder.Entity<Parameters>(entity =>
            {
                entity.HasKey(e => e.ParameterId);

                entity.ToTable("Parameters", "dbo");

                entity.Property(e => e.ParameterId).ValueGeneratedNever();

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentTransaction>(entity =>
            {
                entity.ToTable("PaymentTransaction", "dbo");

                entity.Property(e => e.PayableAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PaymentId).IsRequired();

                entity.Property(e => e.PercentOff).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ReturnStatus).IsRequired();

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionId).IsRequired();

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.PaymentTransaction)
                    .HasForeignKey(d => d.DocumentId)
                    .HasConstraintName("FK_PaymentTransaction");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("Project", "dbo");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Project)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_ProjectPartnerId");
            });

            modelBuilder.Entity<ServerList>(entity =>
            {
                entity.ToTable("ServerList", "dbo");

                entity.Property(e => e.IpAddress).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.ToTable("Team", "dbo");

                entity.Property(e => e.Degination).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Partner)
                    .WithMany(p => p.Team)
                    .HasForeignKey(d => d.PartnerId)
                    .HasConstraintName("FK_PartnerId");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("Users", "dbo");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.FirstName).HasMaxLength(10);

                entity.Property(e => e.LastName).HasMaxLength(10);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.UserCategory).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(15);
            });
        }
    }
}
