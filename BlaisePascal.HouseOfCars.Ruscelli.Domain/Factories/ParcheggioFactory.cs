using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories
{
    public class ParcheggioFactory
    {
        public static Parcheggio CreaVuoto()
        {
            var box = new List<BoxAuto>();

            for (int colonna = 1; colonna <= 10; colonna++)
            {
                for (int piano = -12; piano <= 12; piano++)
                {
                    if (piano == 0) continue;

                    foreach (Cella cella in System.Enum.GetValues<Cella>())
                    {
                        foreach (Box latoBox in System.Enum.GetValues<Box>())
                        {
                            var posizione = new Posizione(
                                new Piano(piano),
                                new Colonna(colonna),
                                cella,
                                latoBox
                            );

                            box.Add(new BoxAuto(posizione));
                        }
                    }
                }
            }

            return new Parcheggio(box);
        }
    }
}