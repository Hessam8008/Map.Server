using Map.Models.Customer;

namespace Map.DataAccess.DAO
{
    internal class CustomerDao
    {
        /// <summary>Initializes a new instance of the <see cref="CustomerDao" /> class.</summary>
        public CustomerDao() { }

        /// <summary>Initializes a new instance of the <see cref="CustomerDao" /> class.</summary>
        /// <param name="customer">The customer.</param>
        public CustomerDao(CustomerInfo customer)
        {
            ID = customer.ID;
            Name = customer.Name;
            Address = customer.Address;
            Latitude = customer.Latitude;
            Longitude = customer.Longitude;
            Active = customer.Active;
            Grade = customer.Grade;
            AreaID = customer.AreaID;
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the Latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the Longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        public byte Grade
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the area (Branch).
        /// </summary>
        public int AreaID { get; set; }

        /// <summary>Converts to customerinfo.</summary>
        /// <returns>
        ///   <br />
        /// </returns>
        public CustomerInfo ToCustomerInfo()
        {
            var customer = new CustomerInfo()
            {
                ID = ID,
                Active = Active,
                Address = Address,
                AreaID = AreaID,
                Grade = Grade,
                Latitude = Latitude,
                Longitude = Longitude,
                Name = Name
            };

            return customer;
        }
    }
}
