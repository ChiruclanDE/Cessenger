using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace Server_chiruclande_Cessenger
{
    class Database
    {
        private string _hostname;
        private ushort _port;
        private string _username;
        private string _password;
        private string _database;
        private Logging logger;

        public string DatabaseName
        {
            get { return _database; }
        }

        private MySqlConnection Connection;

        public Database(string hostname, ushort port, string username, string password, string database, Logging _logger)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
            _database = database;
            logger = _logger;

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

        public List<Dictionary<object, object>> execute_query(string Query, params object[] Params)
        {
            try
            {
                List<Dictionary<object, object>> Result = new List<Dictionary<object, object>>();

                MySqlCommand Command = new MySqlCommand(Query, Connection);

                Connection.Open();

                for (int i = 0; i < Params.Length; i++)
                    Command.Parameters.Add(new MySqlParameter("@" + i.ToString(), value: Params[i]));

                Command.Prepare();

                MySqlDataReader Reader;
                Reader = Command.ExecuteReader();

                while (Reader.Read())
                {
                    Dictionary<object, object> Row = new Dictionary<object, object>();

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

                logger.cerr(ex.Message);
                return null;
            }
        }
    }
}
