using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedEngine;
using Microsoft.Xna.Framework;

namespace SpaceCowboy
{
    public class SolarSystem : IProcedurallyGenerated
    {
        public int StarCount { get; set; }
        public int PlanetCount { get; set; }
        public int AsteroidCount { get; set; }
        public int MaterialType { get; set; }
        public GameObject[] Planets;
        public GameObject[] Stars;
        public GameObject[] Asteroids;
 
        public void Generate(Random rng, int mass)
        {
            //TODO: Generate based off of fixed mass limit

            //Stars
            GenerateStars(rng, 1000);

            //Planets
            GeneratePlanets(rng, 1000);

            //Asteroids (by mass)
            GenerateAsteroids(rng, 1000);

            //Material Type (by mass percentage)
            //Common Gas, Common Mineral, Rare Mineral, Rare Gas
        }

        public void GenerateStars(Random rng, int mass)
        {
            //5% chance of binary
            int percentage = rng.Next(0, 100);
            if (percentage < 5)
            {
                //2 Stars
                StarCount = 2;
            }
            else
            {
                StarCount = 1;
                //1 Star
            }

            Stars = new GameObject[StarCount];
            for (int x = 0; x < StarCount; x++)
            {
                Star star = new Star();
                star.Generate(rng, mass);
                Stars[x] = star;
            }

            if (Stars.Count() == 2)
            {
                Stars[0].Position = new Vector2(512, 512);
                Stars[1].Position = new Vector2(-512, -512);
            }
            else
            {
                Stars[0].Position = Vector2.Zero;
            }
        }

        public void GeneratePlanets(Random rng, int mass)
        {
            //Random 0-10
            int count = rng.Next(0, 100) / 10;
            PlanetCount = count;

            Planets = new GameObject[count];
            for (int x = 0; x < count; x++)
            {
                Planet planet = new Planet();
                planet.Generate(rng, mass);

                Planets[x] = planet;
            }
        }

        public void GenerateAsteroids(Random rng, int mass)
        {
            int count = 256;

            Asteroids = new GameObject[count];
            for (int x = 0; x < count; x++)
            {
                Asteroid asteroid = new Asteroid();
                asteroid.Generate(rng, mass);

                Asteroids[x] = asteroid;
            }
        }
    }
}