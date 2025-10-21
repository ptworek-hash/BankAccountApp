using System.Data.SQLite;
using System.IO;

namespace BT.Model.Customer
{
    internal static class SqliteConnectionFactory
    {
        private static readonly string DbPath = Path.Combine(Path.GetTempPath(), "btmodel_customers.sqlite");
        private static readonly string ConnectionString = "Data Source=" + DbPath + ";Version=3;Foreign Keys=True;";

        public static SQLiteConnection Create()
        {
            var firstRun = !File.Exists(DbPath);
            var connection = new SQLiteConnection(ConnectionString);
            connection.Open();
            if (firstRun)
            {
                CreateSchema(connection);
            }
            return connection;
        }

        private static void CreateSchema(SQLiteConnection connection)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Customers (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NULL,
                    LastName TEXT NOT NULL,
                    CompanyName TEXT NOT NULL,
                    Street TEXT NULL,
                    City TEXT NULL,
                    State TEXT NULL,
                    Zip TEXT NULL
                );";
                cmd.ExecuteNonQuery();
            }
        }
    }
}
