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

namespace appQLKH
{
    public partial class FormQLKH : Form
    {
        public FormQLKH()
        {
            InitializeComponent();
        }

        private void FormQLKH_Load(object sender, EventArgs e)
        {
            radioMale.Checked = true;
            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM KhachHang", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvQLKH.DataSource = dt;
                conn.Close();
            }
        }
        public RadioButton selectedRadio()
        {
            if (radioMale.Checked)
            {
                radioFemale.Checked = false;
                return radioMale;
            }
            else if (radioFemale.Checked)
            {
                radioMale.Checked = false;
                return radioFemale;
            }
            else
                return null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO KhachHang (HoTen, GioiTinh, LoaiPhong, SoPhongThue) VALUES (@HoTen, @GioiTinh, @LoaiPhong, @SoPhongThue)", conn);
                if (txtTenKH.Text == "" || txtSoPhongThue.Text=="" || cbbLoaiPhong.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!","Thiếu thông tin",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                cmd.Parameters.AddWithValue("@HoTen", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", selectedRadio().Text.ToString());
                cmd.Parameters.AddWithValue("@LoaiPhong", cbbLoaiPhong.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@SoPhongThue", txtSoPhongThue.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Thêm khách hàng thành công!");
                    FormQLKH_Load(sender, e);
                }
                else
                {
                    MessageBox.Show("Thêm khách hàng thất bại!");
                }
                conn.Close();
            }
            txtTenKH.Clear();
            cbbLoaiPhong.SelectedIndex = -1;
            txtSoPhongThue.Clear();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                DataGridViewRow dataGridViewRow = dgvQLKH.CurrentRow;
                if (dgvQLKH.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("Vui lòng chọn một khách hàng để sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE KhachHang SET HoTen = @HoTen, GioiTinh = @GioiTinh, LoaiPhong = @LoaiPhong, SoPhongThue = @SoPhongThue WHERE MaKH = @MaKH", conn);
                cmd.Parameters.AddWithValue("@MaKH", dataGridViewRow.Cells[0].Value.ToString());
                cmd.Parameters.AddWithValue("@HoTen", txtTenKH.Text);
                cmd.Parameters.AddWithValue("@GioiTinh", selectedRadio().Text.ToString());
                cmd.Parameters.AddWithValue("@LoaiPhong", cbbLoaiPhong.SelectedItem.ToString());
                cmd.Parameters.AddWithValue("@SoPhongThue", txtSoPhongThue.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật khách hàng thành công!");
                    FormQLKH_Load(sender, e); // Refresh the data grid view
                }
                else
                {
                    MessageBox.Show("Cập nhật khách hàng thất bại!");
                }
                conn.Close();
            }
            txtTenKH.Clear();
            radioMale.Checked = true;
            cbbLoaiPhong.SelectedIndex = -1;
            txtSoPhongThue.Clear();
        }

        private void dgvQLKH_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvQLKH.Rows.Count)
            {
                DataGridViewRow row = dgvQLKH.Rows[e.RowIndex];
                txtTenKH.Text = row.Cells[1].Value.ToString();
                string gioiTinh = row.Cells[2].Value.ToString();
                if (gioiTinh == "Nam")
                {
                    radioMale.Checked = true;
                    radioFemale.Checked = false;
                }
                else
                {
                    radioFemale.Checked = true;
                    radioMale.Checked = false;
                }
                cbbLoaiPhong.SelectedItem = row.Cells[3].Value.ToString();
                txtSoPhongThue.Text = row.Cells[4].Value.ToString();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row
            if (dgvQLKH.CurrentRow == null || dgvQLKH.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Vui lòng chọn một khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow dataGridViewRow = dgvQLKH.CurrentRow;
            object maKHObj = dataGridViewRow.Cells[0].Value;
            string MaKH = maKHObj != null ? maKHObj.ToString() : string.Empty;

            if (string.IsNullOrEmpty(MaKH))
            {
                MessageBox.Show("Không tìm thấy mã khách hàng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (DialogResult.No == MessageBox.Show("Bạn có chắc chắn muốn xóa khách hàng này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }
            using (SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM KhachHang WHERE MaKH = @MaKH", conn);
                cmd.Parameters.AddWithValue("@MaKH", MaKH);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Xóa khách hàng thành công!");
                    FormQLKH_Load(sender, e); // Refresh the data grid view
                }
                else
                {
                    MessageBox.Show("Xóa khách hàng thất bại!");
                }
                conn.Close();
            }
            txtTenKH.Clear();
            cbbLoaiPhong.SelectedIndex = -1;
            txtSoPhongThue.Clear();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using(SqlConnection conn = Connection.GetSqlConnection())
            {
                conn.Open();
                if (txtTen.Text == "")
                {
                    FormQLKH_Load(sender, e);
                    return;
                }
                SqlCommand cmd = new SqlCommand("Select * From KhachHang Where HoTen = @HoTen", conn);
                cmd.Parameters.AddWithValue("@HoTen", txtTen.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvQLKH.DataSource = dt;
                conn.Close();
            }
        }
    }
}
