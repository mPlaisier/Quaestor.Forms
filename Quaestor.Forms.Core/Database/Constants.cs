using System;
using System.IO;

namespace Quaestor.Forms.Core.Database
{
    public static class Constants
    {
        #region Properties

        public static string DatabaseFilename => "QuaestorFormsSQLite.db3";

        public static SQLite.SQLiteOpenFlags Flags =>
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }

        #endregion
    }
}