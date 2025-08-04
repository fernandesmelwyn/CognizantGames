using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public enum GameFormat
    {
        MensSingles,
        WomensSingles,
        MensDoubles,
        WomensDoubles,
        MixedDoubles
    }

    public enum TimeSlot
    {
        Morning,
        Evening,
        Night
    }

    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }

    public class GameFormatType
    {
        [Key]
        public int GameFormatTypeId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; // e.g., Men's Singles, Women's Doubles, etc.
        public ICollection<Game>? Games { get; set; }
    }

    public class Game
    {
        [Key]
        public int GameId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty; // Carrom, Chess, Table Tennis
        public ICollection<GameFormatType>? SupportedFormats { get; set; }
    }

    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }
        [Required]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        [Required]
        public int GameId { get; set; }
        public Game? Game { get; set; }
        [Required]
        public GameFormat Format { get; set; }
        public int? PartnerEmployeeId { get; set; } // For Doubles/MixedDoubles
        public Employee? PartnerEmployee { get; set; }
        [Required]
        public TimeSlot PreferredTimeSlot { get; set; }
        public bool IsTeamNomination { get; set; } // True for team games
    }

    public class Fixture
    {
        [Key]
        public int FixtureId { get; set; }
        [Required]
        public int GameId { get; set; }
        public Game? Game { get; set; }
        [Required]
        public GameFormat Format { get; set; }
        public DateTime ScheduledTime { get; set; }
        public List<Registration>? Registrations { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsBye { get; set; } // If true, player/team gets a bye
        public int? WinnerRegistrationId { get; set; } // Winner of the fixture
        public Registration? Winner { get; set; }
        public bool IsKnockout { get; set; } // True for knockout matches
        public int NumberOfGames { get; set; } = 1; // Default single game, set to 3 for semi/final in Chess
    }

    public class Score
    {
        [Key]
        public int ScoreId { get; set; }
        [Required]
        public int FixtureId { get; set; }
        public Fixture? Fixture { get; set; }
        public int Team1Score { get; set; }
        public int Team2Score { get; set; }
    }

    public class LeaderboardEntry
    {
        [Key]
        public int LeaderboardEntryId { get; set; }
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public int GameId { get; set; }
        public Game? Game { get; set; }
        public GameFormat Format { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
    }
}
