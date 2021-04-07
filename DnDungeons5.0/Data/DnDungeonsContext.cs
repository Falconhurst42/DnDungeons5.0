using Microsoft.EntityFrameworkCore;
using DnDungeons.Models;

namespace DnDungeons.Data
{
    public class DnDungeonsContext : DbContext
    {
        public DnDungeonsContext (DbContextOptions<DnDungeonsContext> options)
            : base(options)
        {
        }

        // regular tables
        public DbSet<Dungeon> Dungeons { get; set; }
        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<EnemyInRoom> EnemyInRooms { get; set; }
        public DbSet<EnemyInSet> EnemyInSets { get; set; }
        public DbSet<EnemyMedia> EnemyMedias { get; set; }
        public DbSet<EnemySet> EnemySets { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<LayoutMedia> LayoutMedias { get; set; }
        public DbSet<Loot> Loots { get; set; }
        public DbSet<LootInRoom> LootInRooms { get; set; }
        public DbSet<LootInSet> LootInSets { get; set; }
        public DbSet<LootMedia> LootMedias { get; set; }
        public DbSet<LootSet> LootSets { get; set; }
        public DbSet<Media> Medias { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomConnection> RoomConnections { get; set; }

        //lookup tables
        public DbSet<CRToXP> CRToXPs { get; set; }
        public DbSet<EncounterMultiplier> EncounterMultipliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dungeon>().ToTable("Dungeon");
            modelBuilder.Entity<Enemy>().ToTable("Enemy");
            modelBuilder.Entity<EnemyInRoom>().ToTable("EnemyInRoom");
            modelBuilder.Entity<EnemyInSet>().ToTable("EnemyInSet");
            modelBuilder.Entity<EnemyMedia>().ToTable("EnemyMedia");
            modelBuilder.Entity<EnemySet>().ToTable("EnemySet");
            modelBuilder.Entity<Layout>().ToTable("Layout");
            modelBuilder.Entity<LayoutMedia>().ToTable("LayoutMedia");
            modelBuilder.Entity<Loot>().ToTable("Loot");
            modelBuilder.Entity<LootInRoom>().ToTable("LootInRoom");
            modelBuilder.Entity<LootInSet>().ToTable("LootInSet");
            modelBuilder.Entity<LootMedia>().ToTable("LootMedia");
            modelBuilder.Entity<LootSet>().ToTable("LootSet");
            modelBuilder.Entity<Media>().ToTable("Media");
            modelBuilder.Entity<Room>().ToTable("Room");
            modelBuilder.Entity<RoomConnection>().ToTable("RoomConnection");
            modelBuilder.Entity<CRToXP>().ToTable("CRToXP");
            modelBuilder.Entity<EncounterMultiplier>().ToTable("EncounterMultiplier");

            modelBuilder.Entity<EnemyInRoom>()
                .HasKey(c => new { c.DungeonID, c.RoomNum, c.EnemyID });
            modelBuilder.Entity<EnemyInSet>()
                .HasKey(c => new { c.EnemySetID, c.EnemyID });
            modelBuilder.Entity<EnemyMedia>()
                .HasKey(c => new { c.EnemyID, c.MediaID });
            modelBuilder.Entity<LayoutMedia>()
                .HasKey(c => new { c.LayoutID, c.MediaID });
            modelBuilder.Entity<LootInRoom>()
                .HasKey(c => new { c.DungeonID, c.RoomNum, c.LootID });
            modelBuilder.Entity<LootInSet>()
                .HasKey(c => new { c.LootSetID, c.LootID });
            modelBuilder.Entity<LootMedia>()
                .HasKey(c => new { c.LootID, c.MediaID });
            modelBuilder.Entity<Room>()
                .HasKey(c => new { c.RoomNumber, c.DungeonID });
            modelBuilder.Entity<RoomConnection>()
                .HasKey(c => new { c.DungeonID, c.Room1Num, c.Room2Num });
            modelBuilder.Entity<CRToXP>()
                .HasNoKey();
            modelBuilder.Entity<EncounterMultiplier>()
                .HasNoKey();

            // Code for configuring roomconnection cascading deletes
            modelBuilder.Entity<RoomConnection>()
                .HasOne(d => d.Dungeon)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<RoomConnection>()
                .HasOne(d => d.Room1)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            // default values for dates https://www.learnentityframeworkcore.com/configuration/data-annotation-attributes/databasegenerated-attribute
            modelBuilder.Entity<Dungeon>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Dungeon>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Enemy>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Enemy>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<EnemySet>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<EnemySet>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Layout>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Layout>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Loot>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Loot>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<LootSet>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<LootSet>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Media>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Media>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Room>()
                .Property(d => d.Created)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Room>()
                .Property(d => d.LastUpdated)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
