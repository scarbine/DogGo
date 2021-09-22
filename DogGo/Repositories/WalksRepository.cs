using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalksRepository :IWalksRepository
    {
        private readonly IConfiguration _config;

        public WalksRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walks> GetAllWalks()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                      SELECT w.Id, w.Date, w.Duration, w.WalkerId, w.DogId ,wk.Name as walkerName, d.Name as dogName
                      FROM Walks w JOIN Walker wk  ON w.WalkerId = wk.Id Join Dog d ON w.DogId = d.Id
                    ";


                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Durration = reader.GetInt32(reader.GetOrdinal("Durration")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId"))
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("WalkerId")))
                        {
                            walk.Walker = new Walker
                            {
                                Name = reader.GetString(reader.GetOrdinal("walkerName"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("DogId")))
                        {
                            walk.Dog = new Dog()
                            {
                                Name = reader.GetString(reader.GetOrdinal("dogName"))
                            };
                        }

                        walks.Add(walk);
                    }

                    reader.Close();

                    return walks;
                }
            }
        }

    }
}
