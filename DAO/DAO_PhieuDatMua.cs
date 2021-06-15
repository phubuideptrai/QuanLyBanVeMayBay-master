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
    public class DAO_PhieuDatMua : DBConnect
    {
        public bool TaoPhieuDatMua(DTO_PhieuDatMua dtoPDM)
        {
            try
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("TaoPhieuDatMua", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MaVe", dtoPDM.MaVe);
                cmd.Parameters.AddWithValue("@MaKH", dtoPDM.MaKH);
                cmd.Parameters.AddWithValue("@ThoiGianDat", dtoPDM.ThoiGianDat);
                cmd.Parameters.AddWithValue("@DaThanhToan", dtoPDM.DaThanhToan);

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
