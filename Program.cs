using System.Linq;
//farven i consollen starter med at være 4
Console.ForegroundColor = ConsoleColor.White;
//laver et object af FileService klassen
FileService fileService = new FileService();

//superliagen
//finder alle klubberne
List<Club> clubs = fileService.getClubs("mandatory-setup.csv");
//laver et dictionary med holdene og stillingen for holdet
Dictionary<string, Standing> standings = fileService.getStanding(clubs);
//regner stillingen ud, ud fra holdene og runderne
standings = fileService.calculateStanding("rounds", clubs, standings);
//sortere standingsene ud fra point, målforskel osv.
List<Standing> finalStanding = fileService.getSortedListOfStanding(clubs, standings);
//printer standingsene ud fra setupfilen(hvilken) turnering det er, og farver de forskellige positioner ud fra oprykning, nedrykning osv
fileService.printStandingWithColorToConsole("mandatory1-superliga-setup.csv", finalStanding);
//udskriver/opdatere standings til en fil
fileService.writeStandingToFile("mandatory-standing.csv", finalStanding);

//nordicbet ligaen
//finder alle klubberne
List<Club> nordicbetClubs = fileService.getClubs("nordicbet-setup.csv");
//laver et dictionary med holdene og stillingen for holdet
Dictionary<string, Standing> nordicbetStandings = fileService.getStanding(nordicbetClubs);
//regner stillingen ud, ud fra holdene og runderne
standings = fileService.calculateStanding("Nb-rounds", nordicbetClubs, nordicbetStandings);
//sortere standingsene ud fra point, målforskel osv.
List<Standing> nordicbetFinalStanding = fileService.getSortedListOfStanding(nordicbetClubs, nordicbetStandings);
//printer standingsene ud fra setupfilen(hvilken) turnering det er, og farver de forskellige positioner ud fra oprykning, nedrykning osv
fileService.printStandingWithColorToConsole("mandatory1-nordicbet-setup.csv", nordicbetFinalStanding);
//udskriver/opdatere standings til en fil
fileService.writeStandingToFile("nordicbet-standing.csv", nordicbetFinalStanding);

