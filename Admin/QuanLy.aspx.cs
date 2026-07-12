using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class QuanLy : Page
    {
        private string[] _thumbIds = { "fuAnh1", "fuAnh2", "fuAnh3", "fuAnh4" };

        protected void Page_Load(object sender, EventArgs e)
        {
            var acc = Session["user"] as Account;
            if (acc == null || !string.Equals(acc.Email, "admin@bookverse.com", StringComparison.OrdinalIgnoreCase))
            {
                Response.Redirect("../DangNhap.aspx");
                return;
            }

            if (!IsPostBack)
            {
                lblAdminInfo.Text = acc.TenDangNhap ?? acc.Email;
                LoadBooks();
                LoadOrders();
                LoadKhachHang();
            }
        }

        protected void SwitchTab_Click(object sender, EventArgs e)
        {
            var btn = sender as LinkButton;
            if (btn == null) return;

            pnlTabSach.Visible = btn.CommandArgument == "sach";
            pnlTabDonHang.Visible = btn.CommandArgument == "donhang";
            pnlTabKhachHang.Visible = btn.CommandArgument == "khachhang";

            lnkTabSach.CssClass = "admin-nav-item" + (pnlTabSach.Visible ? " active" : "");
            lnkTabDonHang.CssClass = "admin-nav-item" + (pnlTabDonHang.Visible ? " active" : "");
            lnkTabKhachHang.CssClass = "admin-nav-item" + (pnlTabKhachHang.Visible ? " active" : "");
        }

        private void LoadBooks()
        {
            var ds = Application["DanhSachSach"] as List<Sach>;
            gvSach.DataSource = ds ?? new List<Sach>();
            gvSach.DataBind();

            lblBookCount.Text = (ds == null ? 0 : ds.Count) + " cuốn";
        }

        private void LoadOrders()
        {
            var ds = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (ds == null)
            {
                ds = new List<DonHangItem>();
                Application["DanhSachDonHang"] = ds;
            }

            gvDonHang.DataSource = ds.OrderByDescending(x => x.NgayDat).ToList();
            gvDonHang.DataBind();

            lblOrderCount.Text = ds.Count + " đơn";
        }

        private void LoadKhachHang()
        {
            // Uu tien lay tu DataService, neu khong co thi lay tu Application
            var ds = DataService.GetTaiKhoan();
            if (ds == null)
            {
                ds = Application["DanhSachTaiKhoan"] as List<Account>;
            }
            if (ds == null)
            {
                ds = new List<Account>();
            }

            var customers = ds.Where(a => a.VaiTro != "Admin").ToList();
            gvKhachHang.DataSource = customers;
            gvKhachHang.DataBind();

            lblKhachHangCount.Text = customers.Count + " khách hàng";
        }

        protected void btnThemSach_Click(object sender, EventArgs e)
        {
            ResetBookForm();
            pnlBookEditor.Visible = true;
        }

        protected void btnLuuSach_Click(object sender, EventArgs e)
        {
            var ds = Application["DanhSachSach"] as List<Sach> ?? new List<Sach>();

            double gia;
            int soTrang;
            double.TryParse(txtGia.Text.Trim(), out gia);
            int.TryParse(txtSoTrang.Text.Trim(), out soTrang);

            string maSach = (txtMaSach.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(maSach))
            {
                maSach = "S" + (ds.Count + 1).ToString("D2");
            }

            string anhBia = LuuAnhBia(hfAnhCu.Value, maSach);
            string anhMinhHoa = LuuAnhMinhHoa(maSach);

            var existing = ds.FirstOrDefault(s =>
                string.Equals(s.MaSach, maSach, StringComparison.OrdinalIgnoreCase));

            if (existing != null)
            {
                existing.TenSach = txtTenSach.Text.Trim();
                existing.TacGia = txtTacGia.Text.Trim();
                existing.NhaXuatBan = txtNxb.Text.Trim();
                existing.Gia = gia > 0 ? gia : existing.Gia;
                existing.SoTrang = soTrang > 0 ? soTrang : existing.SoTrang;
                existing.TinhTrangKho = string.IsNullOrWhiteSpace(ddlTinhTrang.SelectedValue)
                    ? existing.TinhTrangKho : ddlTinhTrang.SelectedValue;
                existing.Anh = anhBia;
                existing.AnhMinhHoa = anhMinhHoa;
                existing.TheLoai = txtTheLoai.Text.Trim();
                existing.MoTa = txtMoTa.Text.Trim();
            }
            else
            {
                var sach = new Sach
                {
                    MaSach = maSach,
                    TenSach = txtTenSach.Text.Trim(),
                    TacGia = txtTacGia.Text.Trim(),
                    NhaXuatBan = txtNxb.Text.Trim(),
                    Gia = gia,
                    SoTrang = soTrang,
                    TinhTrangKho = string.IsNullOrWhiteSpace(ddlTinhTrang.SelectedValue)
                        ? "Con hang" : ddlTinhTrang.SelectedValue,
                    Anh = anhBia,
                    AnhMinhHoa = anhMinhHoa,
                    TheLoai = txtTheLoai.Text.Trim(),
                    MoTa = txtMoTa.Text.Trim()
                };

                ds.Add(sach);
            }

            Application["DanhSachSach"] = ds;
            DataService.SaveSach(ds);
            LoadBooks();
            pnlBookEditor.Visible = false;
        }

        protected void gvSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.CommandArgument?.ToString())) return;

            string maSach = e.CommandArgument.ToString();
            var ds = Application["DanhSachSach"] as List<Sach>;
            if (ds == null) return;

            if (e.CommandName.Equals("EditBook", StringComparison.OrdinalIgnoreCase))
            {
                var sach = ds.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null) return;

                txtMaSach.Text = sach.MaSach;
                txtTenSach.Text = sach.TenSach;
                txtTacGia.Text = sach.TacGia;
                txtNxb.Text = sach.NhaXuatBan;
                txtGia.Text = sach.Gia.ToString();
                txtSoTrang.Text = sach.SoTrang.ToString();
                ddlTinhTrang.SelectedValue = sach.TinhTrangKho;
                txtTheLoai.Text = sach.TheLoai ?? "";
                txtMoTa.Text = sach.MoTa ?? "";

                hfAnhCu.Value = sach.Anh ?? "";
                hfAnhMinhHoaCu.Value = sach.AnhMinhHoa ?? "";

                hfAnhMinhHoaUrl.Value = BuildThumbsJson(sach.AnhMinhHoa, sach.Anh);

                pnlBookEditor.Visible = true;
            }
            else if (e.CommandName.Equals("DeleteBook", StringComparison.OrdinalIgnoreCase))
            {
                var sach = ds.FirstOrDefault(s => s.MaSach == maSach);
                if (sach == null) return;

                ds.Remove(sach);
                Application["DanhSachSach"] = ds;
                DataService.SaveSach(ds);
                LoadBooks();
            }
        }

        protected void gvDonHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!e.CommandName.Equals("UpdateStatus", StringComparison.OrdinalIgnoreCase)) return;

            string maDon = e.CommandArgument?.ToString();
            var ds = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (ds == null) return;

            var don = ds.FirstOrDefault(d => d.MaDon == maDon);
            if (don == null) return;

            var row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
            if (row == null) return;

            var ddl = row.FindControl("ddlTrangThai") as DropDownList;
            if (ddl == null) return;

            don.TrangThaiCode = ddl.SelectedValue;
            don.TrangThai = ddl.SelectedItem.Text;
            Application["DanhSachDonHang"] = ds;
            DataService.SaveDonHang(ds);
            LoadOrders();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("DisableAccount", StringComparison.OrdinalIgnoreCase))
            {
                string email = e.CommandArgument.ToString();

                // Kiểm tra tức thời (Race Condition) - truy vấn lại đơn hàng ngay lúc bấm đồng ý
                var orders = Application["DanhSachDonHang"] as List<DonHangItem>;
                if (orders != null && orders.Any(o => string.Equals(o.Email, email, StringComparison.OrdinalIgnoreCase)))
                {
                    // Lỗi: Khách hàng đã có đơn hàng -> Không thể xóa/vô hiệu hóa cứng, hiển thị thông báo lỗi xung đột
                    ScriptManager.RegisterStartupScript(this, GetType(), "RaceCondition", "alert('Lỗi xung đột dữ liệu (Race Condition): Khách hàng này vừa phát sinh đơn hàng mới ngay trong lúc bạn đang thao tác. Đã hủy thao tác vô hiệu hóa.');", true);
                    return;
                }

                var accounts = Application["DanhSachTaiKhoan"] as List<Account>;
                if (accounts == null) return;

                var acc = accounts.FirstOrDefault(a => string.Equals(a.Email, email, StringComparison.OrdinalIgnoreCase));
                if (acc != null)
                {
                    acc.TrangThai = "VoHieuHoa";
                    Application["DanhSachTaiKhoan"] = accounts;
                    DataService.SaveTaiKhoan(accounts);
                    LoadKhachHang();
                }
            }
        }

        protected void gvSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSach.PageIndex = e.NewPageIndex;
            LoadBooks();
        }

        protected void gvDonHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDonHang.PageIndex = e.NewPageIndex;
            LoadOrders();
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvKhachHang.PageIndex = e.NewPageIndex;
            LoadKhachHang();
        }

        public string GetStatusClass(string code)
        {
            if (string.Equals(code, "dagiao", StringComparison.OrdinalIgnoreCase)) return "badge badge-ok";
            if (string.Equals(code, "dahuy", StringComparison.OrdinalIgnoreCase)) return "badge badge-danger";
            if (string.Equals(code, "danggiao", StringComparison.OrdinalIgnoreCase)) return "badge badge-info";
            return "badge badge-warn";
        }

        public string ResolveAdminCoverUrl(string rawPath)
        {
            string path = (rawPath ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(path)) return "../images/default.jpg";
            if (path.StartsWith("~/") || path.StartsWith("/")) return path;
            if (path.StartsWith("images/", StringComparison.OrdinalIgnoreCase)) return "../" + path;
            return "../images/" + path;
        }

        private void ResetBookForm()
        {
            txtMaSach.Text = string.Empty;
            txtTenSach.Text = string.Empty;
            txtTacGia.Text = string.Empty;
            txtNxb.Text = string.Empty;
            txtGia.Text = string.Empty;
            txtSoTrang.Text = string.Empty;
            ddlTinhTrang.SelectedValue = "Còn hàng";
            hfAnhCu.Value = string.Empty;
            hfAnhMinhHoaCu.Value = string.Empty;
            hfAnhMinhHoaUrl.Value = string.Empty;
            txtTheLoai.Text = string.Empty;
            txtMoTa.Text = string.Empty;
        }

        private string LuuAnhBia(string anhCu, string maSach)
        {
            var fu = FindControlRecursive(formAdmin, "fuAnhBia") as FileUpload;
            if (fu != null && fu.HasFile)
            {
                return SaveCover(fu, anhCu, maSach);
            }
            return string.IsNullOrWhiteSpace(anhCu) ? "images/default.jpg" : anhCu;
        }

        private string LuuAnhMinhHoa(string maSach)
        {
            var currentThumbs = ParseThumbPaths(hfAnhMinhHoaCu.Value);
            var result = new List<string>(currentThumbs);
            bool anyChanged = false;

            for (int i = 0; i < _thumbIds.Length && i < result.Count; i++)
            {
                var fu = FindControlRecursive(formAdmin, _thumbIds[i]) as FileUpload;
                if (fu != null && fu.HasFile)
                {
                    string oldPath = result[i];
                    if (!string.IsNullOrWhiteSpace(oldPath))
                    {
                        DeleteOldImage(oldPath);
                    }
                    result[i] = SaveThumb(fu, maSach, i);
                    anyChanged = true;
                }
            }

            return anyChanged ? string.Join(",", result.Where(p => !string.IsNullOrWhiteSpace(p))) : hfAnhMinhHoaCu.Value;
        }

        private string[] ParseThumbPaths(string anhMinhHoa)
        {
            if (string.IsNullOrWhiteSpace(anhMinhHoa))
                return new string[4];

            var parts = anhMinhHoa.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(p => p.Trim())
                .Take(4)
                .ToArray();

            var padded = new string[4];
            Array.Copy(parts, padded, Math.Min(parts.Length, 4));
            return padded;
        }

        private string SaveCover(FileUpload fu, string oldPath, string maSach)
        {
            string ext = Path.GetExtension(fu.FileName).ToLower();
            string[] allowed = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            if (!allowed.Contains(ext)) return "images/default.jpg";

            string saveDir = Server.MapPath("~/images/");
            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

            string fileName = maSach + "_cover" + ext;
            string newRelative = "images/" + fileName;

            bool shouldDeleteOld = !string.IsNullOrWhiteSpace(oldPath) &&
                                   !string.Equals(oldPath.Trim(), newRelative, StringComparison.OrdinalIgnoreCase);

            fu.SaveAs(Path.Combine(saveDir, fileName));

            if (shouldDeleteOld)
            {
                DeleteOldImage(oldPath);
            }

            return newRelative;
        }

        private string SaveThumb(FileUpload fu, string maSach, int index)
        {
            string ext = Path.GetExtension(fu.FileName).ToLower();
            string[] allowed = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
            if (!allowed.Contains(ext)) return "images/default.jpg";

            string newRelative;
            string folderName;
            if (BookFolders.TryGetValue(maSach, out folderName))
            {
                string folderSaveDir = Server.MapPath("~/images/" + folderName);
                if (!Directory.Exists(folderSaveDir)) Directory.CreateDirectory(folderSaveDir);

                string fileName = (index + 1) + ext;
                newRelative = "images/" + folderName.Replace("\\", "/") + "/" + fileName;
                fu.SaveAs(Path.Combine(folderSaveDir, fileName));
            }
            else
            {
                string saveDir = Server.MapPath("~/images/");
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

                string plainName = maSach + "_thumb" + (index + 1) + ext;
                newRelative = "images/" + plainName;
                fu.SaveAs(Path.Combine(saveDir, plainName));
            }

            return newRelative;
        }

        private static readonly Dictionary<string, string> BookFolders = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "S01", "Đắc Nhân Tâm" },
            { "S02", "Nhà Giả Kim" },
            { "S04", "Tư Duy Nhanh Và Chậm" },
            { "S06", "Chiến Binh Cầu Vồng" },
            { "S07", "Atomic Habits" },
            { "S08", "Tuổi Trẻ Đáng Giá Bao Nhiêu" }
        };

        private void DeleteOldImage(string oldPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldPath)) return;

                string[] parts = oldPath.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string p in parts)
                {
                    string trimmed = p.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed)) continue;

                    string physicalPath = trimmed.StartsWith("images/", StringComparison.OrdinalIgnoreCase)
                        ? Server.MapPath("~/" + trimmed)
                        : Server.MapPath("~/images/" + Path.GetFileName(trimmed));

                    if (File.Exists(physicalPath))
                    {
                        try { File.Delete(physicalPath); }
                        catch { }
                    }
                }
            }
            catch
            {
                // best effort cleanup
            }
        }

        private string BuildThumbsJson(string anhMinhHoa, string anhBia)
        {
            var urls = new List<string>();

            if (!string.IsNullOrWhiteSpace(anhMinhHoa))
            {
                urls.AddRange(anhMinhHoa
                    .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => ResolveAdminCoverUrl(p.Trim()))
                    .Take(4));
            }

            while (urls.Count < 4)
                urls.Add("");

            return "[\"" + string.Join("\",\"", urls) + "\"]";
        }

        private Control FindControlRecursive(Control root, string id)
        {
            if (root == null) return null;
            if (string.Equals(root.ID, id, StringComparison.OrdinalIgnoreCase)) return root;
            foreach (Control child in root.Controls)
            {
                var found = FindControlRecursive(child, id);
                if (found != null) return found;
            }
            return null;
        }

        protected void btnCloseInlineEditor_Click(object sender, EventArgs e)
        {
            pnlBookEditor.Visible = false;
        }
    }
}
