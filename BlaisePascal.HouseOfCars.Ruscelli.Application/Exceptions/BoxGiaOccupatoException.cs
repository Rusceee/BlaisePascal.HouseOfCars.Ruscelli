namespace BlaisePascal.HouseOfCars.Ruscelli.Application.Exceptions;

public sealed class BoxGiaOccupatoException(string posizione)
    : Exception($"Il box alla posizione {posizione} è già occupato.");