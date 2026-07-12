using System;

namespace WBS_BTL
{
    public class Account
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string DiaChi { get; set; } = string.Empty;
        public string VaiTro { get; set; } = "User";
        public string TrangThai { get; set; } = "HoatDong";
    }
}
