using System.Linq;
Console.ForegroundColor = ConsoleColor.White;

//få hvor mange linjer
var reader = File.OpenText("mandatory-setup.csv");
var abbreviations = new List<String>();
var fullName = new List<String>();
var specialRanking = new List<String>();

int lineCount = 0;
try
{

    while (File.OpenText("mandatory-setup.csv").ReadLine() != null)
    {
        var line = reader.ReadLine();
        var values = line.Split(";");
        abbreviations.Add(values[0]);
        fullName.Add(values[1]);
        specialRanking.Add(values[2]);
        lineCount++;
    }
}
catch (Exception e)
{
    Console.WriteLine("fejl " + e.StackTrace);
}

var standings = new Dictionary<string, Standing>();
for (int i = 1; i < abbreviations.Count; i++)
{
    standings.Add(abbreviations[i], null);
}

//få fat i alle ruonds filerne
string[] rounds = Directory.GetFiles("rounds");

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
                int won = 0;
                int lost = 0;
                int draw = 0;
                string[] goals = values[2].Split("-");

                if (int.Parse(goals[0]) > int.Parse(goals[1]))
                {
                    won = +1;
                }
                else if (int.Parse(goals[1]) > int.Parse(goals[0]))
                {
                    lost = +1;
                }
                else if (int.Parse(goals[1]) == int.Parse(goals[0]))
                {
                    draw = +1;
                }
                int markIndex = abbreviations.IndexOf(values[0]);

                standings[values[0]] = new Standing(0, specialRanking[markIndex], values[0], 1, won, draw, lost, int.Parse(goals[0]), int.Parse(goals[1]));
            }
            else if (standings.ContainsKey(values[0]) && standings[values[0]] != null)
            {
                int won = 0;
                int lost = 0;
                int draw = 0;
                string[] goals = values[2].Split("-");

                if (int.Parse(goals[0]) > int.Parse(goals[1]))
                {
                    won = +1;
                }
                else if (int.Parse(goals[1]) > int.Parse(goals[0]))
                {
                    lost = +1;
                }
                else if (int.Parse(goals[1]) == int.Parse(goals[0]))
                {
                    draw = +1;
                }

                standings[values[0]].games = standings[values[0]].games + 1;
                standings[values[0]].draw = standings[values[0]].draw + draw;
                standings[values[0]].won = standings[values[0]].won + won;
                standings[values[0]].lost = standings[values[0]].lost + lost;
                standings[values[0]].goalsFor = standings[values[0]].goalsFor + int.Parse(goals[0]);
                standings[values[0]].goalsAgainst = standings[values[0]].goalsAgainst + int.Parse(goals[1]);
                standings[values[0]].goalDif = standings[values[0]].goalDif + int.Parse(goals[0]) - int.Parse(goals[1]);
                standings[values[0]].points = standings[values[0]].points + draw + won * 3;
                if (won == 1)
                {
                    standings[values[0]].streak = standings[values[0]].streak + "W";
                }
                else if (draw == 1)
                {
                    standings[values[0]].streak = standings[values[0]].streak + "D";
                }
                else
                {
                    standings[values[0]].streak = standings[values[0]].streak + "L";
                }
            }


            if (standings.ContainsKey(values[1]) && standings[values[1]] == null)
            {
                int won = 0;
                int lost = 0;
                int draw = 0;
                string[] goals = values[2].Split("-");

                if (int.Parse(goals[0]) > int.Parse(goals[1]))
                {
                    lost = +1;
                }
                else if (int.Parse(goals[1]) > int.Parse(goals[0]))
                {
                    won = +1;
                }
                else if (int.Parse(goals[1]) == int.Parse(goals[0]))
                {
                    draw = +1;
                }
                int markIndex = abbreviations.IndexOf(values[1]);
                standings[values[1]] = new Standing(0, specialRanking[markIndex], values[1], 1, won, draw, lost, int.Parse(goals[1]), int.Parse(goals[0]));
            }
            else if (standings.ContainsKey(values[1]) && standings[values[1]] != null)
            {

                int won = 0;
                int lost = 0;
                int draw = 0;
                string[] goals = values[2].Split("-");

                if (int.Parse(goals[0]) > int.Parse(goals[1]))
                {
                    lost = +1;
                }
                else if (int.Parse(goals[1]) > int.Parse(goals[0]))
                {
                    won = +1;
                }
                else if (int.Parse(goals[1]) == int.Parse(goals[0]))
                {
                    draw = +1;
                }

                standings[values[1]].games = standings[values[1]].games + 1;
                standings[values[1]].draw = standings[values[1]].draw + draw;
                standings[values[1]].won = standings[values[1]].won + won;
                standings[values[1]].lost = standings[values[1]].lost + lost;
                standings[values[1]].goalsFor = standings[values[1]].goalsFor + int.Parse(goals[1]);
                standings[values[1]].goalsAgainst = standings[values[1]].goalsAgainst + int.Parse(goals[0]);
                standings[values[1]].goalDif = standings[values[1]].goalDif + int.Parse(goals[1]) - int.Parse(goals[0]);
                standings[values[1]].points = standings[values[1]].points + draw + won * 3;
                if (won == 1)
                {
                    standings[values[1]].streak = standings[values[1]].streak + "W";
                }
                else if (draw == 1)
                {
                    standings[values[1]].streak = standings[values[1]].streak + "D";
                }
                else
                {
                    standings[values[1]].streak = standings[values[1]].streak + "L";
                }
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine("fejl " + e.GetType().Name + " " + e.Message + " " + e.StackTrace);
}

List<Standing> finalStanding = new List<Standing>();

for (int i = 1; i < abbreviations.Count; i++)
{
    if (standings[abbreviations[i]].marking == "-")
    {
        standings[abbreviations[i]].marking = "   ";
    }
    finalStanding.Add(standings[abbreviations[i]]);
}

finalStanding = finalStanding.OrderBy(s => s.points).ThenBy(s => s.goalDif).ThenBy(s => s.goalsFor).ThenBy(s => s.goalsAgainst).ThenBy(s => s.fullName).ToList();
finalStanding.Reverse();
finalStanding.ForEach(s => s.position = finalStanding.IndexOf(s) + 1);

//finalStanding.ForEach(s => Console.WriteLine(s.toString()));


var setupFile = File.OpenText("mandatory1-superliga-setup.csv");
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