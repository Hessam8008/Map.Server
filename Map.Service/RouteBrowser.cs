namespace Map.Service
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Map.Models.AVL;

    using Microsoft.AspNetCore.Mvc.TagHelpers;

    public enum ProLocationState
    {
        Stop,

        Moving
    }

    public class ProLocation
    {
        public ProLocation()
        {

        }

        public ProLocation(Location location, ProLocationState state = ProLocationState.Stop)
        {
            this.Location = location;
            this.State = state;
        }
        
        public Location Location { get; set; }

        public double Distance { get; set; }

        public ProLocationState State { get; set; }
    }

    public class RouteBrowser
    {
        private List<ProLocation> proLocations;

        public RouteBrowser()
        {
        }

        public RouteBrowser(List<Location> locations)
        {
            this.Locations = locations;
        }

        public List<Location> Locations { get; set; }

        public List<ProLocation> ProLocations => this.proLocations;

        public ProLocation BeginPoint { get; set; }

        public ProLocation EndPoint { get; set; }

        public void BrowseRoute()
        {
            proLocations = new List<ProLocation>();

            if (this.Locations == null || !this.Locations.Any() || this.Locations.Count == 1)
            {
                return;
            }

            this.Locations.Sort((a, b) => a.Time.CompareTo(b.Time));

            this.BeginPoint = new ProLocation(this.Locations.First());
            this.EndPoint = new ProLocation(this.Locations.Last());


            ProLocation current = null;
            for (int i = 0; i < Locations.Count-2; i++)
            {
                current ??= new ProLocation(this.Locations[i]);
                current.State = current.Location.Speed > 0 ? ProLocationState.Moving : ProLocationState.Stop;
                    
                var next = Locations[i + 1];

                if (current.Location.Latitude == next.Latitude && current.Location.Longitude == next.Longitude)
                {
                    current.Location = next;
                    current.State = ProLocationState.Stop;
                }
                else
                {
                    if (this.proLocations.Count > 0)
                    {
                        current.Distance = distance(
                            this.proLocations[^1].Location,
                            current.Location);
                    }
                    proLocations.Add(current);
                    current = null;
                }
            }
        }


        private static double toRadians(double angleIn10thofaDegree)
        {
            // Angle in 10th 
            // of a degree 
            return (angleIn10thofaDegree * Math.PI) / 180;
        }

        private double distance(Location l1, Location l2)
        {

            // The math module contains  
            // a function named toRadians  
            // which converts from degrees  
            // to radians. 
            var lon1 = toRadians(l1.Longitude);
            var lon2 = toRadians(l2.Longitude);
            var lat1 = toRadians(l1.Latitude);
            var lat2 = toRadians(l2.Latitude);

            // Haversine formula  
            var dlon = lon2 - lon1;
            var dlat = lat2 - lat1;
            var a = Math.Pow(Math.Sin(dlat / 2), 2)+ Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dlon / 2), 2);

            var c = 2 * Math.Asin(Math.Sqrt(a));

            // Radius of earth in  
            // kilometers. Use 3956  
            // for miles 
            const double R = 6371;

            // calculate the result 
            return c * R * 1000; //Meters
        }
    }
}
