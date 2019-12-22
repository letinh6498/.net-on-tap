using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLSinhVien
{
    public partial class Form1 : Form
    {
        Customer model = new Customer();
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Clear();
            PopulateDataGridView();
        }

        void Clear()
        {
            txtFirstName.Text = txtLastName.Text = txtCity.Text = txtAddress.Text = "";
            btnCancel.Text = "Save";
            btnDelete.Enabled = false;
            model.CustomerID = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            model.FirstName = txtFirstName.Text.Trim();
            model.LastName = txtLastName.Text.Trim();
            model.City = txtCity.Text.Trim();
            model.Address = txtAddress.Text.Trim();
            using(EFDBEntities db = new EFDBEntities())
            {
                if (model.CustomerID == 0) // insert
                    db.Customers.Add(model);
                else // update
                    db.Entry(model).State = EntityState.Modified;             
                db.SaveChanges();
            }
            Clear();
            PopulateDataGridView();
            MessageBox.Show("submit successfully");
        }
        void PopulateDataGridView()
        {
            dgvCustomer.AutoGenerateColumns = false;
            using(EFDBEntities db = new EFDBEntities())
            {
                dgvCustomer.DataSource = db.Customers.ToList<Customer>();
            }
        }

        private void dgvCustomer_DoubleClick(object sender, EventArgs e)
        {
            if(dgvCustomer.CurrentRow.Index != -1)
            {
                model.CustomerID = Convert.ToInt32(dgvCustomer.CurrentRow.Cells["CustomerID"].Value);
                using(EFDBEntities db = new EFDBEntities())
                {
                    model = db.Customers.Where(x => x.CustomerID == model.CustomerID).FirstOrDefault();
                    txtFirstName.Text = model.FirstName.Trim();
                    txtLastName.Text = model.LastName.Trim();
                    txtCity.Text = model.City.Trim();
                    txtAddress.Text = model.Address.Trim();
                }
                btnSave.Text = "Update";
                btnDelete.Enabled = true;

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("are you sure to delete this record ?","EF Crud Operaton ", MessageBoxButtons.YesNo)== DialogResult.Yes)
            {
                using(EFDBEntities db = new EFDBEntities())
                {
                    var entry = db.Entry(model);
                    if(entry.State == EntityState.Detached)
                    {
                        db.Customers.Attach(model);
                    }
                    db.Customers.Remove(model);
                    db.SaveChanges();
                    PopulateDataGridView();
                    Clear();
                    MessageBox.Show("Delete successfully");
                }
            }
        }
    }
}
