using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class SportRepository : Repository, ISportRepository
    {
        public SportRepository(IConfiguration config) : base(config)
        {
        }

        /// <summary>
        /// Add a new sport to the Sport table
        /// </summary>
        /// <param name="model">SportContractModel type</param>
        /// <returns>Ok(Status code:200 if created) else BadRequest(Status code: 400 if not created)</returns>
        public async Task<IActionResult> AddSportAsync(SportContractModel model)
        {
            try
            {
                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[Sport]([Name], [Description]) VALUES (@Name, @Description)";
                // save the team model
                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(insertQuery, model);

                this.CommitTransaction();

                // if rows affected (item created)
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

        /// <summary>
        /// Get all sports. 
        /// </summary>
        /// <returns>IEnumerable SportContractModel</returns>
        public async Task<IEnumerable<SportContractModel>> GetAllSportAsync()
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT * FROM [dbo].[Sport]";

                // save the team model
                var result = await LoadDataInTransactionUsingQueryAsync<SportContractModel, dynamic>(getQuery, null);

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
        /// Get sport given id.
        /// </summary>
        /// <param name="id">integer</param>
        /// <returns>SportContractModel</returns>
        public async Task<SportContractModel> GetSportAsync(int id)
        {
            try
            {
                this.StartTransaction();

                string getQuery = @"SELECT * FROM [dbo].[Sport] WHERE SportId = @Id";

                // save the team model
                var result = await LoadSingleDataInTransactionUsingQueryAsync<SportContractModel, dynamic>(getQuery, new { Id = id });

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
        /// Update sport data 
        /// </summary>
        /// <param name="model">SportContractModel type</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> UpdateSportAsync(SportContractModel model)
        {
            try
            {
                this.StartTransaction();

                string updateQuery = @"UPDATE [dbo].[Sport] SET Name= @Name, Description= @Description WHERE SportId = @SportId";

                var rowsAffected = await this.SaveDataInTransactionUsingQueryAsync(updateQuery,model);

                this.CommitTransaction();

                // if rows affected (item created)
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

        /// <summary>
        /// Delete sport given id.
        /// </summary>
        /// <param name="id">integer</param>
        /// <returns>Ok(Status code:200 if updated) else BadRequest(Status code: 400 if not updated)</returns>
        public async Task<IActionResult> DeleteSportAsync(int id)
        {
            try
            {
                this.StartTransaction();

                string deleteQuery = @"DELETE FROM [dbo].[Sport] WHERE SportId = @Id";

                // delete the row with id
                var rowsAffected = await SaveDataInTransactionUsingQueryAsync(deleteQuery,new { Id=id});
                this.CommitTransaction();

                // if rows affected (item deleted)
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
    }
}
