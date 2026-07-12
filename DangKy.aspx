<%@ Page Title="Đăng ký" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DangKy.aspx.cs" Inherits="WBS_BTL.DangKy" %>

<asp:Content ID="ContentRegister" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/Auth.css" />

    <div class="page-auth">
        <div class="auth-card">
            <div class="auth-header">
                <h1 class="auth-title">Tạo tài khoản BookVerse</h1>
                <p class="auth-subtitle">Đăng ký để theo dõi đơn hàng và nhận ưu đãi sách hay.</p>
            </div>

            <div class="auth-body">
                <div id="divThongBao" runat="server" class="alert alert-danger" visible="false" style="display:none;">
                    <asp:Literal ID="litThongBao" runat="server"></asp:Literal>
                </div>

                <div class="form-group">
                    <label for="<%= txtHoTen.ClientID %>">Họ tên <span class="required">*</span></label>
                    <asp:TextBox ID="txtHoTen" runat="server" CssClass="input-control" placeholder="Nhập họ tên của bạn"></asp:TextBox>
                    <span class="field-error" id="errHoTen">Vui lòng nhập họ tên (tối thiểu 2 ký tự).</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtEmail.ClientID %>">Email <span class="required">*</span></label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input-control" placeholder="vidu@email.com" TextMode="Email" autocomplete="off"></asp:TextBox>
                    <span class="field-error" id="errEmail">Email không hợp lệ.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtSoDienThoai.ClientID %>">Số điện thoại <span class="required">*</span></label>
                    <asp:TextBox ID="txtSoDienThoai" runat="server" CssClass="input-control" placeholder="Nhập số điện thoại (VD: 0912345678)" TextMode="Phone"></asp:TextBox>
                    <span class="field-error" id="errSoDienThoai">Số điện thoại phải là 9-11 chữ số, bắt đầu bằng 0 (VD: 0912345678).</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtMatKhau.ClientID %>">Mật khẩu <span class="required">*</span></label>
                    <asp:TextBox ID="txtMatKhau" runat="server" CssClass="input-control" placeholder="Tối thiểu 6 ký tự" TextMode="Password" autocomplete="new-password" readonly="true" onfocus="this.removeAttribute('readonly');"></asp:TextBox>
                    <span class="field-error" id="errMatKhau">Mật khẩu phải từ 6 ký tự trở lên.</span>
                </div>

                <div class="form-group">
                    <label for="<%= txtNhapLaiMatKhau.ClientID %>">Nhập lại mật khẩu <span class="required">*</span></label>
                    <asp:TextBox ID="txtNhapLaiMatKhau" runat="server" CssClass="input-control" placeholder="Nhập lại mật khẩu" TextMode="Password" autocomplete="new-password" readonly="true" onfocus="this.removeAttribute('readonly');"></asp:TextBox>
                    <span class="field-error" id="errNhapLai">Mật khẩu không trùng khớp.</span>
                </div>

                <asp:Button ID="btnDangKy" runat="server" Text="Đăng ký"
                    CssClass="btn btn-primary btn-full"
                    OnClientClick="return validateRegisterForm();"
                    OnClick="btnDangKy_Click" />

                <p class="auth-footer-text">
                    Đã có tài khoản?
                    <asp:HyperLink ID="lnkToDangNhap" runat="server" NavigateUrl="DangNhap.aspx" CssClass="auth-link">Đăng nhập ngay</asp:HyperLink>
                </p>
            </div>
        </div>
    </div>

    <script>
        function validateRegisterForm() {
            var hoTen = document.getElementById('<%= txtHoTen.ClientID %>').value.trim();
            var email = document.getElementById('<%= txtEmail.ClientID %>').value.trim();
            var soDienThoai = document.getElementById('<%= txtSoDienThoai.ClientID %>').value.trim();
            var matKhau = document.getElementById('<%= txtMatKhau.ClientID %>').value;
            var nhapLai = document.getElementById('<%= txtNhapLaiMatKhau.ClientID %>').value;

            var valid = true;
            var errors = [];

            // Validate HoTen
            if (hoTen.length < 2) {
                document.getElementById('errHoTen').style.display = 'block';
                valid = false;
                errors.push('Họ tên phải từ 2 ký tự trở lên.');
            } else {
                document.getElementById('errHoTen').style.display = 'none';
            }

            // Validate Email
            var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(email)) {
                document.getElementById('errEmail').style.display = 'block';
                valid = false;
                errors.push('Email không hợp lệ.');
            } else {
                document.getElementById('errEmail').style.display = 'none';
            }

            // Validate SoDienThoai (9-11 so, bat dau bang 0)
            var sdtRegex = /^0\d{8,10}$/;
            if (!sdtRegex.test(soDienThoai)) {
                document.getElementById('errSoDienThoai').style.display = 'block';
                valid = false;
                errors.push('Số điện thoại phải là 10-11 số, bắt đầu bằng số 0.');
            } else {
                document.getElementById('errSoDienThoai').style.display = 'none';
            }

            // Validate MatKhau
            if (matKhau.length < 6) {
                document.getElementById('errMatKhau').style.display = 'block';
                valid = false;
                errors.push('Mật khẩu phải từ 6 ký tự trở lên.');
            } else {
                document.getElementById('errMatKhau').style.display = 'none';
            }

            // Validate NhapLai
            if (matKhau !== nhapLai || nhapLai.length === 0) {
                document.getElementById('errNhapLai').style.display = 'block';
                valid = false;
                errors.push('Mật khẩu nhập lại không trùng khớp.');
            } else {
                document.getElementById('errNhapLai').style.display = 'none';
            }

            // Hien thi thong bao loi tong hop
            if (!valid && errors.length > 0) {
                var errDiv = document.getElementById('<%= divThongBao.ClientID %>');
                var errLit = document.getElementById('<%= litThongBao.ClientID %>');
                if (errLit) {
                    errLit.innerHTML = errors.join('<br/>');
                }
                if (errDiv) {
                    errDiv.style.display = 'block';
                    errDiv.scrollIntoView({ behavior: 'smooth', block: 'center' });
                }
            }

            return valid;
        }

        // An thong bao khi nguoi dung bat dau nhap
        document.addEventListener('DOMContentLoaded', function() {
            var inputs = document.querySelectorAll('.input-control');
            inputs.forEach(function(input) {
                input.addEventListener('input', function() {
                    var errDiv = document.getElementById('<%= divThongBao.ClientID %>');
                    if (errDiv) errDiv.style.display = 'none';
                });
            });
        });
    </script>
</asp:Content>
