namespace WebApplication1.Models
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class media_info
    {
        public int id { get; set; }

        [StringLength(4000)]
        public string url { get; set; }

        [StringLength(4000)]
        public string imdb_code { get; set; }

        [StringLength(4000)]
        public string title { get; set; }

        [StringLength(4000)]
        public string title_english { get; set; }

        [StringLength(4000)]
        public string title_long { get; set; }

        [StringLength(4000)]
        public string slug { get; set; }

        public string year { get; set; }

        [StringLength(4000)]
        public string rating { get; set; }

        public string runtime { get; set; }

        [StringLength(4000)]
        public string summary { get; set; }

        [StringLength(4000)]
        public string description_full { get; set; }

        [StringLength(4000)]
        public string synopsis { get; set; }

        [StringLength(4000)]
        public string yt_trailer_code { get; set; }

        [StringLength(4000)]
        public string language { get; set; }

        [StringLength(4000)]
        public string mpa_rating { get; set; }

        [StringLength(4000)]
        public string background_image { get; set; }

        [StringLength(4000)]
        public string small_cover_image { get; set; }

        [StringLength(4000)]
        public string medium_cover_image { get; set; }

        [StringLength(4000)]
        public string large_cover_image { get; set; }

        [StringLength(4000)]
        public string state { get; set; }

        [StringLength(4000)]
        public string torrents { get; set; }

        [StringLength(4000)]
        public string date_uploaded { get; set; }

        public DateTime date_updated { get; set; }

    }
}
