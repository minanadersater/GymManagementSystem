using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Comman
{
    public record Result(bool Success,string?Error =null,ResultKind Kind = ResultKind.Ok)
    {

        public static Result Ok() => new Result(true);
        public static Result Faild(string errorMassage , ResultKind kind = ResultKind.Conflict) => new Result(false,errorMassage,kind);
        public static Result NotFound(string errorMassage="Not Found") => new Result(false,errorMassage,ResultKind.NotFound);
        public static Result Validation(string errorMassage) => new Result(false,errorMassage,ResultKind.NotFound);

    }

    public record Result<T>(bool Success, T? Value,string? Error = null, ResultKind Kind = ResultKind.Ok) : Result(Success, Error, Kind)
    {
        public static Result<T> Ok(T value) => new Result<T>(true, value);
        public static new Result<T> Faild(string errorMassage, ResultKind kind = ResultKind.Conflict) => new (false, default, errorMassage, kind);
        public static new Result<T> NotFound(string errorMassage = "Not Found") => new(false, default, errorMassage, ResultKind.NotFound);
     //   public static new Result<T> Validation(string errorMassage) => new Result<T>(false, default, errorMassage, ResultKind.NotFound);
    }

    public enum ResultKind
    {
        Ok,
        NotFound,
        Conflict,
        ValidationFailed,
        Forbidden,
    }
}
