using Marketplace.DAL.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.DAL.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [StringLength(200)]
        public required string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; } = false;
        public string UserId { get; set; }
    }
}
