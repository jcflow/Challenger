using System;
using System.Collections.Generic;
using System.Linq;
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
                   new QueryArgument<NonNullGraphType<ListGraphType<TeamInputType>>> { Name = "teams" },
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "categoryId" }),
               resolve: context =>
               {
                   var tournament = context.GetArgument<Tournament>("tournament");
                   var teamsNames = context.GetArgument<Team[]>("teams");
                   var categoryId = context.GetArgument<int>("categoryId");
                   tournament.CategoryID = categoryId;
                   var createdTournament = tournamentRepository.InsertTournament(tournament);

                   var initialLevel = (int)Math.Log2(teamsNames.Length) - 1;
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
               });
            Field<ScoreType>(
               "updateScore",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "value" }),
               resolve: context =>
               {
                   var scoreId = context.GetArgument<int>("id");
                   var newValue = context.GetArgument<int>("value");
                   var score = scoreRepository.GetScoreByID(scoreId);
                   var bracket = bracketRepository.GetBracketByID(score.BracketID);
                   if (!bracket.Finished && newValue >= 0)
                   {
                       score.Value = newValue;
                       scoreRepository.UpdateScore(score);
                   }
                   return score;
               });
            Field<BracketType>(
               "updateBracket",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" },
                   new QueryArgument<NonNullGraphType<BooleanGraphType>> { Name = "finished" }),
               resolve: context =>
               {
                   var bracketId = context.GetArgument<int>("id");
                   var finished = context.GetArgument<bool>("finished");
                   var bracket = bracketRepository.GetBracketByID(bracketId);
                   if (!bracket.Finished)
                   {
                       bracket.Finished = finished;
                       bracketRepository.UpdateBracket(bracket);
                   }
                   var brackets = (List<Bracket>)bracketRepository.GetBrackets(_ => _.TournamentID == bracket.TournamentID);
                   var notFinished = brackets.Where(b => b.Finished == false).ToList();
                   if (brackets.Any() && !notFinished.Any())
                   {
                       var lowestLevel = brackets.OrderBy(b => b.Level).First().Level;
                       if (lowestLevel > 0)
                       {
                           var filteredBrackets = brackets.Where(b => b.Level == lowestLevel).ToList();
                           for (var index = 0; index + 1 < filteredBrackets.Count; index += 2)
                           {
                               var newBracket = new Bracket()
                               {
                                   Level = lowestLevel - 1,
                                   Finished = false,
                                   TournamentID = bracket.TournamentID
                               };
                               var createdBracket = bracketRepository.InsertBracket(newBracket);

                               var bracketA = filteredBrackets[index];
                               var bracketB = filteredBrackets[index + 1];
                               var winnerAID = FindWinnerID(bracketA.ID, scoreRepository);
                               var winnerBID = FindWinnerID(bracketB.ID, scoreRepository);
                               var winnerIDs = new int[] { winnerAID, winnerBID };
                               var teamsPerBracket = 2;
                               for (var x = 0; x < teamsPerBracket; x++)
                               {
                                   var newScore = new Score()
                                   {
                                       BracketID = createdBracket.ID,
                                       TeamID = winnerIDs[x],
                                       Value = 0
                                   };
                                   scoreRepository.InsertScore(newScore);
                               }
                           }
                       }
                   }
                   return bracket;
               });
        }

        public static int FindWinnerID(int bracketId, IScoreRepository scoreRepository)
        {
            var scores = (List<Score>) scoreRepository.GetScores(_ => _.BracketID == bracketId);
            var winner = scores.OrderByDescending(s => s.Value).First();
            return winner.TeamID;
        }
    }
}
