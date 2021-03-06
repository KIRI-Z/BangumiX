﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BangumiX.Model
{
    public class SubjectProgress
    {
        public uint subject_id { get; set; }
        public List<EpProgress> eps { get; set; }
    }
    public class EpProgress
    {
        public uint id { get; set; }
        public EpStatus status { get; set; }
    }
    public class EpStatus
    {
        public uint id { get; set; }
        public string css_name { get; set; }
        public string url_name { get; set; }
        public string cn_name { get; set; }
    }

    public class SubjectSmall : Common.ObservableViewModelBase
    {
        public uint id { get; set; }
        public string url { get; set; }
        public int type { get; set; }

        //public string name { get; set; }
        private string _name { get; set; }
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == string.Empty) _name = null;
                else _name = value;
            }
        }

        //public string name_cn { get; set; }
        private string _name_cn { get; set; }
        public string name_cn
        {
            get
            {
                return _name_cn;
            }
            set
            {
                if (value == string.Empty) _name_cn = null;
                else _name_cn = value;
            }
        }

        public string summary { get; set; }
        public int eps { get; set; }
        public int eps_count { get; set; }
        public string air_date { get; set; }
        public int air_weekday { get; set; }
        public Rating rating { get; set; }
        public int rank { get; set; }
        public Dictionary<string, string> images { get; set; }
        public Dictionary<string, uint> collection { get; set; }

        public uint collection_total
        {
            get
            {
                return (uint)collection.Sum(x => x.Value);
            }
        }

        public string type_parsed
        {
            get
            {
                switch (type)
                {
                    case 1:
                        return "Book";
                    case 2:
                        return "Anime";
                    case 3:
                        return "Music";
                    case 4:
                        return "Game";
                    case 6:
                        return "Real";
                }
                return string.Empty;
            }
        }
        public string air_weekday_parsed
        {
            get
            {
                switch (air_weekday)
                {
                    case 1:
                        return "周一";
                    case 2:
                        return "周二";
                    case 3:
                        return "周三";
                    case 4:
                        return "周四";
                    case 5:
                        return "周五";
                    case 6:
                        return "周六";
                    case 7:
                        return "周日";
                }
                return string.Empty;
            }
        }
    }
    public class SubjectLarge : SubjectSmall
    {
        new public List<Episode> eps { get; set; }
        public List<Character> crt { get; set; }
        public List<Staff> staff { get; set; }
        public List<Topic> topic { get; set; }
        public List<Blog> blog { get; set; }

        public int eps_offset { get; set; }
        public List<Episode> eps_normal { get; set; }
        public List<Episode> eps_special { get; set; }
        public List<Episode> eps_sub { get; set; }
        public void eps_filter()
        {
            if (eps == null) return;
            eps_normal = new List<Episode>();
            eps_special = new List<Episode>();
            eps_sub = new List<Episode>();
            bool offset_flag = true;
            foreach (var e in eps)
            {
                if (e.type == 0)
                {
                    if (offset_flag)
                    {
                        eps_offset = Convert.ToInt16(e.sort);
                        eps_offset = eps_offset < 0 ? 0 : eps_offset;
                        offset_flag = false;
                    }
                    if (eps_sub.Count < 27)
                    {
                        eps_sub.Add(e);
                    }
                    eps_normal.Add(e);
                }
                else
                {
                    eps_special.Add(e);
                }
            }
            if (eps_sub.Count > 26)
            {
                eps_sub.Add(new Episode() { sort = "…", status = "Air" });
            }
            OnPropertyChanged("eps_sub");
            OnPropertyChanged("eps_normal");
            OnPropertyChanged("eps_special");

            button_count = new Dictionary<int, string>();
            int n = eps_normal.Count;
            int button_n = 0;
            while (n > 0)
            {
                int start = button_n * 100 + eps_offset;
                int end = n < 100 ? button_n * 100 + n + eps_offset - 1 : (button_n + 1) * 100 + eps_offset - 1;
                button_count.Add(button_n, string.Format("{0} - {1}", start, end));
                button_n += 1;
                n -= 100;
            }
            if (eps_special.Count > 0)
            {
                button_count.Add(button_n, "SP");
            }
            if (button_count.Count > 1) button_visibility = System.Windows.Visibility.Visible;
            else button_visibility = System.Windows.Visibility.Collapsed;
            OnPropertyChanged("button_visibility");
            OnPropertyChanged("button_count");
        }
        public System.Windows.Visibility button_visibility { get; set; }
        public Dictionary<int, string> button_count { get; set; }
    }
    public class Episode : Common.ObservableViewModelBase
    {
        public uint id { get; set; }
        public string url { get; set; }
        public int type { get; set; }
        public string sort { get; set; }

        //public string name { get; set; }
        private string _name { get; set; }
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == string.Empty) _name = null;
                else _name = value;
            }
        }

        //public string name_cn { get; set; }
        private string _name_cn { get; set; }
        public string name_cn
        {
            get
            {
                return _name_cn;
            }
            set
            {
                if (value == string.Empty) _name_cn = null;
                else _name_cn = value;
            }
        }

        public string full_name
        {
            get
            {
                string s = sort + ". ";
                if (name_cn != string.Empty) s = s + name_cn + " \\ ";
                s += name;
                return s;
            }
        }
        public System.Windows.Visibility full_name_flag
        {
            get
            {
                return full_name.Length > 43 ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            }
        }

        public string duration { get; set; }
        public string airdate { get; set; }
        public uint comment { get; set; }
        public string desc { get; set; }
        public string status { get; set; }
        //public uint ep_status { get; set; }
        public uint _ep_status { get; set; }
        public uint ep_status
        {
            get
            {
                return _ep_status;
            }
            set
            {
                _ep_status = value;
                OnPropertyChanged("ep_status");
            }
        }
    }
    public class Rating
    {
        public uint total { get; set; }
        public Dictionary<string, uint> count { get; set; }
        public Dictionary<string, float> _count_p { get; set; }
        public Dictionary<string, float> count_p
        {
            get
            {
                if (_count_p == null)
                {
                    _count_p = new Dictionary<string, float>();
                    foreach (var c in count.Keys)
                    {
                        _count_p.Add(c, (float)count[c] / (float)total * 100);
                    }
                }
                return _count_p;
            }
        }
        public float score { get; set; }
    }
    public class Character
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public string role_name { get; set; }
        public Dictionary<string, string> images { get; set; }
        public uint comment { get; set; }
        public uint collcts { get; set; }
        public Info info { get; set; }
        public List<Actor> actors { get; set; }
        public string ImageGrid
        {
            get
            {
                if (images == null) return "https://bangumi.tv/img/info_only.png";
                images.TryGetValue("grid", out string img);
                return img == null ? "https://bangumi.tv/img/info_only.png" : img;
            }
        }
    }
    public class Info
    {
        public dynamic birth { get; set; }
        public dynamic height { get; set; }
        public dynamic gender { get; set; }
        public dynamic alias { get; set; }
        public dynamic source { get; set; }
        public dynamic name_cn { get; set; }
        public dynamic cv { get; set; }
    }
    //public class Info
    //{
    //    public string birth { get; set; }
    //    public string height { get; set; }
    //    public string gender { get; set; }
    //    public Dictionary<string, string> alias { get; set; }
    //    public List<string> source { get; set; }
    //    public string name_cn { get; set; }
    //    public string cv { get; set; }
    //    public Actor[] actors { get; set; }
    //}
    public class Actor
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public Dictionary<string, string> images { get; set; }
        public string ImageGrid
        {
            get
            {
                images.TryGetValue("grid", out string img);
                return img == null ? "https://bangumi.tv/img/info_only.png" : img;
            }
        }
    }
    public class Staff : Character
    {
        public List<string> jobs { get; set; }
        public string Jobs
        {
            get
            {
                return string.Join(" ", jobs.ToArray());
            }
        }
    }
    public class Topic
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public uint main_id { get; set; }
        public ulong timestamp { get; set; }
        public ulong lastpost { get; set; }
        public int replies { get; set; }
        public User user { get; set; }
    }
    public class Blog
    {
        public uint id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string image { get; set; }
        public int replies { get; set; }
        public ulong timestamp { get; set; }
        public string dateline { get; set; }
        public User user { get; set; }
    }

}
