using System.Collections.Generic;
using Pirates;
using System.Linq;

namespace MyBot
{
    public class MyBot : Pirates.IPirateBot
    {
        private List<Island> SortIsland(PirateGame game, City city)
        {
            List<Island> islands = game.GetAllIslands();
            List<Island> New = new List<Island>();
            Island min = islands[0];
            for (int j = 0; j < islands.Count; j++)
            {
                for (int i = 0; i < islands.Count; i++)
                {
                    if (min.Distance(city) >= islands[i].Distance(city))
                    {
                        min = islands[i];
                    }
                }
                New.Add(min);
                islands.Remove(min);
            }
            New.Add(islands[0]);
            return New;
        }
        private List<Location> ClosestCityEnemy(PirateGame game)
        {
            List<Location> loc = new List<Location>();
            for (int i = 0; i < game.GetEnemyCities().Count; i++)
            {
                Location Min = game.GetAllIslands()[0].Location;
                for (int v = 0; v < game.GetAllIslands().Count; v++)
                {
                    if (game.GetEnemyCities()[i].Distance(game.GetAllIslands()[v]) < game.GetEnemyCities()[i].Distance(Min))
                    {
                        Min = game.GetAllIslands()[v].Location;
                    }
                }
                loc[i] = Min;
            }
            return loc;
        }
        private void HandlePirates(PirateGame game)
        {
            List<Pirate> LivingPirates = game.GetMyLivingPirates();
            List<City> Cities = game.GetMyCities();
            for (int j = 0; j < Cities.Count; j++)
            {
                int count = game.GetAllIslands().Count; int x = 0;
                Island destination = SortIsland(game, Cities[j])[x];
                for (int i = 0; i < LivingPirates.Count; i++)
                {
                    if (!TryAttack(LivingPirates[i], game) && x <= count)
                    {
                        // Get sail options
                        List<Location> sailOptions = game.GetSailOptions(LivingPirates[i], destination);
                        // Set sail!
                        game.SetSail(LivingPirates[i], sailOptions[0]);
                        // Print a message
                        game.Debug("pirate " + LivingPirates[i] + " sails to " + sailOptions[0]);
                        if (sailOptions[i] == LivingPirates[i].GetLocation())
                        {
                            destination = SortIsland(game, Cities[j])[x + 1];
                        }
                        else
                            x++;
                    }
                    
                }

            }

        }


        private void HandleDrones(PirateGame game)
        {
            // Go over all of my drones
            foreach (Drone drone in game.GetMyLivingDrones())
            {
                // Get my first city
                City destination = game.GetMyCities()[0];
                // Get sail options
                List<Location> sailOptions = game.GetSailOptions(drone, destination);
                // Set sail!
                game.SetSail(drone, sailOptions[0]);
            }
        }

        public bool TryAttack(Pirate pirate, PirateGame game)
        {
            // Go over all enemies
            foreach (Aircraft enemy in game.GetEnemyLivingAircrafts())
            {
                // Check if the enemy is in attack range
                if (pirate.InAttackRange(enemy))
                {
                    // Fire!
                    game.Attack(pirate, enemy);
                    // Print a message
                    game.Debug("pirate " + pirate + " attacks " + enemy);
                    // Did attack
                    return true;
                }
            }
            // Didnt attack
            return false;
        }

        public void DoTurn(PirateGame game)
        {
            // Give orders to my pirates
            HandlePirates(game);
            // Give orders to my drones
            HandleDrones(game);
        }
    }

}