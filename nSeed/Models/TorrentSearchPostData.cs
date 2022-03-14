using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace nSeed.Models
{
    public class TorrentSearchPostData
    {
        public string? nyit_filmek_resz { get; set; }
        public string? nyit_sorozat_resz { get; set; }
        public string? nyit_zene_resz { get; set; }
        public string? nyit_xxx_resz { get; set; }
        public string? nyit_jatek_resz { get; set; }
        public string? nyit_prog_resz { get; set; }
        public string? nyit_konyv_resz { get; set; }
        public string[]? kivalasztott_tipus { get; set; }
        public string mire { get; set; }
        public string miben { get; set; }
        public string tipus { get; set; }
        [FromForm(Name = "submit.x")]
        public int submit_x { get; set; }
        [FromForm(Name = "submit.y")]
        public int submit_y { get; set; }
        public string tags { get; set; }
        public string inverz_tags { get; set; }
        public NameValueCollection GetPostParametersDict()
        {
            var dict = new NameValueCollection
            {
                {"mire", mire},
                {"miben", miben},
                {"tipus", tipus},
                {"submit.x", submit_x.ToString()}, // todo what this
                {"submit.y", submit_y.ToString()}, // todo what this
                {"tags", tags}
            };
            if (kivalasztott_tipus != null) { dict.Add("kivalasztott_tipus[]", string.Join(",", kivalasztott_tipus)); }
            if (nyit_filmek_resz != null) { dict.Add("nyit_filmek_resz", nyit_filmek_resz); }
            if (nyit_sorozat_resz != null) { dict.Add("nyit_sorozat_resz", nyit_sorozat_resz);}
            if (nyit_zene_resz != null) { dict.Add("nyit_zene_resz", nyit_zene_resz); }
            if (nyit_xxx_resz != null) { dict.Add("nyit_xxx_resz", nyit_xxx_resz); }
            if (nyit_jatek_resz != null) { dict.Add("nyit_jatek_resz", nyit_jatek_resz); }
            if (nyit_prog_resz != null) { dict.Add("nyit_prog_resz", nyit_prog_resz); }
            if (inverz_tags != null) { dict.Add("inverz_tags", inverz_tags); }

            return dict;
        }
        public override string ToString()
        {
            string ret = "";
            foreach (PropertyInfo propertyInfo in this.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(this, null) != null)
                    ret += (propertyInfo.Name + "::" + propertyInfo.GetValue(this, null) + "\n");
            }
            return ret;
        }
        // todo potentional improvement in the future:
        //  miszerint = name, fid, size, times_compleeted, seeders, leechers, 
        //  hogyan = ASC, DESC
        //  (tipus?)

        // submit.x, submit.y   0-85,0-35, kep koordinatai ha input type image van...
    }
}
