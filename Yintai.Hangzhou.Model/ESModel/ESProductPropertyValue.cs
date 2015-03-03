
namespace Yintai.Hangzhou.Model.ESModel
{
    public class ESProductPropertyValue
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public int PropertyId { get; set; }

        public string PropertyDesc { get; set; }

        public string ValueDesc { get; set; }

        public bool IsColor { get; set; }

        public bool IsSize { get; set; }
    }
}
