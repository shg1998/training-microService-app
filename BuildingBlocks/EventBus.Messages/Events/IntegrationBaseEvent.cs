
namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            this.Id = Guid.NewGuid();
            this.CreatedDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(Guid id, DateTime createdDate)
        {
            this.Id = id;
            this.CreatedDate = createdDate;
        }
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; private set; }
    }
}
