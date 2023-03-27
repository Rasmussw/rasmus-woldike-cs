class League
{

    public string name { get; set; }
    public int championsLeague { get; set; }
    public int europeLeague { get; set; }
    public int confreanceLeague { get; set; }
    public int promotion { get; set; }
    public int relegation { get; set; }

    public League(string name, int championsLeague, int europeLeague,
    int confreanceLeague, int promotion, int relegation)
    {
        this.name = name;
        this.championsLeague = championsLeague;
        this.europeLeague = europeLeague;
        this.confreanceLeague = confreanceLeague;
        this.promotion = promotion;
        this.relegation = relegation;
    }
}

