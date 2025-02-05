using System.Net;

namespace Elysio.Models.Exceptions;

public class NotFoundException : Exception
{
    #region Public Fields

    public string Type { get; set; }
    public Guid Id { get; set; }
    public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

    #endregion Public Fields

    #region Ctor.Dtor

    public NotFoundException(string type, Guid id)
        : base($"Le {type} avec l'Id {id} n'a pas été trouvé")
    {
        this.Type = type;
        this.Id = id;
    }

    #endregion Ctor.Dtor
}