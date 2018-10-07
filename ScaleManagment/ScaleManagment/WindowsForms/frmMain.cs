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
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }
        

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            frmInput frm = new frmInput();
            frm.Show();
            this.Hide();
        }

        private void btnCar_Click(object sender, EventArgs e)
        {
            frmCar frm = new frmCar();
            frm.Show();
            this.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            frmSearch frm = new frmSearch();
            frm.Show();
            this.Hide();
        }

        private void btnItem_Click(object sender, EventArgs e)
        {
            frmItem frm = new frmItem();
            frm.Show();
            this.Hide();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FrmReport frm = new FrmReport();
            frm.Show();
            this.Hide();
        }

        private void btnPerson_Click(object sender, EventArgs e)
        {
            frmPerson frm = new frmPerson();
            frm.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            dataGridView1.DataSource = (new weightModel()).getWeights();

            dataGridView2.DataSource = (new userModel()).getUsers(null);

            dataGridView3.DataSource = data.getDataList("cars");

            dataGridView4.DataSource = data.getDataList("items");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void frmMain_Shown(object sender, EventArgs e)
        {
            
        }

        private void DataGridView4_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            var cell = ((DataGridView)sender).SelectedCells[0];
            if (cell.Value.ToString() == "ویرایش")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                frmItem frm = new frmItem((int)id);
                frm.ShowDialog();

            }
            else if (cell.Value.ToString() == "حذف")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[2].Value);
                var result = MessageBox.Show("آیا اطمینان دارید ؟ ", "حذف", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                    BaseData.removeItem((int)id, "items");
            }
        }

        private void DataGridView1_CellContentClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            var cell = ((DataGridView)sender).SelectedCells[0];
            if (cell.Value.ToString() == "ویرایش")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[7].Value);
                frmInput frm = new frmInput(id);
                frm.ShowDialog(); 
                 
            }
            else if (cell.Value.ToString() == "حذف")
            {
                var id = Convert.ToInt16(((DataGridView)sender).CurrentRow.Cells[7].Value);
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

        private void frmMain_Activated(object sender, EventArgs e)
        {
            dataGridView1.DataSource = (new weightModel()).getWeights();

            dataGridView2.DataSource = (new userModel()).getUsers(null);

            dataGridView3.DataSource = data.getDataList("cars");

            dataGridView4.DataSource = data.getDataList("items");
        }
    }
}
