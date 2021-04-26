
namespace Integra.Shared.Base
{
    public abstract class PaginatedListBase
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int PáginaAnterior { get; set; }
        public int PáginaSiguiente { get; set; }


        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
