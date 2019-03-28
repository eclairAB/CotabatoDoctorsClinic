using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Clinic
{
    class Connection
    {
        MySqlCommand _cmd = null;
        private String _server;
        private String _port;
        private String _db;
        private String username;
        private String password;

        public Connection()
        {
           // _server = "192.168.0.115";
            _server = "localhost";
            _port = "3306";
            _db = "capstone_pis";
            username = "root";
            password = "";
        }

        public MySqlConnection Connect()
        {
            String constring = String.Format("SERVER={0};PORT={1};DATABASE=capstone_pis; username={3}; password={4}", _server, _port, _db, username, password);
            try
            {
                return new MySqlConnection(constring);
            }
            catch (MySqlException ex)
            {
                return null;
            }
        }
    }
}
