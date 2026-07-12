using System;

namespace WBS_BTL
{
    public class DonHangItem
    {
        public string MaDon { get; set; }
        public string KhachHang { get; set; }
        public string Email { get; set; }
        public string NgayDat { get; set; }
        public double TongTien { get; set; }
        public string PhuongThuc { get; set; }
        public string TrangThai { get; set; }
        public string TrangThaiCode { get; set; }
        public System.Collections.Generic.List<GioHangItem> ChiTiet { get; set; } = new System.Collections.Generic.List<GioHangItem>();
    }
}
