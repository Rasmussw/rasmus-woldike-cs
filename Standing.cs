class Standing
{

    public int position { get; set; }
    public string marking { get; set; }
    public string fullName { get; set; }
    public int games { get; set; }
    public int won { get; set; }
    public int draw { get; set; }
    public int lost { get; set; }
    public int goalsFor { get; set; }
    public int goalsAgainst { get; set; }
    public int goalDif { get; set; } //= goalsFor - goalsAgainst
    public int points { get; set; } // = won * 3 + draw * 1
    public string streak { get; set; }

    public Standing(int position, string marking, string fullName, int games, int won,
    int draw, int lost, int goalsFor, int goalsAgainst)
    {
        this.position = position;
        this.marking = marking;
        this.fullName = fullName;
        this.games = games;
        this.won = won;
        this.draw = draw;
        this.lost = lost;
        this.goalsFor = goalsFor;
        this.goalsAgainst = goalsAgainst;
        this.goalDif = goalsFor - goalsAgainst;
        this.points = won * 3 + draw * 1;
        if (won == 1)
        {
            this.streak = "W";
        }
        else if (draw == 1)
        {
            this.streak = "D";
        }
        else
        {
            this.streak = "L";
        }
    }

    public Standing() { }


    public string toString()
    {
        // får at få præsentatioenen til at se godt ud
        string s = "";
        if (position <= 9)
        {
            s = " ";
        }

        string n = "";
        if (fullName.Length < 3)
        {
            n = " ";
        }

        return position + s + " " + fullName + marking + n + " " + games + " " + points + " " +
        won + " " + draw + " " + lost + " " + goalsFor + " " + goalsAgainst + " " +
        goalDif + " " + streak.Substring(streak.Length - 5);
    }


}
