using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAO;
using System.Data;
namespace BUS
{
    public class BUS_ChuyenBay
    {
        DAO_ChuyenBay daoChuyenBay = new DAO_ChuyenBay();

        public DataTable TraCuu(string maSBDi, string maSBDen, DateTime datetime)
        {
            return daoChuyenBay.TraCuu(maSBDi, maSBDen, datetime);
        }

        public DataTable ChiTietCB(string maCB)
        {
            return daoChuyenBay.ChiTietCB(maCB);
        }

        public DataTable LoadMaCB()
        {
            return daoChuyenBay.LoadMaCB();
        }

        public DataTable LietKeCB()
        {
            return daoChuyenBay.LietKeCB();
        }

        public bool ThemCB(string maCB, string sanBayDi, string sanBayDen, string HHK, DateTime thoiGianKhoiHanh, DateTime thoiGianDen, int soGheHang1, int soGheHang2, int giaVe)
        {
            DTO_ChuyenBay dtoCB = new DTO_ChuyenBay(maCB, sanBayDi, sanBayDen, HHK, thoiGianKhoiHanh, thoiGianDen, soGheHang1, soGheHang2, giaVe);

            return daoChuyenBay.ThemCB(dtoCB);
        }

        public bool SuaCB(string maCB, string sanBayDi, string sanBayDen, string HHK, DateTime thoiGianKhoiHanh, DateTime thoiGianDen, int soGheHang1, int soGheHang2, int giaVe)
        {
            DTO_ChuyenBay dtoCB = new DTO_ChuyenBay(maCB, sanBayDi, sanBayDen, HHK, thoiGianKhoiHanh, thoiGianDen, soGheHang1, soGheHang2, giaVe);

            return daoChuyenBay.SuaCB(dtoCB);
        }
    }
}
