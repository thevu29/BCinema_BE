﻿namespace BCinema.Application.DTOs;

public class PaymentDetailDto
{
    public Guid Id { get; set; }
    public Guid? SeatId { get; set; }
    public Guid? FoodId { get; set; }
    public int FoodQuantity { get; set; }
    public double Price { get; set; }
}