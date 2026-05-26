namespace GymManagementSystem.DAL.Entities
{
    public class Member:GymUser
    {
        public string photo { get; set; } = null!;
        public HealthRecord healthRecord { get; set; }=null!;

        public ICollection<Membership> Memberships = new HashSet<Membership>();
        public ICollection<Booking> Bookings = new HashSet<Booking>();
    }
}