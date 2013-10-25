using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Server_chiruclande_Cessenger
{
    public struct struct_config
    {

        public _inet inet;
        public struct _inet
        {
            public string address;
            public ushort port;
        }

        public _server server;
        public struct _server
        {
            public string name;

            public _connection connection;
            public struct _connection
            {
                public uint limit;
            }
        }

        public _logging logging;
        public struct _logging
        {
            public bool usedate;

            public _file file;
            public struct _file
            {
                public string basename;
                public string prefix;
                public string suffix;
            }
        }
    }

    public struct struct_socket
    {
        public string address;
        public ushort port;

        public IPEndPoint endpoint
        {
            get { return new IPEndPoint(IPAddress.Parse(address), port); }
        }

        public TcpListener server;
        public TcpClient client;
    }

    public struct struct_connection
    {
        public NetworkStream stream;
        public StreamWriter streamw;
        public StreamReader streamr;
        public uint account_id;
        public string account_name;
    }
}