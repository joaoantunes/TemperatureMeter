namespace Kernel.Messages
{
    public class BaseMessage : IBaseMessage
    {
        private string? type;

        public string Type { get => type ??= this.GetType().FullName ?? 
                throw new ArgumentNullException("Not able to process Type FullName"); set => type = value; }
    }

    public interface IBaseMessage
    {
        string Type { get; set; }
    }
}
