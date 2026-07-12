<%@ Page Title="Tài khoản cá nhân" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="TaiKhoanCaNhan.aspx.cs" Inherits="WBS_BTL.TaiKhoanCaNhan" %>

<asp:Content ID="ContentAccount" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/TaiKhoan.css" />

    <div class="page-account">
        <div class="container">
            <h1 class="page-title">Tài khoản của bạn</h1>

            <div class="account-layout">
                <!-- Khu vực 1: Thông tin tài khoản (bên trái) -->
                <aside class="account-card">
                    <div class="avatar-wrapper">
                        <div class="avatar">
                            <span class="avatar-text">
                                <asp:Label ID="lblAvatarChu" runat="server" Text="?"></asp:Label>
                            </span>
                        </div>
                    </div>

                    <div class="account-info">
                        <h2 class="account-name">
                            <asp:Label ID="lblHoTen" runat="server" Text="Khách hàng"></asp:Label>
                        </h2>
                        <div class="info-list">
                            <div class="info-item">
                                <span class="info-icon">&#9993;</span>
                                <span class="info-text"><asp:Label ID="lblEmail" runat="server" Text="—"></asp:Label></span>
                            </div>
                            <div class="info-item">
                                <span class="info-icon">&#9742;</span>
                                <span class="info-text"><asp:Label ID="lblSoDienThoai" runat="server" Text="—"></asp:Label></span>
                            </div>
                            <div class="info-item">
                                <span class="info-icon">&#127968;</span>
                                <span class="info-text"><asp:Label ID="lblDiaChi" runat="server" Text="—"></asp:Label></span>
                            </div>
                        </div>
                    </div>

                    <div class="card-actions">
                        <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật thông tin"
                            CssClass="btn btn-update" OnClick="btnCapNhat_Click" />
                        <asp:Button ID="btnDangXuat" runat="server" Text="Đăng xuất" CssClass="btn btn-ghost" OnClick="btnDangXuat_Click" />
                    </div>
                </aside>

                <!-- Khu vực 2: Lịch sử đơn hàng & Form cập nhật (bên phải) -->
                <section class="main-content">
                    
                    <!-- Lịch sử đơn hàng -->
                    <div id="orderSection" runat="server">
                        <div class="order-section">
                            <h2 class="section-heading">Lịch sử đơn hàng</h2>

                            <asp:Panel ID="pnlEmptyOrders" runat="server" CssClass="order-empty" Visible="false">
                                <div class="empty-icon">&#128218;</div>
                                <p>Bạn chưa có đơn hàng nào tại BookVerse.</p>
                                <a href="TrangChu.aspx" class="btn btn-primary">Khám phá sách ngay</a>
                            </asp:Panel>

                            <div class="orders-list">
                                <asp:Repeater ID="rptDonHang" runat="server">
                                    <ItemTemplate>
                                        <div class="order-card">
                                            <div class="order-header">
                                                <div class="order-title">
                                                    <span class="order-id">#<%# Eval("MaDon") %></span>
                                                    <span class="order-date"><%# Eval("NgayDat") %></span>
                                                </div>
                                                <span class="order-status status-<%# Eval("TrangThaiCode") %>"><%# Eval("TrangThai") %></span>
                                            </div>

                                            <div class="order-body">
                                                <div class="order-row">
                                                    <span class="order-label">Tổng tiền</span>
                                                    <span class="order-value"><%# Eval("TongTien", "{0:N0}₫") %></span>
                                                </div>
                                                <div class="order-row">
                                                    <span class="order-label">Phương thức thanh toán</span>
                                                    <span class="order-value"><%# Eval("PhuongThuc") %></span>
                                                </div>
                                                <div class="order-items" style="margin-top:16px; border-top:1px dashed #ddd; padding-top:12px;">
                                                    <asp:Repeater ID="rptChiTiet" runat="server" DataSource='<%# Eval("ChiTiet") %>'>
                                                        <ItemTemplate>
                                                            <div class="order-item">
                                                                <span class="item-name">&#8226; <%# Eval("TenSach") %> (x<%# Eval("SoLuong") %>)</span>
                                                                <span class="item-price"><%# Eval("ThanhTien", "{0:N0}₫") %></span>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <!-- Form cập nhật - thay thế lịch sử đơn hàng -->
                    <div id="editSection" runat="server" class="profile-edit-form" visible="false">
                        <div class="form-card">
                            <div class="form-card-header">
                                <h2 class="form-card-title">Cập nhật thông tin</h2>
                                <p class="form-card-subtitle">Thay đổi thông tin cá nhân của bạn</p>
                            </div>
                            <div class="form-card-body">
                                <div class="field-group">
                                    <label for="<%= txtHoTen.ClientID %>">Họ tên</label>
                                    <asp:TextBox ID="txtHoTen" runat="server" placeholder="Nhập họ tên của bạn" />
                                    <span class="field-message" id="errHoTen">Họ tên không hợp lệ</span>
                                </div>

                                <div class="field-group">
                                    <label for="<%= txtEmail.ClientID %>">Email</label>
                                    <asp:TextBox ID="txtEmail" runat="server" placeholder="vidu@email.com" TextMode="Email" autocomplete="off" />
                                    <span class="field-message" id="errEmail">Email không hợp lệ</span>
                                </div>

                                <div class="field-group">
                                    <label for="<%= txtSoDienThoai.ClientID %>">Số điện thoại</label>
                                    <asp:TextBox ID="txtSoDienThoai" runat="server" placeholder="VD: 0912345678" TextMode="Phone" />
                                    <span class="field-message" id="errSoDienThoai">Số điện thoại không hợp lệ (9-11 chữ số)</span>
                                </div>

                                <div class="field-group">
                                    <label for="<%= txtDiaChi.ClientID %>">Địa chỉ</label>
                                    <asp:TextBox ID="txtDiaChi" runat="server" placeholder="VD: 123 Nguyễn Trãi, P.5, Q.1, TP.HCM" TextMode="MultiLine" Rows="2" />
                                </div>

                                <div class="form-buttons">
                                    <asp:Button ID="btnHuy" runat="server" Text="Hủy" CssClass="btn-cancel" OnClick="btnHuy_Click" />
                                    <asp:Button ID="btnLuu" runat="server" Text="Lưu thay đổi" CssClass="btn-save" OnClientClick="return validateProfile();" OnClick="btnLuu_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>

    <script>
        function validateProfile() {
            var sdt = document.getElementById('<%= txtSoDienThoai.ClientID %>').value.trim();
            var errSdt = document.getElementById('errSoDienThoai');
            
            if (sdt.length > 0) {
                var sdtRegex = /^0\d{8,10}$/;
                if (!sdtRegex.test(sdt)) {
                    errSdt.style.display = 'block';
                    return false;
                }
            }
            
            errSdt.style.display = 'none';
            return true;
        }
    </script>
</asp:Content>
