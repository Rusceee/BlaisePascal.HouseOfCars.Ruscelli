using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

public readonly record struct Colonna
{
    public int Valore { get; init; }

    public Colonna(int valore)
    {
        if (valore < 1 || valore > 10)
            throw new ArgumentOutOfRangeException(nameof(valore),
                "La colonna deve essere compresa tra 1 e 10.");

        Valore = valore;
    }

    public override string ToString() => $"Colonna {Valore}";
}
