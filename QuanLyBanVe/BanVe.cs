using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DAO;
using BUS;
using DTO;

namespace QuanLyBanVe
{
    public partial class BanVe : Form
    {
        BUS_Ve busVe = new BUS_Ve();
        BUS_KhachHang busKhachHang = new BUS_KhachHang();
        BUS_PhieuDatMua busPhieuDatMua = new BUS_PhieuDatMua();
        private string maCB;

        public BanVe(string MaCB)
        {
            InitializeComponent();
            this.maCB = MaCB;
        }


        

        private void BanVe_Load(object sender, EventArgs e)
        {
            dgvVe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvVe.DataSource = busVe.LietKeVe(this.maCB);
        }

        private void cboHangVe_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cboHangVe.SelectedItem.ToString() == "Hạng 1")
                {
                    dgvVe.DataSource = busVe.ChonHangVe("HV001", this.maCB);

                    int demVeDaBan = 0;

                    for (int i = 0; i < dgvVe.Rows.Count; i++)
                    {
                        if (dgvVe["TÌNH TRẠNG", i].Value.ToString() == "Còn trống")
                            break;
                        else
                            demVeDaBan++;
                    }

                    if (demVeDaBan == dgvVe.Rows.Count)
                    {
                        MessageBox.Show("Tất cả các vé hạng 1 đã được đặt/mua.", "Thông báo", MessageBoxButtons.OK);
                        dgvVe.DataSource = busVe.LietKeVe(this.maCB);
                    }
                }
                else if (cboHangVe.SelectedItem.ToString() == "Hạng 2")
                {
                    dgvVe.DataSource = busVe.ChonHangVe("HV002", this.maCB);

                    int demVeDaBan = 0;

                    for (int i = 0; i < dgvVe.Rows.Count; i++)
                    {
                        if (dgvVe["TÌNH TRẠNG", i].Value.ToString() == "Còn trống")
                            break;
                        else
                            demVeDaBan++;
                    }

                    if (demVeDaBan == dgvVe.Rows.Count)
                    {
                        MessageBox.Show("Tất cả các vé hạng 2 đã được đặt/mua.", "Thông báo", MessageBoxButtons.OK);
                        dgvVe.DataSource = busVe.LietKeVe(this.maCB);
                    }
                }
                else if (cboHangVe.SelectedItem.ToString() == "Tất cả")
                {
                    dgvVe.DataSource = busVe.LietKeVe(this.maCB);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message.ToString());
            }
        }

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnXacNhanMua_Click(object sender, EventArgs e)
        {
            if (txtCMND.Text == "" || txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng nhập thông tin của khách hàng !", "Nhắc nhở", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Vui lòng kiểm tra thông tin của khách đã đúng hay chưa.", "Nhắc nhở", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    DataTable dt = busKhachHang.LoadKhachHang(txtCMND.Text.Trim());
                    int demVe = 0;

                    for (int i = 0; i < dgvVe.Rows.Count; ++i)
                    {
                        if (dgvVe[0, i].Selected)
                        {
                            // Kiểm tra vé đã bán hay chưa
                            if (dgvVe["TÌNH TRẠNG", i].Value.ToString().Trim() != "Còn trống")
                            {
                                MessageBox.Show("Vé này đã được đặt/mua. Hãy chọn lại một vé khác.", "Thông báo", MessageBoxButtons.OK);
                            }
                            else
                            {
                                if (busVe.CapNhatVe(dgvVe["MAVE", i].Value.ToString(), "TT001"))
                                {
                                    DataRow KH = dt.Rows[dt.Rows.Count - 1];

                                    if (busPhieuDatMua.TaoPhieuDatMua(dgvVe[1, i].Value.ToString(), KH["MAKH"].ToString(), DateTime.Now, true))
                                    {
                                        MessageBox.Show("Bán vé thành công !", "Thông báo", MessageBoxButtons.OK);
                                        dgvVe.DataSource = busVe.LietKeVe(this.maCB);

                                        demVe++;
                                    }
                                }
                            }

                        }
                    }

                    if (demVe == 0)
                        MessageBox.Show("Không có vé nào được chọn. Vui lòng chọn 01 vé.", "Cảnh báo", MessageBoxButtons.OK);

                }
            }
        }


        private void btnXacNhanDat_Click(object sender, EventArgs e)
        {
            if (txtCMND.Text == "" || txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng nhập thông tin của khách hàng !", "Nhắc nhở", MessageBoxButtons.OK);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Vui lòng kiểm tra thông tin của khách đã đúng hay chưa.", "Nhắc nhở", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    DataTable dt = busKhachHang.LoadKhachHang(txtCMND.Text.Trim());
                    int demVe = 0;

                    for (int i = 0; i < dgvVe.Rows.Count; ++i)
                    {
                        if (dgvVe[0, i].Selected)
                        {
                            // Kiểm tra vé đã đặt hay chưa
                            if (dgvVe["TÌNH TRẠNG", i].Value.ToString().Trim() != "Còn trống")
                            {
                                MessageBox.Show("Vé này đã được đặt/mua. Hãy chọn lại một vé khác.", "Thông báo", MessageBoxButtons.OK);
                            }
                            else
                            {
                                if (busVe.CapNhatVe(dgvVe["MAVE", i].Value.ToString(), "TT002"))
                                {
                                    DataRow KH = dt.Rows[dt.Rows.Count - 1];

                                    if (busPhieuDatMua.TaoPhieuDatMua(dgvVe[1, i].Value.ToString(), KH["MAKH"].ToString(), DateTime.Now, false))
                                    {
                                        MessageBox.Show("Đặt vé thành công !", "Thông báo", MessageBoxButtons.OK);
                                        dgvVe.DataSource = busVe.LietKeVe(this.maCB);

                                        demVe++;
                                    }
                                }
                            }

                        }
                    }

                    if (demVe == 0)
                        MessageBox.Show("Không có vé nào được chọn. Vui lòng chọn 01 vé.", "Cảnh báo", MessageBoxButtons.OK);

                }
            }
        }

        private void btnKiemTra_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Properties.Resources.localConnectionString_CamTu))
            {
                DataTable dt = busKhachHang.LoadKhachHang(txtCMND.Text.Trim(), txtHoTen.Text.Trim());

                // Kiểm tra khách hàng đã là thành viên hay chưa
                if (dt.Rows.Count != 0)
                    MessageBox.Show("Đã là thành viên", "Kết quả kiểm tra", MessageBoxButtons.OK);
                else
                {
                    MessageBox.Show("Chưa là thành viên! Vui lòng nhập thông tin!", "Kết quả kiểm tra", MessageBoxButtons.OK);
                    TaoThanhVien ttv = new TaoThanhVien();
                    ttv.ShowDialog();
                }
            }
        }

        private void dgvVe_MouseClick(object sender, MouseEventArgs e)
        {

        }

    }
}
