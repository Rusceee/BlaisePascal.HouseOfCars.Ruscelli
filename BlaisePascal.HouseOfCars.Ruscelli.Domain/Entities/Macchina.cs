using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities
{
    public sealed class Macchina
    {
        public string Targa { get; }

        public Macchina(string targa)
        {
            if (string.IsNullOrWhiteSpace(targa))
                throw new ArgumentException("La targa non può essere vuota.", nameof(targa));
            // metto tutto caps cosi che non ho problemi case sensitive
            Targa = targa.Trim().ToUpperInvariant();
        }
    }
}
