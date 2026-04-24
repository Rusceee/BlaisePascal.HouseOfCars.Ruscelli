using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.Entities;

public class ParcheggioTests
{
    private static Posizione Pos(int piano = 1, int colonna = 1,
        Cella cella = Cella.Nord, Box box = Box.Est)
        => new(new Piano(piano), new Colonna(colonna), cella, box);

    private static Parcheggio CreaParcheggioVuoto() => ParcheggioFactory.CreaVuoto();

    // ── GetBox ────────────────────────────────────────────────────────

    [Fact]
    public void GetBox_PosizioneEsistente_DeveRestituireBox()
    {
        var parcheggio = CreaParcheggioVuoto();
        var box = parcheggio.GetBox(Pos(1, 1, Cella.Nord, Box.Est));
        Assert.NotNull(box);
    }

    [Fact]
    public void GetBox_PosizioneEsistente_DeveEssereLibero()
    {
        var parcheggio = CreaParcheggioVuoto();
        var box = parcheggio.GetBox(Pos(5, 5));
        Assert.False(box.Occupato);
    }

    [Theory]
    [InlineData(1, 1, Cella.Nord, Box.Est)]
    [InlineData(12, 10, Cella.Sud, Box.Ovest)]
    [InlineData(-1, 1, Cella.Nord, Box.Est)]
    [InlineData(-12, 10, Cella.Sud, Box.Ovest)]
    public void GetBox_PosizioniAiLimiti_DeveFunzionare(
        int piano, int colonna, Cella cella, Box box)
    {
        var parcheggio = CreaParcheggioVuoto();
        var result = parcheggio.GetBox(Pos(piano, colonna, cella, box));
        Assert.NotNull(result);
    }

    // ── OccupaBox ─────────────────────────────────────────────────────

    [Fact]
    public void OccupaBox_BoxLibero_DeveOccupare()
    {
        var parcheggio = CreaParcheggioVuoto();
        var posizione = Pos(1, 1);
        var macchina = new Macchina("AB123CD");

        parcheggio.OccupaBox(posizione, macchina);

        var box = parcheggio.GetBox(posizione);
        Assert.True(box.Occupato);
        Assert.Equal("AB123CD", box.MacchinaParcheggiata!.Targa);
    }

    [Fact]
    public void OccupaBox_PiuBox_DeveFunzionare()
    {
        var parcheggio = CreaParcheggioVuoto();

        parcheggio.OccupaBox(Pos(1, 1, Cella.Nord, Box.Est), new Macchina("AA111AA"));
        parcheggio.OccupaBox(Pos(1, 1, Cella.Nord, Box.Ovest), new Macchina("BB222BB"));
        parcheggio.OccupaBox(Pos(1, 1, Cella.Sud, Box.Est), new Macchina("CC333CC"));

        Assert.True(parcheggio.GetBox(Pos(1, 1, Cella.Nord, Box.Est)).Occupato);
        Assert.True(parcheggio.GetBox(Pos(1, 1, Cella.Nord, Box.Ovest)).Occupato);
        Assert.True(parcheggio.GetBox(Pos(1, 1, Cella.Sud, Box.Est)).Occupato);
        Assert.False(parcheggio.GetBox(Pos(1, 1, Cella.Sud, Box.Ovest)).Occupato);
    }

    [Fact]
    public void OccupaBox_BoxGiaOccupato_DeveLanciareEccezione()
    {
        var parcheggio = CreaParcheggioVuoto();
        var posizione = Pos(2, 3);
        parcheggio.OccupaBox(posizione, new Macchina("AB123CD"));

        Assert.Throws<InvalidOperationException>(
            () => parcheggio.OccupaBox(posizione, new Macchina("XY456WZ")));
    }

    // ── LiberaBox ─────────────────────────────────────────────────────

    [Fact]
    public void LiberaBox_BoxOccupato_DeveRestituireMacchina()
    {
        var parcheggio = CreaParcheggioVuoto();
        var posizione = Pos(1, 1);
        parcheggio.OccupaBox(posizione, new Macchina("AB123CD"));

        var macchina = parcheggio.LiberaBox(posizione);

        Assert.Equal("AB123CD", macchina.Targa);
        Assert.False(parcheggio.GetBox(posizione).Occupato);
    }

    [Fact]
    public void LiberaBox_BoxLibero_DeveLanciareEccezione()
    {
        var parcheggio = CreaParcheggioVuoto();
        Assert.Throws<InvalidOperationException>(
            () => parcheggio.LiberaBox(Pos(1, 1)));
    }

    // ── TrovaPrimoPostoLibero ─────────────────────────────────────────

    [Fact]
    public void TrovaPrimoPostoLibero_ParcheggioVuoto_DeveRestituirePosizione()
    {
        var parcheggio = CreaParcheggioVuoto();
        var result = parcheggio.TrovaPrimoPostoLibero(new Colonna(5));
        Assert.NotNull(result);
    }

    [Fact]
    public void TrovaPrimoPostoLibero_DevePreferireColonnaTarget()
    {
        var parcheggio = CreaParcheggioVuoto();
        var target = new Colonna(5);

        var result = parcheggio.TrovaPrimoPostoLibero(target);

        Assert.NotNull(result);
        Assert.Equal(5, result!.Value.Colonna.Valore);
    }

    [Fact]
    public void TrovaPrimoPostoLibero_ColonnaTargetPiena_DeveCercareLaSuccessiva()
    {
        var parcheggio = CreaParcheggioVuoto();
        var target = new Colonna(5);

        // Occupa tutti i box della colonna 5
        for (int piano = -12; piano <= 12; piano++)
        {
            if (piano == 0) continue;
            foreach (Cella cella in System.Enum.GetValues<Cella>())
            {
                foreach (Box box in System.Enum.GetValues<Box>())
                {
                    var pos = Pos(piano, 5, cella, box);
                    parcheggio.OccupaBox(pos, new Macchina($"T{piano}{cella}{box}"));
                }
            }
        }

        var result = parcheggio.TrovaPrimoPostoLibero(target);

        Assert.NotNull(result);
        Assert.NotEqual(5, result!.Value.Colonna.Valore);
    }

    [Fact]
    public void TrovaPrimoPostoLibero_Colonna1_DeveRestituireColonna1()
    {
        var parcheggio = CreaParcheggioVuoto();
        var result = parcheggio.TrovaPrimoPostoLibero(new Colonna(1));

        Assert.NotNull(result);
        Assert.Equal(1, result!.Value.Colonna.Valore);
    }

    [Fact]
    public void TrovaPrimoPostoLibero_Colonna10_DeveRestituireColonna10()
    {
        var parcheggio = CreaParcheggioVuoto();
        var result = parcheggio.TrovaPrimoPostoLibero(new Colonna(10));

        Assert.NotNull(result);
        Assert.Equal(10, result!.Value.Colonna.Valore);
    }

    // ── OttimizzaParcheggio ───────────────────────────────────────────

    [Fact]
    public void OttimizzaParcheggio_ParcheggioVuoto_NonDeveLanciareEccezione()
    {
        var parcheggio = CreaParcheggioVuoto();
        parcheggio.OttimizzaParcheggio();
        // Nessuna eccezione
    }

    [Fact]
    public void OttimizzaParcheggio_MacchineAiPianiAlti_DeveSpostareInSotterranei()
    {
        var parcheggio = CreaParcheggioVuoto();
        var posAlta = Pos(8, 1, Cella.Nord, Box.Est);
        parcheggio.OccupaBox(posAlta, new Macchina("AB123CD"));

        parcheggio.OttimizzaParcheggio();

        Assert.False(parcheggio.GetBox(posAlta).Occupato);
    }

    [Fact]
    public void OttimizzaParcheggio_MacchineAlPiano7_NonDeveSpostare()
    {
        var parcheggio = CreaParcheggioVuoto();
        var pos = Pos(7, 1, Cella.Nord, Box.Est);
        parcheggio.OccupaBox(pos, new Macchina("AB123CD"));

        parcheggio.OttimizzaParcheggio();

        // Piano 7 < 8, non deve essere spostata
        Assert.True(parcheggio.GetBox(pos).Occupato);
    }

    [Fact]
    public void OttimizzaParcheggio_PiuMacchineAiPianiAlti_DeveSpostarleTutte()
    {
        var parcheggio = CreaParcheggioVuoto();

        var pos1 = Pos(8, 1, Cella.Nord, Box.Est);
        var pos2 = Pos(9, 2, Cella.Sud, Box.Ovest);
        var pos3 = Pos(12, 5, Cella.Nord, Box.Ovest);

        parcheggio.OccupaBox(pos1, new Macchina("AA111AA"));
        parcheggio.OccupaBox(pos2, new Macchina("BB222BB"));
        parcheggio.OccupaBox(pos3, new Macchina("CC333CC"));

        parcheggio.OttimizzaParcheggio();

        Assert.False(parcheggio.GetBox(pos1).Occupato);
        Assert.False(parcheggio.GetBox(pos2).Occupato);
        Assert.False(parcheggio.GetBox(pos3).Occupato);
    }

    [Fact]
    public void OttimizzaParcheggio_MacchinaAlPiano8_TargaConservata()
    {
        var parcheggio = CreaParcheggioVuoto();
        var posAlta = Pos(8, 1, Cella.Nord, Box.Est);
        parcheggio.OccupaBox(posAlta, new Macchina("AB123CD"));

        parcheggio.OttimizzaParcheggio();

        // Verifica che la macchina sia finita in un piano sotterraneo
        // cercando in tutti i box sotterranei
        bool trovata = false;
        for (int piano = -12; piano <= -1; piano++)
        {
            for (int col = 1; col <= 10; col++)
            {
                foreach (Cella cella in System.Enum.GetValues<Cella>())
                {
                    foreach (Box box in System.Enum.GetValues<Box>())
                    {
                        var p = Pos(piano, col, cella, box);
                        var b = parcheggio.GetBox(p);
                        if (b.Occupato && b.MacchinaParcheggiata!.Targa == "AB123CD")
                        {
                            trovata = true;
                            Assert.True(b.Posizione.Piano.IsSotterraneo);
                        }
                    }
                }
            }
        }
        Assert.True(trovata, "La macchina AB123CD non è stata trovata nei piani sotterranei.");
    }
}
