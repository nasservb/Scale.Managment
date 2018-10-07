using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ScaleManagment.Model;
using System.Data.SqlServerCe;
using System.Globalization;

namespace ScaleManagment
{
    public partial class frmInput : Form
    {
        private int  currentId = 0 ;
        private bool editMod = false; 
        public frmInput()
        {
            InitializeComponent();
        }

        public frmInput(int id)
        {
            InitializeComponent();
           
            editMod = true;

            currentId = id; 
             
        }

        private void frmInput_Load(object sender, EventArgs e)
        {
            
            txtId.Text = ((new weightModel()).getMaxId() + 1).ToString();

             
            cmbOwner.DataSource =  (new userModel()).getUsers(UserType.Farmer);

             
            cmbDriver.DataSource =   (new userModel()).getUsers(UserType.Driver);

            cmbCar.DataSource = data.getDataList("cars");

            cmbItem.DataSource = data.getDataList("items");

            if (editMod == true)
            {
                weightModel data = (new weightModel()).getWeight(currentId);

                txtWeight.Text = data.weight.ToString();
                txtWeight2.Text = data.weight2.ToString();
                txtPositiveDecreas.Text = data.positiveDecreas.ToString();
                txtDescription.Text = data.description;
                txtNegativeDecreas.Text = data.negativeDecreas.ToString();
                txtSerial.Text = data.serial;
                txtPestPercent.Text = data.pestPercent.ToString();

                cmbDriver.SelectedItem = new userModel(data.driverUserId);

                cmbOwner.SelectedItem = new userModel(data.ownerUserId);

                cmbItem.SelectedItem = new data().getData(data.driverUserId,"items"); 

                cmbCar.SelectedItem= new data().getData(data.driverUserId, "cars");

                 
                txtId.Text = data.id.ToString() ;
            } 

        }

        private void frmInput_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaseData.frmMain.Show();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (cmbOwner.SelectedItem == null || cmbDriver.SelectedItem == null || txtWeight.Text == "0" )
            {
                MessageBox.Show("اطلاعات ورودی ناقض است . ","خطا");
                return;
            }

            weightModel newWeight = new weightModel();
            newWeight.ownerUserId = ((userModel)cmbOwner.SelectedValue).id;
            newWeight.driverUserId = ((userModel)cmbDriver.SelectedValue).id;
            newWeight.opperatorUserId = BaseData.CurrentUser;

            if (cmbCar.SelectedValue != null)
                newWeight.carId = ((data)cmbCar.SelectedValue).id;

            if (cmbItem.SelectedValue != null)
                newWeight.itemTypeId= ((data)cmbItem.SelectedValue).id;

            double data = 0;

            double.TryParse(txtWeight.Text, out data);
            newWeight.weight = data;


            double.TryParse(txtPestPercent.Text, out data);
            newWeight.pestPercent = data;


            double.TryParse(txtPositiveDecreas.Text, out data);
            newWeight.positiveDecreas = data;


            double.TryParse(txtNegativeDecreas.Text, out data);
            newWeight.negativeDecreas = data;

            newWeight.description = 
                "صاحب بار : " + ((userModel)cmbOwner.SelectedItem).ToString() + " , "+
                "راننده : " + ((userModel)cmbDriver.SelectedItem).ToString() + " , "+  
                "ماشین : " + ((data)cmbCar.SelectedItem).ToString() + " \n "+  
                txtDescription.Text;

            newWeight.serial = txtSerial.Text;

            if (editMod == true)
                newWeight.id = currentId; 

            newWeight.save(); 
              
            MessageBox.Show("اطلاعات با موفقیت ثبت شد . ","ثبت");

            txtWeight.Text = "0";
            txtPositiveDecreas.Text = "0";
            txtDescription.Text = "";
            txtNegativeDecreas.Text = "0";
            txtSerial.Text = "0";
            txtPestPercent.Text = "0";
             
            int lastId = newWeight.id;
            txtId.Text = (lastId + 1).ToString();
        }

        private void btnNewOwner_Click(object sender, EventArgs e)
        {
            frmPerson frm = new frmPerson();
            frm.ShowDialog(); 
        }

        private void btnNewDriver_Click(object sender, EventArgs e)
        {
            frmPerson frm = new frmPerson();
            frm.ShowDialog();
        }

        private void btnNewCar_Click(object sender, EventArgs e)
        {
            frmCar frm = new frmCar();
            frm.ShowDialog();
        }

        private void btnNewItem_Click(object sender, EventArgs e)
        {
            frmItem frm = new frmItem();
            frm.ShowDialog();
        }

        private void frmInput_Activated(object sender, EventArgs e)
        {
            cmbOwner.DataSource = (new userModel()).getUsers(UserType.Farmer);


            cmbDriver.DataSource = (new userModel()).getUsers(UserType.Driver);

            cmbCar.DataSource = data.getDataList("cars");

            cmbItem.DataSource = data.getDataList("items");
        }
    }
}
