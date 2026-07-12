using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var acc = Session["user"] as Account;
                bool isLogged = acc != null;
                lnkDangNhap.Visible = !isLogged;
                lnkTaiKhoan.Visible = isLogged;
                pnlUserArea.Visible = isLogged;

                if (isLogged)
                {
                    lblUserName.Text = acc.TenDangNhap ?? string.Empty;
                }

                bool isAdmin = acc != null && string.Equals(acc.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase);
                menuAdmin.Visible = isAdmin;
            }

            UpdateCartCountFromSession();
            RenderBreadcrumb();
        }

        private void RenderBreadcrumb()
        {
            string page = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            string html = null;

            if (page.Equals("TrangChu.aspx", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }
            else if (page.Equals("DanhSachSp.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Danh mục sách", null) });
            }
            else if (page.Equals("ChiTietSanPham.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] {
                    new Crumb("Danh mục sách", "DanhSachSp.aspx"),
                    new Crumb("Chi tiết sản phẩm", null)
                });
            }
            else if (page.Equals("GioHang.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Giỏ hàng", null) });
            }
            else if (page.Equals("ThanhToan.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] {
                    new Crumb("Giỏ hàng", "GioHang.aspx"),
                    new Crumb("Thanh toán", null)
                });
            }
            else if (page.Equals("DangNhap.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Đăng nhập", null) });
            }
            else if (page.Equals("DangKy.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Đăng ký", null) });
            }
            else if (page.Equals("TaiKhoanCaNhan.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Tài khoản", null) });
            }
            else if (page.Equals("DanhMuc.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Danh mục", null) });
            }
            else if (page.Equals("DieuKhoan.aspx", StringComparison.OrdinalIgnoreCase))
            {
                html = BuildBreadcrumb(new[] { new Crumb("Điều khoản", null) });
            }

            if (html != null)
                litBreadcrumb.Text = html;
        }

        private string BuildBreadcrumb(Crumb[] crumbs)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("<nav class=\"breadcrumb-nav\" aria-label=\"Breadcrumb\">");
            sb.Append("<div class=\"container\">");
            sb.Append("<a href=\"TrangChu.aspx\">Trang chủ</a>");

            foreach (var crumb in crumbs)
            {
                sb.Append("<span class=\"breadcrumb-sep\" aria-hidden=\"true\">&rsaquo;</span>");
                if (crumb.Url == null)
                    sb.Append("<span class=\"breadcrumb-current\">" + crumb.Text + "</span>");
                else
                    sb.Append("<a href=\"" + crumb.Url + "\">" + crumb.Text + "</a>");
            }

            sb.Append("</div></nav>");
            return sb.ToString();
        }

        private class Crumb
        {
            public string Text { get; }
            public string Url { get; }

            public Crumb(string text, string url)
            {
                Text = text;
                Url = url;
            }
        }

        public void UpdateCartCountFromSession()
        {
            var cart = Session["giohang"] as List<GioHangItem>;
            int count = cart == null ? 0 : cart.Sum(x => x.SoLuong);
            lblCartCount.Text = count.ToString();
        }

        protected void btnTimKiem_Click(object sender, EventArgs e)
        {
            var q = txtTimKiem.Text == null ? null : txtTimKiem.Text.Trim();
            if (string.IsNullOrWhiteSpace(q))
            {
                Response.Redirect("~/TrangChu.aspx");
                return;
            }

            Response.Redirect("~/DanhSachSp.aspx?q=" + Server.UrlEncode(q));
        }

        protected void btnDangXuat_Click(object sender, EventArgs e)
        {
            Session["user"] = null;
            Response.Redirect("~/TrangChu.aspx");
        }
    }
}
