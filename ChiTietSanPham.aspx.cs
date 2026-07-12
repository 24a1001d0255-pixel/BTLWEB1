using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class ChiTietSanPham : Page
    {
        private static readonly Random Rnd = new Random();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string maSach = Request.QueryString["id"] ?? "";
                if (string.IsNullOrWhiteSpace(maSach))
                {
                    Response.Redirect("~/TrangChu.aspx");
                    return;
                }

                var ds = Application["DanhSachSach"] as List<Sach>;
                var sach = ds?.Find(s =>
                    string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase));

                if (sach == null)
                {
                    Response.Redirect("~/TrangChu.aspx");
                    return;
                }

                BindProductDetails(sach, ds);
            }

            pnlInlineMessage.Visible = false;
        }

        private void BindProductDetails(Sach sach, List<Sach> allBooks)
        {
            imgBiaSach.ImageUrl = ResolveServerUrl(sach.Anh, "default.jpg");
            lblTenSach.Text = sach.TenSach;
            lblBreadcrumbTen.Text = sach.TenSach;
            lblTacGia.Text = sach.TacGia ?? "-";
            lblNhaXuatBan.Text = sach.NhaXuatBan ?? "-";
            lblSoTrang.Text = sach.SoTrang > 0 ? sach.SoTrang + " trang" : "-";
            lblTheLoai.Text = sach.TheLoai ?? "-";
            lblTinhTrang.Text = sach.TinhTrangKho ?? "-";
            lblGia.Text = sach.Gia.ToString("N0") + " VND";

            litMoTa.Text = string.IsNullOrWhiteSpace(sach.MoTa)
                ? "Chưa có mô tả cho cuốn sách này."
                : sach.MoTa.Replace("\r\n", "<br/>").Replace("\n", "<br/>");

            string loaiUrl = Server.UrlEncode(MakeUrlSafe(sach.TheLoai ?? ""));
            lnkBreadcrumbLoai.Text = sach.TheLoai ?? "-";
            lnkBreadcrumbLoai.NavigateUrl = "DanhSachSp.aspx?type=theloai&value=" + loaiUrl;

            BindThumbnails(sach);
            BindRelatedProducts(sach, allBooks);
            BindReviews(sach.TenSach ?? "cuon sach");
        }

        private void BindThumbnails(Sach sach)
        {
            var thumbs = new List<ThumbItem>();
            thumbs.Add(new ThumbItem { Url = ResolveClientUrl(sach.Anh, "default.jpg"), Alt = "Bia chinh" });

            if (!string.IsNullOrWhiteSpace(sach.AnhMinhHoa))
            {
                var parts = sach.AnhMinhHoa.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < parts.Length && thumbs.Count < 5; i++)
                {
                    string p = parts[i].Trim();
                    if (!string.IsNullOrEmpty(p))
                    {
                        thumbs.Add(new ThumbItem { Url = ResolveClientUrl(p, "default.jpg"), Alt = "Anh " + (i + 2) });
                    }
                }
            }

            while (thumbs.Count < 3)
            {
                thumbs.Add(new ThumbItem { Url = ResolveClientUrl(sach.Anh, "default.jpg"), Alt = "Anh " + (thumbs.Count + 1) });
            }

            rptThumbnails.DataSource = thumbs;
            rptThumbnails.DataBind();
            thumbnailStrip.Visible = thumbs.Count > 1;
        }

        private void BindRelatedProducts(Sach sach, List<Sach> allBooks)
        {
            if (allBooks == null || sach == null)
            {
                pnlRelatedEmpty.Visible = true;
                rptSanPhamLienQuan.Visible = false;
                return;
            }

            var related = allBooks
                .Where(b => !string.Equals(b.MaSach, sach.MaSach, StringComparison.OrdinalIgnoreCase))
                .Where(b => string.Equals(b.TheLoai, sach.TheLoai, StringComparison.OrdinalIgnoreCase) ||
                            (sach.TheLoai ?? "").Contains(b.TheLoai ?? ""))
                .OrderByDescending(b => b.Gia)
                .Take(8)
                .ToList();

            if (related.Count == 0)
            {
                related = allBooks
                    .Where(b => !string.Equals(b.MaSach, sach.MaSach, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(b => Rnd.Next())
                    .Take(8)
                    .ToList();
            }

            if (related.Count == 0)
            {
                pnlRelatedEmpty.Visible = true;
                rptSanPhamLienQuan.Visible = false;
            }
            else
            {
                pnlRelatedEmpty.Visible = false;
                rptSanPhamLienQuan.DataSource = related;
                rptSanPhamLienQuan.DataBind();
            }
        }

        private void BindReviews(string tenSach)
        {
            var reviews = GetSampleReviews(tenSach).ToList();
            if (reviews.Count == 0) return;

            double avg = Math.Round(reviews.Average(x => x.SoSao), 1);
            lblTrungBinh.Text = avg.ToString("0.0");
            lblSoLuongDanhGia.Text = "Dựa trên " + reviews.Count + " đánh giá";

            litStars.Text = BuildStarsHtml(avg);

            rptReviews.DataSource = reviews;
            rptReviews.DataBind();
        }

        private string BuildStarsHtml(double rating)
        {
            var sb = new System.Text.StringBuilder();
            int full = (int)Math.Floor(rating);
            bool half = rating - full >= 0.5;

            for (int i = 0; i < 5; i++)
            {
                string cls;
                if (i < full)
                    cls = "star star-filled";
                else if (i == full && half)
                    cls = "star star-half";
                else
                    cls = "star star-empty";

                string star = i < full ? "&#9733;" : "&#9734;";
                sb.Append("<span class=\"");
                sb.Append(cls);
                sb.Append("\">");
                sb.Append(star);
                sb.Append("</span>");
            }

            return sb.ToString();
        }

        private string ResolveServerUrl(string rawPath, string fallback)
        {
            string path = (rawPath ?? "").Trim();
            if (string.IsNullOrEmpty(path))
                return ResolveUrl("~/images/" + fallback);
            if (path.StartsWith("~/") || path.StartsWith("/"))
                return ResolveUrl(path);
            if (path.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
                return ResolveUrl("~/" + path);
            return ResolveUrl("~/images/" + path);
        }

        private string ResolveClientUrl(string rawPath, string fallback)
        {
            string path = (rawPath ?? "").Trim();
            if (string.IsNullOrEmpty(path))
                return ResolveUrl("images/" + fallback);
            if (path.StartsWith("/"))
                return path;
            if (path.StartsWith("images/", StringComparison.OrdinalIgnoreCase))
                return path;
            return "images/" + path;
        }

        private string MakeUrlSafe(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return "";
            return text.Replace(" ", "-")
                       .Replace("đ", "d").Replace("Đ", "D")
                       .Replace("á", "a").Replace("à", "a").Replace("ả", "a").Replace("ã", "a")
                       .Replace("é", "e").Replace("è", "e").Replace("ẻ", "e").Replace("ẽ", "e")
                       .Replace("ê", "e")
                       .Replace("í", "i").Replace("ì", "i").Replace("ỉ", "i").Replace("ĩ", "i")
                       .Replace("ó", "o").Replace("ò", "o").Replace("ỏ", "o").Replace("õ", "o")
                       .Replace("ú", "u").Replace("ù", "u").Replace("ủ", "u").Replace("ũ", "u")
                       .Replace("ý", "y").Replace("ỳ", "y").Replace("ỷ", "y").Replace("ỹ", "y");
        }

        protected void btnThemVaoGio_Click(object sender, EventArgs e)
        {
            AddToCart(Request.QueryString["id"]);
            pnlInlineMessage.Visible = true;
        }

        protected void btnThemLienQuan_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            string maSach = e.CommandArgument != null ? e.CommandArgument.ToString() : "";
            AddToCart(maSach);
        }

        private void AddToCart(string maSach)
        {
            if (string.IsNullOrWhiteSpace(maSach)) return;

            var ds = Application["DanhSachSach"] as List<Sach>;
            var sach = ds != null
                ? ds.Find(s => string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase))
                : null;

            if (sach == null) return;

            var gioHang = Session["giohang"] as List<GioHangItem>;
            if (gioHang == null) gioHang = new List<GioHangItem>();

            var existing = gioHang.Find(x =>
                string.Equals(x.MaSach, sach.MaSach, StringComparison.OrdinalIgnoreCase));

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
            var m = this.Master as Site;
            if (m != null) m.UpdateCartCountFromSession();
        }

        private IEnumerable<ReviewItem> GetSampleReviews(string tenSach)
        {
            string ts = string.IsNullOrWhiteSpace(tenSach) ? "cuốn sách" : tenSach;

            string[] reviewers = { "Minh Anh", "Hoàng Nam", "Thu Hà", "Đức Anh", "Linh Đan" };
            string[] contents = {
                "Bìa đẹp, nội dung hay, mình đọc " + ts + " trong hai ngày và cảm thấy rất đáng tiền.",
                "Sách " + ts + " giao nhanh, đóng gói cẩn thận. Tuy nhiên mình mong lần sau sửa một số lỗi in.",
                "Nội dung sâu sắc, cách viết dễ hiểu, phù hợp cho người mới bắt đầu đọc " + ts + ".",
                "Đây là một trong những cuốn " + ts + " mà mình muốn giữ lại treo kẻ sách lâu dài.",
                "Cuốn " + ts + " phù hợp đọc cuối tuần, nhẹ nhàng mà vẫn truyền tải được thông điệp tốt."
            };

            DateTime now = DateTime.Now;
            for (int i = 0; i < reviewers.Length; i++)
            {
                int stars = 4 + Rnd.Next(0, 2);
                string starsHtml = BuildStarsHtml(stars);

                yield return new ReviewItem
                {
                    TenNguoiDung = reviewers[i],
                    Initial = reviewers[i].Substring(0, 1).ToUpper(),
                    SoSao = stars,
                    Stars = starsHtml,
                    NoiDung = contents[i % contents.Length],
                    NgayDanhGia = now.AddDays(-i - 1).ToString("dd MMM, yyyy")
                };
            }
        }

        private class ThumbItem
        {
            public string Url { get; set; }
            public string Alt { get; set; }
        }

        private class ReviewItem
        {
            public string TenNguoiDung { get; set; }
            public string Initial { get; set; }
            public int SoSao { get; set; }
            public string Stars { get; set; }
            public string NoiDung { get; set; }
            public string NgayDanhGia { get; set; }
        }
    }
}
