﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DTO_HangHangKhong
    {
        private string maHHK;

        public string MaHHK
        {
            get { return maHHK; }
            set { maHHK = value; }
        }
        private string tenHHK;

        public string TenHHK
        {
            get { return tenHHK; }
            set { tenHHK = value; }
        }         
        public DTO_HangHangKhong(string maHHK, string tenHHK)
        {
            this.maHHK = maHHK;
            this.tenHHK = tenHHK;
        }
    }
}
