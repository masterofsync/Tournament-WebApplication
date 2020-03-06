using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;
using Microsoft.Extensions.Configuration;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public class TeamRepository : Repository<TeamContractModel>, ITeamRepository
    {
        public TeamRepository(IConfiguration config) : base(config)
        {
        }

        public bool AddTeam(TeamContractModel model)
        {
            try
            {
                this.StartTransaction();

                string insertQuery = @"INSERT INTO [dbo].[Team]([Name], [Description], [SportId], [TeamStatsId]) VALUES (@Name, @Description, @SportId, @TeamStatsId)";
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

        public bool DeleteTeam(TeamContractModel model)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TeamContractModel> GetAllTeamsOfSport(string sportName)
        {
            throw new NotImplementedException();
        }

        public bool UpdateTeam(TeamContractModel model)
        {
            throw new NotImplementedException();
        }
    }
}
