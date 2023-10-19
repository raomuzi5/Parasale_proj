using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Parasale.Models;

namespace Parasale.Data
{
    public class ParasaleDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        private readonly IConfigurationRoot _config;
        public ParasaleDbContext(DbContextOptions<ParasaleDbContext> options, IConfigurationRoot config)
            : base(options)
        {
            _config = config;
        }

        public DbSet<speechToText> SpeechToTexts { get; set; }
        public DbSet<ObjectionLog> ObjectionLogs { get; set; }
        public DbSet<Objection> Objections { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<QuickCollection> qCollections { get; set; }
        public DbSet<ObjectionNotification> ObjectionNotifications { get; set; }
        public DbSet<AssignedCollection> AssignedCollections { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Invites> Invites { get; set; }
        public DbSet<QuickStartObjections> qObjection { get; set; }
        public DbSet<CCS> CCS { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<InvitesByManager> GetInvitesByManagers { get; set; }
        public DbSet<SessionObjection> sessionObjections { get; set; }
        public DbSet<UserSession> userSessions { get; set; }
        public DbSet<UserHistory> UserHistories { get; set; }
        public DbSet<VoiceOnBoarding> VoiceOnBoardings { get; set; }
        //public DbSet<VoiceOnBoardingSubSteps> VoiceOnBoardingSubSteps { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // optionsBuilder.UseSqlServer(DataSeeder.ConnectionString);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:ParasaleConnectionString"]);
        }
    }
}