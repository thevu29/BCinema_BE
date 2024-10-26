﻿namespace BCinema.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name) : base($"{name} was not found")
        {

        }
    }
}
