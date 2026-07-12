using System;
using System.Collections.Generic;
using System.Linq;

namespace WBS_BTL
{
    public static class DataInitializer
    {
        public static List<Sach> GetDanhSachSach()
        {
            var list = new List<Sach>
            {
                // ── Self-care & Kỹ năng sống (Ky-nang) ──
                new Sach { MaSach = "S01", TenSach = "Đắc Nhân Tâm",    TacGia = "Dale Carnegie",       NhaXuatBan = "NXB Tổng hợp TP.HCM",    SoTrang = 320, TinhTrangKho = "Còn hàng", Gia = 68000,  Anh = "dacnhantam.webp",      TheLoai = "Ky-nang",    MoTa = "Cuốn sách kinh điển về cách thức ứng xử và thấu hiểu con người." },
                new Sach { MaSach = "S07", TenSach = "Atomic Habits",    TacGia = "James Clear",          NhaXuatBan = "NXB Thế Giới",            SoTrang = 336, TinhTrangKho = "Còn hàng", Gia = 145000, Anh = "AtomicHabits.webp",    TheLoai = "Ky-nang",    MoTa = "Thay đổi nhỏ mỗi ngày tạo nên thành công lớn bền vững theo thời gian." },
                new Sach { MaSach = "S08", TenSach = "Tuổi Trẻ Đáng Giá Bao Nhiêu", TacGia = "Rosie Nguyễn", NhaXuatBan = "NXB Hà Nội",        SoTrang = 288, TinhTrangKho = "Còn hàng", Gia = 61000,  Anh = "tuoitredanggiabaonhieu.webp", TheLoai = "Ky-nang", MoTa = "Những suy ngẫm chân thành về học tập, yêu thương và trưởng thành." },
                new Sach { MaSach = "S04", TenSach = "Tư Duy Nhanh Và Chậm", TacGia = "Daniel Kahneman",   NhaXuatBan = "NXB Lao Động",           SoTrang = 610, TinhTrangKho = "Còn hàng", Gia = 168000, Anh = "tuduynhanhvacham.webp", TheLoai = "Ky-nang",    MoTa = "Giải mã cách con người ra quyết định thông qua hai hệ thống tư duy." },

                // ── Tiểu thuyết (Van-hoc) ──
                new Sach { MaSach = "S02", TenSach = "Nhà Giả Kim",        TacGia = "Paulo Coelho",         NhaXuatBan = "NXB Hà Nội",            SoTrang = 228, TinhTrangKho = "Còn hàng", Gia = 79000,  Anh = "nhagiakim.webp",       TheLoai = "Van-hoc",    MoTa = "Hành trình truy tìm ước mơ và ý nghĩa cuộc sống của chàng chăn cừu Santiago." },
                new Sach { MaSach = "S06", TenSach = "Chiến Binh Cầu Vồng", TacGia = "Andrea Hirata",       NhaXuatBan = "NXB Trẻ",               SoTrang = 244, TinhTrangKho = "Còn hàng", Gia = 85000,  Anh = "chienbinhcauvong.webp", TheLoai = "Van-hoc",    MoTa = "Câu chuyện về tình bạn, ước mơ và nghị lực của những đứa trẻ nghèo huyện Belitong." },
                new Sach { MaSach = "S10", TenSach = "Truyện Kiều",         TacGia = "Nguyễn Du",            NhaXuatBan = "NXB Văn Học",           SoTrang = 152, TinhTrangKho = "Còn hàng", Gia = 45000,  Anh = "truyenkieu.webp",       TheLoai = "Van-hoc",    MoTa = "Kiệt tác văn học dân tộc với những vần thơ bất hủ về nhân sinh quan." },
                new Sach { MaSach = "S11", TenSach = "Mắt Biếc",            TacGia = "Nguyễn Nhật Ánh",     NhaXuatBan = "NXB Trẻ",               SoTrang = 316, TinhTrangKho = "Còn hàng", Gia = 92000,  Anh = "matbiec.webp",          TheLoai = "Van-hoc",    MoTa = "Câu chuyện tình yêu trong sáng và day dứt của chàng Trang và cô bé Hà Lan." },

                // ── Truyện thiếu nhi (Thieu-nhi) ──
                new Sach { MaSach = "S05", TenSach = "Dế Mèn Phiêu Lưu Ký", TacGia = "Tô Hoài",            NhaXuatBan = "NXB Kim Đồng",          SoTrang = 172, TinhTrangKho = "Còn hàng", Gia = 52000,  Anh = "demenphieuluuky.jpg",  TheLoai = "Thieu-nhi", MoTa = "Hành trình trưởng thành của chú Dế Mèn qua những cuộc phiêu lưu đáng nhớ." },
                new Sach { MaSach = "S12", TenSach = "Harry Potter và Hòn Đá Phù Thủy", TacGia = "J.K. Rowling", NhaXuatBan = "NXB Phú Hà",  SoTrang = 310, TinhTrangKho = "Còn hàng", Gia = 135000, Anh = "harrypottervahondaphuthuy.jpg", TheLoai = "Thieu-nhi", MoTa = "Cậu bé phù thủy bắt đầu hành trình khám phá thế giới phép thuật kỳ diệu." },
                new Sach { MaSach = "S13", TenSach = "Chú Lính Chì Dũng Sĩ", TacGia = "Anderson",           NhaXuatBan = "NXB Kim Đồng",          SoTrang = 144, TinhTrangKho = "Còn hàng", Gia = 38000,  Anh = "demenphieuluuky.jpg",   TheLoai = "Thieu-nhi", MoTa = "Câu chuyện cảm động về tình bạn, lòng dũng cảm và sự hy sinh của những người lính thiếu nhi." },
                new Sach { MaSach = "S14", TenSach = "Doraemon - Tập 1",     TacGia = "Fujiko F. Fujio",      NhaXuatBan = "NXB Trẻ",               SoTrang = 192, TinhTrangKho = "Còn hàng", Gia = 29000,  Anh = "matbiec.webp",            TheLoai = "Thieu-nhi", MoTa = "Những câu chuyện hài hước và ý nghĩa về chú mèo máy đến từ tương lai." },

                // ── Sách giáo khoa (Giao-khoa) ──
                new Sach { MaSach = "S09", TenSach = "Giáo Khoa Toán 12",    TacGia = "Bộ GD&ĐT",           NhaXuatBan = "NXB Giáo Dục Việt Nam", SoTrang = 196, TinhTrangKho = "Còn hàng", Gia = 32000,  Anh = "toan12.png",           TheLoai = "Giao-khoa", MoTa = "Sách giáo khoa chính thức theo chương trình Giáo dục phổ thông 2018." },
                new Sach { MaSach = "S15", TenSach = "Ngữ Văn 12",           TacGia = "Bộ GD&ĐT",           NhaXuatBan = "NXB Giáo Dục Việt Nam", SoTrang = 226, TinhTrangKho = "Còn hàng", Gia = 35000,  Anh = "luocsuloainguoi.webp",   TheLoai = "Giao-khoa", MoTa = "Sách giáo khoa Ngữ Văn lớp 12 theo chương trình GDPT 2018, tập trung văn học và tiếng Việt." },
                new Sach { MaSach = "S16", TenSach = "Vật Lý 12",            TacGia = "Bộ GD&ĐT",           NhaXuatBan = "NXB Giáo Dục Việt Nam", SoTrang = 244, TinhTrangKho = "Còn hàng", Gia = 38000,  Anh = "nhagiakim.webp",         TheLoai = "Giao-khoa", MoTa = "Sách giáo khoa Vật Lý lớp 12 bao gồm phần cơ học, điện xoay chiều và quang học." },
                new Sach { MaSach = "S17", TenSach = "Hóa Học 12",          TacGia = "Bộ GD&ĐT",           NhaXuatBan = "NXB Giáo Dục Việt Nam", SoTrang = 238, TinhTrangKho = "Còn hàng", Gia = 36000,  Anh = "chienbinhcauvong.webp",   TheLoai = "Giao-khoa", MoTa = "Sách giáo khoa Hóa Học lớp 12 với các chủ đề hóa hữu cơ và vô cơ nâng cao." },

                // ── Khoa học (Khoa-hoc) ──
                new Sach { MaSach = "S03", TenSach = "Sapiens: Lược Sử Loài Người", TacGia = "Yuval Noah Harari", NhaXuatBan = "NXB Tri Thức",    SoTrang = 560, TinhTrangKho = "Còn hàng", Gia = 195000, Anh = "luocsuloainguoi.webp", TheLoai = "Khoa-hoc", MoTa = "Hành trình tiến hóa vĩ đại của loài người từ thời tiền sử đến thời hiện đại." },
            };

            foreach (var s in list)
            {
                if (string.IsNullOrWhiteSpace(s.MoTa))
                    s.MoTa = BuildMoTaSach(s);
                
                s.TomTat = "Tóm tắt nội dung: " + s.MoTa + " Cuốn sách tập trung đi sâu vào các khía cạnh thú vị để người đọc có thể dễ dàng nắm bắt những thông điệp chính mà tác giả muốn truyền tải.";
                s.GioiThieu = "Giới thiệu câu chuyện: " + s.TenSach + " mang đến một góc nhìn mới mẻ và đầy cảm hứng. Ngay từ những trang đầu tiên, độc giả sẽ bị cuốn hút vào mạch nội dung sâu sắc và đầy ý nghĩa.";
            }

            return list;
        }

        private static string BuildMoTaSach(Sach s)
        {
            string theLoai = string.IsNullOrWhiteSpace(s.TheLoai) ? "sách tham khảo" : s.TheLoai;
            return string.Format("{0} của {1} thuộc thể loại {2}. Tác phẩm phù hợp với độc giả yêu sách và muốn khám phá thêm kiến thức qua từng trang viết.", s.TenSach, string.IsNullOrWhiteSpace(s.TacGia) ? "tác giả" : s.TacGia, theLoai);
        }
    }
}
