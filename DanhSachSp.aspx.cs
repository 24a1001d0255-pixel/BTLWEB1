using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class DanhSachSp : Page
    {
        private const int PageSize = 9;
        private int _currentPage = 1;
        private int TotalPages => (int)Math.Ceiling(_totalCount / (double)PageSize);
        private int _totalCount;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _currentPage = GetPageFromQuery();
                LoadFilterOptions();
                ApplyFilters();
            }
        }

        private int GetPageFromQuery()
        {
            int page = 1;
            int.TryParse(Request.QueryString["page"], out page);
            return page < 1 ? 1 : page;
        }

        private void LoadFilterOptions()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null) return;

            var theLoais = ds
                .Where(s => !string.IsNullOrWhiteSpace(s.TheLoai))
                .Select(s => s.TheLoai)
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            var items = theLoais.Select(tl => new ListItem(
                $"{tl} ({ds.Count(s => string.Equals(s.TheLoai, tl, StringComparison.OrdinalIgnoreCase))})",
                tl
            )).ToArray();

            cblTheLoai.Items.AddRange(items);

            // Restore state from query string
            RestoreCheckboxState(cblTheLoai, Request.QueryString["theloai"] ?? "");
            RestoreCheckboxState(cblGia, Request.QueryString["gia"] ?? "");
            RestoreCheckboxState(cblKho, Request.QueryString["kho"] ?? "");
        }

        private void RestoreCheckboxState(CheckBoxList cbl, string values)
        {
            if (string.IsNullOrWhiteSpace(values)) return;
            var arr = values.Split(',').Select(x => x.Trim().ToLower()).ToList();
            foreach (ListItem item in cbl.Items)
            {
                if (arr.Contains(item.Value.ToLower()))
                {
                    item.Selected = true;
                }
            }
        }

        protected void btnApDungLoc_Click(object sender, EventArgs e)
        {
            var theloais = cblTheLoai.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
            var gias = cblGia.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();
            var khos = cblKho.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToList();

            var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            qs.Remove("theloai");
            qs.Remove("gia");
            qs.Remove("kho");
            qs.Remove("page");

            if (theloais.Count > 0) qs["theloai"] = string.Join(",", theloais);
            if (gias.Count > 0) qs["gia"] = string.Join(",", gias);
            if (khos.Count > 0) qs["kho"] = string.Join(",", khos);

            Response.Redirect("DanhSachSp.aspx" + (qs.Count > 0 ? "?" + qs.ToString() : ""));
        }

        private void ApplyFilters()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                ShowEmpty();
                lblSoLuong.Text = "0";
                return;
            }

            IEnumerable<Sach> result = ds;

            string type = (Request.QueryString["type"] ?? "").Trim();
            string typeVal = (Request.QueryString["value"] ?? "").Trim();
            string q = (Request.QueryString["q"] ?? "").Trim();
            string gia = (Request.QueryString["gia"] ?? "all").Trim();
            string kho = (Request.QueryString["kho"] ?? "all").Trim();
            string theloai = (Request.QueryString["theloai"] ?? "").Trim();
            string sort = ddlSort.SelectedValue;

            if (!string.IsNullOrEmpty(q))
            {
                lblTieuDe.Text = "Kết quả tìm kiếm: " + HttpUtility.HtmlEncode(q);
                result = result.Where(s =>
                    (s.TenSach ?? "").ToLower().Contains(q.ToLower()) ||
                    (s.TacGia ?? "").ToLower().Contains(q.ToLower()) ||
                    (s.MoTa ?? "").ToLower().Contains(q.ToLower()));
            }
            else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(typeVal))
            {
                lblTieuDe.Text = type.Equals("theloai", StringComparison.OrdinalIgnoreCase) ? "Thể loại: " + typeVal :
                                 type.Equals("tacgia", StringComparison.OrdinalIgnoreCase) ? "Tác giả: " + typeVal :
                                 type.Equals("nxb", StringComparison.OrdinalIgnoreCase) ? "Nhà xuất bản: " + typeVal :
                                 "Tất cả sách";
            }
            else
            {
                lblTieuDe.Text = "Tat ca sach";
            }

            if (type.Equals("theloai") && !string.IsNullOrEmpty(typeVal))
            {
                result = result.Where(s => string.Equals(s.TheLoai, typeVal, StringComparison.OrdinalIgnoreCase));
            }
            else if (type.Equals("tacgia") && !string.IsNullOrEmpty(typeVal))
            {
                result = result.Where(s => string.Equals(s.TacGia, typeVal, StringComparison.OrdinalIgnoreCase));
            }
            else if (type.Equals("nxb") && !string.IsNullOrEmpty(typeVal))
            {
                result = result.Where(s => string.Equals(s.NhaXuatBan, typeVal, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(theloai))
            {
                var arr = theloai.Split(',').Select(x => x.Trim().ToLower()).ToList();
                result = result.Where(s => arr.Contains((s.TheLoai ?? "").ToLower()));
            }

            if (!string.IsNullOrEmpty(gia) && gia != "all")
            {
                var arr = gia.Split(',').Select(x => x.Trim().ToLower()).ToList();
                result = result.Where(s =>
                {
                    if (arr.Contains("duoi50") && s.Gia < 50000) return true;
                    if (arr.Contains("50-100") && s.Gia >= 50000 && s.Gia <= 100000) return true;
                    if (arr.Contains("100-200") && s.Gia > 100000 && s.Gia <= 200000) return true;
                    if (arr.Contains("tren200") && s.Gia > 200000) return true;
                    return false;
                });
            }

            if (!string.IsNullOrEmpty(kho) && kho != "all")
            {
                var arr = kho.Split(',').Select(x => x.Trim().ToLower()).ToList();
                result = result.Where(s =>
                {
                    string k = (s.TinhTrangKho ?? "").ToLower();
                    if (arr.Contains("conhang") && k.Contains("con") || k.Contains("còn")) return true;
                    if (arr.Contains("dadat") && k.Contains("dat") || k.Contains("đặt")) return true;
                    return false;
                });
            }

            var list = result.ToList();
            _totalCount = list.Count;
            lblSoLuong.Text = _totalCount.ToString();

            list = ApplySort(list, sort);
            activeFilters.Visible = false; // Tạm ẩn active chips vì đã dùng checkbox có trạng thái checked


            if (list.Count == 0)
            {
                ShowEmpty();
                return;
            }

            pnlEmpty.Visible = false;

            int totalPages = (int)Math.Ceiling(list.Count / (double)PageSize);
            if (_currentPage > totalPages) _currentPage = totalPages;
            if (_currentPage < 1) _currentPage = 1;

            var pageItems = list.Skip((_currentPage - 1) * PageSize).Take(PageSize).ToList();
            rptSach.DataSource = pageItems;
            rptSach.DataBind();

            RenderPagination(totalPages);
        }

        private List<Sach> ApplySort(List<Sach> list, string sort)
        {
            switch (sort)
            {
                case "gia_asc":  return list.OrderBy(s => s.Gia).ToList();
                case "gia_desc": return list.OrderByDescending(s => s.Gia).ToList();
                case "ten_asc":  return list.OrderBy(s => s.TenSach).ToList();
                case "ten_desc": return list.OrderByDescending(s => s.TenSach).ToList();
                default:         return list.OrderBy(s => s.TenSach).ToList();
            }
        }

        private void ShowEmpty()
        {
            pnlEmpty.Visible = true;
            rptSach.DataSource = null;
            rptSach.DataBind();
            pnlPagination.Visible = false;
        }

        private void RenderPagination(int totalPages)
        {
            if (totalPages <= 1)
            {
                pnlPagination.Visible = false;
                return;
            }

            pnlPagination.Visible = true;
            lblPage.Text = _currentPage + " / " + totalPages;

            lnkPrev.HRef = _currentPage > 1 ? BuildPageUrl(_currentPage - 1) : "#";
            lnkPrev.Attributes["class"] = _currentPage > 1 ? "page-link" : "page-link disabled";
            lnkNext.HRef = _currentPage < totalPages ? BuildPageUrl(_currentPage + 1) : "#";
            lnkNext.Attributes["class"] = _currentPage < totalPages ? "page-link" : "page-link disabled";
        }

        private void ShowActiveFilters(string type, string typeVal, string q, string gia, string kho, string theloai)
        {
            // Removed
        }

        protected void Sort_Changed(object sender, EventArgs e)
        {
            string url = BuildCurrentUrl();
            url = UpdateOrAddParam(url, "sort", ddlSort.SelectedValue);
            Response.Redirect(url);
        }

        // ── URL helpers ──

        public string BuildFilterUrl(string param, string value)
        {
            return BuildUrl(param, value);
        }

        public string IsActiveGia(string value)
        {
            string current = (Request.QueryString["gia"] ?? "all").Trim();
            return string.Equals(current, value, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }

        public string IsActiveKho(string value)
        {
            string current = (Request.QueryString["kho"] ?? "all").Trim();
            return string.Equals(current, value, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }

        public string IsActiveTheLoai(string value)
        {
            string current = (Request.QueryString["theloai"] ?? "").Trim();
            return string.Equals(current, value, StringComparison.OrdinalIgnoreCase) ? "active" : "";
        }

        private string BuildUrl(string param, string value)
        {
            return BuildUrl(param, value, keepPage: false);
        }

        private string BuildUrl(string param, string value, bool keepPage)
        {
            var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            qs.Remove(param);
            if (!string.IsNullOrEmpty(value) && value != "all")
                qs[param] = value;
            if (!keepPage)
                qs.Remove("page");

            string query = qs.ToString();
            return "DanhSachSp.aspx" + (string.IsNullOrEmpty(query) ? "" : "?" + query);
        }

        private string BuildPageUrl(int page)
        {
            return BuildUrl("page", page.ToString(), keepPage: true);
        }

        private string BuildCurrentUrl()
        {
            return Request.Url.PathAndQuery;
        }

        private string UpdateOrAddParam(string url, string param, string value)
        {
            var uri = new Uri(Request.Url, url);
            var qs = HttpUtility.ParseQueryString(uri.Query);
            qs[param] = value;
            qs.Remove("page");
            return uri.GetLeftPart(UriPartial.Path) + "?" + qs.ToString();
        }

        private string BuildRemoveUrl(string param)
        {
            var qs = HttpUtility.ParseQueryString(Request.QueryString.ToString());
            qs.Remove(param);
            qs.Remove("page");
            return "DanhSachSp.aspx" + (qs.Count > 0 ? "?" + qs.ToString() : "");
        }

        // ── Cart ──

        protected void btnThemVaoGio_Command(object sender, CommandEventArgs e)
        {
            if (Session["user"] == null)
            {
                Response.Redirect("~/DangNhap.aspx");
                return;
            }

            string maSach = e.CommandArgument.ToString();
            var ds = Application["DanhSachSach"] as List<Sach>;
            var sach = ds?.FirstOrDefault(s => s.MaSach == maSach);
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
            (Master as Site)?.UpdateCartCountFromSession();
        }
    }
}
