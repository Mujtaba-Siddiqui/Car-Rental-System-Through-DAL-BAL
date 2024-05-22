namespace LoginForm.BOL
{
    public class UserCredentials
    {
        public string ServerIp { get; set; }
        public string DbName { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
    }

    public class CarReg_
    {
        public string RegNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Available { get; set; }
    }

    public class AutoNoid
    {
        public string c_Id { get; set; }
        public string c_Name { get; set; }
        public string c_address { get; set; }
        public string c_mobile { get; set;}
    }

    public class RentalInfo
    {
        public string CarID { get; set; }
        public string CustomerId { get; set; }
        public string DueDate { get; set; }
        public string ElapsedDays { get; set; }
        public string Fine { get; set; }
    }

}
