using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new CategoryDAO();
                return CategoryDAO.instance;
            }
            private set { CategoryDAO.instance = value; }
        }

        private CategoryDAO() { }

        public List<CategoryDTO> GetListCategory() // Lấy danh sách loại món ăn
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            string query = "select * from LOAIMONAN";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach(DataRow item in data.Rows)
            {
                CategoryDTO category = new CategoryDTO(item);
                list.Add(category);
            }    
            return list;
        }
        public CategoryDTO GetCategoryById(int id)
        {
            CategoryDTO category = null;

            string query = "select * from LOAIMONAN where MALOAIMONAN = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                category = new CategoryDTO(item);
                return category;
            }    

            return category;
        }

        public bool InsertCategory(string name)
        {
            string query = string.Format("INSERT INTO LOAIMONAN VALUES (N'{0}')", name);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteCategory(int id)
        {
            string query = string.Format("DELETE LOAIMONAN WHERE MALOAIMONAN = " + id);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(int id, string name)
        {
            string query = string.Format("UPDATE LOAIMONAN SET TEN = N'{0}' WHERE MALOAIMONAN = {1}", name, id);

            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
