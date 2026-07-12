<%@ Page Title="Chi tiết khách hàng" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ChiTietKhachHang.aspx.cs" Inherits="WBS_BTL.ChiTietKhachHang" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/AdminCustomerDetail.css" />

    <section class="admin-customer-detail">
        <div class="container">
            <!-- Header -->
            <div class="page-header">
                <a href="QuanLy.aspx" class="btn-back">
                    <span>&#8592;</span> Quay lại danh sách
                </a>
                <h1 class="page-title">Chi tiết khách hàng</h1>
            </div>

            <div class="detail-grid">
                <!-- Left Column -->
                <div class="detail-left">
                    <!-- Thong tin ca nhan -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Thông tin cá nhân</h2>
                        </div>
                        <div class="card-body">
                            <div class="customer-avatar-section">
                                <div class="avatar-wrapper">
                                    <img id="imgAvatar" runat="server" src="~/images/default-avatar.png" alt="Avatar" class="customer-avatar" />
                                </div>
                            </div>
                            <div class="info-grid">
                                <div class="info-item">
                                    <label class="info-label">Họ và tên</label>
                                    <span class="info-value" id="lblHoTen" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Email</label>
                                    <span class="info-value" id="lblEmail" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Số điện thoại</label>
                                    <span class="info-value" id="lblSoDienThoai" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Giới tính</label>
                                    <span class="info-value" id="lblGioiTinh" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Ngày sinh</label>
                                    <span class="info-value" id="lblNgaySinh" runat="server">-</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Thong tin tai khoan -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Thông tin tài khoản</h2>
                        </div>
                        <div class="card-body">
                            <div class="info-grid">
                                <div class="info-item">
                                    <label class="info-label">Trạng thái</label>
                                    <span class="info-value">
                                        <span id="lblTrangThai" runat="server" class="badge badge-ok">Hoạt động</span>
                                    </span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Ngày đăng ký</label>
                                    <span class="info-value" id="lblNgayDangKy" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Lần đăng nhập cuối</label>
                                    <span class="info-value" id="lblLanDangNhapCuoi" runat="server">-</span>
                                </div>
                                <div class="info-item">
                                    <label class="info-label">Hạng thành viên</label>
                                    <span class="info-value">
                                        <span id="lblHangThanhVien" runat="server" class="membership-badge">Khách hàng mới</span>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Dia chi -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Địa chỉ</h2>
                        </div>
                        <div class="card-body">
                            <div id="pnlDiaChiMacDinh" runat="server" class="address-section">
                                <label class="info-label">Địa chỉ mặc định</label>
                                <p class="address-text" id="lblDiaChiMacDinh" runat="server">Chưa cập nhật</p>
                            </div>
                            <div id="pnlDiaChiKhac" runat="server" class="address-section" visible="false">
                                <label class="info-label">Địa chỉ khác</label>
                                <p class="address-text" id="lblDiaChiKhac" runat="server">-</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Right Column -->
                <div class="detail-right">
                    <!-- Thong ke mua hang -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Thống kê mua hàng</h2>
                        </div>
                        <div class="card-body">
                            <div class="stats-grid">
                                <div class="stat-item">
                                    <span class="stat-number" id="lblTongDonHang" runat="server">0</span>
                                    <span class="stat-label">Đơn hàng đã đặt</span>
                                </div>
                                <div class="stat-item">
                                    <span class="stat-number" id="lblTongChiTieu" runat="server">0đ</span>
                                    <span class="stat-label">Tổng chi tiêu</span>
                                </div>
                                <div class="stat-item">
                                    <span class="stat-number" id="lblDonThanhCong" runat="server">0</span>
                                    <span class="stat-label">Đơn thành công</span>
                                </div>
                                <div class="stat-item">
                                    <span class="stat-number" id="lblDonHuy" runat="server">0</span>
                                    <span class="stat-label">Đơn đã hủy</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Lich su don hang -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Đơn hàng gần đây</h2>
                        </div>
                        <div class="card-body">
                            <div class="orders-list" id="pnlDonHangList" runat="server">
                                <asp:Repeater ID="rptDonHang" runat="server">
                                    <ItemTemplate>
                                        <div class="order-item">
                                            <div class="order-info">
                                                <span class="order-id">#<%# Eval("MaDon") %></span>
                                                <span class="order-date"><%# Eval("NgayDat") %></span>
                                            </div>
                                            <div class="order-details">
                                                <span class="order-total"><%# Eval("TongTien", "{0:N0}") %>đ</span>
                                                <span class='order-status <%# GetStatusClass(Eval("TrangThaiCode").ToString()) %>'><%# Eval("TrangThai") %></span>
                                            </div>
                                            <a href='DonHangChiTiet.aspx?id=<%# Eval("MaDon") %>' class="btn-view-order">Xem</a>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div id="pnlNoOrders" runat="server" class="no-data" visible="false">
                                    <p>Khách hàng chưa có đơn hàng nào.</p>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Thao tac quan tri -->
                    <div class="detail-card">
                        <div class="card-header">
                            <h2 class="card-title">Thao tác quản trị</h2>
                        </div>
                        <div class="card-body">
                            <div class="admin-actions">
                                <asp:Button ID="btnKhoaTaiKhoan" runat="server" CssClass="btn btn-danger"
                                    Text="Khóa tài khoản" OnClick="btnKhoaTaiKhoan_Click" />
                                <asp:Button ID="btnMoKhoaTaiKhoan" runat="server" CssClass="btn btn-success"
                                    Text="Mở khóa tài khoản" OnClick="btnMoKhoaTaiKhoan_Click" Visible="false" />
                                <asp:Button ID="btnDatLaiMatKhau" runat="server" CssClass="btn btn-warning"
                                    Text="Đặt lại mật khẩu" OnClick="btnDatLaiMatKhau_Click" />
                            </div>
                            <div id="pnlActionMessage" runat="server" class="action-message" visible="false">
                                <asp:Literal ID="litActionMessage" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
