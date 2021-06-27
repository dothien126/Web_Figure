namespace FinalFantasu.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FigureCategory")]
    public partial class FigureCategory
    {
        public int? idFigure { get; set; }

        public int? idCategory { get; set; }

        public int ID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Figure Figure { get; set; }
    }
}
