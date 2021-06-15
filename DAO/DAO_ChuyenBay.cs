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
    public class DAO_ChuyenBay : DBConnect
    {
        public DataTable TraCuu(string maSBDi, string maSBDen, DateTime dateTime)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("TraCuu", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@MaSBDi", maSBDi);
                comm.Parameters.Add(para);

                para = new SqlParameter("@MaSBDen", maSBDen);
                comm.Parameters.Add(para);

                para = new SqlParameter("@ThoiGianBay", dateTime);
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;
                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                conn.Close();
                return datb;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable ChiTietCB(string maCB)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("ChiTietChuyenBay", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@MaCB", maCB);
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                conn.Close();
                return dataTable;
            }
            catch
            {
                return null;
            }
        } 

        public DataTable LoadMaCB()
        {
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT MACB FROM CHUYENBAY", conn);
                DataTable datb = new DataTable();

                adapter.Fill(datb);
                conn.Close();
                return datb;

            }
            catch
            {
                return null;
            }
        }

        public DataTable LietKeCB()
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("LietKeCB", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = cmd;
                DataTable table = new DataTable();
                adapter.Fill(table);

                conn.Close();

                return table;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool ThemCB(DTO_ChuyenBay dtoCB)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("ThemCB", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter;

                parameter = new SqlParameter("@MaCB", dtoCB.MaCB);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenSBDi", dtoCB.SanBayDi);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenSBDen", dtoCB.SanBayDen);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenHHK", dtoCB.MaHHK);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@ThoiGianKhoiHanh", Convert.ToDateTime(dtoCB.ThoiGianBay));
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@ThoiGianDen", Convert.ToDateTime(dtoCB.ThoiGianDen1));
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@SoGheHang1", dtoCB.SoGheHang1);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@SoGheHang2", dtoCB.SoGheHang2);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@GiaVe", dtoCB.GiaVe);
                cmd.Parameters.Add(parameter);
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

        public bool SuaCB(DTO_ChuyenBay dtoCB)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SuaCB", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter;

                parameter = new SqlParameter("@MaCB", dtoCB.MaCB);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenSBDi", dtoCB.SanBayDi);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenSBDen", dtoCB.SanBayDen);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@TenHHK", dtoCB.MaHHK);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@ThoiGianKhoiHanh", Convert.ToDateTime(dtoCB.ThoiGianBay));
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@ThoiGianDen", Convert.ToDateTime(dtoCB.ThoiGianDen1));
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@SoGheHang1", dtoCB.SoGheHang1);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@SoGheHang2", dtoCB.SoGheHang2);
                cmd.Parameters.Add(parameter);

                parameter = new SqlParameter("@GiaVe", dtoCB.GiaVe);
                cmd.Parameters.Add(parameter);
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
