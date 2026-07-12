using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace WBS_BTL
{
    public class TimKiemGoiY : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json; charset=utf-8";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            string keyword = context.Request.QueryString["keyword"] ?? string.Empty;

            if (string.IsNullOrWhiteSpace(keyword) || keyword.Trim().Length < 2)
            {
                context.Response.Write("[]");
                return;
            }

            var ds = context.Application["DanhSachSach"] as List<Sach>;
            if (ds == null)
            {
                context.Response.Write("[]");
                return;
            }

            var kw = keyword.Trim().ToLowerInvariant();
            var results = ds
                .Where(s =>
                    (s.TenSach ?? string.Empty).ToLowerInvariant().Contains(kw) ||
                    (s.TacGia ?? string.Empty).ToLowerInvariant().Contains(kw))
                .Take(8)
                .Select(s => new
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    TacGia = s.TacGia ?? string.Empty,
                    TheLoai = s.TheLoai ?? string.Empty,
                    Gia = s.Gia,
                    Anh = s.Anh ?? string.Empty
                })
                .ToList();

            var serializer = new JavaScriptSerializer();
            context.Response.Write(serializer.Serialize(results));
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
