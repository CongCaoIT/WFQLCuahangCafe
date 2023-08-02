using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class TableManager : Form
    {
        private AccountDTO loginAccount;

        public AccountDTO LoginAccount
        {
            get { return loginAccount; }
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount.Type);
            }
        }

        public TableManager(AccountDTO acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();
            LoadCategory();
            LoadComboxTable();
        }

        #region Method

        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            thôngTinTàiKhoảnToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        void LoadCategory() // Load tên ds món ăn lên cbCategory
        {
            List<CategoryDTO> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategogy.DataSource = listCategory;
            cbCategogy.DisplayMember = "Name"; // hiển thị trường cần lấy
        }

        void LoadFoodListByCategoryID(int id)
        {
            List<FoodDTO> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";
        }

        void LoadComboxTable() //Load ds tên bàn lên cbSwitchTable
        {
            List<TableDTO> listTable = TableDAO.Instance.LoadTableList();
            cbSwitchTable.DataSource = listTable;
            cbSwitchTable.DisplayMember = "Name";
        }

        void LoadTable()
        {
            flpTable.Controls.Clear(); // Xóa hết bàn
            List<TableDTO> tableList = TableDAO.Instance.LoadTableList();

            foreach (TableDTO item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item; //Lưu table

                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Aqua;
                        break;
                    default:
                        btn.BackColor = Color.Pink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();

            List<MenuDTO> listMenu = MenuDAO.Instance.GetListMenuByTable(id);
            double totalPrice = 0;

            foreach (MenuDTO item in listMenu)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Quantity.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }
            CultureInfo culture = new CultureInfo("vi-VN"); //Format setting thành tiền "Ngôn ngữ hiện thị"

            /*Thread.CurrentThread.CurrentCulture = culture;*/ // thay đổi luồng chạy

            txbTotalPrice.Text = totalPrice.ToString("c", culture); // "c" văn hóa
        }

        #endregion

        #region Events
        void btn_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as TableDTO).ID;
            lsvBill.Tag = (sender as Button).Tag; // lưu table vào tag
            string tableName = ((sender as Button).Tag as TableDTO).Name;
            lbBill.Text = "Hóa đơn (" + tableName + ")"; // hiển thị tên bàn trên hóa đơn
            ShowBill(tableID);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AccountProfile f = new AccountProfile(LoginAccount);
            f.UpdateAccountEV += f_UpdateAccountEV;
            f.ShowDialog();
        }

        private void f_UpdateAccountEV(object sender, AccountEvent e)
        {
            thôngTinTàiKhoảnToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Admin f = new Admin();

            f.loginAccount = loginAccount;

            f.InsertFood += f_InsertFood; // xử lý event thêm xóa sửa món
            f.UpdateFood += f_UpdateFood;
            f.DeleteFood += f_DeleteFood;

            f.InsertCategory += f_InsertCategory;
            f.DeleteCategory += f_DeleteCategory;
            f.UpdateCategory += f_UpdateCategory;

            f.InsertTable += f_InsertTable;
            f.DeleteTable += f_DeleteTable;
            f.UpdateTable += f_UpdateTable;

            f.ShowDialog();
        }

        private void f_UpdateTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void f_DeleteTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void f_InsertTable(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void f_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategogy.SelectedItem as CategoryDTO).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as TableDTO).ID);
            LoadTable();
        }

        private void cbCategogy_SelectedIndexChanged(object sender, EventArgs e) // load danh sách món ăn dưa vào MAMONAN
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            CategoryDTO selected = cb.SelectedItem as CategoryDTO;
            id = selected.Id;

            LoadFoodListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;

            if (table == null)
            {
                MessageBox.Show("Hãy chọn bàn cần thêm món!!!");
                return;
            }

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int foodID = (cbFood.SelectedItem as FoodDTO).Id;
            int count = (int)nmFoodCount.Value;

            if (idBill == -1) // không có bill nào cả
            {
                BillDAO.Instance.InsertBill(table.ID);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), foodID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID); //load dữ liệu lại
            LoadTable(); //load lại table
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            TableDTO table = lsvBill.Tag as TableDTO;

            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);
            int disCount = (int)nmDisCount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0].Replace(".", "")); // '100.000,00đ'cắt chuỗi lấy phần chữ số đầu tiên
            double finallPrice = totalPrice - (totalPrice / 100) * disCount;

            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn có chắc thanh toán hóa đơn cho {0}\nTổng tiền hóa đơn sau khi giảm {1}% là: {2}", table.Name, disCount, finallPrice), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, disCount, finallPrice);
                    ShowBill(table.ID);

                    LoadTable();
                }
            }
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
            int id1 = (lsvBill.Tag as TableDTO).ID;
            int id2 = (cbSwitchTable.SelectedItem as TableDTO).ID;
            if (MessageBox.Show(string.Format("Bạn có thật sự muốn chuyển {0} sang {1}", (lsvBill.Tag as TableDTO).Name, (cbSwitchTable.SelectedItem as TableDTO).Name), "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }
        }

        private void thêmMónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddFood_Click(this, new EventArgs());
        }

        private void thanhToánToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCheckOut_Click(this, new EventArgs());
        }
        #endregion
    }
}
