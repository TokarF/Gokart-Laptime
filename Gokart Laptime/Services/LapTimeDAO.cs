using Gokart_Laptime.Models;
using System.Data.SqlClient;

namespace Gokart_Laptime.Services
{
    public class LapTimeDAO : ILapTimeDAO
    {
        private string connectionString;
        public LapTimeDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GokartLaptime");
        }

        public List<LaptimeModel> GetRacerLaptimeByRaceAndRacerId(int raceId, int racerId)
        {
            List<LaptimeModel> lapTimes = new List<LaptimeModel>();
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT LT.* FROM dbo.Laptimes LT WHERE LT.race_id = @raceId AND LT.racer_id = @racerId";
                        command.Parameters.AddWithValue("@raceId", raceId);
                        command.Parameters.AddWithValue("@racerId", racerId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lapTimes.Add(new LaptimeModel { Id = (int)reader[0], RaceId = (int)reader[1], RacerId = (int)reader[2], Lap = (int)reader[3], LapTime = TimeSpan.FromMilliseconds((int)reader[4]) });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lapTimes;
        }

        public void RemoveLapTimes(List<LaptimeModel> lapTimesToDelete)
        {
            throw new NotImplementedException();
        }

        public void UpdateLapTimes(List<LaptimeModel> lapTimesToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
