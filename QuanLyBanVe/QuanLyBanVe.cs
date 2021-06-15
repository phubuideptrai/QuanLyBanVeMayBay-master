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
    public partial class QuanLyBanVe : Form
    {
        // Khoi tao cac doi tuong chinh
        BUS_Ve busVe = new BUS_Ve();
        BUS_SanBay busSanBay = new BUS_SanBay();
        BUS_HHK busHHK = new BUS_HHK();
        BUS_ChuyenBay busChuyenBay = new BUS_ChuyenBay();

        // Khoi tao mau su dung trong chuong trinh
        Color PrimaryKeyColor = Color.DarkRed;
        Color AddedRowColor = Color.LawnGreen;
        Color ModifiedRowColor = Color.Gold;
        Color RemovedRowColor = Color.LightSalmon;
        Color ErrorTextColor = Color.Red;
        Color ChangeLogColor = Color.Black;

        // Khoi tao cac List se su dung
        List<DataGridViewRow> addedRows = new List<DataGridViewRow>();
        List<DataGridViewRow> modifiedRows = new List<DataGridViewRow>();
        List<DataGridViewRow> removedRows = new List<DataGridViewRow>();
        List<string> errors = new List<string>();
        DataGridViewRow duplicate;

        string errorText = "";
        string changeLog = "";

        public QuanLyBanVe()
        {
            InitializeComponent();
        }

        private void QuanLyBanVe_Load(object sender, EventArgs e)
        {
            //dataGridViewLichCB.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewLichCB.DataSource = busChuyenBay.LietKeCB();
            QuanLy.LoadDataToDataGridView(dataGridViewLichCB);


            //foreach (DataGridViewColumn column in dataGridViewLichCB.Columns)
            //{
            //    column.SortMode = DataGridViewColumnSortMode.NotSortable;
            //}

            //foreach (DataGridViewRow row in dataGridViewLichCB.Rows)
            //{
            //    row.Cells[0].Style.ForeColor = this.PrimaryKeyColor;
            //    row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            //}

            //dataGridViewLichCB.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            //dataGridViewLichCB.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            //dataGridViewLichCB.ClearSelection();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llbTaoThanhVien_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TaoThanhVien taoThanhVien = new TaoThanhVien();
            taoThanhVien.ShowDialog();
        }

        private void dataGridViewLichCB_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e) // OK
        {
            if (dataGridViewLichCB.SelectedRows.Count != 0)
                btnXoa.Enabled = true;
            else btnXoa.Enabled = false;

            if (dataGridViewLichCB.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = dataGridViewLichCB.SelectedRows[0];
                if (selectedRow.DefaultCellStyle.BackColor == this.AddedRowColor || selectedRow.DefaultCellStyle.BackColor == this.ModifiedRowColor)
                    btnSua.Enabled = true;
                else if (selectedRow.DefaultCellStyle.BackColor == this.RemovedRowColor)
                    btnSua.Enabled = false;
                else btnSua.Enabled = true;
            }
            else btnSua.Enabled = false;
        }

        private void btnThem_Click(object sender, EventArgs e) // OK
        {
            DataTable table = (DataTable)dataGridViewLichCB.DataSource;
            table.Rows.Add(table.NewRow());
            dataGridViewLichCB.DataSource = table;
            
            DataGridViewRow newRow = dataGridViewLichCB.Rows[dataGridViewLichCB.Rows.Count - 1];
            newRow.HeaderCell.Value = String.Format("{0}", newRow.Index + 1);

            addedRows.Add(newRow);
            newRow.DefaultCellStyle.BackColor = this.AddedRowColor;
            dataGridViewLichCB.ClearSelection();
            newRow.Selected = true;

            btnSave.Enabled = true;
            btnCancelChanges.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            dataGridViewLichCB.CurrentCell = dataGridViewLichCB.SelectedCells[0];
            if (dataGridViewLichCB.CurrentCell.OwningColumn == dataGridViewLichCB.Columns[1] ||
                dataGridViewLichCB.CurrentCell.OwningColumn == dataGridViewLichCB.Columns[2] ||
                dataGridViewLichCB.CurrentCell.OwningColumn == dataGridViewLichCB.Columns[3] ||
                dataGridViewLichCB.CurrentCell.OwningColumn == dataGridViewLichCB.Columns[4] ||
                dataGridViewLichCB.CurrentCell.OwningColumn == dataGridViewLichCB.Columns[5])
            {
                splitContainer1.Panel1.Enabled = false;

                cbbNoiDi.DataSource = busSanBay.LoadSanBay();
                cbbNoiDi.DisplayMember = "TENSANBAY";
                cbbNoiDi.ValueMember = "TENSANBAY";

                cbbNoiDen.DataSource = busSanBay.LoadSanBay();
                cbbNoiDen.DisplayMember = "TENSANBAY";
                cbbNoiDen.ValueMember = "TENSANBAY";

                cbbHHK.DataSource = busHHK.LoadHangHangKhong();
                cbbHHK.DisplayMember = "TENHHK";
                cbbHHK.ValueMember = "TENHHK";

                pnlThongTinCB.Location = new Point(splitContainer1.Panel2.Size.Width - pnlThongTinCB.Size.Width - 18, 0);
                pnlThongTinCB.Visible = true;
            }
            else
            {
                dataGridViewLichCB.BeginEdit(true);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e) // OK
        {
            foreach (DataGridViewRow row in dataGridViewLichCB.SelectedRows)
            {
                if (row.DefaultCellStyle.BackColor != this.RemovedRowColor)
                {
                    row.ReadOnly = true;
                    if (row.DefaultCellStyle.BackColor == this.AddedRowColor)
                    {
                        row.DefaultCellStyle.BackColor = this.RemovedRowColor;
                        addedRows.Remove(row);
                        continue;
                    }
                    if (row.DefaultCellStyle.BackColor == this.ModifiedRowColor)
                    {
                        removedRows.Add(row);
                        row.DefaultCellStyle.BackColor = this.RemovedRowColor;
                        modifiedRows.Remove(row);
                        continue;
                    }
                    removedRows.Add(row);
                    row.DefaultCellStyle.BackColor = this.RemovedRowColor;
                }
            }
            dataGridViewLichCB.ClearSelection();
            btnSave.Enabled = true;
            btnCancelChanges.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e) // OK
        {
            btnSave.Enabled = false;
            if (!QuanLy.CheckAffectedRows(dataGridViewLichCB))
            {
                /* Show error(s) to the user */
                MessageBox.Show("Có lỗi khi cập nhật dữ liệu vào CDSL. Vui lòng kiểm tra lại các thay đổi của bạn.", "Cập nhật không thành công");
                QuanLy.ShowErrorText(lblUpdateStatus);
                errors.Clear();
            }
            else
            {
                /* Save the changes */
                btnCancelChanges.Enabled = false;
                foreach (DataGridViewRow row in addedRows)
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Resources.localConnectionString_VietAnh))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("ThemCB", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        QuanLy.AddParametersToCommand(command, row);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (DataGridViewRow row in modifiedRows)
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Resources.localConnectionString_VietAnh))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SuaCB", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        QuanLy.AddParametersToCommand(command, row);
                        command.ExecuteNonQuery();
                    }
                }

                foreach (DataGridViewRow row in removedRows)
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Resources.localConnectionString_VietAnh))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("XoaCB", connection)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        QuanLy.AddParametersToCommand(command, row);
                        command.ExecuteNonQuery();
                    }
                }
                QuanLy.ShowChangeLog(lblUpdateStatus);
                addedRows.Clear();
                modifiedRows.Clear();
                removedRows.Clear();
                dataGridViewLichCB.DataSource = busChuyenBay.LietKeCB();
            }
        }

        private void btnCancelChanges_Click(object sender, EventArgs e) // OK
        {
            btnSave.Enabled = false;
            btnCancelChanges.Enabled = false;
            addedRows.Clear();
            modifiedRows.Clear();
            removedRows.Clear();
            dataGridViewLichCB.DataSource = busChuyenBay.LietKeCB();
        }

        private void buttonOK_Click(object sender, EventArgs e) // OK
        {
            DataGridViewRow modifiedRow = dataGridViewLichCB.SelectedRows[0];
            if (modifiedRow.DefaultCellStyle.BackColor != this.AddedRowColor &&
                modifiedRow.DefaultCellStyle.BackColor != this.ModifiedRowColor)
            {
                modifiedRows.Add(modifiedRow);
                modifiedRow.DefaultCellStyle.BackColor = this.ModifiedRowColor;
            }

            if (modifiedRow.DefaultCellStyle.BackColor == this.AddedRowColor)
                modifiedRow.Cells[0].Value = tbMaCB.Text;
            modifiedRow.Cells[1].Value = cbbMaSBDi.Text;
            modifiedRow.Cells[2].Value = cbbMaSBDen.Text;
            modifiedRow.Cells[3].Value = cbbHHK.Text;
            modifiedRow.Cells[4].Value = dtmDepartureTime.Text;
            modifiedRow.Cells[5].Value = dtmArrivalTime.Text;

            if (tbSoGheHang1.Text != string.Empty)
                modifiedRow.Cells[6].Value = tbSoGheHang1.Text;
            else modifiedRow.Cells[6].Value = 0;

            if (tbSoGheHang2.Text != string.Empty)
                modifiedRow.Cells[7].Value = tbSoGheHang2.Text;
            else modifiedRow.Cells[7].Value = 0;

            if (tbGiaVe.Text != string.Empty)
                modifiedRow.Cells[8].Value = tbGiaVe.Text;
            else modifiedRow.Cells[8].Value = 0;

            pnlThongTinCB.Visible = false;
            splitContainer1.Panel1.Enabled = true;
            btnSave.Enabled = true;
            btnCancelChanges.Enabled = true;
        }

        private void btnHuy_Click(object sender, EventArgs e) // OK
        {
            pnlThongTinCB.Visible = false;
            splitContainer1.Panel1.Enabled = true;
        }

        /* cellContextMenuStrip1 */
        private void dataGridViewLichCB_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e) // OK
        {
            if (e.Button == MouseButtons.Right)
            {
                dataGridViewLichCB.ClearSelection();
                DataGridViewRow clickedRow = dataGridViewLichCB.Rows[e.RowIndex];
                clickedRow.Selected = true;
                /* Determine whether modifyCellToolStripMenuItem can be enabled */
                if (clickedRow.DefaultCellStyle.BackColor == this.AddedRowColor || clickedRow.DefaultCellStyle.BackColor == this.ModifiedRowColor)
                {
                    modifyToolStripMenuItem.Enabled = true;
                }
                else if (clickedRow.DefaultCellStyle.BackColor == this.RemovedRowColor)
                {
                    modifyToolStripMenuItem.Enabled = false;
                }
                else modifyToolStripMenuItem.Enabled = true;
                /* Determine whether copyRowToolStripMenuItem can be enabled */
                if (QuanLy.OkToCopy(clickedRow))
                {
                    copyToolStripMenuItem.Enabled = true;
                }
                else
                {
                    copyToolStripMenuItem.Enabled = false;
                }
                /* Determine whether pasteRowToolStripMenuItem can be enabled */
                if (duplicate != null && clickedRow.DefaultCellStyle.BackColor != this.RemovedRowColor)
                {
                    pasteToolStripMenuItem.Enabled = true;
                }
                else
                {
                    pasteToolStripMenuItem.Enabled = false;
                }
                rowContextMenuStrip1.Show(MousePosition);
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            btnSua_Click(sender, e);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            btnXoa_Click(sender, e);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            DataGridViewRow rowToCopy = dataGridViewLichCB.SelectedRows[0];
            this.duplicate = (DataGridViewRow)rowToCopy.Clone();
            for (int i = 0; i < 9; i++)
            { 
                this.duplicate.Cells[i].Value = rowToCopy.Cells[i].Value;
            }
            lblUpdateStatus.ForeColor = this.ChangeLogColor;
            lblUpdateStatus.Text = "Đã sao chép hàng " + (rowToCopy.Index + 1).ToString() + " (MACB = '" + rowToCopy.Cells[0].Value.ToString() + "').";
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            DataGridViewRow modifiedRow = dataGridViewLichCB.SelectedRows[0];
            if (modifiedRow.DefaultCellStyle.BackColor != this.AddedRowColor &&
                modifiedRow.DefaultCellStyle.BackColor != this.ModifiedRowColor)
            {
                this.modifiedRows.Add(modifiedRow);
                modifiedRow.DefaultCellStyle.BackColor = this.ModifiedRowColor;
            }
            for (int i = 0; i < 9; i++)
            {
                if (i == 0 && modifiedRow.DefaultCellStyle.BackColor != this.AddedRowColor)
                    continue;
                modifiedRow.Cells[i].Value = this.duplicate.Cells[i].Value;
            }
            dataGridViewLichCB.ClearSelection();
            btnSave.Enabled = true;
            btnCancelChanges.Enabled = true;
        }

        /* contextMenuStrip1 */
        private void dataGridViewLichCB_MouseClick(object sender, MouseEventArgs e) // OK
        {
            if (e.Button == MouseButtons.Right)
            {
                saveToolStripMenuItem.Enabled = btnSave.Enabled;
                contextMenuStrip1.Show(MousePosition);
            }
        }

        private void themToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnThem_Click(sender, e);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            btnCancelChanges_Click(sender, e);
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            dataGridViewLichCB.SelectAll();
        }

        private void clearSelectionToolStripMenuItem_Click(object sender, EventArgs e) // OK
        {
            dataGridViewLichCB.ClearSelection();
        }

        private void QuanLyBanVe_KeyDown(object sender, KeyEventArgs e) // OK
        {
            if (pnlThongTinCB.Visible && e.KeyCode == Keys.Escape)
            {
                pnlThongTinCB.Visible = false;
                splitContainer1.Panel1.Enabled = true;
                e.SuppressKeyPress = true;
            }
            else if (dataGridViewLichCB.SelectedRows.Count != 0 && e.KeyCode == Keys.Delete)
            {
                btnXoa_Click(sender, e);
            }
        }

        // Dinh vi Location
        private Point panel1_MouseDownLocation;

        private void panel1_MouseDown(object sender, MouseEventArgs e) // OK
        {
            if (e.Button == MouseButtons.Left)
            {
                panel1_MouseDownLocation = e.Location;
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e) // OK
        {
            if (e.Button == MouseButtons.Left)
            {
                int left = e.X + pnlThongTinCB.Left - panel1_MouseDownLocation.X;
                if (left >= 0 && left <= splitContainer1.Panel2.Size.Width - pnlThongTinCB.Size.Width - 18)
                    pnlThongTinCB.Left = left;
                else if (left < 0)
                    pnlThongTinCB.Left = 0;
                else
                    pnlThongTinCB.Left = splitContainer1.Panel2.Size.Width - pnlThongTinCB.Size.Width - 18;

                int top = e.Y + pnlThongTinCB.Top - panel1_MouseDownLocation.Y;
                if (top >= 0 && top <= splitContainer1.Panel2.Size.Height - pnlThongTinCB.Size.Height)
                    pnlThongTinCB.Top = top;
                else if (top < 0)
                    pnlThongTinCB.Top = 0;
                else
                    pnlThongTinCB.Top = splitContainer1.Panel2.Size.Height - pnlThongTinCB.Size.Height;
            }
        }

        /* dataGridView2 */
        private void cbbNoiDi_DropDown(object sender, EventArgs e)
        {
            cbbNoiDi.DataSource = busSanBay.LoadSanBay();
            cbbNoiDi.DisplayMember = "TENSANBAY";
            cbbNoiDi.ValueMember = "TENSANBAY";
        }
        private void cbbNoiDen_DropDown(object sender, EventArgs e)
        {
            cbbNoiDen.DataSource = busSanBay.LoadSanBay();
            cbbNoiDen.DisplayMember = "TENSANBAY";
            cbbNoiDen.ValueMember = "TENSANBAY";
        }

        private void gridViewTraCuu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnBanVe2.Enabled = true;
            gridViewChiTiet.DataSource = busChuyenBay.ChiTietCB(getMaCB(gridViewTraCuu));

            gridViewChiTiet.RowHeadersVisible = false;
            gridViewChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewChiTiet.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            gridViewChiTiet.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }

        private void btnBanVe2_Click(object sender, EventArgs e)
        {          
            BanVe formBanVe = new BanVe(getMaCB(gridViewTraCuu));
            formBanVe.ShowDialog();
        }

        private string getMaCB(DataGridView dataGridView1)
        {
            return dataGridView1.CurrentRow.Cells[0].Value.ToString();
        }

        private void gridViewTraCuu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView.HitTestInfo hti = ((DataGridView)sender).HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    contextMenuStrip2.Show(MousePosition.X, MousePosition.Y);
                    gridViewTraCuu.ClearSelection();
                    gridViewTraCuu.Rows[hti.RowIndex].Selected = true;
                }
            }
        }
        private void chiTiếtToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void bánVéToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnBanVe2_Click(sender, e);
        }

        private void làmMớiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            btnTraCuu_Click(sender, e);
        }

        private void báoCáoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            BaoCao formBaoCao = new BaoCao();
            formBaoCao.ShowDialog();
        }

        private void báoCáoNămToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BaoCaoNam formBaoCaoNam = new BaoCaoNam();
            formBaoCaoNam.ShowDialog();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            gridViewCapNhatVe.RowHeadersVisible = false;
            gridViewCapNhatVe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            gridViewCapNhatVe.DataSource = busVe.CapNhatVe(cbbMaCB.Text, cbbMaVe.Text);
            gridViewCapNhatVe.ClearSelection();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            cbbMaVe.DataSource = busVe.LoadVe();
            cbbMaVe.ValueMember = "MAVE";
            cbbMaVe.DisplayMember = "MAVE";
            cbbMaVe.Text = null;

            cbbNoiDi.DataSource = busSanBay.LoadSanBay();
            cbbNoiDi.DisplayMember = "TENSANBAY";
            cbbNoiDi.ValueMember = "TENSANBAY";

            cbbNoiDen.DataSource = busSanBay.LoadSanBay();
            cbbNoiDen.DisplayMember = "TENSANBAY";
            cbbNoiDen.ValueMember = "TENSANBAY";
            // cbbMaCB.DataSource = busCB.LoadCB();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            gridViewCapNhatVe.RowHeadersVisible = false;
            gridViewCapNhatVe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string maVe = "";

            if (cbbMaVe.Text != "")
            {
                maVe = cbbMaVe.Text;
            }
            else
            {
                maVe = gridViewCapNhatVe.CurrentRow.Cells["MAVE"].Value.ToString();
            }

            if (busVe.ThanhToanVe(maVe))
            {
                MessageBox.Show("Thanh toán vé thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Vé đã được thanh toán trước đó!", "Thông báo", MessageBoxButtons.OK);
            }

            gridViewCapNhatVe.DataSource = busVe.CapNhatVe(cbbMaCB.Text, cbbMaVe.Text);
            gridViewCapNhatVe.ClearSelection();
        }
        private void gridViewCapNhatVe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnThanhToan.Enabled = true;
            btnHoanVe.Enabled = true;            
        }

        private void btnTraCuu_Click(object sender, EventArgs e)
        {

            if (cbbNoiDen.Text == cbbNoiDi.Text || cbbNoiDi.Text == null || cbbNoiDen.Text == null)
            {
                MessageBox.Show("Không thể tra cứu");
            }
            else
            {
                gridViewTraCuu.DataSource = busChuyenBay.TraCuu(cbbNoiDi.Text, cbbNoiDen.Text, dateTraCuu.Value);
                gridViewTraCuu.RowHeadersVisible = false;
                gridViewTraCuu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                gridViewTraCuu.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                gridViewTraCuu.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                gridViewTraCuu.ClearSelection();
            }

        }

        private void btnHoanVe_Click(object sender, EventArgs e)
        {
            gridViewCapNhatVe.RowHeadersVisible = false;
            gridViewCapNhatVe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            string maVe = "";
            if (cbbMaVe.Text != "")
            {
                maVe = cbbMaVe.Text;
            }
            else
            {
                maVe = gridViewCapNhatVe.CurrentRow.Cells["MAVE"].Value.ToString();
            }

            if (busVe.HoanVe(maVe))
            {
                MessageBox.Show("Cập nhật vé thành công!", "Thông báo", MessageBoxButtons.OK);
            }
            else
            {
                MessageBox.Show("Cập nhật vé thất bại!", "Thông báo", MessageBoxButtons.OK);
            }

            gridViewCapNhatVe.DataSource = busVe.CapNhatVe(cbbMaCB.Text, cbbMaVe.Text);
            gridViewCapNhatVe.ClearSelection();
        }
    }
}