using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlServerCe;

namespace ScaleManagment.Model
{
    class weightModel
    {
        public int id { get; set; }

        public int ownerUserId { get; set; }

        public int driverUserId { get; set; }

        public int opperatorUserId { get; set; }

        public int itemTypeId { get; set; }

        public int carId { get; set; }

        public string date { get; set; }

        public string time { get; set; }

        public double weight { get; set; }

        public double weight2 { get; set; }

        public double pestPercent { get; set; }

        public double positiveDecreas { get; set; }

        public double negativeDecreas { get; set; }

        public string description { get; set; }

        public string serial { get; set; }

        public weightModel()
        {
            id = 0;
        }

        public weightModel(int id)
        {
            
        }

        public weightModel getWeight(int id)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();
            SqlCeCommand command =  new SqlCeCommand("SELECT * from weight where id = "+ id, BaseData.Connection);


            var reader = command.ExecuteReader();
            reader.Read();
            return map(reader);
        }

        private weightModel map(SqlCeDataReader reader)
        {
            weightModel result = new weightModel();

            try
            {

                result.id = (int)reader["id"];

                result.ownerUserId = (int)reader["owner_user_id"];

                result.driverUserId = (int)reader["driver_user_id"];

                result.opperatorUserId = (int)reader["opperator_user_id"];

                result.itemTypeId = (int)reader["item_type"];

                result.carId = (int)reader["car_id"];

                result.date = reader["insert_date"].ToString();

                result.time = reader["insert_time"].ToString();

                result.weight = (double)reader["weight"];

                result.weight2 = (double)reader["weight2"];

                result.pestPercent = (double)reader["pest_percent"];

                result.positiveDecreas = (double)reader["positive_decrease"];

                result.negativeDecreas = (double)reader["negative_decrease"];

                result.description = reader["description"].ToString();

                result.serial = reader["serial"].ToString();

            }
            catch
            {
            }
            return result; 
        }

        public int save()
        {
            PersianCalendar calendar = new PersianCalendar();

            string date = calendar.GetDayOfMonth(DateTime.Now).ToString() + "/" +
                calendar.GetMonth(DateTime.Now).ToString() + "/" +
                calendar.GetYear(DateTime.Now).ToString() + " ";

            string time = calendar.GetHour(DateTime.Now).ToString() + ":" +
                calendar.GetMinute(DateTime.Now).ToString() + ":" +
                calendar.GetSecond(DateTime.Now).ToString();

            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command;

            if (id == 0)
            {
                command = new SqlCeCommand(
                @"insert into weight (
                    owner_user_id,driver_user_id,opperator_user_id,
                    insert_date,insert_time,weight,weight2,car_id,item_type,
                    positive_decrease,negative_decrease,pest_percent,description,serial)
                    values(" +
                    "'" + ownerUserId + "'," +
                    "'" + driverUserId + "'," +
                    "'" + BaseData.CurrentUser + "'," +
                    "'" + date + "'," +
                    "'" + time + "'," +
                    "'" + weight + "'," +
                    "'" + weight2 + "'," +
                    "'" + carId + "'," +
                    "'" + itemTypeId + "'," +
                    "'" + positiveDecreas + "'," +
                    "'" + negativeDecreas + "'," +
                    "'" + pestPercent + "'," +
                    "'" + description + "'," +
                    "'" + serial + "')"
                    , BaseData.Connection);
            }
            else
            {
                command = new SqlCeCommand(
               @"update weight set  " +
                   "owner_user_id='" + ownerUserId + "'," +
                   "driver_user_id='" + driverUserId + "'," +
                   "opperator_user_id'=" + BaseData.CurrentUser + "'," +
                   "insert_date'=" + date + "'," +
                   "insert_time'=" + time + "'," +
                   "weight'=" + weight + "'," +
                   "weight2'=" + weight2 + "'," +
                   "car_id'=" + carId + "'," +
                   "item_type'=" + itemTypeId + "'," +
                   "positive_decrease'=" + positiveDecreas + "'," +
                   "negative_decrease'=" + negativeDecreas + "'," +
                   "pest_percent'=" + pestPercent + "'," +
                   "description'=" + description + "'," +
                   "serial'=" + serial +
                   "' where id =" + id
                   , BaseData.Connection);
            }

            try
            {


                command.ExecuteNonQuery();

                command = new SqlCeCommand("SELECT @@IDENTITY", BaseData.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                id = Convert.ToInt16(reader[0]);
                return id;
            }
            catch
            {
                return 0; 
            }

        }

        public int getMaxId()
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand("SELECT max(id)as cnt from weight", BaseData.Connection);
            var reader = command.ExecuteReader();
            reader.Read();
            int lastId = 0;

            try
            {
                lastId = (int)reader[0];
            }
            catch
            {
            }

            return lastId; 
        }



        public List<weightModel> getWeights()
        {
            string query = "SELECT top 20 * from weight  ";
           

            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand(query, BaseData.Connection);

            List<weightModel> result = new List<weightModel>();

            try
            {
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(map(reader));
                }
            }
            catch
            { }
            BaseData.Connection.Close();
            return result;

        }


        public List<weightModel> search(weightModel data)
        {
            string query = "SELECT  top 20 * from weight where  1=1 ";

            if (data.ownerUserId > 0 )
            {
                query += " and  owner_user_id = " + data.ownerUserId; 
            }

            if (data.driverUserId > 0)
            {
                query += " and  driver_user_id = " + data.driverUserId;
            }


            if (data.carId > 0)
            {
                query += " and  car_id = " + data.carId;
            }

            if (data.itemTypeId > 0)
            {
                query += " and  item_type = " + data.itemTypeId;
            }

            if (data.date != null  )
            {
                query += " and  date >= " + data.date  + " and date =<"+ data.time ;
            }

            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            List<weightModel> result = new List<weightModel>();
            SqlCeCommand command = new SqlCeCommand(query, BaseData.Connection);
            try
            {

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(map(reader));
                }
            }
            catch
            {
            }
            BaseData.Connection.Close();

            return result;

        }


    }

}
