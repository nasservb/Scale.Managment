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
    public partial class frmItem : Form
    {
        private int currentId = 0;

        private bool editMod = false; 

        public frmItem()
        {
            InitializeComponent();
        }


        public frmItem(int id )
        {
            InitializeComponent();

            currentId = id;
            editMod = true;

            data item = (new data()).getData(id, "items");
            txtDescription.Text = item.description;
            txtTitle.Text = item.name; 

        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (BaseData.Connection.State == ConnectionState.Closed)
                BaseData.Connection.Open();

            SqlCeCommand command = new SqlCeCommand(
                @"insert into items (name,description)
                    values(" +
                    "'" + txtTitle.Text + "'," +
                    "'" + txtDescription.Text + "')"
                    , BaseData.Connection);

            if (editMod == true)
            {
                command = new SqlCeCommand(
                @"update items set 
                    name = '" + txtTitle.Text + @"',
                    description ='" + txtDescription.Text + @"'  
                    where id = " + currentId
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

        private void frmItem_FormClosing(object sender, FormClosingEventArgs e)
        {
            BaseData.frmMain.Show(); 

        }
    }
}
