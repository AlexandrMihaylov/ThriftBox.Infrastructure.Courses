namespace ThriftBox.Infrastructure.Courses.Abstractions;

public interface IParseSymbolsHandler
{
    Task<CoursesResponse> Handle(string symbol);
}