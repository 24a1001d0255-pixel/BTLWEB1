<%@ Page Title="Đăng nhập" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DangNhap.aspx.cs" Inherits="WBS_BTL.DangNhap" %>

<asp:Content ID="ContentLogin" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/Auth.css" />

    <div class="page-auth">
        <div class="auth-card">
            <div class="auth-header">
                <h1 class="auth-title">Chào mừng trở lại BookVerse</h1>
                <p class="auth-subtitle">Đăng nhập để quản lý đơn hàng và tiếp tục mua sắm.</p>
            </div>

            <div class="auth-body">
                <div class="form-group">
                    <label for="<%= txtEmail.ClientID %>">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" placeholder="vidu@email.com" TextMode="Email" autocomplete="off"></asp:TextBox>
                    <span class="field-error" id="errEmail">Email không hợp lệ.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtMatKhau.ClientID %>">Mật khẩu</label>
                    <asp:TextBox ID="txtMatKhau" runat="server" CssClass="input-control" placeholder="Nhập mật khẩu" TextMode="Password" autocomplete="new-password" readonly="true" onfocus="this.removeAttribute('readonly');"></asp:TextBox>
                    <span class="field-error" id="errMatKhau">Vui lòng nhập mật khẩu.</span>
                </div>

                <asp:Button ID="btnDangNhap" runat="server" Text="Đăng nhập"
                    CssClass="btn btn-primary btn-full"
                    OnClientClick="return validateLoginForm();"
                    OnClick="btnDangNhap_Click" />

                <p class="auth-footer-text">
                    Chưa có tài khoản?
                    <asp:HyperLink ID="lnkToDangKy" runat="server" NavigateUrl="DangKy.aspx" CssClass="auth-link">Đăng ký ngay</asp:HyperLink>
                </p>
            </div>
        </div>
    </div>

    <script>
        function validateLoginForm() {
            var email = document.getElementById('<%= txtEmail.ClientID %>').value.trim();
            var matKhau = document.getElementById('<%= txtMatKhau.ClientID %>').value;
            var valid = true;

            if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
                document.getElementById('errEmail').innerText = "Email không hợp lệ.";
                document.getElementById('errEmail').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errEmail').style.display = 'none';
            }

            if (matKhau.length === 0) {
                document.getElementById('errMatKhau').style.display = 'block';
                valid = false;
            } else {
                document.getElementById('errMatKhau').style.display = 'none';
            }

            return valid;
        }
    </script>
</asp:Content>
