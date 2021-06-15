using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DTO;

namespace DAO
{
    public class DAO_BaoCao : DBConnect
    {
        public DataTable BaoCao(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("BaoCao", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@TuNgay", tuNgay);
                comm.Parameters.Add(para);

                para = new SqlParameter("@DenNgay", denNgay);
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;

                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                return datb;
            }
            catch
            {
                return null;
            }
        }
        public DataTable BaoCaoNam(string nam)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("BaoCaoNam", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@NAM", Int32.Parse(nam));
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;

                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                conn.Close();
                return datb;
            }
            catch
            {
                return null;
            }
        }
    }
}
