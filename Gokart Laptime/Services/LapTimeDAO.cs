using Gokart_Laptime.Models;
using Gokart_Laptime.ViewModels;
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

        public void ManageLapTimes(RaceLaptimesViewModel raceLaptimesViewModel)
        {
            List<int> lapTimesIds = new List<int>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                connection.Open();
                SqlTransaction sqlTransaction;
                sqlTransaction = connection.BeginTransaction();

                using (SqlCommand command = new SqlCommand())
                {
                    try
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT LT.id FROM dbo.Laptimes LT WHERE LT.race_id = @raceId AND LT.racer_id = @racerId";
                        command.Parameters.AddWithValue("@raceId", raceLaptimesViewModel.RaceId);
                        command.Parameters.AddWithValue("@racerId", raceLaptimesViewModel.RacerId);
                        command.Transaction = sqlTransaction;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lapTimesIds.Add((int)reader[0]);
                            }
                        }

                        // Remvoe all laptiems that are not in the list
                        
                        List<int> lapTimesToDelete = lapTimesIds.Where(lt => raceLaptimesViewModel.Laptimes.All(lapTime => lapTime.Id != lt)).ToList();

                        if (lapTimesToDelete.Count > 0)
                        {
                            command.CommandText = "DELETE FROM dbo.Laptimes WHERE id = @id";
                            
                            foreach (int id in lapTimesToDelete)
                            {
                                command.Parameters.AddWithValue("@id", id);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                            }

                        }

                        // Update Laptimes

                        List<LaptimeModel> lapTimesToUpdate = raceLaptimesViewModel.Laptimes.Where(l => lapTimesIds.Contains(l.Id)).ToList();

                        if (lapTimesToUpdate.Count > 0)
                        {
                            foreach (LaptimeModel laptime in lapTimesToUpdate)
                            {
                                command.CommandText = "UPDATE dbo.Laptimes SET lap = @lap, laptime = @laptime WHERE id = @id";
                                command.Parameters.AddWithValue("@lap", laptime.Lap);
                                command.Parameters.AddWithValue("@laptime", laptime.LapTime.Days);
                                command.Parameters.AddWithValue("@id", laptime.Id);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();

                            }
                        }

                        // Add new Laptimes where id null
                        List<LaptimeModel> lapTimesToAdd = raceLaptimesViewModel.Laptimes.Where(lt => lt.Id == 0).ToList();

                        if (lapTimesToAdd.Count > 0)
                        {
                            command.CommandText = "INSERT INTO dbo.Laptimes (race_id, racer_id, lap, laptime) VALUES (@race_id, @racer_id, @lap, @laptime)";

                            foreach (LaptimeModel laptime in lapTimesToAdd)
                            {
                                command.Parameters.AddWithValue("@race_id", raceLaptimesViewModel.RaceId);
                                command.Parameters.AddWithValue("@racer_id", raceLaptimesViewModel.RacerId);
                                command.Parameters.AddWithValue("@lap", laptime.Lap);
                                command.Parameters.AddWithValue("@laptime", laptime.LapTime.Days);
                                command.ExecuteNonQuery();
                                command.Parameters.Clear();
                                
                            }
                        }

                        sqlTransaction.Commit();

                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        sqlTransaction.Rollback();
                    }   
      
                }
            }
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

        public void AddLapTimes(List<LaptimeModel> lapTimesToUpdate)
        {
            throw new NotImplementedException();
        }

        public void UpdateLapTimes(List<LaptimeModel> lapTimesToUpdate)
        {
            throw new NotImplementedException();
        }

        public void RemoveLapTimes(List<LaptimeModel> lapTimesToDelete)
        {
            throw new NotImplementedException();
        }



        public List<int> GetIdsByRaceAndRacerID(int raceId, int racerId)
        {
            List<int> lapTimeIds = new List<int>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT LT.id FROM dbo.Laptimes LT WHERE LT.race_id = @raceId AND LT.racer_id = @racerId";
                        command.Parameters.AddWithValue("@raceId", raceId);
                        command.Parameters.AddWithValue("@racerId", racerId);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                lapTimeIds.Add((int)reader[0]);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return lapTimeIds;
        }


    }
}
