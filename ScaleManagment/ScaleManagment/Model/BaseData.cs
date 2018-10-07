using System;
using System.Collections.Generic;
using System.Data.SqlServerCe;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleManagment.Model
{
    public class data {
        public int id { get; set; }
        public string name { get; set; }
        public int ownerUserId{ get; set; }
        public string description { get; set; }

        public data() { }
        public data getData(int id, string objectType)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("SELECT  * from " + objectType +" where id = " +id  , BaseData.Connection);

            data result=null; 

            try
            {
                var reader = command.ExecuteReader();
                reader.Read();
                if (objectType == "items")
                    result = new Model.data(Convert.ToInt16(reader["id"]), reader["name"].ToString(), reader["description"].ToString());
                else
                {
                    result = new Model.data(Convert.ToInt16( reader["id"]), reader["pelak"].ToString(), reader["description"].ToString());
                    result.ownerUserId = Convert.ToInt16(reader["owner_user_id"]); 
                }
            }
            catch
            {
            }
            return result; 
        }
        public data(int _id, string _name)
        {
            id = _id;
            name= _name; 
        }
        public data(int _id, string _name,string _description)
        {
            id = _id;
            name= _name;
            description= _description;
        }
        public List<data> search(data q,string objectType)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            string query = "SELECT  top 20 * from " + objectType + " where 1=1 " ;

            if (q.id > 0 )
            {
                query += " and id = " + q.id; 
            }
            
            if (q.name != "" )
            {
                if (objectType == "items")
                    query += " and name like '%" + q.name + "%'";
                else 
                    query += " and pelak like '%" + q.name + "%'";
            }

            if (q.description != "")
            {
                query += " and description like '%" + q.description + "%'";
            }


            SqlCeCommand command = new SqlCeCommand(query, BaseData.Connection);

            List<data> result = new List<data>();

            try
            {
                var reader = command.ExecuteReader();
               
                while (reader.Read())
                {
                    if (objectType == "items")
                        result.Add(new Model.data(Convert.ToInt16(reader["id"]), reader["name"].ToString(), reader["description"].ToString()));
                    else
                        result.Add(new Model.data(Convert.ToInt16(reader["id"]), reader["pelak"].ToString(), reader["description"].ToString()));
                }
            }
            catch
            {
            }
                 

            return result;
        }

        public static List<data> getDataList(string objectType)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("SELECT top 20 * from " + objectType, BaseData.Connection);

            List<data> result = new List<data>();

            try
            {
                var reader = command.ExecuteReader();
                 
                while (reader.Read())
                {
                    if (objectType == "items")
                        result.Add(new Model.data(Convert.ToInt16(reader["id"]), reader["name"].ToString(), reader["description"].ToString()));
                    else
                        result.Add(new Model.data(Convert.ToInt16(reader["id"]), reader["pelak"].ToString(), reader["description"].ToString()));
                }
            }
            catch
            {
            }
            return result;
        }

        public override string ToString()
        {
            return name;
        }



    }
    class BaseData
    {
        //public static string ConnectionString = @"Data Source=127.0.0.1;Initial Catalog=DecisionForTomorrow;Integrated Security=True";
        public static string ConnectionString = @"Data Source=db.sdf;Password=game@1393";

        public static SqlCeConnection Connection=null ; 

        public static string DBPath = "";

        public static int CurrentUser;

        public static UserType CurrentUserType= UserType.Opperator;

        public static userModel currentUserData { get; set; }


        public static frmMain frmMain;


        public static bool createDB()
        {
            //File.Create(BaseData.DBPath);

            BaseData.ConnectionString = "Data Source=" + BaseData.DBPath + ";Password=1396";

            BaseData.DBPath = BaseData.DBPath;
            SqlCeEngine engine = new SqlCeEngine(BaseData.ConnectionString);
            engine.CreateDatabase();



            try
            {
                BaseData.Connection = new SqlCeConnection(BaseData.ConnectionString);
                BaseData.Connection.Open();

                SqlCeCommand cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"
CREATE TABLE [cars]
(
   [id] INT NOT NULL IDENTITY (100,1),
   [owner_user_id] INT,
   [pelak] NVARCHAR(20) NULL,
   [description] NVARCHAR(2000) NULL
);";
                cmd.ExecuteNonQuery();

                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"ALTER TABLE [cars] ADD CONSTRAINT [PK__cars__000000000000006F] PRIMARY KEY ([id]);";
                cmd.ExecuteNonQuery();



                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"CREATE TABLE [items]
(
   [id] INT NOT NULL IDENTITY (100,1),
   [name] NVARCHAR(100)  NULL,
   [description] NVARCHAR(2000) NULL
);

";
                cmd.ExecuteNonQuery();


                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"ALTER TABLE [items] ADD CONSTRAINT [PK__items__0000000000000061] PRIMARY KEY ([id]);";
                cmd.ExecuteNonQuery();


                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"CREATE TABLE [users]
(
   [id] INT NOT NULL IDENTITY (100,1),
   [username] NVARCHAR(100) NULL,
   [password] NVARCHAR(100) NULL,
   [create_date] NVARCHAR(20) NULL,
   [name] NVARCHAR(50) NULL,
   [family] NVARCHAR(50) NULL,
   [address] NVARCHAR(2000) NULL,
   [melli] NVARCHAR(20) NULL,
   [location] NVARCHAR(50) NULL,
   [car_pelak] NVARCHAR(20) NULL,
   [user_type] INT NULL,
   [description] NVARCHAR(2000) NULL
);
";
                cmd.ExecuteNonQuery();


                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"ALTER TABLE [users] ADD CONSTRAINT [PK__users__000000000000002E] PRIMARY KEY ([id]);";
                cmd.ExecuteNonQuery();



                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"insert into users(username,password,user_type) values('admin','123',1);";
                cmd.ExecuteNonQuery();



                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"CREATE TABLE [weight]
(
   [id] INT NOT NULL IDENTITY (100,1),
   [owner_user_id] INT NULL,
   [driver_user_id] INT NULL,
   [opperator_user_id] INT NULL,
   [serial] NVARCHAR(20) NULL,
   [insert_date] NVARCHAR(20) NULL,
   [insert_time] NVARCHAR(20) NULL,
   [weight] FLOAT NULL,
   [weight2] FLOAT NULL,
   [car_id] INT  NULL,
   [item_type] INT  NULL,
   [positive_decrease] FLOAT NULL,
   [negative_decrease] FLOAT NULL,
   [pest_percent] FLOAT NULL,
   [description] NVARCHAR(2000) NULL
);";
                cmd.ExecuteNonQuery();


                cmd = BaseData.Connection.CreateCommand();
                cmd.CommandText = @"ALTER TABLE [weight] ADD CONSTRAINT [PK__weight__0000000000000055] PRIMARY KEY ([id]);";
                cmd.ExecuteNonQuery();

            }
            catch
            {
                return false;
            }
            finally
            {
                BaseData.Connection.Close();

            }

            return true;


        }

        public static void removeItem(int id , string objectType)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("delete  from " + objectType + " where id = "+ id  , BaseData.Connection);

            command.ExecuteNonQuery(); 
        }
    }
}
