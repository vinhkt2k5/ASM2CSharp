using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM2
{
    public partial class WaterBill : Form
    {
        public WaterBill()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.Show();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void cobCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cobCustomerType.Text.Trim().ToLower();
            if (type.Equals("household customer"))
            {
                txtMember.Enabled = true;
            }
            else
            {
                txtMember.Enabled = false;
                txtMember.Text = null;
            }

        }

        private void WaterBill_Load(object sender, EventArgs e)
        {
            lstVBill.View = View.Details;
            lstVBill.GridLines = true;
            lstVBill.FullRowSelect = true;
            // create column
            lstVBill.Columns.Add("Full name", 150);
            lstVBill.Columns.Add("Type", 100);
            lstVBill.Columns.Add("Member", 50);
            lstVBill.Columns.Add("Last Water number", 100);
            lstVBill.Columns.Add("Current Water number", 100);
            lstVBill.Columns.Add("Water number");
            lstVBill.Columns.Add("Money");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string customer = txtFullName.Text.Trim();
            if (string.IsNullOrEmpty(customer))
            {
                MessageBox.Show("Please enter Fullname", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string customerType = cobCustomerType.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(customerType))
            {
                MessageBox.Show("Please enter Customer's Type", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int countMember = 0;
            if (customerType.Equals("household customer"))
            {
                // kiem tra xem nguoi dung nhap so luong thanh vien chua?
                string Member = txtMember.Text.Trim();
                if (!int.TryParse(Member, out _))
                {
                    MessageBox.Show("Enter family member, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                countMember = Convert.ToInt32(Member);
                if (countMember <= 0)
                {
                    MessageBox.Show("Enter family's member can not less than 0, Please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


            }
            string lastMonth = txtLastMonthWater.Text.Trim();
            if (!int.TryParse(lastMonth, out _))
            {
                MessageBox.Show("Last month's water number is number, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string currentMonth = txtCurrentMonth.Text.Trim();
            if (!int.TryParse(currentMonth, out _))
            {
                MessageBox.Show("Curren month's month's water number is number, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
            int lastMonthWater = Convert.ToInt32(lastMonth);
            int currenMonthWater = Convert.ToInt32(currentMonth);
            if (currenMonthWater < lastMonthWater)
            {
                MessageBox.Show("Current month's water can noy less than last month's water", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int waterNumber = currenMonthWater - lastMonthWater;
            double waterMoneyPayment = 0;
            if (customerType.Equals("household customer"))
            {
                double peopleWater = waterNumber / countMember;
                if (peopleWater >= 0 && peopleWater < 10)
                {
                    waterMoneyPayment = waterNumber * 5.973 * 1.1;
                }
                else if (peopleWater >= 10 && peopleWater < 20)
                {
                    waterMoneyPayment = waterNumber * 7.051 * 1.1;
                }
                else if (peopleWater >= 20 && peopleWater < 30)
                {
                    waterMoneyPayment = waterNumber * 8.699 * 1.1;
                }
                else
                {
                    waterMoneyPayment = waterNumber * 15.929 * 1.1;
                }
            }
            else if (customerType.Equals("public services"))
            {
                waterMoneyPayment = waterNumber * 9.955 * 1.1;

            }
            else if (customerType.Equals("production units"))
            {
                waterMoneyPayment = waterNumber * 11.615 * 1.1;
            }
            else if (customerType.Equals("business services"))
            {
                waterMoneyPayment = waterNumber * 22.068 * 1.1;
            }
            // tien hanh them du lieu vao list
            ListViewItem item = new ListViewItem();
            // them du lieu vao cot dau tien
            item.Text = customer; // ho ten khach hang
            // them du lieu vao cac cot con lai
            item.SubItems.Add(customerType);
            item.SubItems.Add(countMember.ToString());
            item.SubItems.Add(lastMonthWater.ToString());
            item.SubItems.Add(currenMonthWater.ToString());
            item.SubItems.Add(waterNumber.ToString());
            item.SubItems.Add(waterMoneyPayment.ToString());
            lstVBill.Items.Add(item);

            // xoa du lieu vua nhap
            txtFullName.Text = null;
            cobCustomerType.Text = null;
            txtMember.Text = null;
            txtLastMonthWater.Text = null;
            txtCurrentMonth.Text = null;






        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstVBill.SelectedItems.Count > 0)
            {
                DialogResult dl = MessageBox.Show("Do you want to delete it?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dl == DialogResult.Yes)
                {
                    lstVBill.Items.Remove(lstVBill.SelectedItems[0]);
                }
            }
            else
            {
                MessageBox.Show("Please select data to delete", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            txtFullName.Text = null;
            cobCustomerType.Text = null;
            txtMember.Text = null;
            txtLastMonthWater.Text = null;
            txtCurrentMonth.Text = null;
            btnEdit.Enabled = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstVBill.SelectedItems.Count > 0)
            {
                string customer = txtFullName.Text.Trim();
                if (string.IsNullOrEmpty(customer))
                {
                    MessageBox.Show("Enter fullname, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string customerType = cobCustomerType.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(customerType))
                {
                    MessageBox.Show("Enter Customer's type, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int countMember = 0;
                if (customerType.Equals("household customer"))
                {
                    // kiem tra xem nguoi dung nhap so thanh vien chua?
                    string member = txtMember.Text.Trim();
                    // bat buoc nguoi dung phai nhap so, khong duoc nhap chu va phai nhap so lon hon 0
                    if (!int.TryParse(member, out _))
                    {
                        MessageBox.Show("Enter familly's member, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    countMember = Convert.ToInt32(member);
                    if (countMember <= 0)
                    {
                        MessageBox.Show("Enter familly's member, can't less than 0, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                string lastMonth = txtLastMonthWater.Text.Trim();
                if (!int.TryParse(lastMonth, out _))
                {
                    MessageBox.Show("Last month's water number is number, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string currentMonth = txtCurrentMonth.Text.Trim();
                if (!int.TryParse(currentMonth, out _))
                {
                    MessageBox.Show("Current month's month's water number is number, please", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int lastMonthWater = Convert.ToInt32(lastMonth);
                int currentMonthWater = Convert.ToInt32(currentMonth);
                if (currentMonthWater < lastMonthWater)
                {
                    MessageBox.Show("Current month's water can't less than last month's water", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int waterNumber = currentMonthWater - lastMonthWater;
                double waterMoneyPayment = 0;
                if (customerType.Equals("household customer"))
                {
                    double peopleWater = waterNumber / countMember;
                    if (peopleWater >= 0 && peopleWater < 10)
                    {
                        waterMoneyPayment = waterNumber * 5.973 * 1.1;
                    }
                    else if (peopleWater >= 10 && peopleWater < 20)
                    {
                        waterMoneyPayment = waterNumber * 7.052 * 1.1;
                    }
                    else if (peopleWater >= 20 && peopleWater < 30)
                    {
                        waterMoneyPayment = waterNumber * 8.699 * 1.1;
                    }
                    else if (peopleWater >= 30)
                    {
                        waterMoneyPayment = waterNumber * 15.929 * 1.1;
                    }
                }
                else if (customerType.Equals("public services"))
                {
                    waterMoneyPayment = waterNumber * 9.955 * 1.1;
                }
                else if (customerType.Equals("production units"))
                {
                    waterMoneyPayment = waterNumber * 11.615 * 1.1;

                }
                else if (customerType.Equals("business services"))
                {
                    waterMoneyPayment = waterNumber * 22.068 * 1.1;
                }
                // them du lieu vao list view
                ListViewItem items = new ListViewItem();
                // them du lieu vao cot dau tien
                items.Text = customer; // ho ten khach hang
                                       // them du lieu vao cac cot con lai
                items.SubItems.Add(customerType);
                items.SubItems.Add(countMember.ToString());
                items.SubItems.Add(lastMonth.ToString());
                items.SubItems.Add(currentMonth.ToString());
                items.SubItems.Add(waterNumber.ToString());
                items.SubItems.Add(waterMoneyPayment.ToString());
                lstVBill.Items.Add(items);

                // xoa bo dong vua sua
                lstVBill.Items.Remove(lstVBill.SelectedItems[0]);
                // khoa button edit
                btnEdit.Enabled = false;
                // xoa bo du lieu o form
                txtFullName.Text = null;
                cobCustomerType.Text = null;
                txtMember.Text = null;
                txtLastMonthWater.Text = null;
                txtCurrentMonth.Text = null;

            }




        }

        private void lstVBill_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVBill.SelectedItems.Count > 0)
            {
                // nguoi dung chon mot du lieu de chinh sua
                var item = lstVBill.SelectedItems[0];
                string customerName = item.SubItems[0].Text.Trim();
                string customerType = item.SubItems[1].Text.Trim();
                string countMember = item.SubItems[2].Text.Trim();
                string lastMonthWater = item.SubItems[3].Text.Trim();
                string currentMonthWater = item.SubItems[4].Text.Trim();
               
                // do du lieu len form
                txtFullName.Text = customerName;
                cobCustomerType.Text = customerType;
                txtMember.Text = countMember;
                txtLastMonthWater.Text = lastMonthWater;
                txtCurrentMonth.Text = currentMonthWater;
                // mo button edit
                btnEdit.Enabled = true;


            }
        }
    }
}
