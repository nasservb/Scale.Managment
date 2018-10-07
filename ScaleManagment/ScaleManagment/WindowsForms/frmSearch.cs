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

namespace ScaleManagment
{
    public partial class frmSearch : Form
    {
        public frmSearch()
        {
            InitializeComponent();
        }

        private void cmbOwner_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        public void searchCar(object sender, EventArgs e)
        {
            data searchData = new Model.data();

            if (txtCarId.Text != "" )
            {
                int id;
                int.TryParse(txtCarId.Text, out id); 
                searchData.id = id; 
            }

            searchData.name = txtPelak.Text;

            searchData.description = textCarDesc.Text;

            dataGridView3.DataSource = (new Model.data()).search(searchData,"cars"); 
        }
         
         

        private void userSearch(object sender, EventArgs e)
        {
            userModel searchData = new userModel();

            if (txtUserCode.Text != "")
            {
                int id;
                int.TryParse(txtUserCode.Text, out id);
                searchData.id = id;
            }

            searchData.name = txtName.Text;

            searchData.family = txtFamily.Text;

            searchData.melli = txtMelli.Text;

            searchData.location = txtLocation.Text;

            if (cmbUserType.SelectedItem != null)
                searchData.userType =((UserType) ((data)cmbUserType.SelectedItem).id);

            dataGridView2.DataSource = (new Model.userModel()).search(searchData);
        }



        private void weightSearch(object sender, EventArgs e)
        {
            weightModel searchData = new weightModel();

            if (txtCode.Text != "")
            {
                int id;
                int.TryParse(txtCode.Text, out id);
                searchData.id = id;
            } 

            if (cmbOwner.SelectedItem != null)
                searchData.ownerUserId = ((userModel)cmbOwner.SelectedItem).id;


            if (cmbDriver.SelectedItem != null)
                searchData.driverUserId = ((userModel)cmbDriver.SelectedItem).id;


            if (cmbItem.SelectedItem != null)
                searchData.itemTypeId = ((data)cmbItem.SelectedItem).id;

            if(txtStartDate.Text != "" && txtFinishDate.Text != "")
            {
                searchData.date = txtStartDate.Text;
                searchData.time = txtFinishDate.Text; 
            }

            dataGridView1.DataSource = (new Model.weightModel()).search(searchData);
        }

        private void frmSearch_FormClosed(object sender, FormClosedEventArgs e)
        {
            BaseData.frmMain.Show(); 
        }

        private void frmSearch_Load(object sender, EventArgs e)
        {
            
            List<userModel> owners =  (new userModel()).getUsers(UserType.Farmer);

            userModel temp = new userModel();

            temp.id = 0;
            temp.name = "انتخاب نشده";

            owners.Insert(0, temp);
            cmbOwner.DataSource = owners;

            List<userModel> drivers = (new userModel()).getUsers(UserType.Driver);

            drivers.Insert(0, temp);

            cmbDriver.DataSource = drivers;

            List<data> userTypes = new List<data>();

            
            userTypes.Add(new data(4,"انتخاب نشده"));
            userTypes.Add(new data(1,"اپراتور"));
            userTypes.Add(new data(2,"کشاورز"));
            userTypes.Add(new data(3,"راننده"));


            List<data> items = data.getDataList("items");

            items.Insert(0, new data(0, "انتخاب نشده"));

            cmbUserType.DataSource = userTypes;

            cmbItem.DataSource = items;
        }

        private void DataGridView1_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            var cell = ((DataGridView)sender).SelectedCells[0];
            if (cell.Value.ToString() == "ویرایش")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[17].Value);
                frmInput frm = new frmInput(id);
                frm.ShowDialog();

            }
            else if (cell.Value.ToString() == "حذف")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[17].Value);
                var result = MessageBox.Show("آیا اطمینان دارید ؟ ", "حذف", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    BaseData.removeItem((int)id, "weight");

            }

        }

        private void DataGridView2_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            var cell = ((DataGridView)sender).SelectedCells[0];
            if (cell.Value.ToString() == "ویرایش")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                frmPerson frm = new frmPerson(id);
                frm.ShowDialog();
            }
            else if (cell.Value.ToString() == "حذف")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                var result = MessageBox.Show("آیا اطمینان دارید ؟ ", "حذف", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    BaseData.removeItem((int)id, "users");
            }
        }

        private void DataGridView3_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            var cell = ((DataGridView)sender).SelectedCells[0];
            if (cell.Value.ToString() == "ویرایش")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                frmCar frm = new frmCar((int)id);
                frm.ShowDialog();

            }
            else if (cell.Value.ToString() == "حذف")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                var result = MessageBox.Show("آیا اطمینان دارید ؟ ", "حذف", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    BaseData.removeItem((int)id, "cars");
            }
        }

    }
}
