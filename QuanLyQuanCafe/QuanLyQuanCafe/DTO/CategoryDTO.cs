using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class CategoryDTO
    {
        private int id;
        private string name;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public CategoryDTO(int id, string name)
        {
            this.id = Id;
            this.name = Name;
        }

        public CategoryDTO(DataRow row) //LOAIMONAN
        {
            this.Id = (int)row["MALOAIMONAN"];
            this.Name = row["TEN"].ToString();
        }
    }
}
