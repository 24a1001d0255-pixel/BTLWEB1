using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class ThanhToan : System.Web.UI.Page
    {
        private const double PhiVanChuyen = 30000;
        private double _tamTinh;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var gioHang = Session["giohang"] as List<GioHangItem>;
                if (gioHang == null || gioHang.Count == 0)
                {
                    pnlCheckout.Visible = false;
                    pnlSuccess.Visible = false;
                    pnlEmpty.Visible = true;
                    return;
                }

                pnlCheckout.Visible = true;
                pnlEmpty.Visible = false;
                pnlSuccess.Visible = false;

                HienThiDonHang(gioHang);

                _tamTinh = gioHang.Sum(x => x.ThanhTien);
                double tongThanhToan = _tamTinh + PhiVanChuyen;

                lblTamTinh.Text = _tamTinh.ToString("N0") + "₫";
            }
        }

        private void HienThiDonHang(List<GioHangItem> gioHang)
        {
            rptDonHang.DataSource = gioHang;
            rptDonHang.DataBind();

            double tamTinh = gioHang.Sum(x => x.ThanhTien);
            double tongThanhToan = tamTinh + PhiVanChuyen;

            lblTamTinh.Text = tamTinh.ToString("N0") + "₫";
            lblTongThanhToan.Text = tongThanhToan.ToString("N0") + "₫";
        }

        protected void btnDatHang_Click(object sender, EventArgs e)
        {
            var gioHang = Session["giohang"] as List<GioHangItem>;
            if (gioHang == null || gioHang.Count == 0)
            {
                Response.Redirect("~/TrangChu.aspx");
                return;
            }

            string hoTen = txtHoTen.Text?.Trim() ?? string.Empty;
            string soDienThoai = txtSoDienThoai.Text?.Trim() ?? string.Empty;
            string diaChi = txtDiaChi.Text?.Trim() ?? string.Empty;
            string ghiChu = txtGhiChu.Text?.Trim() ?? string.Empty;
            string phuongThuc = rblPayment.SelectedValue ?? "COD";

            if (string.IsNullOrWhiteSpace(hoTen) || string.IsNullOrWhiteSpace(diaChi) || string.IsNullOrWhiteSpace(soDienThoai))
            {
                return;
            }

            var dsSanPham = Application["DanhSachSach"] as List<Sach>;
            if (dsSanPham != null)
            {
                foreach (var item in gioHang)
                {
                    var sach = dsSanPham.FirstOrDefault(s => string.Equals(s.MaSach, item.MaSach, StringComparison.OrdinalIgnoreCase));
                    if (sach != null && sach.TinhTrangKho != null && sach.TinhTrangKho.Equals("Còn hàng", StringComparison.OrdinalIgnoreCase))
                    {
                        sach.TinhTrangKho = "Đã đặt";
                    }
                }
                DataService.SaveSach(dsSanPham);
            }

            string maDon = "BV" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
            DateTime now = DateTime.Now;
            DateTime giaoDuKien = now.AddDays(3);

            Session["giohang"] = new List<GioHangItem>();
            var m = this.Master as Site;
            m?.UpdateCartCountFromSession();

            var donHangs = Application["DanhSachDonHang"] as List<DonHangItem>;
            if (donHangs == null)
            {
                donHangs = new List<DonHangItem>();
            }

            var user = Session["user"] as Account;
            string emailUser = user != null ? user.Email : string.Empty;

            double tamTinh = gioHang.Sum(x => x.ThanhTien);

            donHangs.Add(new DonHangItem
            {
                MaDon = maDon,
                KhachHang = hoTen,
                Email = emailUser,
                NgayDat = now.ToString("dd/MM/yyyy HH:mm"),
                TongTien = tamTinh + PhiVanChuyen,
                PhuongThuc = phuongThuc == "COD" ? "COD" : phuongThuc,
                TrangThai = "Đang xử lý",
                TrangThaiCode = "dangxuly",
                ChiTiet = new List<GioHangItem>(gioHang)
            });
            Application["DanhSachDonHang"] = donHangs;
            DataService.SaveDonHang(donHangs);

            pnlCheckout.Visible = false;
            pnlSuccess.Visible = true;

            lblMaDon.Text = maDon;
            lblGiaoDuKien.Text = giaoDuKien.ToString("dd/MM/yyyy");
        }
    }
}
