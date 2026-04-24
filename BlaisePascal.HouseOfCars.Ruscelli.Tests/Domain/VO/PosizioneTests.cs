using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.VO;

public class PosizioneTests
{
    // ── Creazione ─────────────────────────────────────────────────────

    [Fact]
    public void Crea_Posizione_DeveAvereProprietaCorrette()
    {
        var piano = new Piano(1);
        var colonna = new Colonna(5);
        var cella = Cella.Nord;
        var box = Box.Est;

        var posizione = new Posizione(piano, colonna, cella, box);

        Assert.Equal(piano, posizione.Piano);
        Assert.Equal(colonna, posizione.Colonna);
        Assert.Equal(cella, posizione.Cella);
        Assert.Equal(box, posizione.Box);
    }

    [Fact]
    public void Crea_PosizioneSotterranea_DeveAvereProprietaCorrette()
    {
        var piano = new Piano(-3);
        var colonna = new Colonna(2);
        var cella = Cella.Sud;
        var box = Box.Ovest;

        var posizione = new Posizione(piano, colonna, cella, box);

        Assert.Equal(piano, posizione.Piano);
        Assert.Equal(colonna, posizione.Colonna);
        Assert.Equal(Cella.Sud, posizione.Cella);
        Assert.Equal(Box.Ovest, posizione.Box);
    }

    // ── Tutte le combinazioni di Cella e Box ──────────────────────────

    [Theory]
    [InlineData(Cella.Nord, Box.Est)]
    [InlineData(Cella.Nord, Box.Ovest)]
    [InlineData(Cella.Sud, Box.Est)]
    [InlineData(Cella.Sud, Box.Ovest)]
    public void Crea_TutteLeCombinazioni_CellaBox_DeveEssereValido(Cella cella, Box box)
    {
        var posizione = new Posizione(new Piano(1), new Colonna(1), cella, box);

        Assert.Equal(cella, posizione.Cella);
        Assert.Equal(box, posizione.Box);
    }

    // ── ToString ──────────────────────────────────────────────────────

    [Fact]
    public void ToString_DeveRestituireFormatoCorretto()
    {
        var posizione = new Posizione(
            new Piano(3),
            new Colonna(7),
            Cella.Nord,
            Box.Est
        );

        var result = posizione.ToString();

        Assert.Contains("Colonna 7", result);
        Assert.Contains("Piano 3", result);
        Assert.Contains("Nord", result);
        Assert.Contains("Est", result);
    }

    [Fact]
    public void ToString_PianoNegativo_DeveContenereSegnoMeno()
    {
        var posizione = new Posizione(
            new Piano(-5),
            new Colonna(2),
            Cella.Sud,
            Box.Ovest
        );

        var result = posizione.ToString();
        Assert.Contains("Piano -5", result);
    }

    // ── Uguaglianza (record struct) ───────────────────────────────────

    [Fact]
    public void Uguaglianza_StessaPosizione_DeveEssereUguale()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);

        Assert.Equal(pos1, pos2);
    }

    [Fact]
    public void Uguaglianza_PianoDiverso_DeveEssereDiverso()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(2), new Colonna(5), Cella.Nord, Box.Est);

        Assert.NotEqual(pos1, pos2);
    }

    [Fact]
    public void Uguaglianza_ColonnaDiversa_DeveEssereDiverso()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(6), Cella.Nord, Box.Est);

        Assert.NotEqual(pos1, pos2);
    }

    [Fact]
    public void Uguaglianza_CellaDiversa_DeveEssereDiverso()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(5), Cella.Sud, Box.Est);

        Assert.NotEqual(pos1, pos2);
    }

    [Fact]
    public void Uguaglianza_BoxDiverso_DeveEssereDiverso()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(5), Cella.Nord, Box.Ovest);

        Assert.NotEqual(pos1, pos2);
    }

    [Fact]
    public void GetHashCode_StessaPosizione_StessoHash()
    {
        var pos1 = new Posizione(new Piano(3), new Colonna(7), Cella.Sud, Box.Ovest);
        var pos2 = new Posizione(new Piano(3), new Colonna(7), Cella.Sud, Box.Ovest);

        Assert.Equal(pos1.GetHashCode(), pos2.GetHashCode());
    }

    // ── Uso come chiave dizionario ────────────────────────────────────

    [Fact]
    public void Posizione_UsataComeDizionarioKey_DeveFunzionare()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);

        var dict = new Dictionary<Posizione, string> { { pos1, "test" } };

        Assert.True(dict.ContainsKey(pos2));
        Assert.Equal("test", dict[pos2]);
    }

    [Fact]
    public void Posizione_DiverseNelDizionario_DeveContenereTutte()
    {
        var pos1 = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        var pos2 = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Ovest);
        var pos3 = new Posizione(new Piano(1), new Colonna(1), Cella.Sud, Box.Est);
        var pos4 = new Posizione(new Piano(1), new Colonna(1), Cella.Sud, Box.Ovest);

        var dict = new Dictionary<Posizione, int>
        {
            { pos1, 1 }, { pos2, 2 }, { pos3, 3 }, { pos4, 4 }
        };

        Assert.Equal(4, dict.Count);
    }
}
