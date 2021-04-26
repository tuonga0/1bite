using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using _1bite.Models;
using System.Web.Mvc;
namespace _1bite
{
    public class AccountDAO
    {

        public static SqlConnection setConnection()
        {
            var conn = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!; MultipleActiveResultSets=True"; //DESKTOP-C4SUR0U xem trong SQL
            return new SqlConnection(conn);
        }
        public static string getRankwithId(int rankId)
        {
            string rank;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select rankName from Rank where rankId = @rankid";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@rankid", SqlDbType.Int).Value = rankId;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            rank = sqlRead[0].ToString();
            conn.Close();
            return rank;
        }
        public static int checkAccRank(string username)
        {
            int id;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select rankId from Account where username = @username";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            while (sqlRead.Read())
            {
                id = Int32.Parse(sqlRead[0].ToString());
                conn.Close();
                return id;
            }
            conn.Close();
            return 0;
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
            var conn = setConnection();
            conn.Open();
            string sql = "select * from Account where Username='" + acc + "'and password='" + pwd + "'";
            SqlCommand sqlcom = new SqlCommand(sql, conn);
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
        public static void addDish(string name, int type, int price, string des)
        {
            var sql = "insert into Dish values(@name,@price,@des,@type)";
            var conn = setConnection();
            conn.Open();
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            sqlcom.Parameters.Add("@type", SqlDbType.Int).Value = type;
            sqlcom.Parameters.Add("@price", SqlDbType.Int).Value = price;
            sqlcom.Parameters.Add("@des", SqlDbType.NVarChar).Value = des;
            sqlcom.ExecuteNonQuery();
            conn.Close();
        }
        public static void addProduct(string name, string unit)
        {
            var sql = "insert into Product values(@name,@unit)";
            var conn = setConnection();
            conn.Open();
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            sqlcom.Parameters.Add("@unit", SqlDbType.NVarChar).Value = unit;
            sqlcom.ExecuteNonQuery();
            conn.Close();
        }
        public static void addSource(string name, string address)
        {
            var sql = "insert into Source values(@name,@unit)";
            var conn = setConnection();
            conn.Open();
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            sqlcom.Parameters.Add("@unit", SqlDbType.NVarChar).Value = address;
            sqlcom.ExecuteNonQuery();
            conn.Close();
        }
        public static void addAcc(string acc, string pwd,string name, string phone, string email, int rankId)
        {
            int accId;
            var sql = "insert into Account OUTPUT inserted.accID values('" + acc + "','" + pwd + "',"+rankId+",GETDATE())"; //viết kiểu này sẽ bị hack sql injection
            var insertStaff = "insert into Staff values (@name,@phone,@email,@id)";
            var conn = setConnection();
            conn.Open();
            var sqlcom = new SqlCommand(sql, conn);
            var sqlcom2 = new SqlCommand(insertStaff, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            accId = Convert.ToInt32(sqlRead[0]);
            sqlcom2.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            sqlcom2.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone;
            sqlcom2.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
            sqlcom2.Parameters.Add("@id", SqlDbType.Int).Value = accId;
            sqlcom2.ExecuteNonQuery();
            conn.Close();
        }
        public static void addOrder(int staffId, int discount, string note, int statusId, string address, List<OrderDetails> lod)
        {
            int orderId;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!; MultipleActiveResultSets=True";
            var sql = "insert into [Order] OUTPUT inserted.orderID values (@staffId, GETDATE(), @discount, @note, @statusId, @customerAddress, null)";
            var insertOD = "insert into Order_Item values (@orderID, @dishID, @dishAmount, 6)";
            var conn = new SqlConnection(url);
            var sqlcom = new SqlCommand(sql, conn);
            var sqlcom2 = new SqlCommand(insertOD, conn);
            conn.Open();
            sqlcom.Parameters.Add("@staffId", SqlDbType.Int).Value = staffId;
            sqlcom.Parameters.Add("@discount", SqlDbType.Int).Value = discount;
            if (note == null)
            {
                sqlcom.Parameters.Add("@note", SqlDbType.NVarChar).Value = DBNull.Value;
            }
            else
            {
                sqlcom.Parameters.Add("@note", SqlDbType.NVarChar).Value = note;
            }
            sqlcom.Parameters.Add("@statusId", SqlDbType.Int).Value = statusId;
            if (address == null)
            {
                sqlcom.Parameters.Add("@customerAddress", SqlDbType.NVarChar).Value = DBNull.Value;
            }
            else
            {
                sqlcom.Parameters.Add("@customerAddress", SqlDbType.NVarChar).Value = address;
            }
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            orderId = Convert.ToInt32(sqlRead[0]);
            foreach (OrderDetails od in lod)
            {
                sqlcom2.Parameters.Clear();
                sqlcom2.Parameters.Add("@orderID", SqlDbType.Int).Value = orderId;
                sqlcom2.Parameters.Add("@dishID", SqlDbType.Int).Value = od.id;
                sqlcom2.Parameters.Add("@dishAmount", SqlDbType.Int).Value = od.amount;
                sqlcom2.ExecuteNonQuery();
            }
            conn.Close();
        }
        public static void addImport(int ovadiscount, int shipFee, int sourceId, int staffId, List<ImportDetail> lid)
        {
            int importId;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!; MultipleActiveResultSets=True";
            var sql = "insert into Import OUTPUT inserted.importID values (GETDATE(), @discount, @shipFee, @staffId,@sourceId)";
            var insertID = "insert into Import_Product values (@importID, @productID, @unitPrice,@discount,@amount)";
            var conn = new SqlConnection(url);
            var sqlcom = new SqlCommand(sql, conn);
            var sqlcom2 = new SqlCommand(insertID, conn);
            conn.Open();
            sqlcom.Parameters.Add("@discount", SqlDbType.Int).Value = ovadiscount;
            sqlcom.Parameters.Add("@shipFee", SqlDbType.Int).Value = shipFee;
            sqlcom.Parameters.Add("@sourceId", SqlDbType.Int).Value = sourceId;
            sqlcom.Parameters.Add("@staffId", SqlDbType.Int).Value = staffId;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            importId = Convert.ToInt32(sqlRead[0]);
            foreach (ImportDetail id in lid)
            {
                sqlcom2.Parameters.Clear();
                sqlcom2.Parameters.Add("@importID", SqlDbType.Int).Value = importId;
                sqlcom2.Parameters.Add("@productID", SqlDbType.Int).Value = id.productId;
                sqlcom2.Parameters.Add("@unitPrice", SqlDbType.Int).Value = id.unitPrice;
                sqlcom2.Parameters.Add("@discount", SqlDbType.Int).Value = id.discounted;
                sqlcom2.Parameters.Add("@amount", SqlDbType.Int).Value = id.amount;
                sqlcom2.ExecuteNonQuery();
            }
            conn.Close();
        }
        public static bool checkAdm(string acc, string pwd)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
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
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }
        public static string GetStaffName(string username)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select staffName from Staff where staffId = @staffid";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@staffid", SqlDbType.Int).Value = Int32.Parse(GetStaffId(username));
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }
        public static string GetStaffNameWithID(int id)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select staffName from Staff where staffId = @staffid";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@staffid", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }
        public static string GetProductNameWithID(int id)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select productName from Product where productId = @id";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }
        public static string GetProductUnitWithID(int id)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select productUnit from Product where productId = @id";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }
        public static string GetSourceNameWithID(int id)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select sourceName from Source where SourceId = @id";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }
        public static string GetStatusWithID(int id)
        {
            string status;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select statusName from Status where statusId = @id";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            status = sqlRead[0].ToString();
            conn.Close();
            return status;
        }

        public static string GetDishName(string dishId)
        {
            string name;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select dishName from Dish where dishId = @dishid";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@dishid", SqlDbType.Int).Value = Int32.Parse(dishId);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            name = sqlRead[0].ToString();
            conn.Close();
            return name;
        }

        public static int GetDishPrice(int dishId)
        {
            int price;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select dishPrice from Dish where dishId = @dishid";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@dishid", SqlDbType.Int).Value = dishId;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            price = Int32.Parse(sqlRead[0].ToString());
            conn.Close();
            return price;
        }

        public static string GetStaffId(string username)
        {
            string id;
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            string sql = "Select staffId from Staff where accId = (select accID from Account where username = @username)";
            var sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            while (sqlRead.Read())
            {
                id = sqlRead[0].ToString();
                conn.Close();
                return id;
            }
            conn.Close();
            return null;
        }

        public static List<Dish> getDish()
        {
            String sql = "select dish.dishId,dish.dishName,dish.dishprice,dish.dishDes, dishtype.dishtypename from dish inner join dishtype on dish.dishtypeid = dishtype.dishtypeid";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Dish> ld = new List<Dish>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            //Dish dish = new Dish();
            ld = (from DataRow dr in dt.Rows select new Dish() {
                id = Convert.ToInt32(dr["DishID"]),
                name = dr["DishName"].ToString(),
                price = Convert.ToInt32(dr["DishPrice"]),
                des = dr["DishDes"].ToString(),
                dishtype = dr["DishTypeName"].ToString()
            }).ToList();
            return ld;
        }
        public static List<Rank> getRank()
        {
            String sql = "select rankId,rankName from Rank";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Rank> ld = new List<Rank>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            ld = (from DataRow dr in dt.Rows
                  select new Rank()
                  {
                      id = Convert.ToInt32(dr["rankId"]),
                      rank = dr["rankName"].ToString()
                  }).ToList();
            return ld;
        }
        public static List<DishType> getDishType()
        {
            String sql = "select * from dishtype";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<DishType> ldt = new List<DishType>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            //Dish dish = new Dish();
            ldt = (from DataRow dr in dt.Rows
                   select new DishType()
                   {
                       id = Convert.ToInt32(dr["DishTypeId"]),
                       type = dr["DishTypeName"].ToString(),
                   }).ToList();
            return ldt;
        }
        public static int getDishPrice(int id)
        {
            int price;
            string sql = "select dishprice from Dish where dishid = @id";
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand sqlcom = new SqlCommand(sql, conn);
            sqlcom.Parameters.Add("@id", SqlDbType.Int).Value = id;
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            while (sqlRead.Read())
            {
                price = Convert.ToInt32(sqlRead[0]);
                conn.Close();
                return price;
            }
            conn.Close();
            return 0;
        }

        public static List<Product> GetProducts()
        {
            String sql = "select * from Product";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Product> lp = new List<Product>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lp = (from DataRow dr in dt.Rows
                  select new Product()
                  {
                      id = Convert.ToInt32(dr["productId"]),
                      name = dr["productName"].ToString(),
                      unit = dr["productUnit"].ToString()                    
                  }).ToList();
            return lp;
        }
        public static List<Source> GetSources()
        {
            String sql = "select * from Source";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Source> ls = new List<Source>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            ls = (from DataRow dr in dt.Rows
                  select new Source()
                  {
                      id = Convert.ToInt32(dr["sourceId"]),
                      source = dr["sourceName"].ToString(),
                      address = dr["sourceAddress"].ToString()
                  }).ToList();
            return ls;
        }
        public static List<Import> getImportToday()
        {
            String sql = "select * from Import where ImportDate between Dateadd(DAY, Datediff(DAY, 0, Getdate()) , 0) and DATEADD(second, -1, Dateadd(DAY, Datediff(DAY, 0, Getdate() + 1), 0)) ; ";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Import> li = new List<Import>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            li = (from DataRow dr in dt.Rows
                  select new Import()
                  {
                      id = Convert.ToInt32(dr["importId"]),
                      date = (DateTime)(dr["ImportDate"]),
                      overallDiscount = Convert.ToInt32(dr["overallDiscount"]),
                      shipFee = Convert.ToInt32(dr["shipFee"]),
                      sourceid = Convert.ToInt32(dr["sourceId"]),
                      staffId = Convert.ToInt32(dr["staffId"])
                  }).ToList();
            return li;
        }

        public static List<Order> getOrdersToday()
        {
            String sql = "select * from [Order] where orderDate between Dateadd(DAY, Datediff(DAY, 0, Getdate()) , 0) and DATEADD(second, -1, Dateadd(DAY, Datediff(DAY, 0, Getdate() + 1), 0)) ; ";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Order> lo = new List<Order>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lo = (from DataRow dr in dt.Rows
                  select new Order()
                  {
                      id = Convert.ToInt32(dr["OrderId"]),
                      staffid = Convert.ToInt32(dr["staffId"]),
                      Date = (DateTime)dr["orderDate"],
                      discount = Convert.ToInt32(dr["discount"]),
                      note = dr["note"].ToString(),
                      statusId = Convert.ToInt32(dr["statusID"]),
                      address = dr["customerAddress"].ToString()
                  }).ToList();
            return lo;
        }
        public static List<Order> getOrders()
        {
            String sql = "select * from [Order]";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Order> lo = new List<Order>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lo = (from DataRow dr in dt.Rows
                  select new Order()
                  {
                      id = Convert.ToInt32(dr["OrderId"]),
                      staffid = Convert.ToInt32(dr["staffId"]),
                      Date = (DateTime)dr["orderDate"],
                      discount = Convert.ToInt32(dr["discount"]),
                      note = dr["note"].ToString(),
                      statusId = Convert.ToInt32(dr["statusID"]),
                      address = dr["customerAddress"].ToString()
                  }).ToList();
            return lo;
        }
        public static List<Import> getImport()
        {
            String sql = "select * from Import";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Import> li = new List<Import>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            li = (from DataRow dr in dt.Rows
                  select new Import()
                  {
                      id = Convert.ToInt32(dr["importId"]),
                      date = (DateTime) (dr["ImportDate"]),
                      overallDiscount = Convert.ToInt32(dr["overallDiscount"]),
                      shipFee = Convert.ToInt32(dr["shipFee"]),
                      sourceid = Convert.ToInt32(dr["sourceId"]),
                      staffId = Convert.ToInt32(dr["staffId"])
                  }).ToList();
            return li;
        }
        public static List<Account> GetAccount()
        {
            String sql = "select * from [Account]";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<Account> la = new List<Account>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            la = (from DataRow dr in dt.Rows
                  select new Account()
                  {
                      id = Convert.ToInt32( dr["accId"]),
                      username = dr["username"].ToString(),
                      password = dr["password"].ToString(),
                      date = (DateTime) dr["createdDate"],
                      rank = Convert.ToInt32(dr["rankId"])
                  }).ToList();
            return la;
        }
        public static List<Staff> GetStaffWithId(int id)
        {
            var conn = setConnection();
            conn.Open();
            String sql = "select * from Staff where accID = '" + id + "'";
            var sqlda = new SqlDataAdapter(sql, conn);
            List<Staff> staff = new List<Staff>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            staff = (from DataRow dr in dt.Rows
                     select new Staff()
                     {
                         id = Convert.ToInt32(dr["staffID"]),
                         name = dr["staffName"].ToString(),
                         phone = dr["staffPhone"].ToString(),
                         email = dr["staffEmail"].ToString(),
                     }).ToList();
            conn.Close();
            return staff;
        }
        public static List<OrderDetails> GetOrderDetails()
        {
            String sql = "select * from [Order_Item]";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<OrderDetails> lo = new List<OrderDetails>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lo = (from DataRow dr in dt.Rows
                  select new OrderDetails()
                  {
                      id = Convert.ToInt32(dr["dishId"]),
                      amount = Convert.ToInt32(dr["dishAmount"]),
                      orderId = Convert.ToInt32(dr["orderID"])
                  }).ToList();
            return lo;
        }
        public static List<OrderDetails> GetOrderDetailGrouped()
        {
            String sql = "select SUM(dishAmount) as 'dishAmount',dishID from Order_Item group by dishID";
            var sqlda = new SqlDataAdapter(sql, setConnection());
            List<OrderDetails> lo = new List<OrderDetails>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lo = (from DataRow dr in dt.Rows
                  select new OrderDetails()
                  {
                      id = Convert.ToInt32(dr["dishID"]),
                      amount = Convert.ToInt32(dr["dishAmount"])
                  }).ToList();
            return lo;
        }
        public static int GetAllDiscounted()
        {
            int discounted = 0;
            var conn = setConnection();
            conn.Open();
            string sql = "select SUM(discount) as 'discounted' from [Order] group by discount";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            if (sqlRead.Read())
            {
                discounted = Convert.ToInt32(sqlRead[0]);
            }
            conn.Close();
            return discounted;
        }
        public static int GetSpend()
        {
            int spend = 0;
            var conn = setConnection();
            conn.Open();
            string sql = "select sum((unitPrice*amount)-discount) from Import_Product";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            if (sqlRead.Read())
            {
                if(sqlRead[0] is DBNull)
                {
                    return spend;
                }    
                spend = Convert.ToInt32(sqlRead[0]);
                return spend;
            }
            conn.Close();
            return spend;
        }
        public static int GetBuyingDiscount()
        {
            int discounted = 0;
            var conn = setConnection();
            conn.Open();
            string sql = "select sum(shipFee - overallDiscount) from Import";
            var sqlcom = new SqlCommand(sql, conn);
            SqlDataReader sqlRead = sqlcom.ExecuteReader();
            sqlRead.Read();
            if (sqlRead.Read())
            {
                discounted = Convert.ToInt32(sqlRead[0]);
            }
            conn.Close();
            return discounted;
        }
        public static List<OrderDetails> getOrderDetailsWithId(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            String sql = "select * from Order_Item where orderID = '" + id + "'";
            var sqlda = new SqlDataAdapter(sql, conn);
            List<OrderDetails> lod = new List<OrderDetails>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lod = (from DataRow dr in dt.Rows
                   select new OrderDetails()
                   {
                       id = Convert.ToInt32(dr["DishID"]),
                       amount = Convert.ToInt32(dr["dishAmount"]),
                       orderId = Convert.ToInt32(dr["orderID"])
                   }).ToList();
            conn.Close();
            return lod;
        }
        public static List<ImportDetail> getImportDetailsWithId(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            String sql = "select * from Import_Product where ImportID = '" + id + "'";
            var sqlda = new SqlDataAdapter(sql, conn);
            List<ImportDetail> lid = new List<ImportDetail>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lid = (from DataRow dr in dt.Rows
                   select new ImportDetail()
                   {
                       productId = Convert.ToInt32(dr["productId"]),
                       importId = Convert.ToInt32(dr["importId"]),
                       unitPrice = Convert.ToInt32(dr["unitPrice"]),
                       discounted = Convert.ToInt32(dr["discount"]),
                       amount = Convert.ToInt32(dr["amount"]),
                   }).ToList();
            conn.Close();
            return lid;
        }
        public static List<OrderDetails> getOrderDetailsWithDishId(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            String sql = "select * from Order_Item where DishID = '" + id + "'";
            var sqlda = new SqlDataAdapter(sql, conn);
            List<OrderDetails> lod = new List<OrderDetails>();
            DataTable dt = new DataTable();
            sqlda.Fill(dt);
            lod = (from DataRow dr in dt.Rows
                   select new OrderDetails()
                   {
                       id = Convert.ToInt32(dr["DishID"]),
                       amount = Convert.ToInt32(dr["dishAmount"]),
                       orderId = Convert.ToInt32(dr["orderID"]),
                   }).ToList();
            conn.Close();
            return lod;
        }

        public static void deleteOrders(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete from Order_Item where orderID = @id Delete From [Order] Where orderID = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void deleteImport(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("delete from Import_Product where importId = @id Delete From Import Where importId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void deleteDish(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from Dish where DishId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void deleteProduct(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from Product where productId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void deleteSource(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from Source where sourceID = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        public static void deleteAccount(int id)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("Delete from Staff where accId = @id delete from Account where accID = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void changePass(int id, string password)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Account set password = @password where accID = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public static void changeStaffInfo(int id, string name,string phone, string email)
        {
            var url = "server=137.59.106.96 ; database =dBforStudying;uid=uonga0.HzL12153;pwd=Tuong1998!";
            var conn = new SqlConnection(url);
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Staff set staffName=@name,staffPhone=@phone,staffEmail=@email where staffId = @id", conn);
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
            cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = phone;
            cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}