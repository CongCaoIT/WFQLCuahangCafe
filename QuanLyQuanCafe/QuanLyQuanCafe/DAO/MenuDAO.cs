using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new MenuDAO();
                return instance;
            }
            private set { instance = value; }
        }
        public MenuDAO() { }

        public List<MenuDTO> GetListMenuByTable(int id)
        {
            List<MenuDTO> listMenu = new List<MenuDTO>();

            DataTable data = DataProvider.Instance.ExecuteQuery("select m.TEN, cthd.COUNT, m.GIA, cthd.COUNT * m.GIA as totalPrice from MONAN m join CHITIETHD cthd on m.MAMONAN = cthd.MAMONAN join HOADON hd on cthd.MAHOADON = hd.MAHOADON where hd.TRANGTHAI = 0 and hd.MABAN = " + id);

            foreach(DataRow item in data.Rows)
            {
                MenuDTO menu = new MenuDTO(item);
                listMenu.Add(menu);
            }
            return listMenu;
        }
    }
}
