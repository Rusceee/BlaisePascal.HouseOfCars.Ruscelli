using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities
{
    public sealed class Parcheggio
    {
        private readonly Dictionary<Posizione, BoxAuto> _box;

        public Parcheggio(IEnumerable<BoxAuto> box)
        {
            _box = box.ToDictionary(b => b.Posizione);
        }

        public BoxAuto GetBox(Posizione posizione)
        {
            if (!_box.TryGetValue(posizione, out var box))
                throw new KeyNotFoundException("Box non trovato.");

            return box;
        }

        public void OccupaBox(Posizione posizione, Macchina macchina)
        {
            var box = GetBox(posizione);
            box.Occupa(macchina);
        }

        public Macchina LiberaBox(Posizione posizione)
        {
            var box = GetBox(posizione);    
            return box.Libera();
        }
        public Posizione? TrovaPrimoPostoLibero(Colonna colonnaTarget)
        {
            foreach (var colonna in GetOrdineColonne(colonnaTarget))
            {
                foreach (var piano in GetOrdinePiani())
                {
                    foreach (Cella cella in System.Enum.GetValues<Cella>())
                    {
                        foreach (Box latoBox in System.Enum.GetValues<Box>())
                        {
                            var posizione = new Posizione(
                                piano,
                                colonna,
                                cella,
                                latoBox
                            );

                            if (_box.TryGetValue(posizione, out var box) && !box.Occupato)
                            {
                                return posizione;
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static IEnumerable<Colonna> GetOrdineColonne(Colonna target)
        {
            yield return target;

            for (int offset = 1; offset <= 9; offset++)
            {
                int successiva = target.Valore + offset;
                if (successiva <= 10)
                    yield return new Colonna(successiva);

                int precedente = target.Valore - offset;
                if (precedente >= 1)
                    yield return new Colonna(precedente);
            }
        }

        private static IEnumerable<Piano> GetOrdinePiani()
        {
            for (int i = 1; i <= 12; i++)
            {
                yield return new Piano(i);
                yield return new Piano(-i);
            }
        }
        public void OttimizzaParcheggio()
        {
            var daSpostare = _box.Values
                .Where(b => b.Occupato && b.Posizione.Piano.Valore >= 8)
                .ToList();

            var destinazioni = _box.Values
                .Where(b => !b.Occupato && b.Posizione.Piano.IsSotterraneo)
                .ToList();

            int count = Math.Min(daSpostare.Count, destinazioni.Count);

            for (int i = 0; i < count; i++)
            {
                var macchina = daSpostare[i].Libera();
                destinazioni[i].Occupa(macchina);
            }
        }
    }
}
