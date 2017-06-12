
// 0. Add support for Entity Framework Core by installing the nuget package and modifying csproj file if necessary
// 1. Add a model class

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string TaskContent { get; set; }
        public bool IsComplete { get; set; }
    }
}
