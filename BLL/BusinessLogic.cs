using LoginForm.DAL;
using System;
using System.Collections.Generic;
using Microsoft.Win32;
using LoginForm.BOL;
using System.Xml.Linq;
using System.Data;
using static LoginForm.DAL.DataAccess;

namespace LoginForm.BLL
{
    public class BusinessLogic
    {
        private DataAccess dataAccess;

        public BusinessLogic()
        {
            string serverIp = ReadFromRegistry("ServerIP");
            string dbName = ReadFromRegistry("DatabaseName");
            string username = ReadFromRegistry("UserName");
            string userpassword = ReadFromRegistry("UserPassword");

            if (!string.IsNullOrEmpty(serverIp) && !string.IsNullOrEmpty(dbName) &&
                !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(userpassword))
            {
                dataAccess = new DataAccess(serverIp, dbName, username, userpassword);
            }
        }

        public bool ConnectToDatabase(string serverIp, string dbName, string username, string userpassword)
        {
            dataAccess = new DataAccess(serverIp, dbName, username, userpassword);
            return dataAccess.TestConnection();
        }

        public void SaveToRegistry(string keyName, string value)
        {
            try
            {
                using (RegistryKey regKey = Registry.CurrentUser.CreateSubKey("Software\\OOAD\\ServerValues"))
                {
                    if (regKey != null)
                    {
                        regKey.SetValue(keyName, value, RegistryValueKind.String);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error saving to registry", ex);
            }
        }

        public string ReadFromRegistry(string keyName)
        {
            using (RegistryKey regkey = Registry.CurrentUser.OpenSubKey("Software\\OOAD\\ServerValues"))
            {
                if (regkey != null)
                {
                    object value = regkey.GetValue(keyName);
                    if (value != null)
                        return value.ToString();
                }
                return "";
            }
        }

        public void DeleteRegistryValue(string keyName)
        {
            try
            {
                using (RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\OOAD\\ServerValues", true))
                {
                    if (regKey != null)
                    {
                        regKey.DeleteValue(keyName, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting registry value", ex);
            }
        }

        public bool IsRegistryEmpty()
        {
            string serverIP = ReadFromRegistry("ServerIP");
            string dbName = ReadFromRegistry("DatabaseName");
            string username = ReadFromRegistry("UserName");
            string userpassword = ReadFromRegistry("UserPassword");

            return string.IsNullOrEmpty(serverIP) || string.IsNullOrEmpty(dbName) ||
                   string.IsNullOrEmpty(username) || string.IsNullOrEmpty(userpassword);
        }

        public void InsertCar(string regNo, string Make, string Model, string Avail)
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            dataAccess.InsertData(regNo, Make, Model, Avail);
        }

        public object SelectCar()
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            return dataAccess.selectData();
        }

        public void UpdateCar(string regNo, string Make, string Model, string Avail)
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            dataAccess.updtCar(regNo, Make, Model, Avail);
        }
        public void DeleteCar(string regNo)
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            dataAccess.dltCar(regNo);
        }

        public string C_AutoInc()
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            return dataAccess.Autono();
        }

        public void Cus_Insert(string c_id, string c_name, string c_address, string c_mobile)
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }

            dataAccess.customer_insert(c_id,c_name,c_address,c_mobile);
        }
        public void Cus_Update(string c_id, string c_name, string c_address, string c_mobile)
        {
            EnsureDataAccessInitialized();
            dataAccess.CustomerUpdate(c_id, c_name, c_address, c_mobile);
        }
        public void Cus_Delete(string c_id)
        {
            EnsureDataAccessInitialized();
            dataAccess.CustomerDelete(c_id);
        }

        public DataTable GetCustomers()
        {
            EnsureDataAccessInitialized();
            return dataAccess.GetCustomers();
        }

        public string GetAvailable(string regno)
        {
            EnsureDataAccessInitialized();
            return dataAccess.CarAvail(regno);
        }

        public string GetCarCusId(string c_id)
        {
            EnsureDataAccessInitialized();
            return dataAccess.CarCusId(c_id);
        }
        public RentalInfo GetRentalInfo(string carId)
        {
            EnsureDataAccessInitialized();
            return dataAccess.ReturnCustId(carId);
        }


        public void InsertReturn(string car_id, string cus_id, string date, string elp, string fine)
        {
            EnsureDataAccessInitialized();
            dataAccess.ReturnInsert(car_id, cus_id, date, elp, fine);
        }

        public void InsertRental(string carId, string custId,string custName, string rentalFee, string issueDate, string dueDate)
        {
            EnsureDataAccessInitialized();
            dataAccess.InsertRental(carId, custId, custName, rentalFee, issueDate, dueDate);
        }

        public DataTable GetReturnTable()
        {
            EnsureDataAccessInitialized() ;
            return dataAccess.ReturnTable();
        }
        public DataTable GetRentalCustomers()
        {
            EnsureDataAccessInitialized();
            return dataAccess.RentalTable();
        }
        private void EnsureDataAccessInitialized()
        {
            if (dataAccess == null)
            {
                throw new InvalidOperationException("Database connection is not initialized. Call ConnectToDatabase first.");
            }
        }
        public DataTable GetCarIds()
        {
            EnsureDataAccessInitialized();
            return dataAccess.GetCarIds();
        }
       
    }



}
