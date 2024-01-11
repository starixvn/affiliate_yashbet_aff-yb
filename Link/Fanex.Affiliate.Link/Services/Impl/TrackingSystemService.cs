namespace Fanex.Affiliate.Link.Services.Impl
{
    using Fanex.Affiliate.Link.Commands;
    using Fanex.Affiliate.Link.DataAccess.Impl;
    using Fanex.Affiliate.Link.DataAccess.Interface;
    using Fanex.Affiliate.Link.Services.Interface;
    using Fanex.Data.Repository;

    /// <summary>
    /// Tracking system statistics.
    /// </summary>
    public class TrackingSystemService : ITrackingSystemService
    {
        #region Fields

        /// <summary>
        /// Define Account DataAccess.
        /// </summary>
        private readonly ITrackingSystemDataAccess _trackingSystemDataAccess;

        #endregion Fields

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingSystemService" /> class.
        /// </summary>
        public TrackingSystemService()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingSystemService" /> class.
        /// </summary>
        /// <param name="trackingSystemDataAccess">Account data access.</param>
        private TrackingSystemService(ITrackingSystemDataAccess trackingSystemDataAccess)
        {
            _trackingSystemDataAccess = trackingSystemDataAccess ?? new TrackingSystemDataAccess(new DynamicRepository());
        }

        #endregion Constructor

        /// <summary>
        /// Insert statistics into database.
        /// </summary>
        /// <param name="command">Input command.</param>
        /// <returns>Success or fail.</returns>
        public string InsertStatistic(TrackingSystemCommand command)
        {
            return _trackingSystemDataAccess.InsertStatistic(command);
        }
    }
}
