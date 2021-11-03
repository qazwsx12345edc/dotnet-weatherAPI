using System;
using Domains.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure
{
    public partial class weatherforecastdbContext : DbContext
    {
        public weatherforecastdbContext()
        {
        }

        public weatherforecastdbContext(DbContextOptions<weatherforecastdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<Weather> Weathers { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.HasComment("城市表")
                    .UseCollation("utf8mb4_general_ci");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键");

                entity.Property(e => e.CityName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("city_name")
                    .HasDefaultValueSql("''")
                    .HasComment("城市名");

                entity.Property(e => e.DbCreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("db_created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("创建时间");

                entity.Property(e => e.DbUpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("db_updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("最后修改时间");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("full_name")
                    .HasDefaultValueSql("''")
                    .HasComment("城市全名 省/市/区");

                entity.Property(e => e.Monitor)
                    .HasColumnType("tinyint(1) unsigned zerofill")
                    .HasColumnName("monitor")
                    .HasComment("表示是否开启城市的历史天气查询功能 1开启 0关闭");
            });

            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("weather");

                entity.HasComment("天气表")
                    .UseCollation("utf8mb4_general_ci");

                entity.HasIndex(e => e.CityId, "idx_cityid");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasComment("主键");

                entity.Property(e => e.CityId)
                    .HasColumnName("city_id")
                    .HasComment("城市ID");

                entity.Property(e => e.DbCreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("db_created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("创建时间");

                entity.Property(e => e.DbUpdatedAt)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("db_updated_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("最后修改时间");

                entity.Property(e => e.Preesure)
                    .HasColumnType("int(11) unsigned zerofill")
                    .HasColumnName("preesure")
                    .HasComment("气压");

                entity.Property(e => e.Temp)
                    .HasColumnType("int(11) unsigned zerofill")
                    .HasColumnName("temp")
                    .HasDefaultValueSql("'00000000000'")
                    .HasComment("实时温度");

                entity.Property(e => e.TempHigh)
                    .HasColumnName("temp_high")
                    .HasComment("最高温度");

                entity.Property(e => e.TempLow)
                    .HasColumnName("temp_low")
                    .HasComment("最低温度");

                entity.Property(e => e.WeatherDatetime)
                    .HasColumnType("datetime")
                    .HasColumnName("weather_datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasComment("天气更新时间");

                entity.Property(e => e.WeatherKind)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("weather_kind")
                    .HasDefaultValueSql("''")
                    .HasComment("天气总体情况描述");

                entity.Property(e => e.WindDirect)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("wind_direct")
                    .HasDefaultValueSql("''")
                    .HasComment("风向");

                entity.Property(e => e.WindPower)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("wind_power")
                    .HasDefaultValueSql("''")
                    .HasComment("风力级别");

                entity.Property(e => e.WindSpeed)
                    .HasColumnType("decimal(10,1) unsigned zerofill")
                    .HasColumnName("wind_speed")
                    .HasComment("风速");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
