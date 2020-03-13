﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contract.Models;

namespace TournamentWebApi.Infrastructure.Dapper.Repositories
{
    public interface ITeamsRepository:IRepository
    {
        IEnumerable<TeamContractModel> GetAllTeamsOfSport(string sportName);

        bool AddTeam(TeamContractModel model);
        bool DeleteTeam(TeamContractModel model);
        bool UpdateTeam(TeamContractModel model);
    }
}