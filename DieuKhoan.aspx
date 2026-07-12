<%@ Page Title="Điều khoản & Hỗ trợ" Language="C#" MasterPageFile="~/Site.master"
    AutoEventWireup="true" CodeBehind="DieuKhoan.aspx.cs" Inherits="WBS_BTL.DieuKhoan" %>

<asp:Content ID="ContentTerms" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="Styles/DieuKhoan.css" />

    <div class="page-terms">
        <div class="container">
            <div class="terms-layout">
                <aside class="terms-sidebar">
                    <h2 class="sidebar-title">Mục lục</h2>
                    <nav class="terms-nav" id="termsNav">
                        <a href="#chinh-sach-doi-tra" class="terms-nav-link">Chính sách đổi trả</a>
                        <a href="#chinh-sach-van-chuyen" class="terms-nav-link">Chính sách vận chuyển</a>
                        <a href="#huong-dan-mua-hang" class="terms-nav-link">Hướng dẫn mua hàng</a>
                    </nav>
                </aside>

                <article class="terms-content">
                    <header class="terms-header">
                        <h1 class="terms-title">Điều khoản & Hỗ trợ</h1>
                        <p class="terms-intro">Tài liệu dành cho cộng đồng yêu sách của BookVerse. Chúng tôi cam kết mang đến trải nghiệm mua sắm minh bạch, an toàn và thân thiện.</p>
                    </header>

                    <section id="chinh-sach-doi-tra" class="terms-section">
                        <h2 class="terms-section-title">1. Chính sách đổi trả</h2>
                        <p>BookVerse hỗ trợ đổi trả 1-1 miễn phí trong vòng <strong>7 ngày</strong> kể từ ngày nhận hàng nếu sách thuộc một trong các trường hợp sau:</p>
                        <ul>
                            <li>Sách bị lỗi in ấn: lỗi chính tả nghiêm trọng, in mờ không đọc được, hoặc thiếu phần nội dung do nhà sản xuất gây ra.</li>
                            <li>Sách bị rách trang, bong tróc gáy, gãy đóng gáy hoặc hư hỏng cấu trúc trong quá trình vận chuyển.</li>
                            <li>Sách thiếu trang, thiếu bảng minh họa, hoặc giao nhầm mã sách so với đơn hàng.</li>
                        </ul>
                        <p>Để yêu cầu đổi trả, bạn vui lòng giữ nguyên sách, bao bì và phụ kiện đi kèm, sau đó gửi thông tin qua trang <strong>Hỗ trợ</strong> hoặc email hỗ trợ của chúng tôi. BookVerse sẽ xác nhận và hướng dẫn cách gửi sách về kho trong thời gian sớm nhất.</p>
                    </section>

                    <section id="chinh-sach-van-chuyen" class="terms-section">
                        <h2 class="terms-section-title">2. Chính sách vận chuyển</h2>
                        <p>Chúng tôi hợp tác với các đơn vị vận chuyển uy tín để đảm bảo sách đến tay bạn đúng thời hạn và trong tình trạng tốt nhất.</p>

                        <div class="info-grid">
                            <div class="info-item">
                                <h3>Thời gian giao hàng dự kiến</h3>
                                <ul>
                                    <li>Nội thành Hà Nội, TP.HCM: <strong>1-2 ngày làm việc</strong>.</li>
                                    <li>Các tỉnh thành khác: <strong>2-5 ngày làm việc</strong>.</li>
                                    <li>Khu vực huyện, xã vùng sâu vùng xa: <strong>3-7 ngày làm việc</strong>.</li>
                                </ul>
                            </div>
                            <div class="info-item">
                                <h3>Miễn phí vận chuyển</h3>
                                <ul>
                                    <li>Miễn phí cho đơn hàng từ <strong>300.000₫</strong> trở lên.</li>
                                    <li>Áp dụng toàn quốc cho phương thức giao hàng tiêu chuẩn.</li>
                                    <li>Không áp dụng cho đơn hàng quốc tế hoặc kênh giao hàng nhanh đặc biệt.</li>
                                </ul>
                            </div>
                        </div>

                        <p>Sau khi đặt hàng, bạn sẽ nhận được mã theo dõi để theo dõi trạng thái vận chuyển. Nếu có phát sinh về thời gian giao, BookVerse sẽ chủ động liên hệ và đề xuất phương án phù hợp.</p>
                    </section>

                    <section id="huong-dan-mua-hang" class="terms-section">
                        <h2 class="terms-section-title">3. Hướng dẫn mua hàng</h2>
                        <p>Quy trình mua sách tại BookVerse được thiết kế đơn giản, nhanh chóng và rõ ràng:</p>

                        <ol class="guide-steps">
                            <li>
                                <h3>Bước 1: Tìm kiếm sách</h3>
                                <p>Sử dụng ô tìm kiếm trên Header, duyệt theo <strong>Danh mục</strong> hoặc khám phá mục <strong>Sách nổi bật</strong> ở Trang chủ để chọn cuốn sách phù hợp.</p>
                            </li>
                            <li>
                                <h3>Bước 2: Thêm vào giỏ hàng</h3>
                                <p>Mở trang <strong>Chi tiết sách</strong>, kiểm tra thông tin tác giả, giá bán và tình trạng kho, sau đó bấm nút <strong>Thêm vào giỏ hàng</strong>.</p>
                            </li>
                            <li>
                                <h3>Bước 3: Điền thông tin đơn hàng</h3>
                                <p>Vào trang <strong>Giỏ hàng</strong>, kiểm tra lại danh sách sách, chọn phương thức thanh toán và nhập đầy đủ thông tin giao hàng cần thiết.</p>
                            </li>
                            <li>
                                <h3>Bước 4: Xác nhận đơn hàng</h3>
                                <p>Kiểm tra lại đơn hàng và bấm <strong>Xác nhận</strong>. Bạn sẽ nhận được mã đơn hàng để theo dõi trạng thái xử lý và vận chuyển.</p>
                            </li>
                        </ol>
                    </section>

                    <footer class="terms-footer">
                        <p>Nếu bạn cần hỗ trợ thêm, vui lòng liên hệ với chúng tôi qua kênh hỗ trợ trong mục Liên hệ. BookVerse luôn sẵn sàng lắng nghe và hỗ trợ bạn.</p>
                    </footer>
                </article>
            </div>
        </div>
    </div>

    <script>
        document.querySelectorAll('.terms-nav-link').forEach(function (link) {
            link.addEventListener('click', function (e) {
                e.preventDefault();
                var target = document.querySelector(this.getAttribute('href'));
                if (!target) return;
                target.scrollIntoView({ behavior: 'smooth', block: 'start' });
            });
        });
    </script>
</asp:Content>
