using System;
using System.Data;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Game2048.Repositories
{
    public class BaseSqliteRepository
    {
        public string ConnectionString { get; set; }

        public void CreateDataBase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Users(Id INTEGER " +
                        "NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                        "Login TEXT NOT NULL UNIQUE, " +
                        "Password TEXT NOT NULL);";
                    command.ExecuteNonQuery();
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Scores(Id INTEGER " +
                        "NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, " +
                        "UserId INTEGER NOT NULL, " +
                        "Score INTEGER NOT NULL);";
                    command.ExecuteNonQuery();
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

        protected async Task<int> ExecCommandAsync(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) return 0;
            var result = 0;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            var command = connection.CreateCommand();
                            command.CommandText = cmd;
                            result = await command.ExecuteNonQueryAsync();
                            transaction.Commit();
                        }
                        catch (SQLiteException e)
                        {
                            transaction.Rollback();
                            throw new Exception(e.Message);
                        }
                    }
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
            return result;
        }

        /// <summary>
        /// Выполнение комманд с возвратом кол-ва обработанных строк
        /// </summary>
        /// <param name="cmd">Команда UPDATE, DELETE</param>
        /// <returns>Кол-во обработанных строк</returns>
        /// <exception cref="Exception">Ошибка при выполнении</exception>
        protected async Task<int> ExecCommandByCountResultAsync(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) return 0;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            var command = connection.CreateCommand();
                            command.CommandText = cmd;
                            var count = await command.ExecuteNonQueryAsync();
                            transaction.Commit();
                            return count;
                        }
                        catch
                        {
                            transaction.Rollback();
                            return 0;
                        }
                    }
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