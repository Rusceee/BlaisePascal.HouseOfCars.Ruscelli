using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities
{
    public sealed class BoxAuto
    {
        public Posizione Posizione { get; }
        public Macchina? MacchinaParcheggiata { get; private set; }

        public bool Occupato => MacchinaParcheggiata is not null;

        public BoxAuto(Posizione posizione)
        {
            Posizione = posizione;
        }

        public void Occupa(Macchina macchina)
        {
            if (Occupato)
                throw new InvalidOperationException("Il box è già occupato.");

            MacchinaParcheggiata = macchina;
        }

        public Macchina Libera()
        {
            if (!Occupato)
                throw new InvalidOperationException("Il box è già libero.");

            var macchina = MacchinaParcheggiata!;
            MacchinaParcheggiata = null;
            return macchina;
        }
    }
}
