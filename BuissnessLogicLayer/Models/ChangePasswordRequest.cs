using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Models
{
    public class ChangePasswordRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MinLength(8)]
        public string OldPassword { get; set; }
        [Required]
        [MinLength(8)]
        public string NewPassword { get; set; }
    }
}
