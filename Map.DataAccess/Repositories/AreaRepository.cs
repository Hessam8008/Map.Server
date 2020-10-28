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
            //const string ProcedureName = "[gps].[stpArea_GetAll]";
            //var areaDaos = await QueryAsync<AreaDAO>(ProcedureName);
            //var result =
            //    from d in areaDaos
            //    select d?.ToArea();
            
            //return result;
            var result = new List<Area>();

            result.Add(new Area(){ID = 1, Title = "مشهد" });
            result.Add(new Area() { ID = 2, Title = "تهران" });
            result.Add(new Area() { ID = 3, Title = "مازندران" });
            result.Add(new Area() { ID = 4, Title = "گیلان" });
            result.Add(new Area() { ID = 5, Title = "اصفهان" });

            return result;
        }
    }
}
