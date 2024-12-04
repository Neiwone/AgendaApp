using Microsoft.Data.Sqlite;

namespace App
{
    public class DatabaseConnection
    {
        private static DatabaseConnection _instance;
        public SqliteConnection Connection { get; private set; }

        private DatabaseConnection()
        {
            this.Connection = new SqliteConnection("Data Source=consultorio.db;Version=3;");
            this.Connection.Open();
        }

        public static DatabaseConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection();
            }
            return _instance;
        }
    }
}