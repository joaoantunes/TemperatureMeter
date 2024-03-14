namespace Kernel.Messages
{
    public class BaseNotification : IBaseNotification
    {
        private string? type;

        public string Type { get => type ??= this.GetType().FullName ?? 
                throw new ArgumentNullException("Not able to process Type FullName"); set => type = value; }
    }
}
