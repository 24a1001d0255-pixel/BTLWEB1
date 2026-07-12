using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WBS_BTL
{
    public partial class GioHang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HienThiGioHang();
            }
        }

        private void HienThiGioHang()
        {
            var gioHang = Session["giohang"] as List<GioHangItem>;

            if (gioHang != null && gioHang.Count > 0)
            {
                pnlCart.Visible = true;
                pnlEmpty.Visible = false;

                rptGioHang.DataSource = gioHang;
                rptGioHang.DataBind();

                double tamTinh = gioHang.Sum(x => x.ThanhTien);
                lblTamTinh.Text = tamTinh.ToString("N0") + "₫";
                lblTongTien.Text = tamTinh.ToString("N0") + "₫";
            }
            else
            {
                pnlCart.Visible = false;
                pnlEmpty.Visible = true;
            }
        }

        protected void btnSoLuong_Command(object sender, CommandEventArgs e)
        {
            var gioHang = Session["giohang"] as List<GioHangItem>;
            if (gioHang == null || gioHang.Count == 0)
            {
                HienThiGioHang();
                return;
            }

            string maSach = e.CommandArgument.ToString();
            string lenh = e.CommandName;
            var sach = gioHang.FirstOrDefault(x => string.Equals(x.MaSach, maSach, StringComparison.OrdinalIgnoreCase));

            if (sach == null)
            {
                HienThiGioHang();
                return;
            }

            if (lenh == "Tang")
            {
                sach.SoLuong++;
            }
            else if (lenh == "Giam")
            {
                if (sach.SoLuong > 1)
                {
                    sach.SoLuong--;
                }
                else
                {
                    gioHang.Remove(sach);
                }
            }
            else if (lenh == "Xoa")
            {
                gioHang.Remove(sach);
            }

            Session["giohang"] = gioHang;
            var m = this.Master as Site;
            m?.UpdateCartCountFromSession();
            HienThiGioHang();
        }

        protected void btnXoaHet_Click(object sender, EventArgs e)
        {
            Session["giohang"] = new List<GioHangItem>();
            var m = this.Master as Site;
            m?.UpdateCartCountFromSession();
            HienThiGioHang();
        }

        protected void btnThanhToan_Click(object sender, EventArgs e)
        {
            var gioHang = Session["giohang"] as List<GioHangItem>;
            if (gioHang == null || gioHang.Count == 0)
            {
                return;
            }

            Response.Redirect("ThanhToan.aspx");
        }
    }
}
