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
    public partial class FormMain : Form
    {

        internal bool isThoat;
        internal static Action<object, EventArgs> DangXuat;
        public bool isAdmin;
        public FormMain(bool isAdmin)
        {
            InitializeComponent();
            this.isAdmin = isAdmin;
            KiemTraQuyen();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            connect connectionManager = new connect();

            // Gọi phương thức getConnect() để nhận một đối tượng MySqlConnection
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Thực hiện các thao tác với cơ sở dữ liệu tại đây

                string query = "SELECT * FROM truyentranh";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvTruyenTranh.DataSource = dataTable;
                // Sau khi hoàn thành, đóng kết nối
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

            dgvTruyenTranh.CellClick += dgvTruyenTranh_CellClick;
            btnTimKiemTruyen.Click += btnTimKiemTruyen_Click;
        }

        private void btnTimKiemTruyen_Click(object sender, EventArgs e)
        {
            string searchText = txbMaTruyen.Text.Trim();

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
                string query = "SELECT * FROM truyentranh WHERE matruyentranh LIKE @SearchText OR tentruyen LIKE @SearchText";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                adapter.SelectCommand.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");
                DataTable dataTable = new DataTable();

                // Đổ dữ liệu vào DataTable
                adapter.Fill(dataTable);

                // Hiển thị dữ liệu trên DataGridView
                dgvTruyenTranh.DataSource = dataTable;
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

        private void dgvTruyenTranh_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn không
            if (e.RowIndex >= 0)
            {
                // Lấy dữ liệu của hàng được chọn
                DataGridViewRow row = dgvTruyenTranh.Rows[e.RowIndex];

                // Hiển thị thông tin của truyện lên các điều khiển nhập liệu
                txbMaTruyen.Text = row.Cells["matruyentranh"].Value.ToString();
                txbTenTruyen.Text = row.Cells["tentruyen"].Value.ToString();
                txbSoTap.Text = row.Cells["sotap"].Value.ToString();
                txbGiaBan.Text = row.Cells["giaban"].Value.ToString();
                DateNgayPHTruyen.Value = Convert.ToDateTime(row.Cells["ngayphathanh"].Value);
                txbTonKho.Text = row.Cells["tonkho"].Value.ToString();
            }
        }

        private void KiemTraQuyen()
        {
            if (!isAdmin)
            {
                // Ẩn các chức năng không phù hợp với quyền người dùng
                quảnLýNhânViênToolStripMenuItem.Enabled = false;
                // và các tương tự
            }
        }

        private void TaiLai_Data()
        {
            connect connectionManager = new connect();
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                connection.Open();

                string query = "SELECT * FROM truyentranh";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                dgvTruyenTranh.DataSource = dataTable;
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

        private void btnThemTruyen_Click(object sender, EventArgs e)
        {
            string maTruyen = txbMaTruyen.Text;
            string tenTruyen = txbTenTruyen.Text;
            int soTap = Convert.ToInt32(txbSoTap.Text);
            int giaBan = Convert.ToInt32(txbGiaBan.Text);
            DateTime ngayPhatHanh = DateNgayPHTruyen.Value;
            int tonKho = Convert.ToInt32(txbTonKho.Text);

            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Gọi phương thức getConnect() để nhận một đối tượng MySqlConnection
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để thêm dữ liệu vào bảng truyentranh
                string query = "INSERT INTO truyentranh (matruyentranh,tentruyen, sotap, giaban, ngayphathanh, tonkho) " +
                               "VALUES (@matruyentranh,@tentruyen, @sotap, @giaban, @ngayphathanh, @tonkho)";

                // Tạo một đối tượng MySqlCommand và thiết lập các tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@matruyentranh", maTruyen);
                cmd.Parameters.AddWithValue("@tentruyen", tenTruyen);
                cmd.Parameters.AddWithValue("@sotap", soTap);
                cmd.Parameters.AddWithValue("@giaban", giaBan);
                cmd.Parameters.AddWithValue("@ngayphathanh", ngayPhatHanh);
                cmd.Parameters.AddWithValue("@tonkho", tonKho);


                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Đã Thêm Truyện !");
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

        private void btnXoaTruyen_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem có hàng nào được chọn trên DataGridView không
            if (dgvTruyenTranh.SelectedRows.Count > 0)
            {
                string maTruyen = dgvTruyenTranh.SelectedRows[0].Cells["matruyentranh"].Value.ToString();

                connect connectionManager = new connect();
                MySqlConnection connection = connectionManager.getConnect();

                try
                {
                    connection.Open();

                    string query = "DELETE FROM truyentranh WHERE matruyentranh = @MaTruyen";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@MaTruyen", maTruyen);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Truyện đã được xóa khỏi cơ sở dữ liệu.");

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
                MessageBox.Show("Vui lòng chọn một truyện để xóa.");
            }
        }

        private void btnSuaTruyen_Click(object sender, EventArgs e)
        {
            // Lấy thông tin truyện từ các điều khiển nhập liệu
            string maTruyen = txbMaTruyen.Text;
            string tenTruyen = txbTenTruyen.Text;
            int soTap = Convert.ToInt32(txbSoTap.Text);
            decimal giaBan = Convert.ToDecimal(txbGiaBan.Text);
            DateTime ngayPhatHanh = DateNgayPHTruyen.Value;
            int tonKho = Convert.ToInt32(txbTonKho.Text);

            // Tạo một đối tượng connect
            connect connectionManager = new connect();

            // Tạo kết nối
            MySqlConnection connection = connectionManager.getConnect();

            try
            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để cập nhật thông tin truyện
                string query = "UPDATE truyentranh SET tentruyen = @TenTruyen, sotap = @SoTap, giaban = @GiaBan, ngayphathanh = @NgayPhatHanh, tonkho = @TonKho " +
                               "WHERE matruyentranh = @MaTruyen";

                // Tạo một đối tượng MySqlCommand và thiết lập tham số
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@TenTruyen", tenTruyen);
                cmd.Parameters.AddWithValue("@SoTap", soTap);
                cmd.Parameters.AddWithValue("@GiaBan", giaBan);
                cmd.Parameters.AddWithValue("@NgayPhatHanh", ngayPhatHanh);
                cmd.Parameters.AddWithValue("@TonKho", tonKho);
                cmd.Parameters.AddWithValue("@MaTruyen", maTruyen);

                // Thực thi truy vấn
                cmd.ExecuteNonQuery();

                // Thông báo thành công cho người dùng
                MessageBox.Show("Thông tin truyện đã được cập nhật thành công.");

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

        private void hóaĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
            FormHoaDon formHoaDon = new FormHoaDon(isAdmin);
            formHoaDon.ShowDialog();
            this.Close();
        }

        private void quảnLýKháchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormKhachHang formKhachHang = new FormKhachHang(isAdmin);
            formKhachHang.ShowDialog();
            this.Close();
        }

        private void quảnLýNhânViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNhanVien formNhanVien = new FormNhanVien(isAdmin);
            formNhanVien.ShowDialog();
            this.Close();
        }

        private void quảnLýKhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn đang ở trang quản lý kho.");
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
