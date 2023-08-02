using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class TableDTO
    {
        private int id;
        private string name;
        private string status;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public TableDTO()
        {

        }

        public TableDTO(int id, string name, string status)
        {
            this.id = id;
            this.name = name;
            this.status = status;
        }
        public TableDTO(DataRow row)
        {
            this.ID = (int)row["MABAN"];
            this.Name = row["TEN"].ToString();
            this.Status = row["TRANGTHAI"].ToString();
        }
    }
}
