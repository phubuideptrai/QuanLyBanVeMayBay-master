using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DTO;

namespace DAO
{
    public class DAO_SanBay : DBConnect
    {
        public DataTable LoadSanBay()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT TENSANBAY FROM SANBAY", conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);

                conn.Close();

                return table;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
