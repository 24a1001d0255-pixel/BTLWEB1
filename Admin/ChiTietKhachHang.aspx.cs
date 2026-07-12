using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class ChiTietKhachHang : Page
    {
        private string _customerEmail;
        private Account _customer;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Kiem tra quyen admin
            var acc = Session["user"] as Account;
            if (acc == null || !string.Equals(acc.Email, "admin@bookverse.com", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("../DangNhap.aspx");
                return;
            }

            _customerEmail = Request.QueryString["email"] ?? "";
            if (string.IsNullOrWhiteSpace(_customerEmail))
            {
                Response.Redirect("QuanLy.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCustomerData();
            }
        }

        private void LoadCustomerData()
        {
            // Lay thong tin khach hang
            var accounts = DataService.GetTaiKhoan();
            _customer = accounts.FirstOrDefault(a =>
                string.Equals(a.Email, _customerEmail, StringComparison.OrdinalIgnoreCase));

            if (_customer == null)
            {
                Response.Redirect("QuanLy.aspx");
                return;
            }

            // Thong tin ca nhan
            lblHoTen.InnerText = string.IsNullOrWhiteSpace(_customer.TenDangNhap) ? "-" : _customer.TenDangNhap;
            lblEmail.InnerText = string.IsNullOrWhiteSpace(_customer.Email) ? "-" : _customer.Email;
            lblSoDienThoai.InnerText = string.IsNullOrWhiteSpace(_customer.SoDienThoai) ? "-" : _customer.SoDienThoai;
            lblGioiTinh.InnerText = "-";
            lblNgaySinh.InnerText = "-";

            // Thong tin tai khoan
            bool isActive = string.Equals(_customer.TrangThai, "HoatDong", StringComparison.OrdinalIgnoreCase);
            lblTrangThai.InnerText = isActive ? "Hoạt động" : "Vô hiệu hóa";
            lblTrangThai.Attributes["class"] = isActive ? "badge badge-ok" : "badge badge-danger";

            lblNgayDangKy.InnerText = "-";
            lblLanDangNhapCuoi.InnerText = "-";

            // Gan hang thanh vien
            var orders = DataService.GetDonHang();
            var customerOrders = orders.Where(o =>
                string.Equals(o.Email, _customerEmail, StringComparison.OrdinalIgnoreCase)).ToList();
            double totalSpent = customerOrders.Sum(o => o.TongTien);
            lblHangThanhVien.InnerText = GetMembershipLevel(totalSpent);

            // Dia chi
            if (!string.IsNullOrWhiteSpace(_customer.DiaChi))
            {
                lblDiaChiMacDinh.InnerText = _customer.DiaChi;
            }

            // Thong ke mua hang
            lblTongDonHang.InnerText = customerOrders.Count.ToString();
            lblTongChiTieu.InnerText = totalSpent.ToString("N0") + "đ";
            lblDonThanhCong.InnerText = customerOrders.Count(o =>
                string.Equals(o.TrangThaiCode, "dagiao", StringComparison.OrdinalIgnoreCase)).ToString();
            lblDonHuy.InnerText = customerOrders.Count(o =>
                string.Equals(o.TrangThaiCode, "dahuy", StringComparison.OrdinalIgnoreCase)).ToString();

            // Lich su don hang
            var recentOrders = customerOrders.OrderByDescending(o => o.NgayDat).Take(10).ToList();
            if (recentOrders.Count > 0)
            {
                rptDonHang.DataSource = recentOrders;
                rptDonHang.DataBind();
                pnlNoOrders.Visible = false;
            }
            else
            {
                rptDonHang.DataSource = null;
                rptDonHang.DataBind();
                pnlNoOrders.Visible = true;
            }

            // Nut thao tac
            btnKhoaTaiKhoan.Visible = isActive;
            btnMoKhoaTaiKhoan.Visible = !isActive;
        }

        private string GetMembershipLevel(double totalSpent)
        {
            if (totalSpent >= 5000000) return "Kim Cương";
            if (totalSpent >= 2000000) return "Vàng";
            if (totalSpent >= 500000) return "Bạc";
            return "Khách hàng mới";
        }

        public string GetStatusClass(string code)
        {
            if (string.Equals(code, "dagiao", StringComparison.OrdinalIgnoreCase)) return "status-dagiao";
            if (string.Equals(code, "dahuy", StringComparison.OrdinalIgnoreCase)) return "status-dahuy";
            if (string.Equals(code, "danggiao", StringComparison.OrdinalIgnoreCase)) return "status-danggiao";
            return "status-dangxuly";
        }

        protected void btnKhoaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (_customer == null) return;

            var accounts = DataService.GetTaiKhoan();
            var acc = accounts.FirstOrDefault(a =>
                string.Equals(a.Email, _customerEmail, StringComparison.OrdinalIgnoreCase));

            if (acc != null)
            {
                acc.TrangThai = "VoHieuHoa";
                DataService.SaveTaiKhoan(accounts);
                Application["DanhSachTaiKhoan"] = accounts;

                ShowSuccessMessage("Đã khóa tài khoản thành công.");
                LoadCustomerData();
            }
        }

        protected void btnMoKhoaTaiKhoan_Click(object sender, EventArgs e)
        {
            if (_customer == null) return;

            var accounts = DataService.GetTaiKhoan();
            var acc = accounts.FirstOrDefault(a =>
                string.Equals(a.Email, _customerEmail, StringComparison.OrdinalIgnoreCase));

            if (acc != null)
            {
                acc.TrangThai = "HoatDong";
                DataService.SaveTaiKhoan(accounts);
                Application["DanhSachTaiKhoan"] = accounts;

                ShowSuccessMessage("Đã mở khóa tài khoản thành công.");
                LoadCustomerData();
            }
        }

        protected void btnDatLaiMatKhau_Click(object sender, EventArgs e)
        {
            if (_customer == null) return;

            // Trong thuc te se gui email, o day chi hien thong bao
            ShowSuccessMessage($"Đã gửi yêu cầu đặt lại mật khẩu đến {_customerEmail}. Email chứa link tạo mật khẩu mới sẽ được gửi trong vài phút.");
        }

        private void ShowSuccessMessage(string message)
        {
            litActionMessage.Text = message;
            pnlActionMessage.Visible = true;
        }
    }
}
