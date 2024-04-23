using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanHuuBang_C__lastversion
{
    public partial class FormNhanVien : Form
    {

        public bool isAdmin;
        public FormNhanVien(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            KiemTraQuyen();
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            connect connectionManager = new connect();

            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "SELECT * FROM nhanvien";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvNhanVien.DataSource = dataTable;
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            dgvNhanVien.CellClick += DgvNhanVien_CellClick;
            /* dgvTruyenTranh.CellClick += dgvTruyenTranh_CellClick;
             btnTimKiem_Truyen.Click += btnTimKiem_Truyen_Click; */
        }

        private void DgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvNhanVien.Rows[e.RowIndex];

            // Hiển thị thông tin của truyện lên các điều khiển nhập liệu
            txbMaNV.Text = row.Cells["manhanvien"].Value.ToString();
            txbTenNV.Text = row.Cells["tennv"].Value.ToString();
            txbSDT.Text = row.Cells["sdtnv"].Value.ToString();
            txbTK.Text = row.Cells["taikhoan"].Value.ToString();
            txbMatKhauNV.Text = row.Cells["matkhau"].Value.ToString();
            string loaiTaiKhoan = row.Cells["loaitaikhoan"].Value.ToString();

            cmbLoaiTK.SelectedItem = loaiTaiKhoan;
        }

        private void btnThemNV_Click(object sender, EventArgs e)
        {
            string manhanvien = txbMaNV.Text;
            string tennhanvien = txbTenNV.Text;
            string sodienthoai = txbSDT.Text;
            string taikhoan = txbTK.Text;
            string matkhau = txbMatKhauNV.Text;
            string loaiTaiKhoan = cmbLoaiTK.SelectedItem.ToString();

            connect connectionManager = new connect();

            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "INSERT INTO nhanvien (manhanvien,tennv, sdtnv, taikhoan, matkhau, loaitaikhoan) " +
                               "VALUES (@manhanvien,@tennv , @sdtnv, @taikhoan, @matkhau, @loaitaikhoan)";

                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@manhanvien", manhanvien);
                cmd.Parameters.AddWithValue("@tennv", tennhanvien);
                cmd.Parameters.AddWithValue("@sdtnv", sodienthoai);
                cmd.Parameters.AddWithValue("@taikhoan", taikhoan);
                cmd.Parameters.AddWithValue("@matkhau", matkhau);
                cmd.Parameters.AddWithValue("@loaitaikhoan", loaiTaiKhoan);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Đã Thêm Nhân viên !", "Thông báo !");

                TaiLai_Data();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                string manhanvien = dgvNhanVien.SelectedRows[0].Cells["manhanvien"].Value.ToString();

                connect connectionManager = new connect();
                MySqlConnection connection = connectionManager.getConnect();

                try
                {
                    connection.Open();

                    string query = "DELETE FROM nhanvien WHERE manhanvien = @manv";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@manv", manhanvien);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Nhân viên đã được xóa.", "Thông báo !");

                    TaiLai_Data();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một Nhân viên để xóa.");
            }
        }

        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            string manhanvien = txbMaNV.Text;
            string tennhanvien = txbTenNV.Text;
            string sodienthoai = txbSDT.Text;
            string taikhoan = txbTK.Text;
            string matkhau = txbMatKhauNV.Text;
            string loaiTaiKhoan = cmbLoaiTK.SelectedItem.ToString();

            connect connectionManager = new connect();
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "UPDATE nhanvien SET manhanvien = @manv, tennv = @Tennv, sdtnv = @sodthoai, taikhoan = @Taikhoan, matkhau = @Matkhau, loaitaikhoan = @Loaitk " +
                               "WHERE manhanvien = @manv";

                // Tạo một đối tượng MySqlCommand và thiết lập tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@manv", manhanvien);
                cmd.Parameters.AddWithValue("@Tennv", tennhanvien);
                cmd.Parameters.AddWithValue("@sodthoai", sodienthoai);
                cmd.Parameters.AddWithValue("@Taikhoan", taikhoan);
                cmd.Parameters.AddWithValue("@Matkhau", matkhau);
                cmd.Parameters.AddWithValue("@Loaitk", loaiTaiKhoan);

                // Thực thi truy vấn
                cmd.ExecuteNonQuery();
                MessageBox.Show("Thông tin Nhân viên đã được cập nhật.", "Thông báo !");
                TaiLai_Data();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void btnTimNV_Click(object sender, EventArgs e)
        {
            string searchText = txbMaNV.Text.Trim();

            // Nếu không có dữ liệu để tìm kiếm, hiển thị tất cả dữ liệu
            if (string.IsNullOrEmpty(searchText))
            {
                TaiLai_Data();
                return;
            }

            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Tạo kết nối
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để tìm kiếm dữ liệu
                string query = "SELECT * FROM nhanvien WHERE manhanvien LIKE @SearchText OR tennv LIKE @SearchText";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dataTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(dataTable);

                // Hiển thị dữ liệu trên DataGridView
                dgvNhanVien.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                // Đóng kết nối sau khi hoàn tất
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void TaiLai_Data()
        {
            connect connectionManager = new connect();
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "SELECT * FROM nhanvien";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                dgvNhanVien.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        private void KiemTraQuyen()
        {
            if (!isAdmin)
            {
                // Ẩn các chức năng không phù hợp với quyền người dùng

                quảnLýNhânViênToolStripMenuItem.Enabled = false;
            }
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn đang ở trang quản lý nhân viên.");
        }

        private void quảnLýKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain(isAdmin);
            formMain.ShowDialog();
            this.Close();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKhachHang formKhachHang =  new FormKhachHang(isAdmin);
            formKhachHang.ShowDialog();
            this.Close();
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHoaDon formHoaDon   = new FormHoaDon(isAdmin);
            formHoaDon.ShowDialog();
            this.Close();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.ShowDialog();
            this.Close();
        }

        private void tHoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát khỏi ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void txbTK_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbMatKhauNV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbSDT_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbTenNV_TextChanged(object sender, EventArgs e)
        {

        }

        private void txbMaNV_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbLoaiTK_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
