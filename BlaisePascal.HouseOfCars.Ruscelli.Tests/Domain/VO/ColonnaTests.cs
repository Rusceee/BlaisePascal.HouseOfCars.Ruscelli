using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.VO;

public class ColonnaTests
{
    // ── Creazione valida ──────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Crea_ColonnaValida_DeveAvereValoreCorretto(int valore)
    {
        var colonna = new Colonna(valore);
        Assert.Equal(valore, colonna.Valore);
    }

    [Fact]
    public void Crea_ColonnaMinima_DeveEssereUno()
    {
        var colonna = new Colonna(1);
        Assert.Equal(1, colonna.Valore);
    }

    [Fact]
    public void Crea_ColonnaMassima_DeveEssereDieci()
    {
        var colonna = new Colonna(10);
        Assert.Equal(10, colonna.Valore);
    }

    // ── Creazione NON valida ──────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Crea_ColonnaTroppoBassa_DeveLanciareEccezione(int valore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Colonna(valore));
    }

    [Theory]
    [InlineData(11)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void Crea_ColonnaTroppoAlta_DeveLanciareEccezione(int valore)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Colonna(valore));
    }

    // ── ToString ──────────────────────────────────────────────────────

    [Theory]
    [InlineData(1, "Colonna 1")]
    [InlineData(5, "Colonna 5")]
    [InlineData(10, "Colonna 10")]
    public void ToString_DeveRestituireFormatoCorretto(int valore, string expected)
    {
        var colonna = new Colonna(valore);
        Assert.Equal(expected, colonna.ToString());
    }

    // ── Uguaglianza (record struct) ───────────────────────────────────

    [Fact]
    public void Uguaglianza_StessoValore_DeveEssereUguale()
    {
        var colonna1 = new Colonna(3);
        var colonna2 = new Colonna(3);
        Assert.Equal(colonna1, colonna2);
    }

    [Fact]
    public void Uguaglianza_ValoreDiverso_DeveEssereDiverso()
    {
        var colonna1 = new Colonna(3);
        var colonna2 = new Colonna(7);
        Assert.NotEqual(colonna1, colonna2);
    }

    [Fact]
    public void GetHashCode_StessoValore_StessoHash()
    {
        var colonna1 = new Colonna(4);
        var colonna2 = new Colonna(4);
        Assert.Equal(colonna1.GetHashCode(), colonna2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_ValoreDiverso_HashDiverso()
    {
        var colonna1 = new Colonna(4);
        var colonna2 = new Colonna(8);
        // Non è strettamente garantito, ma per valori distinti piccoli è sempre vero
        Assert.NotEqual(colonna1.GetHashCode(), colonna2.GetHashCode());
    }

    // ── Tutti i valori validi ─────────────────────────────────────────

    [Fact]
    public void Crea_TutteLeColonneValide_NonDeveLanciareEccezione()
    {
        for (int i = 1; i <= 10; i++)
        {
            var colonna = new Colonna(i);
            Assert.Equal(i, colonna.Valore);
        }
    }
}
