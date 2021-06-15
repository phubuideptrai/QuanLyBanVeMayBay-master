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
    public class BUS_SanBay
    {
        DAO_SanBay daoSanBay = new DAO_SanBay();

        public DataTable LoadSanBay()
        {
            return daoSanBay.LoadSanBay();
        }
    }
}
