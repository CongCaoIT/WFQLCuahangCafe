using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;

namespace QuanLyQuanCafe
{
    public partial class Admin : Form
    {
        BindingSource foodlist = new BindingSource();

        BindingSource categoryList = new BindingSource();

        BindingSource tableList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public AccountDTO loginAccount;

        public Admin()
        {
            InitializeComponent();
            Load();
        }

        #region method

        private new void Load()
        {
            dtgvFood.DataSource = foodlist;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillDateAndPage();

            LoadListFood();
            AddFoodBinding();

            LoadListCategory();
            LoadCategoryIntoCombobox(cbFoodCategory);
            AddCategoryBinding();

            LoadListTable();
            AddTableBinding();
            
            LoadListAccount();
            AddAccountBinding();
        }

        void LoadListBillDateAndPage()
        {
            dtgvBill.Font = new System.Drawing.Font("Times New Roman", 14.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dtgvBill.DataSource = BillDAO.Instance.GetListBillByDateAndPage(dtpkFromDate.Value, dtpkToDate.Value, Convert.ToInt32(txbBillPage.Text));
            dtgvBill.Columns["MAHOADON"].Visible = false;
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1); // cộng 1 tháng trừ 1 ngày
        }

        void LoadListFood()
        {
            dtgvFood.Font = new System.Drawing.Font("Times New Roman", 14.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            foodlist.DataSource = FoodDAO.Instance.GetListFood();

            dtgvFood.Columns[0].HeaderText = "Mã";
            dtgvFood.Columns[0].Width = 80;

            dtgvFood.Columns[1].HeaderText = "Tên món ăn";
            dtgvFood.Columns[1].Width = 240;

            dtgvFood.Columns[2].HeaderText = "Mã loại";
            dtgvFood.Columns[2].Width = 80;

            dtgvFood.Columns[3].HeaderText = "Giá";
            dtgvFood.Columns[3].Width = 120;
        }

        List<FoodDTO> SearchFoodByName(string name)
        {
            List<FoodDTO> listFood = FoodDAO.Instance.SearchFoodByName(name);

            if (listFood == null)
            {
                MessageBox.Show("Không tìm thấy món ăn, vui lòng nhập lại món khác!!!");
                return null;
            }

            return listFood;
        }

        void AddFoodBinding() // Binding thông tin thức ăn
        {
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadListCategory()
        {
            dtgvCategory.Font = new System.Drawing.Font("Times New Roman", 14.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();

            dtgvCategory.Columns[0].HeaderText = "Mã loại món ăn";
            dtgvCategory.Columns[1].HeaderText = "Tên loại món ăn";
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cbFoodCategory.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddCategoryBinding()
        {
            txbCategoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void LoadListTable()
        {
            dtgvTable.Font = new System.Drawing.Font("Times New Roman", 14.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            tableList.DataSource = TableDAO.Instance.LoadTableList();

            dtgvTable.Columns[0].HeaderText = "Mã bàn";
            dtgvTable.Columns[1].HeaderText = "Tên bàn";
            dtgvTable.Columns[2].HeaderText = "Trạng thái";
        }
      
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Status", true, DataSourceUpdateMode.Never));
        }

        void LoadListAccount()
        {
            dtgvAccount.Font = new System.Drawing.Font("Times New Roman", 14.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            accountList.DataSource = AccountDAO.Instance.GetListAccount();
            dtgvAccount.Columns["Password"].Visible = false;

            dtgvAccount.Columns[0].HeaderText = "Tên đăng nhập";
            dtgvAccount.Columns[0].Width = 150;

            dtgvAccount.Columns[1].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns[1].Width = 180;

            dtgvAccount.Columns[3].HeaderText = "Loại tài khoản";
            dtgvAccount.Columns[3].Width = 130;

        }

        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            //txbPassWord.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Password", true, DataSourceUpdateMode.Never));
            nmType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        #endregion Method

        #region Event  

        //Doanh thu
        #region EV_REVENUE 

        private void btnViewbill_Click(object sender, EventArgs e)
        {
            LoadListBillDateAndPage();
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbBillPage.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            int lastPage = sumRecord / 10;
            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }
            txbBillPage.Text = lastPage.ToString();
        }

        private void btnPreviousBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbBillPage.Text);

            if (page > 1)
                page--;

            txbBillPage.Text = page.ToString();
        }

        private void btnNextBillPage_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbBillPage.Text);
            int sumRecord = BillDAO.Instance.GetNumListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            if (page <= sumRecord / 10)
                page++;

            txbBillPage.Text = page.ToString();
        }

        private void txbBillPage_TextChanged(object sender, EventArgs e)
        {
            LoadListBillDateAndPage();
        }

        #endregion REVENUE

        #region EV_Food

        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0 && dtgvFood.SelectedCells[0].OwningRow.Cells["Categoryid"].Value != null)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["Categoryid"].Value;
                    CategoryDTO category = CategoryDAO.Instance.GetCategoryById(id);
                    cbFoodCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (CategoryDTO item in cbFoodCategory.Items)
                    {
                        if (item.Id == category.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as CategoryDTO).Id;
            double price = (double)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công!!!");
                LoadListFood();
                if (insertFood != null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi món ăn!!!");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            string name = txbFoodName.Text;
            int categoryID = (cbFoodCategory.SelectedItem as CategoryDTO).Id;
            double price = (double)nmFoodPrice.Value;

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công!!!");
                LoadListFood();
                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi sửa món ăn!!!");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);

            BillInfoDAO.Instance.DeleteBillInfoByFoodID(id); // Xóa món ăn trong chi tiết hóa đơn để tránh việc ràng buộc khóa ngoại

            if (FoodDAO.Instance.DeleteFood(id)) // Xóa món ăn
            {
                MessageBox.Show("Xóa món thành công!!!");
                LoadListFood();
                if (deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi xóa món ăn!!!");
            }
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodlist.DataSource = SearchFoodByName(txbSearchFoodName.Text);
        }

        #endregion EV_Food

        #region EV_Category

        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }

        private void tcAdmin_Click(object sender, EventArgs e)
        {
            InsertCategory += tc_InsertCategory;
            DeleteCategory += tc_DeleteCategory;
            UpdateCategory += tc_UpdateCategory;
        }

        private void tc_UpdateCategory(object sender, EventArgs e)
        {
            LoadCategoryIntoCombobox(cbFoodCategory);
        }

        private void tc_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategoryIntoCombobox(cbFoodCategory);
        }

        private void tc_InsertCategory(object sender, EventArgs e)
        {
            LoadCategoryIntoCombobox(cbFoodCategory);
        }

        private void btnShowCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm loại món ăn thành công!!!");
                LoadListCategory();
                if (insertCategory != null)
                {
                    insertCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm loại món ăn!!!");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);

            FoodDAO.Instance.DeleteFoodByCategoryId(id);

            if (CategoryDAO.Instance.DeleteCategory(id))
            {
                MessageBox.Show("Xóa loại món ăn thành công!!!");
                LoadListCategory();
                if (deleteCategory != null)
                {
                    deleteCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa loại món ăn!!!");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryID.Text);
            string name = txbCategoryName.Text;

            if (CategoryDAO.Instance.UpdateCategory(id, name))
            {
                MessageBox.Show("Sửa tên loại món ăn thành công!!!");
                LoadListCategory();
                if (updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa tên loại món ăn!!!");
            }
        }

        #endregion EV_Category

        #region EV_Table


        private void btnShowTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }

        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string status = txbTableStatus.Text;

            if (TableDAO.Instance.InsertTable(name, status))
            {
                MessageBox.Show("Thêm bàn thành công!!!");
                LoadListTable();
                if (insertTable != null)
                {
                    insertTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn!!!");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);

            BillInfoDAO.Instance.DeleteBillInfoTableId(id);
            BillDAO.Instance.DeleteBill(id);

            if (TableDAO.Instance.DeleteTable(id)) // Xóa món ăn
            {
                MessageBox.Show("Xóa bàn thành công!!!");
                LoadListTable();
                if (deleteTable != null)
                {
                    deleteTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa bàn!!!");
            }
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            string name = txbTableName.Text;
            string status = txbTableStatus.Text;

            if (TableDAO.Instance.UpdateTable(id, name, status))
            {
                MessageBox.Show("Sửa bàn thành công!!!");
                LoadListTable();
                if (updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi sửa bàn!!!");
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        #endregion

        #region EV_Account

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            LoadListAccount();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string name = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmType.Value;

            if (AccountDAO.Instance.InsertAccount(name, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công!!!");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm tài khoản!!!");
            }
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string name = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmType.Value;

            if (AccountDAO.Instance.UpdateAccount(name, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công!!!");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa tài khoản!!!");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string name = txbUserName.Text;

            if (loginAccount.UserName.Equals(name))
            {
                MessageBox.Show("Không được xóa tài khoản đang đăng nhập!!!");
                return;
            }

            if (AccountDAO.Instance.DeleteAccount(name))
            {
                MessageBox.Show("Xóa tài khoản thành công!!!");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản!!!");
            }
        }
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string name = txbUserName.Text;

            if (AccountDAO.Instance.ResetPassWord(name))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công!!!");
                LoadListAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi đặt lại mật khẩu!!!");
            }
        }

        #endregion EV_Account

        #endregion Event
    }
}
