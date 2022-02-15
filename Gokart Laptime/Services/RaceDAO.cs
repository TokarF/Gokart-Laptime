using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using System.Data.SqlClient;

namespace Gokart_Laptime.Services
{
    public class RaceDAO : IRaceDAO
    {
        private string connectionString;

        public RaceDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GokartLaptime");
        }

        public List<RaceModel> GetAllRaces()
        {
            List<RaceModel> races = new List<RaceModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT R.*, RT.name FROM dbo.Races R LEFT JOIN dbo.RaceTracks RT ON RT.id = R.racetrack_id ORDER BY R.id DESC";
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                races.Add(new RaceModel { RaceId = (int)reader[0], RaceTrackId = (int)reader[1], RaceDate = (DateTime)reader[2], Description = reader.IsDBNull(3) ? null : (string)reader[3], Created_By = (int)reader[4], RaceTrackName = (string)reader[5] });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return races;
        }

        public int AddRace(RaceModel race)
        {
            int raceId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO dbo.Races (racetrack_id, racedate, description, created_by) VALUES (@racetrack_id, @racedate, @description, @created_by);SELECT SCOPE_IDENTITY()";
                        connection.Open();
                        command.Parameters.AddWithValue("@racetrack_id", race.RaceTrackId);
                        command.Parameters.AddWithValue("@racedate", race.RaceDate);
                        command.Parameters.AddWithValue("@description", race.Description?.Trim() ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@created_by", race.Created_By);
                        raceId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return raceId;
        }



        public RaceModel GetRaceById(int id)
        {
            RaceModel? race = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT R.*, RT.name FROM dbo.Races R LEFT JOIN dbo.RaceTracks RT ON RT.id = R.racetrack_id WHERE R.iD = @id";
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                race = new RaceModel { RaceId = (int)reader[0], RaceTrackId = (int)reader[1], RaceDate = (DateTime)reader[2], Description = reader.IsDBNull(3) ? null : (string)reader[3], Created_By = (int)reader[4], RaceTrackName = (string)reader[5] };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return race;
        }

        public bool UpdateRace(RaceModel race)
        {
            bool updated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE dbo.Races SET racetrack_id = @racetrack_id, racedate = @racedate, description = @description WHERE id = @id";
                        connection.Open();
                        command.Parameters.AddWithValue("@racetrack_id", race.RaceTrackId);
                        command.Parameters.AddWithValue("@racedate", race.RaceDate);
                        command.Parameters.AddWithValue("@description", race.Description?.Trim() ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id", race.RaceId);
                        updated = command.ExecuteNonQuery() != -1 ? true : false;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return updated;
        }

        public Dictionary<int, string> RaceTracksList()
        {
            Dictionary<int, string> raceTracksList = new Dictionary<int, string>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT RT.id, RT.name FROM dbo.RaceTracks RT";
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                raceTracksList.Add((int)reader[0], (string)reader[1]);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return raceTracksList;

        }


        public bool CreatedByLoggedInUser(int raceId, int racerId)
        {
            int createdBy = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT R.created_by FROM dbo.Races R WHERE R.iD = @id";
                        command.Parameters.AddWithValue("@id", raceId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                createdBy = (int)reader[0];
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return createdBy == racerId;
        }
    }
}
