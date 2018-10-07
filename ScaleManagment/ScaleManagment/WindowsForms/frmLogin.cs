using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.IO;
using ScaleManagment.Model;

namespace ScaleManagment
{
    

    public partial class frmLogin : Form
    {
     
        
        public frmLogin()
        {
            InitializeComponent();

            try
            {
                BaseData.DBPath = Path.GetDirectoryName(Application.ExecutablePath) + "\\db.sdf";

            if (!File.Exists(BaseData.DBPath))
               BaseData.createDB();

            BaseData.ConnectionString = "Data Source=" + BaseData.DBPath + ";Password=1396";
            }
            catch
            {
                MessageBox.Show("در ایجاد یا ارتباط با ابنک اطلاعاتی . مسیر برنامه قابل نوشتن نیست . ", "خطا");
                Application.Exit(); 

            }
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                BaseData.Connection = new SqlCeConnection(BaseData.ConnectionString);
                if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                    BaseData.Connection.Open();

                SqlCeCommand command = new SqlCeCommand("select id from users where users.username='" +
                    txtUserName.Text + "' and users.password='" + txtPassword.Text + "'", BaseData.Connection);
                var result = command.ExecuteReader();



                if (!result.Read())
                {
                    MessageBox.Show("نام کاربری یا رمز عبور اشتباه است ", "خطا");
                    return;
                }
                else
                {
                    BaseData.CurrentUser = (int)result[0];
                    BaseData.currentUserData = new userModel(BaseData.CurrentUser);
                    BaseData.CurrentUserType = BaseData.currentUserData.userType;

                    BaseData.frmMain = new frmMain();
                    BaseData.frmMain.Show();
                    this.Hide();

                }
            }
            catch
            {
                MessageBox.Show("خطا در ورود به سیستم ","خطا"); 
                
            }
        }


       
        private void frmLogin_Load(object sender, EventArgs e)
        {
             
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnLogin_Click(sender , e);
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
