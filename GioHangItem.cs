using System;

namespace WBS_BTL
{
    public class GioHangItem
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public double Gia { get; set; }
        public string Anh { get; set; }
        public int SoLuong { get; set; }

        public double ThanhTien
        {
            get { return Gia * SoLuong; }
        }
    }
}
