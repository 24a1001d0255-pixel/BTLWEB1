using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class DangNhap : System.Web.UI.Page
    {
        private const string SessionUser = "user";
        private const string AppAccounts = "DanhSachTaiKhoan";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnDangNhap_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text?.Trim() ?? string.Empty;
            string matKhau = Request.Form[txtMatKhau.UniqueID] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(matKhau))
            {
                return;
            }

            if (!IsValidEmail(email))
            {
                return;
            }

            var accounts = GetAccounts();
            var account = accounts.FirstOrDefault(a =>
                string.Equals(a.Email, email, StringComparison.OrdinalIgnoreCase));

            if (account == null)
            {
                return;
            }

            Session[SessionUser] = account;
            Response.Redirect("~/TrangChu.aspx");
        }

        private bool IsValidEmail(string email)
        {
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

        private List<Account> GetAccounts()
        {
            var existing = Application[AppAccounts] as List<Account>;
            if (existing != null && existing.Count > 0)
            {
                return existing;
            }

            var defaults = new List<Account>
            {
                new Account { TenDangNhap = "Admin BookVerse", Email = "admin@bookverse.vn", MatKhau = "123456", SoDienThoai = "09091234567", VaiTro = "Admin" },
                new Account { TenDangNhap = "Nguyễn Văn A", Email = "nguyenvana@email.com", MatKhau = "123456", SoDienThoai = "09123456789", VaiTro = "User" }
            };

            Application[AppAccounts] = defaults;
            return defaults;
        }
    }
}
