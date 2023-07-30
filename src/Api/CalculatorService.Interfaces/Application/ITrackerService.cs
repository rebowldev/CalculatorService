namespace CalculatorService.Interfaces.Infrastructure
{
	public interface ITrackerService<T>
	{
		string HeaderKey { get; }

		/// <summary>
		/// Saves an operation for an specific trackerId
		/// </summary>
		/// <param name="trackerId">Tracker identifier</param>
		/// <param name="value">Operation description in JSON format</param>
		/// <returns></returns>
		Task SaveOperation(string trackerId, T value);

		/// <summary>
		/// Gets a list of operations for a specific trackId
		/// </summary>
		/// <param name="trackerId">Tracker identifier</param>
		/// <returns>List of operations in JSON format</returns>
		Task<List<T>> GetOperationsByTracker(string trackerId);
	}
}
