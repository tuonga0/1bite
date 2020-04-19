using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace _1bite
{
    public class AccountDAO
    {
        public static SqlConnection setConnection()
        {
            var conn = "server=118.27.193.68; database =uonga0_HzL12153_db;uid=uonga0_HzL12153_dbuser;pwd=!Q@W#E$R%T^Y&U*I(O)P_{+}"; //DESKTOP-C4SUR0U xem trong SQL
            return new SqlConnection(conn);
        }
        public static string EncodePassword(string originalPassword)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;

            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);

            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }
        public static bool AccountVerify(string acc, string pwd)
        {
            var url = "server=118.27.193.68;database=uonga0_HzL12153_db;uid=uonga0_HzL12153_dbuser;pwd=!Q@W#E$R%T^Y&U*I(O)P_{+}";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "select * from Account where Username='" + acc + "'and password='" + pwd + "'";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            if (sqlRead.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void addAcc(string acc, string pwd)
        {
            var sql = "insert into Account values('" + acc + "','" + pwd + "','False')"; //viết kiểu này sẽ bị hack sql injection
            var sqlda = new SqlDataAdapter(sql, setConnection());
            var ds = new DataSet();
            sqlda.Fill(ds, "Account");
        }

        public static bool checkName(string accName)
        {
            var url = "server=118.27.193.68; database =uonga0_HzL12153_db;uid=uonga0_HzL12153_dbuser;pwd=!Q@W#E$R%T^Y&U*I(O)P_{+}";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "select Username from Account where Username = '" + accName + "'";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlread = sqlcom.ExecuteReader();
            if (sqlread.Read())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool checkAdm(string acc, string pwd)
        {
            var url = "server=118.27.193.68; database =uonga0_HzL12153_db;uid=uonga0_HzL12153_dbuser;pwd=!Q@W#E$R%T^Y&U*I(O)P_{+}";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "select [role] from Account where [Username]='" + acc + "'and [password]='" + pwd + "'";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            bool role = false;
            while (sqlRead.Read())
            {
                role = (bool)sqlRead["role"];
            }
            if (role == true)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}