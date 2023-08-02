using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class AccountProfile : Form
    {
        private AccountDTO loginAccount;

        public AccountDTO LoginAccount
        {
            get { return loginAccount; }
            set
            {
                loginAccount = value;
                ChangeAccount(loginAccount);
            }
        }
        public AccountProfile(AccountDTO acc)
        {
            InitializeComponent();
            this.LoginAccount = acc;
        }

        void ChangeAccount(AccountDTO acc)
        {
            txbUserName.Text = LoginAccount.UserName;
            txbDisPlayName.Text = LoginAccount.DisplayName;
        }

        private event EventHandler<AccountEvent> updateAccountEV;
        public event EventHandler<AccountEvent> UpdateAccountEV
        {
            add { updateAccountEV += value; }
            remove { updateAccountEV -= value; }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void UpdateAccount()
        {
            string userName = txbUserName.Text;
            string displayName = txbDisPlayName.Text;
            string password = txbPassWord.Text;
            string newpassword = txbNewPass.Text;
            string reenterPass = txbReEnterPass.Text;

            if (!newpassword.Equals(reenterPass))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mật khẩu mới!");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(userName, displayName, password, newpassword))
                {
                    MessageBox.Show("Cập nhật thành công!!!");
                    if (updateAccountEV != null)
                        updateAccountEV(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));
                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu!!!");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccount();
        }
    }

    public class AccountEvent : EventArgs
    {
        private AccountDTO acc;
        public AccountDTO Acc { get => acc; set => acc = value; }
        public AccountEvent(AccountDTO acc)
        {
            this.Acc = acc;
        }
    }
}
