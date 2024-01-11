namespace Fanex.Affiliate.Link.DataAccess.Impl
{
    using Fanex.Affiliate.Link.Commands;
    using Fanex.Affiliate.Link.DataAccess.Interface;
    using Fanex.Data.Repository;

    /// <summary>
    /// Tracking statistics.
    /// </summary>
    public class TrackingSystemDataAccess : ITrackingSystemDataAccess
    {
        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="command">Input command.</param>
        /// <returns>Success or fail.</returns>
        private readonly IDynamicRepository _dynamicRepository;

        public TrackingSystemDataAccess(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public string InsertStatistic(TrackingSystemCommand command)
        {
            _dynamicRepository.Execute(command);

            return command.RedirectPage;
        }
    }
}
