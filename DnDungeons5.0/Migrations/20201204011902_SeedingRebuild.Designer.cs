// <auto-generated />
using System;
using DnDungeons.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DnDungeons.Migrations
{
    [DbContext(typeof(DnDungeonsContext))]
    [Migration("20201204011902_SeedingRebuild")]
    partial class SeedingRebuild
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("DnDungeons.Models.Dungeon", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Dungeon");
                });

            modelBuilder.Entity("DnDungeons.Models.Enemy", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<decimal>("CR")
                        .HasColumnType("decimal(5,3)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("XP")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("Enemy");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyInRoom", b =>
                {
                    b.Property<int>("DungeonID")
                        .HasColumnType("int");

                    b.Property<int>("RoomNum")
                        .HasColumnType("int");

                    b.Property<int>("EnemyID")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RoomDungeonID")
                        .HasColumnType("int");

                    b.Property<int?>("RoomNumber")
                        .HasColumnType("int");

                    b.HasKey("DungeonID", "RoomNum", "EnemyID");

                    b.HasIndex("EnemyID");

                    b.HasIndex("RoomNumber", "RoomDungeonID");

                    b.ToTable("EnemyInRoom");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyInSet", b =>
                {
                    b.Property<int>("EnemySetID")
                        .HasColumnType("int");

                    b.Property<int>("EnemyID")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EnemySetID", "EnemyID");

                    b.HasIndex("EnemyID");

                    b.ToTable("EnemyInSet");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyMedia", b =>
                {
                    b.Property<int>("EnemyID")
                        .HasColumnType("int");

                    b.Property<int>("MediaID")
                        .HasColumnType("int");

                    b.Property<string>("MediaLabel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EnemyID", "MediaID");

                    b.HasIndex("MediaID");

                    b.ToTable("EnemyMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemySet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("EnemySet");
                });

            modelBuilder.Entity("DnDungeons.Models.Layout", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Layout");
                });

            modelBuilder.Entity("DnDungeons.Models.LayoutMedia", b =>
                {
                    b.Property<int>("LayoutID")
                        .HasColumnType("int");

                    b.Property<int>("MediaID")
                        .HasColumnType("int");

                    b.Property<string>("MediaLabel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LayoutID", "MediaID");

                    b.HasIndex("MediaID");

                    b.ToTable("LayoutMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.Loot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsMoney")
                        .HasColumnType("bit");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("MonetaryValue")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Loot");
                });

            modelBuilder.Entity("DnDungeons.Models.LootInRoom", b =>
                {
                    b.Property<int>("DungeonID")
                        .HasColumnType("int");

                    b.Property<int>("RoomNum")
                        .HasColumnType("int");

                    b.Property<int>("LootID")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("RoomDungeonID")
                        .HasColumnType("int");

                    b.Property<int?>("RoomNumber")
                        .HasColumnType("int");

                    b.HasKey("DungeonID", "RoomNum", "LootID");

                    b.HasIndex("LootID");

                    b.HasIndex("RoomNumber", "RoomDungeonID");

                    b.ToTable("LootInRoom");
                });

            modelBuilder.Entity("DnDungeons.Models.LootInSet", b =>
                {
                    b.Property<int>("LootSetID")
                        .HasColumnType("int");

                    b.Property<int>("LootID")
                        .HasColumnType("int");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LootSetID", "LootID");

                    b.HasIndex("LootID");

                    b.ToTable("LootInSet");
                });

            modelBuilder.Entity("DnDungeons.Models.LootMedia", b =>
                {
                    b.Property<int>("LootID")
                        .HasColumnType("int");

                    b.Property<int>("MediaID")
                        .HasColumnType("int");

                    b.Property<string>("MediaLabel")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("LootID", "MediaID");

                    b.HasIndex("MediaID");

                    b.ToTable("LootMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.LootSet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("LootSet");
                });

            modelBuilder.Entity("DnDungeons.Models.Media", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Filepath")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Filetype")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("DnDungeons.Models.Room", b =>
                {
                    b.Property<int>("RoomNumber")
                        .HasColumnType("int");

                    b.Property<int>("DungeonID")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int?>("LayoutID")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RoomNumber", "DungeonID");

                    b.HasIndex("DungeonID");

                    b.HasIndex("LayoutID");

                    b.ToTable("Room");
                });

            modelBuilder.Entity("DnDungeons.Models.RoomConnection", b =>
                {
                    b.Property<int>("DungeonID")
                        .HasColumnType("int");

                    b.Property<int>("Room1Num")
                        .HasColumnType("int");

                    b.Property<int>("Room2Num")
                        .HasColumnType("int");

                    b.Property<string>("ConnectionPoint1")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConnectionPoint2")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("Room1DungeonID")
                        .HasColumnType("int");

                    b.Property<int?>("Room1RoomNumber")
                        .HasColumnType("int");

                    b.Property<int?>("Room2DungeonID")
                        .HasColumnType("int");

                    b.Property<int?>("Room2RoomNumber")
                        .HasColumnType("int");

                    b.HasKey("DungeonID", "Room1Num", "Room2Num");

                    b.HasIndex("Room1RoomNumber", "Room1DungeonID");

                    b.HasIndex("Room2RoomNumber", "Room2DungeonID");

                    b.ToTable("RoomConnection");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyInRoom", b =>
                {
                    b.HasOne("DnDungeons.Models.Enemy", "Enemy")
                        .WithMany()
                        .HasForeignKey("EnemyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Room", "Room")
                        .WithMany("Enemies")
                        .HasForeignKey("RoomNumber", "RoomDungeonID");

                    b.Navigation("Enemy");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyInSet", b =>
                {
                    b.HasOne("DnDungeons.Models.Enemy", "Enemy")
                        .WithMany()
                        .HasForeignKey("EnemyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.EnemySet", "EnemySet")
                        .WithMany("EnemiesInSet")
                        .HasForeignKey("EnemySetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enemy");

                    b.Navigation("EnemySet");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemyMedia", b =>
                {
                    b.HasOne("DnDungeons.Models.Enemy", "Enemy")
                        .WithMany("AssociatedMedia")
                        .HasForeignKey("EnemyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Media", "Media")
                        .WithMany()
                        .HasForeignKey("MediaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Enemy");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("DnDungeons.Models.LayoutMedia", b =>
                {
                    b.HasOne("DnDungeons.Models.Layout", "Layout")
                        .WithMany("AssociatedMedia")
                        .HasForeignKey("LayoutID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Media", "Media")
                        .WithMany()
                        .HasForeignKey("MediaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Layout");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("DnDungeons.Models.LootInRoom", b =>
                {
                    b.HasOne("DnDungeons.Models.Loot", "Loot")
                        .WithMany()
                        .HasForeignKey("LootID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Room", "Room")
                        .WithMany("Loot")
                        .HasForeignKey("RoomNumber", "RoomDungeonID");

                    b.Navigation("Loot");

                    b.Navigation("Room");
                });

            modelBuilder.Entity("DnDungeons.Models.LootInSet", b =>
                {
                    b.HasOne("DnDungeons.Models.Loot", "Loot")
                        .WithMany()
                        .HasForeignKey("LootID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.LootSet", "LootSet")
                        .WithMany("LootInSet")
                        .HasForeignKey("LootSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loot");

                    b.Navigation("LootSet");
                });

            modelBuilder.Entity("DnDungeons.Models.LootMedia", b =>
                {
                    b.HasOne("DnDungeons.Models.Loot", "Loot")
                        .WithMany("AssociatedMedia")
                        .HasForeignKey("LootID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Media", "Media")
                        .WithMany()
                        .HasForeignKey("MediaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Loot");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("DnDungeons.Models.Room", b =>
                {
                    b.HasOne("DnDungeons.Models.Dungeon", "Dungeon")
                        .WithMany("Rooms")
                        .HasForeignKey("DungeonID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Layout", "Layout")
                        .WithMany()
                        .HasForeignKey("LayoutID");

                    b.Navigation("Dungeon");

                    b.Navigation("Layout");
                });

            modelBuilder.Entity("DnDungeons.Models.RoomConnection", b =>
                {
                    b.HasOne("DnDungeons.Models.Dungeon", "Dungeon")
                        .WithMany()
                        .HasForeignKey("DungeonID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("DnDungeons.Models.Room", "Room1")
                        .WithMany()
                        .HasForeignKey("Room1RoomNumber", "Room1DungeonID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DnDungeons.Models.Room", "Room2")
                        .WithMany()
                        .HasForeignKey("Room2RoomNumber", "Room2DungeonID");

                    b.Navigation("Dungeon");

                    b.Navigation("Room1");

                    b.Navigation("Room2");
                });

            modelBuilder.Entity("DnDungeons.Models.Dungeon", b =>
                {
                    b.Navigation("Rooms");
                });

            modelBuilder.Entity("DnDungeons.Models.Enemy", b =>
                {
                    b.Navigation("AssociatedMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.EnemySet", b =>
                {
                    b.Navigation("EnemiesInSet");
                });

            modelBuilder.Entity("DnDungeons.Models.Layout", b =>
                {
                    b.Navigation("AssociatedMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.Loot", b =>
                {
                    b.Navigation("AssociatedMedia");
                });

            modelBuilder.Entity("DnDungeons.Models.LootSet", b =>
                {
                    b.Navigation("LootInSet");
                });

            modelBuilder.Entity("DnDungeons.Models.Room", b =>
                {
                    b.Navigation("Enemies");

                    b.Navigation("Loot");
                });
#pragma warning restore 612, 618
        }
    }
}
