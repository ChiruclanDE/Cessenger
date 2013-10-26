using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Reflection;
using DotNetLibrary;

namespace Server_chiruclande_Cessenger
{
    class Program
    {
        #region configuration_external
        private static SimpleConfiguration config;
        private static SimpleCaching cache;
        private static SimpleCryptography crypt;
        #endregion

        #region configuration_io_system
        private static Logging logger;
        private static Database db;
        #endregion

        #region config_sockets
        private static struct_socket sock;
        #endregion

        #region config_vars
        private static struct_config cfg;
        private static _prepared prep = new _prepared();
        #endregion

        static void Main(string[] args)
        {
            try
            {
                if (!init_server())
                    return;

                Console.Title = "(Server) chiruclan.de Cessenger";

                logger.cout(":: chiruclan.de Cessenger Server"
                        + "\n:: ============================="
                        + "\n::"
                        + "\n:: Software powered by chiruclan.de"
                        + "\n::");

                sock.server.Start();
                logger.cout(":: Listening at {0}:{1}", sock.address, sock.port);

                while (true)
                {
                    sock.client = sock.server.AcceptTcpClient();
                    struct_connection connection;
                    connection.stream = sock.client.GetStream();
                    connection.streamw = new StreamWriter(connection.stream);
                    connection.streamr = new StreamReader(connection.stream);
                }

                sock.server.Stop();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:\n{0}", ex.Message);
            }

            Console.ReadKey();
        }

        private static bool init_server()
        {
            try
            {
                config = new SimpleConfiguration("cc_server.conf");
                crypt = new SimpleCryptography();
                cache = new SimpleCaching();

                cfg.logging.file.basename = config.GetKey("logging.file.basename");
                cfg.logging.file.prefix = config.GetKey("logging.file.prefix");
                cfg.logging.file.suffix = config.GetKey("logging.file.suffix");
                bool.TryParse(config.GetKey("logging.usedate"), out cfg.logging.usedate);
                logger = new Logging(cfg.logging.file.basename, cfg.logging.file.prefix, cfg.logging.file.suffix, cfg.logging.usedate);

                cfg.server.name = config.GetKey("server.name");
                uint.TryParse(config.GetKey("server.connection.limit"), out cfg.server.connection.limit);

                cfg.inet.address = config.GetKey("inet.address");
                ushort.TryParse(config.GetKey("inet.port"), out cfg.inet.port);

                cfg.mysql.username = config.GetKey("mysql.username");
                cfg.mysql.password = config.GetKey("mysql.password");
                cfg.mysql.hostname = config.GetKey("mysql.hostname");
                cfg.mysql.database = config.GetKey("mysql.database");
                ushort.TryParse(config.GetKey("mysql.port"), out cfg.mysql.port);

                db = new Database(cfg.mysql.hostname, cfg.mysql.port, cfg.mysql.username, cfg.mysql.password, cfg.mysql.database, logger);
                //db.execute_query(prep.get_account_id, "test");

                sock.address = cfg.inet.address;
                sock.port = cfg.inet.port;
                sock.server = new TcpListener(sock.endpoint);

                return true;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nError:\n{0}", ex.Message);
                return false;
            }
        }

    }
}
