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
    public partial class FormKhachHang : Form
    {
        public bool isAdmin;
        public FormKhachHang(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            KiemTraQuyen();
        }

        private void FormKhachHang_Load(object sender, EventArgs e)
        {
            connect connectionManager = new connect();

            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                string query = "SELECT * FROM khachhang";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvKhachHang.DataSource = dataTable;

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
                dgvKhachHang.CellClick += dgvKhachHang_CellClick;
                btnTimKH.Click += BtnTimKH_Click;
            }

        }

        private void BtnTimKH_Click(object sender, EventArgs e)
        {
            string searchText = txbMaKH.Text.Trim();

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
                string query = "SELECT * FROM khachhang WHERE makh LIKE @SearchText OR tenkhachhang LIKE @SearchText";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dataTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(dataTable);

                // Hiển thị dữ liệu trên DataGridView
                dgvKhachHang.DataSource = dataTable;
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

        private void dgvKhachHang_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu của hàng được chọn
                DataGridViewRow row = dgvKhachHang.Rows[e.RowIndex];

                // Hiển thị thông tin của truyện lên các điều khiển nhập liệu
                txbMaKH.Text = row.Cells["makh"].Value.ToString();
                txbTenKH.Text = row.Cells["tenkhachhang"].Value.ToString();
                txbSDTKH.Text = row.Cells["sdtkhach"].Value.ToString();
                txbDiaChi.Text = row.Cells["diachi"].Value.ToString();
                DateNgayThemKH.Value = Convert.ToDateTime(row.Cells["ngaythemkhach"].Value);

            }
        }

        private void btnXoaKH_Click(object sender, EventArgs e)
        {
            if (dgvKhachHang.SelectedRows.Count > 0)
            {
                string maKhach = dgvKhachHang.SelectedRows[0].Cells["makh"].Value.ToString();

                connect connectionManager = new connect();
                MySqlConnection connection = connectionManager.getConnect();

                try
                {
                    connection.Open();

                    string query = "DELETE FROM khachhang WHERE makh = @makh";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@makh", maKhach);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Khách đã được xóa khỏi cơ sở dữ liệu.");

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
                MessageBox.Show("Vui lòng chọn một Khách để xóa.");
            }

        }

        private void btnSuaKH_Click(object sender, EventArgs e)
        {
            // Lấy thông tin truyện từ các điều khiển nhập liệu
            string maKH = txbMaKH.Text;
            string tenKH = txbTenKH.Text;
            int sdtKH = Convert.ToInt32(txbSDTKH.Text);
            string diachiKH = txbDiaChi.Text;
            DateTime ngayThemKhach = DateNgayThemKH.Value;

            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Tạo kết nối
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();
                //makh,tenkhachhang, sdtkhach, diachi, ngaythemkhach
                // Tạo câu truy vấn SQL để cập nhật thông tin truyện
                string query = "UPDATE khachhang SET makh = @makh, tenkhachhang = @tenkhachhang, sdtkhach = @sdtkhach, diachi = @diachi, ngaythemkhach = @ngaythemkhach " +
                               "WHERE makh = @makh";

                // Tạo một đối tượng MySqlCommand và thiết lập tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@makh", maKH);
                cmd.Parameters.AddWithValue("@tenkhachhang", tenKH);
                cmd.Parameters.AddWithValue("@sdtkhach", sdtKH);
                cmd.Parameters.AddWithValue("@diachi", diachiKH);
                cmd.Parameters.AddWithValue("@ngaythemkhach", ngayThemKhach);
                

                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Thông tin Khách Hàng đã được cập nhật.");

                // Làm mới DataGridView để cập nhật dữ liệu
                TaiLai_Data();
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

                string query = "SELECT * FROM khachhang";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                dgvKhachHang.DataSource = dataTable;
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

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            string maKH = txbMaKH.Text;
            string tenKH = txbTenKH.Text;
            int sdtKH = Convert.ToInt32(txbSDTKH.Text);
            string diachiKH = txbDiaChi.Text;
            DateTime ngayThemKhach = DateNgayThemKH.Value;


            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Gọi phương thức getConnect() để nhận một đối tượng MySqlConnection
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để thêm dữ liệu vào bảng truyentranh,
                string query = "INSERT INTO khachhang (makh,tenkhachhang, sdtkhach, diachi, ngaythemkhach) " +
                               "VALUES (@makh,@tenkhachhang, @sdtkhach, @diachi, @ngaythemkhach)";

                // Tạo một đối tượng MySqlCommand và thiết lập các tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@makh", maKH);
                cmd.Parameters.AddWithValue("@tenkhachhang", tenKH);
                cmd.Parameters.AddWithValue("@sdtkhach", sdtKH);
                cmd.Parameters.AddWithValue("@diachi", diachiKH);
                cmd.Parameters.AddWithValue("@ngaythemkhach", ngayThemKhach);


                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Đã Thêm Khách Hàng vào dữ liệu của Shop !");
                //tải lại 
                TaiLai_Data();
                // Sau khi hoàn thành, đóng kết nối
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
            FormNhanVien formNhanVien = new FormNhanVien(isAdmin);
            formNhanVien.ShowDialog();
            this.Close();
        }

        private void quảnLýKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain(isAdmin);
            formMain.ShowDialog();
            this.Close();
        }

        private void kháchHàngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("bạn đang ở trang quản lý Khách Hàng.");
        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHoaDon formHoaDon = new FormHoaDon(isAdmin); 
            formHoaDon.ShowDialog();
            this.Close();
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDangNhap formDangNhap = new FormDangNhap();
            formDangNhap.ShowDialog();
            this.Close();
        }

        private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát khỏi ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
