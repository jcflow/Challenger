using System;
using GraphQL.Types;
using Models;
using Repository;
using Schema.Types;

namespace Schema
{
    public class ChallengerMutation : ObjectGraphType
    {
        public ChallengerMutation(ITournamentRepository tournamentRepository,
            ITeamRepository teamRepository,
            IBracketRepository bracketRepository,
            IScoreRepository scoreRepository)
        {
            Field<TournamentType>(
               "createTournament",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<TournamentInputType>> { Name = "tournament" },
                   new QueryArgument<NonNullGraphType<ListGraphType<TeamInputType>>> { Name = "teams" }),
               resolve: context =>
               {
                   var tournament = context.GetArgument<Tournament>("tournament");
                   var teamsNames = context.GetArgument<Team[]>("teams");
                   var initialLevel = (int) Math.Log2(teamsNames.Length) - 1;
                   var createdTournament = tournamentRepository.InsertTournament(tournament);
                   for (int i = 0; i + 1 < teamsNames.Length; i += 2)
                   {
                       var team1 = new Team
                       {
                           Name = teamsNames[i].Name
                       };
                       var team2 = new Team
                       {
                           Name = teamsNames[i + 1].Name
                       };
                       var createdTeam1 = teamRepository.InsertTeam(team1);
                       var createdTeam2 = teamRepository.InsertTeam(team2);
                       if (i % 2 == 0)
                       {
                           var bracket = new Bracket
                           {
                               Tournament = createdTournament,
                               Level = initialLevel,
                               Finished = false,
                               TournamentID = createdTournament.ID
                           };
                           var createdBracket = bracketRepository.InsertBracket(bracket);
                           var score1 = new Score
                           {
                               Value = 0,
                               Team = createdTeam1,
                               TeamID = createdTeam1.ID,
                               Bracket = createdBracket,
                               BracketID = createdBracket.ID
                           };
                           var score2 = new Score
                           {
                               Value = 0,
                               Team = createdTeam2,
                               TeamID = createdTeam2.ID,
                               Bracket = createdBracket,
                               BracketID = createdBracket.ID
                           };
                           scoreRepository.InsertScore(score1);
                           scoreRepository.InsertScore(score2);
                       }
                   }
                   return createdTournament;
               }
           );
        }
    }
}
