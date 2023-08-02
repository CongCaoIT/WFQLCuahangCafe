using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class MenuDTO
    {
        private string foodName;
        private int quantity;
        private double price;
        private double totalPrice;

        public string FoodName { get => foodName; set => foodName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public double Price { get => price; set => price = value; }
        public double TotalPrice { get => totalPrice; set => totalPrice = value; }

        public MenuDTO(string foodName, int quanlity, double price, double totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Quantity = quanlity;
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public MenuDTO(DataRow row)
        {
            this.FoodName = row["TEN"].ToString();
            this.Quantity = (int)row["COUNT"];
            this.Price = (double)Convert.ToDouble(row["GIA"].ToString());
            this.TotalPrice = (double)Convert.ToDouble(row["totalPrice"].ToString());
        }
    }
}
