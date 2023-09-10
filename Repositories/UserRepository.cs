using Game2048.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace Game2048.Repositories
{
    public class UserRepository : BaseSqliteRepository, IRepository<UserModel>
    {
        public UserRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string is empty");
            ConnectionString = connectionString;
        }
        
        public async Task<int> AddAsync(UserModel data)
        {
            var cmdText = $"INSERT INTO Users (Login, Password) VALUES ('{data.Login}', '{data.Password}');";
            return await ExecCommandAsync(cmdText);
        }

        public async Task UpdateAsync(UserModel data)
        {
            var cmdText = $"UPDATE Users SET Login={data.Login}, Password={data.Password} WHERE Id={data.Id};";
            await ExecCommandByCountResultAsync(cmdText);
        }

        public async Task DeleteAsync(int id)
        {
            var cmdText = $"DELETE FROM Users WHERE Id={id};";
            await ExecCommandByCountResultAsync(cmdText);
        }

        public async Task<IList<UserModel>> GetList()
        {
            return await ExecQueryAsync("SELECT * FROM Users;") as List<UserModel>;
        }

        public async Task<UserModel> GetByUserId(int id)
        {
            List<UserModel> result = await ExecQueryAsync($"SELECT * FROM Users WHERE Id = '{id}';") as List<UserModel>;
            return result.FirstOrDefault();
        }

        public async Task<UserModel> GetByName(string name)
        {
            List<UserModel> result = await ExecQueryAsync($"SELECT * FROM Users WHERE Login = '{name}';") as List<UserModel>;
            if (result != null) return result.FirstOrDefault();
            else return null;
        }

        protected async Task<ICollection<UserModel>> ExecQueryAsync(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) return null;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = cmd;
                    var resData = await command.ExecuteReaderAsync();
                    if (resData.HasRows)
                    {
                        var result = new List<UserModel>();
                        while (await resData.ReadAsync())
                        {
                            var data = new UserModel
                            {
                                Id = resData.GetInt32(0),
                                Login = resData.GetString(1),
                                Password = resData.GetString(2)
                            };
                            result.Add(data);
                        }
                        return result;
                    }
                    else return null;
                }
                catch (SQLiteException e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}