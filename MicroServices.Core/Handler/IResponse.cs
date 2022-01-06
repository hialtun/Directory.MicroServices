namespace MicroServices.Core.Handler
{
    public interface IResponse
    {
        bool Success { get; set; }
        string Message { get; set; }
    }
}