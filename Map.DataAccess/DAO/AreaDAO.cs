using Map.Models.AVL;

namespace Map.DataAccess.DAO
{
    public class AreaDAO
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        public int ID { get; set; }

        /// <summary>Gets or sets the title.</summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        public AreaDAO() { }

        public AreaDAO(Area area)
        {
            ID = area.ID;
            Title = area.Title;
        }

        public Area ToArea()
            =>
                new Area()
                {
                    ID = ID,
                    Title = Title
                };
    }
}
