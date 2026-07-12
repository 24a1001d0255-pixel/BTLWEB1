using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class TaiKhoanCaNhan : System.Web.UI.Page
    {
        private const string SessionUser = "user";
        private const string SessionOrders = "TaiKhoanDonHang";

        protected void Page_Load(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                EnsureDemoOrders(acc);
                HienThiThongTin(acc);
                HienThiDonHang(acc.Email);
            }
        }

        private void EnsureDemoOrders(Account acc)
        {
            var appOrders = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (appOrders == null)
            {
                appOrders = new List<DonHangItem>();
                Application["DanhSachDonHang"] = appOrders;
            }

            string email = (acc.Email ?? string.Empty).Trim().ToLowerInvariant();

            if (!appOrders.Any(o => string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase)))
            {
                var now = DateTime.Now;
                var demo = new List<DonHangItem>
                {
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "001",
                        Email = email,
                        KhachHang = acc.TenDangNhap,
                        NgayDat = now.AddDays(-5).ToString("dd/MM/yyyy"),
                        TongTien = 285000,
                        PhuongThuc = "COD",
                        TrangThai = "Dang giao hang",
                        TrangThaiCode = "danggiao",
                        ChiTiet = new List<GioHangItem>
                        {
                            new GioHangItem { MaSach = "S001", TenSach = "Đắc Nhân Tâm", Gia = 95000, SoLuong = 3 }
                        }
                    },
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "002",
                        Email = email,
                        KhachHang = acc.TenDangNhap,
                        NgayDat = now.AddDays(-12).ToString("dd/MM/yyyy"),
                        TongTien = 198000,
                        PhuongThuc = "Chuyển khoản ngân hàng",
                        TrangThai = "Da giao thanh cong",
                        TrangThaiCode = "dagiao",
                        ChiTiet = new List<GioHangItem>
                        {
                            new GioHangItem { MaSach = "S002", TenSach = "Nhà Giả Kim", Gia = 99000, SoLuong = 2 }
                        }
                    },
                    new DonHangItem
                    {
                        MaDon = "BV" + now.ToString("yyyyMMdd") + "003",
                        Email = email,
                        KhachHang = acc.TenDangNhap,
                        NgayDat = now.AddDays(-20).ToString("dd/MM/yyyy"),
                        TongTien = 540000,
                        PhuongThuc = "Ví điện tử",
                        TrangThai = "Dang xu ly",
                        TrangThaiCode = "dangxuly",
                        ChiTiet = new List<GioHangItem>
                        {
                            new GioHangItem { MaSach = "S003", TenSach = "Tuesdays with Morrie", Gia = 180000, SoLuong = 3 }
                        }
                    }
                };

                appOrders.AddRange(demo);
            }
        }

        private void HienThiThongTin(Account acc)
        {
            // Avatar chu cai dau
            string hoTen = acc.TenDangNhap ?? "";
            lblAvatarChu.Text = string.IsNullOrWhiteSpace(hoTen) ? "?" : hoTen.Substring(0, 1).ToUpper();

            lblHoTen.Text = string.IsNullOrWhiteSpace(acc.TenDangNhap) ? "Khách hàng" : acc.TenDangNhap;
            lblEmail.Text = string.IsNullOrWhiteSpace(acc.Email) ? "—" : acc.Email;
            lblSoDienThoai.Text = string.IsNullOrWhiteSpace(acc.SoDienThoai) ? "—" : acc.SoDienThoai;
            lblDiaChi.Text = string.IsNullOrWhiteSpace(acc.DiaChi) ? "—" : acc.DiaChi;
        }

        protected void btnCapNhat_Click(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                return;
            }

            txtHoTen.Text = acc.TenDangNhap ?? string.Empty;
            txtEmail.Text = acc.Email ?? string.Empty;
            txtSoDienThoai.Text = acc.SoDienThoai ?? string.Empty;
            txtDiaChi.Text = acc.DiaChi ?? string.Empty;

            editSection.Visible = true;
            orderSection.Visible = false;
            btnCapNhat.Visible = false;
        }

        protected void btnLuu_Click(object sender, EventArgs e)
        {
            var acc = Session[SessionUser] as Account;
            if (acc == null)
            {
                return;
            }

            string hoTen = txtHoTen.Text?.Trim() ?? string.Empty;
            string email = txtEmail.Text?.Trim() ?? string.Empty;
            string soDienThoai = txtSoDienThoai.Text?.Trim() ?? string.Empty;
            string diaChi = txtDiaChi.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(soDienThoai) && !Regex.IsMatch(soDienThoai, @"^0\d{8,10}$"))
            {
                return;
            }

            // Cap nhat thong tin
            acc.TenDangNhap = string.IsNullOrWhiteSpace(hoTen) ? acc.TenDangNhap : hoTen;
            acc.Email = string.IsNullOrWhiteSpace(email) ? acc.Email : email;
            acc.SoDienThoai = soDienThoai;
            acc.DiaChi = diaChi;

            // Cap nhat vao danh sach tai khoan
            var accounts = DataService.GetTaiKhoan();
            var existing = accounts.FirstOrDefault(a =>
                string.Equals(a.Email, acc.Email, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
            {
                existing.TenDangNhap = acc.TenDangNhap;
                existing.Email = acc.Email;
                existing.SoDienThoai = acc.SoDienThoai;
                existing.DiaChi = acc.DiaChi;
                DataService.SaveTaiKhoan(accounts);
            }

            Session[SessionUser] = acc;
            Application["DanhSachTaiKhoan"] = accounts;

            HienThiThongTin(acc);

            // An form, hien lai lich su don hang
            editSection.Visible = false;
            orderSection.Visible = true;
            btnCapNhat.Visible = true;

            // Thong bao thanh cong bang script
            ScriptManager.RegisterStartupScript(this, GetType(), "successAlert",
                "alert('Cập nhật thông tin thành công!');", true);
        }

        protected void btnHuy_Click(object sender, EventArgs e)
        {
            editSection.Visible = false;
            orderSection.Visible = true;
            btnCapNhat.Visible = true;
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/TrangChu.aspx");
        }

        private void HienThiDonHang(string email)
        {
            var appOrders = Application["DanhSachDonHang"] as List<DonHangItem>;
            
            var ds = appOrders?.Where(o => string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase)).ToList();

            if (ds != null && ds.Count > 0)
            {
                pnlEmptyOrders.Visible = false;
                rptDonHang.DataSource = ds.OrderByDescending(x => x.NgayDat).ToList();
                rptDonHang.DataBind();
            }
            else
            {
                pnlEmptyOrders.Visible = true;
                rptDonHang.DataSource = null;
                rptDonHang.DataBind();
            }
        }
    }
}
