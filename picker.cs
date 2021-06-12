using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Restaurant
{
    public class RestaurantPick
    {

        private class Restaurant
        {
            public int RestaurantId { get; set; }
            public Dictionary<string, decimal> Menu { get; set; }
        }

        private readonly List<Restaurant> _restaurants = new List<Restaurant>();

        /// <summary>
        /// Reads the file specified at the path and populates the restaurants list
        /// </summary>
        /// <param name="filePath">Path to the comma separated restuarant menu data</param>
        public void ReadRestaurantData(string filePath)
        {
            try
            {
                var records = File.ReadLines(filePath);

                foreach (var record in records)
                {
                    var data = record.Split(',');
                    var restaurantId = int.Parse(data[0].Trim());
                    var restaurant = _restaurants.Find(r => r.RestaurantId == restaurantId);

                    if (restaurant == null)
                    {
                        restaurant = new Restaurant { Menu = new Dictionary<string, decimal>() };
                        _restaurants.Add(restaurant);
                    }

                    restaurant.RestaurantId = restaurantId;
                    restaurant.Menu.Add(data.Skip(2).Select(s => s.Trim()).Aggregate((a, b) => a.Trim() + "," + b.Trim()), decimal.Parse(data[1].Trim()));
                }

            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        static void Main(string[] args)
        {
            var restaurantPicker = new RestaurantPick();
            
            restaurantPicker.ReadRestaurantData(
                Path.GetFullPath(
                    Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, @"../../../../restaurant_data.csv")
                    )
                );

            // Item is found in restaurant 2 at price 6.50
            var bestRestaurant = restaurantPicker.PickBestRestaurant("gac");

            Console.WriteLine(bestRestaurant.Item1 + ", " + bestRestaurant.Item2);

            Console.WriteLine("Done!");
            Console.ReadLine();
        }

        #endregion



        /// <summary>
        /// Takes in items you would like to eat and returns the best restaurant that serves them.
        /// </summary>
        /// <param name="items">Items you would like to eat (seperated by ',')</param>
        /// <returns>Restaurant Id and price tuple</returns>
        public Tuple<int, decimal> PickBestRestaurant(string items)
        {   
            List<string> meals = items.Split(',').ToList();

            decimal ans_price = 0;
            int ans_id = 0;

            foreach (var restau in _restaurants)
            {
                decimal price_restau = BackTrack(restau.Menu, meals, 0, 0);
    
                if ((price_restau < ans_price && price_restau != 0) || ans_price == 0)
                {
                    ans_price = price_restau;
                    ans_id = restau.RestaurantId;
                }

            }

            if (ans_price == 0)
            {
                return null;
            }
            return new Tuple<int, decimal>(ans_id, (decimal)ans_price);

            decimal BackTrack(Dictionary<string, decimal> menus, List<string> meals, decimal price, decimal total)
            {
                if (meals.Count == 0)
                {
                    if (total != 0)
                    {
                        total = Math.Min(price, total);
                    }
                    else
                    {
                        total = price;
                    }
                    return total;
                }

                List<string> visit = new List<string>();

                int counter = 0;
                foreach (KeyValuePair<string, decimal> menu in menus)
                {
                    counter += 1;

                    foreach (string meal in meals)
                    {
                        if (menu.Key.Contains(meal))
                        {
                            visit.Add(meal);

                        }
                    }

                    if (visit.Any())
                    {
                        price += menu.Value;
                        Dictionary<string, decimal> new_menus = new Dictionary<string, decimal>(menus);
                        for (int i = 1; i <= counter; i++)
                        {
                            new_menus.Remove(new_menus.Keys.First());
                        }

                        List<string> new_meals = new List<string>();
                        foreach (string meal in meals)
                        {
                            if (!visit.Contains(meal))
                            {
                                new_meals.Add(meal);
                            }
                        }

                        total = BackTrack(new_menus, new_meals, price, total);
                        
                        price -= menu.Value;

                    }
                    visit.Clear();
                }              
                return total;
            }
        }

        #endregion
    }
}
