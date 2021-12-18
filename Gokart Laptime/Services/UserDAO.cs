using Gokart_Laptime.Models;
using System.Data.SqlClient;

namespace Gokart_Laptime.Services
{
    public class UserDAO
    {
        private string connectionString;
        public UserDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("GokartLaptime");
        }

        public bool RegisteredEmail(string email)
        {
            bool registeredEmail = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM dbo.Users U WHERE U.email = @email";
                        connection.Open();
                        command.Parameters.AddWithValue("email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) registeredEmail = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return registeredEmail;

        }

        public bool UsernameAlreadyInUse(string userName)
        {
            bool usernameAlreadyInUse = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM dbo.Users U WHERE U.username = @userName";
                        connection.Open();
                        command.Parameters.AddWithValue("userName", userName);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows) usernameAlreadyInUse = true;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return usernameAlreadyInUse;
        }



        public void RegisterUser(UserRegisterViewModel userRegistrationViewModel)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO dbo.Users (username, email, password) VALUES (@username, @email, @password)";
                        connection.Open();
                        command.Parameters.AddWithValue("username", userRegistrationViewModel.UserName);
                        command.Parameters.AddWithValue("email", userRegistrationViewModel.Email);
                        command.Parameters.AddWithValue("password", BCrypt.Net.BCrypt.HashPassword(userRegistrationViewModel.Password));
                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        public UserModel GetUserByEmail(string email)
        {
            UserModel? user = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "SELECT * FROM dbo.Users U WHERE U.email = @email";
                        connection.Open();
                        command.Parameters.AddWithValue("email", email);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    user = new UserModel { UserId = (int)reader[0], UserName = (string)reader[1], Email = (string)reader[2], Password = (string)reader[3], RegisteredAt = (DateTime)reader[4] };
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return user;
        }
    }
}
