using System;

namespace user_bff.Models
{
    public class RestaurantOpenDays
    {
        public int Id { get; set; }
        public Guid RestaurantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Day { get; set; }
        public TimeSpan OpenTime { get; set; }
        public TimeSpan CloseTime { get; set; }
        public bool IsOpen { get; set; }
    }
}