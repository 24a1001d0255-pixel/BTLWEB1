<%@ Page Title="Quản trị hệ thống" Language="C#" AutoEventWireup="true" CodeBehind="QuanLy.aspx.cs" Inherits="WBS_BTL.QuanLy" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title><%: Page.Title %></title>
    <link rel="stylesheet" href="../Styles/style.css" />
    <link rel="stylesheet" href="Admin.css" />
</head>
<body>
    <form id="formAdmin" runat="server">
        <div class="admin-layout">
            <aside class="admin-sidebar">
                <div class="admin-brand">BookVerse Admin</div>
                <nav class="admin-nav">
                    <asp:LinkButton ID="lnkTabSach" runat="server" CssClass="admin-nav-item active" OnClick="SwitchTab_Click" CommandArgument="sach">Quản lý sách</asp:LinkButton>
                    <asp:LinkButton ID="lnkTabDonHang" runat="server" CssClass="admin-nav-item" OnClick="SwitchTab_Click" CommandArgument="donhang">Quản lý đơn hàng</asp:LinkButton>
                    <asp:LinkButton ID="lnkTabKhachHang" runat="server" CssClass="admin-nav-item" OnClick="SwitchTab_Click" CommandArgument="khachhang">Quản lý khách hàng</asp:LinkButton>
                    <asp:LinkButton ID="lnkThoat" runat="server" CssClass="admin-nav-item" PostBackUrl="~/TrangChu.aspx">Thoát quản trị</asp:LinkButton>
                </nav>
            </aside>

            <main class="admin-main">
                <header class="admin-topbar">
                    <h1 class="admin-page-title">Dashboard</h1>
                    <asp:Label ID="lblAdminInfo" runat="server" CssClass="admin-badge" Text="Admin"></asp:Label>
                </header>

                <!-- TAB SACH -->
                <asp:Panel ID="pnlTabSach" runat="server" CssClass="admin-tab">
                    <div class="toolbar">
                        <asp:Label ID="lblBookCount" runat="server" CssClass="admin-count" Text="0 cuốn"></asp:Label>
                        <asp:Button ID="btnThemSach" runat="server" Text="+ Thêm sách mới" CssClass="btn btn-primary btn-sm" OnClick="btnThemSach_Click" />
                    </div>

                    <asp:Panel ID="pnlBookEditor" runat="server" CssClass="book-editor-section" Visible="false">
                        <div class="book-editor">
                            <div class="book-editor-header">
                                <h3>Thông tin sách</h3>
                                <asp:Button ID="btnCloseInlineEditor" runat="server" Text="Đóng" CssClass="btn btn-ghost btn-sm" OnClick="btnCloseInlineEditor_Click" />
                            </div>

                            <!-- HÀNG 1: Mã sách + Tên sách -->
                            <div class="form-row">
                                <div class="form-group">
                                    <label>Mã sách</label>
                                    <asp:TextBox ID="txtMaSach" runat="server" CssClass="form-control" placeholder="Tự sinh nếu để trống" />
                                </div>
                                <div class="form-group form-group-fill">
                                    <label>Tên sách <span class="required">*</span></label>
                                    <asp:TextBox ID="txtTenSach" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <!-- HÀNG 2: Tác giả + NXB -->
                            <div class="form-row">
                                <div class="form-group">
                                    <label>Tác giả</label>
                                    <asp:TextBox ID="txtTacGia" runat="server" CssClass="form-control" />
                                </div>
                                <div class="form-group">
                                    <label>Nhà xuất bản</label>
                                    <asp:TextBox ID="txtNxb" runat="server" CssClass="form-control" />
                                </div>
                            </div>

                            <!-- HÀNG 3: Giá + Số trang + Tình trạng (nhóm ngang) -->
                            <div class="form-row form-row-tight">
                                <div class="form-group">
                                    <label>Giá (VND) <span class="required">*</span></label>
                                    <asp:TextBox ID="txtGia" runat="server" CssClass="form-control" TextMode="Number" placeholder="VD: 89000" />
                                </div>
                                <div class="form-group">
                                    <label>Số trang</label>
                                    <asp:TextBox ID="txtSoTrang" runat="server" CssClass="form-control" TextMode="Number" placeholder="VD: 320" />
                                </div>
                                <div class="form-group">
                                    <label>Tình trạng</label>
                                    <asp:DropDownList ID="ddlTinhTrang" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Còn hàng" Value="Còn hàng" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Đã đặt" Value="Đã đặt"></asp:ListItem>
                                        <asp:ListItem Text="Hết hàng" Value="Hết hàng"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="form-group">
                                    <label>Thể loại</label>
                                    <asp:TextBox ID="txtTheLoai" runat="server" CssClass="form-control" placeholder="VD: Văn học" />
                                </div>
                            </div>

                            <!-- HÀNG 4: Upload ảnh -->
                            <div class="form-section-title">Hình ảnh sách</div>
                            <div class="form-row">
                                <!-- Ảnh bìa chính -->
                                <div class="form-group">
                                    <label>Ảnh bìa chính</label>
                                    <div class="upload-zone upload-zone-main" id="uploadZoneMain" onclick="document.getElementById('<%= fuAnhBia.ClientID %>').click()">
                                        <asp:FileUpload ID="fuAnhBia" runat="server" CssClass="file-input-hidden" accept="image/*" onchange="previewMainImage(this)" />
                                        <div class="upload-placeholder" id="uploadPlaceholderMain">
                                            <span class="upload-icon">+</span>
                                            <span class="upload-text">Chọn ảnh bìa chính</span>
                                        </div>
                                        <div class="upload-preview" id="previewMain">
                                            <img id="previewMainImg" src="" alt="Ảnh bìa" />
                                            <button type="button" class="btn-remove-img" onclick="event.stopPropagation(); removeMainImage()">&#x2715;</button>
                                        </div>
                                    </div>
                                    <asp:HiddenField ID="hfAnhCu" runat="server" />
                                </div>

                                <!-- Ảnh minh họa -->
                                <div class="form-group">
                                    <label>Ảnh minh họa <span class="label-hint">(tối đa 4 ảnh)</span></label>
                                    <div class="upload-strip" id="uploadStrip">
                                        <asp:FileUpload ID="fuAnh1" runat="server" CssClass="file-input-hidden" accept="image/*" onchange="previewThumb(this, 0)" />
                                        <asp:FileUpload ID="fuAnh2" runat="server" CssClass="file-input-hidden" accept="image/*" onchange="previewThumb(this, 1)" />
                                        <asp:FileUpload ID="fuAnh3" runat="server" CssClass="file-input-hidden" accept="image/*" onchange="previewThumb(this, 2)" />
                                        <asp:FileUpload ID="fuAnh4" runat="server" CssClass="file-input-hidden" accept="image/*" onchange="previewThumb(this, 3)" />
                                        <asp:HiddenField ID="hfAnhMinhHoaCu" runat="server" />
                                        <asp:HiddenField ID="hfAnhMinhHoaUrl" runat="server" />

                                        <div class="thumb-slot" id="thumbSlot0" onclick="document.getElementById('<%= fuAnh1.ClientID %>').click()">
                                            <span class="thumb-plus">+</span>
                                        </div>
                                        <div class="thumb-slot" id="thumbSlot1" onclick="document.getElementById('<%= fuAnh2.ClientID %>').click()">
                                            <span class="thumb-plus">+</span>
                                        </div>
                                        <div class="thumb-slot" id="thumbSlot2" onclick="document.getElementById('<%= fuAnh3.ClientID %>').click()">
                                            <span class="thumb-plus">+</span>
                                        </div>
                                        <div class="thumb-slot" id="thumbSlot3" onclick="document.getElementById('<%= fuAnh4.ClientID %>').click()">
                                            <span class="thumb-plus">+</span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- HÀNG 5: Tóm tắt -->
                            <div class="form-group form-group-full">
                                <label>Tóm tắt / Mô tả sách</label>
                                <asp:TextBox ID="txtMoTa" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Nhập tóm tắt nội dung sách..."></asp:TextBox>
                            </div>

                            <!-- Action buttons -->
                            <div class="book-editor-footer">
                                <asp:Button ID="btnLuuSach" runat="server" Text="Lưu thông tin" CssClass="btn btn-primary" OnClick="btnLuuSach_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                    <!-- Bảng sách -->
                    <asp:GridView ID="gvSach" runat="server" AutoGenerateColumns="False" OnRowCommand="gvSach_RowCommand"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvSach_PageIndexChanging"
                        CssClass="table table-striped table-hover table-bordered admin-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="MaSach" HeaderText="Mã" />
                            <asp:TemplateField HeaderText="Ảnh">
                                <ItemTemplate>
                                    <img src='<%# ResolveAdminCoverUrl(Eval("Anh").ToString()) %>' class="admin-thumb" alt='<%# Eval("TenSach") %>' onerror="this.src='../images/default.jpg'" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="TenSach" HeaderText="Tên sách" />
                            <asp:BoundField DataField="TacGia" HeaderText="Tác giả" />
                            <asp:BoundField DataField="Gia" HeaderText="Giá" DataFormatString="{0:N0}VND" />
                            <asp:BoundField DataField="SoTrang" HeaderText="Số trang" />
                            <asp:TemplateField HeaderText="Tình trạng">
                                <ItemTemplate>
                                    <span class='<%# Eval("TinhTrangKho").ToString() == "Còn hàng" ? "badge badge-ok" : "badge badge-warn" %>'><%# Eval("TinhTrangKho") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chức năng">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Sửa" CssClass="btn btn-warning btn-sm" CommandName="EditBook" CommandArgument='<%# Eval("MaSach") %>' />
                                    <asp:Button ID="btnDelete" runat="server" Text="Xóa" CssClass="btn btn-danger btn-sm" CommandName="DeleteBook" CommandArgument='<%# Eval("MaSach") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <!-- TAB DON HANG -->
                <asp:Panel ID="pnlTabDonHang" runat="server" CssClass="admin-tab" Visible="false">
                    <div class="toolbar">
                        <asp:Label ID="lblOrderCount" runat="server" CssClass="admin-count" Text="0 đơn"></asp:Label>
                    </div>

                    <asp:GridView ID="gvDonHang" runat="server" AutoGenerateColumns="False" OnRowCommand="gvDonHang_RowCommand"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvDonHang_PageIndexChanging"
                        CssClass="table table-striped table-hover table-bordered admin-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="MaDon" HeaderText="Mã đơn" />
                            <asp:BoundField DataField="KhachHang" HeaderText="Khách hàng" />
                            <asp:BoundField DataField="NgayDat" HeaderText="Ngày đặt" />
                            <asp:BoundField DataField="TongTien" HeaderText="Tổng tiền" DataFormatString="{0:N0}VND" />
                            <asp:BoundField DataField="PhuongThuc" HeaderText="Phương thức" />
                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <span class='<%# GetStatusClass(Eval("TrangThaiCode").ToString()) %>'><%# Eval("TrangThai") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cập nhật">
                                <ItemTemplate>
                                    <asp:DropDownList ID="ddlTrangThai" runat="server" CssClass="form-select form-select-sm d-inline-block w-auto">
                                        <asp:ListItem Value="dangxuly" Text="Đang xử lý"></asp:ListItem>
                                        <asp:ListItem Value="danggiao" Text="Đang giao hàng"></asp:ListItem>
                                        <asp:ListItem Value="dagiao" Text="Đã giao"></asp:ListItem>
                                        <asp:ListItem Value="dahuy" Text="Đã hủy"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:Button ID="btnCapNhat" runat="server" Text="Cập nhật" CssClass="btn btn-success btn-sm" CommandName="UpdateStatus" CommandArgument='<%# Eval("MaDon") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <!-- TAB KHACH HANG -->
                <asp:Panel ID="pnlTabKhachHang" runat="server" CssClass="admin-tab" Visible="false">
                    <div class="toolbar">
                        <asp:Label ID="lblKhachHangCount" runat="server" CssClass="admin-count" Text="0 khách hàng"></asp:Label>
                    </div>

                    <asp:GridView ID="gvKhachHang" runat="server" AutoGenerateColumns="False" OnRowCommand="gvKhachHang_RowCommand"
                        AllowPaging="True" PageSize="10" OnPageIndexChanging="gvKhachHang_PageIndexChanging"
                        CssClass="table table-striped table-hover table-bordered admin-table" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="TenDangNhap" HeaderText="Tên khách hàng" />
                            <asp:BoundField DataField="Email" HeaderText="Email" />
                            <asp:BoundField DataField="SoDienThoai" HeaderText="Số điện thoại" />
                            <asp:BoundField DataField="DiaChi" HeaderText="Địa chỉ" />
                            <asp:TemplateField HeaderText="Trạng thái">
                                <ItemTemplate>
                                    <span class='<%# Eval("TrangThai").ToString() == "HoatDong" ? "badge badge-ok" : "badge badge-danger" %>'>
                                        <%# Eval("TrangThai").ToString() == "HoatDong" ? "Hoạt động" : "Vô hiệu hóa" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Chức năng">
                                <ItemTemplate>
                                    <div class="action-buttons">
                                        <a href='ChiTietKhachHang.aspx?email=<%# HttpUtility.UrlEncode(Eval("Email").ToString()) %>' class="btn btn-info btn-sm">Xem chi tiết</a>
                                        <asp:Button ID="btnDisable" runat="server" Text="Vô hiệu hóa" CssClass="btn btn-danger btn-sm"
                                            CommandName="DisableAccount" CommandArgument='<%# Eval("Email") %>'
                                            Visible='<%# Eval("TrangThai").ToString() == "HoatDong" && Eval("VaiTro").ToString() != "Admin" %>' />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </main>
        </div>

        <script>
        // --- Main image preview ---
        function previewMainImage(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    var img = document.getElementById('previewMainImg');
                    var placeholder = document.getElementById('uploadPlaceholderMain');
                    img.src = e.target.result;
                    img.style.display = 'block';
                    if (placeholder) placeholder.style.display = 'none';
                    document.getElementById('previewMain').style.display = 'block';
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function removeMainImage() {
            var input = document.getElementById('<%= fuAnhBia.ClientID %>');
            if (input) input.value = '';
            var img = document.getElementById('previewMainImg');
            var placeholder = document.getElementById('uploadPlaceholderMain');
            var preview = document.getElementById('previewMain');
            if (img) img.src = '';
            if (placeholder) placeholder.style.display = 'flex';
            if (preview) preview.style.display = 'none';
        }

        // --- Thumbnail preview ---
        var _thumbUrls = ['', '', '', ''];

        function previewThumb(input, index) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function(e) {
                    _thumbUrls[index] = e.target.result;
                    updateThumbSlot(index, e.target.result);
                };
                reader.readAsDataURL(input.files[0]);
            }
        }

        function updateThumbSlot(index, dataUrl) {
            var slot = document.getElementById('thumbSlot' + index);
            if (!slot) return;
            slot.innerHTML = '<img src="' + dataUrl + '" class="thumb-img" alt="Anh ' + (index + 1) + '" />' +
                '<button type="button" class="btn-remove-thumb" onclick="event.stopPropagation(); clearThumb(' + index + ');">&times;</button>';
            slot.classList.add('has-image');
        }

        function clearThumb(index) {
            var inputs = [
                document.getElementById('<%= fuAnh1.ClientID %>'),
                document.getElementById('<%= fuAnh2.ClientID %>'),
                document.getElementById('<%= fuAnh3.ClientID %>'),
                document.getElementById('<%= fuAnh4.ClientID %>')
            ];
            if (inputs[index]) inputs[index].value = '';
            _thumbUrls[index] = '';
            var slot = document.getElementById('thumbSlot' + index);
            if (slot) {
                slot.innerHTML = '<span class="thumb-plus">+</span>';
                slot.classList.remove('has-image');
            }
        }

        // --- Restore existing images on page load ---
        function resolveAdminUrl(path) {
            if (!path) return '';
            if (path.indexOf('images/') === 0) return '../' + path;
            if (path.indexOf('/') === 0 || path.indexOf('~/') === 0) return path;
            return '../images/' + path;
        }

        function restoreExistingImages(mainUrl, thumbs) {
            if (mainUrl) {
                var img = document.getElementById('previewMainImg');
                var placeholder = document.getElementById('uploadPlaceholderMain');
                var preview = document.getElementById('previewMain');
                if (img) { img.src = resolveAdminUrl(mainUrl); img.style.display = 'block'; }
                if (placeholder) placeholder.style.display = 'none';
                if (preview) preview.style.display = 'block';
            }
        }

        // Run on page load
        document.addEventListener('DOMContentLoaded', function() {
            var storedMain = document.getElementById('<%= hfAnhCu.ClientID %>');
            var storedThumbs = document.getElementById('<%= hfAnhMinhHoaUrl.ClientID %>');
            if (storedMain && storedMain.value) {
                restoreExistingImages(storedMain.value, []);
            }
            if (storedThumbs && storedThumbs.value) {
                try {
                    var arr = JSON.parse(storedThumbs.value);
                    arr.forEach(function(url, i) {
                        if (url && url.trim()) {
                            var slot = document.getElementById('thumbSlot' + i);
                            if (slot) {
                                slot.innerHTML = '<img src="' + resolveAdminUrl(url) + '" class="thumb-img" alt="Anh ' + (i + 1) + '" />' +
                                    '<button type="button" class="btn-remove-thumb" onclick="event.stopPropagation(); clearThumb(' + i + ');">&times;</button>';
                                slot.classList.add('has-image');
                                _thumbUrls[i] = url;
                            }
                        }
                    });
                } catch(e) {}
            }
        });
        </script>
    </form>
</body>
</html>
