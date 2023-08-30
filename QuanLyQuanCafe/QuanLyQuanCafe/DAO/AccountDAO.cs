using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using QuanLyQuanCafe.DTO;
using System.Security.Cryptography;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new AccountDAO();
                return instance;           
            }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public string HasPassWord(string passWord)
        {
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            return hasPass;
        }

        public bool Login(string userName, string passWord)
        {
            // string query = "SELECT * FROM TAIKHOAN WHERE TENDANGNHAP = '" + userName + "' and MATKHAU = '"+ passWord +"'"; // nối chuõi // NHƯNG LỖI BẢO MẬT | ' OR 1 = 1 |

            //var list = hasData.ToString();
            //list.Reverse();  // Đảo ngược lại

            string query = "USP_Login @userName , @passWord"; // phải để khoản trống để kiểm tra
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, HasPassWord(passWord) }); // trả về 1 bản dữ liệu
            
            return result.Rows.Count > 0; //đếm số dòng, nếu lớn hơn không trả về true
        }

        public AccountDTO GetAccountByUserName(string userName)
        {
           DataTable data = DataProvider.Instance.ExecuteQuery("select * from TAIKHOAN where TENDANGNHAP = '" + userName + "'");
            foreach(DataRow item in data.Rows)
            {
                return new AccountDTO(item);
            }
            return null;
        }

        public bool UpdateAccount(string userName, string displayName, string pass, string newpass)
        {
             int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @userName , @displayName , @password , @newPassword", new object[] { userName, displayName, HasPassWord(pass), HasPassWord(newpass)});
            return result > 0;
        }

        public List<AccountDTO> GetListAccount()
        {
            List<AccountDTO> list = new List<AccountDTO>();

            string query = "select * from TAIKHOAN";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                AccountDTO acc = new AccountDTO(item);
                list.Add(acc);
            }

            return list;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("INSERT INTO TAIKHOAN(TENDANGNHAP, TENHIENTHI, MATKHAU, LOAITAIKHOAN) VALUES (N'{0}', N'{1}', N'1962026656160185351301320480154111117132155', {2})", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateAccount(string userName, string displayName, int type)
        {
            string query = string.Format("UPDATE TAIKHOAN SET TENHIENTHI = N'{0}', LOAITAIKHOAN = {1} WHERE TENDANGNHAP = N'{2}'", displayName, type, userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)
        {
            string query = string.Format("DELETE TAIKHOAN WHERE TENDANGNHAP = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassWord(string name)
        {
            string query = string.Format("UPDATE TAIKHOAN SET MATKHAU = N'1962026656160185351301320480154111117132155' WHERE TENDANGNHAP = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
