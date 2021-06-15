using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_PhieuDatMua
    {
        private string maVe;
        public string MaVe
        {
            get { return maVe; }
            set { maVe = value; }
        }

        private string maKH;
        public string MaKH
        {
            get { return maKH; }
            set { maKH = value; }
        }

        private DateTime thoiGianDat;
        public DateTime ThoiGianDat
        {
            get { return thoiGianDat; }
            set { thoiGianDat = value; }
        }

        private bool daThanhToan;
        public bool DaThanhToan
        {
            get { return daThanhToan; }
            set { daThanhToan = value; }
        }

        public DTO_PhieuDatMua(string maVe, string maKH, DateTime thoiGianDat, bool daThanhToan)
        {
            this.maVe = maVe;
            this.maKH = maKH;
            this.thoiGianDat = thoiGianDat;
            this.daThanhToan = daThanhToan;
        }
    }
}
