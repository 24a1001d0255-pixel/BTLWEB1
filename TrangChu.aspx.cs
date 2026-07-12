using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class TrangChu : System.Web.UI.Page
    {
        private static readonly Dictionary<string, string[]> CategoryFilters = new Dictionary<string, string[]>
        {
            { "Thieu-nhi",  new[] { "Thieu-nhi" } },
            { "Van-hoc",     new[] { "Van-hoc" } },
            { "Giao-khoa",  new[] { "Giao-khoa" } },
            { "Ky-nang",    new[] { "Ky-nang" } },
        };
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var ds = Application["DanhSachSach"] as List<Sach>;
                if (ds == null || ds.Count == 0) return;

                var random = new Random();
                var heroBooks = ds.OrderBy(x => random.Next()).Take(3).ToList();
                rptHeroBooks.DataSource = heroBooks;
                rptHeroBooks.DataBind();

                BindSection(ds, rptSachNoiBat, null, 4);
                BindCategory(ds, "Thieu-nhi",  rptThieuNhi,  pnlThieuNhiEmpty);
                BindCategory(ds, "Van-hoc",     rptTieuThuyet, pnlTieuThuyetEmpty);
                BindCategory(ds, "Giao-khoa", rptGiaoKhoa,  pnlGiaoKhoaEmpty);
                BindCategory(ds, "Ky-nang",    rptSelfCare,  pnlSelfCareEmpty);
            }
        }

        private void BindSection(List<Sach> ds, Repeater rep, string[] keywords, int count)
        {
            IEnumerable<Sach> books = ds;
            if (keywords != null && keywords.Length > 0)
            {
                books = ds.Where(s => keywords.Any(k =>
                    string.Equals(s.TheLoai, k, StringComparison.OrdinalIgnoreCase)));
            }
            var result = books.Take(count).ToList();
            rep.DataSource = result;
            rep.DataBind();
        }

        private void BindCategory(List<Sach> ds, string key, Repeater rep, Panel emptyPanel)
        {
            if (!CategoryFilters.ContainsKey(key))
            {
                emptyPanel.Visible = true;
                return;
            }

            var keywords = CategoryFilters[key];
            var result = ds
                .Where(s => keywords.Any(k =>
                    string.Equals(s.TheLoai, k, StringComparison.OrdinalIgnoreCase) ||
                    (s.TheLoai ?? "").Contains(k)))
                .Take(4)
                .ToList();

            if (result.Count == 0)
            {
                emptyPanel.Visible = true;
                rep.Visible = false;
            }
            else
            {
                emptyPanel.Visible = false;
                rep.Visible = true;
                rep.DataSource = result;
                rep.DataBind();
            }
        }

        protected void btnThemVaoGio_Command(object sender, CommandEventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("DangNhap.aspx");
                return;
            }

            string maSach = e.CommandArgument.ToString();
            var dsSanPham = Application["DanhSachSach"] as List<Sach>;
            var sach = dsSanPham?.FirstOrDefault(sp => sp.MaSach == maSach);
            if (sach == null) return;

            var gioHang = Session["giohang"] as List<GioHangItem> ?? new List<GioHangItem>();
            var existing = gioHang.FirstOrDefault(x => x.MaSach == maSach);
            if (existing != null)
                existing.SoLuong++;
            else
                gioHang.Add(new GioHangItem
                {
                    MaSach = sach.MaSach,
                    TenSach = sach.TenSach,
                    Gia = sach.Gia,
                    Anh = sach.Anh,
                    SoLuong = 1
                });

            Session["giohang"] = gioHang;
            (this.Master as Site)?.UpdateCartCountFromSession();
        }
    }
}
