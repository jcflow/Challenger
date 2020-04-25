using System;

namespace Models
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new ChallengerContext())
            {

                Console.WriteLine("Inserting users...");
                var user = new User { Name = "juan", Password = "Juan123" };
                context.Users.Add(user);
                context.SaveChanges();

                Console.WriteLine("Inserting categories...");
                var category1 = new TournamentCategory { Name = "DotA" };
                var category2 = new TournamentCategory { Name = "Soccer" };
                var category3 = new TournamentCategory { Name = "Yu-Gi-Oh!" };
                var category4 = new TournamentCategory { Name = "Darts" };
                var category5 = new TournamentCategory { Name = "Chess" };
                var category6 = new TournamentCategory { Name = "Super Smash Bros. Ultimate" };
                var category7 = new TournamentCategory { Name = "FIFA 20" };
                var category8 = new TournamentCategory { Name = "Call of Duty: Modern Warfare" };
                var category9 = new TournamentCategory { Name = "Pokémon" };
                var category10 = new TournamentCategory { Name = "Volleyball" };
                context.TournamentCategories.Add(category1);
                context.TournamentCategories.Add(category2);
                context.TournamentCategories.Add(category3);
                context.TournamentCategories.Add(category4);
                context.TournamentCategories.Add(category5);
                context.TournamentCategories.Add(category6);
                context.TournamentCategories.Add(category7);
                context.TournamentCategories.Add(category8);
                context.TournamentCategories.Add(category9);
                context.TournamentCategories.Add(category10);
                context.SaveChanges();

                Console.WriteLine("Inserting tournaments...");
                var tournament = new Tournament { Name = "Company League", Category = category1, Administrator = user };
                context.Tournaments.Add(tournament);
                context.SaveChanges();

                Console.WriteLine("Inserting teams...");
                var team1 = new Team { Name = "Junior Developers" };
                var team2 = new Team { Name = "Mid-Senior Developers" };
                var team3 = new Team { Name = "Senior Developers" };
                var team4 = new Team { Name = "QAs" };
                var team5 = new Team { Name = "DevOps" };
                var team6 = new Team { Name = "Human Resources" };
                var team7 = new Team { Name = "Managers" };
                var team8 = new Team { Name = "IT" };
                context.Teams.Add(team1);
                context.Teams.Add(team2);
                context.Teams.Add(team3);
                context.Teams.Add(team4);
                context.Teams.Add(team5);
                context.Teams.Add(team6);
                context.Teams.Add(team7);
                context.Teams.Add(team8);
                context.SaveChanges();

                Console.WriteLine("Inserting brackets...");
                var bracket21 = new Bracket { Tournament = tournament, Level = 2, Finished = true };
                var bracket22 = new Bracket { Tournament = tournament, Level = 2, Finished = true };
                var bracket23 = new Bracket { Tournament = tournament, Level = 2, Finished = true };
                var bracket24 = new Bracket { Tournament = tournament, Level = 2, Finished = true };

                var bracket11 = new Bracket { Tournament = tournament, Level = 1, Finished = true };
                var bracket12 = new Bracket { Tournament = tournament, Level = 1, Finished = true };

                var bracket0 = new Bracket { Tournament = tournament, Level = 0, Finished = true };

                context.Brackets.Add(bracket21);
                context.Brackets.Add(bracket22);
                context.Brackets.Add(bracket23);
                context.Brackets.Add(bracket24);
                context.Brackets.Add(bracket11);
                context.Brackets.Add(bracket12);
                context.Brackets.Add(bracket0);
                context.SaveChanges();

                Console.WriteLine("Inserting scores...");
                context.Scores.Add(new Score { Bracket = bracket21, Team = team1, Value = 3 });
                context.Scores.Add(new Score { Bracket = bracket21, Team = team2, Value = 2 });
                context.Scores.Add(new Score { Bracket = bracket22, Team = team3, Value = 5 });
                context.Scores.Add(new Score { Bracket = bracket22, Team = team4, Value = 1 });
                context.Scores.Add(new Score { Bracket = bracket23, Team = team5, Value = 7 });
                context.Scores.Add(new Score { Bracket = bracket23, Team = team6, Value = 0 });
                context.Scores.Add(new Score { Bracket = bracket24, Team = team7, Value = 2 });
                context.Scores.Add(new Score { Bracket = bracket24, Team = team8, Value = 3 });

                context.Scores.Add(new Score { Bracket = bracket11, Team = team1, Value = 5 });
                context.Scores.Add(new Score { Bracket = bracket11, Team = team3, Value = 2 });
                context.Scores.Add(new Score { Bracket = bracket12, Team = team5, Value = 4 });
                context.Scores.Add(new Score { Bracket = bracket12, Team = team8, Value = 2 });

                context.Scores.Add(new Score { Bracket = bracket0, Team = team1, Value = 3 });
                context.Scores.Add(new Score { Bracket = bracket0, Team = team5, Value = 0 });
                context.SaveChanges();
            }
        }
    }
}
