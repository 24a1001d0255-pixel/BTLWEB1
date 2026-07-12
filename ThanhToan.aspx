<%@ Page Title="Thanh toán" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="ThanhToan.aspx.cs" Inherits="WBS_BTL.ThanhToan" %>

<asp:Content ID="ContentCheckout" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/ThanhToan.css" />

    <div class="page-checkout">
        <div class="container">
            <h1 class="page-title">Thanh toán đơn hàng</h1>


            <!--   GIỎ TRỐNG   -->
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="checkout-empty" Visible="false">
                <div class="empty-icon">📦</div>
                <h2 class="empty-heading">Giỏ hàng của bạn đang trống</h2>
                <p class="empty-desc">Quay lại mua sắm để chọn thêm sách hay nhé.</p>
                <a href="TrangChu.aspx" class="btn btn-continue">Tiếp tục mua sắm</a>
            </asp:Panel>

            <!--   FORM THANH TOÁN   -->
            <asp:Panel ID="pnlCheckout" runat="server">
                <div class="checkout-layout">
                    <!-- Cột trái: Form thông tin -->
                    <div class="checkout-form">
                        <h2 class="section-heading">Thông tin giao hàng</h2>

                        <div class="form-group">
                            <label for="<%= txtHoTen.ClientID %>">Họ tên khách hàng <span class="required">*</span></label>
                            <asp:TextBox ID="txtHoTen" runat="server" CssClass="input-control" placeholder="Nhập họ tên người nhận"></asp:TextBox>
                            <span class="field-error" id="errHoTen">Vui lòng nhập họ tên.</span>
                        </div>

                        <div class="form-group">
                            <label for="<%= txtSoDienThoai.ClientID %>">Số điện thoại <span class="required">*</span></label>
                            <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="input-control" placeholder="Ví dụ: 0909 123 456"></asp:TextBox>
                            <span class="field-error" id="errSoDienThoai">Số điện thoại không hợp lệ.</span>
                        </div>

                        <div class="form-group">
                            <label for="<%= txtDiaChi.ClientID %>">Địa chỉ giao hàng <span class="required">*</span></label>
                            <asp:TextBox ID="txtDiaChi" runat="server" CssClass="input-control" placeholder="Số nhà, đường, quận/huyện, tỉnh/thành"></asp:TextBox>
                            <span class="field-error" id="errDiaChi">Vui lòng nhập địa chỉ giao hàng.</span>
                        </div>

                        <div class="form-group">
                            <label for="<%= txtGhiChu.ClientID %>">Ghi chú đơn hàng</label>
                            <asp:TextBox ID="txtGhiChu" runat="server" TextMode="MultiLine" CssClass="input-control input-textarea" placeholder="Ghi chú thêm cho người bán hoặc shipper..."></asp:TextBox>
                        </div>

                        <fieldset class="payment-fieldset">
                            <legend>Phương thức thanh toán</legend>
                            <asp:RadioButtonList ID="rblPayment" runat="server" CssClass="payment-options">
                                <asp:ListItem Text="COD - Thanh toán khi nhận hàng" Value="COD" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Chuyển khoản ngân hàng" Value="BankTransfer"></asp:ListItem>
                                <asp:ListItem Text="Ví điện tử" Value="EWallet"></asp:ListItem>
                            </asp:RadioButtonList>
                        </fieldset>

                        <div class="form-actions">
                            <asp:Button ID="btnDatHang" runat="server" Text="Xác nhận đặt hàng"
                                CssClass="btn btn-place-order"
                                OnClientClick="return validateCheckoutForm();"
                                OnClick="btnDatHang_Click" />
                        </div>
                    </div>

                    <!-- Cột phải: Tóm tắt đơn hàng -->
                    <aside class="order-summary">
                        <h2 class="section-heading">Đơn hàng của bạn</h2>

                        <div class="summary-items">
                            <asp:Repeater ID="rptDonHang" runat="server">
                                <ItemTemplate>
                                    <div class="summary-item">
                                        <div class="item-info">
                                            <span class="item-name"><%# HttpUtility.HtmlEncode(Eval("TenSach").ToString()) %></span>
                                            <span class="item-qty">x<%# Eval("SoLuong") %></span>
                                        </div>
                                        <span class="item-total"><%# Eval("ThanhTien", "{0:N0}₫") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="summary-divider"></div>

                        <div class="summary-row">
                            <span class="summary-label">Tạm tính</span>
                            <span class="summary-value"><asp:Label ID="lblTamTinh" runat="server" Text="0₫"></asp:Label></span>
                        </div>
                        <div class="summary-row">
                            <span class="summary-label">Phí vận chuyển</span>
                            <span class="summary-value">30.000₫</span>
                        </div>
                        <div class="summary-row summary-row-total">
                            <span class="summary-label">Tổng thanh toán</span>
                            <span class="summary-value"><asp:Label ID="lblTongThanhToan" runat="server" CssClass="total-amount" Text="0₫"></asp:Label></span>
                        </div>

                        <p class="summary-note">Đơn hàng sẽ được xử lý sau khi xác nhận.</p>
                    </aside>
                </div>
            </asp:Panel>

            <!--   THÀNH CÔNG   -->
            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="checkout-success">
                <div class="success-card">
                    <div class="success-icon">✅</div>
                    <h2 class="success-title">Đặt hàng thành công!</h2>
                    <p class="success-desc">Cảm ơn bạn đã mua sách tại BookVerse.</p>

                    <div class="success-meta">
                        <div class="meta-row">
                            <span>Mã đơn hàng</span>
                            <strong><asp:Label ID="lblMaDon" runat="server" Text=""></asp:Label></strong>
                        </div>
                        <div class="meta-row">
                            <span>Thời gian giao dự kiến</span>
                            <strong><asp:Label ID="lblGiaoDuKien" runat="server" Text=""></asp:Label></strong>
                        </div>
                    </div>

                    <div class="success-actions">
                        <a href="TrangChu.aspx" class="btn btn-primary">Tiếp tục mua sắm</a>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <script>
        function validateCheckoutForm() {
            var hoTen = document.getElementById('<%= txtHoTen.ClientID %>').value.trim();
            var diaChi = document.getElementById('<%= txtDiaChi.ClientID %>').value.trim();
            var sdt = document.getElementById('<%= txtSoDienThoai.ClientID %>').value.trim();

            var valid = true;

            if (hoTen.length < 2) {
                document.getElementById('errHoTen').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errHoTen').style.display = 'none';
            }

            if (!/^[0-9]{9,11}$/.test(sdt.replace(/\s/g, ''))) {
                document.getElementById('errSoDienThoai').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errSoDienThoai').style.display = 'none';
            }

            if (diaChi.length < 5) {
                document.getElementById('errDiaChi').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errDiaChi').style.display = 'none';
            }

            return valid;
        }
    </script>
</asp:Content>
