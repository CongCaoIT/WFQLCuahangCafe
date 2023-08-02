using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillDTO
    {
        private int id;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;
        private int tableId;
        private int status;

        public int Id { get => id; set => id = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int TableId { get => tableId; set => tableId = value; }

        public BillDTO(int id, DateTime? dataCheckIn, DateTime? dataCheckOut,int tableId, int status)
        {
            this.Id = id;
            this.DateCheckIn = dataCheckIn;
            this.DateCheckOut = dataCheckOut;
            this.TableId = tableId;
            this.Status = status;
        }

        public BillDTO(DataRow row)
        {
            this.Id = (int)row["MAHOADON"];
            this.DateCheckIn = (DateTime?)row["NGAYVAO"];

            var dataCheckOutTemp = row["NGAYVAO"];
            if(dataCheckOutTemp.ToString() != "")
                this.DateCheckOut = (DateTime?)dataCheckOutTemp;

            this.TableId = (int)row["MABAN"];
            this.Status = (int)row["TRANGTHAI"];
        }
    }
}
