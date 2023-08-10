using System.Collections.Generic;

namespace Shared.Wrapper
{
    public interface IResponse
    {
        string? Messages { get; set; }

        bool Succeeded { get; set; }
    }

    public interface IResponse<out T> : IResponse
    {
        T Data { get; }
    }
}