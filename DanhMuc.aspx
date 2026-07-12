<%@ Page Title="Danh mục" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DanhMuc.aspx.cs" Inherits="WBS_BTL.DanhMuc" %>

<asp:Content ID="ContentCategory" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ProductList.css" />

    <div class="page-catalog">
        <div class="container">
            <h1 class="page-title">Khám phá theo danh mục</h1>
            <p class="page-subtitle">Duyệt sách theo thể loại, tác giả hoặc nhà xuất bản yêu thích.</p>

            <!-- KHU VỰC 1: THỂ LOẠI -->
            <section class="category-section">
                <h2 class="category-heading">Thể loại</h2>
                <asp:Repeater ID="rptTheLoai" runat="server">
                    <ItemTemplate>
                        <a href='<%# Eval("Url") %>' class="category-card">
                            <span class="category-icon">📂</span>
                            <span class="category-name"><%# Eval("Ten") %></span>
                            <span class="category-count"><%# Eval("SoLuong") %> cuốn</span>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </section>

            <!-- KHU VỰC 2: TÁC GIẢ -->
            <section class="category-section">
                <h2 class="category-heading">Tác giả</h2>
                <asp:Repeater ID="rptTacGia" runat="server">
                    <ItemTemplate>
                        <a href='<%# Eval("Url") %>' class="category-card author-card">
                            <span class="category-icon">✍️</span>
                            <span class="category-name"><%# Eval("Ten") %></span>
                            <span class="category-count"><%# Eval("SoLuong") %> cuốn</span>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </section>

            <!-- KHU VỰC 3: NHÀ XUẤT BẢN -->
            <section class="category-section">
                <h2 class="category-heading">Nhà xuất bản</h2>
                <asp:Repeater ID="rptNhaXuatBan" runat="server">
                    <ItemTemplate>
                        <a href='<%# Eval("Url") %>' class="category-card publisher-card">
                            <span class="category-icon">🏢</span>
                            <span class="category-name"><%# Eval("Ten") %></span>
                            <span class="category-count"><%# Eval("SoLuong") %> cuốn</span>
                        </a>
                    </ItemTemplate>
                </asp:Repeater>
            </section>
        </div>
    </div>
</asp:Content>
