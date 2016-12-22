using System.Collections.Generic;
using Pirates;

namespace MyBot
{
    public class MyBot : Pirates.IPirateBot
    {
        private void HandlePirates(PirateGame game)
        {
            // Go over all of my pirates
            foreach (Pirate pirate in game.GetMyLivingPirates())
            {
                if (!TryAttack(pirate, game))
                {
                    // Get the first island
                    Island destination = game.GetAllIslands()[0];
                    // Get sail options
                    List<Location> sailOptions = game.GetSailOptions(pirate, destination);
                    // Set sail!
                    game.SetSail(pirate, sailOptions[0]);
                    // Print a message
                    game.Debug("pirate " + pirate + " sails to " + sailOptions[0]);
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
