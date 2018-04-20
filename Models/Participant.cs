using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using BeltLogin.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeltLogin.Models
{
    public class Participant: BaseEntity
    {
        [Key]
        public int ParticipantId { get; set; }

        [ForeignKey("PartAct")]
        public int Activities_ActivitieId { get; set; }
        public Activitie PartAct { get; set; }

        [ForeignKey("PartId")]
        public int Users_UserId { get; set; }
        public User PartId { get; set; }


    }
}