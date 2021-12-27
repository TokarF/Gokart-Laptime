using Gokart_Laptime.Models;
using System.Data.SqlClient;

namespace Gokart_Laptime.Services
{
    public class RacerDAO : IRacerDAO
    {

        private string connectionString;

        public RacerDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GokartLaptime");
        }

        public List<RacerModel> GetRacersByRaceID(int raceId)
        {
            List<RacerModel> racers = new List<RacerModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT R.*, U.username FROM dbo.Racers R LEFT JOIN dbo.Users U ON U.id = R.racer_id WHERE R.race_id = @race_id";
                        command.Parameters.AddWithValue("@race_id", raceId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                racers.Add(new RacerModel { RaceId = (int)reader[0], RacerId = (int)reader[1], RacerName = (string)reader[2] });
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return racers;
        }
        public List<RacerModel> GetAllRacers(int raceId)
        {
            List<RacerModel> racers = new List<RacerModel>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT U.id, U.username FROM dbo.Users U WHERE U.id NOT IN (SELECT R.racer_id FROM dbo.Racers R WHERE R.race_id = @race_id)";
                        command.Parameters.AddWithValue("@race_id", raceId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                racers.Add(new RacerModel { RacerId = (int)reader[0], RacerName = (string)reader[1] });
                            }
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return racers;
        }

        public void AddRacer(int selectedRacerId, int raceId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO dbo.Racers (race_id, racer_id) VALUES (@race_id, @racer_id)";
                        connection.Open();
                        command.Parameters.AddWithValue("@race_id", raceId);
                        command.Parameters.AddWithValue("@racer_id", selectedRacerId);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RemoveRacer(int raceId, int racerId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "DELETE FROM dbo.Racers WHERE race_id = @race_id AND racer_id = @racer_id";
                        connection.Open();
                        command.Parameters.AddWithValue("@race_id", raceId);
                        command.Parameters.AddWithValue("@racer_id", racerId);
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
