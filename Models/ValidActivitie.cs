using System.ComponentModel.DataAnnotations;
using System;

namespace BeltLogin.Models
{
    public class ValidActivitie: BaseEntity
    {
        [Required]
        [MinLength(2)]
        [RegularExpression(@"^[0-9A-Za-z ]+")]
        public string Title { get; set; }

        [Required]
        [MinLength(10)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        // [MinLength(1)]
        [RegularExpression(@"^-?[0-9]*$")]
        public int Duration { get; set; }

        public int CreatorId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        public string DurationMod { get; set; }
    }
}