using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using Microsoft.SqlServer.Server;
using System.Runtime.InteropServices;
using LoginForm.BOL;


namespace LoginForm.DAL
{
    public class DataAccess
    {
        private string connectionString;
        public SqlCommand cmd;
        public SqlCommand cmd1;
        public SqlDataReader dr;
        bool mode = true;
        string sql;
        public DataAccess(string serverIp, string dbName, string username, string userpassword)
        {
            connectionString = $"Data Source={serverIp},49170;Initial Catalog={dbName};User ID={username};Password={userpassword};";
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection sqlconnect = new SqlConnection(connectionString))
                {
                    sqlconnect.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void InsertData(string regNo, string make, string model, string available)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("insert into carreg values ('" + regNo + "','" + make + "','" + model + "','" + available + "')", connectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
        }

        public object selectData()
        {
            SqlDataAdapter adapter = new SqlDataAdapter("select * from carreg", connectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }

        public void updtCar(string regNo, string make, string model, string available)
        {   
            
            
            SqlDataAdapter adapter = new SqlDataAdapter("update carreg set make = '"+make+"', model ='"+model+ "', available = '" + available+"' where regno = '"+regNo+"'", connectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
        }

        public void dltCar(string regNo)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("delete from carreg where regno = '" + regNo + "'", connectionString);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
        }

        public string Autono()
        {
            sql = "SELECT TOP 1 c_id FROM Customer ORDER BY c_id DESC";
            string newCustomerId;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    int id = int.Parse(dr["c_id"].ToString()) + 1;
                    newCustomerId = id.ToString("D4");
                }
                else
                {
                    newCustomerId = "0001";
                }

                dr.Close();
            }

            return newCustomerId;
        }

        public void customer_insert(string c_id, string c_name, string c_address, string c_mobile)
        {
            
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                if (mode == true) {
                    sql = "insert into Customer(c_id, c_name, c_address, c_mobile)values(@c_id,@c_name,@c_address,@c_mobile)";
                    conn.Open();
                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@c_id", c_id);
                    cmd.Parameters.AddWithValue("@c_name", c_name);
                    cmd.Parameters.AddWithValue("@c_address", c_address);
                    cmd.Parameters.AddWithValue("@c_mobile", c_mobile);
                    cmd.ExecuteNonQuery();

                }
            }
        }
        public void CustomerUpdate(string c_id, string c_name, string c_address, string c_mobile)
        {
            string query = "UPDATE Customer SET c_name = @c_name, c_address = @c_address, c_mobile = @c_mobile WHERE c_id = @c_id";
            ExecuteNonQuery(query, new SqlParameter("@c_id", c_id),
                                    new SqlParameter("@c_name", c_name),
                                    new SqlParameter("@c_address", c_address),
                                    new SqlParameter("@c_mobile", c_mobile));
        }
        public void CustomerDelete(string c_id)
        {
            string query = "DELETE FROM Customer WHERE c_id = @c_id";
            ExecuteNonQuery(query, new SqlParameter("@c_id", c_id));
        }
        public DataTable GetCustomers()
        {
            string query = "SELECT c_id AS CustomerID, c_name AS [Customer Name], c_address AS Address, c_mobile AS Mobile FROM Customer";
            return ExecuteQuery(query);
        }


        public DataTable GetCarIds()
        {
            string query = "SELECT regno FROM carreg";
            return ExecuteQuery(query);
        }

        public string CarAvail(string regno)
        {
            string sql = "select available from carreg where regno = @regno";
            string availability;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@regno", regno);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    availability = dr["available"].ToString();
                }
                else
                {
                    availability = "Not Available";
                }
            }
            return availability;
        }

        public string CarCusId(string cid)
        {
            string sql = "select * from Customer where c_id = @cid";
            string cus_id;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", cid);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cus_id = dr["c_name"].ToString();
                }
                else
                {
                    cus_id = "Not Available";
                 
                }
            }
            return cus_id;
        }

        public void InsertRental(string carId, string custId, string cusName, string rentalFee, string issueDate, string dueDate)
        {
            string sql1 = "update carreg set available = 'NO' where regno = @regno";
            string sql = "INSERT INTO Rental (car_id, cust_id, cust_name, fee, date, due) VALUES (@carId, @custId, @cusName, @rentalFee, @issueDate, @dueDate)";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carId", carId);
                cmd.Parameters.AddWithValue("@custId", custId);
                cmd.Parameters.AddWithValue("@cusName", cusName);
                cmd.Parameters.AddWithValue("@rentalFee", rentalFee);
                cmd.Parameters.AddWithValue("@issueDate", issueDate);
                cmd.Parameters.AddWithValue("@dueDate", dueDate);
                conn.Open();
                cmd.ExecuteNonQuery();

                
                cmd1 = new SqlCommand(sql1, conn);
                
                cmd1.Parameters.AddWithValue("@regno", carId);
                cmd1.ExecuteNonQuery();
                conn.Close();
            }
        }

        public DataTable RentalTable()
        {
            string query = "SELECT car_id AS CarID, cust_id AS CustomerID, cust_name AS CustomerName, fee AS RentalFee, date AS IssuedDate,due AS DueDate FROM Rental";
            return ExecuteQuery(query);
        }

       

        public RentalInfo ReturnCustId(string carId)
        {
            string sql = "select car_id, cust_id, due, DATEDIFF(dd, due, GETDATE()) as elap from Rental where car_id = @carid";
            RentalInfo rentalInfo = new RentalInfo();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@carid", carId);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    rentalInfo.CustomerId = dr["cust_id"].ToString();
                    rentalInfo.DueDate = dr["due"].ToString();
                    string elap = dr["elap"].ToString();
                    int elapsedDays = int.Parse(elap);

                    if (elapsedDays > 0)
                    {
                        rentalInfo.ElapsedDays = elap;
                        int fine = elapsedDays * 100;
                        rentalInfo.Fine = fine.ToString();
                    }
                    else
                    {
                        rentalInfo.ElapsedDays = "0";
                        rentalInfo.Fine = "0";
                    }
                }
            }

            return rentalInfo;
        }
        public void ReturnInsert(string car_id, string cus_id, string date, string elp,string fine)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string sql = "INSERT INTO ReturnCar(car_id, cust_id, date, elapseddays, fine) VALUES (@car_id, @cus_id, @date, @elp, @fine)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@car_id", car_id);
                    cmd.Parameters.AddWithValue("@cus_id", cus_id);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@elp", elp);
                    cmd.Parameters.AddWithValue("@fine", fine);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public DataTable ReturnTable()
        {
            string query = "SELECT car_id AS CarID, cust_id AS CustomerID, date AS Date, elapseddays AS ElapsedDays, fine AS Fine FROM ReturnCar";
            return ExecuteQuery(query);
        }
        private void ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private DataTable ExecuteQuery(string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        
     
    }

}

    
