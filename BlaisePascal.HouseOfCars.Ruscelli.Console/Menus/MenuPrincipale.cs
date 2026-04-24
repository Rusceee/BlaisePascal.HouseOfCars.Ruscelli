namespace BlaisePascal.HouseOfCars.Ruscelli.Presentation.Console.Menus;

using BlaisePascal.HouseOfCars.Ruscelli.Application.Exceptions;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.LiberaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OttimizzaParcheggio;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.TrovaPrimoPostoLibero;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public sealed class MenuPrincipale
{
    private readonly TrovaPrimoPostoLiberoHandler _trovaHandler;
    private readonly OttimizzaParcheggioHandler _ottimizzaHandler;
    private readonly OccupaBoxHandler _occupaHandler;
    private readonly LiberaBoxHandler _liberaHandler;

    public MenuPrincipale(
        TrovaPrimoPostoLiberoHandler trovaHandler,
        OttimizzaParcheggioHandler ottimizzaHandler,
        OccupaBoxHandler occupaHandler,
        LiberaBoxHandler liberaHandler)
    {
        _trovaHandler = trovaHandler;
        _ottimizzaHandler = ottimizzaHandler;
        _occupaHandler = occupaHandler;
        _liberaHandler = liberaHandler;
    }

    public void Avvia()
    {
        while (true)
        {
            MostraMenu();

            var scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1": TrovaPosto(); break;
                case "2": OccupaBox(); break;
                case "3": LiberaBox(); break;
                case "4": OttimizzaParcheggio(); break;
                case "0": return;
                default: Console.WriteLine("Scelta non valida."); break;
            }

            Console.WriteLine("\nPremi INVIO per continuare...");
            Console.ReadLine();
        }
    }

    private static void MostraMenu()
    {
        Console.Clear();
        Console.WriteLine("=== House of Cars ===");
        Console.WriteLine("1. Trova primo posto libero");
        Console.WriteLine("2. Occupa un box");
        Console.WriteLine("3. Libera un box");
        Console.WriteLine("4. Ottimizza parcheggio");
        Console.WriteLine("0. Esci");
        Console.Write("\nScelta: ");
    }

    private void TrovaPosto()
    {
        Console.Write("Colonna target (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int colonna))
        {
            Console.WriteLine("Colonna non valida.");
            return;
        }

        try
        {
            var posizione = _trovaHandler.Handle(new TrovaPrimoPostoLiberoCommand(colonna));

            if (posizione is null)
                Console.WriteLine("Nessun posto libero trovato.");
            else
                Console.WriteLine($"Posto libero trovato: {posizione}");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }

    private void OccupaBox()
    {
        var (piano, col, cella, box) = LeggiCoordinate();
        if (piano is null) return;

        Console.Write("Targa: ");
        var targa = Console.ReadLine();

        try
        {
            _occupaHandler.Handle(new OccupaBoxCommand(piano.Value, col!.Value, cella!, box!, targa!));
            Console.WriteLine("Box occupato con successo.");
        }
        catch (BoxGiaOccupatoException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }

    private void LiberaBox()
    {
        var (piano, col, cella, box) = LeggiCoordinate();
        if (piano is null) return;

        try
        {
            _liberaHandler.Handle(new LiberaBoxCommand(piano.Value, col!.Value, cella!, box!));
            Console.WriteLine("Box liberato con successo.");
        }
        catch (BoxGiaLiberoException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }

    private void OttimizzaParcheggio()
    {
        _ottimizzaHandler.Handle(new OttimizzaParcheggioCommand());
        Console.WriteLine("Ottimizzazione completata.");
    }

    private static (int? Piano, int? Colonna, string? Cella, string? Box) LeggiCoordinate()
    {
        Console.Write("Piano (-12/-1 oppure 1/12): ");
        if (!int.TryParse(Console.ReadLine(), out int piano))
        {
            Console.WriteLine("Piano non valido.");
            return (null, null, null, null);
        }

        Console.Write("Colonna (1-10): ");
        if (!int.TryParse(Console.ReadLine(), out int colonna))
        {
            Console.WriteLine("Colonna non valida.");
            return (null, null, null, null);
        }

        Console.Write("Cella (Nord/Sud): ");
        var cella = Console.ReadLine();

        Console.Write("Box (Est/Ovest): ");
        var box = Console.ReadLine();

        return (piano, colonna, cella, box);
    }
}