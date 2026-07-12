using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;

namespace WBS_BTL
{
    public partial class DanhMuc : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var ds = Application["DanhSachSach"] as List<Sach>;
                if (ds == null || ds.Count == 0)
                {
                    return;
                }

                rptTheLoai.DataSource = BuildGroup(ds, s => s.TheLoai, "theloai");
                rptTheLoai.DataBind();

                rptTacGia.DataSource = BuildGroup(ds, s => s.TacGia, "tacgia");
                rptTacGia.DataBind();

                rptNhaXuatBan.DataSource = BuildGroup(ds, s => s.NhaXuatBan, "nxb");
                rptNhaXuatBan.DataBind();
            }
        }

        private List<dynamic> BuildGroup(List<Sach> ds, Func<Sach, string> selector, string type)
        {
            return ds
                .Where(s => !string.IsNullOrWhiteSpace(selector(s)))
                .GroupBy(selector)
                .Select(g => new
                {
                    Ten = g.Key,
                    SoLuong = g.Count(),
                    Url = "~/DanhSachSp.aspx?type=" + type + "&value=" + Server.UrlEncode(g.Key)
                })
                .OrderBy(x => x.Ten)
                .Cast<dynamic>()
                .ToList();
        }
    }
}
