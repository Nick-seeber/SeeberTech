namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TestTable")]
    public partial class TestTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TestID { get; set; }

        [Required]
        [StringLength(10)]
        public string TestName { get; set; }
    }
}
