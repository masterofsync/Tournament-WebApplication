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

                var result = await LoadSingleDataInTransactionUsingQueryAsync<TournamentTypeContractModel,dynamic>(getQuery,new { Id = id });

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
    }
}
