using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BeltLogin.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltLogin.Models
{
    public class Activitie: BaseEntity
    {
        [Key]
        public int ActivitieId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime Time { get; set; }

        public string DurationMod { get; set; }


        public List<Participant> Participant { get; set; }

        public Activitie()
        {
            Participant = new List<Participant>();
        }

    }
}