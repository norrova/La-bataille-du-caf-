using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;


namespace GameCore
{
    public class IA
    {
        #region Attributes
        private Map m_map;
        private Coordinate m_oldSeed = new Coordinate();
        private Boolean m_firstRound = true;
        #endregion

        #region Getter and Setter
        public Coordinate OldSeed { get => m_oldSeed; set => m_oldSeed = value; }
        public Map Map { get => m_map; set => m_map = value; }
        #endregion

        public IA(ref Map p_map) => this.Map = p_map;

        /// <summary>
        /// Logic IA
        /// </summary>
        /// <param name="p_p1"></param>
        /// <returns></returns>
        public String Logic(ref Player p_p1)
        {
            Coordinate v_coord = null;
            Boolean v_continue = true;
            // Generate seed
            while (v_continue)
            {
                v_coord = this.m_firstRound ? GenerateRandomCoord() : GenerateCoord();
                if (AllowToPlant(v_coord.X, v_coord.Y))
                    this.m_firstRound = v_continue = false;
            }
            // Asign last location the new seed
            byte v_x = v_coord.X;
            byte v_y = v_coord.Y;
            this.Map.ActualMap[v_x, v_y].Owner = p_p1.NumberOwner;
            this.Map.ActualMap[v_x, v_y].Available = false;
            this.OldSeed.Zone = this.Map.GetLetterParcel(v_x, v_y);
            this.OldSeed.X = v_x;
            this.OldSeed.Y = v_y;
            // Calculate owner of the parcel
            this.CalculateOwner();
            return new Coordinate("A", this.OldSeed.X, this.OldSeed.Y).GetCoordinates();
        }

        /// <summary>
        /// Calculate owner of the parcel
        /// </summary>
        public void CalculateOwner()
        {
            byte v_counterMe = 0;
            byte v_counterServer = 0;
            foreach (Coordinate s_coord in this.Map.Parcel[this.OldSeed.Zone.ToLower()].Coordinates)
            {
                byte v_owner = this.Map.ActualMap[s_coord.X, s_coord.Y].Owner;
                if (v_owner == (byte)Seed.Nobody)
                    continue;
                else if(v_owner == (byte)Seed.Me)
                {
                    v_counterMe += 1;
                }
                else if(v_owner == (byte)Seed.Server)
                {
                    v_counterServer += 1;
                }     
            }
            /**
             * Owner :
             * Us > Server = Us
             * Server > Us = Server
             * Us == Server = Nobody
             */
            this.Map.Parcel[this.OldSeed.Zone.ToLower()].Owner = v_counterMe > v_counterServer ? (byte)Seed.Me : 
                v_counterServer > v_counterMe ? (byte)Seed.Server : 
                (byte)Seed.Nobody;
        }

        /// <summary>
        /// Create location of seed with max point for us and min point for ennemy
        /// </summary>
        /// <returns></returns>
        private Coordinate GenerateRandomCoord()
        {
            // We search the best locations in terms of point
            byte v_maxPoint = 0;
            Dictionary<string, Parcel> v_parcels = new Dictionary<string, Parcel>();
            foreach (KeyValuePair<string, Parcel> s_parcel in this.m_map.Parcel)
            {
                if (v_maxPoint <= s_parcel.Value.Coordinates.Count)
                {
                    v_maxPoint = (byte)s_parcel.Value.Coordinates.Count;
                    v_parcels.Add(s_parcel.Key, s_parcel.Value);
                }
            }
            /* 6 points = 100%, 4 points = 66%, 3 points = 50%, 33 points = 33%
             * Calculate the ratio of each seed and add into list of best locations
             * 
             * Ratio :
             * 0% Critical location - 100% Best location
             */
            float v_ratio = 0;
            float v_maxRatio = (byte)Math.Percent(v_maxPoint, v_maxPoint);
            Dictionary<Coordinate, float> v_dictRatio = new Dictionary<Coordinate, float>();
            foreach (KeyValuePair<string, Parcel> s_parcel in v_parcels)
            {
                foreach (Coordinate s_coord in s_parcel.Value.Coordinates)
                {
                    v_ratio = 0;
                    Dictionary<string, Coordinate> v_locations = new Dictionary<string, Coordinate>();
                    foreach (Coordinate s_location in GetAllLocations(s_coord))
                    {
                        if(s_location.Zone != s_coord.Zone && !v_locations.ContainsKey(s_location.Zone))
                        {
                            v_locations.Add(s_location.Zone, s_location);
                        }
                    }
                    foreach (Coordinate v_coord2 in v_locations.Values)
                    {
                        v_ratio += (v_maxRatio - Math.Percent(this.m_map.Parcel[v_coord2.Zone.ToLower()].Coordinates.Count, (int)v_maxPoint));
                    }
                    v_dictRatio.Add(s_coord, (v_ratio/v_locations.Values.Count));
                } 
            }
            // Choice the seed to start with the best location
            Coordinate v_coord = null;
            foreach (KeyValuePair<Coordinate, float> s_coord in v_dictRatio)
            {
                if (s_coord.Value.Equals(v_dictRatio.Values.Max()))
                {
                    v_coord = s_coord.Key;
                    break;
                }
            }
            return v_coord;
        }

        /// <summary>
        /// Generate seed location
        /// </summary>
        /// <returns></returns>
        private Coordinate GenerateCoord()
        {
            return this.GetBestLocation(this.GetAllLocations(this.OldSeed));
        }
   
        /// <summary>
        /// Get all seeds locations based on the seed parameter (same line or column)
        /// </summary>
        /// <param name="p_coord"></param>
        /// <returns></returns>
        private List<Coordinate> GetAllLocations(Coordinate p_coord)
        {
            List<Coordinate> v_locations = new List<Coordinate>();
            // foreach all seed location (row and column) of p_coord
            foreach (string s_direction in new String[] { "vertical", "horizontal" })
            {
                byte v_index = 0;
                while (v_index < 10)
                {
                    Coordinate v_coord = s_direction.Equals("horizontal") ?
                   new Coordinate(this.Map.GetLetterParcel(p_coord.X, v_index), p_coord.X, v_index) :
                   new Coordinate(this.Map.GetLetterParcel(v_index, p_coord.Y), v_index, p_coord.Y);
                    if (this.Map.ActualMap[v_coord.X, v_coord.Y].Available && !CheckSeaOrForest(v_coord.X, v_coord.Y) && !SameParcel(v_coord.X, v_coord.Y))
                        v_locations.Add(v_coord);
                    v_index++;
                }
            }
            return v_locations;
        }

        /// <summary>
        /// Get the best seed location
        /// </summary>
        /// <param name="p_locations"></param>
        /// <returns></returns>
        private Coordinate GetBestLocation(List<Coordinate> p_locations)
        {
            byte v_maxAdjacent = 0;
            byte v_maxPoint = 0;
            Coordinate v_bestLocation = null;
            // Foreach owner of parcel 0 1 3
            foreach (int s_owner in (Seed[])Enum.GetValues(typeof(Seed)))
            {
                foreach (Coordinate s_coord in p_locations)
                {
                    byte v_adjacent = CheckAdjecent((byte)Seed.Me, s_coord);
                    byte v_point = (byte)this.Map.Parcel[s_coord.Zone.ToLower()].Coordinates.Count;
                    // Check seed adjacent and how much point we can get is superior to last best seed
                    if (v_adjacent >= v_maxAdjacent && 
                        v_point >= v_maxPoint && 
                        this.Map.Parcel[s_coord.Zone.ToLower()].Owner.Equals((byte)s_owner))
                    {
                            v_maxAdjacent = v_adjacent;
                            v_maxPoint = v_point;
                            v_bestLocation = s_coord;
                            break;
                    }
                }
                if (v_bestLocation != null) break;
            }
            return v_bestLocation;
        }

        /// <summary>
        /// Check all seed adjacent north, east, south, west
        /// </summary>
        /// <param name="p_owner"></param>
        /// <param name="p_coord"></param>
        /// <returns></returns>
        public byte CheckAdjecent(byte p_owner, Coordinate p_coord)
        {
            int v_x = (int)p_coord.X;
            int v_y = (int)p_coord.Y;
            byte v_count = 0;
            List<Coordinate> v_locations = new List<Coordinate>()
            {
                new Coordinate((byte)(v_x - 1), (byte)v_y), // North
                new Coordinate((byte)v_x, (byte)(v_y +1)),   // East                    
                new Coordinate((byte)(v_x + 1), (byte)v_y), // South
                new Coordinate((byte)v_x, (byte)(v_y -1)),   // West
            };
            foreach (Coordinate v_location in v_locations)
            {
                try
                {
                    // Check if location is owned per us
                    if (this.Map.ActualMap[v_location.X, v_location.Y].Owner == p_owner) 
                        ++v_count;
                }
                catch (IndexOutOfRangeException)
                {
                    continue;
                }
            }
            return v_count;           
        }

        #region Vérification du placement d'une graine
        /// <summary>
        /// Allow to plant or not
        /// </summary>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        /// <returns></returns>
        public Boolean AllowToPlant(byte p_x, byte p_y){
            return this.m_firstRound ? !CheckSeaOrForest(p_x, p_y) ? true : false :
                this.Map.ActualMap[p_x, p_y].Available && !CheckSeaOrForest(p_x, p_y) && SameXorY(p_x, p_y) && !SameParcel(p_x, p_y) ? true : false;
        }

        /// <summary>
        /// Check Sea or Forest type unit
        /// </summary>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        /// <returns></returns>
        private Boolean CheckSeaOrForest(byte p_x, byte p_y)
        {
            return this.Map.ActualMap[p_x, p_y].LetterParcel.Equals('M') ? true :
                this.Map.ActualMap[p_x, p_y].LetterParcel.Equals('F') ? true : false;
        }

        /// <summary>
        /// Check if seed is based on the same line or column from the old seed
        /// </summary>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        /// <returns></returns>
        private Boolean SameXorY(byte p_x, byte p_y)
        {
            return this.OldSeed.X == p_x || this.OldSeed.Y == p_y ? true : false;
        }

        /// <summary>
        /// Check if seed is based on the same parcel from the old seed
        /// </summary>
        /// <param name="p_x"></param>
        /// <param name="p_y"></param>
        /// <returns></returns>
        private Boolean SameParcel(byte p_x, byte p_y)
        {
            return this.OldSeed.Zone == this.Map.GetLetterParcel(p_x, p_y) ? true : false;
        }
        #endregion
    }
}
