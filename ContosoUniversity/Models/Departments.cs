﻿
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Department
    {
        [Key]

        public string? Instructor {  get; set; }
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "Money")]
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string? DepartmentOwner { get; set; } 

        public DateTime EndDate { get; set; } 

        public int? InstructorID { get; set; }

        [Timestamp]
        public byte? RowVersion { get; set; }

        public Instructor? Administrator { get; set; }

        public ICollection<Course>? Courses { get; set; }
    }
}
