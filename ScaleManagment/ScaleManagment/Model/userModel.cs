using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ScaleManagment.Model
{
    public enum UserType
    {
        Opperator = 1,
        Farmer = 2,
        Driver = 3,
        Administrator = 4
    
    }


    public class userModel 
    {
        public int id{ get; set; }
        public string name { get; set; }
        public string family { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string melli{ get; set; }
        public string carPelak { get; set; }
        public string address { get; set; }
        public string insertDate { get; set; }
        public string location { get; set; }
        public string description { get; set; }
        public string typeTitle { get {
                switch (userType)
                {
                    case UserType.Opperator: return "اپراتور";  
                    case UserType.Farmer: return "کشاورز"; 
                    case UserType.Driver: return "راننده"; 
                    case UserType.Administrator: return "مدیر";
                    default : 
                        return "تعیین نشده";
                }
            } }
        public UserType userType { get; set; }
        
        
        public userModel()
        {

        }


        public userModel(int userId) 
        {
            var data= getUser(userId);

            id = data.id;
            name = data.name;
            family = data.family;
            userName = data.userName;
            melli = data.melli;
            carPelak = data.carPelak;
            address = data.address;
            location = data.location;
            userType = data.userType;


        }

        public userModel getUser(int id)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("SELECT * from users where id = '"+id.ToString()+"'", BaseData.Connection);
            var reader = command.ExecuteReader();
            reader.Read();

            return map(reader); 

        }

        public List<userModel> quickSearch(string q)
        {


            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("SELECT top 20 * from users where name like  '%" + name + "%' or  family like  '%" + name + "%'", BaseData.Connection);
            var reader = command.ExecuteReader();

            List<userModel> result = new List<userModel>(); 
            while (reader.Read())
            {
                result.Add( map(reader));
            }

            BaseData.Connection.Close(); 
            return result; 

        }


        public List<userModel> search(userModel data)
        {


            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            string query = "SELECT top 20  * from users where 1=1 ";

            if (data.name != "")
            {
                query += " and name like '%" + data.name + "%'";                  
            }

            if (data.family != "")
            {
                query += " and family like '%" + data.family + "%'";
            }

            if (data.melli != "")
            {
                query += " and melli like '%" + data.melli + "%'";
            }

            if (data.location != "")
            {
                query += " and location like '%" + data.location + "%'";
            }


            if (data.id > 0 )
            {
                query += " and id= " + data.id ;
            }

            if (data.userType != UserType.Administrator   )
            {
                query += " and user_type =" + (int)data.userType;
            }

            SqlCeCommand command = new SqlCeCommand(query, BaseData.Connection);
            var reader = command.ExecuteReader();

            List<userModel> result = new List<userModel>();
            while (reader.Read())
            {
                result.Add(map(reader));
            }

            BaseData.Connection.Close();
            return result;

        }


        public List<userModel> getUsers(UserType ?userType)
        {
            string query = "SELECT top 20 * from users where 1=1 "; 
            if (userType != null)
            {
              
                query += " and  user_type=" + (int)userType; 

            }

            if (BaseData.CurrentUserType == UserType.Opperator)
            {
                query += " and user_type<> 4";
            }

            if(BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand(query, BaseData.Connection);
            var reader = command.ExecuteReader();

            List<userModel> result = new List<userModel>(); 
            while (reader.Read())
            {
                result.Add( map(reader));
            }

            BaseData.Connection.Close(); 
            return result; 

        }


        public userModel map(SqlCeDataReader reader)
        {
            userModel result = new userModel();
            try
            {
                result.id = Convert.ToInt16(reader["id"]);
                result.name = reader["name"].ToString();
                result.family = reader["family"].ToString();
                result.userName = reader["userName"].ToString();
                result.melli = reader["melli"].ToString();
                result.carPelak = reader["car_pelak"].ToString();
                result.address = reader["address"].ToString();
                result.location = reader["location"].ToString();

                result.userType = (UserType)reader["User_type"];
            }
            catch
            {
            }
            return result; 

        }


        public void save()
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = null;
            
            
            if (id >  0 ) // update
            {
                command = new SqlCeCommand(
                                    @"update users set  " +
                                    "name='" + name + "'," +
                                    "family= '" + family + "'," +
                                    "username= '" + userName + "'," +
                                    "create_date= '" + insertDate + "'," +
                                    "address= '" + address + "'," +
                                    "melli= '" + melli+ "'," +
                                    "location= '" + location+ "'," +
                                    "car_pelak= '" + carPelak+ "', " +
                                    "description= '" + description + "'," +
                                    "user_type= '" + ((int)userType)  +"'"+
                                             " where id=  " + id 
                                    , BaseData.Connection);
            }
            else   //save
             {
                PersianCalendar calendar = new PersianCalendar();

                string date = calendar.GetDayOfMonth(DateTime.Now).ToString() + "/" +
                    calendar.GetMonth(DateTime.Now).ToString() + "/" +
                    calendar.GetYear(DateTime.Now).ToString() + " "+
                    calendar.GetHour(DateTime.Now).ToString() + ":" +
                    calendar.GetMinute(DateTime.Now).ToString() + ":" +
                    calendar.GetSecond(DateTime.Now).ToString();

                command = new SqlCeCommand(
                    @"insert into users (
                    name,family,username,password,
                    create_date,address,melli,location,
                    car_pelak,description,user_type )
                    values(" +
                    "'" + name + "'," +
                    "'" + family + "'," +
                    "'" + userName + "'," +
                    "''," + // pass
                    "'" + date + "'," + 
                    "'" + address + "'," +
                    "'" + melli+ "'," + 
                    "'" + location+ "'," + 
                    "'" + carPelak + "'," + 
                    "'" + description + "'," + 
                    "'" + ((int)userType) + "')"
                    , BaseData.Connection);
                
            }
            try
            {
                command.ExecuteNonQuery();

                command = new SqlCeCommand("SELECT @@IDENTITY", BaseData.Connection);
                var reader = command.ExecuteReader();
                reader.Read();

                id = Convert.ToInt32(reader[0]);
            }
            catch
            { }

        }


        public override  string ToString()
        {
            return name + " " + family; 
        }
    }
}
