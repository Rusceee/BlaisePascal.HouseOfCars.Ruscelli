using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.Factories;

public class ParcheggioFactoryTests
{
    [Fact]
    public void CreaVuoto_DeveRestituireParcheggio()
    {
        var parcheggio = ParcheggioFactory.CreaVuoto();
        Assert.NotNull(parcheggio);
    }

    [Fact]
    public void CreaVuoto_TuttiIBoxDevonEssereLiberi()
    {
        var parcheggio = ParcheggioFactory.CreaVuoto();

        for (int colonna = 1; colonna <= 10; colonna++)
        {
            for (int piano = -12; piano <= 12; piano++)
            {
                if (piano == 0) continue;
                foreach (Cella cella in System.Enum.GetValues<Cella>())
                {
                    foreach (Box box in System.Enum.GetValues<Box>())
                    {
                        var pos = new Posizione(new Piano(piano), new Colonna(colonna), cella, box);
                        var b = parcheggio.GetBox(pos);
                        Assert.False(b.Occupato,
                            $"Il box a {pos} doveva essere libero.");
                    }
                }
            }
        }
    }

    [Fact]
    public void CreaVuoto_DeveAvere960BoxPerColonna()
    {
        // 24 piani × 2 celle × 2 box = 96 per colonna, 10 colonne = 960
        var parcheggio = ParcheggioFactory.CreaVuoto();
        int count = 0;

        for (int colonna = 1; colonna <= 10; colonna++)
        {
            for (int piano = -12; piano <= 12; piano++)
            {
                if (piano == 0) continue;
                foreach (Cella cella in System.Enum.GetValues<Cella>())
                {
                    foreach (Box box in System.Enum.GetValues<Box>())
                    {
                        var pos = new Posizione(new Piano(piano), new Colonna(colonna), cella, box);
                        parcheggio.GetBox(pos); // Non deve lanciare eccezione
                        count++;
                    }
                }
            }
        }

        Assert.Equal(960, count);
    }

    [Fact]
    public void CreaVuoto_OgniBoxDeveAverePosCorretta()
    {
        var parcheggio = ParcheggioFactory.CreaVuoto();

        var pos = new Posizione(new Piano(5), new Colonna(3), Cella.Sud, Box.Ovest);
        var b = parcheggio.GetBox(pos);

        Assert.Equal(pos, b.Posizione);
    }

    [Fact]
    public void CreaVuoto_DueChiamate_ParcheggioIndipendenti()
    {
        var p1 = ParcheggioFactory.CreaVuoto();
        var p2 = ParcheggioFactory.CreaVuoto();

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        p1.OccupaBox(pos, new Macchina("AB123CD"));

        Assert.True(p1.GetBox(pos).Occupato);
        Assert.False(p2.GetBox(pos).Occupato);
    }

    [Fact]
    public void CreaVuoto_DeveContenereBoxSotterranei()
    {
        var parcheggio = ParcheggioFactory.CreaVuoto();
        var pos = new Posizione(new Piano(-1), new Colonna(1), Cella.Nord, Box.Est);
        var box = parcheggio.GetBox(pos);

        Assert.True(box.Posizione.Piano.IsSotterraneo);
    }

    [Fact]
    public void CreaVuoto_DeveContenereBoxFuoriTerra()
    {
        var parcheggio = ParcheggioFactory.CreaVuoto();
        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        var box = parcheggio.GetBox(pos);

        Assert.False(box.Posizione.Piano.IsSotterraneo);
    }
}
