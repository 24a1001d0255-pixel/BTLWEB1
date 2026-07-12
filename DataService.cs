using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WBS_BTL
{
    public static class DataService
    {
        private static string _dataFolder;
        private static string _sachFile;
        private static string _taiKhoanFile;
        private static string _donHangFile;
        private static readonly object _lock = new object();

        private static List<Sach> _sachCache;
        private static List<Account> _accountCache;
        private static List<DonHangItem> _donHangCache;
        private static bool _initialized;

        private static void EnsureInitialized()
        {
            if (_initialized) return;

            lock (_lock)
            {
                if (_initialized) return;

                string baseDir;
                try
                {
                    baseDir = HttpContext.Current != null
                        ? HttpContext.Current.Server.MapPath("~")
                        : AppDomain.CurrentDomain.BaseDirectory;
                }
                catch
                {
                    baseDir = AppDomain.CurrentDomain.BaseDirectory;
                }

                _dataFolder = Path.Combine(baseDir, "App_Data");
                if (!Directory.Exists(_dataFolder))
                    Directory.CreateDirectory(_dataFolder);

                _sachFile = Path.Combine(_dataFolder, "sach.json");
                _taiKhoanFile = Path.Combine(_dataFolder, "taikhoan.json");
                _donHangFile = Path.Combine(_dataFolder, "donhang.json");

                _initialized = true;
            }
        }

        public static void Initialize()
        {
            EnsureInitialized();

            _sachCache = LoadSach();
            _accountCache = LoadTaiKhoan();
            _donHangCache = LoadDonHang();

            EnsureDefaultAdmin();
        }

        private static void EnsureDefaultAdmin()
        {
            bool hasAdmin = _accountCache.Exists(a =>
                string.Equals(a.VaiTro, "Admin", StringComparison.OrdinalIgnoreCase));

            if (!hasAdmin)
            {
                _accountCache.Add(new Account
                {
                    TenDangNhap = "admin",
                    Email = "admin@bookverse.com",
                    MatKhau = "123456",
                    VaiTro = "Admin",
                    TrangThai = "HoatDong"
                });
                SaveTaiKhoan(_accountCache);
            }
        }

        public static List<Sach> GetSach()
        {
            if (_sachCache == null)
            {
                _sachCache = LoadSach();
            }
            return _sachCache;
        }

        public static void SaveSach(List<Sach> sach)
        {
            _sachCache = sach;
            SaveSachToFile(sach);
        }

        public static List<Account> GetTaiKhoan()
        {
            if (_accountCache == null)
            {
                _accountCache = LoadTaiKhoan();
            }
            return _accountCache;
        }

        public static void SaveTaiKhoan(List<Account> accounts)
        {
            _accountCache = accounts;
            SaveTaiKhoanToFile(accounts);
        }

        public static List<DonHangItem> GetDonHang()
        {
            if (_donHangCache == null)
            {
                _donHangCache = LoadDonHang();
            }
            return _donHangCache;
        }

        public static void SaveDonHang(List<DonHangItem> donHang)
        {
            _donHangCache = donHang;
            SaveDonHangToFile(donHang);
        }

        private static List<Sach> LoadSach()
        {
            EnsureInitialized();
            if (!File.Exists(_sachFile))
            {
                return DataInitializer.GetDanhSachSach();
            }
            try
            {
                string json = File.ReadAllText(_sachFile);
                return DeserializeSach(json) ?? DataInitializer.GetDanhSachSach();
            }
            catch
            {
                return DataInitializer.GetDanhSachSach();
            }
        }

        private static void SaveSachToFile(List<Sach> sach)
        {
            EnsureInitialized();
            try
            {
                string json = SerializeSach(sach);
                File.WriteAllText(_sachFile, json);
            }
            catch { }
        }

        private static List<Account> LoadTaiKhoan()
        {
            EnsureInitialized();
            if (!File.Exists(_taiKhoanFile))
            {
                return new List<Account>();
            }
            try
            {
                string json = File.ReadAllText(_taiKhoanFile);
                return DeserializeAccount(json) ?? new List<Account>();
            }
            catch
            {
                return new List<Account>();
            }
        }

        private static void SaveTaiKhoanToFile(List<Account> accounts)
        {
            EnsureInitialized();
            try
            {
                string json = SerializeAccount(accounts);
                File.WriteAllText(_taiKhoanFile, json);
            }
            catch { }
        }

        private static List<DonHangItem> LoadDonHang()
        {
            EnsureInitialized();
            if (!File.Exists(_donHangFile))
            {
                return new List<DonHangItem>();
            }
            try
            {
                string json = File.ReadAllText(_donHangFile);
                return DeserializeDonHang(json) ?? new List<DonHangItem>();
            }
            catch
            {
                return new List<DonHangItem>();
            }
        }

        private static void SaveDonHangToFile(List<DonHangItem> donHang)
        {
            EnsureInitialized();
            try
            {
                string json = SerializeDonHang(donHang);
                File.WriteAllText(_donHangFile, json);
            }
            catch { }
        }

        private static string SerializeSach(List<Sach> sach)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("[");
            for (int i = 0; i < sach.Count; i++)
            {
                var s = sach[i];
                if (i > 0) sb.Append(",");
                sb.Append("{");
                sb.Append("\"MaSach\":").Append(Esc(s.MaSach)).Append(",");
                sb.Append("\"TenSach\":").Append(Esc(s.TenSach)).Append(",");
                sb.Append("\"TacGia\":").Append(Esc(s.TacGia)).Append(",");
                sb.Append("\"NhaXuatBan\":").Append(Esc(s.NhaXuatBan)).Append(",");
                sb.Append("\"SoTrang\":").Append(s.SoTrang).Append(",");
                sb.Append("\"TinhTrangKho\":").Append(Esc(s.TinhTrangKho)).Append(",");
                sb.Append("\"Gia\":").Append(s.Gia).Append(",");
                sb.Append("\"Anh\":").Append(Esc(s.Anh)).Append(",");
                sb.Append("\"TheLoai\":").Append(Esc(s.TheLoai)).Append(",");
                sb.Append("\"MoTa\":").Append(Esc(s.MoTa)).Append(",");
                sb.Append("\"TomTat\":").Append(Esc(s.TomTat)).Append(",");
                sb.Append("\"GioiThieu\":").Append(Esc(s.GioiThieu)).Append(",");
                sb.Append("\"AnhMinhHoa\":").Append(Esc(s.AnhMinhHoa));
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static List<Sach> DeserializeSach(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            var result = new List<Sach>();
            int i = 0;
            json = json.Trim();
            if (json[i] != '[') return null;
            i++;
            while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
            if (i < json.Length && json[i] == ']') return result;

            var values = new List<string>();

            while (i < json.Length)
            {
                while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                if (i >= json.Length || json[i] != '{') break;

                values.Clear();

                i++;
                while (i < json.Length && json[i] != '}')
                {
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i >= json.Length || json[i] == '}') break;

                    while (i < json.Length && json[i] != ':') i++;
                    if (i >= json.Length) break;
                    i++;
                    while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
                    if (i >= json.Length) break;

                    string val = "";
                    if (json[i] == '"')
                    {
                        i++;
                        int start = i;
                        while (i < json.Length && json[i] != '"')
                        {
                            if (json[i] == '\\') i++;
                            i++;
                        }
                        val = json.Substring(start, i - start);
                        if (i < json.Length) i++;
                    }
                    else
                    {
                        int start = i;
                        while (i < json.Length && json[i] != ',' && json[i] != '}' && !char.IsWhiteSpace(json[i]))
                        {
                            i++;
                        }
                        val = json.Substring(start, i - start).Trim();
                    }
                    values.Add(val);
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i < json.Length && json[i] == ',') i++;
                }
                if (i < json.Length && json[i] == '}') i++;

                if (values.Count >= 9)
                {
                    result.Add(new Sach
                    {
                        MaSach = values.Count > 0 ? values[0] : "",
                        TenSach = values.Count > 1 ? values[1] : "",
                        TacGia = values.Count > 2 ? values[2] : "",
                        NhaXuatBan = values.Count > 3 ? values[3] : "",
                        SoTrang = values.Count > 4 && int.TryParse(values[4], out int sp) ? sp : 0,
                        TinhTrangKho = values.Count > 5 ? values[5] : "",
                        Gia = values.Count > 6 && double.TryParse(values[6], out double g) ? g : 0,
                        Anh = values.Count > 7 ? values[7] : "",
                        TheLoai = values.Count > 8 ? values[8] : "",
                        MoTa = values.Count > 9 ? values[9] : "",
                        TomTat = values.Count > 10 ? values[10] : "",
                        GioiThieu = values.Count > 11 ? values[11] : "",
                        AnhMinhHoa = values.Count > 12 ? values[12] : ""
                    });
                }
            }

            return result;
        }

        private static string SerializeAccount(List<Account> accounts)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("[");
            for (int i = 0; i < accounts.Count; i++)
            {
                var a = accounts[i];
                if (i > 0) sb.Append(",");
                sb.Append("{");
                sb.Append("\"TenDangNhap\":").Append(Esc(a.TenDangNhap)).Append(",");
                sb.Append("\"MatKhau\":").Append(Esc(a.MatKhau)).Append(",");
                sb.Append("\"Email\":").Append(Esc(a.Email)).Append(",");
                sb.Append("\"SoDienThoai\":").Append(Esc(a.SoDienThoai)).Append(",");
                sb.Append("\"DiaChi\":").Append(Esc(a.DiaChi)).Append(",");
                sb.Append("\"VaiTro\":").Append(Esc(a.VaiTro)).Append(",");
                sb.Append("\"TrangThai\":").Append(Esc(a.TrangThai));
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static List<Account> DeserializeAccount(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            var result = new List<Account>();
            int i = 0;
            json = json.Trim();
            if (json[i] != '[') return null;
            i++;
            while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
            if (i < json.Length && json[i] == ']') return result;

            while (i < json.Length)
            {
                while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                if (i >= json.Length || json[i] != '{') break;

                string tenDangNhap = "", matKhau = "", email = "", soDienThoai = "", diaChi = "", vaiTro = "User", trangThai = "HoatDong";

                i++;
                while (i < json.Length && json[i] != '}')
                {
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i >= json.Length || json[i] == '}') break;

                    while (i < json.Length && json[i] != ':') i++;
                    if (i >= json.Length) break;
                    i++;
                    while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
                    if (i >= json.Length) break;

                    string key = "";
                    if (json[i] == '"')
                    {
                        i++;
                        int start = i;
                        while (i < json.Length && json[i] != '"')
                        {
                            if (json[i] == '\\') i++;
                            i++;
                        }
                        key = json.Substring(start, i - start);
                        if (i < json.Length) i++;
                    }
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i < json.Length && json[i] == ',') { i++; continue; }
                    if (i >= json.Length || json[i] == '}') break;

                    while (i < json.Length && json[i] != ':') i++;
                    if (i >= json.Length) break;
                    i++;
                    while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
                    if (i >= json.Length) break;

                    string val = "";
                    if (json[i] == '"')
                    {
                        i++;
                        int start = i;
                        while (i < json.Length && json[i] != '"')
                        {
                            if (json[i] == '\\') i++;
                            i++;
                        }
                        val = json.Substring(start, i - start);
                        if (i < json.Length) i++;
                    }
                    else
                    {
                        int start = i;
                        while (i < json.Length && json[i] != ',' && json[i] != '}' && !char.IsWhiteSpace(json[i]))
                        {
                            i++;
                        }
                        val = json.Substring(start, i - start).Trim();
                    }

                    switch (key)
                    {
                        case "TenDangNhap": tenDangNhap = val; break;
                        case "MatKhau": matKhau = val; break;
                        case "Email": email = val; break;
                        case "SoDienThoai": soDienThoai = val; break;
                        case "DiaChi": diaChi = val; break;
                        case "VaiTro": vaiTro = val; break;
                        case "TrangThai": trangThai = val; break;
                    }

                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i < json.Length && json[i] == ',') i++;
                }
                if (i < json.Length && json[i] == '}') i++;

                result.Add(new Account
                {
                    TenDangNhap = tenDangNhap,
                    MatKhau = matKhau,
                    Email = email,
                    SoDienThoai = soDienThoai,
                    DiaChi = diaChi,
                    VaiTro = vaiTro,
                    TrangThai = trangThai
                });

                while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
            }

            return result;
        }

        private static string SerializeDonHang(List<DonHangItem> donHang)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append("[");
            for (int i = 0; i < donHang.Count; i++)
            {
                var d = donHang[i];
                if (i > 0) sb.Append(",");
                sb.Append("{");
                sb.Append("\"MaDon\":").Append(Esc(d.MaDon)).Append(",");
                sb.Append("\"Email\":").Append(Esc(d.Email)).Append(",");
                sb.Append("\"KhachHang\":").Append(Esc(d.KhachHang)).Append(",");
                sb.Append("\"NgayDat\":").Append(Esc(d.NgayDat)).Append(",");
                sb.Append("\"TongTien\":").Append(d.TongTien).Append(",");
                sb.Append("\"PhuongThuc\":").Append(Esc(d.PhuongThuc)).Append(",");
                sb.Append("\"TrangThai\":").Append(Esc(d.TrangThai)).Append(",");
                sb.Append("\"TrangThaiCode\":").Append(Esc(d.TrangThaiCode)).Append(",");
                sb.Append("\"ChiTiet\":").Append(SerializeChiTiet(d.ChiTiet));
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static string SerializeChiTiet(List<GioHangItem> chiTiet)
        {
            if (chiTiet == null || chiTiet.Count == 0) return "[]";
            var sb = new System.Text.StringBuilder();
            sb.Append("[");
            for (int i = 0; i < chiTiet.Count; i++)
            {
                var item = chiTiet[i];
                if (i > 0) sb.Append(",");
                sb.Append("{");
                sb.Append("\"MaSach\":").Append(Esc(item.MaSach)).Append(",");
                sb.Append("\"TenSach\":").Append(Esc(item.TenSach)).Append(",");
                sb.Append("\"Gia\":").Append(item.Gia).Append(",");
                sb.Append("\"Anh\":").Append(Esc(item.Anh)).Append(",");
                sb.Append("\"SoLuong\":").Append(item.SoLuong);
                sb.Append("}");
            }
            sb.Append("]");
            return sb.ToString();
        }

        private static List<DonHangItem> DeserializeDonHang(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            var result = new List<DonHangItem>();
            int i = 0;
            json = json.Trim();
            if (json[i] != '[') return null;
            i++;
            while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
            if (i < json.Length && json[i] == ']') return result;

            while (i < json.Length)
            {
                while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                if (i >= json.Length || json[i] != '{') break;

                string maDon = "", email = "", khachHang = "", ngayDat = "", phuongThuc = "", trangThai = "", trangThaiCode = "";
                double tongTien = 0;

                i++;
                while (i < json.Length && json[i] != '}')
                {
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i >= json.Length || json[i] == '}') break;

                    while (i < json.Length && json[i] != ':') i++;
                    if (i >= json.Length) break;
                    i++;
                    while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
                    if (i >= json.Length) break;

                    string key = "";
                    if (json[i] == '"')
                    {
                        i++;
                        int start = i;
                        while (i < json.Length && json[i] != '"')
                        {
                            if (json[i] == '\\') i++;
                            i++;
                        }
                        key = json.Substring(start, i - start);
                        if (i < json.Length) i++;
                    }
                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i < json.Length && json[i] == ',') { i++; continue; }
                    if (i >= json.Length || json[i] == '}') break;

                    while (i < json.Length && json[i] != ':') i++;
                    if (i >= json.Length) break;
                    i++;
                    while (i < json.Length && char.IsWhiteSpace(json[i])) i++;
                    if (i >= json.Length) break;

                    string val = "";
                    if (json[i] == '"')
                    {
                        i++;
                        int start = i;
                        while (i < json.Length && json[i] != '"')
                        {
                            if (json[i] == '\\') i++;
                            i++;
                        }
                        val = json.Substring(start, i - start);
                        if (i < json.Length) i++;
                    }
                    else
                    {
                        int start = i;
                        while (i < json.Length && json[i] != ',' && json[i] != '}' && !char.IsWhiteSpace(json[i]))
                        {
                            i++;
                        }
                        val = json.Substring(start, i - start).Trim();
                    }

                    switch (key)
                    {
                        case "MaDon": maDon = val; break;
                        case "Email": email = val; break;
                        case "KhachHang": khachHang = val; break;
                        case "NgayDat": ngayDat = val; break;
                        case "TongTien": double.TryParse(val, out tongTien); break;
                        case "PhuongThuc": phuongThuc = val; break;
                        case "TrangThai": trangThai = val; break;
                        case "TrangThaiCode": trangThaiCode = val; break;
                    }

                    while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
                    if (i < json.Length && json[i] == ',') i++;
                }
                if (i < json.Length && json[i] == '}') i++;

                result.Add(new DonHangItem
                {
                    MaDon = maDon,
                    Email = email,
                    KhachHang = khachHang,
                    NgayDat = ngayDat,
                    TongTien = tongTien,
                    PhuongThuc = phuongThuc,
                    TrangThai = trangThai,
                    TrangThaiCode = trangThaiCode,
                    ChiTiet = new List<GioHangItem>()
                });

                while (i < json.Length && (char.IsWhiteSpace(json[i]) || json[i] == ',')) i++;
            }

            return result;
        }

        private static string Esc(string s)
        {
            if (string.IsNullOrEmpty(s)) return "\"\"";
            return "\"" + s.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t") + "\"";
        }
    }
}
