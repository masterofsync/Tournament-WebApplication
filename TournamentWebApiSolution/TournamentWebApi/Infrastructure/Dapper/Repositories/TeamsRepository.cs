using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class TeamsRepository : Repository, ITeamsRepository
    {
        public TeamsRepository(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Add a new Team to the Team table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> AddTeamAsync(TeamContractModel model)
        {
            try
            {
                var teamStatsId = await CreateDefaultTeamStats();
                this.StartTransaction();

                model.TeamStatsId = teamStatsId;

                string insertQuery = @"INSERT INTO [dbo].[Team]([Name], [Description], [TeamStatsId],[UserId]) VALUES (@Name, @Description, @TeamStatsId,@UserId)";
                // save the team model
                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(insertQuery, model);

                this.CommitTransaction();

                // if rows affected (item created)
                if (rowsAffected > 0)
                    return new OkResult();
                else
                {
                    return new BadRequestResult();
                }

            }
            catch (Exception)
            {
                this.RollbackTransaction(); // rollback & close
                //return false; ??
                throw;
            }
        }

        /// <summary>
        /// Delete team given id
        /// </summary>
        /// <param name="id">integer id</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> DeleteTeamAsync(int id)
        {
            try
            {
                this.StartTransaction();
                string deleteQuery = @"DELETE FROM [dbo].[Team] WHERE TeamId = @Id";

                var rowsAffected = await SaveDataInTransactionUsingQueryAsync(deleteQuery, new { Id = id });
                this.CommitTransaction();

                // if rows affected (item deleted)
                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
            catch (Exception)
            {
                this.RollbackTransaction(); // rollback & close
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">integer id</param>
        /// <returns>TeamContractModel</returns>
        public async Task<TeamContractModel> GetTeamAsync(int id)
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT * FROM[dbo].[Team] WHERE TeamId=@Id";

                var result = await LoadSingleDataInTransactionUsingQueryAsync<TeamContractModel, dynamic>(getQuery, new { Id = id });
                this.CommitTransaction();

                return result;
            }
            catch (Exception)
            {
                this.RollbackTransaction(); // rollback & close
                throw;
            }
        }

        /// <summary>
        /// Update team data
        /// </summary>
        /// <param name="model">integer id</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> UpdateTeamAsync(TeamContractModel model)
        {
            try
            {
                this.StartTransaction();

                string updateQuery = @"UPDATE [dbo].[Team] SET Name=@Name, Description=@Description WHERE TeamId=@TeamId";

                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(updateQuery, model);

                this.CommitTransaction();

                // if rows affected (item created)
                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
            catch (Exception)
            {
                this.RollbackTransaction(); // rollback & close
                throw;
            }
        }

        /// <summary>
        /// Get all team related to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TeamContractModel>> GetAllTeamsForUser(string userId)
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT * FROM [dbo].[Team] WHERE UserId=@Id";

                // save the team model
                var result = await LoadDataInTransactionUsingQueryAsync<TeamContractModel, dynamic>(getQuery, new { Id = userId });

                this.CommitTransaction();
                return result;
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                //return false; ??
                throw;
            }
        }

        /// <summary>
        /// Get all team related to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> CreateDefaultTeamStats()
        {
            try
            {
                var defaultTeamStatsModel = new TeamStatsContractModel() { Draw = 0, goalsFor = 0, goalsAgainst = 0, Loss = 0, matchesPlayed = 0, Wins = 0 };
                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[TeamStats]([Wins], [Loss], [Draw],[matchesPlayed],[goalsFor],[goalsAgainst]) VALUES (@Wins, @Loss, @Draw, @matchesPlayed, @goalsFor, @goalsAgainst); SELECT CAST(SCOPE_IDENTITY() as int)";

                // save the team model
                var TeamStatsId = await this.SaveDataInTransactionAndGetIdAsync(insertQuery, defaultTeamStatsModel);

                this.CommitTransaction();

                return TeamStatsId;
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                //return false; ??
                throw;
            }
        }

        ///// <summary>
        ///// Get all teams for given user and sport.
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="sportId"></param>
        ///// <returns></returns>
        //public async Task<IEnumerable<TeamContractModel>> GetAllTeamsForUserAndSport(int userId, int sportId)
        //{
        //    try
        //    {
        //        this.StartTransaction();

        //        string getQuery = @"SELECT * FROM [dbo].[Team] WHERE UserId=@UserId AND SportId=@SportId";

        //        // save the team model
        //        var result = await LoadDataInTransactionUsingQueryAsync<TeamContractModel, dynamic>(getQuery, new { UserId = userId, SportId = sportId });

        //        this.CommitTransaction();
        //        return result;
        //    }
        //    catch (Exception)
        //    {
        //        this.RollbackTransaction();
        //        //return false; ??
        //        throw;
        //    }
        //}
    }
}
