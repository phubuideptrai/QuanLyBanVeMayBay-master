using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_KhachHang
    {
        private string maKH;
        public string MaKH
        {
            get { return maKH; }
            set { maKH = value; }
        }

        private string hoTen;
        public string HoTen
        {
            get { return hoTen; }
            set { hoTen = value; }
        }

        private string cmnd;
        public string CMND
        {
            get { return cmnd; }
            set { cmnd = value; }
        }

        private int tuoi;
        public int Tuoi
        {
            get { return tuoi; }
            set { tuoi = value; }
        }

        private bool gioiTinh;
        public bool GioiTinh
        {
            get { return gioiTinh; }
            set { gioiTinh = value; }
        }
        
        private string diaChi;
        public string DiaChi
        {
            get { return diaChi; }
            set { diaChi = value; }
        }

        private string sdt;
        public string SDT
        {
            get { return sdt; }
            set { sdt = value; }
        }

        public DTO_KhachHang(string maKH, string hoTen, int tuoi, bool gioiTinh, string cmnd, string diaChi, string sdt)
        {
            this.maKH = maKH;
            this.hoTen = hoTen;
            this.tuoi = tuoi;
            this.gioiTinh = gioiTinh;
            this.cmnd = cmnd;
            this.diaChi = diaChi;
            this.sdt = sdt;
        }
    }
}
