<%@ Page Title="Giỏ hàng" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="GioHang.aspx.cs" Inherits="WBS_BTL.GioHang" %>

<asp:Content ID="ContentGioHang" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/GioHang.css" />

    <div class="page-cart">
        <div class="container">
            <h1 class="page-title">Giỏ hàng của bạn</h1>

            <!--    TRẠNG THÁI TRỐNG    -->
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="cart-empty" Visible="false">
                <div class="empty-icon">📚</div>
                <h2 class="empty-heading">Giỏ hàng của bạn đang trống</h2>
                <p class="empty-desc">Hãy tìm cho mình một cuốn sách hay để bắt đầu hành trình đọc nhé!</p>
                <a href="TrangChu.aspx" class="btn btn-continue">Tiếp tục mua sắm</a>
            </asp:Panel>

            <!--    GIỎ HÀNG CÓ SẢN PHẨM    -->
            <asp:Panel ID="pnlCart" runat="server" CssClass="cart-layout">
                <!-- DANH SÁCH SẢN PHẨM -->
                <div class="cart-items">
                    <div class="cart-table-wrap">
                        <table class="cart-table">
                            <thead>
                                <tr>
                                    <th class="col-image">Bìa sách</th>
                                    <th class="col-info">Thông tin</th>
                                    <th class="col-price">Đơn giá</th>
                                    <th class="col-qty">Số lượng</th>
                                    <th class="col-total">Thành tiền</th>
                                    <th class="col-action">Xóa</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptGioHang" runat="server">
                                    <ItemTemplate>
                                        <tr class="cart-row">
                                            <td class="col-image">
                                                <a href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>'>
                                                    <img class="item-cover" src='<%# ResolveUrl(Eval("Anh").ToString()) %>'
                                                        alt='<%# HttpUtility.HtmlEncode(Eval("TenSach").ToString()) %>' />
                                                </a>
                                            </td>
                                            <td class="col-info">
                                                <a class="item-title" href='ChiTietSanPham.aspx?id=<%# Eval("MaSach") %>'>
                                                    <%# HttpUtility.HtmlEncode(Eval("TenSach").ToString()) %>
                                                </a>
                                            </td>
                                            <td class="col-price">
                                                <span class="price"><%# Eval("Gia", "{0:N0}₫") %></span>
                                            </td>
                                            <td class="col-qty">
                                                <div class="qty-control">
                                                    <asp:Button ID="btnGiam" runat="server" Text="−"
                                                        CssClass="qty-btn qty-btn-minus"
                                                        CommandName="Giam"
                                                        CommandArgument='<%# Eval("MaSach") %>'
                                                        OnCommand="btnSoLuong_Command" />
                                                    <span class="qty-display"><%# Eval("SoLuong") %></span>
                                                    <asp:Button ID="btnTang" runat="server" Text="+"
                                                        CssClass="qty-btn qty-btn-plus"
                                                        CommandName="Tang"
                                                        CommandArgument='<%# Eval("MaSach") %>'
                                                        OnCommand="btnSoLuong_Command" />
                                                </div>
                                            </td>
                                            <td class="col-total">
                                                <span class="item-total"><%# Eval("ThanhTien", "{0:N0}₫") %></span>
                                            </td>
                                            <td class="col-action">
                                                <asp:Button ID="btnXoa" runat="server" Text="🗑"
                                                    CssClass="btn-delete"
                                                    CommandName="Xoa"
                                                    CommandArgument='<%# Eval("MaSach") %>'
                                                    OnCommand="btnSoLuong_Command" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                    <div class="cart-footer-actions">
                        <a href="TrangChu.aspx" class="btn btn-continue-outline">← Tiếp tục mua sắm</a>
                        <asp:Button ID="btnXoaHet" runat="server" Text="Xóa toàn bộ"
                            CssClass="btn btn-clear" OnClick="btnXoaHet_Click" />
                    </div>
                </div>

                <!-- TỔNG QUAN ĐƠN HÀNG -->
                <aside class="order-summary">
                    <h2 class="summary-heading">Tổng quan đơn hàng</h2>

                    <div class="summary-row">
                        <span class="summary-label">Tạm tính</span>
                        <span class="summary-value"><asp:Label ID="lblTamTinh" runat="server" Text="0₫"></asp:Label></span>
                    </div>
                    <div class="summary-row">
                        <span class="summary-label">Phí vận chuyển</span>
                        <span class="summary-value"><asp:Label ID="lblPhiVanChuyen" runat="server" Text="Miễn phí"></asp:Label></span>
                    </div>
                    <div class="summary-divider"></div>
                    <div class="summary-row summary-row-total">
                        <span class="summary-label">Tổng tiền</span>
                        <span class="summary-value"><asp:Label ID="lblTongTien" runat="server" Text="0₫" CssClass="total-amount"></asp:Label></span>
                    </div>

                    <asp:Button ID="btnThanhToan" runat="server" Text="Tiến hành thanh toán"
                        CssClass="btn btn-checkout" OnClick="btnThanhToan_Click" />

                    <p class="summary-note">Đơn hàng sẽ được xử lý sau khi xác nhận thanh toán.</p>
                </aside>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
