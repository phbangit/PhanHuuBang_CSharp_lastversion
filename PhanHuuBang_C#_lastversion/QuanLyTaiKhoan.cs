using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PhanHuuBang_C__lastversion
{
    public class QuanLyTaiKhoan
    {
        private string connectionString;

        public QuanLyTaiKhoan(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string KiemTraDangNhap(string taiKhoan, string matKhau)
        {
            string loaiTaiKhoan = "";
            connect connectionManager = new connect();

            // Gọi phương thức getConnect() để nhận một đối tượng MySqlConnection
            MySqlConnection connection = connectionManager.getConnect();
            // Tạo kết nối

            {
                // Mở kết nối
                connection.Open();

                // Tạo câu truy vấn SQL để kiểm tra thông tin đăng nhập
                string query = "SELECT loaitaikhoan FROM nhanvien WHERE taikhoan = @TaiKhoan AND matkhau = @MatKhau";

                // Tạo Command và thi hành truy vấn
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@TaiKhoan", taiKhoan);
                    cmd.Parameters.AddWithValue("@MatKhau", matKhau);

                    // Thực hiện truy vấn và đọc dữ liệu
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Nếu có dòng dữ liệu tức là thông tin đăng nhập hợp lệ
                        if (reader.Read())
                        {
                            loaiTaiKhoan = reader.GetString("loaitaikhoan");
                        }
                    }
                }
            }

            return loaiTaiKhoan;
        }
    }
}
