namespace Kernel.Messages
{
    public class BaseNotification : IBaseNotification
    {
        private string? typeContract;

        public string TypeContract { get => typeContract ??= this.GetType().FullName ?? 
                throw new ArgumentNullException("Not able to process Type FullName"); set => typeContract = value; }
    }
}
