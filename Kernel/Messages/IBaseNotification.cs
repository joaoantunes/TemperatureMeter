using MediatR;

namespace Kernel.Messages
{
    public interface IBaseNotification : INotification
    {
        string Type { get; set; }
    }
}
