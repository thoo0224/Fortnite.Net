using System.Net;
using Fortnite.Net.Exceptions;

namespace Fortnite.Net.Objects
{

    public class FortniteResponse
    {

        public EpicError Error { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public bool IsSuccessful => Error == null;

    }

    public class FortniteResponse<T> : FortniteResponse
    {

        public T Data { get; set; }

    }
    
}
