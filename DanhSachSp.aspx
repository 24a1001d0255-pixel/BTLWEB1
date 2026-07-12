<%@ Page Title="Danh sách sản phẩm" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DanhSachSp.aspx.cs" Inherits="WBS_BTL.DanhSachSp" %>

<asp:Content ID="ContentProductList" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ProductList.css" />

    <div class="page-product-list">
        <div class="container">
            <div class="product-layout">
                <!-- Sidebar bộ lọc -->
                <aside class="filter-sidebar">
                    <div class="filter-header">
                        <h2 class="filter-title">Bộ lọc</h2>
                        <a id="lnkResetFilters" runat="server" href="DanhSachSp.aspx" class="filter-reset">Đặt lại</a>
                    </div>

                        <!-- Khoảng giá -->
                        <div class="filter-group">
                            <h3 class="filter-group-title">Khoảng giá</h3>
                            <asp:CheckBoxList ID="cblGia" runat="server" CssClass="filter-checkbox-list">
                                <asp:ListItem Text="Dưới 50.000VND" Value="duoi50"></asp:ListItem>
                                <asp:ListItem Text="50.000 - 100.000VND" Value="50-100"></asp:ListItem>
                                <asp:ListItem Text="100.000 - 200.000VND" Value="100-200"></asp:ListItem>
                                <asp:ListItem Text="Trên 200.000VND" Value="tren200"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>

                        <!-- Thể loại -->
                        <div class="filter-group">
                            <h3 class="filter-group-title">Thể loại</h3>
                            <asp:CheckBoxList ID="cblTheLoai" runat="server" CssClass="filter-checkbox-list">
                            </asp:CheckBoxList>
                        </div>

                        <!-- Tình trạng kho -->
                        <div class="filter-group">
                            <h3 class="filter-group-title">Tình trạng</h3>
                            <asp:CheckBoxList ID="cblKho" runat="server" CssClass="filter-checkbox-list">
                                <asp:ListItem Text="Còn hàng" Value="conhang"></asp:ListItem>
                                <asp:ListItem Text="Đã đặt" Value="dadat"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                        
                        <div class="filter-actions">
                            <asp:Button ID="btnApDungLoc" runat="server" Text="Áp dụng bộ lọc" CssClass="btn btn-primary w-100 mt-3" OnClick="btnApDungLoc_Click" />
                        </div>
                </aside>

                <!-- Vung san pham -->
                <section class="product-main">
                    <div class="product-header">
                        <h1 class="product-page-title">
                            <asp:Label ID="lblTieuDe" runat="server" Text="Tất cả sách"></asp:Label>
                        </h1>
                        <div class="product-header-row">
                            <span class="product-count"><asp:Label ID="lblSoLuong" runat="server" Text="0"></asp:Label> cuốn sách</span>
                            <div class="sort-wrap">
                                <label class="sort-label" for="ddlSort">Sắp xếp:</label>
                                <asp:DropDownList ID="ddlSort" runat="server" CssClass="sort-select"
                                    AutoPostBack="true" OnSelectedIndexChanged="Sort_Changed">
                                    <asp:ListItem Text="Mặc định" Value="default"></asp:ListItem>
                                    <asp:ListItem Text="Giá thấp - cao" Value="gia_asc"></asp:ListItem>
                                    <asp:ListItem Text="Giá cao - thấp" Value="gia_desc"></asp:ListItem>
                                    <asp:ListItem Text="Tên A - Z" Value="ten_asc"></asp:ListItem>
                                    <asp:ListItem Text="Tên Z - A" Value="ten_desc"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <!-- Active filter chips -->
                    <asp:Panel ID="activeFilters" runat="server" CssClass="active-filters" Visible="false">
                        <span class="active-filter-label">Lọc theo:</span>
                        <asp:Repeater ID="rptActiveFilters" runat="server">
                            <ItemTemplate>
                                <span class="filter-chip">
                                    <%# Eval("Text") %>
                                    <a href='<%# Eval("RemoveUrl") %>' class="chip-remove" title="Xóa lọc">&times;</a>
                                </span>
                            </ItemTemplate>
                        </asp:Repeater>
                    </asp:Panel>

                    <asp:Panel ID="pnlEmpty" runat="server" CssClass="product-empty" Visible="false">
                        <div class="empty-icon">📚</div>
                        <p>Không tìm thấy sách phù hợp với điều kiện lọc.</p>
                        <a href="DanhSachSp.aspx" class="btn btn-secondary">Xóa bỏ lọc</a>
                    </asp:Panel>

                    <div class="book-grid">
                        <asp:Repeater ID="rptSach" runat="server">
                            <ItemTemplate>
                                <article class="book-card">
                                    <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link">
                                        <img class="book-cover" src='<%# Eval("Anh") %>' alt='<%# Eval("TenSach") %>' />
                                    </a>
                                    <div class="book-body">
                                        <span class="book-category"><%# Eval("TheLoai") %></span>
                                        <h3 class="book-title">
                                            <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>' class="book-link"><%# Eval("TenSach") %></a>
                                        </h3>
                                        <p class="book-meta"><%# Eval("TacGia") %> &middot; <%# Eval("NhaXuatBan") %></p>
                                        <div class="book-price"><%# Eval("Gia", "{0:N0}VND") %></div>
                                        <asp:Button ID="btnThemVaoGio" runat="server" Text="Thêm vào giỏ"
                                            CommandArgument='<%# Eval("MaSach") %>'
                                            OnCommand="btnThemVaoGio_Command"
                                            CssClass="btn-add-cart" />
                                    </div>
                                </article>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- Phân trang -->
                    <asp:Panel ID="pnlPagination" runat="server" CssClass="pagination" Visible="false">
                        <a id="lnkPrev" runat="server" class="page-link">Trước</a>
                        <span class="page-info">Trang <asp:Label ID="lblPage" runat="server" Text="1"></asp:Label></span>
                        <a id="lnkNext" runat="server" class="page-link">Sau</a>
                    </asp:Panel>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
