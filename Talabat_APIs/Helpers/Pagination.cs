namespace Talabat_APIs.Helpers
{
    public class Pagination<T> where T : class
    {
        //Standerd Response for Pagination
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

    }
}
