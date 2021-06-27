namespace FinalFantasu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        public int? idFigure { get; set; }

        [StringLength(256)]
        public string Url { get; set; }

        [Key]
        public int IDImage { get; set; }

        public virtual Figure Figure { get; set; }
    }
}
