using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class FoodDTO
    {
        private int id;
        private string name;
        private int categoryid;
        private double price;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int Categoryid { get => categoryid; set => categoryid = value; }
        public double Price { get => price; set => price = value; }
        public FoodDTO(int id, string name, int categoryid, double price)
        {
            this.Id = id;
            this.Name = name;
            this.Categoryid = categoryid;
            this.price = price;
        }

        public FoodDTO(DataRow row)
        {
            this.Id = (int)row["MAMONAN"];
            this.Name = row["TEN"].ToString();
            this.Categoryid = (int)row["MALOAIMONAN"];
            this.Price = Convert.ToDouble(row["GIA"]);
        }
    }
}
