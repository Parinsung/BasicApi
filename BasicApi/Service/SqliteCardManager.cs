using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicApi.Service
{
    public class SqliteCardManager : ISqliteCardManager
    {
        private readonly string _dbName;
        public SqliteCardManager(string dbName)
        {
            _dbName = dbName;
            using (var connection = new SqliteConnection($"Data Source={dbName}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"create table if not exists card(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name NVARCHAR(255),
                        Status NVARCHAR(255),
                        Content NVARCHAR(255),
                        Category NVARCHAR(255),
                        Author NVARCHAR(255))"; 
                command.ExecuteNonQuery();

            }
        }

        public IEnumerable<Card> GetAll()
        {
            using (var connection = new SqliteConnection($"Data Source={_dbName}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = " SELECT * FROM card ";

                List<Card> result = new List<Card>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Card card = new Card();
                        card.Id = reader.GetInt32(0);
                        card.Name = reader.GetString(1);
                        card.Status = reader.GetString(2);
                        card.Content = reader.GetString(3);
                        card.Category = reader.GetString(4);
                        card.Author = reader.GetString(5);
                        result.Add(card);
                    }
                }
                return result;
            }
        }

        public Card Post(Card card)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbName}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = " INSERT INTO card (Name,Status,Content,Category,Author)" +
                                        " VALUES(" + $"'{card.Name}','{card.Status}','{card.Content}','{card.Category}','{card.Author}'" + ")";
                command.ExecuteNonQuery();

            }
            var data = GetAll().Where(_ => _.Name == card.Name 
                                            && _.Status == card.Status
                                            && _.Content == card.Content
                                            && _.Category == card.Category
                                            && _.Author == card.Author).OrderByDescending(_ => _.Id).First();
            return data;
        }

        public Card Put(Card card)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbName}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = @" UPDATE card 
                    SET Name = '"+ card.Name + @"',
                        Status = '"+ card.Status + @"',
                        Content = '" + card.Content + @"',
                        Category = '" + card.Category + @"',
                        Author = '" + card.Author + @"'
                    WHERE
                        Id = '" + card.Id + "'";
                command.ExecuteNonQuery();
            }
            var data = GetAll().FirstOrDefault(_ => _.Name == card.Name
                                            && _.Status == card.Status
                                            && _.Content == card.Content
                                            && _.Category == card.Category
                                            && _.Author == card.Author);
            return data;
        }

        public void Delete(int id)
        {
            using (var connection = new SqliteConnection($"Data Source={_dbName}"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "DELETE FROM card WHERE Id = " + id;
                command.ExecuteNonQuery();
            }
        }
    }
}
