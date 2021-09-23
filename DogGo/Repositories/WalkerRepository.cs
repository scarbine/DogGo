using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class WalkerRepository : IWalkerRepository
    {
        private readonly IConfiguration _config;

        public WalkerRepository(IConfiguration config)
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

        public List<Walker> GetAllWalkers()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT w.Id, w.[Name], w.ImageUrl, w.NeighborhoodId , n.Name as hoodName
                        FROM Walker w LEFT JOIN Neighborhood n ON w.NeighborhoodId = n.Id
                    ";


                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("NeighborhoodId")))
                        {
                            walker.Neighborhood = new Neighborhood
                            {
                                Name = reader.GetString(reader.GetOrdinal("hoodName"))
                            };
                        }

                    walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

        public void UpdateWalker(Walker walker)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                            UPDATE Walker
                            SET 
                                [Name] = @name, 
                                ImageUrl = @imageUrl,
                                NeighborhoodId = @neighborhoodId
                            WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@name", walker.Name);
                    cmd.Parameters.AddWithValue("@imageUrl", walker.ImageUrl);
                    cmd.Parameters.AddWithValue("@neighborhoodId", walker.NeighborhoodId);
                    cmd.Parameters.AddWithValue("@id", walker.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Walker GetWalkerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], ImageUrl, NeighborhoodId
                        FROM Walker
                        WHERE Id = @Id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        reader.Close();
                        return walker;

                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }


            }
        }

        public List<Walks> GetAllWalksByWalker(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT w.Id , w.Duration, w.Date, w.DogId, w.WalkerId, d.Name AS dogName, o.Name As ownerName, wk.Name AS walkerName FROM Walks w 
                                        LEFT JOIN Walker wk ON wk.id = w.WalkerId 
                                        LEFT JOIN Dog d ON d.Id = w.DogId 
                                        LEFT Join Owner o ON o.Id = d.OwnerId 
                                        WHERE wk.Id = @id ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                        List<Walks> walks = new List<Walks>();
                    while (reader.Read())
                    {
                        Walks walk = new Walks
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Duration = reader.GetInt32(reader.GetOrdinal("Duration")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                        };

                        if (!reader.IsDBNull(reader.GetOrdinal("walkerName")))
                        {
                            walk.Walker = new Walker
                            {
                                Name = reader.GetString(reader.GetOrdinal("walkerName"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("dogName")))
                        {
                            walk.Dog = new Dog()
                            {
                                Name = reader.GetString(reader.GetOrdinal("dogName"))
                            };
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("ownerName")))
                        {
                            walk.Owner = new Owner()
                            {
                                Name = reader.GetString(reader.GetOrdinal("ownerName"))
                            };
                        }

                        walks.Add(walk);
                    }
                 
                        reader.Close();
                        return walks;
                }
            }
        }

        public List<Walker> GetWalkersInNeighborhood(int neighborhoodId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                SELECT Id, [Name], ImageUrl, NeighborhoodId
                FROM Walker
                WHERE NeighborhoodId = @neighborhoodId
            ";

                    cmd.Parameters.AddWithValue("@neighborhoodId", neighborhoodId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walker> walkers = new List<Walker>();
                    while (reader.Read())
                    {
                        Walker walker = new Walker
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                            
                        };

                        walkers.Add(walker);
                    }

                    reader.Close();

                    return walkers;
                }
            }
        }

    }
}
