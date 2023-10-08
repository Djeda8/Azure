public class GameStat
{
    public string Id { get; set; }

    public string Sport { get; set; }

    public DateTimeOffset DatePlayed { get; set; }

    public string Game { get; set; }

    public IReadOnlyList<string> Teams { get; set; }
    public IReadOnlyList<(string team, int score)> Results { get; set; }

    public GameStat(string sport, DateTimeOffset datePlayed, string game, string[] teams, IEnumerable<(string team, int score)> results)
    {
        Id = Guid.NewGuid().ToString();
        Sport = sport;
        DatePlayed = datePlayed;
        Game = game;
        Teams = teams;
        Results = (IReadOnlyList<(string team, int score)>?)results;
    }
}