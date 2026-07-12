<%@ Page Title="Chi tiết sách" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ChiTietSanPham.aspx.cs" Inherits="WBS_BTL.ChiTietSanPham" %>

<asp:Content ID="ContentDetail" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ChiTietSanPham.css" />

    <section class="page-detail">
        <div class="container detail-layout">

            <!-- COT TRAI: Hinh anh -->
            <div class="detail-visual">
                <div class="cover-main-wrap">
                    <asp:Image ID="imgBiaSach" runat="server" CssClass="cover-main" AlternateText="Bia sach" />
                </div>
                <div class="thumbnail-strip" id="thumbnailStrip" runat="server">
                    <asp:Repeater ID="rptThumbnails" runat="server">
                        <ItemTemplate>
                            <button type="button" class="thumb-btn"
                                onclick="switchCover(this, '<%# Eval("Url") %>')">
                                <img src='<%# Eval("Url") %>' alt='<%# Eval("Alt") %>' />
                            </button>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>

            <!-- COT PHAI: Thong tin san pham -->
            <div class="detail-info">

                <div class="detail-breadcrumb">
                    <a href="TrangChu.aspx">Trang chủ</a>
                    <span class="bc-sep">/</span>
                    <asp:HyperLink ID="lnkBreadcrumbLoai" runat="server" CssClass="bc-link"></asp:HyperLink>
                    <span class="bc-sep">/</span>
                    <span class="bc-current"><asp:Label ID="lblBreadcrumbTen" runat="server" Text="..."></asp:Label></span>
                </div>

                <h1 class="detail-title">
                    <asp:Label ID="lblTenSach" runat="server" Text="Đang tải..."></asp:Label>
                </h1>

                <div class="detail-meta">
                    <div class="meta-item">
                        <span class="meta-label">Tác giả</span>
                        <span class="meta-value"><asp:Label ID="lblTacGia" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Nhà xuất bản</span>
                        <span class="meta-value"><asp:Label ID="lblNhaXuatBan" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Số trang</span>
                        <span class="meta-value"><asp:Label ID="lblSoTrang" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Thể loại</span>
                        <span class="meta-value"><asp:Label ID="lblTheLoai" runat="server" Text="-"></asp:Label></span>
                    </div>
                    <div class="meta-item">
                        <span class="meta-label">Tình trạng</span>
                        <span class="meta-value">
                            <asp:Label ID="lblTinhTrang" runat="server" Text="-"></asp:Label>
                        </span>
                    </div>
                </div>

                <div class="detail-price-row">
                    <span class="price-label">Giá bán</span>
                    <span class="price-value">
                        <asp:Label ID="lblGia" runat="server" CssClass="price-number" Text="-"></asp:Label>
                    </span>
                </div>

                <div class="detail-actions">
                    <asp:Button ID="btnThemVaoGio" runat="server" Text="Thêm vào giỏ hàng"
                        CssClass="btn-add-cart"
                        OnClick="btnThemVaoGio_Click" />
                    <a href="#previewSection" class="btn-preview">Xem nội dung</a>
                </div>

                <asp:Panel ID="pnlInlineMessage" runat="server" CssClass="inline-message" Visible="false">
                    <span class="inline-message-text">Đã thêm vào giỏ hàng!</span>
                </asp:Panel>

            </div>
        </div>

        <!-- MO TA CHI TIET -->
        <div class="detail-description-section">
            <div class="description-tabs">
                <button type="button" class="tab-btn active" onclick="showTab('mota', this)">Mô tả sách</button>
            </div>

            <div class="tab-content active" id="tab-mota">
                <div class="rich-description">
                    <asp:Literal ID="litMoTa" runat="server"></asp:Literal>
                </div>
            </div>
        </div>

        <!-- SẢN PHẨM LIÊN QUAN -->
        <section class="related-section">
            <div class="related-header">
                <h2 class="related-title">Sản phẩm liên quan</h2>
                <a href="#" class="related-more">Xem tất cả sản phẩm cùng thể loại</a>
            </div>
            <div class="related-scroll-wrapper">
                <div class="related-scroll">
                    <asp:Repeater ID="rptSanPhamLienQuan" runat="server">
                        <ItemTemplate>
                            <article class="related-card">
                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="related-card-link">
                                    <img class="related-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                    <div class="related-body">
                                        <h3 class="related-book-title"><%# Eval("TenSach") %></h3>
                                        <p class="related-author"><%# Eval("TacGia") %></p>
                                        <div class="related-price"><%# Eval("Gia", "{0:N0}") %>đ</div>
                                    </div>
                                </a>
                                <asp:Button runat="server" Text="Them"
                                    CommandArgument='<%# Eval("MaSach") %>'
                                    OnCommand="btnThemLienQuan_Command"
                                    CssClass="btn-add-small" />
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Panel ID="pnlRelatedEmpty" runat="server" CssClass="related-empty" Visible="false">
                        <p>Chưa có sản phẩm liên quan.</p>
                    </asp:Panel>
                </div>
            </div>
        </section>

        <!-- REVIEWS -->
        <section class="review-section">
            <h2 class="review-heading">Đánh giá sản phẩm</h2>

            <div class="review-summary">
                <div class="review-summary-score">
                    <asp:Label ID="lblTrungBinh" runat="server" CssClass="review-summary-number" Text="4.5"></asp:Label>
                    <div class="review-stars">
                        <asp:Literal ID="litStars" runat="server"></asp:Literal>
                    </div>
                    <asp:Label ID="lblSoLuongDanhGia" runat="server" CssClass="review-summary-count" Text="Dựa trên 12 đánh giá"></asp:Label>
                </div>
            </div>

                    <asp:Repeater ID="rptReviews" runat="server">
                        <ItemTemplate>
                            <article class="review-card">
                                <div class="review-card-header">
                                    <div class="review-user">
                                        <span class="review-avatar"><%# Eval("Initial") %></span>
                                        <div class="review-user-info">
                                            <span class="review-name"><%# Eval("TenNguoiDung") %></span>
                                            <div class="review-stars-inline">
                                                <%# Eval("Stars") %>
                                            </div>
                                        </div>
                                    </div>
                                    <span class="review-date"><%# Eval("NgayDanhGia") %></span>
                                </div>
                                <p class="review-text"><%# Eval("NoiDung") %></p>
                            </article>
                        </ItemTemplate>
                    </asp:Repeater>
        </section>

    </section>

    <script>
    function switchCover(btn, url) {
        var main = document.querySelector('.cover-main');
        if (main && url) {
            main.src = url;
            document.querySelectorAll('.thumb-btn').forEach(function(b) {
                b.classList.remove('active');
            });
            btn.classList.add('active');
        }
    }

    function showTab(tabId, btn) {
        document.querySelectorAll('.tab-content').forEach(function(el) {
            el.classList.remove('active');
        });
        document.querySelectorAll('.tab-btn').forEach(function(b) {
            b.classList.remove('active');
        });
        var target = document.getElementById('tab-' + tabId);
        if (target) target.classList.add('active');
        if (btn) btn.classList.add('active');
    }
    </script>
</asp:Content>
