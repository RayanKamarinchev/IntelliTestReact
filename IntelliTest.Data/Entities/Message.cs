using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelliTest.Data.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public User Sender { get; set; }
        public string SenderId { get; set; }
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
