
namespace MicroServices.Core.Handler
{
    public abstract class ResponseBase<T> : IResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Model { get; set; }
    }
}