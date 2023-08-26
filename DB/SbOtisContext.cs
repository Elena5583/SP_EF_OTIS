using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SP_EF_OTIS.Entities;

namespace SP_EF_OTIS.DB;

public partial class SbOtisContext : DbContext
{

    private IConfiguration Configuration { get;  }

    public SbOtisContext()
    {
        Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();

    }


    public SbOtisContext(DbContextOptions<SbOtisContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bankaccount> Bankaccounts { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Currency> Currencies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("db"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pkey");

            entity.ToTable("clients");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasDefaultValueSql("'Неизвестно'::character varying")
                .HasColumnName("address");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("firstname");
            entity.Property(e => e.Inn)
                .HasMaxLength(12)
                .HasColumnName("inn");
            entity.Property(e => e.Lastname)
                .HasMaxLength(150)
                .HasColumnName("lastname");
        });

        
        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("currencies_pkey");

            entity.ToTable("currencies");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .HasColumnName("name");
            entity.Property(e => e.Shortname)
                .HasMaxLength(50)
                .HasColumnName("shortname");
        });
        

        modelBuilder.Entity<Bankaccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("bankaccounts_pkey");

            entity.ToTable("bankaccounts");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Bankaccountturnover)
                .HasColumnType("money")
                .HasColumnName("bankaccountturnover");
            entity.Property(e => e.Clientid).HasColumnName("clientid");
            entity.Property(e => e.Currencyid).HasColumnName("currencyid");

            entity.Property(e => e.Numberaccount)
                .HasMaxLength(20)
                .HasColumnName("numberaccount");

            entity.HasOne(d => d.Client).WithMany(p => p.Bankaccounts)
                .HasForeignKey(d => d.Clientid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("bankaccounts_clientid_fkey");


            entity.HasOne(d => d.Currency).WithMany(p => p.Bankaccounts)
                .HasForeignKey(d => d.Currencyid)
                .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("bankaccounts_currencyid_fkey");
        });
        

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
