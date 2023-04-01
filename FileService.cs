class FileService
{
    public List<Club> getClubs(string filePath)
    {
        //mandatory-setup.csv
        var reader = File.OpenText(filePath);
        var clubs = new List<Club>();

        int lineCount = 0;
        try
        {

            while (File.OpenText(filePath).ReadLine() != null)
            {
                var line = reader.ReadLine();
                var values = line.Split(";");
                clubs.Add(new Club(values[0], values[1], values[2]));
                lineCount++;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("fejl " + e.StackTrace);
        }
        return clubs;
    }

    public Dictionary<string, Standing> getStanding(List<Club> clubs)
    {
        var standings = new Dictionary<string, Standing>();
        for (int i = 1; i < clubs.Count; i++)
        {
            standings.Add(clubs[i].abbreviations, null);
        }
        return standings;
    }

    public Dictionary<string, Standing> calculateStanding(string pathToRounds, List<Club> clubs, Dictionary<string, Standing> standings)
    {
        //få fat i alle ruonds filerne
        string[] rounds = Directory.GetFiles(pathToRounds);

        try
        {
            for (int i = 0; i < rounds.Length; i++)
            {
                var li = File.OpenText(rounds[i]);
                var s = File.OpenText(rounds[i]);

                while (s.ReadLine() != null)
                {
                    var line = li.ReadLine();
                    var values = line.Split(";");

                    if (standings.ContainsKey(values[0]) && standings[values[0]] == null)
                    {
                        standings = firstTimeCalculatingClub(values, clubs, standings, 0);
                    }
                    else if (standings.ContainsKey(values[0]) && standings[values[0]] != null)
                    {
                        standings = notFirstTimeCalculatingClub(values, clubs, standings, 0);
                    }

                    if (standings.ContainsKey(values[1]) && standings[values[1]] == null)
                    {
                        standings = firstTimeCalculatingClub(values, clubs, standings, 1);
                    }
                    else if (standings.ContainsKey(values[1]) && standings[values[1]] != null)
                    {
                        standings = notFirstTimeCalculatingClub(values, clubs, standings, 1);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("fejl " + e.GetType().Name + " " + e.Message + " " + e.StackTrace);
        }
        return standings;
    }
    public Dictionary<string, Standing> firstTimeCalculatingClub(string[] roundValues, List<Club> clubs, Dictionary<string, Standing> standings, int firstOrSecondTeam)
    {
        int num = 0;
        if (firstOrSecondTeam == 0)
        {
            num = 1;
        }
        int won = 0;
        int lost = 0;
        int draw = 0;
        string[] goals = roundValues[2].Split("-");

        if (int.Parse(goals[firstOrSecondTeam]) > int.Parse(goals[num]))
        {
            won = +1;
        }
        else if (int.Parse(goals[num]) > int.Parse(goals[firstOrSecondTeam]))
        {
            lost = +1;
        }
        else if (int.Parse(goals[num]) == int.Parse(goals[firstOrSecondTeam]))
        {
            draw = +1;
        }
        int markIndex = clubs.FindIndex(c => c.abbreviations == roundValues[firstOrSecondTeam]);
        standings[roundValues[firstOrSecondTeam]] = new Standing(0, clubs[markIndex].specialRanking, roundValues[firstOrSecondTeam], 1, won, draw, lost, int.Parse(goals[firstOrSecondTeam]), int.Parse(goals[num]));
        return standings;
    }

    public Dictionary<string, Standing> notFirstTimeCalculatingClub(string[] roundValues, List<Club> clubs, Dictionary<string, Standing> standings, int firstOrSecondTeam)
    {
        int num = 0;
        if (firstOrSecondTeam == 0)
        {
            num = 1;
        }
        int won = 0;
        int lost = 0;
        int draw = 0;
        string[] goals = roundValues[2].Split("-");

        if (int.Parse(goals[firstOrSecondTeam]) > int.Parse(goals[num]))
        {
            won = +1;
        }
        else if (int.Parse(goals[num]) > int.Parse(goals[firstOrSecondTeam]))
        {
            lost = +1;
        }
        else if (int.Parse(goals[num]) == int.Parse(goals[firstOrSecondTeam]))
        {
            draw = +1;
        }

        standings[roundValues[firstOrSecondTeam]].games = standings[roundValues[firstOrSecondTeam]].games + 1;
        standings[roundValues[firstOrSecondTeam]].draw = standings[roundValues[firstOrSecondTeam]].draw + draw;
        standings[roundValues[firstOrSecondTeam]].won = standings[roundValues[firstOrSecondTeam]].won + won;
        standings[roundValues[firstOrSecondTeam]].lost = standings[roundValues[firstOrSecondTeam]].lost + lost;
        standings[roundValues[firstOrSecondTeam]].goalsFor = standings[roundValues[firstOrSecondTeam]].goalsFor + int.Parse(goals[firstOrSecondTeam]);
        standings[roundValues[firstOrSecondTeam]].goalsAgainst = standings[roundValues[firstOrSecondTeam]].goalsAgainst + int.Parse(goals[num]);
        standings[roundValues[firstOrSecondTeam]].goalDif = standings[roundValues[firstOrSecondTeam]].goalDif + int.Parse(goals[firstOrSecondTeam]) - int.Parse(goals[num]);
        standings[roundValues[firstOrSecondTeam]].points = standings[roundValues[firstOrSecondTeam]].points + draw + won * 3;
        if (won == 1)
        {
            standings[roundValues[firstOrSecondTeam]].streak = standings[roundValues[firstOrSecondTeam]].streak + "W";
        }
        else if (draw == 1)
        {
            standings[roundValues[firstOrSecondTeam]].streak = standings[roundValues[firstOrSecondTeam]].streak + "D";
        }
        else
        {
            standings[roundValues[firstOrSecondTeam]].streak = standings[roundValues[firstOrSecondTeam]].streak + "L";
        }
        return standings;
    }

    public List<Standing> getSortedListOfStanding(List<Club> clubs, Dictionary<string, Standing> standings)
    {
        List<Standing> finalStanding = new List<Standing>();

        for (int i = 1; i < clubs.Count; i++)
        {
            if (standings[clubs[i].abbreviations].marking == "-")
            {
                standings[clubs[i].abbreviations].marking = "   ";
            }
            finalStanding.Add(standings[clubs[i].abbreviations]);
        }

        finalStanding = finalStanding.OrderBy(s => s.points).ThenBy(s => s.goalDif).ThenBy(s => s.goalsFor).ThenBy(s => s.goalsAgainst).ThenBy(s => s.fullName).ToList();
        finalStanding.Reverse();
        finalStanding.ForEach(s => s.position = finalStanding.IndexOf(s) + 1);
        return finalStanding;
    }

    public void printStandingWithColorToConsole(string leagueSetupFilePath, List<Standing> finalStanding)
    {
        //"mandatory1-superliga-setup.csv"
        var setupFile = File.OpenText(leagueSetupFilePath);
        setupFile.ReadLine();
        string[] setup = setupFile.ReadLine().Split(";");
        League superliga = new League(setup[0], int.Parse(setup[1]), int.Parse(setup[2]),
        int.Parse(setup[3]), int.Parse(setup[4]), int.Parse(setup[5]));


        Console.WriteLine("position klub mark games points won draw lost goalsfor goalsagainst goalsdif streak");
        foreach (var item in finalStanding)
        {
            if (item.position <= superliga.championsLeague || item.position <= superliga.promotion)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(item.toString());
            }
            else if (item.position > superliga.championsLeague && item.position <= superliga.championsLeague + superliga.europeLeague)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(item.toString());
            }
            else if (item.position > superliga.championsLeague + superliga.europeLeague && item.position <= superliga.championsLeague + superliga.europeLeague + superliga.confreanceLeague)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(item.toString());
            }
            else if (finalStanding.Count - item.position < superliga.relegation)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(item.toString());
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(item.toString());
            }
        }
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void writeStandingToFile(string filePath, List<Standing> standings)
    {
        //position;club;mark;games;points;won;draws;lost;goalsfor;goalsagainst;goalsdiff;streak
        File.WriteAllText(filePath, "position;club;mark;games;points;won;draws;lost;goalsfor;goalsagainst;goalsdiff;streak");
        File.AppendAllText(filePath, "\n");
        var standingFile = File.OpenText(filePath);
        string s = "";
        for (int i = 0; i < standings.Count; i++)
        {
            s += standings[i].stringToCSVFile();
        }
        File.AppendAllText(filePath, s);
    }
}