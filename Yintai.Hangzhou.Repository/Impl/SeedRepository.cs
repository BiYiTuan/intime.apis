using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Repository.Contract;

namespace Yintai.Hangzhou.Repository.Impl
{
    public class SeedRepository : RepositoryBase<SeedEntity, string>, ISeedRepository
    {
        public override SeedEntity GetItem(string key)
        {
            throw new NotImplementedException();
        }

        public int Generate(string name, int maxSeed, int k)
        {
            var outputId = new SqlParameter("@id", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var parames = new SqlParameter[6];

            parames[0] = outputId;
            parames[1] = new SqlParameter("@name", name);
            parames[2] = new SqlParameter("@maxSeed", maxSeed);
            parames[3] = new SqlParameter("@k", k);

            var proName = "[dbo].[Seed_Generate4Key]";
            SqlHelper.ExecuteNonQuery(SqlHelper.GetConnection(), CommandType.StoredProcedure,
                                            proName, parames);

            return Int32.Parse(outputId.Value.ToString());
        }
    }
}
