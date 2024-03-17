using MediatR;

namespace Kernel.Messages
{
    public interface IBaseNotification : INotification
    {
        string TypeContract { get; set; }
    }
}
