using BoardingBloodbath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoardingBloodBath
{
    enum Presets
    {
        Galleons,
        Brigs
    }
    public struct Preset
    {
        public TeamShips navy;
        public TeamShips pirate;
        public Preset(ShipsHandler.Ships navyS1, ShipsHandler.Ships navyB, ShipsHandler.Ships navyS2, ShipsHandler.Ships pirateS1, ShipsHandler.Ships pirateB, ShipsHandler.Ships pirateS2)
        {
            navy.smallShip1 = navyS1;
            navy.smallShip2 = navyS2;
            navy.bigShip = navyB;

            pirate.smallShip1 = pirateS1;
            pirate.smallShip2 = pirateS2;
            pirate.bigShip = pirateB;
        }
    }
    public struct TeamShips
    {
        public ShipsHandler.Ships smallShip1;
        public ShipsHandler.Ships bigShip;
        public ShipsHandler.Ships smallShip2;
    }
}
