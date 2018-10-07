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
    public partial class frmCar : Form
    {
        private int currentId = 0;
        private bool editMod = false; 

        public frmCar()
        {
            InitializeComponent();

            
        }


        public frmCar(int id )
        {
            InitializeComponent();

            currentId = id;
            editMod = true;
             
        }

        private void frmCar_Load(object sender, EventArgs e)
        {
            cmbOwner.DataSource = (new userModel()).getUsers(UserType.Driver);

            if (editMod == true)
            {
                data car = (new data()).getData(currentId, "cars");
                txtPelak.Text = car.name;
                txtDescription.Text = car.description;
                cmbOwner.SelectedItem = new userModel(car.ownerUserId);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (BaseData.Connection.State == System.Data.ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand(
                @"insert into cars (
                    pelak,description, owner_user_id)
                    values(" +
                    "'" + txtPelak.Text + "'," +
                    "'" + txtDescription.Text + "'," +
                    "'" + ((userModel)cmbOwner.SelectedItem).id + "')"
                    , BaseData.Connection);
            if (editMod == true)
            {
                command = new SqlCeCommand(
                @"update cars set 
                    pelak = '" + txtPelak.Text + @"',
                    description ='" + txtDescription.Text + @"' 
                    , owner_user_id = '" + ((userModel)cmbOwner.SelectedItem).id + @"' 
                    where id = " +currentId 
                    , BaseData.Connection);
            }
            try
            {
                command.ExecuteNonQuery();

            }
            catch
            {
            }
            MessageBox.Show("اطلاعات با موفقیت ثبت شد . ", "ثبت");

            BaseData.frmMain.Show();
            Close();

        }

        private void frmCar_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaseData.frmMain.Show();
        }
    }
}
