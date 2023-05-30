﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetWebApi.Models
{
    public class LinearSubItem
    {
        [Key]
        public int lsAutoId { get; set; }

        public string lsId { get; set; }

        public int laAutoId { get; set; }
        [ForeignKey("laAutoId")]
        public virtual LinearAssetModel linearAssetModel { get; set; }

        public  string location { get; set; }

        public string description { get; set; }

        public string companyId { get; set; }

    }
}
