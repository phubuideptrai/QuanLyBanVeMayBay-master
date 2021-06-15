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
    public class DAO_HHK : DBConnect
    {
        public DataTable LoadHangHangKhong()
        {
            try
            {
                conn.Open();

                SqlCommand comm = new SqlCommand("SELECT TENHHK FROM HANGHK", conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;

                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);

                return datb;
            }
            catch(Exception)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
