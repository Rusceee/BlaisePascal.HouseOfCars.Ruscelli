namespace BlaisePascal.HouseOfCars.Ruscelli.Application.Exceptions;

public sealed class BoxGiaLiberoException(string posizione)
    : Exception($"Il box alla posizione {posizione} è già libero.");