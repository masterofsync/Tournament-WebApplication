using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class TournamentsRepository : Repository, ITournamentsRepository
    {
        public TournamentsRepository(IConfiguration config) : base(config)
        {

        }


        /// <summary>
        /// Add a new Team to the Team table.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> AddTournamentAsync(TournamentContractModel model)
        {
            try
            {
                this.StartTransaction();

                var newModel = new
                {
                    Name = model.Name,
                    Description = model.Description,
                    SportId = model.Sport.SportId,
                    TournamentTypeId = model.TournamentType.TournamentTypeId,
                    TournamentPointSystemId = model.TournamentPointSystemIdContractModel.TournamentPointSystemId,
                    UserId = model.UserId
                };

                string insertQuery = @"INSERT INTO [dbo].[Tournament]([Name], [Description], [SportId], [TournamentTypeId], [TournamentPointSystemId], [UserId]) 
                                        VALUES (@Name, @Description, @SportId, @TournamentTypeId, @TournamentPointSystemId, @UserId)";
                // save the team model
                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(insertQuery, newModel);

                this.CommitTransaction();

                // if rows affected (item created)
                if (rowsAffected > 0)
                    return new OkResult();
                else
                {
                    return new BadRequestResult();
                }
            }
            catch (Exception ex)
            {
                this.RollbackTransaction(); // rollback & close
                //return false; ??
                throw;
            }
        }


        /// <summary>
        /// Get tournament data given id.
        /// </summary>
        /// <param name="tournamentId">integer tournamentId</param>
        /// <returns>TeamContractModel</returns>
        public async Task<TournamentContractModel> GetTournamentAsync(int tournamentId)
        {
            try
            {
                return null;

                this.StartTransaction();

                // TODO: USE Join to get data for all nested models in tournament
                // SELECT * FROM Tournament JOIN Sport ON (Tournament.SportId = Sport.SportId) JOIN TournamentType ON (Tournament.TournamentTypeId=TournamentType.TournamentTypeId) 
                // JOIN TournamentPointSystem ON (Tournament.TournamentPointSystemId =TournamentPointSystem.TournamentPointSystemId)  WHERE TournamentId=tournamentId

                string getQuery = @"SELECT * FROM[dbo].[Tournament] WHERE TournamentId=@Id";

                var result = await LoadSingleDataInTransactionUsingQueryAsync<TournamentContractModel, dynamic>(getQuery, new { Id = tournamentId });
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
        /// Get all tournaments related to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TournamentContractModel>> GetAllTournamentsForUser(string userId)
        {
            try
            {
                return null;
                this.StartTransaction();

                // TODO: GET all tournaments for the user with JOIN??? 
                string getQuery = @"SELECT * FROM [dbo].[Tournament] WHERE UserId=@Id";

                // save the team model
                var result = await LoadDataInTransactionUsingQueryAsync<TournamentContractModel, dynamic>(getQuery, new { Id = userId });

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
        public async Task<int> CreateSubmittedPointSystem(TournamentPointSystemIdContractModel model)
        {
            try
            {
                var defaultTeamStatsModel = model;
                if (model.DefaultPointSystem == true)
                {
                    defaultTeamStatsModel = new TournamentPointSystemIdContractModel() { Name = "Default", DrawPoint = 1, LossPoint = 0, Winpoint = 3 };
                }

                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[TournamentPointSystem]([Name], [WinPoint], [DrawPoint], [LossPoint]) VALUES (@Name, @WinPoint, @DrawPoint, @LossPoint); SELECT CAST(SCOPE_IDENTITY() as int)";

                // save the tournament Point System model
                var TournamentPointSystemId = await this.SaveDataInTransactionAndGetIdAsync(insertQuery, defaultTeamStatsModel);

                this.CommitTransaction();

                return TournamentPointSystemId;
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                //return false; ??
                throw;
            }
        }

        /// <summary>
        /// Get User Id of the associated tournament.
        /// </summary>
        /// <param name="tournamentId">integer tournament id</param>
        /// <returns>integer UserId for tournament</returns>
        public async Task<string> GetAssociatedUserIdForTournamentAsync(int tournamentId)
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT UserId FROM[dbo].[Tournament] WHERE TournamentId=@Id";

                var result = await LoadSingleDataInTransactionUsingQueryAsync<string, dynamic>(getQuery, new { Id = tournamentId });
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
        /// Get all team related to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public async Task<int> GetDefaultPointSystemId()
        //{
        //try
        //{
        //    this.StartTransaction();

        //    string getQuery = @"SELECT TournamentPointSystemId FROM [dbo].[TournamentPointSystem] WHERE Name='Default'";

        //    // save the team model
        //    var result = await LoadDataInTransactionUsingQueryAsync<int, dynamic>(getQuery, new { Id = userId });

        //    this.CommitTransaction();
        //    return result;
        //}
        //catch (Exception)
        //{
        //    this.RollbackTransaction();
        //    //return false; ??
        //    throw;
        //}
        //}


        #region Tournament type

        public async Task<IActionResult> AddTypeAsync(TournamentTypeContractModel model)
        {
            try
            {
                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[TournamentTypes]([Name],[Description]) VALUES (@Name,@Description)";

                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(insertQuery, model);

                this.CommitTransaction();

                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                throw;
            }
        }

        public async Task<IActionResult> DeleteTypeAsync(int id)
        {
            try
            {
                this.StartTransaction();

                string deleteQuery = @"DELETE FROM [dbo].[TournamentTypes] WHERE TournamentTypeId = @Id";

                var rowsAffected = await SaveDataInTransactionUsingQueryAsync(deleteQuery, new { Id = id });
                this.CommitTransaction();

                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();

            }
            catch (Exception)
            {
                this.RollbackTransaction();
                throw;
            }
        }

        public async Task<IEnumerable<TournamentTypeContractModel>> GetAllTypeAsync()
        {
            try
            {
                this.StartTransaction();

                var getAllTypeQuery = @"SELECT * FROM [dbo].[TournamentTypes]";

                var result = await LoadDataInTransactionUsingQueryAsync<TournamentTypeContractModel, dynamic>(getAllTypeQuery, null);

                this.CommitTransaction();

                return result;

            }
            catch (Exception)
            {
                this.RollbackTransaction();
                throw;
            }
        }

        public async Task<TournamentTypeContractModel> GetTypeAsync(int id)
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT * FROM [dbo].[TournamentTypes] WHERE TournamentTypeId=@Id";

                var result = await LoadSingleDataInTransactionUsingQueryAsync<TournamentTypeContractModel, dynamic>(getQuery, new { Id = id });

                this.CommitTransaction();

                return result;
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                throw;
            }
        }

        public async Task<IActionResult> UpdateTypeAsync(TournamentTypeContractModel model)
        {
            try
            {
                this.StartTransaction();

                string updateQuery = @"UPDATE [dbo].[TournamentTypes] SET Name=@Name,Description=@Description WHERE TournamentTypeId=@TournamentTypeId";

                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(updateQuery, model);

                this.CommitTransaction();

                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                throw;
            }
        }

        #endregion

    }
}
