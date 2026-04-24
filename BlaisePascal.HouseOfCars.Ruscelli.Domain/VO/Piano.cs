using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

public readonly record struct Piano
{
    public int Valore { get; init; }
    public bool IsSotterraneo => Valore < 0;

    public Piano(int valore)
    {
        if (valore == 0 || valore < -12 || valore > 12)
            throw new ArgumentOutOfRangeException(nameof(valore),
                "Il piano deve essere compreso tra -12 e -1 oppure tra 1 e 12.");

        Valore = valore;
    }

    public override string ToString() => $"Piano {Valore}";
}
