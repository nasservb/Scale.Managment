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
using ScaleManagment.Model;

namespace ScaleManagment
{
    public partial class frmPerson : Form
    {
        private int  currentId = 0 ;
        private bool  editMod= false ;
        public frmPerson()
        {
            InitializeComponent();
        }
        public frmPerson(int id )
        {
            InitializeComponent();

            userModel data = new userModel(id);

            currentId = id; 

            editMod = true;

            txtName.Text = data.name;  
            txtFamily.Text = data.family;  
            txtMelli.Text = data.melli;  
            txtLocation.Text = data.location;  
            txtDescription.Text = data.description;  
            txtPassword.Text = data.password;  
            txtUserName.Text = data.userName;
            txtAddress.Text = data.address;

            cmbUserType.SelectedItem = data.userType;  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            


        }

        private void frmPerson_Load(object sender, EventArgs e)
        {

            if (BaseData.CurrentUserType == UserType.Administrator)
            {
                cmbUserType.Items.Add("مدیر");
            }

        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUserType.SelectedItem.ToString() == "اپراتور" || cmbUserType.SelectedItem.ToString() == "مدیر")
            {
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
            }
            else
            {
                txtUserName.Enabled = false;
                txtPassword.Enabled = false;
            }
        }

        private void frmPerson_FormClosed(object sender, FormClosedEventArgs e)
        {
            BaseData.frmMain.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            userModel newUser = new userModel();
            newUser.name = txtName.Text;
            newUser.family = txtFamily.Text;
            newUser.address = txtAddress.Text;
            newUser.userName = txtUserName.Text;
            newUser.melli = txtMelli.Text;
            newUser.location = txtLocation.Text;
            newUser.password = txtPassword.Text;

            if (cmbUserType.SelectedItem == "راننده")
            {
                newUser.userType = UserType.Driver;
            }
            else if (cmbUserType.SelectedItem == "کشاورز")
            {
                newUser.userType = UserType.Farmer;
            }
            else if (cmbUserType.SelectedItem == "اپراتور")
            {
                newUser.userType = UserType.Farmer;
            }
            else if (cmbUserType.SelectedItem == "مدیر")
            {
                newUser.userType = UserType.Administrator;
            }
            if (editMod == true)
                newUser.id = currentId; 

            newUser.save();


            MessageBox.Show("اطلاعات فرد ثبت شد ", "کد : " + newUser.id.ToString());

            BaseData.frmMain.Show();

            Close();
        }
    }
}
