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
    public class DAO_Ve : DBConnect
    {
        DTO_HangVe dtoHangVe = new DTO_HangVe();

        public DataTable LoadVe()
        {
            conn.Open();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT MAVE FROM VE", conn);
            DataTable datb = new DataTable();
            adapter.Fill(datb);
            conn.Close();
            return datb;
        }

        public DataTable LoadVe(string maCB, string maVe)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("LoadVe", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@MaCB", maCB);
                comm.Parameters.Add(para);

                para = new SqlParameter("@MaVe", maVe);
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;
                DataTable datb = new DataTable();
                adapter.Fill(datb);

                conn.Close();

                return datb;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable LietKeVe(string maCB)
        {
            conn.Open();

            SqlCommand comm = new SqlCommand("LietKeVe", conn);
            comm.CommandType = CommandType.StoredProcedure;
            comm.Parameters.AddWithValue("@MaCB", maCB);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = comm;
            DataTable datb = new DataTable();
            adapter.Fill(datb);

            conn.Close();
            return datb;
        }

        public DataTable ChonHangVe(string maHV, string maCB)
        {
            conn.Open();

            SqlCommand comm = new SqlCommand("ChonHangVe", conn);
            comm.CommandType = CommandType.StoredProcedure;

            SqlParameter para = new SqlParameter("@MaHangVe", maHV);
            comm.Parameters.Add(para);

            para = new SqlParameter("@MaCB", maCB);
            comm.Parameters.Add(para);

            SqlDataAdapter adapter = new SqlDataAdapter();
            adapter.SelectCommand = comm;

            DataTable dt = new DataTable();

            adapter.Fill(dt);
            conn.Close();

            return dt;
        }
        
        public bool ThanhToanVe(string maVe)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ThanhToan", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter();
                para = new SqlParameter("@MaVe", maVe);
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                if (comm.ExecuteNonQuery() > 0)
                    return true;
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool HoanVe(string maVe)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("HoanVe", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter();
                para = new SqlParameter("@MaVe", maVe);
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                if (comm.ExecuteNonQuery() > 0)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        public bool CapNhatVe(string maVe, string maTinhTrang)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("UpdateTinhTrangVe", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaTT", maTinhTrang);
                cmd.Parameters.AddWithValue("@MaVe", maVe);

                if (cmd.ExecuteNonQuery() > 0)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }
    }
}
