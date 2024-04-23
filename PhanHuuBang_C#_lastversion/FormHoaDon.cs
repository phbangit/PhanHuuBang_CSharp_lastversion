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
    public partial class FormHoaDon : Form
    {

        public bool isAdmin;
        public FormHoaDon(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            KiemTraQuyen();
            
        }

        private void FormHoaDon_Load(object sender, EventArgs e)
        {
            connect connectionManager = new connect();

            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                string query = "SELECT * FROM hoadon";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvHoaDon.DataSource = dataTable;

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
                // dgvKhachHang.CellClick += dgvTruyenTranh_CellClick;
                // btnTimKiemKH.Click += BtnTimKiemKH_Click;
                dgvHoaDon.CellClick += DgvHoaDon_CellClick;
            }
        }

        private void DgvHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu của hàng được chọn
                DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];

                // Hiển thị thông tin của truyện lên các điều khiển nhập liệu
                txbMaHD.Text = row.Cells["mahoadon"].Value.ToString();
                txbTenKHHD.Text = row.Cells["tenkhachhang"].Value.ToString();
                txbThanhTien.Text = row.Cells["thanhtien"].Value.ToString();
                DateNgayLapHD.Value = Convert.ToDateTime(row.Cells["ngaylaphd"].Value);
            }
        }

        private void btnTimHD_Click(object sender, EventArgs e)
        {
            string searchText = txbMaHD.Text.Trim();


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
                string query = "SELECT * FROM hoadon WHERE mahoadon LIKE @SearchText OR tenkhachhang LIKE @SearchText";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dataTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(dataTable);

                // Hiển thị dữ liệu trên DataGridView
                dgvHoaDon.DataSource = dataTable;
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

        private void btnSuaHD_Click(object sender, EventArgs e)
        {
            // Lấy thông tin truyện từ các điều khiển nhập liệu
            string mahoadon = txbMaHD.Text;
            string tenkhachhang = txbTenKHHD.Text;
            int thanhtien = Convert.ToInt32(txbThanhTien.Text);
            DateTime ngaylaphd = DateNgayLapHD.Value;

            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Tạo kết nối
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để cập nhật thông tin truyện
                string query = "UPDATE hoadon SET mahoadon = @mahoadon, tenkhachhang = @tenkhachhang, thanhtien = @thanhtien, ngaylaphd = @ngaylaphd " +
                               "WHERE mahoadon = @mahoadon";

                // Tạo một đối tượng MySqlCommand và thiết lập tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@mahoadon", mahoadon);
                cmd.Parameters.AddWithValue("@tenkhachhang", tenkhachhang);
                cmd.Parameters.AddWithValue("@thanhtien", thanhtien);
                cmd.Parameters.AddWithValue("@ngaylaphd", ngaylaphd);


                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Thông tin Hóa đơn đã được cập nhật thành công.");

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

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            int maHD = Convert.ToInt32(txbMaHD.Text);
            string tenKHHD = txbTenKHHD.Text;
            int ThanhTien = Convert.ToInt32(txbThanhTien.Text);
            DateTime ngayThemHD = DateNgayLapHD.Value;


            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Gọi phương thức getConnect() để nhận một đối tượng MySqlConnection
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để thêm dữ liệu vào bảng truyentranh,
                string query = "INSERT INTO hoadon (mahoadon,tenkhachhang, thanhtien, ngaylaphd) " +
                               "VALUES (@mahoadon,@tenkhachhang, @thanhtien, @ngaylaphd)";

                // Tạo một đối tượng MySqlCommand và thiết lập các tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@mahoadon", maHD);
                cmd.Parameters.AddWithValue("@tenkhachhang", tenKHHD);
                cmd.Parameters.AddWithValue("@thanhtien", ThanhTien);
                cmd.Parameters.AddWithValue("@ngaylaphd", ngayThemHD);


                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Đã Thêm hóa đơn !", " Thông Báo !");
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

        private void btnXoaHD_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.SelectedRows.Count > 0)
            {
                string mahoadon = dgvHoaDon.SelectedRows[0].Cells["mahoadon"].Value.ToString();

                connect connectionManager = new connect();
                MySqlConnection connection = connectionManager.getConnect();

                try
                {
                    connection.Open();

                    string query = "DELETE FROM hoadon WHERE mahoadon = @mahoadon";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@mahoadon", mahoadon);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Hóa Đơn đã được xóa .", "Thông Báo !");

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
                MessageBox.Show("Vui lòng chọn một hóa đơn để xóa.");
            }
        }


        private void TaiLai_Data()
        {
            connect connectionManager = new connect();
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "SELECT * FROM hoadon";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                dgvHoaDon.DataSource = dataTable;
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
                btnSuaHD.Enabled = false;
                btnXoaHD.Enabled = false;
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
            FormKhachHang formKhachHang = new FormKhachHang(isAdmin);
            formKhachHang.ShowDialog();
            this.Close();

        }

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn đang ở trang quản lý hóa đơn.");
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
