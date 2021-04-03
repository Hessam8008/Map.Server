using Map.DataAccess.Dapper;
using Map.Models.AVL;
using Map.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Map.DataAccess.DAO;

namespace Map.DataAccess.Repositories
{
    public class AreaRepository : DapperRepository, IAreaRepository
    {
        public AreaRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            const string ProcedureName = "[dbo].[stpArea_GetAll]";
            var areaDaos = await QueryAsync<AreaDao>(ProcedureName);
            var result =
                from d in areaDaos
                select d?.ToArea();

            return result;
        }

        public async Task<int> UpdateLocationAsync(int id, float latitude, float longitude)
        {
            const string ProcedureName = "[dbo].[stpArea_SetLocation]";
            var param = new { id, latitude, longitude };
            var result = await this.ExecuteAsync(ProcedureName, param);
            return result;
        }
    }
}
