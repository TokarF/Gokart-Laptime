using Gokart_Laptime.Models;
using Gokart_Laptime.Services;
using System.Data.SqlClient;

namespace Gokart_Laptime.Services
{
    public class RaceTrackDAO : IRaceTrackDAO
    {
        private string connectionString;

        public RaceTrackDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GokartLaptime");
        }

        public List<RaceTrackModel> GetAllRaceTracks()
        {
            List<RaceTrackModel> raceTracks = new List<RaceTrackModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM dbo.RaceTracks";
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                raceTracks.Add(new RaceTrackModel { RaceTrackId = (int)reader[0], RaceTrackName = (string)reader[1], Length = (int)reader[2], Address = (string)reader[3], Description = reader.IsDBNull(4) ? null : (string)reader[4] });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return raceTracks;
        }

        public RaceTrackModel GetRaceTrackById(int id)
        {
            RaceTrackModel raceTrack = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM dbo.RaceTracks RT WHERE RT.Id = @id";
                        command.Parameters.AddWithValue("@id", id);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                raceTrack = new RaceTrackModel { RaceTrackId = (int)reader[0], RaceTrackName = (string)reader[1], Length = (int)reader[2], Address = (string)reader[3], Description = reader.IsDBNull(4) ? null : (string)reader[4] };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return raceTrack;
        }


        public int AddRaceTrack(RaceTrackModel raceTrack)
        {
            int raceTrackId = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO dbo.RaceTracks (name, length, address, description) VALUES (@name, @length, @address, @description);SELECT SCOPE_IDENTITY()";
                        connection.Open();
                        command.Parameters.AddWithValue("@name", raceTrack.RaceTrackName.Trim());
                        command.Parameters.AddWithValue("@length", raceTrack.Length);
                        command.Parameters.AddWithValue("@address", raceTrack.Address.Trim());
                        command.Parameters.AddWithValue("@description", raceTrack.Description?.Trim() ?? (object)DBNull.Value);
                        raceTrackId = Convert.ToInt32(command.ExecuteScalar());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return raceTrackId;

        }

        public bool UpdateRaceTrack(RaceTrackModel raceTrack)
        {
            bool updated = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE dbo.RaceTracks SET name = @name, length = @length, address = @address, description = @description WHERE id = @id";
                        connection.Open();
                        command.Parameters.AddWithValue("@name", raceTrack.RaceTrackName.Trim());
                        command.Parameters.AddWithValue("@length", raceTrack.Length);
                        command.Parameters.AddWithValue("@address", raceTrack.Address.Trim());
                        command.Parameters.AddWithValue("@description", raceTrack.Description?.Trim() ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@id", raceTrack.RaceTrackId);
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

        public List<RaceModel> GetRaceTrackRacesById(int raceTrackId)
        {
            List<RaceModel> races = new List<RaceModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT R.*, RT.name FROM dbo.Races R LEFT JOIN dbo.RaceTracks RT ON RT.id = R.racetrack_id WHERE RT.id = @raceTrackId";
                        connection.Open();
                        command.Parameters.AddWithValue("@raceTrackId", raceTrackId);

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
    }
}
