using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using DotNetLibrary;

namespace Server_chiruclande_Cessenger
{
    class Database
    {
        private string _hostname;
        private ushort _port;
        private string _username;
        private string _password;
        private string _database;

        public string DatabaseName
        {
            get { return _database; }
        }

        private MySqlConnection Connection;

        public Database(string hostname, ushort port, string username, string password, string database)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
            _database = database;

            Connection = new MySqlConnection(CreateConnectionString());
        }

        private string CreateConnectionString()
        {
            MySqlConnectionStringBuilder mCSB = new MySqlConnectionStringBuilder();

            mCSB.Server = _hostname;
            mCSB.Port = _port;
            mCSB.UserID = _username;
            mCSB.Password = _password;
            mCSB.Database = _database;

            return mCSB.ToString();
        }

        public List<Dictionary<Object, Object>> execute_query(string Query, List<Object> Params = null)
        {
            try
            {
                List<Dictionary<Object, Object>> Result = new List<Dictionary<Object, Object>>();

                MySqlCommand Command = new MySqlCommand(Query, Connection);

                Connection.Open();

                if (Params != null)
                {
                    int i = 0;

                    foreach (Object Param in Params)
                    {
                        i++;
                        Command.Parameters.Add(new MySqlParameter("@" + i.ToString(), value: Param));
                    }
                }

                Command.Prepare();

                MySqlDataReader Reader;
                Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    Dictionary<Object, Object> Row = new Dictionary<Object, Object>();

                    for (int i = 0; i < Reader.FieldCount; i++)
                        Row.Add(Reader.GetName(i), Reader.GetValue(i));

                    Result.Add(Row);
                }

                Reader.Close();
                Connection.Close();

                return Result;
            }
            catch (Exception ex)
            {
                if (Connection.State != System.Data.ConnectionState.Closed)
                    Connection.Close();

                return null;
            }
        }
    }
}
