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

    public partial class FormDangNhap : Form
    {
        private string connectionString;
        public bool isAdmin = false;
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {

            string taiKhoan = txbTaiKhoan.Text;
            string matKhau = txbMatKhau.Text;

            // Tạo đối tượng QuanLyTaiKhoan
            QuanLyTaiKhoan quanLyTaiKhoan = new QuanLyTaiKhoan(connectionString);
            string loaiTaiKhoan = quanLyTaiKhoan.KiemTraDangNhap(taiKhoan, matKhau);

            // Phân quyền dựa trên loại tài khoản
            switch (loaiTaiKhoan)
            {
                case "Quản Trị":
                    // Thực hiện hành động cho tài khoản ad
                    isAdmin = true;
                    MessageBox.Show("Bạn đã đăng nhập với quyền Quản Trị !", "Thông báo !");
                    FormMain formMain = new FormMain(isAdmin);
                    formMain.ShowDialog();
                    this.Close();
                    break;
                case "Nhân Viên":
                    // Thực hiện hành động cho tài khoản nv

                    MessageBox.Show("Bạn đã đăng nhập với quyền của Nhân Viên!", "Thông báo !");
                    FormMain f = new FormMain(false);
                    f.ShowDialog();
                    this.Close();
                    break;
                default:
                    // saitkmk

                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng !", "Thử lại !");
                    break;
            }

        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát khỏi ứng dụng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
