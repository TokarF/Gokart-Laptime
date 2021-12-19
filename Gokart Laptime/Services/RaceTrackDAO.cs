using Gokart_Laptime.Models;
using System.Data.SqlClient;

namespace Gokart_Laptime.Controllers
{
    public class RaceTrackDAO
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
    }
}
