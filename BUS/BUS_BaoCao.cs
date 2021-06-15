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
    public class BUS_BaoCao
    {
        DAO_BaoCao daoBaoCao = new DAO_BaoCao();
        public DataTable BaoCao(DateTime tuNgay, DateTime denNgay)
        {
            return daoBaoCao.BaoCao(tuNgay, denNgay);
        }

        public DataTable BaoCaoNam(string nam)
        {
            return daoBaoCao.BaoCaoNam(nam);
        }
    }
}
