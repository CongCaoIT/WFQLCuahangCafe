using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        public static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new BillInfoDAO();
                return instance;
            }
            private set { instance = value; }
        }

        private BillInfoDAO() { }

        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            List<BillInfoDTO> listBillInfo = new List<BillInfoDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select * from CHITIETHD where MAHOADON = " + id);

            foreach(DataRow item in data.Rows)
            {
                BillInfoDTO info = new BillInfoDTO(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count) // thêm chi tiết hóa đơn
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }

        public void DeleteBillInfoByFoodID(int id)
        {
            DataProvider.Instance.ExecuteQuery("DELETE CHITIETHD WHERE MAMONAN = " + id);
        }

        public void DeleteBillInfoTableId(int id)
        {
            string query = string.Format("DELETE CHITIETHD WHERE MAHOADON IN(SELECT MAHOADON FROM HOADON WHERE MABAN = {0})", id);
            DataProvider.Instance.ExecuteQuery(query);
        }
    }
}
