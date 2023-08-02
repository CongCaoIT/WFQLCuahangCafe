using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoodDAO();
                return FoodDAO.instance;
            }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<FoodDTO> GetFoodByCategoryID(int id)
        {
            List<FoodDTO> list = new List<FoodDTO>();

            string query = "select * from MONAN where MALOAIMONAN = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                list.Add(food);
            }

            return list;
        }
        public List<FoodDTO> GetListFood()
        {
            List<FoodDTO> list = new List<FoodDTO>();

            string query = "select * from MONAN";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                list.Add(food);
            }

            return list;
        }

        public bool InsertFood(string name, int catrgoryid, double price)
        {
            string query = string.Format("INSERT INTO MONAN(TEN, MALOAIMONAN, GIA) VALUES (N'{0}', {1}, {2})", name, catrgoryid, price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateFood(int id, string name, int catrgoryid, double price)
        {
            string query = string.Format("UPDATE MONAN SET TEN = N'{0}', MALOAIMONAN = {1}, GIA = {2} WHERE MAMONAN = {3}", name, catrgoryid, price, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int id)
        {
            string query = string.Format("DELETE MONAN WHERE MAMONAN = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public List<FoodDTO> SearchFoodByName(string name)
        {
            List<FoodDTO> list = new List<FoodDTO>();

            string query = string.Format("SELECT * FROM MONAN WHERE [dbo].[fuConvertToUnsign1](TEN) LIKE N'%' + [dbo].[fuConvertToUnsign1](N'{0}') + '%'", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                list.Add(food);
            }

            return list;
        }

        public void DeleteFoodByCategoryId(int id)
        {
            DataProvider.Instance.ExecuteQuery("DELETE MONAN WHERE MALOAIMONAN = " + id);
        }
    }
}
