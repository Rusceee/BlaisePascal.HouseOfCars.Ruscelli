using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

public readonly record struct Posizione
{
    public Piano Piano { get; init; }
    public Colonna Colonna { get; init; }
    public Cella Cella { get; init; }
    public Box Box { get; init; }

    public Posizione(Piano piano, Colonna colonna, Cella cella, Box box)
    {
        Piano = piano;
        Colonna = colonna;
        Cella = cella;
        Box = box;
    }

    public override string ToString() =>
        $"{Colonna}, {Piano}, Cella {Cella}, Box {Box}";
}
