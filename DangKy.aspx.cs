using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class DangKy : Page
    {
        private const string SessionUser = "user";
        private const string AppAccounts = "DanhSachTaiKhoan";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnDangKy_Click(object sender, EventArgs e)
        {
            // Lay du lieu tu form
            string hoTen = txtHoTen.Text?.Trim() ?? string.Empty;
            string email = txtEmail.Text?.Trim() ?? string.Empty;
            string soDienThoai = txtSoDienThoai.Text?.Trim() ?? string.Empty;
            string matKhau = Request.Form[txtMatKhau.UniqueID] ?? string.Empty;
            string nhapLai = Request.Form[txtNhapLaiMatKhau.UniqueID] ?? string.Empty;

            // Khoi tao danh sach loi
            var errors = new List<string>();

            // 1. Validate HoTen
            if (string.IsNullOrWhiteSpace(hoTen))
            {
                errors.Add("Vui lòng nhập họ tên.");
            }
            else if (hoTen.Length < 2)
            {
                errors.Add("Họ tên phải từ 2 ký tự trở lên.");
            }

            // 2. Validate Email (bat buoc)
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Vui lòng nhập email.");
            }
            else if (!IsValidEmail(email))
            {
                errors.Add("Email không hợp lệ.");
            }

            // 3. Validate SoDienThoai (bat buoc)
            if (string.IsNullOrWhiteSpace(soDienThoai))
            {
                errors.Add("Vui lòng nhập số điện thoại.");
            }
            else if (!IsValidPhoneNumber(soDienThoai))
            {
                errors.Add("Số điện thoại phải là 10-11 số, bắt đầu bằng số 0.");
            }

            // 4. Validate MatKhau (bat buoc)
            if (string.IsNullOrWhiteSpace(matKhau))
            {
                errors.Add("Vui lòng nhập mật khẩu.");
            }
            else if (matKhau.Length < 6)
            {
                errors.Add("Mật khẩu phải từ 6 ký tự trở lên.");
            }

            // 5. Validate NhapLai MatKhau
            if (string.IsNullOrWhiteSpace(nhapLai))
            {
                errors.Add("Vui lòng nhập lại mật khẩu.");
            }
            else if (matKhau != nhapLai)
            {
                errors.Add("Mật khẩu nhập lại không trùng khớp.");
            }

            // Neu co loi, hien thi va dung lai
            if (errors.Count > 0)
            {
                HienThiLoi(string.Join(" ", errors));
                return;
            }

            // Kiem tra email da ton tai chua
            var accounts = GetAccounts();
            if (accounts.Any(a => string.Equals(a.Email, email, StringComparison.OrdinalIgnoreCase)))
            {
                HienThiLoi("Email này đã được đăng ký. Vui lòng sử dụng email khác hoặc đăng nhập.");
                return;
            }

            // Tao tai khoan moi
            var newAccount = new Account
            {
                TenDangNhap = hoTen,
                Email = email,
                SoDienThoai = soDienThoai,
                MatKhau = matKhau,
                DiaChi = string.Empty,
                VaiTro = "User",
                TrangThai = "HoatDong"
            };

            // Luu vao danh sach
            accounts.Add(newAccount);
            Application[AppAccounts] = accounts;

            // LUU VAO FILE JSON (fix bug: phai goi SaveTaiKhoan de du lieu khong bi mat khi app restart)
            DataService.SaveTaiKhoan(accounts);

            // Luu session va chuyen huong
            Session[SessionUser] = newAccount;
            Response.Redirect("~/TrangChu.aspx");
        }

        private List<Account> GetAccounts()
        {
            // Uu tien lay tu Application, neu khong co thi lay tu DataService
            var existing = Application[AppAccounts] as List<Account>;
            if (existing != null && existing.Count > 0)
            {
                return existing;
            }

            // Lay tu DataService (file JSON)
            var fromFile = DataService.GetTaiKhoan();
            if (fromFile != null && fromFile.Count > 0)
            {
                Application[AppAccounts] = fromFile;
                return fromFile;
            }

            // Tao admin mac dinh
            var defaults = new List<Account>
            {
                new Account
                {
                    TenDangNhap = "admin",
                    Email = "admin@bookverse.com",
                    MatKhau = "123456",
                    SoDienThoai = "",
                    DiaChi = "",
                    VaiTro = "Admin",
                    TrangThai = "HoatDong"
                }
            };

            Application[AppAccounts] = defaults;
            return defaults;
        }

        private void HienThiLoi(string message)
        {
            litThongBao.Text = message;
            divThongBao.Visible = true;
            divThongBao.Style["display"] = "block";
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // So dien thoai VN: 9-11 so, bat dau bang 0
            if (string.IsNullOrWhiteSpace(phone)) return false;
            return Regex.IsMatch(phone, @"^0\d{8,10}$");
        }
    }
}
