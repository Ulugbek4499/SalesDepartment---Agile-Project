using FluentValidation.Results;

namespace SalesDepartment.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; set; }

        public ValidationException()
            : base("One or more validation failures have occured.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string name, string message)
        {
            Errors = new Dictionary<string, string[]>()
            {
                {name, new string[] {message} }
            };
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key,
                              failureGroup => failureGroup.ToArray());
        }
    }
}
