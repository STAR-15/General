using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;

namespace General.Server.Repository
{
    static class DbManager
    {
        private static readonly Lazy<ConnectionStringSettings> sqlConnectionString = new Lazy<ConnectionStringSettings>(() =>
        {

            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"DBFile\SQLData");
            // Create Data Directory If It Doesn't Already Exist.
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            var dbName = @"AdminDB";
            var dbFile = Path.Combine(dataFolder, "Admin_DB.db");

            if (!File.Exists(dbFile))
            {
                CreateDatabase(dbFile);
            }

            return new ConnectionStringSettings(dbName, @"Data Source=" + dbFile);

        });

        public static IDbConnection NewConnection()
        {
            return new SQLiteConnection(sqlConnectionString.Value.ConnectionString);
        }

        #region Setup DataBase

        public static bool CreateDatabase(string dbFileName)
        {
            string connectionString = String.Format(@"Data Source=" + dbFileName);
            SQLiteConnection.CreateFile(dbFileName);
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.TryExecute(@"
                    CREATE TABLE User
                        (
                            UserId INTEGER PRIMARY KEY autoincrement,
                            UserName TEXT NOT NULL,
                            Password TEXT NOT NULL
                        );
                    insert into User values 
                    (NULL,
                    'Eden',
                    'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=');");
            }

            if (File.Exists(dbFileName)) return true;
            else return false;
        }
        private static void TryExecute(this IDbConnection conn, string sql)
        {
            try
            {
                conn.Execute(sql);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
