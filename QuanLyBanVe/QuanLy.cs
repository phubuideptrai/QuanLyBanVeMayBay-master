﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyBanVe
{
    public static class QuanLy
    {
        public static List<DataGridViewRow> addedRows = new List<DataGridViewRow>();
        public static List<DataGridViewRow> modifiedRows = new List<DataGridViewRow>();
        public static List<DataGridViewRow> removedRows = new List<DataGridViewRow>();
        public static List<string> errors = new List<string>();
        public static DataGridViewRow duplicate;
        public static Point panel1_MouseDownLocation;

        public static Color AddedRowColor
        {
            get { return Color.LawnGreen; }
        }

        public static Color ModifiedRowColor
        {
            get { return Color.Gold; }
        }

        public static Color RemovedRowColor
        {
            get { return Color.LightSalmon; }
        }

        public static Color PrimaryKeyColor
        {
            get { return Color.DarkRed; }
        }

        public static Color ErrorTextColor
        {
            get { return Color.Red; }
        }

        public static Color ChangeLogColor
        {
            get { return Color.Black; }
        }

        private static string errorText = "";
        public static string ErrorText
        {
            get { return errorText; }
        }

        private static string changeLog = "";
        public static string ChangeLog
        {
            get { return changeLog; }
        }

        public static void LoadDataToDataGridView(DataGridView dgv) // >>>>>>>>>>>>>>>>>>>>>>>>> OK
        {
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // >>>>>>>>>>>>>>>>. OK
           
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.Cells[0].Style.ForeColor = PrimaryKeyColor;
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
            dgv.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgv.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            dgv.ClearSelection();
        }

        public static DataGridViewRow FindRowInDataGridView(DataGridView dgv, string keyword)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow && row.Cells[0].Value.ToString() == keyword)
                {
                    return row;
                }
            }
            return null;
        }

        public static DataGridViewRow FindRowInDataGridView(DataGridView dgv, string keyword, DataGridViewRow exceptionRow)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow && row != exceptionRow && row.Cells[0].Value.ToString() == keyword)
                {
                    return row;
                }
            }
            return null;
        }
        public static bool OkToCopy(DataGridViewRow row)
        {
            foreach (DataGridViewCell cell in row.Cells)
            {
                if (cell.ColumnIndex == 0)
                    continue;
                if (string.Empty == cell.Value.ToString())
                    return false;
            }
            return true;
        }

        public static void AddRowToDataGridView(DataGridView dgv) //>>>>>>>>>>>>>>>>>>>>. OK
        {
            DataTable table = (DataTable)dgv.DataSource;
            table.Rows.Add(table.NewRow());
            dgv.DataSource = table;
        }

        public static bool CheckAffectedRows(DataGridView dgv)
        {
            bool saveSuccess = true;
            foreach (DataGridViewRow row in addedRows)
            {
                string rowNumber = "Hàng " + (row.Index + 1).ToString() + "\n";
                string MACB = row.Cells[0].Value.ToString();
                if (MACB == "")
                {
                    saveSuccess = false;
                    string error = rowNumber + "Không thể thêm bản ghi mới: MACB (khóa chính) bị bỏ trống.";
                    errors.Add(error);
                }
                else
                {
                    DataGridViewRow existingRow = FindRowInDataGridView(dgv, MACB, row);
                    if (existingRow != null)
                    {
                        saveSuccess = false;
                        string error = rowNumber + "Không thể thêm bản ghi mới: Đã tồn tại bản ghi có MACB = '" + row.Cells[0].Value.ToString() + "' trong CSDL (hàng " + (existingRow.Index + 1).ToString() + ").";
                        errors.Add(error);
                    }
                }
                for (int i = 1; i < 9; i++)
                {
                    if (row.Cells[i].Value.ToString() == "")
                    {
                        saveSuccess = false;
                        string error = rowNumber + "Không thể thêm bản ghi mới: Có ít nhất một trường bị bỏ trống.";
                        errors.Add(error);
                        break;
                    }
                }
            }
            foreach (DataGridViewRow row in modifiedRows)
            {
                string rowNumber = "Hàng " + (row.Index + 1).ToString() + "\n";
                for (int i = 1; i < 9; i++)
                {
                    if (row.Cells[i].Value.ToString() == "")
                    {
                        saveSuccess = false;
                        string error = rowNumber + "Không thể sửa bản ghi: Có ít nhất một trường bị bỏ trống.";
                        errors.Add(error);
                        break;
                    }
                }
            }
            if (saveSuccess)
                WriteChangeLog();
            else WriteErrorText();
            return saveSuccess;
        }

        public static void AddParametersToCommand(SqlCommand cmd, DataGridViewRow row)
        {
            using (SqlConnection connection = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>>>>>>>>>> OK
            {
                connection.Open();
                SqlParameter parameter;

                parameter = new SqlParameter("@MaCB", row.Cells[0].Value.ToString());
                cmd.Parameters.Add(parameter);
                if (cmd.CommandText == "ThemCB" || cmd.CommandText == "SuaCB")
                {
                    parameter = new SqlParameter("@TenSBDi", row.Cells[1].Value.ToString());
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@TenSBDen", row.Cells[2].Value.ToString());
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@TenHHK", row.Cells[3].Value.ToString());
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ThoiGianKhoiHanh", Convert.ToDateTime(row.Cells[4].Value.ToString()));
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@ThoiGianDen", Convert.ToDateTime(row.Cells[5].Value.ToString()));
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@SoGheHang1", int.Parse(row.Cells[6].Value.ToString()));
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@SoGheHang2", int.Parse(row.Cells[7].Value.ToString()));
                    cmd.Parameters.Add(parameter);

                    parameter = new SqlParameter("@GiaVe", int.Parse(row.Cells[8].Value.ToString()));
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        private static void WriteErrorText()
        {
            errorText = "Cập nhật bảng [CHUYENBAY] không thành công - Không có thay đổi nào được lưu.\n";
            errorText += errors.Count.ToString() + " lỗi đã xảy ra:\n";
            foreach (string error in errors)
            {
                errorText += "* " + error + "\n";
            }
        }

        private static void WriteChangeLog()
        {
            changeLog = "Cập nhật bảng [CHUYENBAY] thành công - ";
            changeLog += (addedRows.Count + modifiedRows.Count + removedRows.Count).ToString() + " thay đổi đã được lưu (";
            changeLog += "thêm " + addedRows.Count.ToString() + " hàng, sửa " + modifiedRows.Count.ToString() + " hàng và xóa " + removedRows.Count.ToString() + " hàng).";
            if (addedRows.Count != 0 || modifiedRows.Count != 0 || removedRows.Count != 0)
            {
                changeLog += "\nCác thay đổi:\n";
                foreach (DataGridViewRow row in addedRows)
                {
                    changeLog += "* Đã thêm bản ghi có MACB = '" + row.Cells[0].Value.ToString() + "'.\n";
                }
                foreach (DataGridViewRow row in modifiedRows)
                {
                    changeLog += "* Đã sửa bản ghi có MACB = '" + row.Cells[0].Value.ToString() + "'.\n";
                }
                foreach (DataGridViewRow row in removedRows)
                {
                    changeLog += "* Đã xóa bản ghi có MACB = '" + row.Cells[0].Value.ToString() + "'.\n";
                }
            }
        }

        public static void ShowErrorText(Label labelToShow)
        {
            labelToShow.ForeColor = ErrorTextColor;
            labelToShow.Text = ErrorText;
        }

        public static void ShowChangeLog(Label labelToShow)
        {
            labelToShow.ForeColor = ChangeLogColor;
            labelToShow.Text = ChangeLog;
        }

        public static void LoadSanBay(ComboBox comboBox)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>>>>>>> OK
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT TENSANBAY FROM SANBAY", conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                DataTable table = new DataTable();
                dataAdapter.Fill(table);
                comboBox.DataSource = table;
                comboBox.DisplayMember = "TENSANBAY";
                comboBox.ValueMember = "TENSANBAY";
            }
        }

        public static void LoadHHK(ComboBox comboBox)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>>>>>> OK
            {
                conn.Open();
                SqlCommand comm = new SqlCommand("SELECT TENHHK FROM HANGHK", conn);
                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;
                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                comboBox.DataSource = datb;
                comboBox.DisplayMember = "TENHHK";
                comboBox.ValueMember = "TENHHK";

            }
        }

        public static void TraCuu(DataGridView dataGridView, ComboBox cbbDi, ComboBox cbbDen, DateTimePicker date)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>>> OK
            {
                dataGridView.RowHeadersVisible = false;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("TraCuu", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@MaSBDi", cbbDi.Text);
                comm.Parameters.Add(para);

                para = new SqlParameter("@MaSBDen", cbbDen.Text);
                comm.Parameters.Add(para);

                para = new SqlParameter("@ThoiGianBay", Convert.ToDateTime(date.Value.ToShortDateString()));
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;
                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                dataGridView.DataSource = datb;
                dataGridView.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                dataGridView.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
            dataGridView.ClearSelection();
        }
        public static void BaoCao(DataGridView gridView, DateTimePicker dateDi, DateTimePicker dateDen, TextBox txt)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>> OK
            {
                gridView.RowHeadersVisible = false;
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("BaoCao", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@TuNgay", Convert.ToDateTime(dateDi.Value.ToString()));
                comm.Parameters.Add(para);

                para = new SqlParameter("@DenNgay", Convert.ToDateTime(dateDen.Value.ToString()));
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;

                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                gridView.DataSource = datb;
                int sum = 0;
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    sum += Int32.Parse(gridView["DOANH THU", i].Value.ToString());
                    gridView["Tỉ lệ", i].Value = Math.Round(Convert.ToDouble(gridView["Tỉ lệ", i].Value), 2);
                }
                txt.Text = sum.ToString();
            }
            gridView.ClearSelection();
        }

        public static void BaoCaoNam(DataGridView gridView, ComboBox box, TextBox txt)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh))//>>>>>>>>> OK
            {
                gridView.RowHeadersVisible = false;
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("BaoCaoNam", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@NAM", Int32.Parse(box.Text));
                comm.Parameters.Add(para);

                SqlDataAdapter dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = comm;

                DataTable datb = new DataTable();
                dataAdapter.Fill(datb);
                gridView.DataSource = datb;
                int sum = 0;
                for (int i = 0; i < gridView.RowCount; i++)
                {
                    sum += Int32.Parse(gridView["DOANH THU", i].Value.ToString());
                    gridView["Tỉ lệ", i].Value = Math.Round(Convert.ToDouble(gridView["Tỉ lệ", i].Value), 2);
                }
                txt.Text = sum.ToString();
            }
            gridView.ClearSelection();
        }
        public static void CapNhatVe(DataGridView gridView, ComboBox boxMaCB, ComboBox boxMaVe)
        {

            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) // >>>>> OK
            {
                try
                {
                    gridView.RowHeadersVisible = false;
                    gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                    conn.Open();
                    SqlCommand comm = new SqlCommand("LoadVe", conn);
                    comm.CommandType = CommandType.StoredProcedure;

                    SqlParameter para = new SqlParameter("@MaCB", boxMaCB.Text);
                    comm.Parameters.Add(para);

                    para = new SqlParameter("@MaVe", boxMaVe.Text);
                    comm.Parameters.Add(para);

                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = comm;
                    DataTable datb = new DataTable();
                    adapter.Fill(datb);
                    gridView.DataSource = datb;
                }
                catch (Exception)
                {
                    gridView.DataSource = null;
                }
            }
            gridView.ClearSelection();
        }
        public static void LoadDuLieu(ComboBox boxMaCB, ComboBox boxMaVe)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>> OK
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT MACB FROM CHUYENBAY", conn);
                DataTable datb = new DataTable();

                adapter.Fill(datb);
                boxMaCB.DataSource = datb;
                boxMaCB.ValueMember = "MACB";
                boxMaCB.DisplayMember = "MACB";
                boxMaCB.Text = null;

                adapter = new SqlDataAdapter("SELECT MAVE FROM VE", conn);
                datb = new DataTable();
                adapter.Fill(datb);
                boxMaVe.DataSource = datb;
                boxMaVe.ValueMember = "MAVE";
                boxMaVe.DisplayMember = "MAVE";
                boxMaVe.Text = null;
            }
        }
        public static void ThanhToan(DataGridView gridView, ComboBox boxMaVe)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>>> OK
            {
                gridView.RowHeadersVisible = false;
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("ThanhToan", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter();
                if (boxMaVe.Text != "")
                {
                    para = new SqlParameter("@MaVe", boxMaVe.Text);
                }
                else
                {
                    string maVe = gridView.CurrentRow.Cells["MAVE"].Value.ToString();
                    para = new SqlParameter("@MaVe", maVe);
                }
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                int check = comm.ExecuteNonQuery();
                if (check != 0)
                {
                    MessageBox.Show("Thanh toán vé thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Vé đã được thanh toán trước đó!", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }
        public static void HoanVe(DataGridView gridView, ComboBox boxMaVe)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>> OK
            {
                gridView.RowHeadersVisible = false;
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("HoanVe", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter();
                if (boxMaVe.Text != "")
                {
                    para = new SqlParameter("@MaVe", boxMaVe.Text);
                }
                else
                {
                    string maVe = gridView.CurrentRow.Cells["MAVE"].Value.ToString();
                    para = new SqlParameter("@MaVe", maVe);
                }
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;

                int check = comm.ExecuteNonQuery();
                if (check != 0)
                {
                    MessageBox.Show("Cập nhật vé thành công!", "Thông báo", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Cập nhật vé thất bại!", "Thông báo", MessageBoxButtons.OK);
                }
            }
        }
        public static void ChiTietChuyenBay(DataGridView gridView, string maCB)
        {
            using (SqlConnection conn = new SqlConnection(Properties.Resources.localConnectionString_VietAnh)) //>>>>>>>> OK
            {
                
                gridView.RowHeadersVisible = false;
                gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                conn.Open();
                SqlCommand comm = new SqlCommand("ChiTietChuyenBay", conn);
                comm.CommandType = CommandType.StoredProcedure;

                SqlParameter para = new SqlParameter("@MaCB", maCB);
                comm.Parameters.Add(para);

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = comm;
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                gridView.DataSource = dataTable;

                gridView.Columns["THỜI GIAN KHỞI HÀNH"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                gridView.Columns["THỜI GIAN ĐẾN"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }
    }
}