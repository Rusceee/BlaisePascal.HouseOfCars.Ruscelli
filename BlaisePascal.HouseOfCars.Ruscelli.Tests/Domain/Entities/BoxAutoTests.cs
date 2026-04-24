using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.Entities;

public class BoxAutoTests
{
    private static Posizione CreaPosizione(int piano = 1, int colonna = 1,
        Cella cella = Cella.Nord, Box box = Box.Est)
        => new(new Piano(piano), new Colonna(colonna), cella, box);

    // ── Stato iniziale ────────────────────────────────────────────────

    [Fact]
    public void NuovoBox_DeveEssereLibero()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        Assert.False(boxAuto.Occupato);
    }

    [Fact]
    public void NuovoBox_MacchinaParcheggiata_DeveEssereNull()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        Assert.Null(boxAuto.MacchinaParcheggiata);
    }

    [Fact]
    public void NuovoBox_DeveAverePosizione()
    {
        var pos = CreaPosizione(3, 7, Cella.Sud, Box.Ovest);
        var boxAuto = new BoxAuto(pos);
        Assert.Equal(pos, boxAuto.Posizione);
    }

    // ── Occupa ────────────────────────────────────────────────────────

    [Fact]
    public void Occupa_BoxLibero_DeveOccupare()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        var macchina = new Macchina("AB123CD");

        boxAuto.Occupa(macchina);

        Assert.True(boxAuto.Occupato);
        Assert.Equal(macchina, boxAuto.MacchinaParcheggiata);
    }

    [Fact]
    public void Occupa_BoxLibero_DeveAvereMacchinaCorretta()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        var macchina = new Macchina("XY456WZ");

        boxAuto.Occupa(macchina);

        Assert.Equal("XY456WZ", boxAuto.MacchinaParcheggiata!.Targa);
    }

    [Fact]
    public void Occupa_BoxGiaOccupato_DeveLanciareEccezione()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        boxAuto.Occupa(new Macchina("AB123CD"));

        Assert.Throws<InvalidOperationException>(
            () => boxAuto.Occupa(new Macchina("XY456WZ")));
    }

    [Fact]
    public void Occupa_BoxGiaOccupato_MacchinaOriginaleRimane()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        var macchinaOriginale = new Macchina("AB123CD");
        boxAuto.Occupa(macchinaOriginale);

        try { boxAuto.Occupa(new Macchina("XY456WZ")); } catch { }

        Assert.Equal(macchinaOriginale, boxAuto.MacchinaParcheggiata);
    }

    // ── Libera ────────────────────────────────────────────────────────

    [Fact]
    public void Libera_BoxOccupato_DeveRestituireMacchina()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        var macchina = new Macchina("AB123CD");
        boxAuto.Occupa(macchina);

        var result = boxAuto.Libera();

        Assert.Equal(macchina, result);
    }

    [Fact]
    public void Libera_BoxOccupato_DeveRenderloLibero()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        boxAuto.Occupa(new Macchina("AB123CD"));

        boxAuto.Libera();

        Assert.False(boxAuto.Occupato);
        Assert.Null(boxAuto.MacchinaParcheggiata);
    }

    [Fact]
    public void Libera_BoxGiaLibero_DeveLanciareEccezione()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        Assert.Throws<InvalidOperationException>(() => boxAuto.Libera());
    }

    // ── Ciclo occupa/libera ───────────────────────────────────────────

    [Fact]
    public void OccupaELibera_Ripetuto_DeveFunzionare()
    {
        var boxAuto = new BoxAuto(CreaPosizione());

        for (int i = 0; i < 5; i++)
        {
            var macchina = new Macchina($"TARGA{i}");
            boxAuto.Occupa(macchina);
            Assert.True(boxAuto.Occupato);

            var liberata = boxAuto.Libera();
            Assert.False(boxAuto.Occupato);
            Assert.Equal(macchina, liberata);
        }
    }

    [Fact]
    public void Libera_RestituisceTargaCorretta()
    {
        var boxAuto = new BoxAuto(CreaPosizione());
        boxAuto.Occupa(new Macchina("primo"));
        var m1 = boxAuto.Libera();
        Assert.Equal("PRIMO", m1.Targa);

        boxAuto.Occupa(new Macchina("secondo"));
        var m2 = boxAuto.Libera();
        Assert.Equal("SECONDO", m2.Targa);
    }

    // ── Posizioni diverse ─────────────────────────────────────────────

    [Fact]
    public void BoxConPosizioniDiverse_SonoIndipendenti()
    {
        var box1 = new BoxAuto(CreaPosizione(1, 1, Cella.Nord, Box.Est));
        var box2 = new BoxAuto(CreaPosizione(1, 1, Cella.Nord, Box.Ovest));

        box1.Occupa(new Macchina("AB123CD"));

        Assert.True(box1.Occupato);
        Assert.False(box2.Occupato);
    }
}
