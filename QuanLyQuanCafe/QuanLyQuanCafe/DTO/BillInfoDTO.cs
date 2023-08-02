using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfoDTO
    {
        private int id;
        private int billID;
        private int foodID;
        private int quantity; // Số lượng món ăn

        public int Id { get => id; set => id = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        public BillInfoDTO(int id, int billId, int foodId, int quanlity)
        {
            this.Id = id;
            this.BillID = billId;
            this.FoodID = foodId;
            this.Quantity = quanlity;
        }

        public BillInfoDTO(DataRow row)
        {
            this.Id = (int)row["MACHITETHD"];
            this.BillID = (int)row["MAHOADON"];
            this.FoodID = (int)row["MAMONAN"];
            this.Quantity = (int)row["COUNT"];
        }
    }
}
