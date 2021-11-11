using ExceptionMiddleware.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace StatusAppBackend.Exceptions
{
    public class ConflictException : AppException
    {
        private static readonly string TITLE = "Conflict";
        private static readonly string DEFAULT_MESSAGE = "The request resulted in a conflict";

        public override IActionResult ResponseObject => new ConflictObjectResult(this.GetErrorObject());

        public ConflictException() : this(DEFAULT_MESSAGE)
        {

        }

        public ConflictException(string detailMessage) : this(detailMessage, (int)StatusAppErrorCodes.Conflict)
        {

        }

        public ConflictException(string detailMessage, int errorCode) : base(TITLE, detailMessage, errorCode)
        {

        }
    }
}