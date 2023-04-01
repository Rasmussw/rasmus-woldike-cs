class Club
{
    public string abbreviations { get; set; }
    public string fullName { get; set; }
    public string specialRanking { get; set; }

    public Club(string abbreviations, string fullName, string specialRanking)
    {
        this.abbreviations = abbreviations;
        this.fullName = fullName;
        this.specialRanking = specialRanking;
    }
}