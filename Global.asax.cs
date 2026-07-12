using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace WBS_BTL
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            DataService.Initialize();
            Application["DanhSachSach"] = DataService.GetSach();
            Application["DanhSachTaiKhoan"] = DataService.GetTaiKhoan();
            Application["DanhSachDonHang"] = DataService.GetDonHang();
            Application["SoNguoiTruyCap"] = 0;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
        }

        protected void Application_End(object sender, EventArgs e)
        {
            DataService.SaveSach(Application["DanhSachSach"] as List<Sach> ?? new List<Sach>());
            DataService.SaveTaiKhoan(Application["DanhSachTaiKhoan"] as List<Account> ?? new List<Account>());
            DataService.SaveDonHang(Application["DanhSachDonHang"] as List<DonHangItem> ?? new List<DonHangItem>());
        }
    }
}
