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
    public class BUS_Ve
    {
        DAO_Ve daoVe = new DAO_Ve();

        public DataTable LoadVe()
        {
            return daoVe.LoadVe();
        }
        
        public DataTable LoadVe(string maCB, string maVe)
        {
            return daoVe.LoadVe(maCB, maVe);
        }

        public DataTable LietKeVe(string maCB)
        {
            return daoVe.LietKeVe(maCB);
        }

        public DataTable ChonHangVe(string maHV, string maCB)
        {
            return daoVe.ChonHangVe(maHV, maCB);
        }

        public bool ThanhToanVe(string maVe)
        {
            return daoVe.ThanhToanVe(maVe);
        }

        public bool HoanVe(string maVe)
        {
            return daoVe.HoanVe(maVe);
        }

        public bool CapNhatVe(string maVe, string maTinhTrang)
        {
            return daoVe.CapNhatVe(maVe, maTinhTrang);
        }
    }
}
