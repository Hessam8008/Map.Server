namespace Map.DataAccess.DAO
{
    using Map.Models.AVL;

    /// <summary>
    /// The area data access object.
    /// </summary>
    public class AreaDAO
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the distribution station id.
        /// </summary>
        public int DistributionStationID { get; set; }

        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaDAO"/> class.
        /// </summary>
        public AreaDAO() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaDAO"/> class.
        /// </summary>
        /// <param name="area">
        /// The area.
        /// </param>
        public AreaDAO(Area area)
        {
            this.ID = area.ID;
            this.Title = area.Title;
            this.DistributionStationID = area.DistributionStationID;
            this.Longitude = area.Longitude;
            this.Latitude = area.Latitude;
        }

        public Area ToArea()
            =>
                new Area
                {
                    ID = this.ID,
                    Title = this.Title,
                    DistributionStationID = this.DistributionStationID,
                    Longitude = this.Longitude,
                    Latitude = this.Latitude
                };
    }
}
