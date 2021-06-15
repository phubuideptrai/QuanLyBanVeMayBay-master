using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DAO;
using DTO;

namespace BUS
{
    public class BUS_HHK
    {
        DAO_HHK daoHHK = new DAO_HHK();

        public DataTable LoadHangHangKhong()
        {
            return daoHHK.LoadHangHangKhong(); 
        }
    }
}
