<%@ Page Title="Trang chủ" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="TrangChu.aspx.cs" Inherits="WBS_BTL.TrangChu" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/TrangChu.css" />

    <!-- HERO — chỉ xuất hiện trên trang chủ -->
    <section class="hero">
        <div class="container hero-inner">
            <div class="hero-content">
                <h1 class="hero-title">Khởi nguồn tri thức từ những trang sách</h1>
                <p class="hero-subtitle">Khám phá bộ sưu tập sách hay, cập nhật xu hướng đọc và gửi tặng người thân yêu thương.</p>
                <div class="hero-actions">
                    <asp:HyperLink runat="server" NavigateUrl="DanhSachSp.aspx" CssClass="btn btn-primary">Khám phá ngay</asp:HyperLink>
                </div>
            </div>
            <div class="hero-visual">
                <div class="hero-books-wrapper">
                    <asp:Repeater ID="rptHeroBooks" runat="server">
                        <ItemTemplate>
                            <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="hero-book" title='<%# Eval("TenSach") %>'>
                                <img src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                            </a>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </section>

    <section class="page-home">
        <div class="container">

            <!-- SÁCH NỔI BẬT -->
            <div class="home-section">
                <div class="section-header">
                    <h2 class="section-title">Sách nổi bật</h2>
                    <a href="DanhSachSp.aspx" class="section-more">Xem tất cả</a>
                </div>
                <div class="book-grid">
                    <asp:Repeater ID="rptSachNoiBat" runat="server">
                        <ItemTemplate>
                            <article class="book-card">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
                                    <p class="book-meta"><%# Eval("TacGia") %></p>
                                    <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                    <asp:Button ID="btnThemVaoGio" runat="server" Text="Thêm vào giỏ"
                                        CommandArgument='<%# Eval("MaSach") %>'
                                        OnCommand="btnThemVaoGio_Command"
                                        CssClass="btn-add-cart" />
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <!-- TRUYỆN THIẾU NHI -->
            <div class="home-section">
                <div class="section-header">
                    <h2 class="section-title">Truyện thiếu nhi</h2>
                    <a href="DanhSachSp.aspx?type=theloai&value=Thieu-nhi" class="section-more">Xem tất cả</a>
                </div>
                <div class="book-grid book-grid-compact">
                    <asp:Repeater ID="rptThieuNhi" runat="server">
                        <ItemTemplate>
                            <article class="book-card book-card-compact">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
                                    <p class="book-meta"><%# Eval("TacGia") %></p>
                                    <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlThieuNhiEmpty" runat="server" CssClass="section-empty" Visible="false" />
                </div>
            </div>

            <!-- TIỂU THUYẾT -->
            <div class="home-section">
                <div class="section-header">
                    <h2 class="section-title">Tiểu thuyết</h2>
                    <a href="DanhSachSp.aspx?type=theloai&value=Van-hoc" class="section-more">Xem tất cả</a>
                </div>
                <div class="book-grid book-grid-compact">
                    <asp:Repeater ID="rptTieuThuyet" runat="server">
                        <ItemTemplate>
                            <article class="book-card book-card-compact">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
                                    <p class="book-meta"><%# Eval("TacGia") %></p>
                                    <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlTieuThuyetEmpty" runat="server" CssClass="section-empty" Visible="false" />
                </div>
            </div>

            <!-- SÁCH GIÁO KHOA -->
            <div class="home-section">
                <div class="section-header">
                    <h2 class="section-title">Sách giáo khoa</h2>
                    <a href="DanhSachSp.aspx?type=theloai&value=Giao-khoa" class="section-more">Xem tất cả</a>
                </div>
                <div class="book-grid book-grid-compact">
                    <asp:Repeater ID="rptGiaoKhoa" runat="server">
                        <ItemTemplate>
                            <article class="book-card book-card-compact">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
                                    <p class="book-meta"><%# Eval("TacGia") %></p>
                                    <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlGiaoKhoaEmpty" runat="server" CssClass="section-empty" Visible="false" />
                </div>
            </div>

            <!-- SELF-CARE -->
            <div class="home-section">
                <div class="section-header">
                    <h2 class="section-title">Self-care & Kỹ năng sống</h2>
                    <a href="DanhSachSp.aspx?type=theloai&value=Ky-nang" class="section-more">Xem tất cả</a>
                </div>
                <div class="book-grid book-grid-compact">
                    <asp:Repeater ID="rptSelfCare" runat="server">
                        <ItemTemplate>
                            <article class="book-card book-card-compact">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                    <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                </a>
                                <div class="book-body">
                                    <h3 class="book-title"><a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a></h3>
                                    <p class="book-meta"><%# Eval("TacGia") %></p>
                                    <div class="book-price"><%# Eval("Gia", "{0:N0}₫") %></div>
                                </div>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlSelfCareEmpty" runat="server" CssClass="section-empty" Visible="false" />
                </div>
            </div>

        </div>
    </section>
</asp:Content>
