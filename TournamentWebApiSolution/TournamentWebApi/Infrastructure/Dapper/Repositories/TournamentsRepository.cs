﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;

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
                    TournamentPointSystemId = model.TournamentPointSystem.TournamentPointSystemId,
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


        //TODO :: GET and GETALL repository

        /// <summary>
        /// Get tournament data given id.
        /// </summary>
        /// <param name="tournamentId">integer tournamentId</param>
        /// <returns>TeamContractModel</returns>
        public async Task<TournamentContractModel> GetTournamentAsync(int tournamentId)
        {
            try
            {

                this.StartTransaction();

                // Sql Query to join and get all the nested object data.
                string getQuery = @"SELECT t.*, s.*,tt.*, tps.* FROM [dbo].[Tournament] t 
                                    LEFT JOIN Sport s ON (s.SportId=t.SportId)
                                    LEFT JOIN TournamentType tt ON (tt.TournamentTypeId=t.TournamentTypeId)
                                    LEFT JOIN TournamentPointSystem tps ON (tps.TournamentPointSystemId=t.TournamentPointSystemId)
                                    WHERE t.TournamentId=@Id";

                // expression to set all the required nested objects.
                var result = await GetConnection.QueryAsync<TournamentContractModel, SportContractModel, TournamentTypeContractModel, TournamentPointSystemContractModel, TournamentContractModel>(getQuery,
                            (t, s, tt, tps) =>
                            {
                                t.Sport = new SportContractModel();
                                t.Sport.SportId = s.SportId;
                                t.Sport.Description = s.Description;
                                t.Sport.Name = s.Name;
                                t.TournamentType = new TournamentTypeContractModel();
                                t.TournamentType.Name = tt.Name;
                                t.TournamentType.TournamentTypeId = tt.TournamentTypeId;
                                t.TournamentType.Description = tt.Description;
                                t.TournamentPointSystem = new TournamentPointSystemContractModel();
                                t.TournamentPointSystem.TournamentPointSystemId = tps.TournamentPointSystemId;
                                t.TournamentPointSystem.Name = tps.Name;
                                t.TournamentPointSystem.Winpoint = tps.Winpoint;
                                t.TournamentPointSystem.DrawPoint = tps.DrawPoint;
                                t.TournamentPointSystem.LossPoint = tps.LossPoint;
                                return t;
                            }, new { Id = tournamentId }, splitOn: "TournamentId,SportId,TournamentTypeId,TournamentPointSystemId", commandType: CommandType.Text, transaction: Transaction);

                this.CommitTransaction();

                return result.FirstOrDefault();
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
                this.StartTransaction();

                // Sql Query to join and get all the nested object data.
                string getQuery = @"SELECT t.*, s.*,tt.*, tps.* FROM [dbo].[Tournament] t 
                                    LEFT JOIN Sport s ON (s.SportId=t.SportId)
                                    LEFT JOIN TournamentType tt ON (tt.TournamentTypeId=t.TournamentTypeId)
                                    LEFT JOIN TournamentPointSystem tps ON (tps.TournamentPointSystemId=t.TournamentPointSystemId)
                                    WHERE t.UserId=@Id";

                // expression to set all the required nested objects.
                var result = await GetConnection.QueryAsync<TournamentContractModel, SportContractModel, TournamentTypeContractModel, TournamentPointSystemContractModel, TournamentContractModel>(getQuery,
                            (t, s, tt, tps) =>
                            {
                                t.Sport = new SportContractModel();
                                t.Sport.SportId = s.SportId;
                                t.Sport.Description = s.Description;
                                t.Sport.Name = s.Name;
                                t.TournamentType = new TournamentTypeContractModel();
                                t.TournamentType.Name = tt.Name;
                                t.TournamentType.TournamentTypeId = tt.TournamentTypeId;
                                t.TournamentType.Description = tt.Description;
                                t.TournamentPointSystem = new TournamentPointSystemContractModel();
                                t.TournamentPointSystem.TournamentPointSystemId = tps.TournamentPointSystemId;
                                t.TournamentPointSystem.Name = tps.Name;
                                t.TournamentPointSystem.Winpoint = tps.Winpoint;
                                t.TournamentPointSystem.DrawPoint = tps.DrawPoint;
                                t.TournamentPointSystem.LossPoint = tps.LossPoint;
                                return t;
                            }, new { Id = userId }, splitOn: "TournamentId,SportId,TournamentTypeId,TournamentPointSystemId", commandType: CommandType.Text, transaction: Transaction);

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
        /// Update tournament data
        /// </summary>
        /// <param name="model">TournamentContractModel</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> UpdateTournamentAsync(TournamentContractModel model)
        {
            try
            {
                this.StartTransaction();

                var newModel = new
                {
                    TournamentId = model.TournamentId,
                    Name = model.Name,
                    Description = model.Description,
                    SportId = model.Sport.SportId,
                    TournamentTypeId = model.TournamentType.TournamentTypeId,
                    TournamentPointSystemId = model.TournamentPointSystem.TournamentPointSystemId
                };

                string updateQuery = @"UPDATE [dbo].[Tournament] SET Name=@Name, Description=@Description, SportId=@SportId, TournamentTypeId=@TournamentTypeId, TournamentPointSystemId=@TournamentPointSystemId WHERE TournamentId=@TournamentId";

                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(updateQuery, newModel);

                this.CommitTransaction();

                // if rows affected (item created)
                if (rowsAffected > 0)
                    return new OkResult();
                else
                    return new BadRequestResult();
            }
            catch (Exception s)
            {
                this.RollbackTransaction(); // rollback & close
                throw;
            }
        }

        /// <summary>
        /// Delete tournament given id
        /// </summary>
        /// <param name="id">integer id</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> DeleteTournamentAsync(int tournamentId)
        {
            try
            {
                this.StartTransaction();

                // delete all the rows for tournament associated tables
                string deleteTournamentTeamsQuery = @"DELETE FROM [dbo].[Tournament_Team] WHERE TournamentId=@Id";

                var rowsAffected = await SaveDataInTransactionUsingQueryAsync(deleteTournamentTeamsQuery, new { Id = tournamentId });

                string deleteTournamentQuery = @"DELETE FROM [dbo].[Tournament] WHERE TournamentId = @Id";

                rowsAffected = await SaveDataInTransactionUsingQueryAsync(deleteTournamentQuery, new { Id = tournamentId });
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
        /// Get all team related to user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> CreateSubmittedPointSystem(TournamentPointSystemContractModel model)
        {
            try
            {
                var defaultTeamStatsModel = model;
                if (model.DefaultPointSystem == true)
                {
                    defaultTeamStatsModel = new TournamentPointSystemContractModel() { Name = "Default", DrawPoint = 1, LossPoint = 0, Winpoint = 3 };
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
