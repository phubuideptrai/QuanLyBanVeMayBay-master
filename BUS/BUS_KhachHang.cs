using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DTO;
using DAO;

namespace BUS
{
    public class BUS_KhachHang
    {
        DAO_KhachHang daoKhachHang = new DAO_KhachHang();

        public DataTable LoadKhachHang(string CMND)
        {
            return daoKhachHang.LoadKhachHang(CMND);
        }

        public DataTable LoadKhachHang(string CMND, string hoTen)
        {
            return daoKhachHang.LoadKhachHang(CMND, hoTen);
        }

        public bool TaoThanhVien(string maKH, string hoTen, string tuoi, bool gioiTinh, string cmnd, string diaChi, string sdt)
        {
            DTO_KhachHang dtoKhachHang = new DTO_KhachHang(maKH, hoTen, Int32.Parse(tuoi), gioiTinh, cmnd, diaChi, sdt);
            return daoKhachHang.TaoThanhVien(dtoKhachHang);
        }
    }
}
