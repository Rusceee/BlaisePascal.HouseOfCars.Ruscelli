using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.VO;

public class PianoTests
{
    // ── Creazione valida ──────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void Crea_PianoPositivoValido_DeveAvereValoreCorretto(int valore)
    {
        var piano = new Piano(valore);
        Assert.Equal(valore, piano.Valore);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-6)]
    [InlineData(-12)]
    public void Crea_PianoNegativoValido_DeveAvereValoreCorretto(int valore)
    {
        var piano = new Piano(valore);
        Assert.Equal(valore, piano.Valore);
    }

    [Fact]
    public void Crea_PianoMinimo_DeveEssereMenoUno()
    {
        var piano = new Piano(-12);
        Assert.Equal(-12, piano.Valore);
    }

    [Fact]
    public void Crea_PianoMassimo_DeveEssereDodici()
    {
        var piano = new Piano(12);
        Assert.Equal(12, piano.Valore);
    }

    // ── Creazione NON valida ──────────────────────────────────────────

    [Fact]
    public void Crea_PianoZero_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Piano(0));
    }

    [Theory]
    [InlineData(13)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void Crea_PianoTroppoAlto_DeveLanciareEccezione(int valore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Piano(valore));
    }

    [Theory]
    [InlineData(-13)]
    [InlineData(-100)]
    [InlineData(int.MinValue)]
    public void Crea_PianoTroppoBasso_DeveLanciareEccezione(int valore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Piano(valore));
    }

    // ── IsSotterraneo ─────────────────────────────────────────────────

    [Theory]
    [InlineData(-1)]
    [InlineData(-6)]
    [InlineData(-12)]
    public void IsSotterraneo_PianoNegativo_DeveRestituireTrue(int valore)
    {
        var piano = new Piano(valore);
        Assert.True(piano.IsSotterraneo);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(6)]
    [InlineData(12)]
    public void IsSotterraneo_PianoPositivo_DeveRestituireFalse(int valore)
    {
        var piano = new Piano(valore);
        Assert.False(piano.IsSotterraneo);
    }

    // ── ToString ──────────────────────────────────────────────────────

    [Fact]
    public void ToString_PianoPositivo_DeveRestituireFormatoCorretto()
    {
        var piano = new Piano(5);
        Assert.Equal("Piano 5", piano.ToString());
    }

    [Fact]
    public void ToString_PianoNegativo_DeveRestituireFormatoCorretto()
    {
        var piano = new Piano(-3);
        Assert.Equal("Piano -3", piano.ToString());
    }

    // ── Uguaglianza (record struct) ───────────────────────────────────

    [Fact]
    public void Uguaglianza_StessoValore_DeveEssereUguale()
    {
        var piano1 = new Piano(5);
        var piano2 = new Piano(5);
        Assert.Equal(piano1, piano2);
    }

    [Fact]
    public void Uguaglianza_ValoreDiverso_DeveEssereDiverso()
    {
        var piano1 = new Piano(5);
        var piano2 = new Piano(6);
        Assert.NotEqual(piano1, piano2);
    }

    [Fact]
    public void Uguaglianza_PositivoVsNegativo_DeveEssereDiverso()
    {
        var piano1 = new Piano(5);
        var piano2 = new Piano(-5);
        Assert.NotEqual(piano1, piano2);
    }

    [Fact]
    public void GetHashCode_StessoValore_StessoHash()
    {
        var piano1 = new Piano(7);
        var piano2 = new Piano(7);
        Assert.Equal(piano1.GetHashCode(), piano2.GetHashCode());
    }

    // ── Tutti i valori validi ─────────────────────────────────────────

    [Fact]
    public void Crea_TuttiIPianiValidi_NonDeveLanciareEccezione()
    {
        for (int i = -12; i <= 12; i++)
        {
            if (i == 0) continue;
            var piano = new Piano(i);
            Assert.Equal(i, piano.Valore);
        }
    }
}
