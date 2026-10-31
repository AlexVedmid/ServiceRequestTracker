namespace ServiceRequestTracker.Common.Models
{
	public class SearchResult<T>
	{
		public SearchResult(List<T> items, int totalCount)
		{
			Items = items;
			TotalCount = totalCount;
		}

		public List<T> Items { get; }

		public int TotalCount { get; }
	}
}
