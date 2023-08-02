using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class AccountDTO
    {
        private string userName;
        private string displayName;
        private string password;
        private int type;

        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }

        public AccountDTO(string userName, string displayName, int type, string password = null)
        {
            this.UserName = userName;
            this.DisplayName = displayName;
            this.Type = type;
            this.Password = password;
        }
        public AccountDTO(DataRow row)
        {
            this.UserName = row["TENDANGNHAP"].ToString();
            this.DisplayName = row["TENHIENTHI"].ToString();
            this.Type = (int)row["LOAITAIKHOAN"];
            this.Password = row["MATKHAU"].ToString();
        }
    }
}
