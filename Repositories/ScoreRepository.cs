using Game2048.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Game2048.Repositories
{
    public class ScoreRepository : BaseSqliteRepository, IRepository<ScoreModel>
    {
        public ScoreRepository(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new Exception("Connection string is empty");
            ConnectionString = connectionString;
        }

        public async Task<int> AddAsync(ScoreModel data)
        {
            var cmdText = $"INSERT INTO Scores (UserId, Score) VALUES ({data.UserId}, {data.Score})";
            return await ExecCommandAsync(cmdText);
        }

        public async Task DeleteAsync(int id)
        {
            var cmdText = $"DELETE FROM Scores WHERE Id={id};";
            await ExecCommandByCountResultAsync(cmdText);
        }

        public async Task DeleteScoreByUserAsync(int userId)
        {
            var cmdText = $"DELETE FROM Scores WHERE UserId={userId};";
            await ExecCommandByCountResultAsync(cmdText);
        }
        public async Task UpdateAsync(ScoreModel data)
        {
            var cmdText = $"UPDATE Scores SET Score={data.Score} WHERE Id = {data.Id} AND UserId={data.UserId};";
            await ExecCommandByCountResultAsync(cmdText);
        }
        public async Task<ICollection<ScoreModel>> GetByUserId(int id)
        {
            return await ExecQueryAsync($"SELECT Scores.Id, Users.Login, max(Scores.Score) AS Score FROM Scores " +
                    "INNER JOIN Users " +
                    "ON Scores.UserId = Users.Id " +
                     $"WHERE Users.Id = '{id}' " +
                    "GROUP BY Scores.Id, Users.Login " +
                    "ORDER BY Score DESC LIMIT 15;");
        }

        public async Task<ICollection<ScoreModel>> GetAllScores()
        {
            return await ExecQueryAsync($"SELECT Scores.Id, Users.Login, max(Scores.Score) AS Score FROM Scores " +
                  "INNER JOIN Users ON Scores.UserId = Users.Id " +
                  "GROUP BY Scores.Id, Users.Login " +
                  "ORDER BY Score DESC LIMIT 15;");
        }

        public async Task<int> GetLastId()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT max(Id) AS Id FROM Scores;";
                    var resData = await command.ExecuteReaderAsync();
                    if (resData.HasRows)
                    {
                        var result = 0;
                        while (await resData.ReadAsync())
                        {
                            result = resData.GetInt32(0);
                        }
                        return result;
                    }
                    else return 0;
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

        public async Task<int> GetHighScore(int userId)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = $"SELECT IIF(MAX(s.Score) IS NULL,0,MAX(s.Score)) AS HighScore FROM Scores s " +
                        $"WHERE s.UserId = {userId};";
                    var resData = await command.ExecuteReaderAsync();
                    if(resData.HasRows)
                    {
                        var result = 0;
                        while (await resData.ReadAsync())
                        {
                            result = resData.GetInt32(0);
                        }
                        return result;
                    }
                    else return 0;
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

        protected async Task<ICollection<ScoreModel>> ExecQueryAsync(string cmd)
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
                        var result = new List<ScoreModel>();
                        while (await resData.ReadAsync())
                        {
                            var data = new ScoreModel
                            {
                                Id = resData.GetInt32(0),
                                Login = resData.GetString(1),
                                Score = resData.GetInt32(2)
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