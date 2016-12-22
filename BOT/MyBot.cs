using System.Collections.Generic;
using Pirates;

namespace MyBot
{
    public class MyBot : Pirates.IPirateBot
    {
        private List<Location> ClosestCity(PirateGame game)
        {
            List<Location> loc = new List<Location>();
            for (int i = 0; i < game.GetMyCities().Count; i++)
            {
                Location Min = game.GetAllIslands()[0].Location;
                for (int v = 0; v < game.GetAllIslands().Count; v++)
                {
                    if (game.GetMyCities()[i].Distance(game.GetAllIslands()[v]) < game.GetMyCities()[i].Distance(Min))
                    {
                        Min = game.GetAllIslands()[v].Location;
                    }
                }
                loc.Add(Min);
            }
            return loc;
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
            for (int j = 0; j < game.GetMyCities().Count; j++)
            {
                for (int i = 0; i < LivingPirates.Count; i++)
                {
                    if (!TryAttack(LivingPirates[i], game))
                    {
                        // Get the first island
                        game.Debug(ClosestCity(game)[j]);
                        Location destination = ClosestCity(game)[j];
                        // Get sail options
                        List<Location> sailOptions = game.GetSailOptions(LivingPirates[i], destination);
                        // Set sail!
                        game.SetSail(LivingPirates[i], sailOptions[0]);
                        // Print a message
                        game.Debug("pirate " + LivingPirates[i] + " sails to " + sailOptions[0]);
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
