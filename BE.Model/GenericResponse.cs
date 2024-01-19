using BE.Model;

namespace User_RESTful.Controllers
{
    public interface IGenericResponse
    {
        bool Succeeded { get; set; }
        string Message { get; set; }
        object Value { get; set; }
    }
    internal class GenericResponse : IGenericResponse
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public object Value { get; set; }
        public static GenericResponse Success(string message)
        {
            return new GenericResponse() { Succeeded = true, Message = message };
        }
        public static GenericResponse Success(string message, object value)
        {
            return new GenericResponse() { Succeeded = true, Message = message, Value = value };
        }
        public static GenericResponse Fail(string message)
        {
            return new GenericResponse() { Succeeded = false, Message = message };
        }
    }
}