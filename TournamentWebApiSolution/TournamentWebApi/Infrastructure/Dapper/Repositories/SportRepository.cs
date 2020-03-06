using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class SportRepository : Repository<SportContractModel>, ISportRepository
    {
        public SportRepository(IConfiguration config) :base(config)
        {

        }

        public bool AddSport(SportContractModel model)
        {
            try
            {
                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[Sport]([Name], [Description]) VALUES (@Name, @Description)";
                // save the team model
                this.SaveDataInTransactionUsingQuery(insertQuery, model);

                this.CommitTransaction();

                return true;
            }
            catch (Exception)
            {
                this.RollbackTransaction();
                //return false; ??
                throw;
            }
        }

        public bool DeleteSport(SportContractModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SportContractModel> GetAllSport()
        {
            throw new NotImplementedException();
        }

        public async Task<SportContractModel> GetSport(int id)
        {
            //try
            //{
            //    this.StartTransaction();

            //    string insertQuery = @"SELECT SportId, Name, Description FROM [dbo].[Sport] WHERE SportId={id}";
            //    // save the team model
            //    this.SaveDataInTransactionUsingQuery(insertQuery, model);

            //    this.CommitTransaction();

            //    return true;
            //}
            //catch (Exception)
            //{
            //    this.RollbackTransaction();
            //    //return false; ??
            //    throw;
            //}
        }

        public bool UpdateSport(SportContractModel model)
        {
            throw new NotImplementedException();
        }
    }
}
